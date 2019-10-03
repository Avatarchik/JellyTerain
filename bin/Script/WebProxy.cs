using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace System.Net
{
	/// <summary>Contains HTTP proxy settings for the <see cref="T:System.Net.WebRequest" /> class.</summary>
	[Serializable]
	public class WebProxy : ISerializable, IWebProxy
	{
		private Uri address;

		private bool bypassOnLocal;

		private ArrayList bypassList;

		private ICredentials credentials;

		private bool useDefaultCredentials;

		/// <summary>Gets or sets the address of the proxy server.</summary>
		/// <returns>A <see cref="T:System.Uri" /> instance that contains the address of the proxy server.</returns>
		public Uri Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		/// <summary>Gets a list of addresses that do not use the proxy server.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> that contains a list of <see cref="P:System.Net.WebProxy.BypassList" /> arrays that represents URIs that do not use the proxy server when accessed.</returns>
		public ArrayList BypassArrayList
		{
			get
			{
				if (bypassList == null)
				{
					bypassList = new ArrayList();
				}
				return bypassList;
			}
		}

		/// <summary>Gets or sets an array of addresses that do not use the proxy server.</summary>
		/// <returns>An array that contains a list of regular expressions that describe URIs that do not use the proxy server when accessed.</returns>
		public string[] BypassList
		{
			get
			{
				return (string[])BypassArrayList.ToArray(typeof(string));
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				bypassList = new ArrayList(value);
				CheckBypassList();
			}
		}

		/// <summary>Gets or sets a value that indicates whether to bypass the proxy server for local addresses.</summary>
		/// <returns>true to bypass the proxy server for local addresses; otherwise, false. The default value is false.</returns>
		public bool BypassProxyOnLocal
		{
			get
			{
				return bypassOnLocal;
			}
			set
			{
				bypassOnLocal = value;
			}
		}

		/// <summary>Gets or sets the credentials to submit to the proxy server for authentication.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> instance that contains the credentials to submit to the proxy server for authentication.</returns>
		/// <exception cref="T:System.InvalidOperationException">You attempted to set this property when the <see cref="P:System.Net.WebProxy.UseDefaultCredentials" />  property was set to true. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
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

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise, false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">You attempted to set this property when the <see cref="P:System.Net.WebProxy.Credentials" /> property contains credentials other than the default credentials. For more information, see the Remarks section.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		[MonoTODO("Does not affect Credentials, since CredentialCache.DefaultCredentials is not implemented.")]
		public bool UseDefaultCredentials
		{
			get
			{
				return useDefaultCredentials;
			}
			set
			{
				useDefaultCredentials = value;
			}
		}

		/// <summary>Initializes an empty instance of the <see cref="T:System.Net.WebProxy" /> class.</summary>
		public WebProxy()
			: this((Uri)null, bypassOnLocal: false, (string[])null, (ICredentials)null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified URI.</summary>
		/// <param name="Address">The URI of the proxy server. </param>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="Address" /> is an invalid URI. </exception>
		public WebProxy(string address)
			: this(ToUri(address), bypassOnLocal: false, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class from the specified <see cref="T:System.Uri" /> instance.</summary>
		/// <param name="Address">A <see cref="T:System.Uri" /> instance that contains the address of the proxy server. </param>
		public WebProxy(Uri address)
			: this(address, bypassOnLocal: false, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified URI and bypass setting.</summary>
		/// <param name="Address">The URI of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="Address" /> is an invalid URI. </exception>
		public WebProxy(string address, bool bypassOnLocal)
			: this(ToUri(address), bypassOnLocal, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified host and port number.</summary>
		/// <param name="Host">The name of the proxy host. </param>
		/// <param name="Port">The port number on <paramref name="Host" /> to use. </param>
		/// <exception cref="T:System.UriFormatException">The URI formed by combining <paramref name="Host" /> and <paramref name="Port" /> is not a valid URI. </exception>
		public WebProxy(string host, int port)
			: this(new Uri("http://" + host + ":" + port))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the <see cref="T:System.Uri" /> instance and bypass setting.</summary>
		/// <param name="Address">A <see cref="T:System.Uri" /> instance that contains the address of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		public WebProxy(Uri address, bool bypassOnLocal)
			: this(address, bypassOnLocal, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified URI, bypass setting, and list of URIs to bypass.</summary>
		/// <param name="Address">The URI of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		/// <param name="BypassList">An array of regular expression strings that contain the URIs of the servers to bypass. </param>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="Address" /> is an invalid URI. </exception>
		public WebProxy(string address, bool bypassOnLocal, string[] bypassList)
			: this(ToUri(address), bypassOnLocal, bypassList, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified <see cref="T:System.Uri" /> instance, bypass setting, and list of URIs to bypass.</summary>
		/// <param name="Address">A <see cref="T:System.Uri" /> instance that contains the address of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		/// <param name="BypassList">An array of regular expression strings that contains the URIs of the servers to bypass. </param>
		public WebProxy(Uri address, bool bypassOnLocal, string[] bypassList)
			: this(address, bypassOnLocal, bypassList, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified URI, bypass setting, list of URIs to bypass, and credentials.</summary>
		/// <param name="Address">The URI of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		/// <param name="BypassList">An array of regular expression strings that contains the URIs of the servers to bypass. </param>
		/// <param name="Credentials">An <see cref="T:System.Net.ICredentials" /> instance to submit to the proxy server for authentication. </param>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="Address" /> is an invalid URI. </exception>
		public WebProxy(string address, bool bypassOnLocal, string[] bypassList, ICredentials credentials)
			: this(ToUri(address), bypassOnLocal, bypassList, credentials)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebProxy" /> class with the specified <see cref="T:System.Uri" /> instance, bypass setting, list of URIs to bypass, and credentials.</summary>
		/// <param name="Address">A <see cref="T:System.Uri" /> instance that contains the address of the proxy server. </param>
		/// <param name="BypassOnLocal">true to bypass the proxy for local addresses; otherwise, false. </param>
		/// <param name="BypassList">An array of regular expression strings that contains the URIs of the servers to bypass. </param>
		/// <param name="Credentials">An <see cref="T:System.Net.ICredentials" /> instance to submit to the proxy server for authentication. </param>
		public WebProxy(Uri address, bool bypassOnLocal, string[] bypassList, ICredentials credentials)
		{
			this.address = address;
			this.bypassOnLocal = bypassOnLocal;
			if (bypassList != null)
			{
				this.bypassList = new ArrayList(bypassList);
			}
			this.credentials = credentials;
			CheckBypassList();
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Net.WebProxy" /> class using previously serialized content.</summary>
		/// <param name="serializationInfo">The serialization data. </param>
		/// <param name="streamingContext">The context for the serialized data. </param>
		protected WebProxy(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			address = (Uri)serializationInfo.GetValue("_ProxyAddress", typeof(Uri));
			bypassOnLocal = serializationInfo.GetBoolean("_BypassOnLocal");
			bypassList = (ArrayList)serializationInfo.GetValue("_BypassList", typeof(ArrayList));
			useDefaultCredentials = serializationInfo.GetBoolean("_UseDefaultCredentials");
			credentials = null;
			CheckBypassList();
		}

		/// <summary>Creates the serialization data and context that are used by the system to serialize a <see cref="T:System.Net.WebProxy" /> object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that indicates the destination for this serialization. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Reads the Internet Explorer nondynamic proxy settings.</summary>
		/// <returns>A <see cref="T:System.Net.WebProxy" /> instance that contains the nondynamic proxy settings from Internet Explorer 5.5 and later.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("This method has been deprecated", false)]
		[MonoTODO("Can we get this info under windows from the system?")]
		public static WebProxy GetDefaultProxy()
		{
			IWebProxy select = GlobalProxySelection.Select;
			if (select is WebProxy)
			{
				return (WebProxy)select;
			}
			return new WebProxy();
		}

		/// <summary>Returns the proxied URI for a request.</summary>
		/// <returns>The <see cref="T:System.Uri" /> instance of the Internet resource, if the resource is on the bypass list; otherwise, the <see cref="T:System.Uri" /> instance of the proxy.</returns>
		/// <param name="destination">The <see cref="T:System.Uri" /> instance of the requested Internet resource. </param>
		public Uri GetProxy(Uri destination)
		{
			if (IsBypassed(destination))
			{
				return destination;
			}
			return address;
		}

		/// <summary>Indicates whether to use the proxy server for the specified host.</summary>
		/// <returns>true if the proxy server should not be used for <paramref name="host" />; otherwise, false.</returns>
		/// <param name="host">The <see cref="T:System.Uri" /> instance of the host to check for proxy use. </param>
		public bool IsBypassed(Uri host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (host.IsLoopback && bypassOnLocal)
			{
				return true;
			}
			if (address == null)
			{
				return true;
			}
			string host2 = host.Host;
			if (bypassOnLocal && host2.IndexOf('.') == -1)
			{
				return true;
			}
			if (!bypassOnLocal)
			{
				if (string.Compare(host2, "localhost", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
				if (string.Compare(host2, "loopback", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
				IPAddress addr = null;
				if (IPAddress.TryParse(host2, out addr) && IPAddress.IsLoopback(addr))
				{
					return true;
				}
			}
			if (bypassList == null || bypassList.Count == 0)
			{
				return false;
			}
			try
			{
				string input = host.Scheme + "://" + host.Authority;
				int i;
				for (i = 0; i < bypassList.Count; i++)
				{
					Regex regex = new Regex((string)bypassList[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
					if (regex.IsMatch(input))
					{
						break;
					}
				}
				if (i == bypassList.Count)
				{
					return false;
				}
				for (; i < bypassList.Count; i++)
				{
					new Regex((string)bypassList[i]);
				}
				return true;
				IL_0189:
				bool result;
				return result;
			}
			catch (ArgumentException)
			{
				return false;
				IL_0197:
				bool result;
				return result;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data that is needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("_BypassOnLocal", bypassOnLocal);
			serializationInfo.AddValue("_ProxyAddress", address);
			serializationInfo.AddValue("_BypassList", bypassList);
			serializationInfo.AddValue("_UseDefaultCredentials", UseDefaultCredentials);
		}

		private void CheckBypassList()
		{
			if (bypassList != null)
			{
				for (int i = 0; i < bypassList.Count; i++)
				{
					new Regex((string)bypassList[i]);
				}
			}
		}

		private static Uri ToUri(string address)
		{
			if (address == null)
			{
				return null;
			}
			if (address.IndexOf("://") == -1)
			{
				address = "http://" + address;
			}
			return new Uri(address);
		}
	}
}
