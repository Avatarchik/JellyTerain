using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal sealed class Win32IPv4InterfaceProperties : IPv4InterfaceProperties
	{
		private Win32_IP_ADAPTER_INFO ainfo;

		private Win32_IP_PER_ADAPTER_INFO painfo;

		private Win32_MIB_IFROW mib;

		public override int Index => mib.Index;

		public override bool IsAutomaticPrivateAddressingActive => painfo.AutoconfigActive != 0;

		public override bool IsAutomaticPrivateAddressingEnabled => painfo.AutoconfigEnabled != 0;

		public override bool IsDhcpEnabled => ainfo.DhcpEnabled != 0;

		public override bool IsForwardingEnabled => Win32_FIXED_INFO.Instance.EnableRouting != 0;

		public override int Mtu => mib.Mtu;

		public override bool UsesWins => ainfo.HaveWins;

		public Win32IPv4InterfaceProperties(Win32_IP_ADAPTER_INFO ainfo, Win32_MIB_IFROW mib)
		{
			this.ainfo = ainfo;
			this.mib = mib;
			int pOutBufLen = 0;
			GetPerAdapterInfo(mib.Index, null, ref pOutBufLen);
			painfo = new Win32_IP_PER_ADAPTER_INFO();
			int perAdapterInfo = GetPerAdapterInfo(mib.Index, painfo, ref pOutBufLen);
			if (perAdapterInfo != 0)
			{
				throw new NetworkInformationException(perAdapterInfo);
			}
		}

		[DllImport("iphlpapi.dll")]
		private static extern int GetPerAdapterInfo(int IfIndex, Win32_IP_PER_ADAPTER_INFO pPerAdapterInfo, ref int pOutBufLen);
	}
}
