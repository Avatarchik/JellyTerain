using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32UnicastIPAddressInformation : UnicastIPAddressInformation
	{
		private int if_index;

		private Win32_IP_ADAPTER_UNICAST_ADDRESS info;

		public override IPAddress Address => info.Address.GetIPAddress();

		public override bool IsDnsEligible => info.LengthFlags.IsDnsEligible;

		public override bool IsTransient => info.LengthFlags.IsTransient;

		public override long AddressPreferredLifetime => info.PreferredLifetime;

		public override long AddressValidLifetime => info.ValidLifetime;

		public override long DhcpLeaseLifetime => info.LeaseLifetime;

		public override DuplicateAddressDetectionState DuplicateAddressDetectionState => info.DadState;

		public override IPAddress IPv4Mask
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(if_index);
				if (adapterInfoByIndex == null)
				{
					throw new Exception("huh? " + if_index);
				}
				if (Address == null)
				{
					return null;
				}
				string b = Address.ToString();
				Win32_IP_ADDR_STRING win32_IP_ADDR_STRING = adapterInfoByIndex.IpAddressList;
				while (true)
				{
					if (win32_IP_ADDR_STRING.IpAddress == b)
					{
						return IPAddress.Parse(win32_IP_ADDR_STRING.IpMask);
					}
					if (win32_IP_ADDR_STRING.Next == IntPtr.Zero)
					{
						break;
					}
					win32_IP_ADDR_STRING = (Win32_IP_ADDR_STRING)Marshal.PtrToStructure(win32_IP_ADDR_STRING.Next, typeof(Win32_IP_ADDR_STRING));
				}
				return null;
			}
		}

		public override PrefixOrigin PrefixOrigin => info.PrefixOrigin;

		public override SuffixOrigin SuffixOrigin => info.SuffixOrigin;

		public Win32UnicastIPAddressInformation(int ifIndex, Win32_IP_ADAPTER_UNICAST_ADDRESS info)
		{
			if_index = ifIndex;
			this.info = info;
		}
	}
}
