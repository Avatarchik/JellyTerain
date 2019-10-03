namespace System.Net
{
	/// <summary>Contains a global default proxy instance for all HTTP requests.</summary>
	[Obsolete("Use WebRequest.DefaultProxy instead")]
	public class GlobalProxySelection
	{
		internal class EmptyWebProxy : IWebProxy
		{
			private ICredentials credentials;

			public ICredentials Credentials
			{
				get
				{
					return credentials;
				}
				set
				{
					credentials = value;
				}
			}

			internal EmptyWebProxy()
			{
			}

			public Uri GetProxy(Uri destination)
			{
				return destination;
			}

			public bool IsBypassed(Uri host)
			{
				return true;
			}
		}

		/// <summary>Gets or sets the global HTTP proxy.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> that every call to <see cref="M:System.Net.HttpWebRequest.GetResponse" /> uses.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation was null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have permission for the requested operation. </exception>
		public static IWebProxy Select
		{
			get
			{
				return WebRequest.DefaultWebProxy;
			}
			set
			{
				WebRequest.DefaultWebProxy = value;
			}
		}

		/// <summary>Returns an empty proxy instance.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> that contains no information.</returns>
		public static IWebProxy GetEmptyWebProxy()
		{
			return new EmptyWebProxy();
		}
	}
}
