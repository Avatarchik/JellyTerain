namespace System.Net.NetworkInformation
{
	internal class Win32IPInterfaceProperties2 : IPInterfaceProperties
	{
		private readonly Win32_IP_ADAPTER_ADDRESSES addr;

		private readonly Win32_MIB_IFROW mib4;

		private readonly Win32_MIB_IFROW mib6;

		public override IPAddressInformationCollection AnycastAddresses => IPAddressInformationImplCollection.Win32FromAnycast(addr.FirstAnycastAddress);

		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				Win32_MIB_IFROW win32_MIB_IFROW = mib4;
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
				return (adapterInfoByIndex == null) ? Win32IPAddressCollection.Empty : new Win32IPAddressCollection(adapterInfoByIndex.DhcpServer);
			}
		}

		public override IPAddressCollection DnsAddresses => Win32IPAddressCollection.FromDnsServer(addr.FirstDnsServerAddress);

		public override string DnsSuffix => addr.DnsSuffix;

		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				Win32_MIB_IFROW win32_MIB_IFROW = mib4;
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
				return (adapterInfoByIndex == null) ? Win32GatewayIPAddressInformationCollection.Empty : new Win32GatewayIPAddressInformationCollection(adapterInfoByIndex.GatewayList);
			}
		}

		public override bool IsDnsEnabled => Win32_FIXED_INFO.Instance.EnableDns != 0;

		public override bool IsDynamicDnsEnabled => addr.DdnsEnabled;

		public override MulticastIPAddressInformationCollection MulticastAddresses => MulticastIPAddressInformationImplCollection.Win32FromMulticast(addr.FirstMulticastAddress);

		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				Win32_MIB_IFROW win32_MIB_IFROW = mib4;
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
				return (adapterInfoByIndex == null) ? UnicastIPAddressInformationImplCollection.Empty : UnicastIPAddressInformationImplCollection.Win32FromUnicast((int)adapterInfoByIndex.Index, addr.FirstUnicastAddress);
			}
		}

		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				Win32_MIB_IFROW win32_MIB_IFROW = mib4;
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
				return (adapterInfoByIndex == null) ? Win32IPAddressCollection.Empty : new Win32IPAddressCollection(adapterInfoByIndex.PrimaryWinsServer, adapterInfoByIndex.SecondaryWinsServer);
			}
		}

		public Win32IPInterfaceProperties2(Win32_IP_ADAPTER_ADDRESSES addr, Win32_MIB_IFROW mib4, Win32_MIB_IFROW mib6)
		{
			this.addr = addr;
			this.mib4 = mib4;
			this.mib6 = mib6;
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			Win32_MIB_IFROW win32_MIB_IFROW = mib4;
			Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
			return (adapterInfoByIndex == null) ? null : new Win32IPv4InterfaceProperties(adapterInfoByIndex, mib4);
		}

		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			Win32_MIB_IFROW win32_MIB_IFROW = mib6;
			Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(win32_MIB_IFROW.Index);
			return (adapterInfoByIndex == null) ? null : new Win32IPv6InterfaceProperties(mib6);
		}
	}
}
