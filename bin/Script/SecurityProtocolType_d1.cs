namespace System.Net
{
	/// <summary>Specifies the security protocols that are supported by the Schannel security package.</summary>
	[Flags]
	public enum SecurityProtocolType
	{
		/// <summary>Specifies the Secure Socket Layer (SSL) 3.0 security protocol.</summary>
		Ssl3 = 0x30,
		/// <summary>Specifies the Transport Layer Security (TLS) 1.0 security protocol.</summary>
		Tls = 0xC0
	}
}
