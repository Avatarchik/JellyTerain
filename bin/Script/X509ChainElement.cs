namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents an element of an X.509 chain.</summary>
	public class X509ChainElement
	{
		private X509Certificate2 certificate;

		private X509ChainStatus[] status;

		private string info;

		private X509ChainStatusFlags compressed_status_flags;

		/// <summary>Gets the X.509 certificate at a particular chain element.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object.</returns>
		public X509Certificate2 Certificate => certificate;

		/// <summary>Gets the error status of the current X.509 certificate in a chain.</summary>
		/// <returns>An array of <see cref="T:System.Security.Cryptography.X509Certificates.X509ChainStatus" /> objects.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public X509ChainStatus[] ChainElementStatus => status;

		/// <summary>Gets additional error information from an unmanaged certificate chain structure.</summary>
		/// <returns>A string representing the pwszExtendedErrorInfo member of the unmanaged CERT_CHAIN_ELEMENT structure in the Crypto API.</returns>
		public string Information => info;

		internal X509ChainStatusFlags StatusFlags
		{
			get
			{
				return compressed_status_flags;
			}
			set
			{
				compressed_status_flags = value;
			}
		}

		internal X509ChainElement(X509Certificate2 certificate)
		{
			this.certificate = certificate;
			info = string.Empty;
		}

		private int Count(X509ChainStatusFlags flags)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 1;
			while (num2++ < 32)
			{
				if (((int)flags & num3) == num3)
				{
					num++;
				}
				num3 <<= 1;
			}
			return num;
		}

		private void Set(X509ChainStatus[] status, ref int position, X509ChainStatusFlags flags, X509ChainStatusFlags mask)
		{
			if ((flags & mask) != 0)
			{
				status[position].Status = mask;
				status[position].StatusInformation = X509ChainStatus.GetInformation(mask);
				position++;
			}
		}

		internal void UncompressFlags()
		{
			if (compressed_status_flags == X509ChainStatusFlags.NoError)
			{
				status = new X509ChainStatus[0];
				return;
			}
			int num = Count(compressed_status_flags);
			status = new X509ChainStatus[num];
			int position = 0;
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.UntrustedRoot);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.NotTimeValid);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.NotTimeNested);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.Revoked);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.NotSignatureValid);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.NotValidForUsage);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.RevocationStatusUnknown);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.Cyclic);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.InvalidExtension);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.InvalidPolicyConstraints);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.InvalidBasicConstraints);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.InvalidNameConstraints);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.HasNotSupportedNameConstraint);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.HasNotDefinedNameConstraint);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.HasNotPermittedNameConstraint);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.HasExcludedNameConstraint);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.PartialChain);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.CtlNotTimeValid);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.CtlNotSignatureValid);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.CtlNotValidForUsage);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.OfflineRevocation);
			Set(status, ref position, compressed_status_flags, X509ChainStatusFlags.NoIssuanceChainPolicy);
		}
	}
}
