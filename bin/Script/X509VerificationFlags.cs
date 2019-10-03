namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies conditions under which verification of certificates in the X509 chain should be conducted.</summary>
	[Flags]
	public enum X509VerificationFlags
	{
		/// <summary>No flags pertaining to verification are included.</summary>
		NoFlag = 0x0,
		/// <summary>Ignore certificates in the chain that are not valid either because they have expired or they are not yet in effect when determining certificate validity.</summary>
		IgnoreNotTimeValid = 0x1,
		/// <summary>Ignore that the certificate trust list (CTL) is not valid, for reasons such as the CTL has expired, when determining certificate verification.</summary>
		IgnoreCtlNotTimeValid = 0x2,
		/// <summary>Ignore that the CA (certificate authority) certificate and the issued certificate have validity periods that are not nested when verifying the certificate. For example, the CA cert can be valid from January 1 to December 1 and the issued certificate from January 2 to December 2, which would mean the validity periods are not nested.</summary>
		IgnoreNotTimeNested = 0x4,
		/// <summary>Ignore that the basic constraints are not valid when determining certificate verification.</summary>
		IgnoreInvalidBasicConstraints = 0x8,
		/// <summary>Ignore that the chain cannot be verified due to an unknown certificate authority (CA).</summary>
		AllowUnknownCertificateAuthority = 0x10,
		/// <summary>Ignore that the certificate was not issued for the current use when determining certificate verification.</summary>
		IgnoreWrongUsage = 0x20,
		/// <summary>Ignore that the certificate has an invalid name when determining certificate verification.</summary>
		IgnoreInvalidName = 0x40,
		/// <summary>Ignore that the certificate has invalid policy when determining certificate verification.</summary>
		IgnoreInvalidPolicy = 0x80,
		/// <summary>Ignore that the end certificate (the user certificate) revocation is unknown when determining certificate verification.</summary>
		IgnoreEndRevocationUnknown = 0x100,
		/// <summary>Ignore that the certificate trust list (CTL) signer revocation is unknown when determining certificate verification.</summary>
		IgnoreCtlSignerRevocationUnknown = 0x200,
		/// <summary>Ignore that the certificate authority revocation is unknown when determining certificate verification.</summary>
		IgnoreCertificateAuthorityRevocationUnknown = 0x400,
		/// <summary>Ignore that the root revocation is unknown when determining certificate verification.</summary>
		IgnoreRootRevocationUnknown = 0x800,
		/// <summary>All flags pertaining to verification are included.</summary>
		AllFlags = 0xFFF
	}
}
