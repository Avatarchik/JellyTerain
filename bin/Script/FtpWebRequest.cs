using System.IO;
using System.Net.Cache;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Implements a File Transfer Protocol (FTP) client.</summary>
	public sealed class FtpWebRequest : WebRequest
	{
		private enum RequestState
		{
			Before,
			Scheduled,
			Connecting,
			Authenticating,
			OpeningData,
			TransferInProgress,
			Finished,
			Aborted,
			Error
		}

		private const string ChangeDir = "CWD";

		private const string UserCommand = "USER";

		private const string PasswordCommand = "PASS";

		private const string TypeCommand = "TYPE";

		private const string PassiveCommand = "PASV";

		private const string PortCommand = "PORT";

		private const string AbortCommand = "ABOR";

		private const string AuthCommand = "AUTH";

		private const string RestCommand = "REST";

		private const string RenameFromCommand = "RNFR";

		private const string RenameToCommand = "RNTO";

		private const string QuitCommand = "QUIT";

		private const string EOL = "\r\n";

		private Uri requestUri;

		private string file_name;

		private ServicePoint servicePoint;

		private Stream origDataStream;

		private Stream dataStream;

		private Stream controlStream;

		private StreamReader controlReader;

		private NetworkCredential credentials;

		private IPHostEntry hostEntry;

		private IPEndPoint localEndPoint;

		private IWebProxy proxy;

		private int timeout = 100000;

		private int rwTimeout = 300000;

		private long offset;

		private bool binary = true;

		private bool enableSsl;

		private bool usePassive = true;

		private bool keepAlive;

		private string method = "RETR";

		private string renameTo;

		private object locker = new object();

		private RequestState requestState;

		private FtpAsyncResult asyncResult;

		private FtpWebResponse ftpResponse;

		private Stream requestStream;

		private string initial_path;

		private static readonly string[] supportedCommands = new string[13]
		{
			"APPE",
			"DELE",
			"LIST",
			"MDTM",
			"MKD",
			"NLST",
			"PWD",
			"RENAME",
			"RETR",
			"RMD",
			"SIZE",
			"STOR",
			"STOU"
		};

		private RemoteCertificateValidationCallback callback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (ServicePointManager.ServerCertificateValidationCallback != null)
			{
				return ServicePointManager.ServerCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors);
			}
			if (sslPolicyErrors != 0)
			{
				throw new InvalidOperationException("SSL authentication error: " + sslPolicyErrors);
			}
			return true;
		};

		/// <summary>Gets the certificates used for establishing an encrypted connection to the FTP server.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> object that contains the client certificates.</returns>
		[MonoTODO]
		public X509CertificateCollection ClientCertificates
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

		/// <summary>Gets or sets the name of the connection group that contains the service point used to send the current request.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains a connection group name.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		[MonoTODO]
		public override string ConnectionGroupName
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

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Content type information is not supported for FTP.</exception>
		public override string ContentType
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets or sets a value that is ignored by the <see cref="T:System.Net.FtpWebRequest" /> class.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that should be ignored.</returns>
		public override long ContentLength
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a byte offset into the file being downloaded by this request.</summary>
		/// <returns>An <see cref="T:System.Int64" /> instance that specifies the file offset, in bytes. The default value is zero.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for this property is less than zero. </exception>
		public long ContentOffset
		{
			get
			{
				return offset;
			}
			set
			{
				CheckRequestStarted();
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				offset = value;
			}
		}

		/// <summary>Gets or sets the credentials used to communicate with the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> instance; otherwise, null if the property has not been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">An <see cref="T:System.Net.ICredentials" /> of a type other than <see cref="T:System.Net.NetworkCredential" /> was specified for a set operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override ICredentials Credentials
		{
			get
			{
				return credentials;
			}
			set
			{
				CheckRequestStarted();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!(value is NetworkCredential))
				{
					throw new ArgumentException();
				}
				credentials = (value as NetworkCredential);
			}
		}

		/// <summary>Defines the default cache policy for all FTP requests.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCachePolicy" /> that defines the cache policy for FTP requests.</returns>
		/// <exception cref="T:System.ArgumentNullException">The caller tried to set this property to null.</exception>
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

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> that specifies that an SSL connection should be used.</summary>
		/// <returns>true if control and data transmissions are encrypted; otherwise, false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The connection to the FTP server has already been established.</exception>
		public bool EnableSsl
		{
			get
			{
				return enableSsl;
			}
			set
			{
				CheckRequestStarted();
				enableSsl = value;
			}
		}

		/// <summary>Gets an empty <see cref="T:System.Net.WebHeaderCollection" /> object.</summary>
		/// <returns>An empty <see cref="T:System.Net.WebHeaderCollection" /> object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO]
		public override WebHeaderCollection Headers
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

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the control connection to the FTP server is closed after the request completes.</summary>
		/// <returns>true if the connection to the server should not be destroyed; otherwise, false. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		[MonoTODO("We don't support KeepAlive = true")]
		public bool KeepAlive
		{
			get
			{
				return keepAlive;
			}
			set
			{
				CheckRequestStarted();
			}
		}

		/// <summary>Gets or sets the command to send to the FTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the FTP command to send to the server. The default value is <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <exception cref="T:System.ArgumentException">The method is invalid.- or -The method is not supported.- or -Multiple methods were specified.</exception>
		public override string Method
		{
			get
			{
				return method;
			}
			set
			{
				CheckRequestStarted();
				if (value == null)
				{
					throw new ArgumentNullException("Method string cannot be null");
				}
				if (value.Length == 0 || Array.BinarySearch(supportedCommands, value) < 0)
				{
					throw new ArgumentException("Method not supported", "value");
				}
				method = value;
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Preauthentication is not supported for FTP.</exception>
		public override bool PreAuthenticate
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets or sets the proxy used to communicate with the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> instance responsible for communicating with the FTP server.</returns>
		/// <exception cref="T:System.ArgumentNullException">This property cannot be set to null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <PermissionSet>
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
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				proxy = value;
			}
		}

		/// <summary>Gets or sets a time-out when reading from or writing to a stream.</summary>
		/// <returns>The number of milliseconds before the reading or writing times out. The default value is 300,000 milliseconds (5 minutes).</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has already been sent. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		public int ReadWriteTimeout
		{
			get
			{
				return rwTimeout;
			}
			set
			{
				CheckRequestStarted();
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				rwTimeout = value;
			}
		}

		/// <summary>Gets or sets the new name of a file being renamed.</summary>
		/// <returns>The new name of the file being renamed.</returns>
		public string RenameTo
		{
			get
			{
				return renameTo;
			}
			set
			{
				CheckRequestStarted();
				if (value == null || value.Length == 0)
				{
					throw new ArgumentException("RenameTo value can't be null or empty", "RenameTo");
				}
				renameTo = value;
			}
		}

		/// <summary>Gets the URI requested by this instance.</summary>
		/// <returns>A <see cref="T:System.Uri" /> instance that identifies a resource that is accessed using the File Transfer Protocol.</returns>
		public override Uri RequestUri => requestUri;

		/// <summary>Gets the <see cref="T:System.Net.ServicePoint" /> object used to connect to the FTP server.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> object that can be used to customize connection behavior.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public ServicePoint ServicePoint => GetServicePoint();

		/// <summary>Gets or sets the behavior of a client application's data transfer process.</summary>
		/// <returns>false if the client application's data transfer process listens for a connection on the data port; otherwise, true if the client should initiate a connection on the data port. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		public bool UsePassive
		{
			get
			{
				return usePassive;
			}
			set
			{
				CheckRequestStarted();
				usePassive = value;
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Default credentials are not supported for FTP.</exception>
		[MonoTODO]
		public override bool UseDefaultCredentials
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

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies the data type for file transfers.</summary>
		/// <returns>true to indicate to the server that the data to be transferred is binary; false to indicate that the data is text. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress.</exception>
		public bool UseBinary
		{
			get
			{
				return binary;
			}
			set
			{
				CheckRequestStarted();
				binary = value;
			}
		}

		/// <summary>Gets or sets the number of milliseconds to wait for a request.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the number of milliseconds to wait before a request times out. The default value is <see cref="F:System.Threading.Timeout.Infinite" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than zero and is not <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		public override int Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				CheckRequestStarted();
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				timeout = value;
			}
		}

		private string DataType => (!binary) ? "A" : "I";

		private RequestState State
		{
			get
			{
				lock (locker)
				{
					return requestState;
					IL_0019:
					RequestState result;
					return result;
				}
			}
			set
			{
				lock (locker)
				{
					CheckIfAborted();
					CheckFinalState();
					requestState = value;
				}
			}
		}

		internal FtpWebRequest(Uri uri)
		{
			requestUri = uri;
			proxy = GlobalProxySelection.Select;
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Terminates an asynchronous FTP operation.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Abort()
		{
			lock (locker)
			{
				if (State == RequestState.TransferInProgress)
				{
					SendCommand(false, "ABOR");
				}
				if (!InFinalState())
				{
					State = RequestState.Aborted;
					ftpResponse = new FtpWebResponse(this, requestUri, method, FtpStatusCode.FileActionAborted, "Aborted by request");
				}
			}
		}

		/// <summary>Begins sending a request and receiving a response from an FTP server asynchronously.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that indicates the status of the operation.</returns>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.GetResponse" /> or <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> has already been called for this instance. </exception>
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
			if (asyncResult != null && !asyncResult.IsCompleted)
			{
				throw new InvalidOperationException("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress");
			}
			CheckIfAborted();
			asyncResult = new FtpAsyncResult(callback, state);
			lock (locker)
			{
				if (InFinalState())
				{
					asyncResult.SetCompleted(synch: true, ftpResponse);
				}
				else
				{
					if (State == RequestState.Before)
					{
						State = RequestState.Scheduled;
					}
					Thread thread = new Thread(ProcessRequest);
					thread.Start();
				}
			}
			return asyncResult;
		}

		/// <summary>Ends a pending asynchronous operation started with <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> reference that contains an <see cref="T:System.Net.FtpWebResponse" /> instance. This object contains the FTP server's response to the request.</returns>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> that was returned when the operation started. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("AsyncResult cannot be null!");
			}
			if (!(asyncResult is FtpAsyncResult) || asyncResult != this.asyncResult)
			{
				throw new ArgumentException("AsyncResult is from another request!");
			}
			FtpAsyncResult ftpAsyncResult = (FtpAsyncResult)asyncResult;
			if (!ftpAsyncResult.WaitUntilComplete(timeout, exitContext: false))
			{
				Abort();
				throw new WebException("Transfer timed out.", WebExceptionStatus.Timeout);
			}
			CheckIfAborted();
			asyncResult = null;
			if (ftpAsyncResult.GotException)
			{
				throw ftpAsyncResult.Exception;
			}
			return ftpAsyncResult.Response;
		}

		/// <summary>Returns the FTP server response.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> reference that contains an <see cref="T:System.Net.FtpWebResponse" /> instance. This object contains the FTP server's response to the request.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.GetResponse" /> or <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> has already been called for this instance.- or -An HTTP proxy is enabled, and you attempted to use an FTP command other than <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />, <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectory" />, or <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectoryDetails" />.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="P:System.Net.FtpWebRequest.EnableSsl" /> is set to true, but the server does not support this feature.</exception>
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
			IAsyncResult asyncResult = BeginGetResponse(null, null);
			return EndGetResponse(asyncResult);
		}

		/// <summary>Begins asynchronously opening a request's content stream for writing.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that indicates the status of the operation.</returns>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes. </param>
		/// <exception cref="T:System.InvalidOperationException">A previous call to this method or <see cref="M:System.Net.FtpWebRequest.GetRequestStream" /> has not yet completed. </exception>
		/// <exception cref="T:System.Net.WebException">A connection to the FTP server could not be established. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FtpWebRequest.Method" /> property is not set to <see cref="F:System.Net.WebRequestMethods.Ftp.UploadFile" />. </exception>
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
			if (method != "STOR" && method != "STOU" && method != "APPE")
			{
				throw new ProtocolViolationException();
			}
			lock (locker)
			{
				CheckIfAborted();
				if (State != 0)
				{
					throw new InvalidOperationException("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress");
				}
				State = RequestState.Scheduled;
			}
			asyncResult = new FtpAsyncResult(callback, state);
			Thread thread = new Thread(ProcessRequest);
			thread.Start();
			return asyncResult;
		}

		/// <summary>Ends a pending asynchronous operation started with <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A writable <see cref="T:System.IO.Stream" /> instance associated with this instance.</returns>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> object that was returned when the operation started. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is FtpAsyncResult))
			{
				throw new ArgumentException("asyncResult");
			}
			if (State == RequestState.Aborted)
			{
				throw new WebException("Request aborted", WebExceptionStatus.RequestCanceled);
			}
			if (asyncResult != this.asyncResult)
			{
				throw new ArgumentException("AsyncResult is from another request!");
			}
			FtpAsyncResult ftpAsyncResult = (FtpAsyncResult)asyncResult;
			if (!ftpAsyncResult.WaitUntilComplete(timeout, exitContext: false))
			{
				Abort();
				throw new WebException("Request timed out");
			}
			if (ftpAsyncResult.GotException)
			{
				throw ftpAsyncResult.Exception;
			}
			return ftpAsyncResult.Stream;
		}

		/// <summary>Retrieves the stream used to upload data to an FTP server.</summary>
		/// <returns>A writable <see cref="T:System.IO.Stream" /> instance used to store data to be sent to the server by the current request.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" /> has been called and has not completed. - or -An HTTP proxy is enabled, and you attempted to use an FTP command other than <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />, <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectory" />, or <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectoryDetails" />.</exception>
		/// <exception cref="T:System.Net.WebException">A connection to the FTP server could not be established. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FtpWebRequest.Method" /> property is not set to <see cref="F:System.Net.WebRequestMethods.Ftp.UploadFile" /> or <see cref="F:System.Net.WebRequestMethods.Ftp.AppendFile" />. </exception>
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
			IAsyncResult asyncResult = BeginGetRequestStream(null, null);
			return EndGetRequestStream(asyncResult);
		}

		private ServicePoint GetServicePoint()
		{
			if (servicePoint == null)
			{
				servicePoint = ServicePointManager.FindServicePoint(requestUri, proxy);
			}
			return servicePoint;
		}

		private void ResolveHost()
		{
			CheckIfAborted();
			hostEntry = GetServicePoint().HostEntry;
			if (hostEntry == null)
			{
				ftpResponse.UpdateStatus(new FtpStatus(FtpStatusCode.ActionAbortedLocalProcessingError, "Cannot resolve server name"));
				throw new WebException("The remote server name could not be resolved: " + requestUri, null, WebExceptionStatus.NameResolutionFailure, ftpResponse);
			}
		}

		private void ProcessRequest()
		{
			if (State == RequestState.Scheduled)
			{
				ftpResponse = new FtpWebResponse(this, requestUri, method, keepAlive);
				try
				{
					ProcessMethod();
					asyncResult.SetCompleted(synch: false, ftpResponse);
				}
				catch (Exception completeWithError)
				{
					State = RequestState.Error;
					SetCompleteWithError(completeWithError);
				}
				return;
			}
			if (InProgress())
			{
				FtpStatus responseStatus = GetResponseStatus();
				ftpResponse.UpdateStatus(responseStatus);
				if (ftpResponse.IsFinal())
				{
					State = RequestState.Finished;
				}
			}
			asyncResult.SetCompleted(synch: false, ftpResponse);
		}

		private void SetType()
		{
			if (binary)
			{
				FtpStatus ftpStatus = SendCommand("TYPE", DataType);
				if (ftpStatus.StatusCode < FtpStatusCode.CommandOK || ftpStatus.StatusCode >= (FtpStatusCode)300)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
			}
		}

		private string GetRemoteFolderPath(Uri uri)
		{
			string text = Uri.UnescapeDataString(uri.LocalPath);
			string text2;
			if (initial_path == null || initial_path == "/")
			{
				text2 = text;
			}
			else
			{
				if (text[0] == '/')
				{
					text = text.Substring(1);
				}
				Uri baseUri = new Uri("ftp://dummy-host" + initial_path);
				text2 = new Uri(baseUri, text).LocalPath;
			}
			int num = text2.LastIndexOf('/');
			if (num == -1)
			{
				return null;
			}
			return text2.Substring(0, num + 1);
		}

		private void CWDAndSetFileName(Uri uri)
		{
			string remoteFolderPath = GetRemoteFolderPath(uri);
			if (remoteFolderPath != null)
			{
				FtpStatus ftpStatus = SendCommand("CWD", remoteFolderPath);
				if (ftpStatus.StatusCode < FtpStatusCode.CommandOK || ftpStatus.StatusCode >= (FtpStatusCode)300)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				int num = uri.LocalPath.LastIndexOf('/');
				if (num >= 0)
				{
					file_name = Uri.UnescapeDataString(uri.LocalPath.Substring(num + 1));
				}
			}
		}

		private void ProcessMethod()
		{
			State = RequestState.Connecting;
			ResolveHost();
			OpenControlConnection();
			CWDAndSetFileName(requestUri);
			SetType();
			switch (method)
			{
			case "RETR":
			case "NLST":
			case "LIST":
				DownloadData();
				break;
			case "APPE":
			case "STOR":
			case "STOU":
				UploadData();
				break;
			case "SIZE":
			case "MDTM":
			case "PWD":
			case "MKD":
			case "RENAME":
			case "DELE":
				ProcessSimpleMethod();
				break;
			default:
				throw new Exception($"Support for command {method} not implemented yet");
			}
			CheckIfAborted();
		}

		private void CloseControlConnection()
		{
			if (controlStream != null)
			{
				SendCommand("QUIT");
				controlStream.Close();
				controlStream = null;
			}
		}

		internal void CloseDataConnection()
		{
			if (origDataStream != null)
			{
				origDataStream.Close();
				origDataStream = null;
			}
		}

		private void CloseConnection()
		{
			CloseControlConnection();
			CloseDataConnection();
		}

		private void ProcessSimpleMethod()
		{
			State = RequestState.TransferInProgress;
			if (method == "PWD")
			{
				method = "PWD";
			}
			if (method == "RENAME")
			{
				method = "RNFR";
			}
			FtpStatus ftpStatus = SendCommand(method, file_name);
			ftpResponse.Stream = Stream.Null;
			string statusDescription = ftpStatus.StatusDescription;
			switch (method)
			{
			case "SIZE":
			{
				if (ftpStatus.StatusCode != FtpStatusCode.FileStatus)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				int num = 4;
				int num2 = 0;
				while (num < statusDescription.Length && char.IsDigit(statusDescription[num]))
				{
					num++;
					num2++;
				}
				if (num2 == 0)
				{
					throw new WebException("Bad format for server response in " + method);
				}
				if (!long.TryParse(statusDescription.Substring(4, num2), out long result))
				{
					throw new WebException("Bad format for server response in " + method);
				}
				ftpResponse.contentLength = result;
				break;
			}
			case "MDTM":
				if (ftpStatus.StatusCode != FtpStatusCode.FileStatus)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				ftpResponse.LastModified = DateTime.ParseExact(statusDescription.Substring(4), "yyyyMMddHHmmss", null);
				break;
			case "MKD":
				if (ftpStatus.StatusCode != FtpStatusCode.PathnameCreated)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "CWD":
				method = "PWD";
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = SendCommand(method);
				if (ftpStatus.StatusCode != FtpStatusCode.PathnameCreated)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "RNFR":
				method = "RENAME";
				if (ftpStatus.StatusCode != FtpStatusCode.FileCommandPending)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = SendCommand("RNTO", (renameTo == null) ? string.Empty : renameTo);
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "DELE":
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				break;
			}
			State = RequestState.Finished;
		}

		private void UploadData()
		{
			State = RequestState.OpeningData;
			OpenDataConnection();
			State = RequestState.TransferInProgress;
			requestStream = new FtpDataStream(this, dataStream, isRead: false);
			asyncResult.Stream = requestStream;
		}

		private void DownloadData()
		{
			State = RequestState.OpeningData;
			OpenDataConnection();
			State = RequestState.TransferInProgress;
			ftpResponse.Stream = new FtpDataStream(this, dataStream, isRead: true);
		}

		private void CheckRequestStarted()
		{
			if (State != 0)
			{
				throw new InvalidOperationException("There is a request currently in progress");
			}
		}

		private void OpenControlConnection()
		{
			Exception innerException = null;
			Socket socket = null;
			IPAddress[] addressList = hostEntry.AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, requestUri.Port);
				if (!ServicePoint.CallEndPointDelegate(socket, iPEndPoint))
				{
					socket.Close();
					socket = null;
					continue;
				}
				try
				{
					socket.Connect(iPEndPoint);
					localEndPoint = (IPEndPoint)socket.LocalEndPoint;
					goto end_IL_005e;
					IL_007c:
					continue;
					end_IL_005e:;
				}
				catch (SocketException ex)
				{
					innerException = ex;
					socket.Close();
					socket = null;
					continue;
				}
				break;
			}
			if (socket == null)
			{
				throw new WebException("Unable to connect to remote server", innerException, WebExceptionStatus.UnknownError, ftpResponse);
			}
			controlStream = new NetworkStream(socket);
			controlReader = new StreamReader(controlStream, Encoding.ASCII);
			State = RequestState.Authenticating;
			Authenticate();
			FtpStatus ftpStatus = SendCommand("OPTS", "utf8", "on");
			ftpStatus = SendCommand("PWD");
			initial_path = GetInitialPath(ftpStatus);
		}

		private static string GetInitialPath(FtpStatus status)
		{
			int statusCode = (int)status.StatusCode;
			if (statusCode < 200 || statusCode > 300 || status.StatusDescription.Length <= 4)
			{
				throw new WebException("Error getting current directory: " + status.StatusDescription, null, WebExceptionStatus.UnknownError, null);
			}
			string text = status.StatusDescription.Substring(4);
			if (text[0] == '"')
			{
				int num = text.IndexOf('"', 1);
				if (num == -1)
				{
					throw new WebException("Error getting current directory: PWD -> " + status.StatusDescription, null, WebExceptionStatus.UnknownError, null);
				}
				text = text.Substring(1, num - 1);
			}
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			return text;
		}

		private Socket SetupPassiveConnection(string statusDescription)
		{
			if (statusDescription.Length < 4)
			{
				throw new WebException("Cannot open passive data connection");
			}
			int i;
			for (i = 3; i < statusDescription.Length && !char.IsDigit(statusDescription[i]); i++)
			{
			}
			if (i >= statusDescription.Length)
			{
				throw new WebException("Cannot open passive data connection");
			}
			string[] array = statusDescription.Substring(i).Split(new char[1]
			{
				','
			}, 6);
			if (array.Length != 6)
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num = array[5].Length - 1;
			while (num >= 0 && !char.IsDigit(array[5][num]))
			{
				num--;
			}
			if (num < 0)
			{
				throw new WebException("Cannot open passive data connection");
			}
			array[5] = array[5].Substring(0, num + 1);
			IPAddress address;
			try
			{
				address = IPAddress.Parse(string.Join(".", array, 0, 4));
			}
			catch (FormatException)
			{
				throw new WebException("Cannot open passive data connection");
				IL_00f9:;
			}
			if (!int.TryParse(array[4], out int result) || !int.TryParse(array[5], out int result2))
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num2 = (result << 8) + result2;
			if (num2 < 0 || num2 > 65535)
			{
				throw new WebException("Cannot open passive data connection");
			}
			IPEndPoint iPEndPoint = new IPEndPoint(address, num2);
			Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				socket.Connect(iPEndPoint);
				return socket;
			}
			catch (SocketException)
			{
				socket.Close();
				throw new WebException("Cannot open passive data connection");
				IL_018b:
				return socket;
			}
		}

		private Exception CreateExceptionFromResponse(FtpStatus status)
		{
			FtpWebResponse response = new FtpWebResponse(this, requestUri, method, status);
			return new WebException("Server returned an error: " + status.StatusDescription, null, WebExceptionStatus.ProtocolError, response);
		}

		internal void SetTransferCompleted()
		{
			if (!InFinalState())
			{
				State = RequestState.Finished;
				FtpStatus responseStatus = GetResponseStatus();
				ftpResponse.UpdateStatus(responseStatus);
				if (!keepAlive)
				{
					CloseConnection();
				}
			}
		}

		internal void OperationCompleted()
		{
			if (!keepAlive)
			{
				CloseConnection();
			}
		}

		private void SetCompleteWithError(Exception exc)
		{
			if (asyncResult != null)
			{
				asyncResult.SetCompleted(synch: false, exc);
			}
		}

		private Socket InitDataConnection()
		{
			FtpStatus ftpStatus;
			if (usePassive)
			{
				ftpStatus = SendCommand("PASV");
				if (ftpStatus.StatusCode != FtpStatusCode.EnteringPassive)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				return SetupPassiveConnection(ftpStatus.StatusDescription);
			}
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				socket.Bind(new IPEndPoint(localEndPoint.Address, 0));
				socket.Listen(1);
			}
			catch (SocketException innerException)
			{
				socket.Close();
				throw new WebException("Couldn't open listening socket on client", innerException);
				IL_0081:;
			}
			IPEndPoint iPEndPoint = (IPEndPoint)socket.LocalEndPoint;
			string text = iPEndPoint.Address.ToString().Replace('.', ',');
			int num = iPEndPoint.Port >> 8;
			int num2 = iPEndPoint.Port % 256;
			string text2 = text + "," + num + "," + num2;
			ftpStatus = SendCommand("PORT", text2);
			if (ftpStatus.StatusCode != FtpStatusCode.CommandOK)
			{
				socket.Close();
				throw CreateExceptionFromResponse(ftpStatus);
			}
			return socket;
		}

		private void OpenDataConnection()
		{
			Socket socket = InitDataConnection();
			FtpStatus ftpStatus;
			if (offset > 0)
			{
				ftpStatus = SendCommand("REST", offset.ToString());
				if (ftpStatus.StatusCode != FtpStatusCode.FileCommandPending)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
			}
			ftpStatus = ((!(method != "NLST") || !(method != "LIST") || !(method != "STOU")) ? SendCommand(method) : SendCommand(method, file_name));
			if (ftpStatus.StatusCode != FtpStatusCode.OpeningData && ftpStatus.StatusCode != FtpStatusCode.DataAlreadyOpen)
			{
				throw CreateExceptionFromResponse(ftpStatus);
			}
			if (usePassive)
			{
				origDataStream = new NetworkStream(socket, owns_socket: true);
				dataStream = origDataStream;
				if (EnableSsl)
				{
					ChangeToSSLSocket(ref dataStream);
				}
			}
			else
			{
				Socket socket2 = null;
				try
				{
					socket2 = socket.Accept();
				}
				catch (SocketException)
				{
					socket.Close();
					socket2?.Close();
					throw new ProtocolViolationException("Server commited a protocol violation.");
					IL_0151:;
				}
				socket.Close();
				origDataStream = new NetworkStream(socket, owns_socket: true);
				dataStream = origDataStream;
				if (EnableSsl)
				{
					ChangeToSSLSocket(ref dataStream);
				}
			}
			ftpResponse.UpdateStatus(ftpStatus);
		}

		private void Authenticate()
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			if (credentials != null)
			{
				text = credentials.UserName;
				text2 = credentials.Password;
				text3 = credentials.Domain;
			}
			if (text == null)
			{
				text = "anonymous";
			}
			if (text2 == null)
			{
				text2 = "@anonymous";
			}
			if (!string.IsNullOrEmpty(text3))
			{
				text = text3 + '\\' + text;
			}
			FtpStatus ftpStatus = GetResponseStatus();
			ftpResponse.BannerMessage = ftpStatus.StatusDescription;
			if (EnableSsl)
			{
				InitiateSecureConnection(ref controlStream);
				controlReader = new StreamReader(controlStream, Encoding.ASCII);
				ftpStatus = SendCommand("PBSZ", "0");
				int statusCode = (int)ftpStatus.StatusCode;
				if (statusCode < 200 || statusCode >= 300)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = SendCommand("PROT", "P");
				statusCode = (int)ftpStatus.StatusCode;
				if (statusCode < 200 || statusCode >= 300)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = new FtpStatus(FtpStatusCode.SendUserCommand, string.Empty);
			}
			if (ftpStatus.StatusCode != FtpStatusCode.SendUserCommand)
			{
				throw CreateExceptionFromResponse(ftpStatus);
			}
			ftpStatus = SendCommand("USER", text);
			switch (ftpStatus.StatusCode)
			{
			case FtpStatusCode.SendPasswordCommand:
				ftpStatus = SendCommand("PASS", text2);
				if (ftpStatus.StatusCode != FtpStatusCode.LoggedInProceed)
				{
					throw CreateExceptionFromResponse(ftpStatus);
				}
				break;
			default:
				throw CreateExceptionFromResponse(ftpStatus);
			case FtpStatusCode.LoggedInProceed:
				break;
			}
			ftpResponse.WelcomeMessage = ftpStatus.StatusDescription;
			ftpResponse.UpdateStatus(ftpStatus);
		}

		private FtpStatus SendCommand(string command, params string[] parameters)
		{
			return SendCommand(waitResponse: true, command, parameters);
		}

		private FtpStatus SendCommand(bool waitResponse, string command, params string[] parameters)
		{
			string str = command;
			if (parameters.Length > 0)
			{
				str = str + " " + string.Join(" ", parameters);
			}
			str += "\r\n";
			byte[] bytes = Encoding.ASCII.GetBytes(str);
			try
			{
				controlStream.Write(bytes, 0, bytes.Length);
			}
			catch (IOException)
			{
				return new FtpStatus(FtpStatusCode.ServiceNotAvailable, "Write failed");
				IL_0065:;
			}
			if (!waitResponse)
			{
				return null;
			}
			FtpStatus responseStatus = GetResponseStatus();
			if (ftpResponse != null)
			{
				ftpResponse.UpdateStatus(responseStatus);
			}
			return responseStatus;
		}

		internal static FtpStatus ServiceNotAvailable()
		{
			return new FtpStatus(FtpStatusCode.ServiceNotAvailable, Locale.GetText("Invalid response from server"));
		}

		internal FtpStatus GetResponseStatus()
		{
			IL_0000:
			string text = null;
			try
			{
				text = controlReader.ReadLine();
			}
			catch (IOException)
			{
			}
			if (text == null || text.Length < 3)
			{
				return ServiceNotAvailable();
			}
			if (!int.TryParse(text.Substring(0, 3), out int result))
			{
				return ServiceNotAvailable();
			}
			if (text.Length > 3 && text[3] == '-')
			{
				string text2 = null;
				string value = result.ToString() + ' ';
				do
				{
					text2 = null;
					try
					{
						text2 = controlReader.ReadLine();
					}
					catch (IOException)
					{
					}
					if (text2 == null)
					{
						return ServiceNotAvailable();
					}
					text = text + Environment.NewLine + text2;
				}
				while (!text2.StartsWith(value, StringComparison.Ordinal));
			}
			return new FtpStatus((FtpStatusCode)result, text);
			IL_00cc:
			goto IL_0000;
		}

		private void InitiateSecureConnection(ref Stream stream)
		{
			FtpStatus ftpStatus = SendCommand("AUTH", "TLS");
			if (ftpStatus.StatusCode != FtpStatusCode.ServerWantsSecureSession)
			{
				throw CreateExceptionFromResponse(ftpStatus);
			}
			ChangeToSSLSocket(ref stream);
		}

		internal bool ChangeToSSLSocket(ref Stream stream)
		{
			SslStream sslStream = new SslStream(stream, leaveStreamOpen: true, callback, null);
			sslStream.AuthenticateAsClient(requestUri.Host, null, SslProtocols.Default, checkCertificateRevocation: false);
			stream = sslStream;
			return true;
		}

		private bool InFinalState()
		{
			return State == RequestState.Aborted || State == RequestState.Error || State == RequestState.Finished;
		}

		private bool InProgress()
		{
			return State != 0 && !InFinalState();
		}

		internal void CheckIfAborted()
		{
			if (State == RequestState.Aborted)
			{
				throw new WebException("Request aborted", WebExceptionStatus.RequestCanceled);
			}
		}

		private void CheckFinalState()
		{
			if (InFinalState())
			{
				throw new InvalidOperationException("Cannot change final state");
			}
		}
	}
}
