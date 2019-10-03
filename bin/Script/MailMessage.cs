using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents an e-mail message that can be sent using the <see cref="T:System.Net.Mail.SmtpClient" /> class.</summary>
	public class MailMessage : IDisposable
	{
		private AlternateViewCollection alternateViews;

		private AttachmentCollection attachments;

		private MailAddressCollection bcc;

		private MailAddressCollection replyTo;

		private string body;

		private MailPriority priority;

		private MailAddress sender;

		private DeliveryNotificationOptions deliveryNotificationOptions;

		private MailAddressCollection cc;

		private MailAddress from;

		private NameValueCollection headers;

		private MailAddressCollection to;

		private string subject;

		private Encoding subjectEncoding;

		private Encoding bodyEncoding;

		private Encoding headersEncoding = Encoding.UTF8;

		private bool isHtml;

		/// <summary>Gets the attachment collection used to store alternate forms of the message body.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.AlternateViewCollection" />.</returns>
		public AlternateViewCollection AlternateViews => alternateViews;

		/// <summary>Gets the attachment collection used to store data attached to this e-mail message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.AttachmentCollection" />.</returns>
		public AttachmentCollection Attachments => attachments;

		/// <summary>Gets the address collection that contains the blind carbon copy (BCC) recipients for this e-mail message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		public MailAddressCollection Bcc => bcc;

		/// <summary>Gets or sets the message body.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the body text.</returns>
		public string Body
		{
			get
			{
				return body;
			}
			set
			{
				if (value != null && bodyEncoding == null)
				{
					bodyEncoding = (GuessEncoding(value) ?? Encoding.ASCII);
				}
				body = value;
			}
		}

		internal ContentType BodyContentType
		{
			get
			{
				ContentType contentType = new ContentType((!isHtml) ? "text/plain" : "text/html");
				contentType.CharSet = (BodyEncoding ?? Encoding.ASCII).HeaderName;
				return contentType;
			}
		}

		internal TransferEncoding ContentTransferEncoding => ContentType.GuessTransferEncoding(BodyEncoding);

		/// <summary>Gets or sets the encoding used to encode the message body.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> applied to the contents of the <see cref="P:System.Net.Mail.MailMessage.Body" />.</returns>
		public Encoding BodyEncoding
		{
			get
			{
				return bodyEncoding;
			}
			set
			{
				bodyEncoding = value;
			}
		}

		/// <summary>Gets the address collection that contains the carbon copy (CC) recipients for this e-mail message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		public MailAddressCollection CC => cc;

		/// <summary>Gets or sets the delivery notifications for this e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.DeliveryNotificationOptions" /> value that contains the delivery notifications for this message.</returns>
		public DeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return deliveryNotificationOptions;
			}
			set
			{
				deliveryNotificationOptions = value;
			}
		}

		/// <summary>Gets or sets the from address for this e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailAddress" /> that contains the from address information.</returns>
		public MailAddress From
		{
			get
			{
				return from;
			}
			set
			{
				from = value;
			}
		}

		/// <summary>Gets the e-mail headers that are transmitted with this e-mail message.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains the e-mail headers.</returns>
		public NameValueCollection Headers => headers;

		/// <summary>Gets or sets a value indicating whether the mail message body is in Html.</summary>
		/// <returns>true if the message body is in Html; else false. The default is false.</returns>
		public bool IsBodyHtml
		{
			get
			{
				return isHtml;
			}
			set
			{
				isHtml = value;
			}
		}

		/// <summary>Gets or sets the priority of this e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailPriority" /> that contains the priority of this message.</returns>
		public MailPriority Priority
		{
			get
			{
				return priority;
			}
			set
			{
				priority = value;
			}
		}

		internal Encoding HeadersEncoding
		{
			get
			{
				return headersEncoding;
			}
			set
			{
				headersEncoding = value;
			}
		}

		internal MailAddressCollection ReplyToList => replyTo;

		/// <summary>Gets or sets the ReplyTo address for the mail message.</summary>
		/// <returns>A MailAddress that indicates the value of the <see cref="P:System.Net.Mail.MailMessage.ReplyTo" /> field.</returns>
		public MailAddress ReplyTo
		{
			get
			{
				if (replyTo.Count == 0)
				{
					return null;
				}
				return replyTo[0];
			}
			set
			{
				replyTo.Clear();
				replyTo.Add(value);
			}
		}

		/// <summary>Gets or sets the sender's address for this e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.MailAddress" /> that contains the sender's address information.</returns>
		public MailAddress Sender
		{
			get
			{
				return sender;
			}
			set
			{
				sender = value;
			}
		}

		/// <summary>Gets or sets the subject line for this e-mail message.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the subject content.</returns>
		public string Subject
		{
			get
			{
				return subject;
			}
			set
			{
				if (value != null && subjectEncoding == null)
				{
					subjectEncoding = GuessEncoding(value);
				}
				subject = value;
			}
		}

		/// <summary>Gets or sets the encoding used for the subject content for this e-mail message.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> that was used to encode the <see cref="P:System.Net.Mail.MailMessage.Subject" /> property.</returns>
		public Encoding SubjectEncoding
		{
			get
			{
				return subjectEncoding;
			}
			set
			{
				subjectEncoding = value;
			}
		}

		/// <summary>Gets the address collection that contains the recipients of this e-mail message.</summary>
		/// <returns>A writable <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</returns>
		public MailAddressCollection To => to;

		/// <summary>Initializes an empty instance of the <see cref="T:System.Net.Mail.MailMessage" /> class.</summary>
		public MailMessage()
		{
			to = new MailAddressCollection();
			alternateViews = new AlternateViewCollection();
			attachments = new AttachmentCollection();
			bcc = new MailAddressCollection();
			cc = new MailAddressCollection();
			replyTo = new MailAddressCollection();
			headers = new NameValueCollection();
			headers.Add("MIME-Version", "1.0");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class by using the specified <see cref="T:System.Net.Mail.MailAddress" /> class objects. </summary>
		/// <param name="from">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail message.</param>
		/// <param name="to">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the recipient of the e-mail message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="to" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		public MailMessage(MailAddress from, MailAddress to)
			: this()
		{
			if (from == null || to == null)
			{
				throw new ArgumentNullException();
			}
			From = from;
			this.to.Add(to);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class by using the specified <see cref="T:System.String" /> class objects. </summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
		/// <param name="to">A <see cref="T:System.String" /> that contains the addresses of the recipients of the e-mail message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="to" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").-or-<paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		public MailMessage(string from, string to)
			: this()
		{
			if (from == null || from == string.Empty)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null || to == string.Empty)
			{
				throw new ArgumentNullException("to");
			}
			this.from = new MailAddress(from);
			string[] array = to.Split(',');
			foreach (string text in array)
			{
				this.to.Add(new MailAddress(text.Trim()));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.MailMessage" /> class. </summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
		/// <param name="to">A <see cref="T:System.String" /> that contains the address of the recipient of the e-mail message.</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject text.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="to" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").-or-<paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="from" /> or <paramref name="to" /> is malformed.</exception>
		public MailMessage(string from, string to, string subject, string body)
			: this()
		{
			if (from == null || from == string.Empty)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null || to == string.Empty)
			{
				throw new ArgumentNullException("to");
			}
			this.from = new MailAddress(from);
			string[] array = to.Split(',');
			foreach (string text in array)
			{
				this.to.Add(new MailAddress(text.Trim()));
			}
			Body = body;
			Subject = subject;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.MailMessage" />. </summary>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Mail.MailMessage" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
		}

		private Encoding GuessEncoding(string s)
		{
			return ContentType.GuessEncoding(s);
		}
	}
}
