namespace System.Net.NetworkInformation
{
	internal class LinuxGatewayIPAddressInformationCollection : GatewayIPAddressInformationCollection
	{
		public static readonly LinuxGatewayIPAddressInformationCollection Empty = new LinuxGatewayIPAddressInformationCollection(isReadOnly: true);

		private bool is_readonly;

		public override bool IsReadOnly => is_readonly;

		private LinuxGatewayIPAddressInformationCollection(bool isReadOnly)
		{
			is_readonly = isReadOnly;
		}

		public LinuxGatewayIPAddressInformationCollection(IPAddressCollection col)
		{
			foreach (IPAddress item in col)
			{
				Add(new GatewayIPAddressInformationImpl(item));
			}
			is_readonly = true;
		}
	}
}
