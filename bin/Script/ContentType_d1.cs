using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	/// <summary>Represents a MIME protocol Content-Type header.</summary>
	public class ContentType
	{
		private static Encoding utf8unmarked;

		private string mediaType;

		private StringDictionary parameters = new StringDictionary();

		private static readonly char[] especials = new char[16]
		{
			'(',
			')',
			'<',
			'>',
			'@',
			',',
			';',
			':',
			'<',
			'>',
			'/',
			'[',
			']',
			'?',
			'.',
			'='
		};

		private static Encoding UTF8Unmarked
		{
			get
			{
				if (utf8unmarked == null)
				{
					utf8unmarked = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				}
				return utf8unmarked;
			}
		}

		/// <summary>Gets or sets the value of the boundary parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the boundary parameter.</returns>
		public string Boundary
		{
			get
			{
				return parameters["boundary"];
			}
			set
			{
				parameters["boundary"] = value;
			}
		}

		/// <summary>Gets or sets the value of the charset parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the charset parameter.</returns>
		public string CharSet
		{
			get
			{
				return parameters["charset"];
			}
			set
			{
				parameters["charset"] = value;
			}
		}

		/// <summary>Gets or sets the media type value included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the media type and subtype value. This value does not include the semicolon (;) separator that follows the subtype.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">The value specified for a set operation is in a form that cannot be parsed.</exception>
		public string MediaType
		{
			get
			{
				return mediaType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.Length < 1)
				{
					throw new ArgumentException();
				}
				if (value.IndexOf('/') < 1)
				{
					throw new FormatException();
				}
				if (value.IndexOf(';') != -1)
				{
					throw new FormatException();
				}
				mediaType = value;
			}
		}

		/// <summary>Gets or sets the value of the name parameter included in the Content-Type header represented by this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value associated with the name parameter. </returns>
		public string Name
		{
			get
			{
				return parameters["name"];
			}
			set
			{
				parameters["name"] = value;
			}
		}

		/// <summary>Gets the dictionary that contains the parameters included in the Content-Type header represented by this instance.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains name and value pairs.</returns>
		public StringDictionary Parameters => parameters;

		/// <summary>Initializes a new default instance of the <see cref="T:System.Net.Mime.ContentType" /> class. </summary>
		public ContentType()
		{
			mediaType = "application/octet-stream";
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentType" /> class using the specified string. </summary>
		/// <param name="contentType">A <see cref="T:System.String" />, for example, "text/plain; charset=us-ascii", that contains the MIME media type, subtype, and optional parameters.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentType" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="contentType" /> is <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is in a form that cannot be parsed.</exception>
		public ContentType(string contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (contentType.Length < 1)
			{
				throw new ArgumentException("contentType");
			}
			int num = contentType.IndexOf(';');
			if (num > 0)
			{
				string[] array = contentType.Split(';');
				MediaType = array[0].Trim();
				for (int i = 1; i < array.Length; i++)
				{
					Parse(array[i]);
				}
			}
			else
			{
				MediaType = contentType.Trim();
			}
		}

		private void Parse(string pair)
		{
			if (pair != null && pair.Length >= 1)
			{
				string[] array = pair.Split('=');
				if (array.Length == 2)
				{
					parameters.Add(array[0].Trim(), array[1].Trim());
				}
			}
		}

		/// <summary>Determines whether the content-type header of the specified <see cref="T:System.Net.Mime.ContentType" /> object is equal to the content-type header of this object.</summary>
		/// <returns>true if the content-type headers are the same; otherwise false.</returns>
		/// <param name="rparam">The <see cref="T:System.Net.Mime.ContentType" /> object to compare with this object.</param>
		public override bool Equals(object obj)
		{
			return Equals(obj as ContentType);
		}

		private bool Equals(ContentType other)
		{
			return other != null && ToString() == other.ToString();
		}

		/// <summary>Determines the hash code of the specified <see cref="T:System.Net.Mime.ContentType" /> object</summary>
		/// <returns>An integer hash value.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>Returns a string representation of this <see cref="T:System.Net.Mime.ContentType" /> object.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the current settings for this <see cref="T:System.Net.Mime.ContentType" />.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Encoding enc = (CharSet == null) ? Encoding.UTF8 : Encoding.GetEncoding(CharSet);
			stringBuilder.Append(MediaType);
			if (Parameters != null && Parameters.Count > 0)
			{
				foreach (DictionaryEntry parameter in parameters)
				{
					if (parameter.Value != null && parameter.Value.ToString().Length > 0)
					{
						stringBuilder.Append("; ");
						stringBuilder.Append(parameter.Key);
						stringBuilder.Append("=");
						stringBuilder.Append(WrapIfEspecialsExist(EncodeSubjectRFC2047(parameter.Value as string, enc)));
					}
				}
			}
			return stringBuilder.ToString();
		}

		private static string WrapIfEspecialsExist(string s)
		{
			s = s.Replace("\"", "\\\"");
			if (s.IndexOfAny(especials) >= 0)
			{
				return '"' + s + '"';
			}
			return s;
		}

		internal static Encoding GuessEncoding(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= '\u0080')
				{
					return UTF8Unmarked;
				}
			}
			return null;
		}

		internal static TransferEncoding GuessTransferEncoding(Encoding enc)
		{
			if (Encoding.ASCII.Equals(enc))
			{
				return TransferEncoding.SevenBit;
			}
			if (Encoding.UTF8.CodePage == enc.CodePage || Encoding.Unicode.CodePage == enc.CodePage)
			{
				return TransferEncoding.Base64;
			}
			return TransferEncoding.QuotedPrintable;
		}

		internal static string To2047(byte[] bytes)
		{
			StringWriter stringWriter = new StringWriter();
			foreach (byte b in bytes)
			{
				if (b > 127 || b == 9)
				{
					stringWriter.Write("=");
					stringWriter.Write(Convert.ToString(b, 16).ToUpper());
				}
				else
				{
					stringWriter.Write(Convert.ToChar(b));
				}
			}
			return stringWriter.GetStringBuilder().ToString();
		}

		internal static string EncodeSubjectRFC2047(string s, Encoding enc)
		{
			if (s == null || Encoding.ASCII.Equals(enc))
			{
				return s;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= '\u0080')
				{
					string text = To2047(enc.GetBytes(s));
					return "=?" + enc.HeaderName + "?Q?" + text + "?=";
				}
			}
			return s;
		}
	}
}
