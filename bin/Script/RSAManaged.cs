using Mono.Math;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Cryptography
{
	public class RSAManaged : RSA
	{
		public delegate void KeyGeneratedEventHandler(object sender, EventArgs e);

		private const int defaultKeySize = 1024;

		private bool isCRTpossible;

		private bool keyBlinding = true;

		private bool keypairGenerated;

		private bool m_disposed;

		private BigInteger d;

		private BigInteger p;

		private BigInteger q;

		private BigInteger dp;

		private BigInteger dq;

		private BigInteger qInv;

		private BigInteger n;

		private BigInteger e;

		public override int KeySize
		{
			get
			{
				if (keypairGenerated)
				{
					int num = n.BitCount();
					if ((num & 7) != 0)
					{
						num += 8 - (num & 7);
					}
					return num;
				}
				return base.KeySize;
			}
		}

		public override string KeyExchangeAlgorithm => "RSA-PKCS1-KeyEx";

		public bool PublicOnly => keypairGenerated && (d == null || n == null);

		public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

		public bool UseKeyBlinding
		{
			get
			{
				return keyBlinding;
			}
			set
			{
				keyBlinding = value;
			}
		}

		public bool IsCrtPossible => !keypairGenerated || isCRTpossible;

		public event KeyGeneratedEventHandler KeyGenerated;

		public RSAManaged()
			: this(1024)
		{
		}

		public RSAManaged(int keySize)
		{
			LegalKeySizesValue = new KeySizes[1];
			LegalKeySizesValue[0] = new KeySizes(384, 16384, 8);
			base.KeySize = keySize;
		}

		~RSAManaged()
		{
			Dispose(disposing: false);
		}

		private void GenerateKeyPair()
		{
			int num = KeySize + 1 >> 1;
			int bits = KeySize - num;
			e = 17u;
			do
			{
				p = BigInteger.GeneratePseudoPrime(num);
			}
			while (p % 17u == 1);
			while (true)
			{
				q = BigInteger.GeneratePseudoPrime(bits);
				if (q % 17u != 1 && p != q)
				{
					n = p * q;
					if (n.BitCount() == KeySize)
					{
						break;
					}
					if (p < q)
					{
						p = q;
					}
				}
			}
			BigInteger bigInteger = p - 1;
			BigInteger bi = q - 1;
			BigInteger modulus = bigInteger * bi;
			d = e.ModInverse(modulus);
			dp = d % bigInteger;
			dq = d % bi;
			qInv = q.ModInverse(p);
			keypairGenerated = true;
			isCRTpossible = true;
			if (this.KeyGenerated != null)
			{
				this.KeyGenerated(this, null);
			}
		}

		public override byte[] DecryptValue(byte[] rgb)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException("private key");
			}
			if (!keypairGenerated)
			{
				GenerateKeyPair();
			}
			BigInteger bigInteger = new BigInteger(rgb);
			BigInteger bigInteger2 = null;
			if (keyBlinding)
			{
				bigInteger2 = BigInteger.GenerateRandom(n.BitCount());
				bigInteger = bigInteger2.ModPow(e, n) * bigInteger % n;
			}
			BigInteger bigInteger5;
			if (isCRTpossible)
			{
				BigInteger bigInteger3 = bigInteger.ModPow(dp, p);
				BigInteger bigInteger4 = bigInteger.ModPow(dq, q);
				if (bigInteger4 > bigInteger3)
				{
					BigInteger bi = p - (bigInteger4 - bigInteger3) * qInv % p;
					bigInteger5 = bigInteger4 + q * bi;
				}
				else
				{
					BigInteger bi = (bigInteger3 - bigInteger4) * qInv % p;
					bigInteger5 = bigInteger4 + q * bi;
				}
			}
			else
			{
				if (PublicOnly)
				{
					throw new CryptographicException(Locale.GetText("Missing private key to decrypt value."));
				}
				bigInteger5 = bigInteger.ModPow(d, n);
			}
			if (keyBlinding)
			{
				bigInteger5 = bigInteger5 * bigInteger2.ModInverse(n) % n;
				bigInteger2.Clear();
			}
			byte[] paddedValue = GetPaddedValue(bigInteger5, KeySize >> 3);
			bigInteger.Clear();
			bigInteger5.Clear();
			return paddedValue;
		}

		public override byte[] EncryptValue(byte[] rgb)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException("public key");
			}
			if (!keypairGenerated)
			{
				GenerateKeyPair();
			}
			BigInteger bigInteger = new BigInteger(rgb);
			BigInteger bigInteger2 = bigInteger.ModPow(e, n);
			byte[] paddedValue = GetPaddedValue(bigInteger2, KeySize >> 3);
			bigInteger.Clear();
			bigInteger2.Clear();
			return paddedValue;
		}

		public override RSAParameters ExportParameters(bool includePrivateParameters)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException(Locale.GetText("Keypair was disposed"));
			}
			if (!keypairGenerated)
			{
				GenerateKeyPair();
			}
			RSAParameters result = default(RSAParameters);
			result.Exponent = e.GetBytes();
			result.Modulus = n.GetBytes();
			if (includePrivateParameters)
			{
				if (d == null)
				{
					throw new CryptographicException("Missing private key");
				}
				result.D = d.GetBytes();
				if (result.D.Length != result.Modulus.Length)
				{
					byte[] array = new byte[result.Modulus.Length];
					Buffer.BlockCopy(result.D, 0, array, array.Length - result.D.Length, result.D.Length);
					result.D = array;
				}
				if (p != null && q != null && dp != null && dq != null && qInv != null)
				{
					int length = KeySize >> 4;
					result.P = GetPaddedValue(p, length);
					result.Q = GetPaddedValue(q, length);
					result.DP = GetPaddedValue(dp, length);
					result.DQ = GetPaddedValue(dq, length);
					result.InverseQ = GetPaddedValue(qInv, length);
				}
			}
			return result;
		}

		public override void ImportParameters(RSAParameters parameters)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException(Locale.GetText("Keypair was disposed"));
			}
			if (parameters.Exponent == null)
			{
				throw new CryptographicException(Locale.GetText("Missing Exponent"));
			}
			if (parameters.Modulus == null)
			{
				throw new CryptographicException(Locale.GetText("Missing Modulus"));
			}
			e = new BigInteger(parameters.Exponent);
			n = new BigInteger(parameters.Modulus);
			if (parameters.D != null)
			{
				d = new BigInteger(parameters.D);
			}
			if (parameters.DP != null)
			{
				dp = new BigInteger(parameters.DP);
			}
			if (parameters.DQ != null)
			{
				dq = new BigInteger(parameters.DQ);
			}
			if (parameters.InverseQ != null)
			{
				qInv = new BigInteger(parameters.InverseQ);
			}
			if (parameters.P != null)
			{
				p = new BigInteger(parameters.P);
			}
			if (parameters.Q != null)
			{
				q = new BigInteger(parameters.Q);
			}
			keypairGenerated = true;
			bool flag = p != null && q != null && dp != null;
			isCRTpossible = (flag && dq != null && qInv != null);
			if (!flag)
			{
				return;
			}
			bool flag2 = n == p * q;
			if (flag2)
			{
				BigInteger bigInteger = p - 1;
				BigInteger bi = q - 1;
				BigInteger modulus = bigInteger * bi;
				BigInteger bigInteger2 = e.ModInverse(modulus);
				flag2 = (d == bigInteger2);
				if (!flag2 && isCRTpossible)
				{
					flag2 = (dp == bigInteger2 % bigInteger && dq == bigInteger2 % bi && qInv == q.ModInverse(p));
				}
			}
			if (flag2)
			{
				return;
			}
			throw new CryptographicException(Locale.GetText("Private/public key mismatch"));
		}

		protected override void Dispose(bool disposing)
		{
			if (!m_disposed)
			{
				if (d != null)
				{
					d.Clear();
					d = null;
				}
				if (p != null)
				{
					p.Clear();
					p = null;
				}
				if (q != null)
				{
					q.Clear();
					q = null;
				}
				if (dp != null)
				{
					dp.Clear();
					dp = null;
				}
				if (dq != null)
				{
					dq.Clear();
					dq = null;
				}
				if (qInv != null)
				{
					qInv.Clear();
					qInv = null;
				}
				if (disposing)
				{
					if (e != null)
					{
						e.Clear();
						e = null;
					}
					if (n != null)
					{
						n.Clear();
						n = null;
					}
				}
			}
			m_disposed = true;
		}

		public override string ToXmlString(bool includePrivateParameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			RSAParameters rSAParameters = ExportParameters(includePrivateParameters);
			try
			{
				stringBuilder.Append("<RSAKeyValue>");
				stringBuilder.Append("<Modulus>");
				stringBuilder.Append(Convert.ToBase64String(rSAParameters.Modulus));
				stringBuilder.Append("</Modulus>");
				stringBuilder.Append("<Exponent>");
				stringBuilder.Append(Convert.ToBase64String(rSAParameters.Exponent));
				stringBuilder.Append("</Exponent>");
				if (includePrivateParameters)
				{
					if (rSAParameters.P != null)
					{
						stringBuilder.Append("<P>");
						stringBuilder.Append(Convert.ToBase64String(rSAParameters.P));
						stringBuilder.Append("</P>");
					}
					if (rSAParameters.Q != null)
					{
						stringBuilder.Append("<Q>");
						stringBuilder.Append(Convert.ToBase64String(rSAParameters.Q));
						stringBuilder.Append("</Q>");
					}
					if (rSAParameters.DP != null)
					{
						stringBuilder.Append("<DP>");
						stringBuilder.Append(Convert.ToBase64String(rSAParameters.DP));
						stringBuilder.Append("</DP>");
					}
					if (rSAParameters.DQ != null)
					{
						stringBuilder.Append("<DQ>");
						stringBuilder.Append(Convert.ToBase64String(rSAParameters.DQ));
						stringBuilder.Append("</DQ>");
					}
					if (rSAParameters.InverseQ != null)
					{
						stringBuilder.Append("<InverseQ>");
						stringBuilder.Append(Convert.ToBase64String(rSAParameters.InverseQ));
						stringBuilder.Append("</InverseQ>");
					}
					stringBuilder.Append("<D>");
					stringBuilder.Append(Convert.ToBase64String(rSAParameters.D));
					stringBuilder.Append("</D>");
				}
				stringBuilder.Append("</RSAKeyValue>");
			}
			catch
			{
				if (rSAParameters.P != null)
				{
					Array.Clear(rSAParameters.P, 0, rSAParameters.P.Length);
				}
				if (rSAParameters.Q != null)
				{
					Array.Clear(rSAParameters.Q, 0, rSAParameters.Q.Length);
				}
				if (rSAParameters.DP != null)
				{
					Array.Clear(rSAParameters.DP, 0, rSAParameters.DP.Length);
				}
				if (rSAParameters.DQ != null)
				{
					Array.Clear(rSAParameters.DQ, 0, rSAParameters.DQ.Length);
				}
				if (rSAParameters.InverseQ != null)
				{
					Array.Clear(rSAParameters.InverseQ, 0, rSAParameters.InverseQ.Length);
				}
				if (rSAParameters.D != null)
				{
					Array.Clear(rSAParameters.D, 0, rSAParameters.D.Length);
				}
				throw;
				IL_0294:;
			}
			return stringBuilder.ToString();
		}

		private byte[] GetPaddedValue(BigInteger value, int length)
		{
			byte[] bytes = value.GetBytes();
			if (bytes.Length >= length)
			{
				return bytes;
			}
			byte[] array = new byte[length];
			Buffer.BlockCopy(bytes, 0, array, length - bytes.Length, bytes.Length);
			Array.Clear(bytes, 0, bytes.Length);
			return array;
		}
	}
}
