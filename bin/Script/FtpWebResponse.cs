using System.IO;

namespace System.Net
{
	/// <summary>Encapsulates a File Transfer Protocol (FTP) server's response to a request.</summary>
	public class FtpWebResponse : WebResponse
	{
		private Stream stream;

		private Uri uri;

		private FtpStatusCode statusCode;

		private DateTime lastModified = DateTime.MinValue;

		private string bannerMessage = string.Empty;

		private string welcomeMessage = string.Empty;

		private string exitMessage = string.Empty;

		private string statusDescription;

		private string method;

		private bool disposed;

		private FtpWebRequest request;

		internal long contentLength = -1L;

		/// <summary>Gets the length of the data received from the FTP server.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that contains the number of bytes of data received from the FTP server. </returns>
		public override long ContentLength => contentLength;

		/// <summary>Gets an empty <see cref="T:System.Net.WebHeaderCollection" /> object.</summary>
		/// <returns>An empty <see cref="T:System.Net.WebHeaderCollection" /> object.</returns>
		public override WebHeaderCollection Headers => new WebHeaderCollection();

		/// <summary>Gets the URI that sent the response to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> instance that identifies the resource associated with this response.</returns>
		public override Uri ResponseUri => uri;

		/// <summary>Gets the date and time that a file on an FTP server was last modified.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the last modified date and time for a file.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime LastModified
		{
			get
			{
				return lastModified;
			}
			internal set
			{
				lastModified = value;
			}
		}

		/// <summary>Gets the message sent by the FTP server when a connection is established prior to logon.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the banner message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string BannerMessage
		{
			get
			{
				return bannerMessage;
			}
			internal set
			{
				bannerMessage = value;
			}
		}

		/// <summary>Gets the message sent by the FTP server when authentication is complete.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the welcome message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string WelcomeMessage
		{
			get
			{
				return welcomeMessage;
			}
			internal set
			{
				welcomeMessage = value;
			}
		}

		/// <summary>Gets the message sent by the server when the FTP session is ending.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the exit message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string ExitMessage
		{
			get
			{
				return exitMessage;
			}
			internal set
			{
				exitMessage = value;
			}
		}

		/// <summary>Gets the most recent status code sent from the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.FtpStatusCode" /> value that indicates the most recent status code returned with this response.</returns>
		public FtpStatusCode StatusCode
		{
			get
			{
				return statusCode;
			}
			private set
			{
				statusCode = value;
			}
		}

		/// <summary>Gets text that describes a status code sent from the FTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> instance that contains the status code and message returned with this response.</returns>
		public string StatusDescription
		{
			get
			{
				return statusDescription;
			}
			private set
			{
				statusDescription = value;
			}
		}

		internal Stream Stream
		{
			get
			{
				return stream;
			}
			set
			{
				stream = value;
			}
		}

		internal FtpWebResponse(FtpWebRequest request, Uri uri, string method, bool keepAlive)
		{
			this.request = request;
			this.uri = uri;
			this.method = method;
		}

		internal FtpWebResponse(FtpWebRequest request, Uri uri, string method, FtpStatusCode statusCode, string statusDescription)
		{
			this.request = request;
			this.uri = uri;
			this.method = method;
			this.statusCode = statusCode;
			this.statusDescription = statusDescription;
		}

		internal FtpWebResponse(FtpWebRequest request, Uri uri, string method, FtpStatus status)
			: this(request, uri, method, status.StatusCode, status.StatusDescription)
		{
		}

		/// <summary>Frees the resources held by the response.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Close()
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			if (stream != null)
			{
				stream.Close();
				if (stream == Stream.Null)
				{
					request.OperationCompleted();
				}
			}
			stream = null;
		}

		/// <summary>Retrieves the stream that contains response data sent from an FTP server.</summary>
		/// <returns>A readable <see cref="T:System.IO.Stream" /> instance that contains data returned with the response; otherwise, <see cref="F:System.IO.Stream.Null" /> if no response data was returned by the server.</returns>
		/// <exception cref="T:System.InvalidOperationException">The response did not return a data stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream GetResponseStream()
		{
			if (stream == null)
			{
				return Stream.Null;
			}
			if (method != "RETR" && method != "NLST")
			{
				CheckDisposed();
			}
			return stream;
		}

		internal void UpdateStatus(FtpStatus status)
		{
			statusCode = status.StatusCode;
			statusDescription = status.StatusDescription;
		}

		private void CheckDisposed()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
		}

		internal bool IsFinal()
		{
			return statusCode >= FtpStatusCode.CommandOK;
		}
	}
}
