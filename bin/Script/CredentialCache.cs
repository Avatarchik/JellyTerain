using System.Collections;

namespace System.Net
{
	/// <summary>Provides storage for multiple credentials.</summary>
	public class CredentialCache : IEnumerable, ICredentials, ICredentialsByHost
	{
		private class CredentialCacheKey
		{
			private Uri uriPrefix;

			private string authType;

			private string absPath;

			private int len;

			private int hash;

			public int Length => len;

			public string AbsPath => absPath;

			public Uri UriPrefix => uriPrefix;

			public string AuthType => authType;

			internal CredentialCacheKey(Uri uriPrefix, string authType)
			{
				this.uriPrefix = uriPrefix;
				this.authType = authType;
				absPath = uriPrefix.AbsolutePath;
				absPath = absPath.Substring(0, absPath.LastIndexOf('/'));
				len = uriPrefix.AbsoluteUri.Length;
				hash = uriPrefix.GetHashCode() + authType.GetHashCode();
			}

			public override int GetHashCode()
			{
				return hash;
			}

			public override bool Equals(object obj)
			{
				CredentialCacheKey credentialCacheKey = obj as CredentialCacheKey;
				return credentialCacheKey != null && hash == credentialCacheKey.hash;
			}

			public override string ToString()
			{
				return absPath + " : " + authType + " : len=" + len;
			}
		}

		private class CredentialCacheForHostKey
		{
			private string host;

			private int port;

			private string authType;

			private int hash;

			public string Host => host;

			public int Port => port;

			public string AuthType => authType;

			internal CredentialCacheForHostKey(string host, int port, string authType)
			{
				this.host = host;
				this.port = port;
				this.authType = authType;
				hash = host.GetHashCode() + port.GetHashCode() + authType.GetHashCode();
			}

			public override int GetHashCode()
			{
				return hash;
			}

			public override bool Equals(object obj)
			{
				CredentialCacheForHostKey credentialCacheForHostKey = obj as CredentialCacheForHostKey;
				return credentialCacheForHostKey != null && hash == credentialCacheForHostKey.hash;
			}

			public override string ToString()
			{
				return host + " : " + authType;
			}
		}

		private static NetworkCredential empty = new NetworkCredential(string.Empty, string.Empty, string.Empty);

		private Hashtable cache;

		private Hashtable cacheForHost;

		/// <summary>Gets the system credentials of the application.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that represents the system credentials of the application.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		[MonoTODO("Need EnvironmentPermission implementation first")]
		public static ICredentials DefaultCredentials => empty;

		/// <summary>Gets the network credentials of the current security context.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkCredential" /> that represents the network credentials of the current user or application.</returns>
		public static NetworkCredential DefaultNetworkCredentials => empty;

		/// <summary>Creates a new instance of the <see cref="T:System.Net.CredentialCache" /> class.</summary>
		public CredentialCache()
		{
			cache = new Hashtable();
			cacheForHost = new Hashtable();
		}

		/// <summary>Returns the <see cref="T:System.Net.NetworkCredential" /> instance associated with the specified Uniform Resource Identifier (URI) and authentication type.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> or, if there is no matching credential in the cache, null.</returns>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential grants access to. </param>
		/// <param name="authType">The authentication scheme used by the resource named in <paramref name="uriPrefix" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriPrefix" /> or <paramref name="authType" /> is null. </exception>
		public NetworkCredential GetCredential(Uri uriPrefix, string authType)
		{
			int num = -1;
			NetworkCredential result = null;
			if (uriPrefix == null || authType == null)
			{
				return null;
			}
			string absolutePath = uriPrefix.AbsolutePath;
			absolutePath = absolutePath.Substring(0, absolutePath.LastIndexOf('/'));
			IDictionaryEnumerator enumerator = cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialCacheKey credentialCacheKey = enumerator.Key as CredentialCacheKey;
				if (credentialCacheKey.Length > num && string.Compare(credentialCacheKey.AuthType, authType, ignoreCase: true) == 0)
				{
					Uri uriPrefix2 = credentialCacheKey.UriPrefix;
					if (!(uriPrefix2.Scheme != uriPrefix.Scheme) && uriPrefix2.Port == uriPrefix.Port && !(uriPrefix2.Host != uriPrefix.Host) && absolutePath.StartsWith(credentialCacheKey.AbsPath))
					{
						num = credentialCacheKey.Length;
						result = (NetworkCredential)enumerator.Value;
					}
				}
			}
			return result;
		}

		/// <summary>Returns an enumerator that can iterate through the <see cref="T:System.Net.CredentialCache" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Net.CredentialCache" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return cache.Values.GetEnumerator();
		}

		/// <summary>Adds a <see cref="T:System.Net.NetworkCredential" /> instance to the credential cache for use with protocols other than SMTP and associates it with a Uniform Resource Identifier (URI) prefix and authentication protocol. </summary>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential grants access to. </param>
		/// <param name="authType">The authentication scheme used by the resource named in <paramref name="uriPrefix" />. </param>
		/// <param name="cred">The <see cref="T:System.Net.NetworkCredential" /> to add to the credential cache. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriPrefix" /> is null. -or- <paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The same credentials are added more than once. </exception>
		public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			cache.Add(new CredentialCacheKey(uriPrefix, authType), cred);
		}

		/// <summary>Deletes a <see cref="T:System.Net.NetworkCredential" /> instance from the cache if it is associated with the specified Uniform Resource Identifier (URI) prefix and authentication protocol.</summary>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential is used for. </param>
		/// <param name="authType">The authentication scheme used by the host named in <paramref name="uriPrefix" />. </param>
		public void Remove(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			cache.Remove(new CredentialCacheKey(uriPrefix, authType));
		}

		/// <summary>Returns the <see cref="T:System.Net.NetworkCredential" /> instance associated with the specified host, port, and authentication protocol.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> or, if there is no matching credential in the cache, null.</returns>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" />. See Remarks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. -or- <paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="authType" /> not an accepted value. See Remarks. -or-<paramref name="host" /> is equal to the empty string ("").</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than zero.</exception>
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			NetworkCredential result = null;
			if (host == null || port < 0 || authenticationType == null)
			{
				return null;
			}
			IDictionaryEnumerator enumerator = cacheForHost.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialCacheForHostKey credentialCacheForHostKey = enumerator.Key as CredentialCacheForHostKey;
				if (string.Compare(credentialCacheForHostKey.AuthType, authenticationType, ignoreCase: true) == 0 && !(credentialCacheForHostKey.Host != host) && credentialCacheForHostKey.Port == port)
				{
					result = (NetworkCredential)enumerator.Value;
				}
			}
			return result;
		}

		/// <summary>Adds a <see cref="T:System.Net.NetworkCredential" /> instance for use with SMTP to the credential cache and associates it with a host computer, port, and authentication protocol. Credentials added using this method are valid for SMTP only. This method does not work for HTTP or FTP requests.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" /> using <paramref name="cred" />. See Remarks.</param>
		/// <param name="credential">The <see cref="T:System.Net.NetworkCredential" /> to add to the credential cache. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. -or-<paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="authType" /> not an accepted value. See Remarks. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than zero.</exception>
		public void Add(string host, int port, string authenticationType, NetworkCredential credential)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (authenticationType == null)
			{
				throw new ArgumentOutOfRangeException("authenticationType");
			}
			cacheForHost.Add(new CredentialCacheForHostKey(host, port, authenticationType), credential);
		}

		/// <summary>Deletes a <see cref="T:System.Net.NetworkCredential" /> instance from the cache if it is associated with the specified host, port, and authentication protocol.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" />. See Remarks.</param>
		public void Remove(string host, int port, string authenticationType)
		{
			if (host != null && authenticationType != null)
			{
				cacheForHost.Remove(new CredentialCacheForHostKey(host, port, authenticationType));
			}
		}
	}
}
