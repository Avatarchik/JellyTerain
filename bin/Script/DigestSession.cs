using System.Security.Cryptography;
using System.Text;

namespace System.Net
{
	internal class DigestSession
	{
		private static RandomNumberGenerator rng;

		private DateTime lastUse;

		private int _nc;

		private HashAlgorithm hash;

		private DigestHeaderParser parser;

		private string _cnonce;

		public string Algorithm => parser.Algorithm;

		public string Realm => parser.Realm;

		public string Nonce => parser.Nonce;

		public string Opaque => parser.Opaque;

		public string QOP => parser.QOP;

		public string CNonce
		{
			get
			{
				if (_cnonce == null)
				{
					byte[] array = new byte[15];
					rng.GetBytes(array);
					_cnonce = Convert.ToBase64String(array);
					Array.Clear(array, 0, array.Length);
				}
				return _cnonce;
			}
		}

		public DateTime LastUse => lastUse;

		public DigestSession()
		{
			_nc = 1;
			lastUse = DateTime.Now;
		}

		static DigestSession()
		{
			rng = RandomNumberGenerator.Create();
		}

		public bool Parse(string challenge)
		{
			parser = new DigestHeaderParser(challenge);
			if (!parser.Parse())
			{
				return false;
			}
			if (parser.Algorithm == null || parser.Algorithm.ToUpper().StartsWith("MD5"))
			{
				hash = HashAlgorithm.Create("MD5");
			}
			return true;
		}

		private string HashToHexString(string toBeHashed)
		{
			if (hash == null)
			{
				return null;
			}
			hash.Initialize();
			byte[] array = hash.ComputeHash(Encoding.ASCII.GetBytes(toBeHashed));
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		private string HA1(string username, string password)
		{
			string toBeHashed = $"{username}:{Realm}:{password}";
			if (Algorithm != null && Algorithm.ToLower() == "md5-sess")
			{
				toBeHashed = $"{HashToHexString(toBeHashed)}:{Nonce}:{CNonce}";
			}
			return HashToHexString(toBeHashed);
		}

		private string HA2(HttpWebRequest webRequest)
		{
			string toBeHashed = $"{webRequest.Method}:{webRequest.RequestUri.PathAndQuery}";
			if (QOP == "auth-int")
			{
			}
			return HashToHexString(toBeHashed);
		}

		private string Response(string username, string password, HttpWebRequest webRequest)
		{
			string str = $"{HA1(username, password)}:{Nonce}:";
			if (QOP != null)
			{
				str += string.Format("{0}:{1}:{2}:", _nc.ToString("X8"), CNonce, QOP);
			}
			str += HA2(webRequest);
			return HashToHexString(str);
		}

		public Authorization Authenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (parser == null)
			{
				throw new InvalidOperationException();
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			lastUse = DateTime.Now;
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.RequestUri, "digest");
			if (credential == null)
			{
				return null;
			}
			string userName = credential.UserName;
			if (userName == null || userName == string.Empty)
			{
				return null;
			}
			string password = credential.Password;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Digest username=\"{0}\", ", userName);
			stringBuilder.AppendFormat("realm=\"{0}\", ", Realm);
			stringBuilder.AppendFormat("nonce=\"{0}\", ", Nonce);
			stringBuilder.AppendFormat("uri=\"{0}\", ", httpWebRequest.Address.PathAndQuery);
			if (Algorithm != null)
			{
				stringBuilder.AppendFormat("algorithm=\"{0}\", ", Algorithm);
			}
			stringBuilder.AppendFormat("response=\"{0}\", ", Response(userName, password, httpWebRequest));
			if (QOP != null)
			{
				stringBuilder.AppendFormat("qop=\"{0}\", ", QOP);
			}
			lock (this)
			{
				if (QOP != null)
				{
					stringBuilder.AppendFormat("nc={0:X8}, ", _nc);
					_nc++;
				}
			}
			if (CNonce != null)
			{
				stringBuilder.AppendFormat("cnonce=\"{0}\", ", CNonce);
			}
			if (Opaque != null)
			{
				stringBuilder.AppendFormat("opaque=\"{0}\", ", Opaque);
			}
			stringBuilder.Length -= 2;
			return new Authorization(stringBuilder.ToString());
		}
	}
}
