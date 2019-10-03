using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>Provides an HTTP-specific implementation of the <see cref="T:System.Net.WebResponse" /> class.</summary>
	[Serializable]
	public class HttpWebResponse : WebResponse, IDisposable, ISerializable
	{
		private Uri uri;

		private WebHeaderCollection webHeaders;

		private CookieCollection cookieCollection;

		private string method;

		private Version version;

		private HttpStatusCode statusCode;

		private string statusDescription;

		private long contentLength;

		private string contentType;

		private CookieContainer cookie_container;

		private bool disposed;

		private Stream stream;

		private string[] cookieExpiresFormats = new string[3]
		{
			"r",
			"ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'",
			"ddd, dd'-'MMM'-'yy HH':'mm':'ss 'GMT'"
		};

		/// <summary>Gets the character set of the response.</summary>
		/// <returns>A string that contains the character set of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public string CharacterSet
		{
			get
			{
				string text = ContentType;
				if (text == null)
				{
					return "ISO-8859-1";
				}
				string text2 = text.ToLower();
				int num = text2.IndexOf("charset=");
				if (num == -1)
				{
					return "ISO-8859-1";
				}
				num += 8;
				int num2 = text2.IndexOf(';', num);
				return (num2 != -1) ? text.Substring(num, num2 - num) : text.Substring(num);
			}
		}

		/// <summary>Gets the method that is used to encode the body of the response.</summary>
		/// <returns>A string that describes the method that is used to encode the body of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string ContentEncoding
		{
			get
			{
				CheckDisposed();
				string text = webHeaders["Content-Encoding"];
				return (text == null) ? string.Empty : text;
			}
		}

		/// <summary>Gets the length of the content returned by the request.</summary>
		/// <returns>The number of bytes returned by the request. Content length does not include header information.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override long ContentLength => contentLength;

		/// <summary>Gets the content type of the response.</summary>
		/// <returns>A string that contains the content type of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override string ContentType
		{
			get
			{
				CheckDisposed();
				if (contentType == null)
				{
					contentType = webHeaders["Content-Type"];
				}
				return contentType;
			}
		}

		/// <summary>Gets or sets the cookies that are associated with this response.</summary>
		/// <returns>A <see cref="T:System.Net.CookieCollection" /> that contains the cookies that are associated with this response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public CookieCollection Cookies
		{
			get
			{
				CheckDisposed();
				if (cookieCollection == null)
				{
					cookieCollection = new CookieCollection();
				}
				return cookieCollection;
			}
			set
			{
				CheckDisposed();
				cookieCollection = value;
			}
		}

		/// <summary>Gets the headers that are associated with this response from the server.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the header information returned with the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override WebHeaderCollection Headers => webHeaders;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether both client and server were authenticated.</summary>
		/// <returns>true if mutual authentication occurred; otherwise, false.</returns>
		[MonoTODO]
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets the last date and time that the contents of the response were modified.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the date and time that the contents of the response were modified.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime LastModified
		{
			get
			{
				CheckDisposed();
				try
				{
					string dateStr = webHeaders["Last-Modified"];
					return MonoHttpDate.Parse(dateStr);
					IL_0023:
					DateTime result;
					return result;
				}
				catch (Exception)
				{
					return DateTime.Now;
					IL_0034:
					DateTime result;
					return result;
				}
			}
		}

		/// <summary>Gets the method that is used to return the response.</summary>
		/// <returns>A string that contains the HTTP method that is used to return the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string Method
		{
			get
			{
				CheckDisposed();
				return method;
			}
		}

		/// <summary>Gets the version of the HTTP protocol that is used in the response.</summary>
		/// <returns>A <see cref="T:System.Version" /> that contains the HTTP protocol version of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public Version ProtocolVersion
		{
			get
			{
				CheckDisposed();
				return version;
			}
		}

		/// <summary>Gets the URI of the Internet resource that responded to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the Internet resource that responded to the request.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override Uri ResponseUri
		{
			get
			{
				CheckDisposed();
				return uri;
			}
		}

		/// <summary>Gets the name of the server that sent the response.</summary>
		/// <returns>A string that contains the name of the server that sent the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string Server
		{
			get
			{
				CheckDisposed();
				return webHeaders["Server"];
			}
		}

		/// <summary>Gets the status of the response.</summary>
		/// <returns>One of the <see cref="T:System.Net.HttpStatusCode" /> values.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public HttpStatusCode StatusCode => statusCode;

		/// <summary>Gets the status description returned with the response.</summary>
		/// <returns>A string that describes the status of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string StatusDescription
		{
			get
			{
				CheckDisposed();
				return statusDescription;
			}
		}

		internal HttpWebResponse(Uri uri, string method, WebConnectionData data, CookieContainer container)
		{
			this.uri = uri;
			this.method = method;
			webHeaders = data.Headers;
			version = data.Version;
			statusCode = (HttpStatusCode)data.StatusCode;
			statusDescription = data.StatusDescription;
			stream = data.stream;
			contentLength = -1L;
			try
			{
				string text = webHeaders["Content-Length"];
				if (string.IsNullOrEmpty(text) || !long.TryParse(text, out contentLength))
				{
					contentLength = -1L;
				}
			}
			catch (Exception)
			{
				contentLength = -1L;
			}
			if (container != null)
			{
				cookie_container = container;
				FillCookies();
			}
			string a = webHeaders["Content-Encoding"];
			if (a == "gzip" && (data.request.AutomaticDecompression & DecompressionMethods.GZip) != 0)
			{
				stream = new GZipStream(stream, CompressionMode.Decompress);
			}
			else if (a == "deflate" && (data.request.AutomaticDecompression & DecompressionMethods.Deflate) != 0)
			{
				stream = new DeflateStream(stream, CompressionMode.Decompress);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebResponse" /> class from the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> instances.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.HttpWebRequest" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.HttpWebRequest" />. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected HttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			contentLength = serializationInfo.GetInt64("contentLength");
			contentType = serializationInfo.GetString("contentType");
			method = serializationInfo.GetString("method");
			statusDescription = serializationInfo.GetString("statusDescription");
			cookieCollection = (CookieCollection)serializationInfo.GetValue("cookieCollection", typeof(CookieCollection));
			version = (Version)serializationInfo.GetValue("version", typeof(Version));
			statusCode = (HttpStatusCode)(int)serializationInfo.GetValue("statusCode", typeof(HttpStatusCode));
		}

		/// <summary>Serializes this instance into the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</summary>
		/// <param name="serializationInfo">The object into which this <see cref="T:System.Net.HttpWebResponse" /> will be serialized. </param>
		/// <param name="streamingContext">The destination of the serialization. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.HttpWebResponse" />.</summary>
		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets the contents of a header that was returned with the response.</summary>
		/// <returns>The contents of the specified header.</returns>
		/// <param name="headerName">The header value to return. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string GetResponseHeader(string headerName)
		{
			CheckDisposed();
			string text = webHeaders[headerName];
			return (text == null) ? string.Empty : text;
		}

		internal void ReadAll()
		{
			WebConnectionStream webConnectionStream = stream as WebConnectionStream;
			if (webConnectionStream != null)
			{
				try
				{
					webConnectionStream.ReadAll();
				}
				catch
				{
				}
			}
		}

		/// <summary>Gets the stream that is used to read the body of the response from the server.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> containing the body of the response.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">There is no response stream. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream GetResponseStream()
		{
			CheckDisposed();
			if (stream == null)
			{
				return Stream.Null;
			}
			if (string.Compare(method, "HEAD", ignoreCase: true) == 0)
			{
				return Stream.Null;
			}
			return stream;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("uri", uri);
			serializationInfo.AddValue("contentLength", contentLength);
			serializationInfo.AddValue("contentType", contentType);
			serializationInfo.AddValue("method", method);
			serializationInfo.AddValue("statusDescription", statusDescription);
			serializationInfo.AddValue("cookieCollection", cookieCollection);
			serializationInfo.AddValue("version", version);
			serializationInfo.AddValue("statusCode", statusCode);
		}

		/// <summary>Closes the response stream.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Close()
		{
			((IDisposable)this).Dispose();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.HttpWebResponse" />, and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources. </param>
		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
				if (disposing)
				{
					uri = null;
					cookieCollection = null;
					method = null;
					version = null;
					statusDescription = null;
				}
				Stream stream = this.stream;
				this.stream = null;
				stream?.Close();
			}
		}

		private void CheckDisposed()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
		}

		private void FillCookies()
		{
			if (webHeaders == null)
			{
				return;
			}
			string[] values = webHeaders.GetValues("Set-Cookie");
			if (values != null)
			{
				string[] array = values;
				foreach (string cookie in array)
				{
					SetCookie(cookie);
				}
			}
			values = webHeaders.GetValues("Set-Cookie2");
			if (values != null)
			{
				string[] array2 = values;
				foreach (string cookie2 in array2)
				{
					SetCookie2(cookie2);
				}
			}
		}

		private void SetCookie(string header)
		{
			Cookie cookie = null;
			CookieParser cookieParser = new CookieParser(header);
			string name;
			string val;
			while (cookieParser.GetNextNameValue(out name, out val))
			{
				if ((name == null || name == string.Empty) && cookie == null)
				{
					continue;
				}
				if (cookie == null)
				{
					cookie = new Cookie(name, val);
					continue;
				}
				name = name.ToUpper();
				switch (name)
				{
				case "COMMENT":
					if (cookie.Comment == null)
					{
						cookie.Comment = val;
					}
					break;
				case "COMMENTURL":
					if (cookie.CommentUri == null)
					{
						cookie.CommentUri = new Uri(val);
					}
					break;
				case "DISCARD":
					cookie.Discard = true;
					break;
				case "DOMAIN":
					if (cookie.Domain == string.Empty)
					{
						cookie.Domain = val;
					}
					break;
				case "HTTPONLY":
					cookie.HttpOnly = true;
					break;
				case "MAX-AGE":
					if (cookie.Expires == DateTime.MinValue)
					{
						try
						{
							cookie.Expires = cookie.TimeStamp.AddSeconds(uint.Parse(val));
						}
						catch
						{
						}
					}
					break;
				case "EXPIRES":
					if (!(cookie.Expires != DateTime.MinValue))
					{
						cookie.Expires = TryParseCookieExpires(val);
					}
					break;
				case "PATH":
					cookie.Path = val;
					break;
				case "PORT":
					if (cookie.Port == null)
					{
						cookie.Port = val;
					}
					break;
				case "SECURE":
					cookie.Secure = true;
					break;
				case "VERSION":
					try
					{
						cookie.Version = (int)uint.Parse(val);
					}
					catch
					{
					}
					break;
				}
			}
			if (cookie != null)
			{
				if (cookieCollection == null)
				{
					cookieCollection = new CookieCollection();
				}
				if (cookie.Domain == string.Empty)
				{
					cookie.Domain = uri.Host;
				}
				cookieCollection.Add(cookie);
				if (cookie_container != null)
				{
					cookie_container.Add(uri, cookie);
				}
			}
		}

		private void SetCookie2(string cookies_str)
		{
			string[] array = cookies_str.Split(',');
			string[] array2 = array;
			foreach (string cookie in array2)
			{
				SetCookie(cookie);
			}
		}

		private DateTime TryParseCookieExpires(string value)
		{
			if (value == null || value.Length == 0)
			{
				return DateTime.MinValue;
			}
			for (int i = 0; i < cookieExpiresFormats.Length; i++)
			{
				try
				{
					DateTime value2 = DateTime.ParseExact(value, cookieExpiresFormats[i], CultureInfo.InvariantCulture);
					value2 = DateTime.SpecifyKind(value2, DateTimeKind.Utc);
					return TimeZone.CurrentTimeZone.ToLocalTime(value2);
					IL_004b:;
				}
				catch
				{
				}
			}
			return DateTime.MinValue;
		}
	}
}
