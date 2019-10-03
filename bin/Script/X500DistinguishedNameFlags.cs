namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies characteristics of the X.500 distinguished name.</summary>
	[Flags]
	public enum X500DistinguishedNameFlags
	{
		/// <summary>The distinguished name has no special characteristics.</summary>
		None = 0x0,
		/// <summary>The distinguished name is reversed.</summary>
		Reversed = 0x1,
		/// <summary>The distinguished name uses semicolons.</summary>
		UseSemicolons = 0x10,
		/// <summary>The distinguished name does not use the plus sign.</summary>
		DoNotUsePlusSign = 0x20,
		/// <summary>The distinguished name does not use quotation marks.</summary>
		DoNotUseQuotes = 0x40,
		/// <summary>The distinguished name uses commas.</summary>
		UseCommas = 0x80,
		/// <summary>The distinguished name uses the new line character.</summary>
		UseNewLines = 0x100,
		/// <summary>The distinguished name uses UTF8 encoding.</summary>
		UseUTF8Encoding = 0x1000,
		/// <summary>The distinguished name uses T61 encoding.</summary>
		UseT61Encoding = 0x2000,
		/// <summary>The distinguished name uses UTF8 encoding.</summary>
		ForceUTF8Encoding = 0x4000
	}
}
