namespace System.Net.NetworkInformation
{
	internal class IPAddressInformationImpl : IPAddressInformation
	{
		private IPAddress address;

		private bool is_dns_eligible;

		private bool is_transient;

		public override IPAddress Address => address;

		public override bool IsDnsEligible => is_dns_eligible;

		[MonoTODO("Always false on Linux")]
		public override bool IsTransient => is_transient;

		public IPAddressInformationImpl(IPAddress address, bool isDnsEligible, bool isTransient)
		{
			this.address = address;
			is_dns_eligible = isDnsEligible;
			is_transient = isTransient;
		}
	}
}
