using System.Collections;

namespace System.Net
{
	internal class DigestClient : IAuthenticationModule
	{
		private static readonly Hashtable cache = Hashtable.Synchronized(new Hashtable());

		private static Hashtable Cache
		{
			get
			{
				lock (cache.SyncRoot)
				{
					CheckExpired(cache.Count);
				}
				return cache;
			}
		}

		public string AuthenticationType => "Digest";

		public bool CanPreAuthenticate => true;

		private static void CheckExpired(int count)
		{
			if (count >= 10)
			{
				DateTime t = DateTime.MaxValue;
				DateTime now = DateTime.Now;
				ArrayList arrayList = null;
				foreach (int key in cache.Keys)
				{
					DigestSession digestSession = (DigestSession)cache[key];
					if (digestSession.LastUse < t && (digestSession.LastUse - now).Ticks > 6000000000L)
					{
						t = digestSession.LastUse;
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(key);
					}
				}
				if (arrayList != null)
				{
					foreach (int item in arrayList)
					{
						cache.Remove(item);
					}
				}
			}
		}

		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || challenge == null)
			{
				return null;
			}
			string text = challenge.Trim();
			if (text.ToLower().IndexOf("digest") == -1)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			int num = httpWebRequest.Address.GetHashCode() ^ credentials.GetHashCode();
			DigestSession digestSession = (DigestSession)Cache[num];
			bool flag = digestSession == null;
			if (flag)
			{
				digestSession = new DigestSession();
			}
			if (!digestSession.Parse(challenge))
			{
				return null;
			}
			if (flag)
			{
				Cache.Add(num, digestSession);
			}
			return digestSession.Authenticate(webRequest, credentials);
		}

		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			if (credentials == null)
			{
				return null;
			}
			int num = httpWebRequest.Address.GetHashCode() ^ credentials.GetHashCode();
			return ((DigestSession)Cache[num])?.Authenticate(webRequest, credentials);
		}
	}
}
