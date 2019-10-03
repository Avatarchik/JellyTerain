using Mono.Security.Protocol.Tls;
using Mono.Security.X509;
using Mono.Security.X509.Extensions;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace System.Net
{
	/// <summary>Manages the collection of <see cref="T:System.Net.ServicePoint" /> objects.</summary>
	public class ServicePointManager
	{
		private class SPKey
		{
			private Uri uri;

			private bool use_connect;

			public Uri Uri => uri;

			public bool UseConnect => use_connect;

			public SPKey(Uri uri, bool use_connect)
			{
				this.uri = uri;
				this.use_connect = use_connect;
			}

			public override int GetHashCode()
			{
				return uri.GetHashCode() + (use_connect ? 1 : 0);
			}

			public override bool Equals(object obj)
			{
				SPKey sPKey = obj as SPKey;
				if (obj == null)
				{
					return false;
				}
				return uri.Equals(sPKey.uri) && sPKey.use_connect == use_connect;
			}
		}

		internal class ChainValidationHelper
		{
			private object sender;

			private string host;

			private static bool is_macosx = File.Exists("/System/Library/Frameworks/Security.framework/Security");

			private static X509KeyUsageFlags s_flags = X509KeyUsageFlags.KeyAgreement | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature;

			public string Host
			{
				get
				{
					if (host == null && sender is HttpWebRequest)
					{
						host = ((HttpWebRequest)sender).Address.Host;
					}
					return host;
				}
				set
				{
					host = value;
				}
			}

			public ChainValidationHelper(object sender)
			{
				this.sender = sender;
			}

			internal ValidationResult ValidateChain(Mono.Security.X509.X509CertificateCollection certs)
			{
				bool user_denied = false;
				if (certs == null || certs.Count == 0)
				{
					return null;
				}
				ICertificatePolicy certificatePolicy = CertificatePolicy;
				RemoteCertificateValidationCallback serverCertificateValidationCallback = ServerCertificateValidationCallback;
				System.Security.Cryptography.X509Certificates.X509Chain x509Chain = new System.Security.Cryptography.X509Certificates.X509Chain();
				x509Chain.ChainPolicy = new X509ChainPolicy();
				for (int i = 1; i < certs.Count; i++)
				{
					X509Certificate2 certificate = new X509Certificate2(certs[i].RawData);
					x509Chain.ChainPolicy.ExtraStore.Add(certificate);
				}
				X509Certificate2 x509Certificate = new X509Certificate2(certs[0].RawData);
				int num = 0;
				SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
				try
				{
					if (!x509Chain.Build(x509Certificate))
					{
						sslPolicyErrors |= GetErrorsFromChain(x509Chain);
					}
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR building certificate chain: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
				}
				if (!CheckCertificateUsage(x509Certificate))
				{
					sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
					num = -2146762490;
				}
				if (!CheckServerIdentity(certs[0], Host))
				{
					sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNameMismatch;
					num = -2146762481;
				}
				bool flag = false;
				try
				{
					Mono.Security.X509.OSX509Certificates.SecTrustResult secTrustResult = Mono.Security.X509.OSX509Certificates.TrustEvaluateSsl(certs);
					flag = (secTrustResult == Mono.Security.X509.OSX509Certificates.SecTrustResult.Proceed || secTrustResult == Mono.Security.X509.OSX509Certificates.SecTrustResult.Unspecified);
				}
				catch
				{
				}
				if (flag)
				{
					num = 0;
					sslPolicyErrors = SslPolicyErrors.None;
				}
				if (certificatePolicy != null && (!(certificatePolicy is DefaultCertificatePolicy) || serverCertificateValidationCallback == null))
				{
					ServicePoint srvPoint = null;
					HttpWebRequest httpWebRequest = sender as HttpWebRequest;
					if (httpWebRequest != null)
					{
						srvPoint = httpWebRequest.ServicePoint;
					}
					if (num == 0 && sslPolicyErrors != 0)
					{
						num = GetStatusFromChain(x509Chain);
					}
					flag = certificatePolicy.CheckValidationResult(srvPoint, x509Certificate, httpWebRequest, num);
					user_denied = (!flag && !(certificatePolicy is DefaultCertificatePolicy));
				}
				if (serverCertificateValidationCallback != null)
				{
					flag = serverCertificateValidationCallback(sender, x509Certificate, x509Chain, sslPolicyErrors);
					user_denied = !flag;
				}
				return new ValidationResult(flag, user_denied, num);
			}

			private static int GetStatusFromChain(System.Security.Cryptography.X509Certificates.X509Chain chain)
			{
				long num = 0L;
				X509ChainStatus[] chainStatus = chain.ChainStatus;
				for (int i = 0; i < chainStatus.Length; i++)
				{
					X509ChainStatus x509ChainStatus = chainStatus[i];
					System.Security.Cryptography.X509Certificates.X509ChainStatusFlags status = x509ChainStatus.Status;
					if (status != 0)
					{
						num = (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotTimeValid) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotTimeNested) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.Revoked) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotSignatureValid) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotValidForUsage) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.Cyclic) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidExtension) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidPolicyConstraints) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidBasicConstraints) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidNameConstraints) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotSupportedNameConstraint) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotDefinedNameConstraint) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotPermittedNameConstraint) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasExcludedNameConstraint) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.PartialChain) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotTimeValid) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotSignatureValid) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotValidForUsage) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.OfflineRevocation) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? (((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoIssuanceChainPolicy) == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError) ? 2148204811u : 2148204807u) : 2148081682u) : 2148204816u) : 2148098052u) : 2148204801u) : 2148204810u) : 2148204820u) : 2148204820u) : 2148204820u) : 2148204820u) : 2148204820u) : 2148098073u) : 2148204813u) : 2148204811u) : 2148204810u) : 2148081682u) : 2148204809u) : 2148204816u) : 2148098052u) : 2148204812u) : 2148204802u) : 2148204801u);
						break;
					}
				}
				return (int)num;
			}

			private static SslPolicyErrors GetErrorsFromChain(System.Security.Cryptography.X509Certificates.X509Chain chain)
			{
				SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
				X509ChainStatus[] chainStatus = chain.ChainStatus;
				for (int i = 0; i < chainStatus.Length; i++)
				{
					X509ChainStatus x509ChainStatus = chainStatus[i];
					if (x509ChainStatus.Status != 0)
					{
						sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
						break;
					}
				}
				return sslPolicyErrors;
			}

			private static bool CheckCertificateUsage(X509Certificate2 cert)
			{
				try
				{
					if (cert.Version < 3)
					{
						return true;
					}
					X509KeyUsageExtension x509KeyUsageExtension = (X509KeyUsageExtension)cert.Extensions["2.5.29.15"];
					X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = (X509EnhancedKeyUsageExtension)cert.Extensions["2.5.29.37"];
					if (x509KeyUsageExtension != null && x509EnhancedKeyUsageExtension != null)
					{
						if ((x509KeyUsageExtension.KeyUsages & s_flags) == X509KeyUsageFlags.None)
						{
							return false;
						}
						return x509EnhancedKeyUsageExtension.EnhancedKeyUsages["1.3.6.1.5.5.7.3.1"] != null || x509EnhancedKeyUsageExtension.EnhancedKeyUsages["2.16.840.1.113730.4.1"] != null;
					}
					if (x509KeyUsageExtension != null)
					{
						return (x509KeyUsageExtension.KeyUsages & s_flags) != X509KeyUsageFlags.None;
					}
					if (x509EnhancedKeyUsageExtension != null)
					{
						return x509EnhancedKeyUsageExtension.EnhancedKeyUsages["1.3.6.1.5.5.7.3.1"] != null || x509EnhancedKeyUsageExtension.EnhancedKeyUsages["2.16.840.1.113730.4.1"] != null;
					}
					System.Security.Cryptography.X509Certificates.X509Extension x509Extension = cert.Extensions["2.16.840.1.113730.1.1"];
					if (x509Extension != null)
					{
						string text = x509Extension.NetscapeCertType(multiLine: false);
						return text.IndexOf("SSL Server Authentication") != -1;
					}
					return true;
					IL_0133:
					bool result;
					return result;
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR processing certificate: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					return false;
					IL_0162:
					bool result;
					return result;
				}
			}

			private static bool CheckServerIdentity(Mono.Security.X509.X509Certificate cert, string targetHost)
			{
				try
				{
					Mono.Security.X509.X509Extension x509Extension = cert.Extensions["2.5.29.17"];
					if (x509Extension != null)
					{
						SubjectAltNameExtension subjectAltNameExtension = new SubjectAltNameExtension(x509Extension);
						string[] dNSNames = subjectAltNameExtension.DNSNames;
						foreach (string pattern in dNSNames)
						{
							if (Match(targetHost, pattern))
							{
								return true;
							}
						}
						string[] iPAddresses = subjectAltNameExtension.IPAddresses;
						foreach (string a in iPAddresses)
						{
							if (a == targetHost)
							{
								return true;
							}
						}
					}
					return CheckDomainName(cert.SubjectName, targetHost);
					IL_00a6:
					bool result;
					return result;
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR processing certificate: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					return false;
					IL_00d5:
					bool result;
					return result;
				}
			}

			private static bool CheckDomainName(string subjectName, string targetHost)
			{
				string pattern = string.Empty;
				Regex regex = new Regex("CN\\s*=\\s*([^,]*)");
				MatchCollection matchCollection = regex.Matches(subjectName);
				if (matchCollection.Count == 1 && matchCollection[0].Success)
				{
					pattern = matchCollection[0].Groups[1].Value.ToString();
				}
				return Match(targetHost, pattern);
			}

			private static bool Match(string hostname, string pattern)
			{
				int num = pattern.IndexOf('*');
				if (num == -1)
				{
					return string.Compare(hostname, pattern, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
				}
				if (num != pattern.Length - 1 && pattern[num + 1] != '.')
				{
					return false;
				}
				int num2 = pattern.IndexOf('*', num + 1);
				if (num2 != -1)
				{
					return false;
				}
				string text = pattern.Substring(num + 1);
				int num3 = hostname.Length - text.Length;
				if (num3 <= 0)
				{
					return false;
				}
				if (string.Compare(hostname, num3, text, 0, text.Length, ignoreCase: true, CultureInfo.InvariantCulture) != 0)
				{
					return false;
				}
				if (num == 0)
				{
					int num4 = hostname.IndexOf('.');
					return num4 == -1 || num4 >= hostname.Length - text.Length;
				}
				string text2 = pattern.Substring(0, num);
				return string.Compare(hostname, 0, text2, 0, text2.Length, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
			}
		}

		/// <summary>The default number of non-persistent connections (4) allowed on a <see cref="T:System.Net.ServicePoint" /> object connected to an HTTP/1.0 or later server. This field is constant but is no longer used in the .NET Framework 2.0.</summary>
		public const int DefaultNonPersistentConnectionLimit = 4;

		/// <summary>The default number of persistent connections (2) allowed on a <see cref="T:System.Net.ServicePoint" /> object connected to an HTTP/1.1 or later server. This field is constant and is used to initialize the <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> property if the value of the <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> property has not been set either directly or through configuration.</summary>
		public const int DefaultPersistentConnectionLimit = 2;

		private static HybridDictionary servicePoints;

		private static ICertificatePolicy policy;

		private static int defaultConnectionLimit;

		private static int maxServicePointIdleTime;

		private static int maxServicePoints;

		private static bool _checkCRL;

		private static SecurityProtocolType _securityProtocol;

		private static bool expectContinue;

		private static bool useNagle;

		private static RemoteCertificateValidationCallback server_cert_cb;

		/// <summary>Gets or sets policy for server certificates.</summary>
		/// <returns>An object that implements the <see cref="T:System.Net.ICertificatePolicy" /> interface.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use ServerCertificateValidationCallback instead", false)]
		public static ICertificatePolicy CertificatePolicy
		{
			get
			{
				return policy;
			}
			set
			{
				policy = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that indicates whether the certificate is checked against the certificate authority revocation list.</summary>
		/// <returns>true if the certificate revocation list is checked; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO("CRL checks not implemented")]
		public static bool CheckCertificateRevocationList
		{
			get
			{
				return _checkCRL;
			}
			set
			{
				_checkCRL = false;
			}
		}

		/// <summary>Gets or sets the maximum number of concurrent connections allowed by a <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum number of concurrent connections allowed by a <see cref="T:System.Net.ServicePoint" /> object. The default value is 2.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> is less than or equal to 0. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int DefaultConnectionLimit
		{
			get
			{
				return defaultConnectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				defaultConnectionLimit = value;
			}
		}

		/// <summary>Gets or sets a value that indicates how long a Domain Name Service (DNS) resolution is considered valid.</summary>
		/// <returns>The time-out value, in milliseconds. A value of -1 indicates an infinite time-out period. The default value is 120,000 milliseconds (two minutes).</returns>
		[MonoTODO]
		public static int DnsRefreshTimeout
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets a value that indicates whether a Domain Name Service (DNS) resolution rotates among the applicable Internet Protocol (IP) addresses.</summary>
		/// <returns>false if a DNS resolution always returns the first IP address for a particular host; otherwise true. The default is false.</returns>
		[MonoTODO]
		public static bool EnableDnsRoundRobin
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets the maximum idle time of a <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum idle time, in milliseconds, of a <see cref="T:System.Net.ServicePoint" /> object. The default value is 100,000 milliseconds (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.MaxServicePointIdleTime" /> is less than <see cref="F:System.Threading.Timeout.Infinite" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int MaxServicePointIdleTime
		{
			get
			{
				return maxServicePointIdleTime;
			}
			set
			{
				if (value < -2 || value > int.MaxValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				maxServicePointIdleTime = value;
			}
		}

		/// <summary>Gets or sets the maximum number of <see cref="T:System.Net.ServicePoint" /> objects to maintain at any time.</summary>
		/// <returns>The maximum number of <see cref="T:System.Net.ServicePoint" /> objects to maintain. The default value is 0, which means there is no limit to the number of <see cref="T:System.Net.ServicePoint" /> objects.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int MaxServicePoints
		{
			get
			{
				return maxServicePoints;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("value");
				}
				maxServicePoints = value;
				RecycleServicePoints();
			}
		}

		/// <summary>Gets or sets the security protocol used by the <see cref="T:System.Net.ServicePoint" /> objects managed by the <see cref="T:System.Net.ServicePointManager" /> object.</summary>
		/// <returns>One of the values defined in the <see cref="T:System.Net.SecurityProtocolType" /> enumeration.</returns>
		/// <exception cref="T:System.NotSupportedException">The value specified to set the property is not a valid <see cref="T:System.Net.SecurityProtocolType" /> enumeration value. </exception>
		public static SecurityProtocolType SecurityProtocol
		{
			get
			{
				return _securityProtocol;
			}
			set
			{
				_securityProtocol = value;
			}
		}

		/// <summary>Gets or sets the callback to validate a server certificate.</summary>
		/// <returns>A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> The default value is null.</returns>
		public static RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				return server_cert_cb;
			}
			set
			{
				server_cert_cb = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether 100-Continue behavior is used.</summary>
		/// <returns>true to enable 100-Continue behavior. The default value is true.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static bool Expect100Continue
		{
			get
			{
				return expectContinue;
			}
			set
			{
				expectContinue = value;
			}
		}

		/// <summary>Determines whether the Nagle algorithm is used by the service points managed by this <see cref="T:System.Net.ServicePointManager" /> object.</summary>
		/// <returns>true to use the Nagle algorithm; otherwise, false. The default value is true.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static bool UseNagleAlgorithm
		{
			get
			{
				return useNagle;
			}
			set
			{
				useNagle = value;
			}
		}

		private ServicePointManager()
		{
		}

		static ServicePointManager()
		{
			servicePoints = new HybridDictionary();
			policy = new DefaultCertificatePolicy();
			defaultConnectionLimit = 2;
			maxServicePointIdleTime = 900000;
			maxServicePoints = 0;
			_checkCRL = false;
			_securityProtocol = (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls);
			expectContinue = true;
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified <see cref="T:System.Uri" /> object.</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="address">The <see cref="T:System.Uri" /> object of the Internet resource to contact. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(Uri address)
		{
			return FindServicePoint(address, GlobalProxySelection.Select);
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified Uniform Resource Identifier (URI).</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="uriString">The URI of the Internet resource to be contacted. </param>
		/// <param name="proxy">The proxy data for this request. </param>
		/// <exception cref="T:System.UriFormatException">The URI specified in <paramref name="uriString" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(string uriString, IWebProxy proxy)
		{
			return FindServicePoint(new Uri(uriString), proxy);
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified <see cref="T:System.Uri" /> object.</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> object that contains the address of the Internet resource to contact. </param>
		/// <param name="proxy">The proxy data for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(Uri address, IWebProxy proxy)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			RecycleServicePoints();
			bool usesProxy = false;
			bool flag = false;
			if (proxy != null && !proxy.IsBypassed(address))
			{
				usesProxy = true;
				bool flag2 = address.Scheme == "https";
				address = proxy.GetProxy(address);
				if (address.Scheme != "http" && !flag2)
				{
					throw new NotSupportedException("Proxy scheme not supported.");
				}
				if (flag2 && address.Scheme == "http")
				{
					flag = true;
				}
			}
			address = new Uri(address.Scheme + "://" + address.Authority);
			ServicePoint servicePoint = null;
			lock (servicePoints)
			{
				SPKey key = new SPKey(address, flag);
				servicePoint = (servicePoints[key] as ServicePoint);
				if (servicePoint != null)
				{
					return servicePoint;
				}
				if (maxServicePoints > 0 && servicePoints.Count >= maxServicePoints)
				{
					throw new InvalidOperationException("maximum number of service points reached");
				}
				string text = address.ToString();
				int connectionLimit = defaultConnectionLimit;
				servicePoint = new ServicePoint(address, connectionLimit, maxServicePointIdleTime);
				servicePoint.Expect100Continue = expectContinue;
				servicePoint.UseNagleAlgorithm = useNagle;
				servicePoint.UsesProxy = usesProxy;
				servicePoint.UseConnect = flag;
				servicePoints.Add(key, servicePoint);
				return servicePoint;
			}
		}

		internal static void RecycleServicePoints()
		{
			ArrayList arrayList = new ArrayList();
			lock (servicePoints)
			{
				IDictionaryEnumerator enumerator = servicePoints.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ServicePoint servicePoint = (ServicePoint)enumerator.Value;
					if (servicePoint.AvailableForRecycling)
					{
						arrayList.Add(enumerator.Key);
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					servicePoints.Remove(arrayList[i]);
				}
				if (maxServicePoints != 0 && servicePoints.Count > maxServicePoints)
				{
					SortedList sortedList = new SortedList(servicePoints.Count);
					enumerator = servicePoints.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ServicePoint servicePoint2 = (ServicePoint)enumerator.Value;
						if (servicePoint2.CurrentConnections == 0)
						{
							while (sortedList.ContainsKey(servicePoint2.IdleSince))
							{
								servicePoint2.IdleSince = servicePoint2.IdleSince.AddMilliseconds(1.0);
							}
							sortedList.Add(servicePoint2.IdleSince, servicePoint2.Address);
						}
					}
					for (int j = 0; j < sortedList.Count; j++)
					{
						if (servicePoints.Count <= maxServicePoints)
						{
							break;
						}
						servicePoints.Remove(sortedList.GetByIndex(j));
					}
				}
			}
		}
	}
}
