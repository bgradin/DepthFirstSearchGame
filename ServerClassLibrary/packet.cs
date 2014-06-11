using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ServerClassLibrary
{
	//Packet Builder - essentially a wrapper that creates a byte array
	public class Packet
	{
		private static IDictionary<Type, Func<object, byte[]>> Converters = new Dictionary<Type, Func<object, byte[]>> 
		{
			{ typeof(int), s => ByteConverter.ToBytes((int)s) },
			{ typeof(uint), s => ByteConverter.ToBytes((uint)s) },
			{ typeof(long), s => ByteConverter.ToBytes((long)s) },
			{ typeof(ulong), s => ByteConverter.ToBytes((ulong)s) },
			{ typeof(short), s => ByteConverter.ToBytes((short)s) },
			{ typeof(ushort), s => ByteConverter.ToBytes((ushort)s) },
			{ typeof(string), s => ByteConverter.ToBytes((string)s) },
			{ typeof(byte[]), s => (byte[])s },
			{ typeof(byte), s => new byte[] { (byte)s } },
			{ typeof(Direction), s => ByteConverter.ToBytes((int)s) }
		};

		private List<byte> _data;
		public byte[] Data { get { if (isValid) return this._data.ToArray(); else return new byte[0]; } }

		private bool isValid;
		bool Valid { get { return this.isValid; } }

		public Packet()
		{
			this._data = new List<byte>();
			this.isValid = false;
		}

		public Packet(ServerAction sa)
		{
			this._data = new List<byte>();
			this._data.Add((byte)sa);
			this.isValid = true;
		}

		public void SetAction(ServerAction sa)
		{
			if (this._data.Count < 1)
				this._data.Add((byte)sa);
			else
				this._data.Insert(0, (byte)sa);

			if (this._data.Count > 1)
				this.isValid = false;
		}

		public bool AddData<T>(T data)
		{
			//Holy crap. Mind blown. 
			//http://stackoverflow.com/questions/18154929/avoid-excessive-type-checking-in-generic-methods
			//use dictionary of lambdas to provide the proper conversion method for the type specified.
			Func<object, byte[]> conv;
			if (!Converters.TryGetValue(typeof(T), out conv))
				return false;
			this._data.AddRange(conv(data));

			return true;
		}

		public void Clear()
		{
			this._data.Clear();
			this.isValid = false;
		}
	}
	
	//Packet Stream - essentially a wrapper for reading data from a byte array
	//  that was encoded using the ByteConverter class
	public class PacketStream
	{
		private int ptr = 0;
		private byte[] _data;

		private ServerAction sAction;
		public ServerAction Action { get { return this.sAction; } }
		public int DataLength { get { return this._data.Length; } }

		public PacketStream(byte[] data)
		{
			this._data = data;
			if (data.Length == 0)
				this.sAction = ServerAction.None;
			else
				this.sAction = (ServerAction)data[0];
			ptr += 1;
		}

		public bool GetByte(ref byte val)
		{
			bool ret = false;
			if(ptr + sizeof(byte) <= _data.Length)
				ret = true;
			else
				return ret;
			val = _data[ptr];
			ptr += sizeof(byte);
			return ret;
		}

		public bool GetInt(ref int val)
		{
			bool ret = false;
			if (ptr + sizeof(int) <= _data.Length)
				ret = true;
			else
				return ret;
			val = ByteConverter.IntFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(int)));
			ptr += sizeof(int);
			return ret;
		}
		public bool GetuInt(ref uint val)
		{
			bool ret = false;
			if (ptr + sizeof(uint) <= _data.Length)
				ret = true;
			else
				return ret;
			val = ByteConverter.uIntFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(uint)));
			ptr += sizeof(uint);
			return ret;
		}
		public bool GetLong(ref long val)
		{
			bool ret = false;
			if (ptr + sizeof(long) <= _data.Length)
				ret = true;
			else
				return ret;
			val = ByteConverter.LongFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(long)));
			ptr += sizeof(long);
			return ret;
		}
		public bool GetuLong(ref ulong val)
		{
			bool ret = false;
			if (ptr + sizeof(ulong) <= _data.Length)
				ret = true;
			else
				return ret;
			val = ByteConverter.uIntFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(ulong)));
			ptr += sizeof(ulong);
			return ret;
		}
		public bool GetShort(ref short val)
		{
			bool ret = false;
			if (ptr + sizeof(short) <= _data.Length)
				ret = true;
			else
				return ret;
			val = ByteConverter.ShortFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(short)));
			ptr += sizeof(short);
			return ret;
		}
		public bool GetuShort(ref ushort val)
		{
			bool ret = false;
			if (ptr + sizeof(ushort) <= _data.Length)
				ret = true;
			else 
				return ret;
			val = ByteConverter.uShortFromBytes(ByteConverter.SubArray(_data, ptr, sizeof(ushort)));
			ptr += sizeof(ushort);
			return ret;
		}
		public bool GetString(ref string val)
		{
			int strLen = 0;
			if (!GetInt(ref strLen) || ptr + strLen > _data.Length)
				return false;
			val = ByteConverter.StringFromBytes(ByteConverter.SubArray(this._data, ptr, strLen));
			ptr += strLen;
			return true;
		}

		public bool Seek(int offset, System.IO.SeekOrigin orig = System.IO.SeekOrigin.Current)
		{
			if (offset == 0)
			{
				//if Begin: ptr = 0, if End: ptr = datalength, if Current: ptr = ptr
				ptr = (orig == System.IO.SeekOrigin.Begin ? 0 : (orig == System.IO.SeekOrigin.End ? this._data.Length : ptr));
				return true;
			}

			switch (orig)
			{
				case System.IO.SeekOrigin.Begin:
					if (offset < 0)
						return false;
					else if (offset < this._data.Length)
						ptr = offset;
					else
						return false;
					break;
				case System.IO.SeekOrigin.Current:
					if (ptr + offset < this._data.Length && ptr + offset >= 0)
						ptr += offset;
					else
						return false;
					break;
				case System.IO.SeekOrigin.End:
					if (offset > 0 || this._data.Length + offset < 0)
						return false;
					else
						ptr = this._data.Length + offset;
					break;
			}

			return true;
		}
		public byte Peek()
		{
			if (ptr < this._data.Length)
				return _data[ptr];
			else
				return 255;
		}
	}
}
