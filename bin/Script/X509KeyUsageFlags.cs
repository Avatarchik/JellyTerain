namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines how the certificate key can be used. If this value is not defined, the key can be used for any purpose.</summary>
	[Flags]
	public enum X509KeyUsageFlags
	{
		/// <summary>No key usage parameters.</summary>
		None = 0x0,
		/// <summary>The key can be used for encryption only.</summary>
		EncipherOnly = 0x1,
		/// <summary>The key can be used to sign a certificate revocation list (CRL).</summary>
		CrlSign = 0x2,
		/// <summary>The key can be used to sign certificates.</summary>
		KeyCertSign = 0x4,
		/// <summary>The key can be used to determine key agreement, such as a key created using the Diffie-Hellman key agreement algorithm.</summary>
		KeyAgreement = 0x8,
		/// <summary>The key can be used for data encryption.</summary>
		DataEncipherment = 0x10,
		/// <summary>The key can be used for key encryption.</summary>
		KeyEncipherment = 0x20,
		/// <summary>The key can be used for authentication.</summary>
		NonRepudiation = 0x40,
		/// <summary>The key can be used as a digital signature.</summary>
		DigitalSignature = 0x80,
		/// <summary>The key can be used for decryption only.</summary>
		DecipherOnly = 0x8000
	}
}
