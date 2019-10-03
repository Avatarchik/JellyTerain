using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace System.Net.Mime
{
	/// <summary>Represents a MIME protocol Content-Disposition header.</summary>
	public class ContentDisposition
	{
		private const string rfc822 = "dd MMM yyyy HH':'mm':'ss zz00";

		private string dispositionType;

		private StringDictionary parameters = new StringDictionary();

		/// <summary>Gets or sets the creation date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file creation date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		public DateTime CreationDate
		{
			get
			{
				if (parameters.ContainsKey("creation-date"))
				{
					return DateTime.ParseExact(parameters["creation-date"], "dd MMM yyyy HH':'mm':'ss zz00", null);
				}
				return DateTime.MinValue;
			}
			set
			{
				if (value > DateTime.MinValue)
				{
					parameters["creation-date"] = value.ToString("dd MMM yyyy HH':'mm':'ss zz00");
				}
				else
				{
					parameters.Remove("modification-date");
				}
			}
		}

		/// <summary>Gets or sets the disposition type for an e-mail attachment.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the disposition type. The value is not restricted but is typically one of the <see cref="P:System.Net.Mime.ContentDisposition.DispositionType" /> values.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is equal to <see cref="F:System.String.Empty" /> ("").</exception>
		public string DispositionType
		{
			get
			{
				return dispositionType;
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
				dispositionType = value;
			}
		}

		/// <summary>Gets or sets the suggested file name for an e-mail attachment.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the file name. </returns>
		public string FileName
		{
			get
			{
				return parameters["filename"];
			}
			set
			{
				parameters["filename"] = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines the disposition type (Inline or Attachment) for an e-mail attachment.</summary>
		/// <returns>true if content in the attachment is presented inline as part of the e-mail body; otherwise, false. </returns>
		public bool Inline
		{
			get
			{
				return string.Compare(dispositionType, "inline", ignoreCase: true, CultureInfo.InvariantCulture) == 0;
			}
			set
			{
				if (value)
				{
					dispositionType = "inline";
				}
				else
				{
					dispositionType = "attachment";
				}
			}
		}

		/// <summary>Gets or sets the modification date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file modification date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		public DateTime ModificationDate
		{
			get
			{
				if (parameters.ContainsKey("modification-date"))
				{
					return DateTime.ParseExact(parameters["modification-date"], "dd MMM yyyy HH':'mm':'ss zz00", null);
				}
				return DateTime.MinValue;
			}
			set
			{
				if (value > DateTime.MinValue)
				{
					parameters["modification-date"] = value.ToString("dd MMM yyyy HH':'mm':'ss zz00");
				}
				else
				{
					parameters.Remove("modification-date");
				}
			}
		}

		/// <summary>Gets the parameters included in the Content-Disposition header represented by this instance.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains parameter name/value pairs.</returns>
		public StringDictionary Parameters => parameters;

		/// <summary>Gets or sets the read date for a file attachment.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that indicates the file read date; otherwise, <see cref="F:System.DateTime.MinValue" /> if no date was specified.</returns>
		public DateTime ReadDate
		{
			get
			{
				if (parameters.ContainsKey("read-date"))
				{
					return DateTime.ParseExact(parameters["read-date"], "dd MMM yyyy HH':'mm':'ss zz00", null);
				}
				return DateTime.MinValue;
			}
			set
			{
				if (value > DateTime.MinValue)
				{
					parameters["read-date"] = value.ToString("dd MMM yyyy HH':'mm':'ss zz00");
				}
				else
				{
					parameters.Remove("read-date");
				}
			}
		}

		/// <summary>Gets or sets the size of a file attachment.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the number of bytes in the file attachment. The default value is -1, which indicates that the file size is unknown.</returns>
		public long Size
		{
			get
			{
				if (parameters.ContainsKey("size"))
				{
					return long.Parse(parameters["size"]);
				}
				return -1L;
			}
			set
			{
				if (value > -1)
				{
					parameters["size"] = value.ToString();
				}
				else
				{
					parameters.Remove("size");
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentDisposition" /> class with a <see cref="P:System.Net.Mime.ContentDisposition.DispositionType" /> of <see cref="F:System.Net.Mime.DispositionTypeNames.Attachment" />. </summary>
		public ContentDisposition()
			: this("attachment")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mime.ContentDisposition" /> class with the specified disposition information.</summary>
		/// <param name="disposition">A <see cref="T:System.Net.Mime.DispositionTypeNames" /> value that contains the disposition.</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="disposition" /> is null or equal to <see cref="F:System.String.Empty" /> ("").</exception>
		public ContentDisposition(string disposition)
		{
			if (disposition == null)
			{
				throw new ArgumentNullException();
			}
			if (disposition.Length < 1)
			{
				throw new FormatException();
			}
			Size = -1L;
			try
			{
				int num = disposition.IndexOf(';');
				if (num < 0)
				{
					dispositionType = disposition.Trim();
				}
				else
				{
					string[] array = disposition.Split(';');
					dispositionType = array[0].Trim();
					for (int i = 1; i < array.Length; i++)
					{
						Parse(array[i]);
					}
				}
			}
			catch
			{
				throw new FormatException();
				IL_00a1:;
			}
		}

		private void Parse(string pair)
		{
			if (pair != null && pair.Length >= 0)
			{
				string[] array = pair.Split('=');
				if (array.Length != 2)
				{
					throw new FormatException();
				}
				parameters.Add(array[0].Trim(), array[1].Trim());
			}
		}

		/// <summary>Determines whether the content-disposition header of the specified <see cref="T:System.Net.Mime.ContentDisposition" /> object is equal to the content-disposition header of this object.</summary>
		/// <returns>true if the content-disposition headers are the same; otherwise false.</returns>
		/// <param name="rparam">The <see cref="T:System.Net.Mime.ContentDisposition" /> object to compare with this object.</param>
		public override bool Equals(object obj)
		{
			return Equals(obj as ContentDisposition);
		}

		private bool Equals(ContentDisposition other)
		{
			return other != null && ToString() == other.ToString();
		}

		/// <summary>Determines the hash code of the specified <see cref="T:System.Net.Mime.ContentDisposition" /> object</summary>
		/// <returns>An integer hash value.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the property values for this instance.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DispositionType.ToLower());
			if (Parameters != null && Parameters.Count > 0)
			{
				bool flag = false;
				foreach (DictionaryEntry parameter in Parameters)
				{
					if (parameter.Value != null && parameter.Value.ToString().Length > 0)
					{
						stringBuilder.Append("; ");
						stringBuilder.Append(parameter.Key);
						stringBuilder.Append("=");
						string text = parameter.Key.ToString();
						string text2 = parameter.Value.ToString();
						flag = (((text == "filename" && text2.IndexOf(' ') != -1) || text.EndsWith("date")) ? true : false);
						if (flag)
						{
							stringBuilder.Append("\"");
						}
						stringBuilder.Append(text2);
						if (flag)
						{
							stringBuilder.Append("\"");
						}
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
