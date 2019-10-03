namespace System.Net.NetworkInformation
{
	internal class Win32IPv6InterfaceProperties : IPv6InterfaceProperties
	{
		private Win32_MIB_IFROW mib;

		public override int Index => mib.Index;

		public override int Mtu => mib.Mtu;

		public Win32IPv6InterfaceProperties(Win32_MIB_IFROW mib)
		{
			this.mib = mib;
		}
	}
}
