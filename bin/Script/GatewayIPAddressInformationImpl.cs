namespace System.Net.NetworkInformation
{
	internal class GatewayIPAddressInformationImpl : GatewayIPAddressInformation
	{
		private IPAddress address;

		public override IPAddress Address => address;

		public GatewayIPAddressInformationImpl(IPAddress address)
		{
			this.address = address;
		}
	}
}
