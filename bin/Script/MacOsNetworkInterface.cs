using System.Collections.Generic;
using System.Net.NetworkInformation.MacOsStructs;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class MacOsNetworkInterface : UnixNetworkInterface
	{
		private const int AF_INET = 2;

		private const int AF_INET6 = 30;

		private const int AF_LINK = 18;

		public override OperationalStatus OperationalStatus => OperationalStatus.Unknown;

		public override bool SupportsMulticast => false;

		private MacOsNetworkInterface(string name)
			: base(name)
		{
		}

		[DllImport("libc")]
		private static extern int getifaddrs(out IntPtr ifap);

		[DllImport("libc")]
		private static extern void freeifaddrs(IntPtr ifap);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Dictionary<string, MacOsNetworkInterface> dictionary = new Dictionary<string, MacOsNetworkInterface>();
			if (getifaddrs(out IntPtr ifap) != 0)
			{
				throw new SystemException("getifaddrs() failed");
			}
			try
			{
				IntPtr intPtr = ifap;
				while (intPtr != IntPtr.Zero)
				{
					System.Net.NetworkInformation.MacOsStructs.ifaddrs ifaddrs = (System.Net.NetworkInformation.MacOsStructs.ifaddrs)Marshal.PtrToStructure(intPtr, typeof(System.Net.NetworkInformation.MacOsStructs.ifaddrs));
					IPAddress iPAddress = IPAddress.None;
					string ifa_name = ifaddrs.ifa_name;
					int index = -1;
					byte[] array = null;
					NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
					if (ifaddrs.ifa_addr != IntPtr.Zero)
					{
						System.Net.NetworkInformation.MacOsStructs.sockaddr sockaddr = (System.Net.NetworkInformation.MacOsStructs.sockaddr)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr));
						if (sockaddr.sa_family == 30)
						{
							System.Net.NetworkInformation.MacOsStructs.sockaddr_in6 sockaddr_in = (System.Net.NetworkInformation.MacOsStructs.sockaddr_in6)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_in6));
							iPAddress = new IPAddress(sockaddr_in.sin6_addr.u6_addr8, sockaddr_in.sin6_scope_id);
						}
						else if (sockaddr.sa_family == 2)
						{
							System.Net.NetworkInformation.MacOsStructs.sockaddr_in sockaddr_in2 = (System.Net.NetworkInformation.MacOsStructs.sockaddr_in)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_in));
							iPAddress = new IPAddress(sockaddr_in2.sin_addr);
						}
						else if (sockaddr.sa_family == 18)
						{
							System.Net.NetworkInformation.MacOsStructs.sockaddr_dl sockaddr_dl = (System.Net.NetworkInformation.MacOsStructs.sockaddr_dl)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_dl));
							array = new byte[sockaddr_dl.sdl_alen];
							Array.Copy(sockaddr_dl.sdl_data, sockaddr_dl.sdl_nlen, array, 0, Math.Min(array.Length, sockaddr_dl.sdl_data.Length - sockaddr_dl.sdl_nlen));
							index = sockaddr_dl.sdl_index;
							int sdl_type = sockaddr_dl.sdl_type;
							if (Enum.IsDefined(typeof(MacOsArpHardware), sdl_type))
							{
								switch (sdl_type)
								{
								case 6:
									networkInterfaceType = NetworkInterfaceType.Ethernet;
									break;
								case 37:
									networkInterfaceType = NetworkInterfaceType.Atm;
									break;
								case 28:
									networkInterfaceType = NetworkInterfaceType.Slip;
									break;
								case 23:
									networkInterfaceType = NetworkInterfaceType.Ppp;
									break;
								case 24:
									networkInterfaceType = NetworkInterfaceType.Loopback;
									array = null;
									break;
								case 15:
									networkInterfaceType = NetworkInterfaceType.Fddi;
									break;
								}
							}
						}
					}
					MacOsNetworkInterface value = null;
					if (!dictionary.TryGetValue(ifa_name, out value))
					{
						value = new MacOsNetworkInterface(ifa_name);
						dictionary.Add(ifa_name, value);
					}
					if (!iPAddress.Equals(IPAddress.None))
					{
						value.AddAddress(iPAddress);
					}
					if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
					{
						value.SetLinkLayerInfo(index, array, networkInterfaceType);
					}
					intPtr = ifaddrs.ifa_next;
				}
			}
			finally
			{
				freeifaddrs(ifap);
			}
			NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
			int num = 0;
			foreach (MacOsNetworkInterface value2 in dictionary.Values)
			{
				NetworkInterface networkInterface = array2[num] = value2;
				num++;
			}
			return array2;
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			if (ipproperties == null)
			{
				ipproperties = new MacOsIPInterfaceProperties(this, addresses);
			}
			return ipproperties;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			if (ipv4stats == null)
			{
				ipv4stats = new MacOsIPv4InterfaceStatistics(this);
			}
			return ipv4stats;
		}
	}
}
