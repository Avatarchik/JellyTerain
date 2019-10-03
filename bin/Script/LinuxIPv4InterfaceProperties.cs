using System.IO;

namespace System.Net.NetworkInformation
{
	internal sealed class LinuxIPv4InterfaceProperties : UnixIPv4InterfaceProperties
	{
		public override bool IsForwardingEnabled
		{
			get
			{
				string path = "/proc/sys/net/ipv4/conf/" + iface.Name + "/forwarding";
				if (File.Exists(path))
				{
					string a = NetworkInterface.ReadLine(path);
					return a != "0";
				}
				return false;
			}
		}

		public override int Mtu
		{
			get
			{
				string path = (iface as LinuxNetworkInterface).IfacePath + "mtu";
				int result = 0;
				if (File.Exists(path))
				{
					string s = NetworkInterface.ReadLine(path);
					try
					{
						result = int.Parse(s);
						return result;
					}
					catch
					{
						return result;
					}
				}
				return result;
			}
		}

		public LinuxIPv4InterfaceProperties(LinuxNetworkInterface iface)
			: base(iface)
		{
		}
	}
}
