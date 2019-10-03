using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides common methods for sending data to and receiving data from a resource identified by a URI.</summary>
	[ComVisible(true)]
	public class WebClient : Component
	{
		private static readonly string urlEncodedCType;

		private static byte[] hexBytes;

		private ICredentials credentials;

		private WebHeaderCollection headers;

		private WebHeaderCollection responseHeaders;

		private Uri baseAddress;

		private string baseString;

		private NameValueCollection queryString;

		private bool is_busy;

		private bool async;

		private Thread async_thread;

		private Encoding encoding = Encoding.Default;

		private IWebProxy proxy;

		/// <summary>Gets or sets the base URI for requests made by a <see cref="T:System.Net.WebClient" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the base URI for requests made by a <see cref="T:System.Net.WebClient" /> or <see cref="F:System.String.Empty" /> if no base address has been specified.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.WebClient.BaseAddress" /> is set to an invalid URI. The inner exception may contain information that will help you locate the error.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string BaseAddress
		{
			get
			{
				if (baseString == null && baseAddress == null)
				{
					return string.Empty;
				}
				baseString = baseAddress.ToString();
				return baseString;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					baseAddress = null;
				}
				else
				{
					baseAddress = new Uri(value);
				}
			}
		}

		/// <summary>Gets or sets the application's cache policy for any resources obtained by this WebClient instance using <see cref="T:System.Net.WebRequest" /> objects.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCachePolicy" /> object that represents the application's caching requirements.</returns>
		[MonoTODO]
		public RequestCachePolicy CachePolicy
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

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		[MonoTODO]
		public bool UseDefaultCredentials
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

		/// <summary>Gets or sets the network credentials that are sent to the host and used to authenticate the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> containing the authentication credentials for the request. The default is null.</returns>
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

		/// <summary>Gets or sets a collection of header name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with this request.</returns>
		public WebHeaderCollection Headers
		{
			get
			{
				if (headers == null)
				{
					headers = new WebHeaderCollection();
				}
				return headers;
			}
			set
			{
				headers = value;
			}
		}

		/// <summary>Gets or sets a collection of query name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains query name/value pairs associated with the request. If no pairs are associated with the request, the value is an empty <see cref="T:System.Collections.Specialized.NameValueCollection" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public NameValueCollection QueryString
		{
			get
			{
				if (queryString == null)
				{
					queryString = new NameValueCollection();
				}
				return queryString;
			}
			set
			{
				queryString = value;
			}
		}

		/// <summary>Gets a collection of header name/value pairs associated with the response.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with the response, or null if no response has been received.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public WebHeaderCollection ResponseHeaders => responseHeaders;

		/// <summary>Gets and sets the <see cref="T:System.Text.Encoding" /> used to upload and download strings.</summary>
		/// <returns>A <see cref="T:System.Text.Encoding" /> that is used to encode strings. The default value of this property is the encoding returned by <see cref="P:System.Text.Encoding.Default" />.</returns>
		public Encoding Encoding
		{
			get
			{
				return encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				encoding = value;
			}
		}

		/// <summary>Gets or sets the proxy used by this <see cref="T:System.Net.WebClient" /> object.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> instance used to send requests.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.WebClient.Proxy" /> is set to null. </exception>
		public IWebProxy Proxy
		{
			get
			{
				return proxy;
			}
			set
			{
				proxy = value;
			}
		}

		/// <summary>Gets whether a Web request is in progress.</summary>
		/// <returns>true if the Web request is still in progress; otherwise false.</returns>
		public bool IsBusy => is_busy;

		/// <summary>Occurs when an asynchronous data download operation completes.</summary>
		public event DownloadDataCompletedEventHandler DownloadDataCompleted;

		/// <summary>Occurs when an asynchronous file download operation completes.</summary>
		public event AsyncCompletedEventHandler DownloadFileCompleted;

		/// <summary>Occurs when an asynchronous download operation successfully transfers some or all of the data.</summary>
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		/// <summary>Occurs when an asynchronous resource-download operation completes.</summary>
		public event DownloadStringCompletedEventHandler DownloadStringCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream containing a resource completes.</summary>
		public event OpenReadCompletedEventHandler OpenReadCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream to write data to a resource completes.</summary>
		public event OpenWriteCompletedEventHandler OpenWriteCompleted;

		/// <summary>Occurs when an asynchronous data-upload operation completes.</summary>
		public event UploadDataCompletedEventHandler UploadDataCompleted;

		/// <summary>Occurs when an asynchronous file-upload operation completes.</summary>
		public event UploadFileCompletedEventHandler UploadFileCompleted;

		/// <summary>Occurs when an asynchronous upload operation successfully transfers some or all of the data.</summary>
		public event UploadProgressChangedEventHandler UploadProgressChanged;

		/// <summary>Occurs when an asynchronous string-upload operation completes.</summary>
		public event UploadStringCompletedEventHandler UploadStringCompleted;

		/// <summary>Occurs when an asynchronous upload of a name/value collection completes.</summary>
		public event UploadValuesCompletedEventHandler UploadValuesCompleted;

		static WebClient()
		{
			urlEncodedCType = "application/x-www-form-urlencoded";
			hexBytes = new byte[16];
			int num = 0;
			int num2 = 48;
			while (num2 <= 57)
			{
				hexBytes[num] = (byte)num2;
				num2++;
				num++;
			}
			int num3 = 97;
			while (num3 <= 102)
			{
				hexBytes[num] = (byte)num3;
				num3++;
				num++;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		private void CheckBusy()
		{
			if (IsBusy)
			{
				throw new NotSupportedException("WebClient does not support conccurent I/O operations.");
			}
		}

		private void SetBusy()
		{
			lock (this)
			{
				CheckBusy();
				is_busy = true;
			}
		}

		/// <summary>Downloads the resource with the specified URI as a <see cref="T:System.Byte" /> array.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <param name="address">The URI from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] DownloadData(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return DownloadData(CreateUri(address));
		}

		/// <summary>Downloads the resource with the specified URI as a <see cref="T:System.Byte" /> array.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <param name="address">The URI represented by the <see cref="T:System.Uri" />  object, from which to download data.</param>
		public byte[] DownloadData(Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			try
			{
				SetBusy();
				async = false;
				return DownloadDataCore(address, null);
				IL_0032:
				byte[] result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		private byte[] DownloadDataCore(Uri address, object userToken)
		{
			WebRequest webRequest = null;
			try
			{
				webRequest = SetupRequest(address);
				WebResponse webResponse = GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				return ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
				IL_002f:
				byte[] result;
				return result;
			}
			catch (ThreadInterruptedException)
			{
				webRequest?.Abort();
				throw;
				IL_0043:
				byte[] result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_004b:
				byte[] result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_005f:
				byte[] result;
				return result;
			}
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI from which to download data. </param>
		/// <param name="fileName">The name of the local file that is to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="filename" /> is null or <see cref="F:System.String.Empty" />.-or-The file does not exist.-or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void DownloadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			DownloadFile(CreateUri(address), fileName);
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI specified as a <see cref="T:System.String" />, from which to download data. </param>
		/// <param name="fileName">The name of the local file that is to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="filename" /> is null or <see cref="F:System.String.Empty" />.-or- The file does not exist. -or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		public void DownloadFile(Uri address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			try
			{
				SetBusy();
				async = false;
				DownloadFileCore(address, fileName, null);
			}
			catch (WebException)
			{
				throw;
				IL_0046:;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_0058:;
			}
			finally
			{
				is_busy = false;
			}
		}

		private void DownloadFileCore(Uri address, string fileName, object userToken)
		{
			WebRequest webRequest = null;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				try
				{
					webRequest = SetupRequest(address);
					WebResponse webResponse = GetWebResponse(webRequest);
					Stream responseStream = webResponse.GetResponseStream();
					int num = (int)webResponse.ContentLength;
					int num2 = (num > -1 && num <= 32768) ? num : 32768;
					byte[] array = new byte[num2];
					int num3 = 0;
					long num4 = 0L;
					while ((num3 = responseStream.Read(array, 0, num2)) != 0)
					{
						if (async)
						{
							num4 += num3;
							OnDownloadProgressChanged(new DownloadProgressChangedEventArgs(num4, webResponse.ContentLength, userToken));
						}
						fileStream.Write(array, 0, num3);
					}
				}
				catch (ThreadInterruptedException)
				{
					webRequest?.Abort();
					throw;
					IL_00ba:;
				}
			}
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <param name="address">The URI specified as a <see cref="T:System.String" /> from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenRead(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return OpenRead(CreateUri(address));
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.Uri" /></summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <param name="address">The URI specified as a <see cref="T:System.Uri" /> from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		public Stream OpenRead(Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			WebRequest webRequest = null;
			try
			{
				SetBusy();
				async = false;
				webRequest = SetupRequest(address);
				WebResponse webResponse = GetWebResponse(webRequest);
				return webResponse.GetResponseStream();
				IL_0043:
				Stream result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_004b:
				Stream result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_005d:
				Stream result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenWrite(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return OpenWrite(CreateUri(address));
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenWrite(string address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return OpenWrite(CreateUri(address), method);
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public Stream OpenWrite(Uri address)
		{
			return OpenWrite(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, by using the specified method.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public Stream OpenWrite(Uri address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			try
			{
				SetBusy();
				async = false;
				WebRequest webRequest = SetupRequest(address, method, is_upload: true);
				return webRequest.GetRequestStream();
				IL_003a:
				Stream result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_0042:
				Stream result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_0054:
				Stream result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		private string DetermineMethod(Uri address, string method, bool is_upload)
		{
			if (method != null)
			{
				return method;
			}
			if (address.Scheme == Uri.UriSchemeFtp)
			{
				return (!is_upload) ? "RETR" : "STOR";
			}
			return (!is_upload) ? "GET" : "POST";
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null. -or-An error occurred while sending the data.-or- There was no response from the server hosting the resource. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadData(string address, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return UploadData(CreateUri(address), data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while uploading the data.-or- There was no response from the server hosting the resource. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadData(string address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return UploadData(CreateUri(address), method, data);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null. -or-An error occurred while sending the data.-or- There was no response from the server hosting the resource. </exception>
		public byte[] UploadData(Uri address, byte[] data)
		{
			return UploadData(address, null, data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while uploading the data.-or- There was no response from the server hosting the resource. </exception>
		public byte[] UploadData(Uri address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			try
			{
				SetBusy();
				async = false;
				return UploadDataCore(address, method, data, null);
				IL_0045:
				byte[] result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_004d:
				byte[] result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_005f:
				byte[] result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		private byte[] UploadDataCore(Uri address, string method, byte[] data, object userToken)
		{
			WebRequest webRequest = SetupRequest(address, method, is_upload: true);
			try
			{
				int num = data.Length;
				webRequest.ContentLength = num;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(data, 0, num);
				}
				WebResponse webResponse = GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				return ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
				IL_0060:
				byte[] result;
				return result;
			}
			catch (ThreadInterruptedException)
			{
				webRequest?.Abort();
				throw;
				IL_0074:
				byte[] result;
				return result;
			}
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return UploadFile(CreateUri(address), fileName);
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public byte[] UploadFile(Uri address, string fileName)
		{
			return UploadFile(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadFile(string address, string method, string fileName)
		{
			return UploadFile(CreateUri(address), method, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public byte[] UploadFile(Uri address, string method, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			try
			{
				SetBusy();
				async = false;
				return UploadFileCore(address, method, fileName, null);
				IL_0045:
				byte[] result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_004d:
				byte[] result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_005f:
				byte[] result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		private byte[] UploadFileCore(Uri address, string method, string fileName, object userToken)
		{
			string text = Headers["Content-Type"];
			if (text != null)
			{
				string text2 = text.ToLower();
				if (text2.StartsWith("multipart/"))
				{
					throw new WebException("Content-Type cannot be set to a multipart type for this request.");
				}
			}
			else
			{
				text = "application/octet-stream";
			}
			string text3 = "------------" + DateTime.Now.Ticks.ToString("x");
			Headers["Content-Type"] = $"multipart/form-data; boundary={text3}";
			Stream stream = null;
			Stream stream2 = null;
			byte[] array = null;
			fileName = Path.GetFullPath(fileName);
			WebRequest webRequest = null;
			try
			{
				stream2 = File.OpenRead(fileName);
				webRequest = SetupRequest(address, method, is_upload: true);
				stream = webRequest.GetRequestStream();
				byte[] bytes = Encoding.ASCII.GetBytes("--" + text3 + "\r\n");
				stream.Write(bytes, 0, bytes.Length);
				string s = $"Content-Disposition: form-data; name=\"file\"; filename=\"{Path.GetFileName(fileName)}\"\r\nContent-Type: {text}\r\n\r\n";
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				stream.Write(bytes2, 0, bytes2.Length);
				byte[] buffer = new byte[4096];
				int count;
				while ((count = stream2.Read(buffer, 0, 4096)) != 0)
				{
					stream.Write(buffer, 0, count);
				}
				stream.WriteByte(13);
				stream.WriteByte(10);
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
				stream = null;
				WebResponse webResponse = GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				return ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
			}
			catch (ThreadInterruptedException)
			{
				webRequest?.Abort();
				throw;
				IL_01a0:
				return array;
			}
			finally
			{
				stream2?.Close();
				stream?.Close();
			}
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- There was no response from the server hosting the resource.-or- An error occurred while opening the stream.-or- The Content-type header is not null or "application/x-www-form-urlencoded". </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadValues(string address, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return UploadValues(CreateUri(address), data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header value is not null and is not application/x-www-form-urlencoded. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadValues(string address, string method, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return UploadValues(CreateUri(address), method, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- There was no response from the server hosting the resource.-or- An error occurred while opening the stream.-or- The Content-type header is not null or "application/x-www-form-urlencoded". </exception>
		public byte[] UploadValues(Uri address, NameValueCollection data)
		{
			return UploadValues(address, null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header value is not null and is not application/x-www-form-urlencoded. </exception>
		public byte[] UploadValues(Uri address, string method, NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			try
			{
				SetBusy();
				async = false;
				return UploadValuesCore(address, method, data, null);
				IL_0045:
				byte[] result;
				return result;
			}
			catch (WebException)
			{
				throw;
				IL_004d:
				byte[] result;
				return result;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
				IL_005f:
				byte[] result;
				return result;
			}
			finally
			{
				is_busy = false;
			}
		}

		private byte[] UploadValuesCore(Uri uri, string method, NameValueCollection data, object userToken)
		{
			string text = Headers["Content-Type"];
			if (text != null && string.Compare(text, urlEncodedCType, ignoreCase: true) != 0)
			{
				throw new WebException("Content-Type header cannot be changed from its default value for this request.");
			}
			Headers["Content-Type"] = urlEncodedCType;
			WebRequest webRequest = SetupRequest(uri, method, is_upload: true);
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				foreach (string datum in data)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(datum);
					UrlEncodeAndWrite(memoryStream, bytes);
					memoryStream.WriteByte(61);
					bytes = Encoding.UTF8.GetBytes(data[datum]);
					UrlEncodeAndWrite(memoryStream, bytes);
					memoryStream.WriteByte(38);
				}
				int num = (int)memoryStream.Length;
				if (num > 0)
				{
					memoryStream.SetLength(--num);
				}
				byte[] buffer = memoryStream.GetBuffer();
				webRequest.ContentLength = num;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(buffer, 0, num);
				}
				memoryStream.Close();
				WebResponse webResponse = GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				return ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
				IL_0162:
				byte[] result;
				return result;
			}
			catch (ThreadInterruptedException)
			{
				webRequest.Abort();
				throw;
				IL_0170:
				byte[] result;
				return result;
			}
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.String" /> containing the URI.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <param name="address">A <see cref="T:System.String" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string DownloadString(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return encoding.GetString(DownloadData(CreateUri(address)));
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.Uri" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> object containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		public string DownloadString(Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return encoding.GetString(DownloadData(CreateUri(address)));
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string UploadString(string address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = UploadData(address, encoding.GetBytes(data));
			return encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the file. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method. </param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string UploadString(string address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = UploadData(address, method, encoding.GetBytes(data));
			return encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public string UploadString(Uri address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = UploadData(address, encoding.GetBytes(data));
			return encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the file. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method. </param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public string UploadString(Uri address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = UploadData(address, method, encoding.GetBytes(data));
			return encoding.GetString(bytes);
		}

		private Uri CreateUri(string address)
		{
			return MakeUri(address);
		}

		private Uri CreateUri(Uri address)
		{
			string query = address.Query;
			if (string.IsNullOrEmpty(query))
			{
				query = GetQueryString(add_qmark: true);
			}
			if (baseAddress == null && query == null)
			{
				return address;
			}
			if (baseAddress == null)
			{
				return new Uri(address.ToString() + query, query != null);
			}
			if (query == null)
			{
				return new Uri(baseAddress, address.ToString());
			}
			return new Uri(baseAddress, address.ToString() + query, query != null);
		}

		private string GetQueryString(bool add_qmark)
		{
			if (queryString == null || queryString.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (add_qmark)
			{
				stringBuilder.Append('?');
			}
			foreach (string item in queryString)
			{
				stringBuilder.AppendFormat("{0}={1}&", item, UrlEncode(queryString[item]));
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Length--;
			}
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		private Uri MakeUri(string path)
		{
			string text = GetQueryString(add_qmark: true);
			if (baseAddress == null && text == null)
			{
				try
				{
					return new Uri(path);
					IL_002b:;
				}
				catch (ArgumentNullException)
				{
					if (Environment.UnityWebSecurityEnabled)
					{
						throw;
					}
					path = Path.GetFullPath(path);
					return new Uri("file://" + path);
					IL_005b:;
				}
				catch (UriFormatException)
				{
					if (Environment.UnityWebSecurityEnabled)
					{
						throw;
					}
					path = Path.GetFullPath(path);
					return new Uri("file://" + path);
					IL_008b:;
				}
			}
			if (baseAddress == null)
			{
				return new Uri(path + text, text != null);
			}
			if (text == null)
			{
				return new Uri(baseAddress, path);
			}
			return new Uri(baseAddress, path + text, text != null);
		}

		private WebRequest SetupRequest(Uri uri)
		{
			WebRequest webRequest = GetWebRequest(uri);
			if (Proxy != null)
			{
				webRequest.Proxy = Proxy;
			}
			webRequest.Credentials = credentials;
			if (headers != null && headers.Count != 0 && webRequest is HttpWebRequest)
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)webRequest;
				string text = headers["Expect"];
				string text2 = headers["Content-Type"];
				string text3 = headers["Accept"];
				string text4 = headers["Connection"];
				string text5 = headers["User-Agent"];
				string text6 = headers["Referer"];
				headers.RemoveInternal("Expect");
				headers.RemoveInternal("Content-Type");
				headers.RemoveInternal("Accept");
				headers.RemoveInternal("Connection");
				headers.RemoveInternal("Referer");
				headers.RemoveInternal("User-Agent");
				webRequest.Headers = headers;
				if (text != null && text.Length > 0)
				{
					httpWebRequest.Expect = text;
				}
				if (text3 != null && text3.Length > 0)
				{
					httpWebRequest.Accept = text3;
				}
				if (text2 != null && text2.Length > 0)
				{
					httpWebRequest.ContentType = text2;
				}
				if (text4 != null && text4.Length > 0)
				{
					httpWebRequest.Connection = text4;
				}
				if (text5 != null && text5.Length > 0)
				{
					httpWebRequest.UserAgent = text5;
				}
				if (text6 != null && text6.Length > 0)
				{
					httpWebRequest.Referer = text6;
				}
			}
			responseHeaders = null;
			return webRequest;
		}

		private WebRequest SetupRequest(Uri uri, string method, bool is_upload)
		{
			WebRequest webRequest = SetupRequest(uri);
			webRequest.Method = DetermineMethod(uri, method, is_upload);
			return webRequest;
		}

		private byte[] ReadAll(Stream stream, int length, object userToken)
		{
			MemoryStream memoryStream = null;
			bool flag = length == -1;
			int num = (!flag) ? length : 8192;
			if (flag)
			{
				memoryStream = new MemoryStream();
			}
			int num2 = 0;
			int num3 = 0;
			byte[] array = new byte[num];
			while ((num2 = stream.Read(array, num3, num)) != 0)
			{
				if (flag)
				{
					memoryStream.Write(array, 0, num2);
				}
				else
				{
					num3 += num2;
					num -= num2;
				}
				if (async)
				{
					OnDownloadProgressChanged(new DownloadProgressChangedEventArgs(num2, length, userToken));
				}
			}
			if (flag)
			{
				return memoryStream.ToArray();
			}
			return array;
		}

		private string UrlEncode(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				if (c == ' ')
				{
					stringBuilder.Append('+');
				}
				else if ((c < '0' && c != '-' && c != '.') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a' && c != '_') || c > 'z')
				{
					stringBuilder.Append('%');
					int num = (int)c >> 4;
					stringBuilder.Append((char)hexBytes[num]);
					num = (c & 0xF);
					stringBuilder.Append((char)hexBytes[num]);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static void UrlEncodeAndWrite(Stream stream, byte[] bytes)
		{
			if (bytes == null)
			{
				return;
			}
			int num = bytes.Length;
			if (num == 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				char c = (char)bytes[i];
				if (c == ' ')
				{
					stream.WriteByte(43);
				}
				else if ((c < '0' && c != '-' && c != '.') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a' && c != '_') || c > 'z')
				{
					stream.WriteByte(37);
					int num2 = (int)c >> 4;
					stream.WriteByte(hexBytes[num2]);
					num2 = (c & 0xF);
					stream.WriteByte(hexBytes[num2]);
				}
				else
				{
					stream.WriteByte((byte)c);
				}
			}
		}

		/// <summary>Cancels a pending asynchronous operation.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void CancelAsync()
		{
			lock (this)
			{
				if (async_thread != null)
				{
					Thread thread = async_thread;
					CompleteAsync();
					thread.Interrupt();
				}
			}
		}

		private void CompleteAsync()
		{
			lock (this)
			{
				is_busy = false;
				async_thread = null;
			}
		}

		/// <summary>Downloads the specified resource as a <see cref="T:System.Byte" /> array. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadDataAsync(Uri address)
		{
			DownloadDataAsync(address, null);
		}

		/// <summary>Downloads the specified resource as a <see cref="T:System.Byte" /> array. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadDataAsync(Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = DownloadDataCore((Uri)array[0], array[1]);
						OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(result, null, cancelled: false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(null, null, cancelled: true, array[1]));
						throw;
						IL_0043:;
					}
					catch (Exception error)
					{
						OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(null, error, cancelled: false, array[1]));
					}
				});
				object[] parameter = new object[2]
				{
					address,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download. </param>
		/// <param name="fileName">The name of the file to be placed on the local computer. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		public void DownloadFileAsync(Uri address, string fileName)
		{
			DownloadFileAsync(address, fileName, null);
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download. </param>
		/// <param name="fileName">The name of the file to be placed on the local computer. </param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		public void DownloadFileAsync(Uri address, string fileName, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						DownloadFileCore((Uri)array[0], (string)array[1], array[2]);
						OnDownloadFileCompleted(new AsyncCompletedEventArgs(null, cancelled: false, array[2]));
					}
					catch (ThreadInterruptedException)
					{
						OnDownloadFileCompleted(new AsyncCompletedEventArgs(null, cancelled: true, array[2]));
					}
					catch (Exception error)
					{
						OnDownloadFileCompleted(new AsyncCompletedEventArgs(error, cancelled: false, array[2]));
					}
				});
				object[] parameter = new object[3]
				{
					address,
					fileName,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Downloads the resource specified as a <see cref="T:System.Uri" />. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadStringAsync(Uri address)
		{
			DownloadStringAsync(address, null);
		}

		/// <summary>Downloads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadStringAsync(Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						string @string = encoding.GetString(DownloadDataCore((Uri)array[0], array[1]));
						OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(@string, null, cancelled: false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(null, null, cancelled: true, array[1]));
					}
					catch (Exception error)
					{
						OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(null, error, cancelled: false, array[1]));
					}
				});
				object[] parameter = new object[2]
				{
					address,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.-or- An error occurred while downloading the resource. -or- An error occurred while opening the stream.</exception>
		public void OpenReadAsync(Uri address)
		{
			OpenReadAsync(address, null);
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.-or- An error occurred while downloading the resource. -or- An error occurred while opening the stream.</exception>
		public void OpenReadAsync(Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					WebRequest webRequest = null;
					try
					{
						webRequest = SetupRequest((Uri)array[0]);
						WebResponse webResponse = GetWebResponse(webRequest);
						Stream responseStream = webResponse.GetResponseStream();
						OnOpenReadCompleted(new OpenReadCompletedEventArgs(responseStream, null, cancelled: false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						webRequest?.Abort();
						OnOpenReadCompleted(new OpenReadCompletedEventArgs(null, null, cancelled: true, array[1]));
					}
					catch (Exception error)
					{
						OnOpenReadCompleted(new OpenReadCompletedEventArgs(null, error, cancelled: false, array[1]));
					}
				});
				object[] parameter = new object[2]
				{
					address,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		public void OpenWriteAsync(Uri address)
		{
			OpenWriteAsync(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		public void OpenWriteAsync(Uri address, string method)
		{
			OpenWriteAsync(address, method, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public void OpenWriteAsync(Uri address, string method, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					WebRequest webRequest = null;
					try
					{
						webRequest = SetupRequest((Uri)array[0], (string)array[1], is_upload: true);
						Stream requestStream = webRequest.GetRequestStream();
						OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(requestStream, null, cancelled: false, array[2]));
					}
					catch (ThreadInterruptedException)
					{
						webRequest?.Abort();
						OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(null, null, cancelled: true, array[2]));
					}
					catch (Exception error)
					{
						OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(null, error, cancelled: false, array[2]));
					}
				});
				object[] parameter = new object[3]
				{
					address,
					method,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(Uri address, byte[] data)
		{
			UploadDataAsync(address, null, data);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(Uri address, string method, byte[] data)
		{
			UploadDataAsync(address, method, data, null);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = UploadDataCore((Uri)array[0], (string)array[1], (byte[])array[2], array[3]);
						OnUploadDataCompleted(new UploadDataCompletedEventArgs(result, null, cancelled: false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						OnUploadDataCompleted(new UploadDataCompletedEventArgs(null, null, cancelled: true, array[3]));
					}
					catch (Exception error)
					{
						OnUploadDataCompleted(new UploadDataCompletedEventArgs(null, error, cancelled: false, array[3]));
					}
				});
				object[] parameter = new object[4]
				{
					address,
					method,
					data,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(Uri address, string fileName)
		{
			UploadFileAsync(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(Uri address, string method, string fileName)
		{
			UploadFileAsync(address, method, fileName, null);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(Uri address, string method, string fileName, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			lock (this)
			{
				SetBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = UploadFileCore((Uri)array[0], (string)array[1], (string)array[2], array[3]);
						OnUploadFileCompleted(new UploadFileCompletedEventArgs(result, null, cancelled: false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						OnUploadFileCompleted(new UploadFileCompletedEventArgs(null, null, cancelled: true, array[3]));
					}
					catch (Exception error)
					{
						OnUploadFileCompleted(new UploadFileCompletedEventArgs(null, error, cancelled: false, array[3]));
					}
				});
				object[] parameter = new object[4]
				{
					address,
					method,
					fileName,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(Uri address, string data)
		{
			UploadStringAsync(address, null, data);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(Uri address, string method, string data)
		{
			UploadStringAsync(address, method, data, null);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(Uri address, string method, string data, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			lock (this)
			{
				CheckBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						string result = UploadString((Uri)array[0], (string)array[1], (string)array[2]);
						OnUploadStringCompleted(new UploadStringCompletedEventArgs(result, null, cancelled: false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						OnUploadStringCompleted(new UploadStringCompletedEventArgs(null, null, cancelled: true, array[3]));
					}
					catch (Exception error)
					{
						OnUploadStringCompleted(new UploadStringCompletedEventArgs(null, error, cancelled: false, array[3]));
					}
				});
				object[] parameter = new object[4]
				{
					address,
					method,
					data,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the default method. See remarks.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadValuesAsync(Uri address, NameValueCollection values)
		{
			UploadValuesAsync(address, null, values);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null. -or- <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public void UploadValuesAsync(Uri address, string method, NameValueCollection values)
		{
			UploadValuesAsync(address, method, values, null);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null. -or- <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public void UploadValuesAsync(Uri address, string method, NameValueCollection values, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			lock (this)
			{
				CheckBusy();
				async = true;
				async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = UploadValuesCore((Uri)array[0], (string)array[1], (NameValueCollection)array[2], array[3]);
						OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(result, null, cancelled: false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(null, null, cancelled: true, array[3]));
					}
					catch (Exception error)
					{
						OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(null, error, cancelled: false, array[3]));
					}
				});
				object[] parameter = new object[4]
				{
					address,
					method,
					values,
					userToken
				};
				async_thread.Start(parameter);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadDataCompletedEventArgs" /> object that contains event data.</param>
		protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.DownloadDataCompleted != null)
			{
				this.DownloadDataCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.DownloadFileCompleted != null)
			{
				this.DownloadFileCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadProgressChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadProgressChangedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			if (this.DownloadProgressChanged != null)
			{
				this.DownloadProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadStringCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadStringCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.DownloadStringCompleted != null)
			{
				this.DownloadStringCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenReadCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenReadCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.OpenReadCompleted != null)
			{
				this.OpenReadCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenWriteCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenWriteCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.OpenWriteCompleted != null)
			{
				this.OpenWriteCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadDataCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.UploadDataCompleted != null)
			{
				this.UploadDataCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadFileCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.UploadFileCompleted != null)
			{
				this.UploadFileCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadProgressChangedEventArgs" /> object containing event data.</param>
		protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
		{
			if (this.UploadProgressChanged != null)
			{
				this.UploadProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadStringCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadStringCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.UploadStringCompleted != null)
			{
				this.UploadStringCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadValuesCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadValuesCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs args)
		{
			CompleteAsync();
			if (this.UploadValuesCompleted != null)
			{
				this.UploadValuesCompleted(this, args);
			}
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" /> using the specified <see cref="T:System.IAsyncResult" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response.</param>
		/// <param name="result">An <see cref="T:System.IAsyncResult" /> object obtained from a previous call to <see cref="M:System.Net.WebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> .</param>
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			responseHeaders = webResponse.Headers;
			return webResponse;
		}

		/// <summary>Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.</summary>
		/// <returns>A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
		protected virtual WebRequest GetWebRequest(Uri address)
		{
			return WebRequest.Create(address);
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response. </param>
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = request.GetResponse();
			responseHeaders = response.Headers;
			return response;
		}
	}
}
