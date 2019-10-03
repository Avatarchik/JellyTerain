namespace System.Net.NetworkInformation
{
	internal abstract class UnixIPv4InterfaceProperties : IPv4InterfaceProperties
	{
		protected UnixNetworkInterface iface;

		public override int Index => UnixNetworkInterface.IfNameToIndex(iface.Name);

		public override bool IsAutomaticPrivateAddressingActive => false;

		public override bool IsAutomaticPrivateAddressingEnabled => false;

		public override bool IsDhcpEnabled => false;

		public override bool UsesWins => false;

		public UnixIPv4InterfaceProperties(UnixNetworkInterface iface)
		{
			this.iface = iface;
		}
	}
}
