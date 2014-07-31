using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerClassLibrary
{
	//Note: these may need to be changed to a new class deriving from IPAddress that provides support for the mapping operations
	static class IPAddressExtensions
	{
		public static IPAddress MapToIPv6(this IPAddress addr)
		{
			/*Untested!*/
			if(addr.AddressFamily != AddressFamily.InterNetwork)
				throw new ArgumentException("Must pass an IPv4 address to MapToIPv6");

			string ipv4str = addr.ToString();

			return IPAddress.Parse("::ffff:" + ipv4str);
		}

		public static bool IsIPv4MappedToIPv6(this IPAddress addr)
		{
			/*Untested!*/
			bool pass1 = addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6, pass2;

			try
			{
				pass2 = (addr.ToString().StartsWith("0000:0000:0000:0000:0000:ffff:") ||
						addr.ToString().StartsWith("0:0:0:0:0:ffff:") ||
						addr.ToString().StartsWith("::ffff:")) && 
						IPAddress.Parse(addr.ToString().Substring(addr.ToString().LastIndexOf(":") + 1)).AddressFamily == AddressFamily.InterNetwork;
			}
			catch
			{
				return false;
			}

			return pass1 && pass2;
		}
	}
}
