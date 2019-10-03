using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	internal class MacOsIPInterfaceProperties : UnixIPInterfaceProperties
	{
		public MacOsIPInterfaceProperties(MacOsNetworkInterface iface, List<IPAddress> addresses)
			: base(iface, addresses)
		{
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (ipv4iface_properties == null)
			{
				ipv4iface_properties = new MacOsIPv4InterfaceProperties(iface as MacOsNetworkInterface);
			}
			return ipv4iface_properties;
		}
	}
}
