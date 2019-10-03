using Mono.Security;
using Mono.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Security.Cryptography
{
	/// <summary>Represents Abstract Syntax Notation One (ASN.1)-encoded data.</summary>
	public class AsnEncodedData
	{
		internal Oid _oid;

		internal byte[] _raw;

		/// <summary>Gets or sets the <see cref="T:System.Security.Cryptography.Oid" /> value for an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.Oid" /> object.</returns>
		public Oid Oid
		{
			get
			{
				return _oid;
			}
			set
			{
				if (value == null)
				{
					_oid = null;
				}
				else
				{
					_oid = new Oid(value);
				}
			}
		}

		/// <summary>Gets or sets the Abstract Syntax Notation One (ASN.1)-encoded data represented in a byte array.</summary>
		/// <returns>A byte array that represents the Abstract Syntax Notation One (ASN.1)-encoded data.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value is null.</exception>
		public byte[] RawData
		{
			get
			{
				return _raw;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("RawData");
				}
				_raw = (byte[])value.Clone();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</summary>
		protected AsnEncodedData()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using a byte array.</summary>
		/// <param name="oid">A string that represents <see cref="T:System.Security.Cryptography.Oid" /> information.</param>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(string oid, byte[] rawData)
		{
			_oid = new Oid(oid);
			RawData = rawData;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using an <see cref="T:System.Security.Cryptography.Oid" /> object and a byte array.</summary>
		/// <param name="oid">An <see cref="T:System.Security.Cryptography.Oid" /> object.</param>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(Oid oid, byte[] rawData)
		{
			Oid = oid;
			RawData = rawData;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using an instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</summary>
		/// <param name="asnEncodedData">An instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData" /> is null.</exception>
		public AsnEncodedData(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (asnEncodedData._oid != null)
			{
				Oid = new Oid(asnEncodedData._oid);
			}
			RawData = asnEncodedData._raw;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using a byte array.</summary>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(byte[] rawData)
		{
			RawData = rawData;
		}

		/// <summary>Copies information from an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to base the new object on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData " />is null.</exception>
		public virtual void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (asnEncodedData._oid == null)
			{
				Oid = null;
			}
			else
			{
				Oid = new Oid(asnEncodedData._oid);
			}
			RawData = asnEncodedData._raw;
		}

		/// <summary>Returns a formatted version of the Abstract Syntax Notation One (ASN.1)-encoded data as a string.</summary>
		/// <returns>A formatted string that represents the Abstract Syntax Notation One (ASN.1)-encoded data.</returns>
		/// <param name="multiLine">true if the return string should contain carriage returns; otherwise, false.</param>
		public virtual string Format(bool multiLine)
		{
			if (_raw == null)
			{
				return string.Empty;
			}
			if (_oid == null)
			{
				return Default(multiLine);
			}
			return ToString(multiLine);
		}

		internal virtual string ToString(bool multiLine)
		{
			switch (_oid.Value)
			{
			case "2.5.29.19":
				return BasicConstraintsExtension(multiLine);
			case "2.5.29.37":
				return EnhancedKeyUsageExtension(multiLine);
			case "2.5.29.15":
				return KeyUsageExtension(multiLine);
			case "2.5.29.14":
				return SubjectKeyIdentifierExtension(multiLine);
			case "2.5.29.17":
				return SubjectAltName(multiLine);
			case "2.16.840.1.113730.1.1":
				return NetscapeCertType(multiLine);
			default:
				return Default(multiLine);
			}
		}

		internal string Default(bool multiLine)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < _raw.Length; i++)
			{
				stringBuilder.Append(_raw[i].ToString("x2"));
				if (i != _raw.Length - 1)
				{
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}

		internal string BasicConstraintsExtension(bool multiLine)
		{
			try
			{
				X509BasicConstraintsExtension x509BasicConstraintsExtension = new X509BasicConstraintsExtension(this, critical: false);
				return x509BasicConstraintsExtension.ToString(multiLine);
				IL_0015:
				string result;
				return result;
			}
			catch
			{
				return string.Empty;
				IL_0026:
				string result;
				return result;
			}
		}

		internal string EnhancedKeyUsageExtension(bool multiLine)
		{
			try
			{
				X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = new X509EnhancedKeyUsageExtension(this, critical: false);
				return x509EnhancedKeyUsageExtension.ToString(multiLine);
				IL_0015:
				string result;
				return result;
			}
			catch
			{
				return string.Empty;
				IL_0026:
				string result;
				return result;
			}
		}

		internal string KeyUsageExtension(bool multiLine)
		{
			try
			{
				X509KeyUsageExtension x509KeyUsageExtension = new X509KeyUsageExtension(this, critical: false);
				return x509KeyUsageExtension.ToString(multiLine);
				IL_0015:
				string result;
				return result;
			}
			catch
			{
				return string.Empty;
				IL_0026:
				string result;
				return result;
			}
		}

		internal string SubjectKeyIdentifierExtension(bool multiLine)
		{
			try
			{
				X509SubjectKeyIdentifierExtension x509SubjectKeyIdentifierExtension = new X509SubjectKeyIdentifierExtension(this, critical: false);
				return x509SubjectKeyIdentifierExtension.ToString(multiLine);
				IL_0015:
				string result;
				return result;
			}
			catch
			{
				return string.Empty;
				IL_0026:
				string result;
				return result;
			}
		}

		internal string SubjectAltName(bool multiLine)
		{
			if (_raw.Length < 5)
			{
				return "Information Not Available";
			}
			try
			{
				ASN1 aSN = new ASN1(_raw);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < aSN.Count; i++)
				{
					ASN1 aSN2 = aSN[i];
					string text = null;
					string text2 = null;
					switch (aSN2.Tag)
					{
					case 129:
						text = "RFC822 Name=";
						text2 = Encoding.ASCII.GetString(aSN2.Value);
						break;
					case 130:
						text = "DNS Name=";
						text2 = Encoding.ASCII.GetString(aSN2.Value);
						break;
					default:
						text = $"Unknown ({aSN2.Tag})=";
						text2 = CryptoConvert.ToHex(aSN2.Value);
						break;
					}
					stringBuilder.Append(text);
					stringBuilder.Append(text2);
					if (multiLine)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					else if (i < aSN.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				return stringBuilder.ToString();
				IL_0125:
				string result;
				return result;
			}
			catch
			{
				return string.Empty;
				IL_0137:
				string result;
				return result;
			}
		}

		internal string NetscapeCertType(bool multiLine)
		{
			if (_raw.Length < 4 || _raw[0] != 3 || _raw[1] != 2)
			{
				return "Information Not Available";
			}
			int num = _raw[3] >> (int)_raw[2] << (int)_raw[2];
			StringBuilder stringBuilder = new StringBuilder();
			if ((num & 0x80) == 128)
			{
				stringBuilder.Append("SSL Client Authentication");
			}
			if ((num & 0x40) == 64)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SSL Server Authentication");
			}
			if ((num & 0x20) == 32)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SMIME");
			}
			if ((num & 0x10) == 16)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Signature");
			}
			if ((num & 8) == 8)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Unknown cert type");
			}
			if ((num & 4) == 4)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SSL CA");
			}
			if ((num & 2) == 2)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SMIME CA");
			}
			if ((num & 1) == 1)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Signature CA");
			}
			stringBuilder.AppendFormat(" ({0})", num.ToString("x2"));
			return stringBuilder.ToString();
		}
	}
}
