using System;

namespace Mono.Security.Protocol.Tls
{
	internal class CipherSuiteFactory
	{
		public static CipherSuiteCollection GetSupportedCiphers(SecurityProtocolType protocol)
		{
			switch (protocol)
			{
			case SecurityProtocolType.Default:
			case SecurityProtocolType.Tls:
				return GetTls1SupportedCiphers();
			case SecurityProtocolType.Ssl3:
				return GetSsl3SupportedCiphers();
			default:
				throw new NotSupportedException("Unsupported security protocol type");
			}
		}

		private static CipherSuiteCollection GetTls1SupportedCiphers()
		{
			CipherSuiteCollection cipherSuiteCollection = new CipherSuiteCollection(SecurityProtocolType.Tls);
			cipherSuiteCollection.Add(53, "TLS_RSA_WITH_AES_256_CBC_SHA", CipherAlgorithmType.Rijndael, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 32, 32, 256, 16, 16);
			cipherSuiteCollection.Add(47, "TLS_RSA_WITH_AES_128_CBC_SHA", CipherAlgorithmType.Rijndael, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 16, 16, 128, 16, 16);
			cipherSuiteCollection.Add(10, "TLS_RSA_WITH_3DES_EDE_CBC_SHA", CipherAlgorithmType.TripleDes, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 24, 24, 168, 8, 8);
			cipherSuiteCollection.Add(5, "TLS_RSA_WITH_RC4_128_SHA", CipherAlgorithmType.Rc4, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: false, 16, 16, 128, 0, 0);
			cipherSuiteCollection.Add(4, "TLS_RSA_WITH_RC4_128_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: false, 16, 16, 128, 0, 0);
			cipherSuiteCollection.Add(9, "TLS_RSA_WITH_DES_CBC_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 8, 8, 56, 8, 8);
			cipherSuiteCollection.Add(3, "TLS_RSA_EXPORT_WITH_RC4_40_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 5, 16, 40, 0, 0);
			cipherSuiteCollection.Add(6, "TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5", CipherAlgorithmType.Rc2, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 5, 16, 40, 8, 8);
			cipherSuiteCollection.Add(8, "TLS_RSA_EXPORT_WITH_DES40_CBC_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 5, 8, 40, 8, 8);
			cipherSuiteCollection.Add(96, "TLS_RSA_EXPORT_WITH_RC4_56_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 7, 16, 56, 0, 0);
			cipherSuiteCollection.Add(97, "TLS_RSA_EXPORT_WITH_RC2_CBC_56_MD5", CipherAlgorithmType.Rc2, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 7, 16, 56, 8, 8);
			cipherSuiteCollection.Add(98, "TLS_RSA_EXPORT_WITH_DES_CBC_56_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 8, 8, 64, 8, 8);
			cipherSuiteCollection.Add(100, "TLS_RSA_EXPORT_WITH_RC4_56_SHA", CipherAlgorithmType.Rc4, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 7, 16, 56, 0, 0);
			return cipherSuiteCollection;
		}

		private static CipherSuiteCollection GetSsl3SupportedCiphers()
		{
			CipherSuiteCollection cipherSuiteCollection = new CipherSuiteCollection(SecurityProtocolType.Ssl3);
			cipherSuiteCollection.Add(53, "SSL_RSA_WITH_AES_256_CBC_SHA", CipherAlgorithmType.Rijndael, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 32, 32, 256, 16, 16);
			cipherSuiteCollection.Add(10, "SSL_RSA_WITH_3DES_EDE_CBC_SHA", CipherAlgorithmType.TripleDes, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 24, 24, 168, 8, 8);
			cipherSuiteCollection.Add(5, "SSL_RSA_WITH_RC4_128_SHA", CipherAlgorithmType.Rc4, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: false, 16, 16, 128, 0, 0);
			cipherSuiteCollection.Add(4, "SSL_RSA_WITH_RC4_128_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: false, 16, 16, 128, 0, 0);
			cipherSuiteCollection.Add(9, "SSL_RSA_WITH_DES_CBC_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: false, blockMode: true, 8, 8, 56, 8, 8);
			cipherSuiteCollection.Add(3, "SSL_RSA_EXPORT_WITH_RC4_40_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 5, 16, 40, 0, 0);
			cipherSuiteCollection.Add(6, "SSL_RSA_EXPORT_WITH_RC2_CBC_40_MD5", CipherAlgorithmType.Rc2, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 5, 16, 40, 8, 8);
			cipherSuiteCollection.Add(8, "SSL_RSA_EXPORT_WITH_DES40_CBC_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 5, 8, 40, 8, 8);
			cipherSuiteCollection.Add(96, "SSL_RSA_EXPORT_WITH_RC4_56_MD5", CipherAlgorithmType.Rc4, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 7, 16, 56, 0, 0);
			cipherSuiteCollection.Add(97, "SSL_RSA_EXPORT_WITH_RC2_CBC_56_MD5", CipherAlgorithmType.Rc2, HashAlgorithmType.Md5, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 7, 16, 56, 8, 8);
			cipherSuiteCollection.Add(98, "SSL_RSA_EXPORT_WITH_DES_CBC_56_SHA", CipherAlgorithmType.Des, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: true, 8, 8, 64, 8, 8);
			cipherSuiteCollection.Add(100, "SSL_RSA_EXPORT_WITH_RC4_56_SHA", CipherAlgorithmType.Rc4, HashAlgorithmType.Sha1, ExchangeAlgorithmType.RsaKeyX, exportable: true, blockMode: false, 7, 16, 56, 0, 0);
			return cipherSuiteCollection;
		}
	}
}
