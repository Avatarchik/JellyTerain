using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	internal class LinuxIPInterfaceProperties : UnixIPInterfaceProperties
	{
		public LinuxIPInterfaceProperties(LinuxNetworkInterface iface, List<IPAddress> addresses)
			: base(iface, addresses)
		{
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (ipv4iface_properties == null)
			{
				ipv4iface_properties = new LinuxIPv4InterfaceProperties(iface as LinuxNetworkInterface);
			}
			return ipv4iface_properties;
		}
	}
}
