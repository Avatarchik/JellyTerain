using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net.Mail
{
	/// <summary>Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).</summary>
	public class SmtpClient
	{
		[Flags]
		private enum AuthMechs
		{
			None = 0x0,
			CramMD5 = 0x1,
			DigestMD5 = 0x2,
			GssAPI = 0x4,
			Kerberos4 = 0x8,
			Login = 0x10,
			Plain = 0x20
		}

		private class CancellationException : Exception
		{
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct HeaderName
		{
			public const string ContentTransferEncoding = "Content-Transfer-Encoding";

			public const string ContentType = "Content-Type";

			public const string Bcc = "Bcc";

			public const string Cc = "Cc";

			public const string From = "From";

			public const string Subject = "Subject";

			public const string To = "To";

			public const string MimeVersion = "MIME-Version";

			public const string MessageId = "Message-ID";

			public const string Priority = "Priority";

			public const string Importance = "Importance";

			public const string XPriority = "X-Priority";

			public const string Date = "Date";
		}

		private struct SmtpResponse
		{
			public SmtpStatusCode StatusCode;

			public string Description;

			public static SmtpResponse Parse(string line)
			{
				SmtpResponse result = default(SmtpResponse);
				if (line.Length < 4)
				{
					throw new SmtpException("Response is to short " + line.Length + ".");
				}
				if (line[3] != ' ' && line[3] != '-')
				{
					throw new SmtpException("Response format is wrong.(" + line + ")");
				}
				result.StatusCode = (SmtpStatusCode)int.Parse(line.Substring(0, 3));
				result.Description = line;
				return result;
			}
		}

		private string host;

		private int port;

		private int timeout = 100000;

		private ICredentialsByHost credentials;

		private string pickupDirectoryLocation;

		private SmtpDeliveryMethod deliveryMethod;

		private bool enableSsl;

		private X509CertificateCollection clientCertificates;

		private TcpClient client;

		private Stream stream;

		private StreamWriter writer;

		private StreamReader reader;

		private int boundaryIndex;

		private MailAddress defaultFrom;

		private MailMessage messageInProcess;

		private BackgroundWorker worker;

		private object user_async_state;

		private AuthMechs authMechs;

		private Mutex mutex = new Mutex();

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

		/// <summary>Specify which certificates should be used to establish the Secure Sockets Layer (SSL) connection.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" />, holding one or more client certificates. The default value is derived from the mail configuration attributes in a configuration file.</returns>
		[MonoTODO("Client certificates not used")]
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (clientCertificates == null)
				{
					clientCertificates = new X509CertificateCollection();
				}
				return clientCertificates;
			}
		}

		/// <summary>Gets or sets the Service Provider Name (SPN) to use for authentication when using extended protection.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the SPN to use for extended protection. The default value for this SPN is of the form "SMTPSVC/&lt;host&gt;" where &lt;host&gt; is the hostname of the SMTP mail server. </returns>
		private string TargetName
		{
			get;
			set;
		}

		/// <summary>Gets or sets the credentials used to authenticate the sender.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentialsByHost" /> that represents the credentials to use for authentication; or null if no credentials have been specified.</returns>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public ICredentialsByHost Credentials
		{
			get
			{
				return credentials;
			}
			set
			{
				CheckState();
				credentials = value;
			}
		}

		/// <summary>Specifies how outgoing email messages will be handled.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.SmtpDeliveryMethod" /> that indicates how email messages are delivered.</returns>
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return deliveryMethod;
			}
			set
			{
				CheckState();
				deliveryMethod = value;
			}
		}

		/// <summary>Specify whether the <see cref="T:System.Net.Mail.SmtpClient" /> uses Secure Sockets Layer (SSL) to encrypt the connection.</summary>
		/// <returns>true if the <see cref="T:System.Net.Mail.SmtpClient" /> uses SSL; otherwise, false. The default is false.</returns>
		public bool EnableSsl
		{
			get
			{
				return enableSsl;
			}
			set
			{
				CheckState();
				enableSsl = value;
			}
		}

		/// <summary>Gets or sets the name or IP address of the host used for SMTP transactions.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name or IP address of the computer to use for SMTP transactions.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is equal to <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public string Host
		{
			get
			{
				return host;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("An empty string is not allowed.", "value");
				}
				CheckState();
				host = value;
			}
		}

		/// <summary>Gets or sets the folder where applications save mail messages to be processed by the local SMTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the pickup directory for mail messages.</returns>
		public string PickupDirectoryLocation
		{
			get
			{
				return pickupDirectoryLocation;
			}
			set
			{
				pickupDirectoryLocation = value;
			}
		}

		/// <summary>Gets or sets the port used for SMTP transactions.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the port number on the SMTP host. The default value is 25.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				CheckState();
				port = value;
			}
		}

		/// <summary>Gets the network connection used to transmit the e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> that connects to the <see cref="P:System.Net.Mail.SmtpClient.Host" /> property used for SMTP.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null or the empty string ("").-or-<see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		[MonoTODO]
		public ServicePoint ServicePoint
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Mail.SmtpClient.Send" /> call times out.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that specifies the time-out value in milliseconds. The default value is 100,000 (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation was less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public int Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				CheckState();
				timeout = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an e-mail is being sent.</exception>
		public bool UseDefaultCredentials
		{
			get
			{
				return false;
			}
			[MonoNotSupported("no DefaultCredential support in Mono")]
			set
			{
				if (value)
				{
					throw new NotImplementedException("Default credentials are not supported");
				}
				CheckState();
			}
		}

		/// <summary>Occurs when an asynchronous e-mail send operation completes.</summary>
		public event SendCompletedEventHandler SendCompleted;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class by using configuration file settings. </summary>
		public SmtpClient()
			: this(null, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class that sends e-mail by using the specified SMTP server. </summary>
		/// <param name="host">A <see cref="T:System.String" /> that contains the name or IP address of the host computer used for SMTP transactions.</param>
		public SmtpClient(string host)
			: this(host, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class that sends e-mail by using the specified SMTP server and port.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that contains the name or IP address of the host used for SMTP transactions.</param>
		/// <param name="port">An <see cref="T:System.Int32" /> greater than zero that contains the port to be used on <paramref name="host" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> cannot be less than zero.</exception>
		public SmtpClient(string host, int port)
		{
			if (!string.IsNullOrEmpty(host))
			{
				this.host = host;
			}
			if (port != 0)
			{
				this.port = port;
			}
		}

		private void CheckState()
		{
			if (messageInProcess != null)
			{
				throw new InvalidOperationException("Cannot set Timeout while Sending a message");
			}
		}

		private static string EncodeAddress(MailAddress address)
		{
			string text = ContentType.EncodeSubjectRFC2047(address.DisplayName, Encoding.UTF8);
			return "\"" + text + "\" <" + address.Address + ">";
		}

		private static string EncodeAddresses(MailAddressCollection addresses)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (MailAddress address in addresses)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(EncodeAddress(address));
				flag = false;
			}
			return stringBuilder.ToString();
		}

		private string EncodeSubjectRFC2047(MailMessage message)
		{
			return ContentType.EncodeSubjectRFC2047(message.Subject, message.SubjectEncoding);
		}

		private string EncodeBody(MailMessage message)
		{
			string body = message.Body;
			Encoding bodyEncoding = message.BodyEncoding;
			switch (message.ContentTransferEncoding)
			{
			case TransferEncoding.SevenBit:
				return body;
			case TransferEncoding.Base64:
				return Convert.ToBase64String(bodyEncoding.GetBytes(body), Base64FormattingOptions.InsertLineBreaks);
			default:
				return ToQuotedPrintable(body, bodyEncoding);
			}
		}

		private string EncodeBody(AlternateView av)
		{
			byte[] array = new byte[av.ContentStream.Length];
			av.ContentStream.Read(array, 0, array.Length);
			switch (av.TransferEncoding)
			{
			case TransferEncoding.SevenBit:
				return Encoding.ASCII.GetString(array);
			case TransferEncoding.Base64:
				return Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks);
			default:
				return ToQuotedPrintable(array);
			}
		}

		private void EndSection(string section)
		{
			SendData($"--{section}--");
			SendData(string.Empty);
		}

		private string GenerateBoundary()
		{
			string result = GenerateBoundary(boundaryIndex);
			boundaryIndex++;
			return result;
		}

		private static string GenerateBoundary(int index)
		{
			return string.Format("--boundary_{0}_{1}", index, Guid.NewGuid().ToString("D"));
		}

		private bool IsError(SmtpResponse status)
		{
			return status.StatusCode >= (SmtpStatusCode)400;
		}

		/// <summary>Raises the <see cref="E:System.Net.Mail.SmtpClient.SendCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> that contains event data.</param>
		protected void OnSendCompleted(AsyncCompletedEventArgs e)
		{
			try
			{
				if (this.SendCompleted != null)
				{
					this.SendCompleted(this, e);
				}
			}
			finally
			{
				worker = null;
				user_async_state = null;
			}
		}

		private void CheckCancellation()
		{
			if (worker != null && worker.CancellationPending)
			{
				throw new CancellationException();
			}
		}

		private SmtpResponse Read()
		{
			byte[] array = new byte[512];
			int num = 0;
			bool flag = false;
			do
			{
				CheckCancellation();
				int num2 = stream.Read(array, num, array.Length - num);
				if (num2 <= 0)
				{
					break;
				}
				int num3 = num + num2 - 1;
				if (num3 > 4 && (array[num3] == 10 || array[num3] == 13))
				{
					int num4 = num3 - 3;
					while (num4 >= 0 && array[num4] != 10 && array[num4] != 13)
					{
						num4--;
					}
					flag = (array[num4 + 4] == 32);
				}
				num += num2;
				if (num == array.Length)
				{
					byte[] array2 = new byte[array.Length * 2];
					Array.Copy(array, 0, array2, 0, array.Length);
					array = array2;
				}
			}
			while (!flag);
			if (num > 0)
			{
				Encoding encoding = new ASCIIEncoding();
				string @string = encoding.GetString(array, 0, num - 1);
				return SmtpResponse.Parse(@string);
			}
			throw new IOException("Connection closed");
		}

		private void ResetExtensions()
		{
			authMechs = AuthMechs.None;
		}

		private void ParseExtensions(string extens)
		{
			char[] separator = new char[1]
			{
				' '
			};
			string[] array = extens.Split('\n');
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.Length < 4)
				{
					continue;
				}
				string text2 = text.Substring(4);
				if (!text2.StartsWith("AUTH ", StringComparison.Ordinal))
				{
					continue;
				}
				string[] array3 = text2.Split(separator);
				for (int j = 1; j < array3.Length; j++)
				{
					switch (array3[j].Trim())
					{
					case "CRAM-MD5":
						authMechs |= AuthMechs.CramMD5;
						break;
					case "DIGEST-MD5":
						authMechs |= AuthMechs.DigestMD5;
						break;
					case "GSSAPI":
						authMechs |= AuthMechs.GssAPI;
						break;
					case "KERBEROS_V4":
						authMechs |= AuthMechs.Kerberos4;
						break;
					case "LOGIN":
						authMechs |= AuthMechs.Login;
						break;
					case "PLAIN":
						authMechs |= AuthMechs.Plain;
						break;
					}
				}
			}
		}

		/// <summary>Sends the specified message to an SMTP server for delivery.</summary>
		/// <param name="message">A <see cref="T:System.Net.Mail.MailMessage" /> that contains the message to send.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.Mail.MailMessage.From" /> is null.-or-<see cref="P:System.Net.Mail.MailMessage.To" /> is null.-or- <paramref name="message" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">There are no recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, and <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, or <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		public void Send(MailMessage message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (deliveryMethod == SmtpDeliveryMethod.Network && (Host == null || Host.Trim().Length == 0))
			{
				throw new InvalidOperationException("The SMTP host was not specified");
			}
			if (deliveryMethod == SmtpDeliveryMethod.PickupDirectoryFromIis)
			{
				throw new NotSupportedException("IIS delivery is not supported");
			}
			if (port == 0)
			{
				port = 25;
			}
			mutex.WaitOne();
			try
			{
				messageInProcess = message;
				if (deliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
				{
					SendToFile(message);
				}
				else
				{
					SendInternal(message);
				}
			}
			catch (CancellationException)
			{
			}
			catch (SmtpException)
			{
				throw;
				IL_00b1:;
			}
			catch (Exception innerException)
			{
				throw new SmtpException("Message could not be sent.", innerException);
				IL_00c3:;
			}
			finally
			{
				mutex.ReleaseMutex();
				messageInProcess = null;
			}
		}

		private void SendInternal(MailMessage message)
		{
			CheckCancellation();
			try
			{
				client = new TcpClient(host, port);
				stream = client.GetStream();
				writer = new StreamWriter(stream);
				reader = new StreamReader(stream);
				SendCore(message);
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
				if (reader != null)
				{
					reader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (client != null)
				{
					client.Close();
				}
			}
		}

		private void SendToFile(MailMessage message)
		{
			if (!Path.IsPathRooted(pickupDirectoryLocation))
			{
				throw new SmtpException("Only absolute directories are allowed for pickup directory.");
			}
			string path = Path.Combine(pickupDirectoryLocation, Guid.NewGuid() + ".eml");
			try
			{
				writer = new StreamWriter(path);
				MailAddress from = message.From;
				if (from == null)
				{
					from = defaultFrom;
				}
				SendHeader("Date", DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss zzz", DateTimeFormatInfo.InvariantInfo));
				SendHeader("From", from.ToString());
				SendHeader("To", message.To.ToString());
				if (message.CC.Count > 0)
				{
					SendHeader("Cc", message.CC.ToString());
				}
				SendHeader("Subject", EncodeSubjectRFC2047(message));
				string[] allKeys = message.Headers.AllKeys;
				foreach (string name in allKeys)
				{
					SendHeader(name, message.Headers[name]);
				}
				AddPriorityHeader(message);
				boundaryIndex = 0;
				if (message.Attachments.Count > 0)
				{
					SendWithAttachments(message);
				}
				else
				{
					SendWithoutAttachments(message, null, attachmentExists: false);
				}
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
				writer = null;
			}
		}

		private void SendCore(MailMessage message)
		{
			SmtpResponse status = Read();
			if (IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			status = SendCommand("EHLO " + Dns.GetHostName());
			if (IsError(status))
			{
				status = SendCommand("HELO " + Dns.GetHostName());
				if (IsError(status))
				{
					throw new SmtpException(status.StatusCode, status.Description);
				}
			}
			else
			{
				string description = status.Description;
				if (description != null)
				{
					ParseExtensions(description);
				}
			}
			if (enableSsl)
			{
				InitiateSecureConnection();
				ResetExtensions();
				writer = new StreamWriter(stream);
				reader = new StreamReader(stream);
				status = SendCommand("EHLO " + Dns.GetHostName());
				if (IsError(status))
				{
					status = SendCommand("HELO " + Dns.GetHostName());
					if (IsError(status))
					{
						throw new SmtpException(status.StatusCode, status.Description);
					}
				}
				else
				{
					string description2 = status.Description;
					if (description2 != null)
					{
						ParseExtensions(description2);
					}
				}
			}
			if (authMechs != 0)
			{
				Authenticate();
			}
			MailAddress from = message.From;
			if (from == null)
			{
				from = defaultFrom;
			}
			status = SendCommand("MAIL FROM:<" + from.Address + '>');
			if (IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			List<SmtpFailedRecipientException> list = new List<SmtpFailedRecipientException>();
			for (int i = 0; i < message.To.Count; i++)
			{
				status = SendCommand("RCPT TO:<" + message.To[i].Address + '>');
				if (IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.To[i].Address));
				}
			}
			for (int j = 0; j < message.CC.Count; j++)
			{
				status = SendCommand("RCPT TO:<" + message.CC[j].Address + '>');
				if (IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.CC[j].Address));
				}
			}
			for (int k = 0; k < message.Bcc.Count; k++)
			{
				status = SendCommand("RCPT TO:<" + message.Bcc[k].Address + '>');
				if (IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.Bcc[k].Address));
				}
			}
			if (list.Count > 0)
			{
				throw new SmtpFailedRecipientsException("failed recipients", list.ToArray());
			}
			status = SendCommand("DATA");
			if (IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			string text = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss zzz", DateTimeFormatInfo.InvariantInfo);
			text = text.Remove(text.Length - 3, 1);
			SendHeader("Date", text);
			SendHeader("From", EncodeAddress(from));
			SendHeader("To", EncodeAddresses(message.To));
			if (message.CC.Count > 0)
			{
				SendHeader("Cc", EncodeAddresses(message.CC));
			}
			SendHeader("Subject", EncodeSubjectRFC2047(message));
			string value = "normal";
			switch (message.Priority)
			{
			case MailPriority.Normal:
				value = "normal";
				break;
			case MailPriority.Low:
				value = "non-urgent";
				break;
			case MailPriority.High:
				value = "urgent";
				break;
			}
			SendHeader("Priority", value);
			if (message.Sender != null)
			{
				SendHeader("Sender", EncodeAddress(message.Sender));
			}
			if (message.ReplyToList.Count > 0)
			{
				SendHeader("Reply-To", EncodeAddresses(message.ReplyToList));
			}
			string[] allKeys = message.Headers.AllKeys;
			foreach (string name in allKeys)
			{
				SendHeader(name, message.Headers[name]);
			}
			AddPriorityHeader(message);
			boundaryIndex = 0;
			if (message.Attachments.Count > 0)
			{
				SendWithAttachments(message);
			}
			else
			{
				SendWithoutAttachments(message, null, attachmentExists: false);
			}
			SendDot();
			status = Read();
			if (IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			try
			{
				status = SendCommand("QUIT");
			}
			catch (IOException)
			{
			}
		}

		/// <summary>Sends the specified e-mail message to an SMTP server for delivery. The message sender, recipients, subject, and message body are specified using <see cref="T:System.String" /> objects.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address information of the message sender.</param>
		/// <param name="recipients">A <see cref="T:System.String" /> that contains the addresses that the message is sent to.</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject line for the message.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="recipient" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" />.-or-<paramref name="recipient" /> is <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <paramref name="recipients" />. </exception>
		public void Send(string from, string to, string subject, string body)
		{
			Send(new MailMessage(from, to, subject, body));
		}

		private void SendDot()
		{
			writer.Write(".\r\n");
			writer.Flush();
		}

		private void SendData(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				writer.Write("\r\n");
				writer.Flush();
				return;
			}
			StringReader stringReader = new StringReader(data);
			bool flag = deliveryMethod == SmtpDeliveryMethod.Network;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				CheckCancellation();
				if (flag)
				{
					int i;
					for (i = 0; i < text.Length && text[i] == '.'; i++)
					{
					}
					if (i > 0 && i == text.Length)
					{
						text += ".";
					}
				}
				writer.Write(text);
				writer.Write("\r\n");
			}
			writer.Flush();
		}

		/// <summary>Sends the specified e-mail message to an SMTP server for delivery. This method does not block the calling thread and allows the caller to pass an object to the method that is invoked when the operation completes. </summary>
		/// <param name="message">A <see cref="T:System.Net.Mail.MailMessage" /> that contains the message to send.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.Mail.MailMessage.From" /> is null.-or-<see cref="P:System.Net.Mail.MailMessage.To" /> is null.-or- <paramref name="message" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">There are no recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, and <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The <paramref name="message" /> could not be delivered to one or more of the recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, or <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		public void SendAsync(MailMessage message, object userToken)
		{
			if (worker != null)
			{
				throw new InvalidOperationException("Another SendAsync operation is in progress");
			}
			worker = new BackgroundWorker();
			worker.DoWork += delegate(object o, DoWorkEventArgs ea)
			{
				try
				{
					user_async_state = ea.Argument;
					Send(message);
				}
				catch (Exception ex)
				{
					Exception ex3 = (Exception)(ea.Result = ex);
					throw ex3;
					IL_0031:;
				}
			};
			worker.WorkerSupportsCancellation = true;
			worker.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs ea)
			{
				OnSendCompleted(new AsyncCompletedEventArgs(ea.Error, ea.Cancelled, user_async_state));
			};
			worker.RunWorkerAsync(userToken);
		}

		/// <summary>Sends an e-mail message to an SMTP server for delivery. The message sender, recipients, subject, and message body are specified using <see cref="T:System.String" /> objects. This method does not block the calling thread and allows the caller to pass an object to the method that is invoked when the operation completes.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address information of the message sender.</param>
		/// <param name="recipients">A <see cref="T:System.String" /> that contains the address that the message is sent to.</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject line for the message.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="recipient" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" />.-or-<paramref name="recipient" /> is <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <paramref name="recipients" />. </exception>
		public void SendAsync(string from, string to, string subject, string body, object userToken)
		{
			SendAsync(new MailMessage(from, to, subject, body), userToken);
		}

		/// <summary>Cancels an asynchronous operation to send an e-mail message.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public void SendAsyncCancel()
		{
			if (worker == null)
			{
				throw new InvalidOperationException("SendAsync operation is not in progress");
			}
			worker.CancelAsync();
		}

		private void AddPriorityHeader(MailMessage message)
		{
			switch (message.Priority)
			{
			case MailPriority.High:
				SendHeader("Priority", "Urgent");
				SendHeader("Importance", "high");
				SendHeader("X-Priority", "1");
				break;
			case MailPriority.Low:
				SendHeader("Priority", "Non-Urgent");
				SendHeader("Importance", "low");
				SendHeader("X-Priority", "5");
				break;
			}
		}

		private void SendSimpleBody(MailMessage message)
		{
			SendHeader("Content-Type", message.BodyContentType.ToString());
			if (message.ContentTransferEncoding != TransferEncoding.SevenBit)
			{
				SendHeader("Content-Transfer-Encoding", GetTransferEncodingName(message.ContentTransferEncoding));
			}
			SendData(string.Empty);
			SendData(EncodeBody(message));
		}

		private void SendBodylessSingleAlternate(AlternateView av)
		{
			SendHeader("Content-Type", av.ContentType.ToString());
			if (av.TransferEncoding != TransferEncoding.SevenBit)
			{
				SendHeader("Content-Transfer-Encoding", GetTransferEncodingName(av.TransferEncoding));
			}
			SendData(string.Empty);
			SendData(EncodeBody(av));
		}

		private void SendWithoutAttachments(MailMessage message, string boundary, bool attachmentExists)
		{
			if (message.Body == null && message.AlternateViews.Count == 1)
			{
				SendBodylessSingleAlternate(message.AlternateViews[0]);
			}
			else if (message.AlternateViews.Count > 0)
			{
				SendBodyWithAlternateViews(message, boundary, attachmentExists);
			}
			else
			{
				SendSimpleBody(message);
			}
		}

		private void SendWithAttachments(MailMessage message)
		{
			string text = GenerateBoundary();
			ContentType contentType = new ContentType();
			contentType.Boundary = text;
			contentType.MediaType = "multipart/mixed";
			contentType.CharSet = null;
			SendHeader("Content-Type", contentType.ToString());
			SendData(string.Empty);
			Attachment attachment = null;
			if (message.AlternateViews.Count > 0)
			{
				SendWithoutAttachments(message, text, attachmentExists: true);
			}
			else
			{
				attachment = Attachment.CreateAttachmentFromString(message.Body, null, message.BodyEncoding, (!message.IsBodyHtml) ? "text/plain" : "text/html");
				message.Attachments.Insert(0, attachment);
			}
			try
			{
				SendAttachments(message, attachment, text);
			}
			finally
			{
				if (attachment != null)
				{
					message.Attachments.Remove(attachment);
				}
			}
			EndSection(text);
		}

		private void SendBodyWithAlternateViews(MailMessage message, string boundary, bool attachmentExists)
		{
			AlternateViewCollection alternateViews = message.AlternateViews;
			string text = GenerateBoundary();
			ContentType contentType = new ContentType();
			contentType.Boundary = text;
			contentType.MediaType = "multipart/alternative";
			if (!attachmentExists)
			{
				SendHeader("Content-Type", contentType.ToString());
				SendData(string.Empty);
			}
			AlternateView alternateView = null;
			if (message.Body != null)
			{
				alternateView = AlternateView.CreateAlternateViewFromString(message.Body, message.BodyEncoding, (!message.IsBodyHtml) ? "text/plain" : "text/html");
				alternateViews.Insert(0, alternateView);
				StartSection(boundary, contentType);
			}
			try
			{
				foreach (AlternateView item in alternateViews)
				{
					string text2 = null;
					if (item.LinkedResources.Count > 0)
					{
						text2 = GenerateBoundary();
						ContentType contentType2 = new ContentType("multipart/related");
						contentType2.Boundary = text2;
						contentType2.Parameters["type"] = item.ContentType.ToString();
						StartSection(text, contentType2);
						StartSection(text2, item.ContentType, item.TransferEncoding);
					}
					else
					{
						ContentType contentType2 = new ContentType(item.ContentType.ToString());
						StartSection(text, contentType2, item.TransferEncoding);
					}
					switch (item.TransferEncoding)
					{
					case TransferEncoding.Base64:
					{
						byte[] array = new byte[item.ContentStream.Length];
						item.ContentStream.Read(array, 0, array.Length);
						SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
						break;
					}
					case TransferEncoding.QuotedPrintable:
					{
						byte[] array2 = new byte[item.ContentStream.Length];
						item.ContentStream.Read(array2, 0, array2.Length);
						SendData(ToQuotedPrintable(array2));
						break;
					}
					case TransferEncoding.Unknown:
					case TransferEncoding.SevenBit:
					{
						byte[] array = new byte[item.ContentStream.Length];
						item.ContentStream.Read(array, 0, array.Length);
						SendData(Encoding.ASCII.GetString(array));
						break;
					}
					}
					if (item.LinkedResources.Count > 0)
					{
						SendLinkedResources(message, item.LinkedResources, text2);
						EndSection(text2);
					}
					if (!attachmentExists)
					{
						SendData(string.Empty);
					}
				}
			}
			finally
			{
				if (alternateView != null)
				{
					alternateViews.Remove(alternateView);
				}
			}
			EndSection(text);
		}

		private void SendLinkedResources(MailMessage message, LinkedResourceCollection resources, string boundary)
		{
			foreach (LinkedResource resource in resources)
			{
				StartSection(boundary, resource.ContentType, resource.TransferEncoding, resource);
				switch (resource.TransferEncoding)
				{
				case TransferEncoding.Base64:
				{
					byte[] array = new byte[resource.ContentStream.Length];
					resource.ContentStream.Read(array, 0, array.Length);
					SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
					break;
				}
				case TransferEncoding.QuotedPrintable:
				{
					byte[] array2 = new byte[resource.ContentStream.Length];
					resource.ContentStream.Read(array2, 0, array2.Length);
					SendData(ToQuotedPrintable(array2));
					break;
				}
				case TransferEncoding.Unknown:
				case TransferEncoding.SevenBit:
				{
					byte[] array = new byte[resource.ContentStream.Length];
					resource.ContentStream.Read(array, 0, array.Length);
					SendData(Encoding.ASCII.GetString(array));
					break;
				}
				}
			}
		}

		private void SendAttachments(MailMessage message, Attachment body, string boundary)
		{
			foreach (Attachment attachment in message.Attachments)
			{
				ContentType contentType = new ContentType(attachment.ContentType.ToString());
				if (attachment.Name != null)
				{
					contentType.Name = attachment.Name;
					if (attachment.NameEncoding != null)
					{
						contentType.CharSet = attachment.NameEncoding.HeaderName;
					}
					attachment.ContentDisposition.FileName = attachment.Name;
				}
				StartSection(boundary, contentType, attachment.TransferEncoding, (attachment != body) ? attachment.ContentDisposition : null);
				byte[] array = new byte[attachment.ContentStream.Length];
				attachment.ContentStream.Read(array, 0, array.Length);
				switch (attachment.TransferEncoding)
				{
				case TransferEncoding.Base64:
					SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
					break;
				case TransferEncoding.QuotedPrintable:
					SendData(ToQuotedPrintable(array));
					break;
				case TransferEncoding.Unknown:
				case TransferEncoding.SevenBit:
					SendData(Encoding.ASCII.GetString(array));
					break;
				}
				SendData(string.Empty);
			}
		}

		private SmtpResponse SendCommand(string command)
		{
			writer.Write(command);
			writer.Write("\r\n");
			writer.Flush();
			return Read();
		}

		private void SendHeader(string name, string value)
		{
			SendData($"{name}: {value}");
		}

		private void StartSection(string section, ContentType sectionContentType)
		{
			SendData($"--{section}");
			SendHeader("content-type", sectionContentType.ToString());
			SendData(string.Empty);
		}

		private void StartSection(string section, ContentType sectionContentType, TransferEncoding transferEncoding)
		{
			SendData($"--{section}");
			SendHeader("content-type", sectionContentType.ToString());
			SendHeader("content-transfer-encoding", GetTransferEncodingName(transferEncoding));
			SendData(string.Empty);
		}

		private void StartSection(string section, ContentType sectionContentType, TransferEncoding transferEncoding, LinkedResource lr)
		{
			SendData($"--{section}");
			SendHeader("content-type", sectionContentType.ToString());
			SendHeader("content-transfer-encoding", GetTransferEncodingName(transferEncoding));
			if (lr.ContentId != null && lr.ContentId.Length > 0)
			{
				SendHeader("content-ID", "<" + lr.ContentId + ">");
			}
			SendData(string.Empty);
		}

		private void StartSection(string section, ContentType sectionContentType, TransferEncoding transferEncoding, ContentDisposition contentDisposition)
		{
			SendData($"--{section}");
			SendHeader("content-type", sectionContentType.ToString());
			SendHeader("content-transfer-encoding", GetTransferEncodingName(transferEncoding));
			if (contentDisposition != null)
			{
				SendHeader("content-disposition", contentDisposition.ToString());
			}
			SendData(string.Empty);
		}

		private string ToQuotedPrintable(string input, Encoding enc)
		{
			byte[] bytes = enc.GetBytes(input);
			return ToQuotedPrintable(bytes);
		}

		private string ToQuotedPrintable(byte[] bytes)
		{
			StringWriter stringWriter = new StringWriter();
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder("=", 3);
			byte b = 61;
			char c = '\0';
			foreach (byte b2 in bytes)
			{
				int num2;
				if (b2 > 127 || b2 == b)
				{
					stringBuilder.Length = 1;
					stringBuilder.Append(Convert.ToString(b2, 16).ToUpperInvariant());
					num2 = 3;
				}
				else
				{
					c = Convert.ToChar(b2);
					if (c == '\r' || c == '\n')
					{
						stringWriter.Write(c);
						num = 0;
						continue;
					}
					num2 = 1;
				}
				num += num2;
				if (num > 75)
				{
					stringWriter.Write("=\r\n");
					num = num2;
				}
				if (num2 == 1)
				{
					stringWriter.Write(c);
				}
				else
				{
					stringWriter.Write(stringBuilder.ToString());
				}
			}
			return stringWriter.ToString();
		}

		private static string GetTransferEncodingName(TransferEncoding encoding)
		{
			switch (encoding)
			{
			case TransferEncoding.QuotedPrintable:
				return "quoted-printable";
			case TransferEncoding.SevenBit:
				return "7bit";
			case TransferEncoding.Base64:
				return "base64";
			default:
				return "unknown";
			}
		}

		private void InitiateSecureConnection()
		{
			SmtpResponse status = SendCommand("STARTTLS");
			if (IsError(status))
			{
				throw new SmtpException(SmtpStatusCode.GeneralFailure, "Server does not support secure connections.");
			}
			SslStream sslStream = new SslStream(stream, leaveStreamOpen: false, callback, null);
			CheckCancellation();
			sslStream.AuthenticateAsClient(Host, ClientCertificates, SslProtocols.Default, checkCertificateRevocation: false);
			stream = sslStream;
		}

		private void Authenticate()
		{
			string text = null;
			string text2 = null;
			if (UseDefaultCredentials)
			{
				text = CredentialCache.DefaultCredentials.GetCredential(new Uri("smtp://" + host), "basic").UserName;
				text2 = CredentialCache.DefaultCredentials.GetCredential(new Uri("smtp://" + host), "basic").Password;
			}
			else
			{
				if (Credentials == null)
				{
					return;
				}
				text = Credentials.GetCredential(host, port, "smtp").UserName;
				text2 = Credentials.GetCredential(host, port, "smtp").Password;
			}
			Authenticate(text, text2);
		}

		private void Authenticate(string Username, string Password)
		{
			SmtpResponse smtpResponse = SendCommand("AUTH LOGIN");
			if (smtpResponse.StatusCode != (SmtpStatusCode)334)
			{
				throw new SmtpException(smtpResponse.StatusCode, smtpResponse.Description);
			}
			smtpResponse = SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(Username)));
			if (smtpResponse.StatusCode != (SmtpStatusCode)334)
			{
				throw new SmtpException(smtpResponse.StatusCode, smtpResponse.Description);
			}
			smtpResponse = SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(Password)));
			if (IsError(smtpResponse))
			{
				throw new SmtpException(smtpResponse.StatusCode, smtpResponse.Description);
			}
		}
	}
}
