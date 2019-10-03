using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	/// <summary>Parses a new URI scheme. This is an abstract class.</summary>
	public abstract class UriParser
	{
		private static object lock_object = new object();

		private static Hashtable table;

		internal string scheme_name;

		private int default_port;

		private static readonly Regex uri_regex = new Regex("^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\\?([^#]*))?(#(.*))?");

		private static readonly Regex auth_regex = new Regex("^(([^@]+)@)?(.*?)(:([0-9]+))?$");

		internal string SchemeName
		{
			get
			{
				return scheme_name;
			}
			set
			{
				scheme_name = value;
			}
		}

		internal int DefaultPort
		{
			get
			{
				return default_port;
			}
			set
			{
				default_port = value;
			}
		}

		private static Match ParseAuthority(Group g)
		{
			return auth_regex.Match(g.Value);
		}

		/// <summary>Gets the components from a URI.</summary>
		/// <returns>A string that contains the components.</returns>
		/// <param name="uri">The URI to parse.</param>
		/// <param name="components">The <see cref="T:System.UriComponents" /> to retrieve from <paramref name="uri" />.</param>
		/// <param name="format">One of the <see cref="T:System.UriFormat" /> values that controls how special characters are escaped.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="uriFormat" /> is invalid.- or -<paramref name="uriComponents" /> is not a combination of valid <see cref="T:System.UriComponents" /> values. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="uri" /> requires user-driven parsing- or -<paramref name="uri" /> is not an absolute URI. Relative URIs cannot be used with this method.</exception>
		protected internal virtual string GetComponents(Uri uri, UriComponents components, UriFormat format)
		{
			if (format < UriFormat.UriEscaped || format > UriFormat.SafeUnescaped)
			{
				throw new ArgumentOutOfRangeException("format");
			}
			Match match = uri_regex.Match(uri.OriginalString);
			string value = scheme_name;
			int defaultPort = default_port;
			if (value == null || value == "*")
			{
				value = match.Groups[2].Value;
				defaultPort = Uri.GetDefaultPort(value);
			}
			else if (string.Compare(value, match.Groups[2].Value, ignoreCase: true) != 0)
			{
				throw new SystemException("URI Parser: scheme mismatch: " + value + " vs. " + match.Groups[2].Value);
			}
			switch (components)
			{
			case UriComponents.Scheme:
				return value;
			case UriComponents.UserInfo:
				return ParseAuthority(match.Groups[4]).Groups[2].Value;
			case UriComponents.Host:
				return ParseAuthority(match.Groups[4]).Groups[3].Value;
			case UriComponents.Port:
			{
				string value2 = ParseAuthority(match.Groups[4]).Groups[5].Value;
				if (value2 != null && value2 != string.Empty && value2 != defaultPort.ToString())
				{
					return value2;
				}
				return string.Empty;
			}
			case UriComponents.Path:
				return Format(IgnoreFirstCharIf(match.Groups[5].Value, '/'), format);
			case UriComponents.Query:
				return Format(match.Groups[7].Value, format);
			case UriComponents.Fragment:
				return Format(match.Groups[9].Value, format);
			case UriComponents.StrongPort:
			{
				Group group = ParseAuthority(match.Groups[4]).Groups[5];
				return (!group.Success) ? defaultPort.ToString() : group.Value;
			}
			case UriComponents.SerializationInfoString:
				components = UriComponents.AbsoluteUri;
				break;
			}
			Match match2 = ParseAuthority(match.Groups[4]);
			StringBuilder stringBuilder = new StringBuilder();
			if ((components & UriComponents.Scheme) != 0)
			{
				stringBuilder.Append(value);
				stringBuilder.Append(Uri.GetSchemeDelimiter(value));
			}
			if ((components & UriComponents.UserInfo) != 0)
			{
				stringBuilder.Append(match2.Groups[1].Value);
			}
			if ((components & UriComponents.Host) != 0)
			{
				stringBuilder.Append(match2.Groups[3].Value);
			}
			if ((components & UriComponents.StrongPort) != 0)
			{
				Group group2 = match2.Groups[4];
				stringBuilder.Append((!group2.Success) ? (":" + defaultPort) : group2.Value);
			}
			if ((components & UriComponents.Port) != 0)
			{
				string value3 = match2.Groups[5].Value;
				if (value3 != null && value3 != string.Empty && value3 != defaultPort.ToString())
				{
					stringBuilder.Append(match2.Groups[4].Value);
				}
			}
			if ((components & UriComponents.Path) != 0)
			{
				stringBuilder.Append(match.Groups[5]);
			}
			if ((components & UriComponents.Query) != 0)
			{
				stringBuilder.Append(match.Groups[6]);
			}
			if ((components & UriComponents.Fragment) != 0)
			{
				stringBuilder.Append(match.Groups[8]);
			}
			return Format(stringBuilder.ToString(), format);
		}

		/// <summary>Initialize the state of the parser and validate the URI.</summary>
		/// <param name="uri">The T:System.Uri to validate.</param>
		/// <param name="parsingError">Validation errors, if any.</param>
		protected internal virtual void InitializeAndValidate(Uri uri, out UriFormatException parsingError)
		{
			if (uri.Scheme != scheme_name && scheme_name != "*")
			{
				parsingError = new UriFormatException("The argument Uri's scheme does not match");
			}
			else
			{
				parsingError = null;
			}
		}

		/// <summary>Determines whether <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />.</summary>
		/// <returns>true if <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />; otherwise, false.</returns>
		/// <param name="baseUri">The base URI.</param>
		/// <param name="relativeUri">The URI to test.</param>
		protected internal virtual bool IsBaseOf(Uri baseUri, Uri relativeUri)
		{
			if (Uri.Compare(baseUri, relativeUri, UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port, UriFormat.Unescaped, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				return false;
			}
			string localPath = baseUri.LocalPath;
			int length = localPath.LastIndexOf('/') + 1;
			return string.Compare(localPath, 0, relativeUri.LocalPath, 0, length, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		/// <summary>Indicates whether a URI is well-formed.</summary>
		/// <returns>true if <paramref name="uri" /> is well-formed; otherwise, false.</returns>
		/// <param name="uri">The URI to check.</param>
		protected internal virtual bool IsWellFormedOriginalString(Uri uri)
		{
			return uri.IsWellFormedOriginalString();
		}

		/// <summary>Invoked by a <see cref="T:System.Uri" /> constructor to get a <see cref="T:System.UriParser" /> instance</summary>
		/// <returns>A <see cref="T:System.UriParser" /> for the constructed <see cref="T:System.Uri" />.</returns>
		protected internal virtual UriParser OnNewUri()
		{
			return this;
		}

		/// <summary>Invoked by the Framework when a <see cref="T:System.UriParser" /> method is registered.</summary>
		/// <param name="schemeName">The scheme that is associated with this <see cref="T:System.UriParser" />.</param>
		/// <param name="defaultPort">The port number of the scheme.</param>
		[MonoTODO]
		protected virtual void OnRegister(string schemeName, int defaultPort)
		{
		}

		/// <summary>Called by <see cref="T:System.Uri" /> constructors and <see cref="Overload:System.Uri.TryCreate" /> to resolve a relative URI.</summary>
		/// <returns>The string of the resolved relative <see cref="T:System.Uri" />.</returns>
		/// <param name="baseUri">A base URI.</param>
		/// <param name="relativeUri">A relative URI.</param>
		/// <param name="parsingError">Errors during the resolve process, if any.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="baseUri" /> parameter is not an absolute <see cref="T:System.Uri" />- or -<paramref name="baseUri" /> parameter requires user-driven parsing.</exception>
		[MonoTODO]
		protected internal virtual string Resolve(Uri baseUri, Uri relativeUri, out UriFormatException parsingError)
		{
			throw new NotImplementedException();
		}

		private string IgnoreFirstCharIf(string s, char c)
		{
			if (s.Length == 0)
			{
				return string.Empty;
			}
			if (s[0] == c)
			{
				return s.Substring(1);
			}
			return s;
		}

		private string Format(string s, UriFormat format)
		{
			if (s.Length == 0)
			{
				return string.Empty;
			}
			switch (format)
			{
			case UriFormat.UriEscaped:
				return Uri.EscapeString(s, escapeReserved: false, escapeHex: true, escapeBrackets: true);
			case UriFormat.SafeUnescaped:
				s = Uri.Unescape(s, excludeSpecial: false);
				return s;
			case UriFormat.Unescaped:
				return Uri.Unescape(s, excludeSpecial: false);
			default:
				throw new ArgumentOutOfRangeException("format");
			}
		}

		private static void CreateDefaults()
		{
			if (table == null)
			{
				Hashtable hashtable = new Hashtable();
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeFile, -1);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeFtp, 21);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeGopher, 70);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeHttp, 80);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeHttps, 443);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeMailto, 25);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeNetPipe, -1);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeNetTcp, -1);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeNews, 119);
				InternalRegister(hashtable, new DefaultUriParser(), Uri.UriSchemeNntp, 119);
				InternalRegister(hashtable, new DefaultUriParser(), "ldap", 389);
				lock (lock_object)
				{
					if (table == null)
					{
						table = hashtable;
					}
					else
					{
						hashtable = null;
					}
				}
			}
		}

		/// <summary>Indicates whether the parser for a scheme is registered.</summary>
		/// <returns>true if <paramref name="schemeName" /> has been registered; otherwise, false.</returns>
		/// <param name="schemeName">The scheme name to check.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="schemeName" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="schemeName" /> parameter is not valid. </exception>
		public static bool IsKnownScheme(string schemeName)
		{
			if (schemeName == null)
			{
				throw new ArgumentNullException("schemeName");
			}
			if (schemeName.Length == 0)
			{
				throw new ArgumentOutOfRangeException("schemeName");
			}
			CreateDefaults();
			string key = schemeName.ToLower(CultureInfo.InvariantCulture);
			return table[key] != null;
		}

		private static void InternalRegister(Hashtable table, UriParser uriParser, string schemeName, int defaultPort)
		{
			uriParser.SchemeName = schemeName;
			uriParser.DefaultPort = defaultPort;
			if (uriParser is GenericUriParser)
			{
				table.Add(schemeName, uriParser);
			}
			else
			{
				DefaultUriParser defaultUriParser = new DefaultUriParser();
				defaultUriParser.SchemeName = schemeName;
				defaultUriParser.DefaultPort = defaultPort;
				table.Add(schemeName, defaultUriParser);
			}
			uriParser.OnRegister(schemeName, defaultPort);
		}

		/// <summary>Associates a scheme and port number with a <see cref="T:System.UriParser" />.</summary>
		/// <param name="uriParser">The URI parser to register.</param>
		/// <param name="schemeName">The name of the scheme that is associated with this parser.</param>
		/// <param name="defaultPort">The default port number for the specified scheme.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriParser" /> parameter is null- or -<paramref name="schemeName" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="schemeName" /> parameter is not valid- or -<paramref name="defaultPort" /> parameter is not valid. The <paramref name="defaultPort" /> parameter must be not be less than zero or greater than 65,534.</exception>
		public static void Register(UriParser uriParser, string schemeName, int defaultPort)
		{
			if (uriParser == null)
			{
				throw new ArgumentNullException("uriParser");
			}
			if (schemeName == null)
			{
				throw new ArgumentNullException("schemeName");
			}
			if (defaultPort < -1 || defaultPort >= 65535)
			{
				throw new ArgumentOutOfRangeException("defaultPort");
			}
			CreateDefaults();
			string text = schemeName.ToLower(CultureInfo.InvariantCulture);
			if (table[text] != null)
			{
				string text2 = Locale.GetText("Scheme '{0}' is already registred.");
				throw new InvalidOperationException(text2);
			}
			InternalRegister(table, uriParser, text, defaultPort);
		}

		internal static UriParser GetParser(string schemeName)
		{
			if (schemeName == null)
			{
				return null;
			}
			CreateDefaults();
			string key = schemeName.ToLower(CultureInfo.InvariantCulture);
			return (UriParser)table[key];
		}
	}
}
