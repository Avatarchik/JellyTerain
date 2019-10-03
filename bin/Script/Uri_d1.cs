using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
	/// <summary>Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.</summary>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	[TypeConverter(typeof(UriTypeConverter))]
	public class Uri : ISerializable
	{
		private struct UriScheme
		{
			public string scheme;

			public string delimiter;

			public int defaultPort;

			public UriScheme(string s, string d, int p)
			{
				scheme = s;
				delimiter = d;
				defaultPort = p;
			}
		}

		private const int MaxUriLength = 32766;

		private bool isUnixFilePath;

		private string source;

		private string scheme = string.Empty;

		private string host = string.Empty;

		private int port = -1;

		private string path = string.Empty;

		private string query = string.Empty;

		private string fragment = string.Empty;

		private string userinfo = string.Empty;

		private bool isUnc;

		private bool isOpaquePart;

		private bool isAbsoluteUri = true;

		private string[] segments;

		private bool userEscaped;

		private string cachedAbsoluteUri;

		private string cachedToString;

		private string cachedLocalPath;

		private int cachedHashCode;

		private static readonly string hexUpperChars = "0123456789ABCDEF";

		/// <summary>Specifies the characters that separate the communication protocol scheme from the address portion of the URI. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string SchemeDelimiter = "://";

		/// <summary>Specifies that the URI is a pointer to a file. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeFile = "file";

		/// <summary>Specifies that the URI is accessed through the File Transfer Protocol (FTP). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeFtp = "ftp";

		/// <summary>Specifies that the URI is accessed through the Gopher protocol. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeGopher = "gopher";

		/// <summary>Specifies that the URI is accessed through the Hypertext Transfer Protocol (HTTP). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeHttp = "http";

		/// <summary>Specifies that the URI is accessed through the Secure Hypertext Transfer Protocol (HTTPS). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeHttps = "https";

		/// <summary>Specifies that the URI is an e-mail address and is accessed through the Simple Mail Transport Protocol (SMTP). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeMailto = "mailto";

		/// <summary>Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeNews = "news";

		/// <summary>Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP). This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly string UriSchemeNntp = "nntp";

		/// <summary>Specifies that the URI is accessed through the NetPipe scheme used by Windows Communication Foundation (WCF). This field is read-only.</summary>
		public static readonly string UriSchemeNetPipe = "net.pipe";

		/// <summary>Specifies that the URI is accessed through the NetTcp scheme used by Windows Communication Foundation (WCF). This field is read-only.</summary>
		public static readonly string UriSchemeNetTcp = "net.tcp";

		private static UriScheme[] schemes = new UriScheme[8]
		{
			new UriScheme(UriSchemeHttp, SchemeDelimiter, 80),
			new UriScheme(UriSchemeHttps, SchemeDelimiter, 443),
			new UriScheme(UriSchemeFtp, SchemeDelimiter, 21),
			new UriScheme(UriSchemeFile, SchemeDelimiter, -1),
			new UriScheme(UriSchemeMailto, ":", 25),
			new UriScheme(UriSchemeNews, ":", 119),
			new UriScheme(UriSchemeNntp, SchemeDelimiter, 119),
			new UriScheme(UriSchemeGopher, SchemeDelimiter, 70)
		};

		[NonSerialized]
		private UriParser parser;

		/// <summary>Gets the absolute path of the URI.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the absolute path to the resource.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string AbsolutePath
		{
			get
			{
				EnsureAbsoluteUri();
				switch (Scheme)
				{
				case "mailto":
				case "file":
					return path;
				default:
					if (path.Length == 0)
					{
						string value = Scheme + SchemeDelimiter;
						if (path.StartsWith(value))
						{
							return "/";
						}
						return string.Empty;
					}
					return path;
				}
			}
		}

		/// <summary>Gets the absolute URI.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the entire URI.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string AbsoluteUri
		{
			get
			{
				EnsureAbsoluteUri();
				if (cachedAbsoluteUri == null)
				{
					cachedAbsoluteUri = GetLeftPart(UriPartial.Path);
					if (query.Length > 0)
					{
						cachedAbsoluteUri += query;
					}
					if (fragment.Length > 0)
					{
						cachedAbsoluteUri += fragment;
					}
				}
				return cachedAbsoluteUri;
			}
		}

		/// <summary>Gets the Domain Name System (DNS) host name or IP address and the port number for a server.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the authority component of the URI represented by this instance.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Authority
		{
			get
			{
				EnsureAbsoluteUri();
				return (GetDefaultPort(Scheme) != port) ? (host + ":" + port) : host;
			}
		}

		/// <summary>Gets the escaped URI fragment.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains any URI fragment information.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Fragment
		{
			get
			{
				EnsureAbsoluteUri();
				return fragment;
			}
		}

		/// <summary>Gets the host component of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the host name. This is usually the DNS host name or IP address of the server.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Host
		{
			get
			{
				EnsureAbsoluteUri();
				return host;
			}
		}

		/// <summary>Gets the type of the host name specified in the URI.</summary>
		/// <returns>A member of the <see cref="T:System.UriHostNameType" /> enumeration.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public UriHostNameType HostNameType
		{
			get
			{
				EnsureAbsoluteUri();
				UriHostNameType uriHostNameType = CheckHostName(Host);
				if (uriHostNameType != 0)
				{
					return uriHostNameType;
				}
				switch (Scheme)
				{
				case "mailto":
					return UriHostNameType.Basic;
				default:
					return IsFile ? UriHostNameType.Basic : uriHostNameType;
				}
			}
		}

		/// <summary>Gets whether the port value of the URI is the default for this scheme.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the value in the <see cref="P:System.Uri.Port" /> property is the default port for this scheme; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool IsDefaultPort
		{
			get
			{
				EnsureAbsoluteUri();
				return GetDefaultPort(Scheme) == port;
			}
		}

		/// <summary>Gets a value indicating whether the specified <see cref="T:System.Uri" /> is a file URI.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> is a file URI; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool IsFile
		{
			get
			{
				EnsureAbsoluteUri();
				return Scheme == UriSchemeFile;
			}
		}

		/// <summary>Gets whether the specified <see cref="T:System.Uri" /> references the local host.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if this <see cref="T:System.Uri" /> references the local host; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool IsLoopback
		{
			get
			{
				EnsureAbsoluteUri();
				if (Host.Length == 0)
				{
					return IsFile;
				}
				if (host == "loopback" || host == "localhost")
				{
					return true;
				}
				if (IPAddress.TryParse(host, out IPAddress address) && IPAddress.Loopback.Equals(address))
				{
					return true;
				}
				if (System.Net.IPv6Address.TryParse(host, out System.Net.IPv6Address result) && System.Net.IPv6Address.IsLoopback(result))
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>Gets whether the specified <see cref="T:System.Uri" /> is a universal naming convention (UNC) path.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> is a UNC path; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool IsUnc
		{
			get
			{
				EnsureAbsoluteUri();
				return isUnc;
			}
		}

		/// <summary>Gets a local operating-system representation of a file name.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the local operating-system representation of a file name.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string LocalPath
		{
			get
			{
				EnsureAbsoluteUri();
				if (cachedLocalPath != null)
				{
					return cachedLocalPath;
				}
				if (!IsFile)
				{
					return AbsolutePath;
				}
				bool flag = path.Length > 3 && path[1] == ':' && (path[2] == '\\' || path[2] == '/');
				if (!IsUnc)
				{
					string text = Unescape(path);
					if (flag)
					{
						cachedLocalPath = text.Replace('/', '\\');
					}
					else
					{
						cachedLocalPath = text;
					}
				}
				else if (path.Length > 1 && path[1] == ':')
				{
					cachedLocalPath = Unescape(path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
				}
				else if (Path.DirectorySeparatorChar == '\\')
				{
					string text2 = host;
					if (path.Length > 0 && (path.Length > 1 || path[0] != '/'))
					{
						text2 += path.Replace('/', '\\');
					}
					cachedLocalPath = "\\\\" + Unescape(text2);
				}
				else
				{
					cachedLocalPath = Unescape(path);
				}
				if (cachedLocalPath.Length == 0)
				{
					cachedLocalPath = Path.DirectorySeparatorChar.ToString();
				}
				return cachedLocalPath;
			}
		}

		/// <summary>Gets the <see cref="P:System.Uri.AbsolutePath" /> and <see cref="P:System.Uri.Query" /> properties separated by a question mark (?).</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the <see cref="P:System.Uri.AbsolutePath" /> and <see cref="P:System.Uri.Query" /> properties separated by a question mark (?).</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string PathAndQuery
		{
			get
			{
				EnsureAbsoluteUri();
				return path + Query;
			}
		}

		/// <summary>Gets the port number of this URI.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the port number for this URI.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public int Port
		{
			get
			{
				EnsureAbsoluteUri();
				return port;
			}
		}

		/// <summary>Gets any query information included in the specified URI.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains any query information included in the specified URI.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Query
		{
			get
			{
				EnsureAbsoluteUri();
				return query;
			}
		}

		/// <summary>Gets the scheme name for this URI.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the scheme for this URI, converted to lowercase.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Scheme
		{
			get
			{
				EnsureAbsoluteUri();
				return scheme;
			}
		}

		/// <summary>Gets an array containing the path segments that make up the specified URI.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains the path segments that make up the specified URI.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string[] Segments
		{
			get
			{
				EnsureAbsoluteUri();
				if (segments != null)
				{
					return segments;
				}
				if (path.Length == 0)
				{
					segments = new string[0];
					return segments;
				}
				string[] array = segments = path.Split('/');
				bool flag = path.EndsWith("/");
				if (array.Length > 0 && flag)
				{
					string[] array2 = new string[array.Length - 1];
					Array.Copy(array, 0, array2, 0, array.Length - 1);
					array = array2;
				}
				int i = 0;
				if (IsFile && path.Length > 1 && path[1] == ':')
				{
					string[] array3 = new string[array.Length + 1];
					Array.Copy(array, 1, array3, 2, array.Length - 1);
					array = array3;
					array[0] = path.Substring(0, 2);
					array[1] = string.Empty;
					i++;
				}
				for (int num = array.Length; i < num; i++)
				{
					if (i != num - 1 || flag)
					{
						array[i] += '/';
					}
				}
				segments = array;
				return segments;
			}
		}

		/// <summary>Indicates that the URI string was completely escaped before the <see cref="T:System.Uri" /> instance was created.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <paramref name="dontEscape" /> parameter was set to true when the <see cref="T:System.Uri" /> instance was created; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool UserEscaped => userEscaped;

		/// <summary>Gets the user name, password, or other user-specific information associated with the specified URI.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the user information associated with the URI. The returned value does not include the '@' character reserved for delimiting the user information part of the URI.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string UserInfo
		{
			get
			{
				EnsureAbsoluteUri();
				return userinfo;
			}
		}

		/// <summary>Gets an unescaped host name that is safe to use for DNS resolution.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the unescaped host part of the URI that is suitable for DNS resolution; or the original unescaped host string, if it is already suitable for resolution.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[MonoTODO("add support for IPv6 address")]
		public string DnsSafeHost
		{
			get
			{
				EnsureAbsoluteUri();
				return Unescape(Host);
			}
		}

		/// <summary>Gets whether the <see cref="T:System.Uri" /> instance is absolute.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> instance is absolute; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public bool IsAbsoluteUri => isAbsoluteUri;

		/// <summary>Gets the original URI string that was passed to the <see cref="T:System.Uri" /> constructor.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the exact URI specified when this instance was constructed; otherwise, <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string OriginalString => (source == null) ? ToString() : source;

		private UriParser Parser
		{
			get
			{
				if (parser == null)
				{
					parser = UriParser.GetParser(Scheme);
					if (parser == null)
					{
						parser = new DefaultUriParser("*");
					}
				}
				return parser;
			}
			set
			{
				parser = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI.</summary>
		/// <param name="uriString">A URI. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="uriString" /> is empty.-or- The scheme specified in <paramref name="uriString" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or- <paramref name="uriString" /> contains too many slashes.-or- The password specified in <paramref name="uriString" /> is not valid.-or- The host name specified in <paramref name="uriString" /> is not valid.-or- The file name specified in <paramref name="uriString" /> is not valid. -or- The user name specified in <paramref name="uriString" /> is not valid.-or- The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.-or- The length of <paramref name="uriString" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="uriString" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		public Uri(string uriString)
			: this(uriString, dontEscape: false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">An instance of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class containing the information required to serialize the new <see cref="T:System.Uri" /> instance. </param>
		/// <param name="streamingContext">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class containing the source of the serialized stream associated with the new <see cref="T:System.Uri" /> instance. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationInfo" /> parameter contains a null URI. </exception>
		/// <exception cref="T:System.UriFormatException">The <paramref name="serializationInfo" /> parameter contains a URI that is empty.-or- The scheme specified is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or- The URI contains too many slashes.-or- The password specified in the URI is not valid.-or- The host name specified in URI is not valid.-or- The file name specified in the URI is not valid. -or- The user name specified in the URI is not valid.-or- The host or authority name specified in the URI cannot be terminated by backslashes.-or- The port number specified in the URI is not valid or cannot be parsed.-or- The length of URI exceeds 65519 characters.-or- The length of the scheme specified in the URI exceeds 1023 characters.-or- There is an invalid character sequence in the URI.-or- The MS-DOS path specified in the URI must start with c:\\.</exception>
		protected Uri(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: this(serializationInfo.GetString("AbsoluteUri"), dontEscape: true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI. This constructor allows you to specify if the URI string is a relative URI, absolute URI, or is indeterminate.</summary>
		/// <param name="uriString">A string that identifies the resource to be represented by the <see cref="T:System.Uri" /> instance.</param>
		/// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="uriKind" /> is invalid. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="uriString" /> contains a relative URI and <paramref name="uriKind" /> is <see cref="F:System.UriKind.Absolute" />.or<paramref name="uriString" /> contains an absolute URI and <paramref name="uriKind" /> is <see cref="F:System.UriKind.Relative" />.or<paramref name="uriString" /> is empty.-or- The scheme specified in <paramref name="uriString" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or- <paramref name="uriString" /> contains too many slashes.-or- The password specified in <paramref name="uriString" /> is not valid.-or- The host name specified in <paramref name="uriString" /> is not valid.-or- The file name specified in <paramref name="uriString" /> is not valid. -or- The user name specified in <paramref name="uriString" /> is not valid.-or- The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.-or- The length of <paramref name="uriString" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="uriString" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		public Uri(string uriString, UriKind uriKind)
		{
			source = uriString;
			ParseUri(uriKind);
			switch (uriKind)
			{
			case UriKind.RelativeOrAbsolute:
				break;
			case UriKind.Absolute:
				if (!IsAbsoluteUri)
				{
					throw new UriFormatException("Invalid URI: The format of the URI could not be determined.");
				}
				break;
			case UriKind.Relative:
				if (IsAbsoluteUri)
				{
					throw new UriFormatException("Invalid URI: The format of the URI could not be determined because the parameter 'uriString' represents an absolute URI.");
				}
				break;
			default:
			{
				string text = Locale.GetText("Invalid UriKind value '{0}'.", uriKind);
				throw new ArgumentException(text);
			}
			}
		}

		private Uri(string uriString, UriKind uriKind, out bool success)
		{
			if (uriString == null)
			{
				success = false;
				return;
			}
			if (uriKind != 0 && uriKind != UriKind.Absolute && uriKind != UriKind.Relative)
			{
				string text = Locale.GetText("Invalid UriKind value '{0}'.", uriKind);
				throw new ArgumentException(text);
			}
			source = uriString;
			if (ParseNoExceptions(uriKind, uriString) != null)
			{
				success = false;
				return;
			}
			success = true;
			switch (uriKind)
			{
			case UriKind.RelativeOrAbsolute:
				break;
			case UriKind.Absolute:
				if (!IsAbsoluteUri)
				{
					success = false;
				}
				break;
			case UriKind.Relative:
				if (IsAbsoluteUri)
				{
					success = false;
				}
				break;
			default:
				success = false;
				break;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the combination of a specified base <see cref="T:System.Uri" /> instance and a relative <see cref="T:System.Uri" /> instance.</summary>
		/// <param name="baseUri">An absolute <see cref="T:System.Uri" /> that is the base for the new <see cref="T:System.Uri" /> instance. </param>
		/// <param name="relativeUri">A relative <see cref="T:System.Uri" /> instance that is combined with <paramref name="baseUri" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="baseUri" /> is null.-or- <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance. </exception>
		/// <exception cref="T:System.UriFormatException">The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.-or- The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.-or- The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid. -or- The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.-or- The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.-or- The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.-or- The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.-or- There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		public Uri(Uri baseUri, Uri relativeUri)
		{
			Merge(baseUri, (!(relativeUri == null)) ? relativeUri.OriginalString : string.Empty);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI, with explicit control of character escaping.</summary>
		/// <param name="uriString">The URI. </param>
		/// <param name="dontEscape">true if <paramref name="uriString" /> is completely escaped; otherwise, false. See Remarks. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="uriString" /> is empty or contains only spaces.-or- The scheme specified in <paramref name="uriString" /> is not valid.-or- <paramref name="uriString" /> contains too many slashes.-or- The password specified in <paramref name="uriString" /> is not valid.-or- The host name specified in <paramref name="uriString" /> is not valid.-or- The file name specified in <paramref name="uriString" /> is not valid. -or- The user name specified in <paramref name="uriString" /> is not valid.-or- The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.-or- The length of <paramref name="uriString" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="uriString" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		[Obsolete]
		public Uri(string uriString, bool dontEscape)
		{
			userEscaped = dontEscape;
			source = uriString;
			ParseUri(UriKind.Absolute);
			if (!isAbsoluteUri)
			{
				throw new UriFormatException("Invalid URI: The format of the URI could not be determined: " + uriString);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the specified base URI and relative URI string.</summary>
		/// <param name="baseUri">The base URI. </param>
		/// <param name="relativeUri">The relative URI to add to the base URI. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance. </exception>
		/// <exception cref="T:System.UriFormatException">The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.-or- The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.-or- The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid. -or- The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.-or- The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.-or- The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.-or- The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.-or- There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		public Uri(Uri baseUri, string relativeUri)
		{
			Merge(baseUri, relativeUri);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the specified base and relative URIs, with explicit control of character escaping.</summary>
		/// <param name="baseUri">The base URI. </param>
		/// <param name="relativeUri">The relative URI to add to the base URI. </param>
		/// <param name="dontEscape">true if <paramref name="uriString" /> is completely escaped; otherwise, false. See Remarks. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance. </exception>
		/// <exception cref="T:System.UriFormatException">The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.-or- The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.-or- The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid. -or- The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.-or- The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.-or- The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.-or- The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.-or- The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.-or- There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.-or- The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
		[Obsolete("dontEscape is always false")]
		public Uri(Uri baseUri, string relativeUri, bool dontEscape)
		{
			userEscaped = dontEscape;
			Merge(baseUri, relativeUri);
		}

		/// <summary>Returns the data needed to serialize the current instance.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Uri" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Uri" />.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AbsoluteUri", AbsoluteUri);
		}

		private void Merge(Uri baseUri, string relativeUri)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException("baseUri");
			}
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ArgumentOutOfRangeException("baseUri");
			}
			if (relativeUri == null)
			{
				relativeUri = string.Empty;
			}
			if (relativeUri.Length >= 2 && relativeUri[0] == '\\' && relativeUri[1] == '\\')
			{
				source = relativeUri;
				ParseUri(UriKind.Absolute);
				return;
			}
			int num = relativeUri.IndexOf(':');
			if (num != -1)
			{
				int num2 = relativeUri.IndexOfAny(new char[3]
				{
					'/',
					'\\',
					'?'
				});
				if (num2 > num || num2 < 0)
				{
					if (string.CompareOrdinal(baseUri.Scheme, 0, relativeUri, 0, num) != 0 || !IsPredefinedScheme(baseUri.Scheme) || (relativeUri.Length > num + 1 && relativeUri[num + 1] == '/'))
					{
						source = relativeUri;
						ParseUri(UriKind.Absolute);
						return;
					}
					relativeUri = relativeUri.Substring(num + 1);
				}
			}
			scheme = baseUri.scheme;
			host = baseUri.host;
			port = baseUri.port;
			userinfo = baseUri.userinfo;
			isUnc = baseUri.isUnc;
			isUnixFilePath = baseUri.isUnixFilePath;
			isOpaquePart = baseUri.isOpaquePart;
			if (relativeUri == string.Empty)
			{
				path = baseUri.path;
				query = baseUri.query;
				fragment = baseUri.fragment;
				return;
			}
			num = relativeUri.IndexOf('#');
			if (num != -1)
			{
				if (userEscaped)
				{
					fragment = relativeUri.Substring(num);
				}
				else
				{
					fragment = "#" + EscapeString(relativeUri.Substring(num + 1));
				}
				relativeUri = relativeUri.Substring(0, num);
			}
			num = relativeUri.IndexOf('?');
			if (num != -1)
			{
				query = relativeUri.Substring(num);
				if (!userEscaped)
				{
					query = EscapeString(query);
				}
				relativeUri = relativeUri.Substring(0, num);
			}
			if (relativeUri.Length > 0 && relativeUri[0] == '/')
			{
				if (relativeUri.Length > 1 && relativeUri[1] == '/')
				{
					source = scheme + ':' + relativeUri;
					ParseUri(UriKind.Absolute);
					return;
				}
				path = relativeUri;
				if (!userEscaped)
				{
					path = EscapeString(path);
				}
				return;
			}
			path = baseUri.path;
			if (relativeUri.Length > 0 || query.Length > 0)
			{
				num = path.LastIndexOf('/');
				if (num >= 0)
				{
					path = path.Substring(0, num + 1);
				}
			}
			if (relativeUri.Length == 0)
			{
				return;
			}
			path += relativeUri;
			int startIndex = 0;
			while (true)
			{
				num = path.IndexOf("./", startIndex);
				switch (num)
				{
				case 0:
					path = path.Remove(0, 2);
					break;
				default:
					if (path[num - 1] != '.')
					{
						path = path.Remove(num, 2);
					}
					else
					{
						startIndex = num + 1;
					}
					break;
				case -1:
					if (path.Length > 1 && path[path.Length - 1] == '.' && path[path.Length - 2] == '/')
					{
						path = path.Remove(path.Length - 1, 1);
					}
					startIndex = 0;
					while (true)
					{
						num = path.IndexOf("/../", startIndex);
						switch (num)
						{
						case 0:
							startIndex = 3;
							break;
						default:
						{
							int num3 = path.LastIndexOf('/', num - 1);
							if (num3 == -1)
							{
								startIndex = num + 1;
							}
							else if (path.Substring(num3 + 1, num - num3 - 1) != "..")
							{
								path = path.Remove(num3 + 1, num - num3 + 3);
							}
							else
							{
								startIndex = num + 1;
							}
							break;
						}
						case -1:
							if (path.Length > 3 && path.EndsWith("/.."))
							{
								num = path.LastIndexOf('/', path.Length - 4);
								if (num != -1 && path.Substring(num + 1, path.Length - num - 4) != "..")
								{
									path = path.Remove(num + 1, path.Length - num - 1);
								}
							}
							if (!userEscaped)
							{
								path = EscapeString(path);
							}
							return;
						}
					}
				}
			}
		}

		/// <summary>Determines whether the specified host name is a valid DNS name.</summary>
		/// <returns>A <see cref="T:System.UriHostNameType" /> that indicates the type of the host name. If the type of the host name cannot be determined or if the host name is null or a zero-length string, this method returns <see cref="F:System.UriHostNameType.Unknown" />.</returns>
		/// <param name="name">The host name to validate. This can be an IPv4 or IPv6 address or an Internet host name. </param>
		/// <filterpriority>1</filterpriority>
		public static UriHostNameType CheckHostName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return UriHostNameType.Unknown;
			}
			if (IsIPv4Address(name))
			{
				return UriHostNameType.IPv4;
			}
			if (IsDomainAddress(name))
			{
				return UriHostNameType.Dns;
			}
			if (System.Net.IPv6Address.TryParse(name, out System.Net.IPv6Address _))
			{
				return UriHostNameType.IPv6;
			}
			return UriHostNameType.Unknown;
		}

		internal static bool IsIPv4Address(string name)
		{
			string[] array = name.Split('.');
			if (array.Length != 4)
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				if (array[i].Length == 0)
				{
					return false;
				}
				if (!uint.TryParse(array[i], out uint result))
				{
					return false;
				}
				if (result > 255)
				{
					return false;
				}
			}
			return true;
		}

		internal static bool IsDomainAddress(string name)
		{
			int length = name.Length;
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				char c = name[i];
				if (num == 0)
				{
					if (!char.IsLetterOrDigit(c))
					{
						return false;
					}
				}
				else if (c == '.')
				{
					num = 0;
				}
				else if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
				{
					return false;
				}
				if (++num == 64)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Determines whether the specified scheme name is valid.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the scheme name is valid; otherwise, false.</returns>
		/// <param name="schemeName">The scheme name to validate. </param>
		/// <filterpriority>1</filterpriority>
		public static bool CheckSchemeName(string schemeName)
		{
			if (schemeName == null || schemeName.Length == 0)
			{
				return false;
			}
			if (!IsAlpha(schemeName[0]))
			{
				return false;
			}
			int length = schemeName.Length;
			for (int i = 1; i < length; i++)
			{
				char c = schemeName[i];
				if (!char.IsDigit(c) && !IsAlpha(c) && c != '.' && c != '+' && c != '-')
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsAlpha(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		/// <summary>Compares two <see cref="T:System.Uri" /> instances for equality.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the two instances represent the same URI; otherwise, false.</returns>
		/// <param name="comparand">The <see cref="T:System.Uri" /> instance or a URI identifier to compare with the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object comparant)
		{
			if (comparant == null)
			{
				return false;
			}
			Uri uri = comparant as Uri;
			if ((object)uri == null)
			{
				string text = comparant as string;
				if (text == null)
				{
					return false;
				}
				uri = new Uri(text);
			}
			return InternalEquals(uri);
		}

		private bool InternalEquals(Uri uri)
		{
			if (isAbsoluteUri != uri.isAbsoluteUri)
			{
				return false;
			}
			if (!isAbsoluteUri)
			{
				return source == uri.source;
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return scheme.ToLower(invariantCulture) == uri.scheme.ToLower(invariantCulture) && host.ToLower(invariantCulture) == uri.host.ToLower(invariantCulture) && port == uri.port && query == uri.query && path == uri.path;
		}

		/// <summary>Gets the hash code for the URI.</summary>
		/// <returns>An <see cref="T:System.Int32" /> containing the hash value generated for this URI.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			if (cachedHashCode == 0)
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				if (isAbsoluteUri)
				{
					cachedHashCode = (scheme.ToLower(invariantCulture).GetHashCode() ^ host.ToLower(invariantCulture).GetHashCode() ^ port ^ query.GetHashCode() ^ path.GetHashCode());
				}
				else
				{
					cachedHashCode = source.GetHashCode();
				}
			}
			return cachedHashCode;
		}

		/// <summary>Gets the specified portion of a <see cref="T:System.Uri" /> instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the specified portion of the <see cref="T:System.Uri" /> instance.</returns>
		/// <param name="part">One of the <see cref="T:System.UriPartial" /> values that specifies the end of the URI portion to return. </param>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Uri" /> instance is not an absolute instance. </exception>
		/// <exception cref="T:System.ArgumentException">The specified <paramref name="part" /> is not valid. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string GetLeftPart(UriPartial part)
		{
			EnsureAbsoluteUri();
			switch (part)
			{
			case UriPartial.Scheme:
				return scheme + GetOpaqueWiseSchemeDelimiter();
			case UriPartial.Authority:
			{
				if (scheme == UriSchemeMailto || scheme == UriSchemeNews)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(scheme);
				stringBuilder2.Append(GetOpaqueWiseSchemeDelimiter());
				if (path.Length > 1 && path[1] == ':' && UriSchemeFile == scheme)
				{
					stringBuilder2.Append('/');
				}
				if (userinfo.Length > 0)
				{
					stringBuilder2.Append(userinfo).Append('@');
				}
				stringBuilder2.Append(host);
				int defaultPort = GetDefaultPort(scheme);
				if (port != -1 && port != defaultPort)
				{
					stringBuilder2.Append(':').Append(port);
				}
				return stringBuilder2.ToString();
			}
			case UriPartial.Path:
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(scheme);
				stringBuilder.Append(GetOpaqueWiseSchemeDelimiter());
				if (path.Length > 1 && path[1] == ':' && UriSchemeFile == scheme)
				{
					stringBuilder.Append('/');
				}
				if (userinfo.Length > 0)
				{
					stringBuilder.Append(userinfo).Append('@');
				}
				stringBuilder.Append(host);
				int defaultPort = GetDefaultPort(scheme);
				if (port != -1 && port != defaultPort)
				{
					stringBuilder.Append(':').Append(port);
				}
				if (path.Length > 0)
				{
					switch (Scheme)
					{
					case "mailto":
					case "news":
						stringBuilder.Append(path);
						break;
					default:
						stringBuilder.Append(Reduce(path, CompactEscaped(Scheme)));
						break;
					}
				}
				return stringBuilder.ToString();
			}
			default:
				return null;
			}
		}

		/// <summary>Gets the decimal value of a hexadecimal digit.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains a number from 0 to 15 that corresponds to the specified hexadecimal digit.</returns>
		/// <param name="digit">The hexadecimal digit (0-9, a-f, A-F) to convert. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="digit" /> is not a valid hexadecimal digit (0-9, a-f, A-F). </exception>
		/// <filterpriority>1</filterpriority>
		public static int FromHex(char digit)
		{
			if ('0' <= digit && digit <= '9')
			{
				return digit - 48;
			}
			if ('a' <= digit && digit <= 'f')
			{
				return digit - 97 + 10;
			}
			if ('A' <= digit && digit <= 'F')
			{
				return digit - 65 + 10;
			}
			throw new ArgumentException("digit");
		}

		/// <summary>Converts a specified character into its hexadecimal equivalent.</summary>
		/// <returns>The hexadecimal representation of the specified character.</returns>
		/// <param name="character">The character to convert to hexadecimal representation. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="character" /> is greater than 255. </exception>
		/// <filterpriority>1</filterpriority>
		public static string HexEscape(char character)
		{
			if (character > 'ÿ')
			{
				throw new ArgumentOutOfRangeException("character");
			}
			return "%" + hexUpperChars[(character & 0xF0) >> 4] + hexUpperChars[character & 0xF];
		}

		/// <summary>Converts a specified hexadecimal representation of a character to the character.</summary>
		/// <returns>The character represented by the hexadecimal encoding at position <paramref name="index" />. If the character at <paramref name="index" /> is not hexadecimal encoded, the character at <paramref name="index" /> is returned. The value of <paramref name="index" /> is incremented to point to the character following the one returned.</returns>
		/// <param name="pattern">The hexadecimal representation of a character. </param>
		/// <param name="index">The location in <paramref name="pattern" /> where the hexadecimal representation of a character begins. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0 or greater than or equal to the number of characters in <paramref name="pattern" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static char HexUnescape(string pattern, ref int index)
		{
			if (pattern == null)
			{
				throw new ArgumentException("pattern");
			}
			if (index < 0 || index >= pattern.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (!IsHexEncoding(pattern, index))
			{
				return pattern[index++];
			}
			index++;
			int num = FromHex(pattern[index++]);
			int num2 = FromHex(pattern[index++]);
			return (char)((num << 4) | num2);
		}

		/// <summary>Determines whether a specified character is a valid hexadecimal digit.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the character is a valid hexadecimal digit; otherwise false.</returns>
		/// <param name="character">The character to validate. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsHexDigit(char digit)
		{
			return ('0' <= digit && digit <= '9') || ('a' <= digit && digit <= 'f') || ('A' <= digit && digit <= 'F');
		}

		/// <summary>Determines whether a character in a string is hexadecimal encoded.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if <paramref name="pattern" /> is hexadecimal encoded at the specified location; otherwise, false.</returns>
		/// <param name="pattern">The string to check. </param>
		/// <param name="index">The location in <paramref name="pattern" /> to check for hexadecimal encoding. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsHexEncoding(string pattern, int index)
		{
			if (index + 3 > pattern.Length)
			{
				return false;
			}
			return pattern[index++] == '%' && IsHexDigit(pattern[index++]) && IsHexDigit(pattern[index]);
		}

		/// <summary>Determines the difference between two <see cref="T:System.Uri" /> instances.</summary>
		/// <returns>If the hostname and scheme of this URI instance and <paramref name="toUri" /> are the same, then this method returns a relative <see cref="T:System.Uri" /> that, when appended to the current URI instance, yields <paramref name="toUri" />.If the hostname or scheme is different, then this method returns a <see cref="T:System.Uri" />  that represents the <paramref name="toUri" /> parameter.</returns>
		/// <param name="uri">The URI to compare to the current URI.</param>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs. </exception>
		public Uri MakeRelativeUri(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (Host != uri.Host || Scheme != uri.Scheme)
			{
				return uri;
			}
			string result = string.Empty;
			if (path != uri.path)
			{
				string[] array = Segments;
				string[] array2 = uri.Segments;
				int i = 0;
				for (int num = Math.Min(array.Length, array2.Length); i < num && !(array[i] != array2[i]); i++)
				{
				}
				for (int j = i + 1; j < array.Length; j++)
				{
					result += "../";
				}
				for (int k = i; k < array2.Length; k++)
				{
					result += array2[k];
				}
			}
			uri.AppendQueryAndFragment(ref result);
			return new Uri(result, UriKind.Relative);
		}

		/// <summary>Determines the difference between two <see cref="T:System.Uri" /> instances.</summary>
		/// <returns>If the hostname and scheme of this URI instance and <paramref name="toUri" /> are the same, then this method returns a <see cref="T:System.String" /> that represents a relative URI that, when appended to the current URI instance, yields the <paramref name="toUri" /> parameter.If the hostname or scheme is different, then this method returns a <see cref="T:System.String" /> that represents the <paramref name="toUri" /> parameter.</returns>
		/// <param name="toUri">The URI to compare to the current URI. </param>
		/// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this method is valid only for absolute URIs. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("Use MakeRelativeUri(Uri uri) instead.")]
		public string MakeRelative(Uri toUri)
		{
			if (Scheme != toUri.Scheme || Authority != toUri.Authority)
			{
				return toUri.ToString();
			}
			string text = string.Empty;
			if (path != toUri.path)
			{
				string[] array = Segments;
				string[] array2 = toUri.Segments;
				int i = 0;
				for (int num = Math.Min(array.Length, array2.Length); i < num && !(array[i] != array2[i]); i++)
				{
				}
				for (int j = i + 1; j < array.Length; j++)
				{
					text += "../";
				}
				for (int k = i; k < array2.Length; k++)
				{
					text += array2[k];
				}
			}
			return text;
		}

		private void AppendQueryAndFragment(ref string result)
		{
			if (query.Length > 0)
			{
				string text = (query[0] != '?') ? Unescape(query, excludeSpecial: false) : ('?' + Unescape(query.Substring(1), excludeSpecial: false));
				result += text;
			}
			if (fragment.Length > 0)
			{
				result += fragment;
			}
		}

		/// <summary>Gets a canonical string representation for the specified <see cref="T:System.Uri" /> instance.</summary>
		/// <returns>A <see cref="T:System.String" /> instance that contains the unescaped canonical representation of the <see cref="T:System.Uri" /> instance. All characters are unescaped except #, ?, and %.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override string ToString()
		{
			if (cachedToString != null)
			{
				return cachedToString;
			}
			if (isAbsoluteUri)
			{
				cachedToString = Unescape(GetLeftPart(UriPartial.Path), excludeSpecial: true);
			}
			else
			{
				cachedToString = Unescape(path);
			}
			AppendQueryAndFragment(ref cachedToString);
			return cachedToString;
		}

		/// <summary>Returns the data needed to serialize the current instance.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Uri" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Uri" />.</param>
		protected void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AbsoluteUri", AbsoluteUri);
		}

		/// <summary>Converts any unsafe or reserved characters in the path component to their hexadecimal character representations.</summary>
		/// <exception cref="T:System.UriFormatException">The URI passed from the constructor is invalid. This exception can occur if a URI has too many characters or the URI is relative.</exception>
		[Obsolete]
		protected virtual void Escape()
		{
			path = EscapeString(path);
		}

		/// <summary>Converts a string to its escaped representation.</summary>
		/// <returns>The escaped representation of the string.</returns>
		/// <param name="str">The string to transform to its escaped representation. </param>
		[Obsolete]
		protected static string EscapeString(string str)
		{
			return EscapeString(str, escapeReserved: false, escapeHex: true, escapeBrackets: true);
		}

		internal static string EscapeString(string str, bool escapeReserved, bool escapeHex, bool escapeBrackets)
		{
			if (str == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				if (IsHexEncoding(str, i))
				{
					stringBuilder.Append(str.Substring(i, 3));
					i += 2;
					continue;
				}
				byte[] bytes = Encoding.UTF8.GetBytes(new char[1]
				{
					str[i]
				});
				int num = bytes.Length;
				for (int j = 0; j < num; j++)
				{
					char c = (char)bytes[j];
					if (c <= ' ' || c >= '\u007f' || "<>%\"{}|\\^`".IndexOf(c) != -1 || (escapeHex && c == '#') || (escapeBrackets && (c == '[' || c == ']')) || (escapeReserved && ";/?:@&=+$,".IndexOf(c) != -1))
					{
						stringBuilder.Append(HexEscape(c));
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>Parses the URI of the current instance to ensure it contains all the parts required for a valid URI.</summary>
		/// <exception cref="T:System.UriFormatException">The Uri passed from the constructor is invalid. </exception>
		[Obsolete("The method has been deprecated. It is not used by the system.")]
		protected virtual void Parse()
		{
		}

		private void ParseUri(UriKind kind)
		{
			Parse(kind, source);
			if (!userEscaped)
			{
				host = EscapeString(host, escapeReserved: false, escapeHex: true, escapeBrackets: false);
				if (host.Length > 1 && host[0] != '[' && host[host.Length - 1] != ']')
				{
					host = host.ToLower(CultureInfo.InvariantCulture);
				}
				if (path.Length > 0)
				{
					path = EscapeString(path);
				}
			}
		}

		/// <summary>Converts the specified string by replacing any escape sequences with their unescaped representation.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the unescaped value of the <paramref name="path" /> parameter.</returns>
		/// <param name="path">The <see cref="T:System.String" /> to convert. </param>
		[Obsolete]
		protected virtual string Unescape(string str)
		{
			return Unescape(str, excludeSpecial: false);
		}

		internal static string Unescape(string str, bool excludeSpecial)
		{
			if (str == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				if (c == '%')
				{
					char surrogate;
					char c2 = HexUnescapeMultiByte(str, ref i, out surrogate);
					if (excludeSpecial && c2 == '#')
					{
						stringBuilder.Append("%23");
					}
					else if (excludeSpecial && c2 == '%')
					{
						stringBuilder.Append("%25");
					}
					else if (excludeSpecial && c2 == '?')
					{
						stringBuilder.Append("%3F");
					}
					else
					{
						stringBuilder.Append(c2);
						if (surrogate != 0)
						{
							stringBuilder.Append(surrogate);
						}
					}
					i--;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private void ParseAsWindowsUNC(string uriString)
		{
			scheme = UriSchemeFile;
			port = -1;
			fragment = string.Empty;
			query = string.Empty;
			isUnc = true;
			uriString = uriString.TrimStart('\\');
			int num = uriString.IndexOf('\\');
			if (num > 0)
			{
				path = uriString.Substring(num);
				host = uriString.Substring(0, num);
			}
			else
			{
				host = uriString;
				path = string.Empty;
			}
			path = path.Replace("\\", "/");
		}

		private string ParseAsWindowsAbsoluteFilePath(string uriString)
		{
			if (uriString.Length > 2 && uriString[2] != '\\' && uriString[2] != '/')
			{
				return "Relative file path is not allowed.";
			}
			scheme = UriSchemeFile;
			host = string.Empty;
			port = -1;
			path = uriString.Replace("\\", "/");
			fragment = string.Empty;
			query = string.Empty;
			return null;
		}

		private void ParseAsUnixAbsoluteFilePath(string uriString)
		{
			isUnixFilePath = true;
			scheme = UriSchemeFile;
			port = -1;
			fragment = string.Empty;
			query = string.Empty;
			host = string.Empty;
			path = null;
			if (uriString.Length >= 2 && uriString[0] == '/' && uriString[1] == '/')
			{
				uriString = uriString.TrimStart('/');
				path = '/' + uriString;
			}
			if (path == null)
			{
				path = uriString;
			}
		}

		private void Parse(UriKind kind, string uriString)
		{
			if (uriString == null)
			{
				throw new ArgumentNullException("uriString");
			}
			string text = ParseNoExceptions(kind, uriString);
			if (text != null)
			{
				throw new UriFormatException(text);
			}
		}

		private string ParseNoExceptions(UriKind kind, string uriString)
		{
			uriString = uriString.Trim();
			int length = uriString.Length;
			if (length == 0 && (kind == UriKind.Relative || kind == UriKind.RelativeOrAbsolute))
			{
				isAbsoluteUri = false;
				return null;
			}
			if (length <= 1 && kind != UriKind.Relative)
			{
				return "Absolute URI is too short";
			}
			int num = 0;
			num = uriString.IndexOf(':');
			if (num == 0)
			{
				return "Invalid URI: The format of the URI could not be determined.";
			}
			if (num < 0)
			{
				if (uriString[0] == '/' && Path.DirectorySeparatorChar == '/')
				{
					ParseAsUnixAbsoluteFilePath(uriString);
					if (kind == UriKind.Relative)
					{
						isAbsoluteUri = false;
					}
				}
				else if (uriString.Length >= 2 && uriString[0] == '\\' && uriString[1] == '\\')
				{
					ParseAsWindowsUNC(uriString);
				}
				else
				{
					isAbsoluteUri = false;
					path = uriString;
				}
				return null;
			}
			if (num == 1)
			{
				if (!IsAlpha(uriString[0]))
				{
					return "URI scheme must start with a letter.";
				}
				string text = ParseAsWindowsAbsoluteFilePath(uriString);
				if (text != null)
				{
					return text;
				}
				return null;
			}
			scheme = uriString.Substring(0, num).ToLower(CultureInfo.InvariantCulture);
			if (!CheckSchemeName(scheme))
			{
				return Locale.GetText("URI scheme must start with a letter and must consist of one of alphabet, digits, '+', '-' or '.' character.");
			}
			int i = num + 1;
			int num2 = uriString.Length;
			num = uriString.IndexOf('#', i);
			if (!IsUnc && num != -1)
			{
				if (userEscaped)
				{
					fragment = uriString.Substring(num);
				}
				else
				{
					fragment = "#" + EscapeString(uriString.Substring(num + 1));
				}
				num2 = num;
			}
			num = uriString.IndexOf('?', i, num2 - i);
			if (num != -1)
			{
				query = uriString.Substring(num, num2 - num);
				num2 = num;
				if (!userEscaped)
				{
					query = EscapeString(query);
				}
			}
			if (IsPredefinedScheme(scheme) && scheme != UriSchemeMailto && scheme != UriSchemeNews && (num2 - i < 2 || (num2 - i >= 2 && uriString[i] == '/' && uriString[i + 1] != '/')))
			{
				return "Invalid URI: The Authority/Host could not be parsed.";
			}
			bool flag = num2 - i >= 2 && uriString[i] == '/' && uriString[i + 1] == '/';
			bool flag2 = scheme == UriSchemeFile && flag && (num2 - i == 2 || uriString[i + 2] == '/');
			bool flag3 = false;
			if (flag)
			{
				if (kind == UriKind.Relative)
				{
					return "Absolute URI when we expected a relative one";
				}
				if (scheme != UriSchemeMailto && scheme != UriSchemeNews)
				{
					i += 2;
				}
				if (scheme == UriSchemeFile)
				{
					int num3 = 2;
					for (int j = i; j < num2 && uriString[j] == '/'; j++)
					{
						num3++;
					}
					if (num3 >= 4)
					{
						flag2 = false;
						for (; i < num2 && uriString[i] == '/'; i++)
						{
						}
					}
					else if (num3 >= 3)
					{
						i++;
					}
				}
				if (num2 - i > 1 && uriString[i + 1] == ':')
				{
					flag2 = false;
					flag3 = true;
				}
			}
			else if (!IsPredefinedScheme(scheme))
			{
				path = uriString.Substring(i, num2 - i);
				isOpaquePart = true;
				return null;
			}
			if (flag2)
			{
				num = -1;
			}
			else
			{
				num = uriString.IndexOf('/', i, num2 - i);
				if (num == -1 && flag3)
				{
					num = uriString.IndexOf('\\', i, num2 - i);
				}
			}
			if (num == -1)
			{
				if (scheme != UriSchemeMailto && scheme != UriSchemeNews)
				{
					path = "/";
				}
			}
			else
			{
				path = uriString.Substring(num, num2 - num);
				num2 = num;
			}
			num = ((!flag2) ? uriString.IndexOf('@', i, num2 - i) : (-1));
			if (num != -1)
			{
				userinfo = uriString.Substring(i, num - i);
				i = num + 1;
			}
			port = -1;
			num = ((!flag2) ? uriString.LastIndexOf(':', num2 - 1, num2 - i) : (-1));
			if (num != -1 && num != num2 - 1)
			{
				string text2 = uriString.Substring(num + 1, num2 - (num + 1));
				if (text2.Length > 0 && text2[text2.Length - 1] != ']')
				{
					if (!int.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out port) || port < 0 || port > 65535)
					{
						return "Invalid URI: Invalid port number";
					}
					num2 = num;
				}
				else if (port == -1)
				{
					port = GetDefaultPort(scheme);
				}
			}
			else if (port == -1)
			{
				port = GetDefaultPort(scheme);
			}
			uriString = uriString.Substring(i, num2 - i);
			host = uriString;
			if (flag2)
			{
				path = Reduce('/' + uriString, compact_escaped: true);
				host = string.Empty;
			}
			else if (host.Length == 2 && host[1] == ':')
			{
				path = host + path;
				host = string.Empty;
			}
			else if (isUnixFilePath)
			{
				uriString = "//" + uriString;
				host = string.Empty;
			}
			else if (scheme == UriSchemeFile)
			{
				isUnc = true;
			}
			else if (scheme == UriSchemeNews)
			{
				if (host.Length > 0)
				{
					path = host;
					host = string.Empty;
				}
			}
			else if (host.Length == 0 && (scheme == UriSchemeHttp || scheme == UriSchemeGopher || scheme == UriSchemeNntp || scheme == UriSchemeHttps || scheme == UriSchemeFtp))
			{
				return "Invalid URI: The hostname could not be parsed";
			}
			bool flag4 = host.Length > 0 && CheckHostName(host) == UriHostNameType.Unknown;
			if (!flag4 && host.Length > 1 && host[0] == '[' && host[host.Length - 1] == ']')
			{
				if (System.Net.IPv6Address.TryParse(host, out System.Net.IPv6Address result))
				{
					host = "[" + result.ToString(fullLength: true) + "]";
				}
				else
				{
					flag4 = true;
				}
			}
			if (flag4 && (Parser is DefaultUriParser || Parser == null))
			{
				return Locale.GetText("Invalid URI: The hostname could not be parsed. (" + host + ")");
			}
			UriFormatException parsingError = null;
			if (Parser != null)
			{
				Parser.InitializeAndValidate(this, out parsingError);
			}
			if (parsingError != null)
			{
				return parsingError.Message;
			}
			if (scheme != UriSchemeMailto && scheme != UriSchemeNews && scheme != UriSchemeFile)
			{
				path = Reduce(path, CompactEscaped(scheme));
			}
			return null;
		}

		private static bool CompactEscaped(string scheme)
		{
			switch (scheme)
			{
			case "file":
			case "http":
			case "https":
			case "net.pipe":
			case "net.tcp":
				return true;
			default:
				return false;
			}
		}

		private static string Reduce(string path, bool compact_escaped)
		{
			if (path == "/")
			{
				return path;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (compact_escaped)
			{
				for (int i = 0; i < path.Length; i++)
				{
					char c = path[i];
					switch (c)
					{
					case '\\':
						stringBuilder.Append('/');
						break;
					case '%':
						if (i < path.Length - 2)
						{
							char c2 = path[i + 1];
							char c3 = char.ToUpper(path[i + 2]);
							if ((c2 == '2' && c3 == 'F') || (c2 == '5' && c3 == 'C'))
							{
								stringBuilder.Append('/');
								i += 2;
							}
							else
							{
								stringBuilder.Append(c);
							}
						}
						else
						{
							stringBuilder.Append(c);
						}
						break;
					default:
						stringBuilder.Append(c);
						break;
					}
				}
				path = stringBuilder.ToString();
			}
			else
			{
				path = path.Replace('\\', '/');
			}
			ArrayList arrayList = new ArrayList();
			int num = 0;
			while (num < path.Length)
			{
				int num2 = path.IndexOf('/', num);
				if (num2 == -1)
				{
					num2 = path.Length;
				}
				string text = path.Substring(num, num2 - num);
				num = num2 + 1;
				if (text.Length == 0 || text == ".")
				{
					continue;
				}
				if (text == "..")
				{
					int count = arrayList.Count;
					if (count != 0)
					{
						arrayList.RemoveAt(count - 1);
					}
				}
				else
				{
					arrayList.Add(text);
				}
			}
			if (arrayList.Count == 0)
			{
				return "/";
			}
			stringBuilder.Length = 0;
			if (path[0] == '/')
			{
				stringBuilder.Append('/');
			}
			bool flag = true;
			foreach (string item in arrayList)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append('/');
				}
				stringBuilder.Append(item);
			}
			if (path.EndsWith("/"))
			{
				stringBuilder.Append('/');
			}
			return stringBuilder.ToString();
		}

		private static char HexUnescapeMultiByte(string pattern, ref int index, out char surrogate)
		{
			surrogate = '\0';
			if (pattern == null)
			{
				throw new ArgumentException("pattern");
			}
			if (index < 0 || index >= pattern.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (!IsHexEncoding(pattern, index))
			{
				return pattern[index++];
			}
			int num = index++;
			int num2 = FromHex(pattern[index++]);
			int num3 = FromHex(pattern[index++]);
			int num4 = num2;
			int num5 = 0;
			while ((num4 & 8) == 8)
			{
				num5++;
				num4 <<= 1;
			}
			if (num5 <= 1)
			{
				return (char)((num2 << 4) | num3);
			}
			byte[] array = new byte[num5];
			bool flag = false;
			array[0] = (byte)((num2 << 4) | num3);
			for (int i = 1; i < num5; i++)
			{
				if (!IsHexEncoding(pattern, index++))
				{
					flag = true;
					break;
				}
				int num6 = FromHex(pattern[index++]);
				if ((num6 & 0xC) != 8)
				{
					flag = true;
					break;
				}
				int num7 = FromHex(pattern[index++]);
				array[i] = (byte)((num6 << 4) | num7);
			}
			if (flag)
			{
				index = num + 3;
				return (char)array[0];
			}
			byte b = byte.MaxValue;
			b = (byte)(b >> num5 + 1);
			int num8 = array[0] & b;
			for (int j = 1; j < num5; j++)
			{
				num8 <<= 6;
				num8 |= (array[j] & 0x3F);
			}
			if (num8 <= 65535)
			{
				return (char)num8;
			}
			num8 -= 65536;
			surrogate = (char)((num8 & 0x3FF) | 0xDC00);
			return (char)((num8 >> 10) | 0xD800);
		}

		internal static string GetSchemeDelimiter(string scheme)
		{
			for (int i = 0; i < schemes.Length; i++)
			{
				if (schemes[i].scheme == scheme)
				{
					return schemes[i].delimiter;
				}
			}
			return SchemeDelimiter;
		}

		internal static int GetDefaultPort(string scheme)
		{
			return UriParser.GetParser(scheme)?.DefaultPort ?? (-1);
		}

		private string GetOpaqueWiseSchemeDelimiter()
		{
			if (isOpaquePart)
			{
				return ":";
			}
			return GetSchemeDelimiter(scheme);
		}

		/// <summary>Gets whether a character is invalid in a file system name.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the specified character is invalid; otherwise false.</returns>
		/// <param name="character">The <see cref="T:System.Char" /> to test. </param>
		[Obsolete]
		protected virtual bool IsBadFileSystemCharacter(char ch)
		{
			if (ch < ' ' || (ch < '@' && ch > '9'))
			{
				return true;
			}
			switch (ch)
			{
			case '\0':
			case '"':
			case '&':
			case '*':
			case ',':
			case '/':
			case '\\':
			case '^':
			case '|':
				return true;
			default:
				return false;
			}
		}

		/// <summary>Gets whether the specified character should be escaped.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the specified character should be escaped; otherwise, false.</returns>
		/// <param name="character">The <see cref="T:System.Char" /> to test. </param>
		[Obsolete]
		protected static bool IsExcludedCharacter(char ch)
		{
			switch (ch)
			{
			default:
				return true;
			case '"':
			case '#':
			case '%':
			case '<':
			case '>':
			case '[':
			case '\\':
			case ']':
			case '^':
			case '`':
			case '{':
			case '|':
			case '}':
				return true;
			case '!':
			case '$':
			case '&':
			case '\'':
			case '(':
			case ')':
			case '*':
			case '+':
			case ',':
			case '-':
			case '.':
			case '/':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
			case ':':
			case ';':
			case '=':
			case '?':
			case '@':
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
			case 'G':
			case 'H':
			case 'I':
			case 'J':
			case 'K':
			case 'L':
			case 'M':
			case 'N':
			case 'O':
			case 'P':
			case 'Q':
			case 'R':
			case 'S':
			case 'T':
			case 'U':
			case 'V':
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
			case '_':
			case 'a':
			case 'b':
			case 'c':
			case 'd':
			case 'e':
			case 'f':
			case 'g':
			case 'h':
			case 'i':
			case 'j':
			case 'k':
			case 'l':
			case 'm':
			case 'n':
			case 'o':
			case 'p':
			case 'q':
			case 'r':
			case 's':
			case 't':
			case 'u':
			case 'v':
			case 'w':
			case 'x':
			case 'y':
			case 'z':
			case '~':
				return false;
			}
		}

		internal static bool MaybeUri(string s)
		{
			int num = s.IndexOf(':');
			if (num == -1)
			{
				return false;
			}
			if (num >= 10)
			{
				return false;
			}
			return IsPredefinedScheme(s.Substring(0, num));
		}

		private static bool IsPredefinedScheme(string scheme)
		{
			switch (scheme)
			{
			case "http":
			case "https":
			case "file":
			case "ftp":
			case "nntp":
			case "gopher":
			case "mailto":
			case "news":
			case "net.pipe":
			case "net.tcp":
				return true;
			default:
				return false;
			}
		}

		/// <summary>Gets whether the specified character is a reserved character.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the specified character is a reserved character otherwise, false.</returns>
		/// <param name="character">The <see cref="T:System.Char" /> to test. </param>
		[Obsolete]
		protected virtual bool IsReservedCharacter(char ch)
		{
			if (ch == '$' || ch == '&' || ch == '+' || ch == ',' || ch == '/' || ch == ':' || ch == ';' || ch == '=' || ch == '@')
			{
				return true;
			}
			return false;
		}

		/// <summary>Gets the specified components of the current instance using the specified escaping for special characters.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the components.</returns>
		/// <param name="components">A bitwise combination of the <see cref="T:System.UriComponents" /> values that specifies which parts of the current instance to return to the caller.</param>
		/// <param name="format">One of the <see cref="T:System.UriFormat" /> values that controls how special characters are escaped. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="uriComponents" /> is not a combination of valid <see cref="T:System.UriComponents" /> values.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Uri" /> is not an absolute URI. Relative URIs cannot be used with this method.</exception>
		/// <filterpriority>1</filterpriority>
		public string GetComponents(UriComponents components, UriFormat format)
		{
			return Parser.GetComponents(this, components, format);
		}

		/// <summary>Determines whether the current <see cref="T:System.Uri" /> instance is a base of the specified <see cref="T:System.Uri" /> instance.</summary>
		/// <returns>true if the current <see cref="T:System.Uri" /> instance is a base of <paramref name="uri" />; otherwise, false.</returns>
		/// <param name="uri">The specified <see cref="T:System.Uri" /> instance to test. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool IsBaseOf(Uri uri)
		{
			return Parser.IsBaseOf(this, uri);
		}

		/// <summary>Indicates whether the string used to construct this <see cref="T:System.Uri" /> was well-formed and is not required to be further escaped.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the string was well-formed; else false.</returns>
		public bool IsWellFormedOriginalString()
		{
			return EscapeString(OriginalString) == OriginalString;
		}

		/// <summary>Compares the specified parts of two URIs using the specified comparison rules.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that indicates the lexical relationship between the compared <see cref="T:System.Uri" /> components.ValueMeaningLess than zero<paramref name="uri1" /> is less than <paramref name="uri2" />.Zero<paramref name="uri1" /> equals <paramref name="uri2" />.Greater than zero<paramref name="uri1" /> is greater than <paramref name="uri2" />.</returns>
		/// <param name="uri1">The first <see cref="T:System.Uri" />.</param>
		/// <param name="uri2">The second <see cref="T:System.Uri" />.</param>
		/// <param name="partsToCompare">A bitwise combination of the <see cref="T:System.UriComponents" /> values that specifies the parts of <paramref name="uri1" /> and <paramref name="uri2" /> to compare.</param>
		/// <param name="compareFormat">One of the <see cref="T:System.UriFormat" /> values that specifies the character escaping used when the URI components are compared.</param>
		/// <param name="comparisonType">One of the <see cref="T:System.StringComparison" /> values.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Compare(Uri uri1, Uri uri2, UriComponents partsToCompare, UriFormat compareFormat, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				string text = Locale.GetText("Invalid StringComparison value '{0}'", comparisonType);
				throw new ArgumentException("comparisonType", text);
			}
			if (uri1 == null && uri2 == null)
			{
				return 0;
			}
			string components = uri1.GetComponents(partsToCompare, compareFormat);
			string components2 = uri2.GetComponents(partsToCompare, compareFormat);
			return string.Compare(components, components2, comparisonType);
		}

		private static bool NeedToEscapeDataChar(char b)
		{
			return (b < 'A' || b > 'Z') && (b < 'a' || b > 'z') && (b < '0' || b > '9') && b != '_' && b != '~' && b != '!' && b != '\'' && b != '(' && b != ')' && b != '*' && b != '-' && b != '.';
		}

		/// <summary>Converts a string to its escaped representation.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the escaped representation of <paramref name="stringToEscape" />.</returns>
		/// <param name="stringToEscape">The string to escape.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stringToEscape" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">The length of <paramref name="stringToEscape" /> exceeds 32766 characters.</exception>
		public static string EscapeDataString(string stringToEscape)
		{
			if (stringToEscape == null)
			{
				throw new ArgumentNullException("stringToEscape");
			}
			if (stringToEscape.Length > 32766)
			{
				string text = Locale.GetText("Uri is longer than the maximum {0} characters.");
				throw new UriFormatException(text);
			}
			bool flag = false;
			foreach (char b in stringToEscape)
			{
				if (NeedToEscapeDataChar(b))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return stringToEscape;
			}
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = Encoding.UTF8.GetBytes(stringToEscape);
			byte[] array = bytes;
			foreach (byte b2 in array)
			{
				if (NeedToEscapeDataChar((char)b2))
				{
					stringBuilder.Append(HexEscape((char)b2));
				}
				else
				{
					stringBuilder.Append((char)b2);
				}
			}
			return stringBuilder.ToString();
		}

		private static bool NeedToEscapeUriChar(char b)
		{
			return (b < 'A' || b > 'Z') && (b < 'a' || b > 'z') && (b < '&' || b > ';') && b != '!' && b != '#' && b != '$' && b != '=' && b != '?' && b != '@' && b != '_' && b != '~';
		}

		/// <summary>Converts a URI string to its escaped representation.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the escaped representation of <paramref name="stringToEscape" />.</returns>
		/// <param name="stringToEscape">The string to escape.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stringToEscape" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">The length of <paramref name="stringToEscape" /> exceeds 32766 characters.</exception>
		public static string EscapeUriString(string stringToEscape)
		{
			if (stringToEscape == null)
			{
				throw new ArgumentNullException("stringToEscape");
			}
			if (stringToEscape.Length > 32766)
			{
				string text = Locale.GetText("Uri is longer than the maximum {0} characters.");
				throw new UriFormatException(text);
			}
			bool flag = false;
			foreach (char b in stringToEscape)
			{
				if (NeedToEscapeUriChar(b))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return stringToEscape;
			}
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = Encoding.UTF8.GetBytes(stringToEscape);
			byte[] array = bytes;
			foreach (byte b2 in array)
			{
				if (NeedToEscapeUriChar((char)b2))
				{
					stringBuilder.Append(HexEscape((char)b2));
				}
				else
				{
					stringBuilder.Append((char)b2);
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>Indicates whether the string is well-formed by attempting to construct a URI with the string and ensures that the string does not require further escaping.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the string was well-formed; else false.</returns>
		/// <param name="uriString">The string used to attempt to construct a <see cref="T:System.Uri" />.</param>
		/// <param name="uriKind">The type of the <see cref="T:System.Uri" /> in <paramref name="uriString" />.</param>
		public static bool IsWellFormedUriString(string uriString, UriKind uriKind)
		{
			if (uriString == null)
			{
				return false;
			}
			if (TryCreate(uriString, uriKind, out Uri result))
			{
				return result.IsWellFormedOriginalString();
			}
			return false;
		}

		/// <summary>Creates a new <see cref="T:System.Uri" /> using the specified <see cref="T:System.String" /> instance and a <see cref="T:System.UriKind" />.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> was successfully created; otherwise, false.</returns>
		/// <param name="uriString">The <see cref="T:System.String" /> representing the <see cref="T:System.Uri" />.</param>
		/// <param name="uriKind">The type of the Uri.</param>
		/// <param name="result">When this method returns, contains the constructed <see cref="T:System.Uri" />.</param>
		public static bool TryCreate(string uriString, UriKind uriKind, out Uri result)
		{
			bool success;
			Uri uri = new Uri(uriString, uriKind, out success);
			if (success)
			{
				result = uri;
				return true;
			}
			result = null;
			return false;
		}

		/// <summary>Creates a new <see cref="T:System.Uri" /> using the specified base and relative <see cref="T:System.String" /> instances.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> was successfully created; otherwise, false.</returns>
		/// <param name="baseUri">The base <see cref="T:System.Uri" />.</param>
		/// <param name="relativeUri">The relative <see cref="T:System.Uri" />, represented as a <see cref="T:System.String" />, to add to the base <see cref="T:System.Uri" />.</param>
		/// <param name="result">When this method returns, contains a <see cref="T:System.Uri" /> constructed from <paramref name="baseUri" /> and <paramref name="relativeUri" />. This parameter is passed uninitialized.</param>
		public static bool TryCreate(Uri baseUri, string relativeUri, out Uri result)
		{
			try
			{
				result = new Uri(baseUri, relativeUri);
				return true;
				IL_0010:
				bool result2;
				return result2;
			}
			catch (UriFormatException)
			{
				result = null;
				return false;
				IL_0020:
				bool result2;
				return result2;
			}
		}

		/// <summary>Creates a new <see cref="T:System.Uri" /> using the specified base and relative <see cref="T:System.Uri" /> instances.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> was successfully created; otherwise, false.</returns>
		/// <param name="baseUri">The base <see cref="T:System.Uri" />. </param>
		/// <param name="relativeUri">The relative <see cref="T:System.Uri" /> to add to the base <see cref="T:System.Uri" />. </param>
		/// <param name="result">When this method returns, contains a <see cref="T:System.Uri" /> constructed from <paramref name="baseUri" /> and <paramref name="relativeUri" />. This parameter is passed uninitialized.</param>
		/// <filterpriority>1</filterpriority>
		public static bool TryCreate(Uri baseUri, Uri relativeUri, out Uri result)
		{
			try
			{
				result = new Uri(baseUri, relativeUri.OriginalString);
				return true;
				IL_0015:
				bool result2;
				return result2;
			}
			catch (UriFormatException)
			{
				result = null;
				return false;
				IL_0025:
				bool result2;
				return result2;
			}
		}

		/// <summary>Converts a string to its unescaped representation.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the unescaped representation of <paramref name="stringToUnescape" />. </returns>
		/// <param name="stringToUnescape">The string to unescape.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stringToUnescape" /> is null. </exception>
		public static string UnescapeDataString(string stringToUnescape)
		{
			if (stringToUnescape == null)
			{
				throw new ArgumentNullException("stringToUnescape");
			}
			if (stringToUnescape.IndexOf('%') == -1 && stringToUnescape.IndexOf('+') == -1)
			{
				return stringToUnescape;
			}
			StringBuilder stringBuilder = new StringBuilder();
			long num = stringToUnescape.Length;
			MemoryStream memoryStream = new MemoryStream();
			for (int i = 0; i < num; i++)
			{
				if (stringToUnescape[i] == '%' && i + 2 < num && stringToUnescape[i + 1] != '%')
				{
					int @char;
					if (stringToUnescape[i + 1] == 'u' && i + 5 < num)
					{
						if (memoryStream.Length > 0)
						{
							stringBuilder.Append(GetChars(memoryStream, Encoding.UTF8));
							memoryStream.SetLength(0L);
						}
						@char = GetChar(stringToUnescape, i + 2, 4);
						if (@char != -1)
						{
							stringBuilder.Append((char)@char);
							i += 5;
						}
						else
						{
							stringBuilder.Append('%');
						}
					}
					else if ((@char = GetChar(stringToUnescape, i + 1, 2)) != -1)
					{
						memoryStream.WriteByte((byte)@char);
						i += 2;
					}
					else
					{
						stringBuilder.Append('%');
					}
				}
				else
				{
					if (memoryStream.Length > 0)
					{
						stringBuilder.Append(GetChars(memoryStream, Encoding.UTF8));
						memoryStream.SetLength(0L);
					}
					stringBuilder.Append(stringToUnescape[i]);
				}
			}
			if (memoryStream.Length > 0)
			{
				stringBuilder.Append(GetChars(memoryStream, Encoding.UTF8));
			}
			memoryStream = null;
			return stringBuilder.ToString();
		}

		private static int GetInt(byte b)
		{
			char c = (char)b;
			if (c >= '0' && c <= '9')
			{
				return c - 48;
			}
			if (c >= 'a' && c <= 'f')
			{
				return c - 97 + 10;
			}
			if (c >= 'A' && c <= 'F')
			{
				return c - 65 + 10;
			}
			return -1;
		}

		private static int GetChar(string str, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				char c = str[i];
				if (c > '\u007f')
				{
					return -1;
				}
				int @int = GetInt((byte)c);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		private static char[] GetChars(MemoryStream b, Encoding e)
		{
			return e.GetChars(b.GetBuffer(), 0, (int)b.Length);
		}

		private void EnsureAbsoluteUri()
		{
			if (!IsAbsoluteUri)
			{
				throw new InvalidOperationException("This operation is not supported for a relative URI.");
			}
		}

		/// <summary>Determines whether two <see cref="T:System.Uri" /> instances have the same value.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the <see cref="T:System.Uri" /> instances are equivalent; otherwise, false.</returns>
		/// <param name="uri1">A <see cref="T:System.Uri" /> instance to compare with <paramref name="uri2" />. </param>
		/// <param name="uri2">A <see cref="T:System.Uri" /> instance to compare with <paramref name="uri1" />. </param>
		/// <filterpriority>3</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static bool operator ==(Uri u1, Uri u2)
		{
			return object.Equals(u1, u2);
		}

		/// <summary>Determines whether two <see cref="T:System.Uri" /> instances do not have the same value.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that is true if the two <see cref="T:System.Uri" /> instances are not equal; otherwise, false. If either parameter is null, this method returns true.</returns>
		/// <param name="uri1">A <see cref="T:System.Uri" /> instance to compare with <paramref name="uri2" />. </param>
		/// <param name="uri2">A <see cref="T:System.Uri" /> instance to compare with <paramref name="uri1" />. </param>
		/// <filterpriority>3</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static bool operator !=(Uri u1, Uri u2)
		{
			return !(u1 == u2);
		}
	}
}
