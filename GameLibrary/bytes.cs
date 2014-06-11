using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace GameClassLibrary
{
	public class ByteConverter
	{
		public static byte[] ToBytes(string str)
		{
			byte[] strLen = ToBytes(str.Length);
			byte[] ret = new byte[strLen.Length + str.Length];

			int i = 0;
			for (; i < strLen.Length; ++i)
				ret[i] = (byte)strLen[i];
			for (i = ret.Length - 1; i >= strLen.Length; --i)
				ret[i] = (byte)~(str[ret.Length - 1 - i]); //bitwise inverse of byte and store in reverse order
			return ret;
		}

		public static byte[] ToBytes(Int32 num)
		{
			return ToBytes((uint)num);
		}

		public static byte[] ToBytes(UInt32 num)
		{
			byte[] ret = new byte[4];
			ret[0] = (byte)((num >> 24) & 0xFF);
			ret[1] = (byte)((num >> 16) & 0xFF);
			ret[2] = (byte)((num >> 8) & 0xFF);
			ret[3] = (byte)(num & 0xFF);
			return ret;
		}

		public static byte[] ToBytes(Int16 num)
		{
			return ToBytes((UInt16)num);
		}

		public static byte[] ToBytes(UInt16 num)
		{
			byte[] ret = new byte[2];
			ret[0] = (byte)((num >> 8) & 0xFF);
			ret[1] = (byte)(num & 0xFF);
			return ret;
		}

		public static byte[] ToBytes(Int64 num)
		{
			return ToBytes((UInt64)num);
		}

		public static byte[] ToBytes(UInt64 num)
		{
			byte[] ret = new byte[8];
			for (int i = 0; i < 8; ++i)
				ret[i] = (byte)((num >> (7 - i) * 8) & 0xFF);
			return ret;
		}

		public static string StringFromBytes(byte[] bytes)
		{
			if (bytes == null)
				return "";
			string ret = "";
			for (int i = bytes.Length - 1; i >= 0; --i)
				ret += (char)(byte)~bytes[i];
			return ret;
		}

		public static Int32 IntFromBytes(byte[] bytes)
		{
			return (int)uIntFromBytes(bytes);
		}

		public static UInt32 uIntFromBytes(byte[] bytes)
		{
			uint ret = 0, temp;
			if (bytes.Length != 4)
				return ret;
			for (int i = 0; i < 4; ++i)
			{
				temp = (uint)bytes[i];
				ret |= temp << ((3 - i) * 8);
			}
			return ret;
		}

		public static Int16 ShortFromBytes(byte[] bytes)
		{
			return (Int16)uShortFromBytes(bytes);
		}

		public static UInt16 uShortFromBytes(byte[] bytes)
		{
			UInt16 ret = 0;
			int temp;
			if (bytes.Length != 2)
				return ret;
			for (int i = 0; i < 2; ++i)
			{
				temp = (UInt16)bytes[i];
				ret |= (UInt16)(temp << ((1 - i) * 8));
			}
			return ret;
		}

		public static Int64 LongFromBytes(byte[] bytes)
		{
			return (Int64)uLongFromBytes(bytes);
		}

		public static UInt64 uLongFromBytes(byte[] bytes)
		{
			UInt64 ret = 0, temp;
			if (bytes.Length != 8)
				return ret;
			int i;
			for (i = 0; i < 8; ++i)
			{
				temp = (UInt64)bytes[i];
				ret |= (UInt64)(temp << ((7 - i) * 8));
			}

			return ret;
		}

		public static byte[] SubArray(byte[] input, int offset, int length)
		{
			if (offset < 0 || length < 1)
				return null;

			byte[] ret = new byte[length];
			Array.Copy(input, offset, ret, 0, length);
			return ret;
		}
	}

	public class CRC32
	{
		//TODO: Add default parameters to member functions for later .NET framework stuff
		public uint Magic = 0x04c11db7;

		private uint[] lookup = new uint[256];
		private bool generated = false;

		public CRC32() { GenerateTable(true); }
		public void RegenerateTable(uint num) { Magic = num; RegenerateTable(); }
		public void RegenerateTable() { GenerateTable(true); }
		private void GenerateTable(bool over)
		{
			if (!over && generated)
				return;

			for (int i = 0; i < 256; ++i)
			{
				uint temp = (uint)i;
				for (int j = 8; j > 0; --j)
					if ((temp & 1) != 0)
						temp = (temp >> 1) ^ Magic;
					else
						temp >>= 1;
				lookup[i] = temp;
			}
		}

		public uint Check(string data) { return Check(System.Text.Encoding.ASCII.GetBytes(data)); }
		public uint Check(byte[] data) { return Check(data, 0, (uint)data.Length); }
		public uint Check(byte[] data, uint offset, uint length)
		{
			uint crc = 0xFFFFFFFF;
			if (offset > data.Length || length > data.Length || offset + length > data.Length)
				return crc;
			for (uint i = offset; i < offset + length; ++i)
			{
				uint index = crc >> 24, mod = crc << 8;
				index &= 0xFF;
				index ^= data[i];

				crc = lookup[index] ^ mod;
			}
			return crc ^ 0xFFFFFFFF;
		}
	}

	public static class Encryption
	{
		private static SHA256Managed sha = new SHA256Managed();
		private static bool init = false;

		public static string SHA256(string input)
		{
			if (!init)
				sha.Initialize();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(input));
			return BitConverter.ToString(hash).Replace("-", "");
		}
	}
}