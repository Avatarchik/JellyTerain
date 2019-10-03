using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class LinuxNetworkInterface : UnixNetworkInterface
	{
		private const int AF_INET = 2;

		private const int AF_INET6 = 10;

		private const int AF_PACKET = 17;

		private NetworkInterfaceType type;

		private string iface_path;

		private string iface_operstate_path;

		private string iface_flags_path;

		internal string IfacePath => iface_path;

		public override OperationalStatus OperationalStatus
		{
			get
			{
				if (!Directory.Exists(iface_path))
				{
					return OperationalStatus.Unknown;
				}
				try
				{
					string text = NetworkInterface.ReadLine(iface_operstate_path);
					string text2 = text;
					if (text2 != null)
					{
						if (_003C_003Ef__switch_0024map3 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
							dictionary.Add("unknown", 0);
							dictionary.Add("notpresent", 1);
							dictionary.Add("down", 2);
							dictionary.Add("lowerlayerdown", 3);
							dictionary.Add("testing", 4);
							dictionary.Add("dormant", 5);
							dictionary.Add("up", 6);
							_003C_003Ef__switch_0024map3 = dictionary;
						}
						if (_003C_003Ef__switch_0024map3.TryGetValue(text2, out int value))
						{
							switch (value)
							{
							case 0:
								return OperationalStatus.Unknown;
							case 1:
								return OperationalStatus.NotPresent;
							case 2:
								return OperationalStatus.Down;
							case 3:
								return OperationalStatus.LowerLayerDown;
							case 4:
								return OperationalStatus.Testing;
							case 5:
								return OperationalStatus.Dormant;
							case 6:
								return OperationalStatus.Up;
							}
						}
					}
				}
				catch
				{
				}
				return OperationalStatus.Unknown;
			}
		}

		public override bool SupportsMulticast
		{
			get
			{
				if (!Directory.Exists(iface_path))
				{
					return false;
				}
				try
				{
					string text = NetworkInterface.ReadLine(iface_flags_path);
					if (text.Length > 2 && text[0] == '0' && text[1] == 'x')
					{
						text = text.Substring(2);
					}
					ulong num = ulong.Parse(text, NumberStyles.HexNumber);
					return (num & 0x1000) == 4096;
					IL_0070:
					bool result;
					return result;
				}
				catch
				{
					return false;
					IL_007d:
					bool result;
					return result;
				}
			}
		}

		private LinuxNetworkInterface(string name)
			: base(name)
		{
			iface_path = "/sys/class/net/" + name + "/";
			iface_operstate_path = iface_path + "operstate";
			iface_flags_path = iface_path + "flags";
		}

		static LinuxNetworkInterface()
		{
			InitializeInterfaceAddresses();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitializeInterfaceAddresses();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetInterfaceAddresses(out IntPtr ifap);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeInterfaceAddresses(IntPtr ifap);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Dictionary<string, LinuxNetworkInterface> dictionary = new Dictionary<string, LinuxNetworkInterface>();
			if (GetInterfaceAddresses(out IntPtr ifap) != 0)
			{
				throw new SystemException("getifaddrs() failed");
			}
			try
			{
				IntPtr intPtr = ifap;
				int num = 0;
				while (intPtr != IntPtr.Zero)
				{
					ifaddrs ifaddrs = (ifaddrs)Marshal.PtrToStructure(intPtr, typeof(ifaddrs));
					IPAddress iPAddress = IPAddress.None;
					string text = ifaddrs.ifa_name;
					int index = -1;
					byte[] array = null;
					NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
					if (ifaddrs.ifa_addr != IntPtr.Zero)
					{
						sockaddr_in sockaddr_in = (sockaddr_in)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in));
						if (sockaddr_in.sin_family == 10)
						{
							sockaddr_in6 sockaddr_in2 = (sockaddr_in6)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in6));
							iPAddress = new IPAddress(sockaddr_in2.sin6_addr.u6_addr8, sockaddr_in2.sin6_scope_id);
						}
						else if (sockaddr_in.sin_family == 2)
						{
							iPAddress = new IPAddress(sockaddr_in.sin_addr);
						}
						else if (sockaddr_in.sin_family == 17)
						{
							sockaddr_ll sockaddr_ll = (sockaddr_ll)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_ll));
							if (sockaddr_ll.sll_halen > sockaddr_ll.sll_addr.Length)
							{
								Console.Error.WriteLine("Got a bad hardware address length for an AF_PACKET {0} {1}", sockaddr_ll.sll_halen, sockaddr_ll.sll_addr.Length);
								intPtr = ifaddrs.ifa_next;
								continue;
							}
							array = new byte[sockaddr_ll.sll_halen];
							Array.Copy(sockaddr_ll.sll_addr, 0, array, 0, array.Length);
							index = sockaddr_ll.sll_ifindex;
							int sll_hatype = sockaddr_ll.sll_hatype;
							if (Enum.IsDefined(typeof(LinuxArpHardware), sll_hatype))
							{
								switch (sll_hatype)
								{
								case 1:
								case 2:
									networkInterfaceType = NetworkInterfaceType.Ethernet;
									break;
								case 4:
									networkInterfaceType = NetworkInterfaceType.TokenRing;
									break;
								case 19:
									networkInterfaceType = NetworkInterfaceType.Atm;
									break;
								case 256:
									networkInterfaceType = NetworkInterfaceType.Slip;
									break;
								case 512:
									networkInterfaceType = NetworkInterfaceType.Ppp;
									break;
								case 772:
									networkInterfaceType = NetworkInterfaceType.Loopback;
									array = null;
									break;
								case 774:
									networkInterfaceType = NetworkInterfaceType.Fddi;
									break;
								case 768:
								case 769:
									networkInterfaceType = NetworkInterfaceType.Tunnel;
									break;
								}
							}
						}
					}
					LinuxNetworkInterface value = null;
					if (string.IsNullOrEmpty(text))
					{
						int num2 = ++num;
						text = "\0" + num2.ToString();
					}
					if (!dictionary.TryGetValue(text, out value))
					{
						value = new LinuxNetworkInterface(text);
						dictionary.Add(text, value);
					}
					if (!iPAddress.Equals(IPAddress.None))
					{
						value.AddAddress(iPAddress);
					}
					if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
					{
						if (networkInterfaceType == NetworkInterfaceType.Ethernet && Directory.Exists(value.IfacePath + "wireless"))
						{
							networkInterfaceType = NetworkInterfaceType.Wireless80211;
						}
						value.SetLinkLayerInfo(index, array, networkInterfaceType);
					}
					intPtr = ifaddrs.ifa_next;
				}
			}
			finally
			{
				FreeInterfaceAddresses(ifap);
			}
			NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
			int num3 = 0;
			foreach (LinuxNetworkInterface value2 in dictionary.Values)
			{
				NetworkInterface networkInterface = array2[num3] = value2;
				num3++;
			}
			return array2;
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			if (ipproperties == null)
			{
				ipproperties = new LinuxIPInterfaceProperties(this, addresses);
			}
			return ipproperties;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			if (ipv4stats == null)
			{
				ipv4stats = new LinuxIPv4InterfaceStatistics(this);
			}
			return ipv4stats;
		}
	}
}
