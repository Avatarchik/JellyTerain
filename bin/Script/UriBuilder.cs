using System.Text;

namespace System
{
	/// <summary>Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs for the <see cref="T:System.Uri" /> class.</summary>
	/// <filterpriority>2</filterpriority>
	public class UriBuilder
	{
		private string scheme;

		private string host;

		private int port;

		private string path;

		private string query;

		private string fragment;

		private string username;

		private string password;

		private Uri uri;

		private bool modified;

		/// <summary>Gets or sets the fragment portion of the URI.</summary>
		/// <returns>The fragment portion of the URI. The fragment identifier ("#") is added to the beginning of the fragment.</returns>
		/// <filterpriority>2</filterpriority>
		public string Fragment
		{
			get
			{
				return fragment;
			}
			set
			{
				fragment = value;
				if (fragment == null)
				{
					fragment = string.Empty;
				}
				else if (fragment.Length > 0)
				{
					fragment = "#" + value.Replace("%23", "#");
				}
				modified = true;
			}
		}

		/// <summary>Gets or sets the Domain Name System (DNS) host name or IP address of a server.</summary>
		/// <returns>The DNS host name or IP address of the server.</returns>
		/// <filterpriority>1</filterpriority>
		public string Host
		{
			get
			{
				return host;
			}
			set
			{
				host = ((value != null) ? value : string.Empty);
				modified = true;
			}
		}

		/// <summary>Gets or sets the password associated with the user that accesses the URI.</summary>
		/// <returns>The password of the user that accesses the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = ((value != null) ? value : string.Empty);
				modified = true;
			}
		}

		/// <summary>Gets or sets the path to the resource referenced by the URI.</summary>
		/// <returns>The path to the resource referenced by the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					path = "/";
				}
				else
				{
					path = Uri.EscapeString(value.Replace('\\', '/'), escapeReserved: false, escapeHex: true, escapeBrackets: true);
				}
				modified = true;
			}
		}

		/// <summary>Gets or sets the port number of the URI.</summary>
		/// <returns>The port number of the URI.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port cannot be set to a value less than 0 or greater than 65,535. </exception>
		/// <filterpriority>1</filterpriority>
		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				port = value;
				modified = true;
			}
		}

		/// <summary>Gets or sets any query information included in the URI.</summary>
		/// <returns>The query information included in the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Query
		{
			get
			{
				return query;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					query = string.Empty;
				}
				else
				{
					query = "?" + value;
				}
				modified = true;
			}
		}

		/// <summary>Gets or sets the scheme name of the URI.</summary>
		/// <returns>The scheme of the URI.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The scheme cannot be set to an invalid scheme name. </exception>
		/// <filterpriority>1</filterpriority>
		public string Scheme
		{
			get
			{
				return scheme;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				int num = value.IndexOf(':');
				if (num != -1)
				{
					value = value.Substring(0, num);
				}
				scheme = value.ToLower();
				modified = true;
			}
		}

		/// <summary>Gets the <see cref="T:System.Uri" /> instance constructed by the specified <see cref="T:System.UriBuilder" /> instance.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI constructed by the <see cref="T:System.UriBuilder" />.</returns>
		/// <exception cref="T:System.UriFormatException">The URI constructed by the <see cref="T:System.UriBuilder" /> properties is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public Uri Uri
		{
			get
			{
				if (!modified)
				{
					return uri;
				}
				uri = new Uri(ToString(), dontEscape: true);
				modified = false;
				return uri;
			}
		}

		/// <summary>The user name associated with the user that accesses the URI.</summary>
		/// <returns>The user name of the user that accesses the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string UserName
		{
			get
			{
				return username;
			}
			set
			{
				username = ((value != null) ? value : string.Empty);
				modified = true;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class.</summary>
		public UriBuilder()
			: this(Uri.UriSchemeHttp, "localhost")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified URI.</summary>
		/// <param name="uri">A URI string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="uri" /> is a zero length string or contains only spaces.-or- The parsing routine detected a scheme in an invalid form.-or- The parser detected more than two consecutive slashes in a URI that does not use the "file" scheme.-or- <paramref name="uri" /> is not a valid URI. </exception>
		public UriBuilder(string uri)
			: this(new Uri(uri))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified <see cref="T:System.Uri" /> instance.</summary>
		/// <param name="uri">An instance of the <see cref="T:System.Uri" /> class. </param>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="uri" /> is null. </exception>
		public UriBuilder(Uri uri)
		{
			scheme = uri.Scheme;
			host = uri.Host;
			port = uri.Port;
			path = uri.AbsolutePath;
			query = uri.Query;
			fragment = uri.Fragment;
			username = uri.UserInfo;
			int num = username.IndexOf(':');
			if (num != -1)
			{
				password = username.Substring(num + 1);
				username = username.Substring(0, num);
			}
			else
			{
				password = string.Empty;
			}
			modified = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme and host.</summary>
		/// <param name="schemeName">An Internet access protocol. </param>
		/// <param name="hostName">A DNS-style domain name or IP address. </param>
		public UriBuilder(string schemeName, string hostName)
		{
			Scheme = schemeName;
			Host = hostName;
			port = -1;
			Path = string.Empty;
			query = string.Empty;
			fragment = string.Empty;
			username = string.Empty;
			password = string.Empty;
			modified = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, and port.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="portNumber">An IP port number for the service. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="portNumber" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int portNumber)
			: this(scheme, host)
		{
			Port = portNumber;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, and path.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="port">An IP port number for the service. </param>
		/// <param name="pathValue">The path to the Internet resource. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int port, string pathValue)
			: this(scheme, host, port)
		{
			Path = pathValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, path and query string or fragment identifier.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="port">An IP port number for the service. </param>
		/// <param name="path">The path to the Internet resource. </param>
		/// <param name="extraValue">A query string or fragment identifier. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="extraValue" /> is neither null nor <see cref="F:System.String.Empty" />, nor does a valid fragment identifier begin with a number sign (#), nor a valid query string begin with a question mark (?). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int port, string pathValue, string extraValue)
			: this(scheme, host, port, pathValue)
		{
			if (extraValue == null || extraValue.Length == 0)
			{
				return;
			}
			if (extraValue[0] == '#')
			{
				Fragment = extraValue.Remove(0, 1);
				return;
			}
			if (extraValue[0] == '?')
			{
				Query = extraValue.Remove(0, 1);
				return;
			}
			throw new ArgumentException("extraValue");
		}

		/// <summary>Compares an existing <see cref="T:System.Uri" /> instance with the contents of the <see cref="T:System.UriBuilder" /> for equality.</summary>
		/// <returns>true if <paramref name="rparam" /> represents the same <see cref="T:System.Uri" /> as the <see cref="T:System.Uri" /> constructed by this <see cref="T:System.UriBuilder" /> instance; otherwise, false.</returns>
		/// <param name="rparam">The object to compare with the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object rparam)
		{
			return rparam != null && Uri.Equals(rparam.ToString());
		}

		/// <summary>Returns the hash code for the URI.</summary>
		/// <returns>The hash code generated for the URI.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return Uri.GetHashCode();
		}

		/// <summary>Returns the display string for the specified <see cref="T:System.UriBuilder" /> instance.</summary>
		/// <returns>The string that contains the unescaped display string of the <see cref="T:System.UriBuilder" />.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(scheme);
			stringBuilder.Append("://");
			if (username != string.Empty)
			{
				stringBuilder.Append(username);
				if (password != string.Empty)
				{
					stringBuilder.Append(":" + password);
				}
				stringBuilder.Append('@');
			}
			stringBuilder.Append(host);
			if (port > 0)
			{
				stringBuilder.Append(":" + port);
			}
			if (path != string.Empty && stringBuilder[stringBuilder.Length - 1] != '/' && path.Length > 0 && path[0] != '/')
			{
				stringBuilder.Append('/');
			}
			stringBuilder.Append(path);
			stringBuilder.Append(query);
			stringBuilder.Append(fragment);
			return stringBuilder.ToString();
		}
	}
}
