using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
	/// <summary>Provides a container for a collection of <see cref="T:System.Net.CookieCollection" /> objects.</summary>
	[Serializable]
	public sealed class CookieContainer
	{
		/// <summary>Represents the default maximum size, in bytes, of the <see cref="T:System.Net.Cookie" /> instances that the <see cref="T:System.Net.CookieContainer" /> can hold. This field is constant.</summary>
		public const int DefaultCookieLengthLimit = 4096;

		/// <summary>Represents the default maximum number of <see cref="T:System.Net.Cookie" /> instances that the <see cref="T:System.Net.CookieContainer" /> can hold. This field is constant.</summary>
		public const int DefaultCookieLimit = 300;

		/// <summary>Represents the default maximum number of <see cref="T:System.Net.Cookie" /> instances that the <see cref="T:System.Net.CookieContainer" /> can reference per domain. This field is constant.</summary>
		public const int DefaultPerDomainCookieLimit = 20;

		private int capacity = 300;

		private int perDomainCapacity = 20;

		private int maxCookieSize = 4096;

		private CookieCollection cookies;

		/// <summary>Gets the number of <see cref="T:System.Net.Cookie" /> instances that a <see cref="T:System.Net.CookieContainer" /> currently holds.</summary>
		/// <returns>The number of <see cref="T:System.Net.Cookie" /> instances that a <see cref="T:System.Net.CookieContainer" /> currently holds. This is the total of <see cref="T:System.Net.Cookie" /> instances in all domains.</returns>
		public int Count => (cookies != null) ? cookies.Count : 0;

		/// <summary>Gets and sets the number of <see cref="T:System.Net.Cookie" /> instances that a <see cref="T:System.Net.CookieContainer" /> can hold.</summary>
		/// <returns>The number of <see cref="T:System.Net.Cookie" /> instances that a <see cref="T:System.Net.CookieContainer" /> can hold. This is a hard limit and cannot be exceeded by adding a <see cref="T:System.Net.Cookie" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="Capacity" /> is less than or equal to zero or (value is less than <see cref="P:System.Net.CookieContainer.PerDomainCapacity" /> and <see cref="P:System.Net.CookieContainer.PerDomainCapacity" /> is not equal to <see cref="F:System.Int32.MaxValue" />). </exception>
		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (value < 0 || (value < perDomainCapacity && perDomainCapacity != int.MaxValue))
				{
					throw new ArgumentOutOfRangeException("value", $"Capacity must be greater than {0} and less than {perDomainCapacity}.");
				}
				capacity = value;
			}
		}

		/// <summary>Represents the maximum allowed length of a <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>The maximum allowed length, in bytes, of a <see cref="T:System.Net.Cookie" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="MaxCookieSize" /> is less than or equal to zero. </exception>
		public int MaxCookieSize
		{
			get
			{
				return maxCookieSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				maxCookieSize = value;
			}
		}

		/// <summary>Gets and sets the number of <see cref="T:System.Net.Cookie" /> instances that a <see cref="T:System.Net.CookieContainer" /> can hold per domain.</summary>
		/// <returns>The number of <see cref="T:System.Net.Cookie" /> instances that are allowed per domain.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="PerDomainCapacity" /> is less than or equal to zero. -or- <paramref name="(PerDomainCapacity" /> is greater than the maximum allowable number of cookies instances, 300, and is not equal to <see cref="F:System.Int32.MaxValue" />). </exception>
		public int PerDomainCapacity
		{
			get
			{
				return perDomainCapacity;
			}
			set
			{
				if (value != int.MaxValue && (value <= 0 || value > capacity))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				perDomainCapacity = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.CookieContainer" /> class.</summary>
		public CookieContainer()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.CookieContainer" /> class with a specified value for the number of <see cref="T:System.Net.Cookie" /> instances that the container can hold.</summary>
		/// <param name="capacity">The number of <see cref="T:System.Net.Cookie" /> instances that the <see cref="T:System.Net.CookieContainer" /> can hold. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="capacity" /> is less than or equal to zero. </exception>
		public CookieContainer(int capacity)
		{
			if (capacity <= 0)
			{
				throw new ArgumentException("Must be greater than zero", "Capacity");
			}
			this.capacity = capacity;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.CookieContainer" /> class with specific properties.</summary>
		/// <param name="capacity">The number of <see cref="T:System.Net.Cookie" /> instances that the <see cref="T:System.Net.CookieContainer" /> can hold. </param>
		/// <param name="perDomainCapacity">The number of <see cref="T:System.Net.Cookie" /> instances per domain. </param>
		/// <param name="maxCookieSize">The maximum size in bytes for any single <see cref="T:System.Net.Cookie" /> in a <see cref="T:System.Net.CookieContainer" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="perDomainCapacity" /> is not equal to <see cref="F:System.Int32.MaxValue" />. and <paramref name="(perDomainCapacity" /> is less than or equal to zero or <paramref name="perDomainCapacity" /> is greater than <paramref name="capacity)" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="maxCookieSize" /> is less than or equal to zero. </exception>
		public CookieContainer(int capacity, int perDomainCapacity, int maxCookieSize)
			: this(capacity)
		{
			if (perDomainCapacity != int.MaxValue && (perDomainCapacity <= 0 || perDomainCapacity > capacity))
			{
				throw new ArgumentOutOfRangeException("perDomainCapacity", $"PerDomainCapacity must be greater than {0} and less than {capacity}.");
			}
			if (maxCookieSize <= 0)
			{
				throw new ArgumentException("Must be greater than zero", "MaxCookieSize");
			}
			this.perDomainCapacity = perDomainCapacity;
			this.maxCookieSize = maxCookieSize;
		}

		/// <summary>Adds a <see cref="T:System.Net.Cookie" /> to a <see cref="T:System.Net.CookieContainer" />. This method uses the domain from the <see cref="T:System.Net.Cookie" /> to determine which domain collection to associate the <see cref="T:System.Net.Cookie" /> with.</summary>
		/// <param name="cookie">The <see cref="T:System.Net.Cookie" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookie" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The domain for <paramref name="cookie" /> is null or the empty string (""). </exception>
		/// <exception cref="T:System.Net.CookieException">
		///   <paramref name="cookie" /> is larger than <paramref name="maxCookieSize" />. -or- the domain for <paramref name="cookie" /> is not a valid URI. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (cookie.Domain.Length == 0)
			{
				throw new ArgumentException("Cookie domain not set.", "cookie.Domain");
			}
			if (cookie.Value.Length > maxCookieSize)
			{
				throw new CookieException("value is larger than MaxCookieSize.");
			}
			Cookie cookie2 = new Cookie(cookie.Name, cookie.Value);
			cookie2.Path = ((cookie.Path.Length != 0) ? cookie.Path : "/");
			cookie2.Domain = cookie.Domain;
			cookie2.ExactDomain = cookie.ExactDomain;
			cookie2.Version = cookie.Version;
			AddCookie(cookie2);
		}

		private void AddCookie(Cookie cookie)
		{
			if (cookies == null)
			{
				cookies = new CookieCollection();
			}
			if (cookies.Count >= capacity)
			{
				RemoveOldest(null);
			}
			if (cookies.Count >= perDomainCapacity && CountDomain(cookie.Domain) >= perDomainCapacity)
			{
				RemoveOldest(cookie.Domain);
			}
			Cookie cookie2 = new Cookie(cookie.Name, cookie.Value);
			cookie2.Path = ((cookie.Path.Length != 0) ? cookie.Path : "/");
			cookie2.Domain = cookie.Domain;
			cookie2.ExactDomain = cookie.ExactDomain;
			cookie2.Version = cookie.Version;
			cookie2.Expires = cookie.Expires;
			cookie2.CommentUri = cookie.CommentUri;
			cookie2.Comment = cookie.Comment;
			cookie2.Discard = cookie.Discard;
			cookie2.HttpOnly = cookie.HttpOnly;
			cookie2.Secure = cookie.Secure;
			cookies.Add(cookie2);
			CheckExpiration();
		}

		private int CountDomain(string domain)
		{
			int num = 0;
			foreach (Cookie cooky in cookies)
			{
				if (CheckDomain(domain, cooky.Domain, exact: true))
				{
					num++;
				}
			}
			return num;
		}

		private void RemoveOldest(string domain)
		{
			int index = 0;
			DateTime t = DateTime.MaxValue;
			for (int i = 0; i < cookies.Count; i++)
			{
				Cookie cookie = cookies[i];
				if (cookie.TimeStamp < t && (domain == null || domain == cookie.Domain))
				{
					t = cookie.TimeStamp;
					index = i;
				}
			}
			cookies.List.RemoveAt(index);
		}

		private void CheckExpiration()
		{
			if (cookies == null)
			{
				return;
			}
			for (int num = cookies.Count - 1; num >= 0; num--)
			{
				Cookie cookie = cookies[num];
				if (cookie.Expired)
				{
					cookies.List.RemoveAt(num);
				}
			}
		}

		/// <summary>Adds the contents of a <see cref="T:System.Net.CookieCollection" /> to the <see cref="T:System.Net.CookieContainer" />.</summary>
		/// <param name="cookies">The <see cref="T:System.Net.CookieCollection" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookies" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (Cookie cooky in cookies)
			{
				Add(cooky);
			}
		}

		private void Cook(Uri uri, Cookie cookie)
		{
			if (IsNullOrEmpty(cookie.Name))
			{
				throw new CookieException("Invalid cookie: name");
			}
			if (cookie.Value == null)
			{
				throw new CookieException("Invalid cookie: value");
			}
			if (uri != null && cookie.Domain.Length == 0)
			{
				cookie.Domain = uri.Host;
			}
			if (cookie.Version == 0 && IsNullOrEmpty(cookie.Path))
			{
				if (uri != null)
				{
					cookie.Path = uri.AbsolutePath;
				}
				else
				{
					cookie.Path = "/";
				}
			}
			if (cookie.Port.Length == 0 && uri != null && !uri.IsDefaultPort)
			{
				cookie.Port = "\"" + uri.Port.ToString() + "\"";
			}
		}

		/// <summary>Adds a <see cref="T:System.Net.Cookie" /> to the <see cref="T:System.Net.CookieContainer" /> for a particular URI.</summary>
		/// <param name="uri">The URI of the <see cref="T:System.Net.Cookie" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <param name="cookie">The <see cref="T:System.Net.Cookie" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null or <paramref name="cookie" /> is null. </exception>
		/// <exception cref="T:System.Net.CookieException">
		///   <paramref name="cookie" /> is larger than <paramref name="maxCookieSize" />. -or- The domain for <paramref name="cookie" /> is not a valid URI. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Add(Uri uri, Cookie cookie)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (!cookie.Expired)
			{
				Cook(uri, cookie);
				AddCookie(cookie);
			}
		}

		/// <summary>Adds the contents of a <see cref="T:System.Net.CookieCollection" /> to the <see cref="T:System.Net.CookieContainer" /> for a particular URI.</summary>
		/// <param name="uri">The URI of the <see cref="T:System.Net.CookieCollection" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <param name="cookies">The <see cref="T:System.Net.CookieCollection" /> to be added to the <see cref="T:System.Net.CookieContainer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookies" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The domain for one of the cookies in <paramref name="cookies" /> is null. </exception>
		/// <exception cref="T:System.Net.CookieException">One of the cookies in <paramref name="cookies" /> contains an invalid domain. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Add(Uri uri, CookieCollection cookies)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (Cookie cooky in cookies)
			{
				if (!cooky.Expired)
				{
					Cook(uri, cooky);
					AddCookie(cooky);
				}
			}
		}

		/// <summary>Gets the HTTP cookie header that contains the HTTP cookies that represent the <see cref="T:System.Net.Cookie" /> instances that are associated with a specific URI.</summary>
		/// <returns>An HTTP cookie header, with strings representing <see cref="T:System.Net.Cookie" /> instances delimited by semicolons.</returns>
		/// <param name="uri">The URI of the <see cref="T:System.Net.Cookie" /> instances desired. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string GetCookieHeader(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			CookieCollection cookieCollection = GetCookies(uri);
			if (cookieCollection.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Cookie item in cookieCollection)
			{
				stringBuilder.Append(item.ToString(uri));
				stringBuilder.Append("; ");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Length -= 2;
			}
			return stringBuilder.ToString();
		}

		private static bool CheckDomain(string domain, string host, bool exact)
		{
			if (domain.Length == 0)
			{
				return false;
			}
			if (exact)
			{
				return string.Compare(host, domain, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
			}
			if (!CultureInfo.InvariantCulture.CompareInfo.IsSuffix(host, domain, CompareOptions.IgnoreCase))
			{
				return false;
			}
			if (domain[0] == '.')
			{
				return true;
			}
			int num = host.Length - domain.Length - 1;
			if (num < 0)
			{
				return false;
			}
			return host[num] == '.';
		}

		/// <summary>Gets a <see cref="T:System.Net.CookieCollection" /> that contains the <see cref="T:System.Net.Cookie" /> instances that are associated with a specific URI.</summary>
		/// <returns>A <see cref="T:System.Net.CookieCollection" /> that contains the <see cref="T:System.Net.Cookie" /> instances that are associated with a specific URI.</returns>
		/// <param name="uri">The URI of the <see cref="T:System.Net.Cookie" /> instances desired. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public CookieCollection GetCookies(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			CheckExpiration();
			CookieCollection cookieCollection = new CookieCollection();
			if (cookies == null)
			{
				return cookieCollection;
			}
			foreach (Cookie cooky in cookies)
			{
				string domain = cooky.Domain;
				if (CheckDomain(domain, uri.Host, cooky.ExactDomain) && (cooky.Port.Length <= 0 || cooky.Ports == null || uri.Port == -1 || Array.IndexOf(cooky.Ports, uri.Port) != -1))
				{
					string path = cooky.Path;
					string absolutePath = uri.AbsolutePath;
					if ((!(path != string.Empty) || !(path != "/") || !(absolutePath != path) || (absolutePath.StartsWith(path) && (path[path.Length - 1] == '/' || absolutePath.Length <= path.Length || absolutePath[path.Length] == '/'))) && (!cooky.Secure || !(uri.Scheme != "https")))
					{
						cookieCollection.Add(cooky);
					}
				}
			}
			cookieCollection.Sort();
			return cookieCollection;
		}

		/// <summary>Adds <see cref="T:System.Net.Cookie" /> instances for one or more cookies from an HTTP cookie header to the <see cref="T:System.Net.CookieContainer" /> for a specific URI.</summary>
		/// <param name="uri">The URI of the <see cref="T:System.Net.CookieCollection" />. </param>
		/// <param name="cookieHeader">The contents of an HTTP set-cookie header as returned by a HTTP server, with <see cref="T:System.Net.Cookie" /> instances delimited by commas. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookieHeader" /> is null. </exception>
		/// <exception cref="T:System.Net.CookieException">One of the cookies is invalid. -or- An error occurred while adding one of the cookies to the container. </exception>
		public void SetCookies(Uri uri, string cookieHeader)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookieHeader == null)
			{
				throw new ArgumentNullException("cookieHeader");
			}
			if (cookieHeader.Length == 0)
			{
				return;
			}
			string[] array = cookieHeader.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (array.Length > i + 1 && Regex.IsMatch(array[i], ".*expires\\s*=\\s*(Mon|Tue|Wed|Thu|Fri|Sat|Sun)", RegexOptions.IgnoreCase) && Regex.IsMatch(array[i + 1], "\\s\\d{2}-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\\d{4} \\d{2}:\\d{2}:\\d{2} GMT", RegexOptions.IgnoreCase))
				{
					text = new StringBuilder(text).Append(",").Append(array[++i]).ToString();
				}
				try
				{
					Cookie cookie = Parse(text);
					if (cookie.Path.Length == 0)
					{
						cookie.Path = uri.AbsolutePath;
					}
					else if (!uri.AbsolutePath.StartsWith(cookie.Path))
					{
						string msg = $"'Path'='{cookie.Path}' is invalid with URI";
						throw new CookieException(msg);
					}
					if (cookie.Domain.Length == 0)
					{
						cookie.Domain = uri.Host;
						cookie.ExactDomain = true;
					}
					AddCookie(cookie);
				}
				catch (Exception e)
				{
					string msg2 = $"Could not parse cookies for '{uri}'.";
					throw new CookieException(msg2, e);
					IL_0146:;
				}
			}
		}

		private static Cookie Parse(string s)
		{
			string[] array = s.Split(';');
			Cookie cookie = new Cookie();
			for (int i = 0; i < array.Length; i++)
			{
				int num = array[i].IndexOf('=');
				string text;
				string text2;
				if (num == -1)
				{
					text = array[i].Trim();
					text2 = string.Empty;
				}
				else
				{
					text = array[i].Substring(0, num).Trim();
					text2 = array[i].Substring(num + 1).Trim();
				}
				switch (text.ToLower(CultureInfo.InvariantCulture))
				{
				case "path":
				case "$path":
					if (cookie.Path.Length == 0)
					{
						cookie.Path = text2;
					}
					break;
				case "domain":
				case "$domain":
					if (cookie.Domain.Length == 0)
					{
						cookie.Domain = text2;
						cookie.ExactDomain = false;
					}
					break;
				case "expires":
				case "$expires":
					if (cookie.Expires == DateTime.MinValue)
					{
						cookie.Expires = DateTime.SpecifyKind(DateTime.ParseExact(text2, "ddd, dd-MMM-yyyy HH:mm:ss G\\MT", CultureInfo.InvariantCulture), DateTimeKind.Utc);
					}
					break;
				case "httponly":
					cookie.HttpOnly = true;
					break;
				case "secure":
					cookie.Secure = true;
					break;
				default:
					if (cookie.Name.Length == 0)
					{
						cookie.Name = text;
						cookie.Value = text2;
					}
					break;
				}
			}
			return cookie;
		}

		private static bool IsNullOrEmpty(string s)
		{
			return s == null || s.Length == 0;
		}
	}
}
