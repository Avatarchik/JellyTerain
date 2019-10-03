namespace System.Net.NetworkInformation
{
	/// <summary>Specifies permission to access information about network interfaces and traffic statistics.</summary>
	[Flags]
	public enum NetworkInformationAccess
	{
		/// <summary>No access to network information.</summary>
		None = 0x0,
		/// <summary>Read access to network information.</summary>
		Read = 0x1,
		/// <summary>Ping access to network information.</summary>
		Ping = 0x4
	}
}
