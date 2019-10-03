using System;
using System.Collections;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class CRLDistributionPointsExtension : X509Extension
	{
		internal class DP
		{
			public string DistributionPoint;

			public ReasonFlags Reasons;

			public string CRLIssuer;

			public DP(string dp, ReasonFlags reasons, string issuer)
			{
				DistributionPoint = dp;
				Reasons = reasons;
				CRLIssuer = issuer;
			}

			public DP(ASN1 dp)
			{
				for (int i = 0; i < dp.Count; i++)
				{
					ASN1 aSN = dp[i];
					switch (aSN.Tag)
					{
					case 160:
						for (int j = 0; j < aSN.Count; j++)
						{
							ASN1 aSN2 = aSN[j];
							if (aSN2.Tag == 160)
							{
								DistributionPoint = new GeneralNames(aSN2).ToString();
							}
						}
						break;
					}
				}
			}
		}

		[Flags]
		public enum ReasonFlags
		{
			Unused = 0x0,
			KeyCompromise = 0x1,
			CACompromise = 0x2,
			AffiliationChanged = 0x3,
			Superseded = 0x4,
			CessationOfOperation = 0x5,
			CertificateHold = 0x6,
			PrivilegeWithdrawn = 0x7,
			AACompromise = 0x8
		}

		private ArrayList dps;

		public override string Name => "CRL Distribution Points";

		public CRLDistributionPointsExtension()
		{
			extnOid = "2.5.29.31";
			dps = new ArrayList();
		}

		public CRLDistributionPointsExtension(ASN1 asn1)
			: base(asn1)
		{
		}

		public CRLDistributionPointsExtension(X509Extension extension)
			: base(extension)
		{
		}

		protected override void Decode()
		{
			dps = new ArrayList();
			ASN1 aSN = new ASN1(extnValue.Value);
			if (aSN.Tag != 48)
			{
				throw new ArgumentException("Invalid CRLDistributionPoints extension");
			}
			for (int i = 0; i < aSN.Count; i++)
			{
				dps.Add(new DP(aSN[i]));
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			foreach (DP dp in dps)
			{
				stringBuilder.Append("[");
				stringBuilder.Append(num++);
				stringBuilder.Append("]CRL Distribution Point");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("\tDistribution Point Name:");
				stringBuilder.Append("\t\tFull Name:");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("\t\t\t");
				stringBuilder.Append(dp.DistributionPoint);
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
	}
}
