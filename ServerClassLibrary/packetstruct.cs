using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerClassLibrary
{
	public abstract class PacketData
	{
		//all data types passed to send methods MUST derive from this class (using polymorphism we can enforce type checking a lot better)
		public PacketData() { } //so that derived stuff compiles
		//public PacketData(byte[] arr)
		//{
		//    ConstructFromByteArray(arr);
		//}
		//force derived types to convert themselves to byte arrays and to construct themselves from byte arrays
		public abstract byte[] ConvertToByteArray();
		//public abstract void ConstructFromByteArray(byte[] array);
	}

	public class KickData : PacketData
	{
		private string m_user;
		private int m_index;

		public string UserToKick { get { return m_user; } }
		public int IndexToKick { get { return m_index; } }

		public KickData(string user) { m_user = user; m_index = -1; }
		public KickData(int index) { m_index = index; m_user = null; }

		public override byte[] ConvertToByteArray()
		{
			List<byte> ret = new List<byte>();
			ret.AddRange(ByteConverter.ToBytes(m_index));
			ret.AddRange(ByteConverter.ToBytes(m_user));
			return ret.ToArray();
		}
	}

	public class LoginData : PacketData
	{
		private string m_user;
		private string m_pass;

		public string Username { get { return m_user; } }
		public string Password { get { return m_pass; } }

		public LoginData(string user, string pass)
		{
			m_user = user;
			m_pass = pass;
		}
		
		public override byte[] ConvertToByteArray()
		{
			List<byte> ret = new List<byte>();
			ret.AddRange(ByteConverter.ToBytes(m_user));
			ret.AddRange(ByteConverter.ToBytes(m_pass));
			return ret.ToArray();
		}
	}

	public class LargeData : PacketData
	{
		private int len;
		public int Length { get { return len; } }
		
		public LargeData(int length)
		{
			len = length;
		}

		//public override void ConstructFromByteArray(byte[] array)
		//{
		//    len = ByteConverter.IntFromBytes(array);
		//}

		public override byte[] ConvertToByteArray()
		{
			//generate random large amount of data when converting to a byte array
			byte[] ret = new byte[len];
			Random r = new Random();
			r.NextBytes(ret);
			return ret;
		}
	}

	public class TalkData : PacketData
	{
		private string m_msg;
		public string Message { get { return m_msg; } }

		public TalkData(string msg)
		{
			m_msg = msg;
		}

		public override byte[] ConvertToByteArray()
		{
			return ByteConverter.ToBytes(m_msg);
		}

		//public override void ConstructFromByteArray(byte[] array)
		//{
		//    m_msg = ByteConverter.StringFromBytes(array);
		//}
	}

	public class WalkData : PacketData
	{
		private int m_distance;
		private Direction m_dir;

		public int Distance { get { return this.m_distance; } }
		public Direction Dir { get { return this.m_dir; } }
		
		public WalkData(Direction dir, int distance = 1)
		{
			m_distance = distance;
			m_dir = dir;
		}

		public override byte[] ConvertToByteArray()
		{
			List<byte> ret = new List<byte>();
			ret.Add((byte)m_dir);
			ret.AddRange(ByteConverter.ToBytes(m_distance));
			return ret.ToArray();
		}

		//public override void ConstructFromByteArray(byte[] array)
		//{
		//    PacketStream p = new PacketStream(array);
		//    int tmp = -1;
		//    if (!p.GetInt(ref tmp))
		//        throw new Exception("Unable to get direction from walk packet!");
		//    m_dir = (Direction)tmp;
		//    p.GetInt(ref m_distance);
		//}
	}

	//data sent when an item is collected
	public class ItemCollectData : PacketData
	{
		private int id;
		private int quantity;

		public int ID { get { return id; } }
		public int Quantity { get { return quantity; } }

		public ItemCollectData(int id, int qty)
		{
			this.id = id;
			quantity = qty;
		}

		public override byte[] ConvertToByteArray()
		{
			List<byte> ret = new List<byte>();
			ret.AddRange(ByteConverter.ToBytes(id));
			ret.AddRange(ByteConverter.ToBytes(quantity));
			return ret.ToArray();
		}
	}

	//data sent as a content-type message when bacteria is generated
	public class ItemData : PacketData
	{
		public ItemTileSpec Type { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Quantity { get; private set; }

		public ItemData(ItemTileSpec type, int x, int y, int quantity)
		{
			Type = type;
			X = x;
			Y = y;
			Quantity = quantity;
		}

		public override byte[] ConvertToByteArray()
		{
			List<byte> ret = new List<byte>();
			ret.Add((byte)Type);
			ret.AddRange(ByteConverter.ToBytes(X));
			ret.AddRange(ByteConverter.ToBytes(Y));
			ret.AddRange(ByteConverter.ToBytes(Quantity));
			return ret.ToArray();
		}
	}
}
