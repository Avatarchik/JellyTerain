namespace System.Net.NetworkInformation
{
	internal class MulticastIPAddressInformationImpl : MulticastIPAddressInformation
	{
		private IPAddress address;

		private bool is_dns_eligible;

		private bool is_transient;

		public override IPAddress Address => address;

		public override bool IsDnsEligible => is_dns_eligible;

		public override bool IsTransient => is_transient;

		public override long AddressPreferredLifetime => 0L;

		public override long AddressValidLifetime => 0L;

		public override long DhcpLeaseLifetime => 0L;

		public override DuplicateAddressDetectionState DuplicateAddressDetectionState => DuplicateAddressDetectionState.Invalid;

		public override PrefixOrigin PrefixOrigin => PrefixOrigin.Other;

		public override SuffixOrigin SuffixOrigin => SuffixOrigin.Other;

		public MulticastIPAddressInformationImpl(IPAddress address, bool isDnsEligible, bool isTransient)
		{
			this.address = address;
			is_dns_eligible = isDnsEligible;
			is_transient = isTransient;
		}
	}
}
