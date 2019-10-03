using System.IO;
using System.Net.Cache;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides an HTTP-specific implementation of the <see cref="T:System.Net.WebRequest" /> class.</summary>
	[Serializable]
	public class HttpWebRequest : WebRequest, ISerializable
	{
		private Uri requestUri;

		private Uri actualUri;

		private bool hostChanged;

		private bool allowAutoRedirect = true;

		private bool allowBuffering = true;

		private X509CertificateCollection certificates;

		private string connectionGroup;

		private long contentLength = -1L;

		private HttpContinueDelegate continueDelegate;

		private CookieContainer cookieContainer;

		private ICredentials credentials;

		private bool haveResponse;

		private bool haveRequest;

		private bool requestSent;

		private WebHeaderCollection webHeaders = new WebHeaderCollection(internallyCreated: true);

		private bool keepAlive = true;

		private int maxAutoRedirect = 50;

		private string mediaType = string.Empty;

		private string method = "GET";

		private string initialMethod = "GET";

		private bool pipelined = true;

		private bool preAuthenticate;

		private bool usedPreAuth;

		private Version version = HttpVersion.Version11;

		private Version actualVersion;

		private IWebProxy proxy;

		private bool sendChunked;

		private ServicePoint servicePoint;

		private int timeout = 100000;

		private WebConnectionStream writeStream;

		private HttpWebResponse webResponse;

		private WebAsyncResult asyncWrite;

		private WebAsyncResult asyncRead;

		private EventHandler abortHandler;

		private int aborted;

		private bool gotRequestStream;

		private int redirects;

		private bool expectContinue;

		private bool authCompleted;

		private byte[] bodyBuffer;

		private int bodyBufferLength;

		private bool getResponseCalled;

		private Exception saved_exc;

		private object locker = new object();

		private bool is_ntlm_auth;

		private bool finished_reading;

		internal WebConnection WebConnection;

		private DecompressionMethods auto_decomp;

		private int maxResponseHeadersLength;

		private static int defaultMaxResponseHeadersLength;

		private int readWriteTimeout = 300000;

		private bool unsafe_auth_blah;

		internal bool UsesNtlmAuthentication => is_ntlm_auth;

		/// <summary>Gets or sets the value of the Accept HTTP header.</summary>
		/// <returns>The value of the Accept HTTP header. The default value is null.</returns>
		public string Accept
		{
			get
			{
				return webHeaders["Accept"];
			}
			set
			{
				CheckRequestStarted();
				webHeaders.RemoveAndAdd("Accept", value);
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the Internet resource that actually responds to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that identifies the Internet resource that actually responds to the request. The default is the URI used by the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method to initialize the request.</returns>
		public Uri Address => actualUri;

		/// <summary>Gets or sets a value that indicates whether the request should follow redirection responses.</summary>
		/// <returns>true if the request should automatically follow redirection responses from the Internet resource; otherwise, false. The default value is true.</returns>
		public bool AllowAutoRedirect
		{
			get
			{
				return allowAutoRedirect;
			}
			set
			{
				allowAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to buffer the data sent to the Internet resource.</summary>
		/// <returns>true to enable buffering of the data sent to the Internet resource; false to disable buffering. The default is true.</returns>
		public bool AllowWriteStreamBuffering
		{
			get
			{
				return allowBuffering;
			}
			set
			{
				allowBuffering = value;
			}
		}

		/// <summary>Gets or sets the type of decompression that is used.</summary>
		/// <returns>A T:System.Net.DecompressionMethods object that indicates the type of decompression that is used. </returns>
		/// <exception cref="T:System.InvalidOperationException">The object's current state does not allow this property to be set.</exception>
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return auto_decomp;
			}
			set
			{
				CheckRequestStarted();
				auto_decomp = value;
			}
		}

		internal bool InternalAllowBuffering => allowBuffering && method != "HEAD" && method != "GET" && method != "MKCOL" && method != "CONNECT" && method != "DELETE" && method != "TRACE";

		/// <summary>Gets or sets the collection of security certificates that are associated with this request.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> that contains the security certificates associated with this request.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null. </exception>
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (certificates == null)
				{
					certificates = new X509CertificateCollection();
				}
				return certificates;
			}
			[MonoTODO]
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets the value of the Connection HTTP header.</summary>
		/// <returns>The value of the Connection HTTP header. The default value is null.</returns>
		/// <exception cref="T:System.ArgumentException">The value of <see cref="P:System.Net.HttpWebRequest.Connection" /> is set to Keep-alive or Close. </exception>
		public string Connection
		{
			get
			{
				return webHeaders["Connection"];
			}
			set
			{
				CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					webHeaders.RemoveInternal("Connection");
					return;
				}
				if (text == "keep-alive" || text == "close")
				{
					throw new ArgumentException("Keep-Alive and Close may not be set with this property");
				}
				if (keepAlive && text.IndexOf("keep-alive") == -1)
				{
					value += ", Keep-Alive";
				}
				webHeaders.RemoveAndAdd("Connection", value);
			}
		}

		/// <summary>Gets or sets the name of the connection group for the request.</summary>
		/// <returns>The name of the connection group for this request. The default value is null.</returns>
		public override string ConnectionGroupName
		{
			get
			{
				return connectionGroup;
			}
			set
			{
				connectionGroup = value;
			}
		}

		/// <summary>Gets or sets the Content-length HTTP header.</summary>
		/// <returns>The number of bytes of data to send to the Internet resource. The default is -1, which indicates the property has not been set and that there is no request data to send.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The new <see cref="P:System.Net.HttpWebRequest.ContentLength" /> value is less than 0. </exception>
		public override long ContentLength
		{
			get
			{
				return contentLength;
			}
			set
			{
				CheckRequestStarted();
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", "Content-Length must be >= 0");
				}
				contentLength = value;
			}
		}

		internal long InternalContentLength
		{
			set
			{
				contentLength = value;
			}
		}

		/// <summary>Gets or sets the value of the Content-type HTTP header.</summary>
		/// <returns>The value of the Content-type HTTP header. The default value is null.</returns>
		public override string ContentType
		{
			get
			{
				return webHeaders["Content-Type"];
			}
			set
			{
				if (value == null || value.Trim().Length == 0)
				{
					webHeaders.RemoveInternal("Content-Type");
				}
				else
				{
					webHeaders.RemoveAndAdd("Content-Type", value);
				}
			}
		}

		/// <summary>Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource.</summary>
		/// <returns>A delegate that implements the callback method that executes when an HTTP Continue response is returned from the Internet resource. The default value is null.</returns>
		public HttpContinueDelegate ContinueDelegate
		{
			get
			{
				return continueDelegate;
			}
			set
			{
				continueDelegate = value;
			}
		}

		/// <summary>Gets or sets the cookies associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.CookieContainer" /> that contains the cookies associated with this request.</returns>
		public CookieContainer CookieContainer
		{
			get
			{
				return cookieContainer;
			}
			set
			{
				cookieContainer = value;
			}
		}

		/// <summary>Gets or sets authentication information for the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that contains the authentication credentials associated with the request. The default is null.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override ICredentials Credentials
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

		/// <summary>Gets or sets the default cache policy for this request.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> that specifies the cache policy in effect for this request when no other policy is applicable.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO]
		public new static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets the default maximum length of an HTTP error response.</summary>
		/// <returns>An integer that represents the default maximum length of an HTTP error response.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1. </exception>
		[MonoTODO]
		public static int DefaultMaximumErrorResponseLength
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets the value of the Expect HTTP header.</summary>
		/// <returns>The contents of the Expect HTTP header. The default value is null.Note:The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		/// <exception cref="T:System.ArgumentException">Expect is set to a string that contains "100-continue" as a substring. </exception>
		public string Expect
		{
			get
			{
				return webHeaders["Expect"];
			}
			set
			{
				CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					webHeaders.RemoveInternal("Expect");
					return;
				}
				if (text == "100-continue")
				{
					throw new ArgumentException("100-Continue cannot be set with this property.", "value");
				}
				webHeaders.RemoveAndAdd("Expect", value);
			}
		}

		/// <summary>Gets a value that indicates whether a response has been received from an Internet resource.</summary>
		/// <returns>true if a response has been received; otherwise, false.</returns>
		public bool HaveResponse => haveResponse;

		/// <summary>Specifies a collection of the name/value pairs that make up the HTTP headers.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the name/value pairs that make up the headers for the HTTP request.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override WebHeaderCollection Headers
		{
			get
			{
				return webHeaders;
			}
			set
			{
				CheckRequestStarted();
				WebHeaderCollection webHeaderCollection = new WebHeaderCollection(internallyCreated: true);
				int count = value.Count;
				for (int i = 0; i < count; i++)
				{
					webHeaderCollection.Add(value.GetKey(i), value.Get(i));
				}
				webHeaders = webHeaderCollection;
			}
		}

		/// <summary>Gets or sets the value of the If-Modified-Since HTTP header.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the contents of the If-Modified-Since HTTP header. The default value is the current date and time.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime IfModifiedSince
		{
			get
			{
				string text = webHeaders["If-Modified-Since"];
				if (text == null)
				{
					return DateTime.Now;
				}
				try
				{
					return MonoHttpDate.Parse(text);
					IL_0029:
					DateTime result;
					return result;
				}
				catch (Exception)
				{
					return DateTime.Now;
					IL_003a:
					DateTime result;
					return result;
				}
			}
			set
			{
				CheckRequestStarted();
				webHeaders.SetInternal("If-Modified-Since", value.ToUniversalTime().ToString("r", null));
			}
		}

		/// <summary>Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.</summary>
		/// <returns>true if the request to the Internet resource should contain a Connection HTTP header with the value Keep-alive; otherwise, false. The default is true.</returns>
		public bool KeepAlive
		{
			get
			{
				return keepAlive;
			}
			set
			{
				keepAlive = value;
			}
		}

		/// <summary>Gets or sets the maximum number of redirects that the request follows.</summary>
		/// <returns>The maximum number of redirection responses that the request follows. The default value is 50.</returns>
		/// <exception cref="T:System.ArgumentException">The value is set to 0 or less. </exception>
		public int MaximumAutomaticRedirections
		{
			get
			{
				return maxAutoRedirect;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Must be > 0", "value");
				}
				maxAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets the maximum allowed length of the response headers.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the response headers.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is set after the request has already been submitted. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1. </exception>
		[MonoTODO("Use this")]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return maxResponseHeadersLength;
			}
			set
			{
				maxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets the default for the <see cref="P:System.Net.HttpWebRequest.MaximumResponseHeadersLength" /> property.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the default maximum for response headers received. The default configuration file sets this value to 64 kilobytes.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is not equal to -1 and is less than zero. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO("Use this")]
		public static int DefaultMaximumResponseHeadersLength
		{
			get
			{
				return defaultMaxResponseHeadersLength;
			}
			set
			{
				defaultMaxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets a time-out in milliseconds when writing to or reading from a stream.</summary>
		/// <returns>The number of milliseconds before the writing or reading times out. The default value is 300,000 milliseconds (5 minutes).</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has already been sent. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /></exception>
		public int ReadWriteTimeout
		{
			get
			{
				return readWriteTimeout;
			}
			set
			{
				if (requestSent)
				{
					throw new InvalidOperationException("The request has already been sent.");
				}
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value", "Must be >= -1");
				}
				readWriteTimeout = value;
			}
		}

		/// <summary>Gets or sets the media type of the request.</summary>
		/// <returns>The media type of the request. The default value is null.</returns>
		public string MediaType
		{
			get
			{
				return mediaType;
			}
			set
			{
				mediaType = value;
			}
		}

		/// <summary>Gets or sets the method for the request.</summary>
		/// <returns>The request method to use to contact the Internet resource. The default value is GET.</returns>
		/// <exception cref="T:System.ArgumentException">No method is supplied.-or- The method string contains invalid characters. </exception>
		public override string Method
		{
			get
			{
				return method;
			}
			set
			{
				if (value == null || value.Trim() == string.Empty)
				{
					throw new ArgumentException("not a valid method");
				}
				method = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to pipeline the request to the Internet resource.</summary>
		/// <returns>true if the request should be pipelined; otherwise, false. The default is true.</returns>
		public bool Pipelined
		{
			get
			{
				return pipelined;
			}
			set
			{
				pipelined = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to send an Authorization header with the request.</summary>
		/// <returns>true to send an HTTP Authorization header with requests after authentication has taken place; otherwise, false. The default is false.</returns>
		public override bool PreAuthenticate
		{
			get
			{
				return preAuthenticate;
			}
			set
			{
				preAuthenticate = value;
			}
		}

		/// <summary>Gets or sets the version of HTTP to use for the request.</summary>
		/// <returns>The HTTP version to use for the request. The default is <see cref="F:System.Net.HttpVersion.Version11" />.</returns>
		/// <exception cref="T:System.ArgumentException">The HTTP version is set to a value other than 1.0 or 1.1. </exception>
		public Version ProtocolVersion
		{
			get
			{
				return version;
			}
			set
			{
				if (value != HttpVersion.Version10 && value != HttpVersion.Version11)
				{
					throw new ArgumentException("value");
				}
				version = value;
			}
		}

		/// <summary>Gets or sets proxy information for the request.</summary>
		/// <returns>The <see cref="T:System.Net.IWebProxy" /> object to use to proxy the request. The default value is set by calling the <see cref="P:System.Net.GlobalProxySelection.Select" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.HttpWebRequest.Proxy" /> is set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have permission for the requested operation. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IWebProxy Proxy
		{
			get
			{
				return proxy;
			}
			set
			{
				CheckRequestStarted();
				proxy = value;
				servicePoint = null;
			}
		}

		/// <summary>Gets or sets the value of the Referer HTTP header.</summary>
		/// <returns>The value of the Referer HTTP header. The default value is null.</returns>
		public string Referer
		{
			get
			{
				return webHeaders["Referer"];
			}
			set
			{
				CheckRequestStarted();
				if (value == null || value.Trim().Length == 0)
				{
					webHeaders.RemoveInternal("Referer");
				}
				else
				{
					webHeaders.SetInternal("Referer", value);
				}
			}
		}

		/// <summary>Gets the original Uniform Resource Identifier (URI) of the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the Internet resource passed to the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method.</returns>
		public override Uri RequestUri => requestUri;

		/// <summary>Gets or sets a value that indicates whether to send data in segments to the Internet resource.</summary>
		/// <returns>true to send data to the Internet resource in segments; otherwise, false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		public bool SendChunked
		{
			get
			{
				return sendChunked;
			}
			set
			{
				CheckRequestStarted();
				sendChunked = value;
			}
		}

		/// <summary>Gets the service point to use for the request.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> that represents the network connection to the Internet resource.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public ServicePoint ServicePoint => GetServicePoint();

		/// <summary>Gets or sets the time-out value in milliseconds for the <see cref="M:System.Net.HttpWebRequest.GetResponse" /> and <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> methods.</summary>
		/// <returns>The number of milliseconds to wait before the request times out. The default value is 100,000 milliseconds (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than zero and is not <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
		public override int Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				timeout = value;
			}
		}

		/// <summary>Gets or sets the value of the Transfer-encoding HTTP header.</summary>
		/// <returns>The value of the Transfer-encoding HTTP header. The default value is null.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set when <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to the value "Chunked". </exception>
		public string TransferEncoding
		{
			get
			{
				return webHeaders["Transfer-Encoding"];
			}
			set
			{
				CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					webHeaders.RemoveInternal("Transfer-Encoding");
					return;
				}
				if (text == "chunked")
				{
					throw new ArgumentException("Chunked encoding must be set with the SendChunked property");
				}
				if (!sendChunked)
				{
					throw new ArgumentException("SendChunked must be True", "value");
				}
				webHeaders.RemoveAndAdd("Transfer-Encoding", value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether default credentials are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">You attempted to set this property after the request was sent.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		public override bool UseDefaultCredentials
		{
			get
			{
				return CredentialCache.DefaultCredentials == Credentials;
			}
			set
			{
				object obj;
				if (value)
				{
					ICredentials defaultCredentials = CredentialCache.DefaultCredentials;
					obj = defaultCredentials;
				}
				else
				{
					obj = null;
				}
				Credentials = (ICredentials)obj;
			}
		}

		/// <summary>Gets or sets the value of the User-agent HTTP header.</summary>
		/// <returns>The value of the User-agent HTTP header. The default value is null.Note:The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		public string UserAgent
		{
			get
			{
				return webHeaders["User-Agent"];
			}
			set
			{
				webHeaders.SetInternal("User-Agent", value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing.</summary>
		/// <returns>true to keep the authenticated connection open; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return unsafe_auth_blah;
			}
			set
			{
				unsafe_auth_blah = value;
			}
		}

		internal bool GotRequestStream => gotRequestStream;

		internal bool ExpectContinue
		{
			get
			{
				return expectContinue;
			}
			set
			{
				expectContinue = value;
			}
		}

		internal Uri AuthUri => actualUri;

		internal bool ProxyQuery => servicePoint.UsesProxy && !servicePoint.UseConnect;

		internal bool FinishedReading
		{
			get
			{
				return finished_reading;
			}
			set
			{
				finished_reading = value;
			}
		}

		internal bool Aborted => Interlocked.CompareExchange(ref aborted, 0, 0) == 1;

		public HttpWebRequest(Uri uri)
		{
			requestUri = uri;
			actualUri = uri;
			proxy = GlobalProxySelection.Select;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebRequest" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Net.HttpWebRequest" /> object. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Net.HttpWebRequest" /> object. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected HttpWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			requestUri = (Uri)serializationInfo.GetValue("requestUri", typeof(Uri));
			actualUri = (Uri)serializationInfo.GetValue("actualUri", typeof(Uri));
			allowAutoRedirect = serializationInfo.GetBoolean("allowAutoRedirect");
			allowBuffering = serializationInfo.GetBoolean("allowBuffering");
			certificates = (X509CertificateCollection)serializationInfo.GetValue("certificates", typeof(X509CertificateCollection));
			connectionGroup = serializationInfo.GetString("connectionGroup");
			contentLength = serializationInfo.GetInt64("contentLength");
			webHeaders = (WebHeaderCollection)serializationInfo.GetValue("webHeaders", typeof(WebHeaderCollection));
			keepAlive = serializationInfo.GetBoolean("keepAlive");
			maxAutoRedirect = serializationInfo.GetInt32("maxAutoRedirect");
			mediaType = serializationInfo.GetString("mediaType");
			method = serializationInfo.GetString("method");
			initialMethod = serializationInfo.GetString("initialMethod");
			pipelined = serializationInfo.GetBoolean("pipelined");
			version = (Version)serializationInfo.GetValue("version", typeof(Version));
			proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			sendChunked = serializationInfo.GetBoolean("sendChunked");
			timeout = serializationInfo.GetInt32("timeout");
			redirects = serializationInfo.GetInt32("redirects");
		}

		static HttpWebRequest()
		{
			defaultMaxResponseHeadersLength = 65536;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			GetObjectData(serializationInfo, streamingContext);
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		internal ServicePoint GetServicePoint()
		{
			lock (locker)
			{
				if (hostChanged || servicePoint == null)
				{
					servicePoint = ServicePointManager.FindServicePoint(actualUri, proxy);
					hostChanged = false;
				}
			}
			return servicePoint;
		}

		/// <summary>Adds a byte range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="range">The starting or ending point of the range. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(int range)
		{
			AddRange("bytes", range);
		}

		/// <summary>Adds a byte range header to the request for a specified range.</summary>
		/// <param name="from">The position at which to start sending data. </param>
		/// <param name="to">The position at which to stop sending data. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />-or- <paramref name="from" /> or <paramref name="to" /> is less than 0. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(int from, int to)
		{
			AddRange("bytes", from, to);
		}

		/// <summary>Adds a Range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="rangeSpecifier">The description of the range. </param>
		/// <param name="range">The starting or ending point of the range. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(string rangeSpecifier, int range)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			string text = webHeaders["Range"];
			if (text == null || text.Length == 0)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (!text.ToLower().StartsWith(rangeSpecifier.ToLower() + "="))
				{
					throw new InvalidOperationException("rangeSpecifier");
				}
				text += ",";
			}
			webHeaders.RemoveAndAdd("Range", text + range + "-");
		}

		/// <summary>Adds a range header to a request for a specified range.</summary>
		/// <param name="rangeSpecifier">The description of the range. </param>
		/// <param name="from">The position at which to start sending data. </param>
		/// <param name="to">The position at which to stop sending data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />-or- <paramref name="from" /> or <paramref name="to" /> is less than 0. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(string rangeSpecifier, int from, int to)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (from < 0 || to < 0 || from > to)
			{
				throw new ArgumentOutOfRangeException();
			}
			string text = webHeaders["Range"];
			if (text == null || text.Length == 0)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (!text.ToLower().StartsWith(rangeSpecifier.ToLower() + "="))
				{
					throw new InvalidOperationException("rangeSpecifier");
				}
				text += ",";
			}
			webHeaders.RemoveAndAdd("Range", text + from + "-" + to);
		}

		/// <summary>Begins an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">The state object for this request. </param>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false.-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called. </exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (Aborted)
			{
				throw new WebException("The request was canceled.", WebExceptionStatus.RequestCanceled);
			}
			bool flag = !(method == "GET") && !(method == "CONNECT") && !(method == "HEAD") && !(method == "TRACE") && !(method == "DELETE");
			if (method == null || !flag)
			{
				throw new ProtocolViolationException("Cannot send data when method is: " + method);
			}
			if (contentLength == -1 && !sendChunked && !allowBuffering && KeepAlive)
			{
				throw new ProtocolViolationException("Content-Length not set");
			}
			string transferEncoding = TransferEncoding;
			if (!sendChunked && transferEncoding != null && transferEncoding.Trim() != string.Empty)
			{
				throw new ProtocolViolationException("SendChunked should be true.");
			}
			lock (locker)
			{
				if (asyncWrite != null)
				{
					throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
				}
				asyncWrite = new WebAsyncResult(this, callback, state);
				initialMethod = method;
				if (haveRequest && writeStream != null)
				{
					asyncWrite.SetCompleted(synch: true, writeStream);
					asyncWrite.DoCallback();
					return asyncWrite;
				}
				gotRequestStream = true;
				WebAsyncResult result = asyncWrite;
				if (!requestSent)
				{
					requestSent = true;
					redirects = 0;
					servicePoint = GetServicePoint();
					abortHandler = servicePoint.SendRequest(this, connectionGroup);
				}
				return result;
				IL_01ea:
				IAsyncResult result2;
				return result2;
			}
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <param name="asyncResult">The pending request for a stream. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.IO.IOException">The request did not complete, and no stream is available. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult" />. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- An error occurred while processing the request. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WebAsyncResult webAsyncResult = asyncResult as WebAsyncResult;
			if (webAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult");
			}
			asyncWrite = webAsyncResult;
			webAsyncResult.WaitUntilComplete();
			Exception exception = webAsyncResult.Exception;
			if (exception != null)
			{
				throw exception;
			}
			return webAsyncResult.WriteStream;
		}

		/// <summary>Gets a <see cref="T:System.IO.Stream" /> object to use to write request data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method is called more than once.-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- The time-out period for the request expired.-or- An error occurred while processing the request. </exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult = asyncWrite;
			if (asyncResult == null)
			{
				asyncResult = BeginGetRequestStream(null, null);
				asyncWrite = (WebAsyncResult)asyncResult;
			}
			if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(timeout, exitContext: false))
			{
				Abort();
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			return EndGetRequestStream(asyncResult);
		}

		private void CheckIfForceWrite()
		{
			if (writeStream != null && !writeStream.RequestWritten && contentLength >= 0 && InternalAllowBuffering && writeStream.WriteBufferLength == contentLength)
			{
				writeStream.WriteRequest();
			}
		}

		/// <summary>Begins an asynchronous request to an Internet resource.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request for a response.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate </param>
		/// <param name="state">The state object for this request. </param>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false.-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater than zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is true.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (Aborted)
			{
				throw new WebException("The request was canceled.", WebExceptionStatus.RequestCanceled);
			}
			if (method == null)
			{
				throw new ProtocolViolationException("Method is null.");
			}
			string transferEncoding = TransferEncoding;
			if (!sendChunked && transferEncoding != null && transferEncoding.Trim() != string.Empty)
			{
				throw new ProtocolViolationException("SendChunked should be true.");
			}
			Monitor.Enter(locker);
			getResponseCalled = true;
			if (asyncRead != null && !haveResponse)
			{
				Monitor.Exit(locker);
				throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
			}
			CheckIfForceWrite();
			asyncRead = new WebAsyncResult(this, callback, state);
			WebAsyncResult webAsyncResult = asyncRead;
			initialMethod = method;
			if (haveResponse)
			{
				Exception ex = saved_exc;
				if (webResponse != null)
				{
					Monitor.Exit(locker);
					if (ex == null)
					{
						webAsyncResult.SetCompleted(synch: true, webResponse);
					}
					else
					{
						webAsyncResult.SetCompleted(synch: true, ex);
					}
					webAsyncResult.DoCallback();
					return webAsyncResult;
				}
				if (ex != null)
				{
					Monitor.Exit(locker);
					webAsyncResult.SetCompleted(synch: true, ex);
					webAsyncResult.DoCallback();
					return webAsyncResult;
				}
			}
			if (!requestSent)
			{
				requestSent = true;
				redirects = 0;
				servicePoint = GetServicePoint();
				abortHandler = servicePoint.SendRequest(this, connectionGroup);
			}
			Monitor.Exit(locker);
			return webAsyncResult;
		}

		/// <summary>Ends an asynchronous request to an Internet resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <param name="asyncResult">The pending request for a response. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult." />-or- The <see cref="P:System.Net.HttpWebRequest.ContentLength" /> property is greater than 0 but the data has not been written to the request stream. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- An error occurred while processing the request. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WebAsyncResult webAsyncResult = asyncResult as WebAsyncResult;
			if (webAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (!webAsyncResult.WaitUntilComplete(timeout, exitContext: false))
			{
				Abort();
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			if (webAsyncResult.GotException)
			{
				throw webAsyncResult.Exception;
			}
			return webAsyncResult.Response;
		}

		/// <summary>Returns a response from an Internet resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater or equal to zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is true.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, this request includes data to be sent to the server. Requests that send data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- The time-out period for the request expired.-or- An error occurred while processing the request. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override WebResponse GetResponse()
		{
			WebAsyncResult asyncResult = (WebAsyncResult)BeginGetResponse(null, null);
			return EndGetResponse(asyncResult);
		}

		/// <summary>Cancels a request to an Internet resource.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Abort()
		{
			if (Interlocked.CompareExchange(ref aborted, 1, 0) == 1 || (haveResponse && finished_reading))
			{
				return;
			}
			haveResponse = true;
			if (abortHandler != null)
			{
				try
				{
					abortHandler(this, EventArgs.Empty);
				}
				catch (Exception)
				{
				}
				abortHandler = null;
			}
			if (asyncWrite != null)
			{
				WebAsyncResult webAsyncResult = asyncWrite;
				if (!webAsyncResult.IsCompleted)
				{
					try
					{
						WebException e = new WebException("Aborted.", WebExceptionStatus.RequestCanceled);
						webAsyncResult.SetCompleted(synch: false, e);
						webAsyncResult.DoCallback();
					}
					catch
					{
					}
				}
				asyncWrite = null;
			}
			if (asyncRead != null)
			{
				WebAsyncResult webAsyncResult2 = asyncRead;
				if (!webAsyncResult2.IsCompleted)
				{
					try
					{
						WebException e2 = new WebException("Aborted.", WebExceptionStatus.RequestCanceled);
						webAsyncResult2.SetCompleted(synch: false, e2);
						webAsyncResult2.DoCallback();
					}
					catch
					{
					}
				}
				asyncRead = null;
			}
			if (writeStream != null)
			{
				try
				{
					writeStream.Close();
					writeStream = null;
				}
				catch
				{
				}
			}
			if (webResponse != null)
			{
				try
				{
					webResponse.Close();
					webResponse = null;
				}
				catch
				{
				}
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data required to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("requestUri", requestUri, typeof(Uri));
			serializationInfo.AddValue("actualUri", actualUri, typeof(Uri));
			serializationInfo.AddValue("allowAutoRedirect", allowAutoRedirect);
			serializationInfo.AddValue("allowBuffering", allowBuffering);
			serializationInfo.AddValue("certificates", certificates, typeof(X509CertificateCollection));
			serializationInfo.AddValue("connectionGroup", connectionGroup);
			serializationInfo.AddValue("contentLength", contentLength);
			serializationInfo.AddValue("webHeaders", webHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("keepAlive", keepAlive);
			serializationInfo.AddValue("maxAutoRedirect", maxAutoRedirect);
			serializationInfo.AddValue("mediaType", mediaType);
			serializationInfo.AddValue("method", method);
			serializationInfo.AddValue("initialMethod", initialMethod);
			serializationInfo.AddValue("pipelined", pipelined);
			serializationInfo.AddValue("version", version, typeof(Version));
			serializationInfo.AddValue("proxy", proxy, typeof(IWebProxy));
			serializationInfo.AddValue("sendChunked", sendChunked);
			serializationInfo.AddValue("timeout", timeout);
			serializationInfo.AddValue("redirects", redirects);
		}

		private void CheckRequestStarted()
		{
			if (requestSent)
			{
				throw new InvalidOperationException("request started");
			}
		}

		internal void DoContinueDelegate(int statusCode, WebHeaderCollection headers)
		{
			if (continueDelegate != null)
			{
				continueDelegate(statusCode, headers);
			}
		}

		private bool Redirect(WebAsyncResult result, HttpStatusCode code)
		{
			redirects++;
			Exception ex = null;
			string text = null;
			switch (code)
			{
			case HttpStatusCode.MultipleChoices:
				ex = new WebException("Ambiguous redirect.");
				break;
			case HttpStatusCode.MovedPermanently:
			case HttpStatusCode.Found:
			case HttpStatusCode.TemporaryRedirect:
				contentLength = -1L;
				bodyBufferLength = 0;
				bodyBuffer = null;
				method = "GET";
				text = webResponse.Headers["Location"];
				break;
			case HttpStatusCode.SeeOther:
				method = "GET";
				text = webResponse.Headers["Location"];
				break;
			case HttpStatusCode.NotModified:
				return false;
			case HttpStatusCode.UseProxy:
				ex = new NotImplementedException("Proxy support not available.");
				break;
			default:
				ex = new ProtocolViolationException("Invalid status code: " + (int)code);
				break;
			}
			if (ex != null)
			{
				throw ex;
			}
			if (text == null)
			{
				throw new WebException("No Location header found for " + (int)code, WebExceptionStatus.ProtocolError);
			}
			Uri uri = actualUri;
			try
			{
				actualUri = new Uri(actualUri, text);
			}
			catch (Exception)
			{
				throw new WebException($"Invalid URL ({text}) for {(int)code}", WebExceptionStatus.ProtocolError);
				IL_0140:;
			}
			hostChanged = (actualUri.Scheme != uri.Scheme || actualUri.Host != uri.Host || actualUri.Port != uri.Port);
			return true;
		}

		private string GetHeaders()
		{
			bool flag = false;
			if (sendChunked)
			{
				flag = true;
				webHeaders.RemoveAndAdd("Transfer-Encoding", "chunked");
				webHeaders.RemoveInternal("Content-Length");
			}
			else if (contentLength != -1)
			{
				if (contentLength > 0)
				{
					flag = true;
				}
				webHeaders.SetInternal("Content-Length", contentLength.ToString());
				webHeaders.RemoveInternal("Transfer-Encoding");
			}
			if (actualVersion == HttpVersion.Version11 && flag && servicePoint.SendContinue)
			{
				webHeaders.RemoveAndAdd("Expect", "100-continue");
				expectContinue = true;
			}
			else
			{
				webHeaders.RemoveInternal("Expect");
				expectContinue = false;
			}
			bool proxyQuery = ProxyQuery;
			string name = (!proxyQuery) ? "Connection" : "Proxy-Connection";
			webHeaders.RemoveInternal(proxyQuery ? "Connection" : "Proxy-Connection");
			Version protocolVersion = servicePoint.ProtocolVersion;
			bool flag2 = protocolVersion == null || protocolVersion == HttpVersion.Version10;
			if (keepAlive && (version == HttpVersion.Version10 || flag2))
			{
				webHeaders.RemoveAndAdd(name, "keep-alive");
			}
			else if (!keepAlive && version == HttpVersion.Version11)
			{
				webHeaders.RemoveAndAdd(name, "close");
			}
			webHeaders.SetInternal("Host", actualUri.Authority);
			if (cookieContainer != null)
			{
				string cookieHeader = cookieContainer.GetCookieHeader(actualUri);
				if (cookieHeader != string.Empty)
				{
					webHeaders.SetInternal("Cookie", cookieHeader);
				}
			}
			string text = null;
			if ((auto_decomp & DecompressionMethods.GZip) != 0)
			{
				text = "gzip";
			}
			if ((auto_decomp & DecompressionMethods.Deflate) != 0)
			{
				text = ((text == null) ? "deflate" : "gzip, deflate");
			}
			if (text != null)
			{
				webHeaders.RemoveAndAdd("Accept-Encoding", text);
			}
			if (!usedPreAuth && preAuthenticate)
			{
				DoPreAuthenticate();
			}
			return webHeaders.ToString();
		}

		private void DoPreAuthenticate()
		{
			bool flag = proxy != null && !proxy.IsBypassed(actualUri);
			object obj;
			if (!flag || this.credentials != null)
			{
				ICredentials credentials = this.credentials;
				obj = credentials;
			}
			else
			{
				obj = proxy.Credentials;
			}
			ICredentials credentials2 = (ICredentials)obj;
			Authorization authorization = AuthenticationManager.PreAuthenticate(this, credentials2);
			if (authorization != null)
			{
				webHeaders.RemoveInternal("Proxy-Authorization");
				webHeaders.RemoveInternal("Authorization");
				string name = (!flag || this.credentials != null) ? "Authorization" : "Proxy-Authorization";
				webHeaders[name] = authorization.Message;
				usedPreAuth = true;
			}
		}

		internal void SetWriteStreamError(WebExceptionStatus status, Exception exc)
		{
			if (Aborted)
			{
				return;
			}
			WebAsyncResult webAsyncResult = asyncWrite;
			if (webAsyncResult == null)
			{
				webAsyncResult = asyncRead;
			}
			if (webAsyncResult != null)
			{
				WebException e;
				if (exc == null)
				{
					string message = "Error: " + status;
					e = new WebException(message, status);
				}
				else
				{
					string message = $"Error: {status} ({exc.Message})";
					e = new WebException(message, exc, status);
				}
				webAsyncResult.SetCompleted(synch: false, e);
				webAsyncResult.DoCallback();
			}
		}

		internal void SendRequestHeaders(bool propagate_error)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = (!ProxyQuery) ? actualUri.PathAndQuery : ((!actualUri.IsDefaultPort) ? $"{actualUri.Scheme}://{actualUri.Host}:{actualUri.Port}{actualUri.PathAndQuery}" : $"{actualUri.Scheme}://{actualUri.Host}{actualUri.PathAndQuery}");
			if (servicePoint.ProtocolVersion != null && servicePoint.ProtocolVersion < version)
			{
				actualVersion = servicePoint.ProtocolVersion;
			}
			else
			{
				actualVersion = version;
			}
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}.{3}\r\n", method, text, actualVersion.Major, actualVersion.Minor);
			stringBuilder.Append(GetHeaders());
			string s = stringBuilder.ToString();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				writeStream.SetHeaders(bytes);
			}
			catch (WebException ex)
			{
				SetWriteStreamError(ex.Status, ex);
				if (propagate_error)
				{
					throw;
				}
			}
			catch (Exception exc)
			{
				SetWriteStreamError(WebExceptionStatus.SendFailure, exc);
				if (propagate_error)
				{
					throw;
				}
			}
		}

		internal void SetWriteStream(WebConnectionStream stream)
		{
			if (!Aborted)
			{
				writeStream = stream;
				if (bodyBuffer != null)
				{
					webHeaders.RemoveInternal("Transfer-Encoding");
					contentLength = bodyBufferLength;
					writeStream.SendChunked = false;
				}
				SendRequestHeaders(propagate_error: false);
				haveRequest = true;
				if (bodyBuffer != null)
				{
					writeStream.Write(bodyBuffer, 0, bodyBufferLength);
					bodyBuffer = null;
					writeStream.Close();
				}
				else if (method != "HEAD" && method != "GET" && method != "MKCOL" && method != "CONNECT" && method != "DELETE" && method != "TRACE" && getResponseCalled && !writeStream.RequestWritten)
				{
					writeStream.WriteRequest();
				}
				if (asyncWrite != null)
				{
					asyncWrite.SetCompleted(synch: false, stream);
					asyncWrite.DoCallback();
					asyncWrite = null;
				}
			}
		}

		internal void SetResponseError(WebExceptionStatus status, Exception e, string where)
		{
			if (!Aborted)
			{
				lock (locker)
				{
					string message = $"Error getting response stream ({where}): {status}";
					WebAsyncResult webAsyncResult = asyncRead;
					if (webAsyncResult == null)
					{
						webAsyncResult = asyncWrite;
					}
					WebException e2 = (!(e is WebException)) ? new WebException(message, e, status, null) : ((WebException)e);
					if (webAsyncResult != null)
					{
						if (!webAsyncResult.IsCompleted)
						{
							webAsyncResult.SetCompleted(synch: false, e2);
							webAsyncResult.DoCallback();
						}
						else if (webAsyncResult == asyncWrite)
						{
							saved_exc = e2;
						}
						haveResponse = true;
						asyncRead = null;
						asyncWrite = null;
					}
					else
					{
						haveResponse = true;
						saved_exc = e2;
					}
				}
			}
		}

		private void CheckSendError(WebConnectionData data)
		{
			int statusCode = data.StatusCode;
			if (statusCode >= 400 && statusCode != 401 && statusCode != 407 && writeStream != null && asyncRead == null && !writeStream.CompleteRequestWritten)
			{
				saved_exc = new WebException(data.StatusDescription, null, WebExceptionStatus.ProtocolError, webResponse);
				webResponse.ReadAll();
			}
		}

		private void HandleNtlmAuth(WebAsyncResult r)
		{
			WebConnectionStream webConnectionStream = webResponse.GetResponseStream() as WebConnectionStream;
			if (webConnectionStream != null)
			{
				WebConnection connection = webConnectionStream.Connection;
				connection.PriorityRequest = this;
				object obj;
				if (proxy == null || proxy.IsBypassed(actualUri))
				{
					ICredentials credentials = this.credentials;
					obj = credentials;
				}
				else
				{
					obj = proxy.Credentials;
				}
				ICredentials credentials2 = (ICredentials)obj;
				if (credentials2 != null)
				{
					connection.NtlmCredential = credentials2.GetCredential(requestUri, "NTLM");
					connection.UnsafeAuthenticatedConnectionSharing = unsafe_auth_blah;
				}
			}
			r.Reset();
			haveResponse = false;
			webResponse.ReadAll();
			webResponse = null;
		}

		internal void SetResponseData(WebConnectionData data)
		{
			lock (locker)
			{
				if (Aborted)
				{
					if (data.stream != null)
					{
						data.stream.Close();
					}
				}
				else
				{
					WebException ex = null;
					try
					{
						webResponse = new HttpWebResponse(actualUri, method, data, cookieContainer);
					}
					catch (Exception ex2)
					{
						ex = new WebException(ex2.Message, ex2, WebExceptionStatus.ProtocolError, null);
						if (data.stream != null)
						{
							data.stream.Close();
						}
					}
					if (ex == null && (method == "POST" || method == "PUT"))
					{
						lock (locker)
						{
							CheckSendError(data);
							if (saved_exc != null)
							{
								ex = (WebException)saved_exc;
							}
						}
					}
					WebAsyncResult webAsyncResult = asyncRead;
					bool flag = false;
					if (webAsyncResult == null && webResponse != null)
					{
						flag = true;
						webAsyncResult = new WebAsyncResult(null, null);
						webAsyncResult.SetCompleted(synch: false, webResponse);
					}
					if (webAsyncResult != null)
					{
						if (ex != null)
						{
							webAsyncResult.SetCompleted(synch: false, ex);
							webAsyncResult.DoCallback();
						}
						else
						{
							try
							{
								if (!CheckFinalStatus(webAsyncResult))
								{
									if (is_ntlm_auth && authCompleted && webResponse != null && webResponse.StatusCode < HttpStatusCode.BadRequest)
									{
										WebConnectionStream webConnectionStream = webResponse.GetResponseStream() as WebConnectionStream;
										if (webConnectionStream != null)
										{
											WebConnection connection = webConnectionStream.Connection;
											connection.NtlmAuthenticated = true;
										}
									}
									if (writeStream != null)
									{
										writeStream.KillBuffer();
									}
									haveResponse = true;
									webAsyncResult.SetCompleted(synch: false, webResponse);
									webAsyncResult.DoCallback();
								}
								else
								{
									if (webResponse == null)
									{
										goto IL_0219;
									}
									if (!is_ntlm_auth)
									{
										webResponse.Close();
										goto IL_0219;
									}
									HandleNtlmAuth(webAsyncResult);
								}
								goto end_IL_0143;
								IL_0219:
								finished_reading = false;
								haveResponse = false;
								webResponse = null;
								webAsyncResult.Reset();
								servicePoint = GetServicePoint();
								abortHandler = servicePoint.SendRequest(this, connectionGroup);
								end_IL_0143:;
							}
							catch (WebException e)
							{
								if (flag)
								{
									saved_exc = e;
									haveResponse = true;
								}
								webAsyncResult.SetCompleted(synch: false, e);
								webAsyncResult.DoCallback();
								goto end_IL_025e;
								IL_028c:
								end_IL_025e:;
							}
							catch (Exception ex3)
							{
								ex = new WebException(ex3.Message, ex3, WebExceptionStatus.ProtocolError, null);
								if (flag)
								{
									saved_exc = ex;
									haveResponse = true;
								}
								webAsyncResult.SetCompleted(synch: false, ex);
								webAsyncResult.DoCallback();
								goto end_IL_0291;
								IL_02ce:
								end_IL_0291:;
							}
						}
					}
				}
			}
		}

		private bool CheckAuthorization(WebResponse response, HttpStatusCode code)
		{
			authCompleted = false;
			if (code == HttpStatusCode.Unauthorized && this.credentials == null)
			{
				return false;
			}
			bool flag = code == HttpStatusCode.ProxyAuthenticationRequired;
			if (flag && (proxy == null || proxy.Credentials == null))
			{
				return false;
			}
			string[] values = response.Headers.GetValues((!flag) ? "WWW-Authenticate" : "Proxy-Authenticate");
			if (values == null || values.Length == 0)
			{
				return false;
			}
			object obj;
			if (!flag)
			{
				ICredentials credentials = this.credentials;
				obj = credentials;
			}
			else
			{
				obj = proxy.Credentials;
			}
			ICredentials credentials2 = (ICredentials)obj;
			Authorization authorization = null;
			string[] array = values;
			foreach (string challenge in array)
			{
				authorization = AuthenticationManager.Authenticate(challenge, this, credentials2);
				if (authorization != null)
				{
					break;
				}
			}
			if (authorization == null)
			{
				return false;
			}
			webHeaders[(!flag) ? "Authorization" : "Proxy-Authorization"] = authorization.Message;
			authCompleted = authorization.Complete;
			is_ntlm_auth = (authorization.Module.AuthenticationType == "NTLM");
			return true;
		}

		private bool CheckFinalStatus(WebAsyncResult result)
		{
			if (result.GotException)
			{
				throw result.Exception;
			}
			Exception ex = result.Exception;
			bodyBuffer = null;
			HttpWebResponse response = result.Response;
			WebExceptionStatus status = WebExceptionStatus.ProtocolError;
			HttpStatusCode httpStatusCode = (HttpStatusCode)0;
			if (ex == null && webResponse != null)
			{
				httpStatusCode = webResponse.StatusCode;
				if (!authCompleted && ((httpStatusCode == HttpStatusCode.Unauthorized && credentials != null) || (ProxyQuery && httpStatusCode == HttpStatusCode.ProxyAuthenticationRequired)) && !usedPreAuth && CheckAuthorization(webResponse, httpStatusCode))
				{
					if (InternalAllowBuffering)
					{
						bodyBuffer = writeStream.WriteBuffer;
						bodyBufferLength = writeStream.WriteBufferLength;
						return true;
					}
					if (method != "PUT" && method != "POST")
					{
						return true;
					}
					writeStream.InternalClose();
					writeStream = null;
					webResponse.Close();
					webResponse = null;
					throw new WebException("This request requires buffering of data for authentication or redirection to be sucessful.");
				}
				if (httpStatusCode >= HttpStatusCode.BadRequest)
				{
					string message = $"The remote server returned an error: ({(int)httpStatusCode}) {webResponse.StatusDescription}.";
					ex = new WebException(message, null, status, webResponse);
					webResponse.ReadAll();
				}
				else if (httpStatusCode == HttpStatusCode.NotModified && allowAutoRedirect)
				{
					string message2 = $"The remote server returned an error: ({(int)httpStatusCode}) {webResponse.StatusDescription}.";
					ex = new WebException(message2, null, status, webResponse);
				}
				else if (httpStatusCode >= HttpStatusCode.MultipleChoices && allowAutoRedirect && redirects >= maxAutoRedirect)
				{
					ex = new WebException("Max. redirections exceeded.", null, status, webResponse);
					webResponse.ReadAll();
				}
			}
			if (ex == null)
			{
				bool result2 = false;
				int num = (int)httpStatusCode;
				if (allowAutoRedirect && num >= 300)
				{
					if (InternalAllowBuffering && writeStream.WriteBufferLength > 0)
					{
						bodyBuffer = writeStream.WriteBuffer;
						bodyBufferLength = writeStream.WriteBufferLength;
					}
					result2 = Redirect(result, httpStatusCode);
				}
				if (response != null && num >= 300 && num != 304)
				{
					response.ReadAll();
				}
				return result2;
			}
			if (writeStream != null)
			{
				writeStream.InternalClose();
				writeStream = null;
			}
			webResponse = null;
			throw ex;
		}
	}
}
