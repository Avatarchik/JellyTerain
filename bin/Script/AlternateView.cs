using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents the format to view an email message.</summary>
	public class AlternateView : AttachmentBase
	{
		private Uri baseUri;

		private LinkedResourceCollection linkedResources = new LinkedResourceCollection();

		/// <summary>Gets or sets the Base URI to use for resolving relative URIs in the <see cref="T:System.Net.Mail.AlternateView" />.</summary>
		/// <returns>A <see cref="T:System.Uri" />.</returns>
		public Uri BaseUri
		{
			get
			{
				return baseUri;
			}
			set
			{
				baseUri = value;
			}
		}

		/// <summary>Gets the set of embedded resources referred to by this attachment.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.LinkedResourceCollection" /> object that stores the collection of linked resources to be sent as part of an e-mail message.</returns>
		public LinkedResourceCollection LinkedResources => linkedResources;

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		public AlternateView(string fileName)
			: base(fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name and content type.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		public AlternateView(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name and media type.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		public AlternateView(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		public AlternateView(Stream contentStream)
			: base(contentStream)
		{
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" /> and media type.</summary>
		/// <param name="contentStream">A stream that contains the content for this attachment.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		public AlternateView(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" /> and <see cref="T:System.Net.Mime.ContentType" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this attachment.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		public AlternateView(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		/// <summary>Creates a <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="System.String" />.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <param name="content">The <see cref="T:System.String" /> that contains the content of the email message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		public static AlternateView CreateAlternateViewFromString(string content)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			MemoryStream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
			AlternateView alternateView = new AlternateView(contentStream);
			alternateView.TransferEncoding = TransferEncoding.QuotedPrintable;
			return alternateView;
		}

		/// <summary>Creates an <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="System.String" /> and the specified MIME media type of the content.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
		/// <param name="contentType">A <see cref="T:System.Net.Mime.ContentType" /> that describes the data in <paramref name="string" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			Encoding encoding = (contentType.CharSet == null) ? Encoding.UTF8 : Encoding.GetEncoding(contentType.CharSet);
			MemoryStream contentStream = new MemoryStream(encoding.GetBytes(content));
			AlternateView alternateView = new AlternateView(contentStream, contentType);
			alternateView.TransferEncoding = TransferEncoding.QuotedPrintable;
			return alternateView;
		}

		/// <summary>Creates an <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="System.String" />, the specified text encoding, and MIME media type of the content.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
		/// <param name="contentEncoding">An <see cref="T:System.Text.Encoding" />. This value can be null.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		public static AlternateView CreateAlternateViewFromString(string content, Encoding encoding, string mediaType)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			MemoryStream contentStream = new MemoryStream(encoding.GetBytes(content));
			ContentType contentType = new ContentType();
			contentType.MediaType = mediaType;
			contentType.CharSet = encoding.HeaderName;
			AlternateView alternateView = new AlternateView(contentStream, contentType);
			alternateView.TransferEncoding = TransferEncoding.QuotedPrintable;
			return alternateView;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Mail.AlternateView" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (LinkedResource linkedResource in linkedResources)
				{
					linkedResource.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}
