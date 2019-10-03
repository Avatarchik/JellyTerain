using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.Net
{
	/// <summary>Contains protocol headers associated with a request or response.</summary>
	[Serializable]
	[ComVisible(true)]
	public class WebHeaderCollection : NameValueCollection, ISerializable
	{
		private static readonly Hashtable restricted;

		private static readonly Hashtable multiValue;

		private static readonly Dictionary<string, bool> restricted_response;

		private bool internallyCreated;

		private static bool[] allowed_chars;

		/// <summary>Gets all header names (keys) in the collection.</summary>
		/// <returns>An array of type <see cref="T:System.String" /> containing all header names in a Web request.</returns>
		public override string[] AllKeys => base.AllKeys;

		/// <summary>Gets the number of headers in the collection.</summary>
		/// <returns>An <see cref="T:System.Int32" /> indicating the number of headers in a request.</returns>
		public override int Count => base.Count;

		/// <summary>Gets the collection of header names (keys) in the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> containing all header names in a Web request.</returns>
		public override KeysCollection Keys => base.Keys;

		/// <summary>Gets or sets the specified request header.</summary>
		/// <returns>A <see cref="T:System.String" /> instance containing the specified header value.</returns>
		/// <param name="header">The request header value.</param>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpRequestHeader" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string this[HttpRequestHeader hrh]
		{
			get
			{
				return Get(RequestHeaderToString(hrh));
			}
			set
			{
				Add(RequestHeaderToString(hrh), value);
			}
		}

		/// <summary>Gets or sets the specified response header.</summary>
		/// <returns>A <see cref="T:System.String" /> instance containing the specified header.</returns>
		/// <param name="header">The response header value.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpResponseHeader" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string this[HttpResponseHeader hrh]
		{
			get
			{
				return Get(ResponseHeaderToString(hrh));
			}
			set
			{
				Add(ResponseHeaderToString(hrh), value);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebHeaderCollection" /> class.</summary>
		public WebHeaderCollection()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebHeaderCollection" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> containing the information required to serialize the <see cref="T:System.Net.WebHeaderCollection" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> containing the source of the serialized stream associated with the new <see cref="T:System.Net.WebHeaderCollection" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="headerName" /> contains invalid characters. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="headerName" /> is a null reference or <see cref="F:System.String.Empty" />. </exception>
		protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			try
			{
				int @int = serializationInfo.GetInt32("Count");
				for (int i = 0; i < @int; i++)
				{
					Add(serializationInfo.GetString(i.ToString()), serializationInfo.GetString((@int + i).ToString()));
				}
			}
			catch (SerializationException)
			{
				int @int = serializationInfo.GetInt32("count");
				for (int j = 0; j < @int; j++)
				{
					Add(serializationInfo.GetString("k" + j), serializationInfo.GetString("v" + j));
				}
			}
		}

		internal WebHeaderCollection(bool internallyCreated)
		{
			this.internallyCreated = internallyCreated;
		}

		static WebHeaderCollection()
		{
			allowed_chars = new bool[126]
			{
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				true,
				false,
				true,
				true,
				true,
				true,
				false,
				false,
				false,
				true,
				true,
				false,
				true,
				true,
				false,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				false,
				false,
				false,
				false,
				false,
				false,
				false,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				false,
				false,
				false,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				false,
				true,
				false
			};
			restricted = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
			restricted.Add("accept", true);
			restricted.Add("connection", true);
			restricted.Add("content-length", true);
			restricted.Add("content-type", true);
			restricted.Add("date", true);
			restricted.Add("expect", true);
			restricted.Add("host", true);
			restricted.Add("if-modified-since", true);
			restricted.Add("range", true);
			restricted.Add("referer", true);
			restricted.Add("transfer-encoding", true);
			restricted.Add("user-agent", true);
			restricted.Add("proxy-connection", true);
			restricted_response = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
			restricted_response.Add("Content-Length", value: true);
			restricted_response.Add("Transfer-Encoding", value: true);
			restricted_response.Add("WWW-Authenticate", value: true);
			multiValue = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
			multiValue.Add("accept", true);
			multiValue.Add("accept-charset", true);
			multiValue.Add("accept-encoding", true);
			multiValue.Add("accept-language", true);
			multiValue.Add("accept-ranges", true);
			multiValue.Add("allow", true);
			multiValue.Add("authorization", true);
			multiValue.Add("cache-control", true);
			multiValue.Add("connection", true);
			multiValue.Add("content-encoding", true);
			multiValue.Add("content-language", true);
			multiValue.Add("expect", true);
			multiValue.Add("if-match", true);
			multiValue.Add("if-none-match", true);
			multiValue.Add("proxy-authenticate", true);
			multiValue.Add("public", true);
			multiValue.Add("range", true);
			multiValue.Add("transfer-encoding", true);
			multiValue.Add("upgrade", true);
			multiValue.Add("vary", true);
			multiValue.Add("via", true);
			multiValue.Add("warning", true);
			multiValue.Add("www-authenticate", true);
			multiValue.Add("set-cookie", true);
			multiValue.Add("set-cookie2", true);
		}

		/// <summary>Serializes this instance into the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</summary>
		/// <param name="serializationInfo">The object into which this <see cref="T:System.Net.WebHeaderCollection" /> will be serialized. </param>
		/// <param name="streamingContext">The destination of the serialization. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Inserts the specified header into the collection.</summary>
		/// <param name="header">The header to add, with the name and value separated by a colon. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="header" /> is null or <see cref="F:System.String.Empty" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="header" /> does not contain a colon (:) character.The length of <paramref name="value" /> is greater than 65535.-or- The name part of <paramref name="header" /> is <see cref="F:System.String.Empty" /> or contains invalid characters.-or- <paramref name="header" /> is a restricted header that should be set with a property.-or- The value part of <paramref name="header" /> contains invalid characters. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length the string after the colon (:) is greater than 65535. </exception>
		public void Add(string header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			int num = header.IndexOf(':');
			if (num == -1)
			{
				throw new ArgumentException("no colon found", "header");
			}
			Add(header.Substring(0, num), header.Substring(num + 1));
		}

		/// <summary>Inserts a header with the specified name and value into the collection.</summary>
		/// <param name="name">The header to add to the collection. </param>
		/// <param name="value">The content of the header. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is null, <see cref="F:System.String.Empty" />, or contains invalid characters.-or- <paramref name="name" /> is a restricted header that must be set with a property setting.-or- <paramref name="value" /> contains invalid characters. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		public override void Add(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (internallyCreated && IsRestricted(name))
			{
				throw new ArgumentException("This header must be modified with the appropiate property.");
			}
			AddWithoutValidate(name, value);
		}

		/// <summary>Inserts a header into the collection without checking whether the header is on the restricted header list.</summary>
		/// <param name="headerName">The header to add to the collection. </param>
		/// <param name="headerValue">The content of the header. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="headerName" /> is null, <see cref="F:System.String.Empty" />, or contains invalid characters.-or- <paramref name="headerValue" /> contains invalid characters. </exception>
		protected void AddWithoutValidate(string headerName, string headerValue)
		{
			if (!IsHeaderName(headerName))
			{
				throw new ArgumentException("invalid header name: " + headerName, "headerName");
			}
			headerValue = ((headerValue != null) ? headerValue.Trim() : string.Empty);
			if (!IsHeaderValue(headerValue))
			{
				throw new ArgumentException("invalid header value: " + headerValue, "headerValue");
			}
			base.Add(headerName, headerValue);
		}

		/// <summary>Gets an array of header values stored in a header.</summary>
		/// <returns>An array of header strings.</returns>
		/// <param name="header">The header to return. </param>
		public override string[] GetValues(string header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			string[] values = base.GetValues(header);
			if (values == null || values.Length == 0)
			{
				return null;
			}
			return values;
		}

		/// <summary>Gets an array of header values stored in the <paramref name="index" /> position of the header collection.</summary>
		/// <returns>An array of header strings.</returns>
		/// <param name="index">The header index to return.</param>
		public override string[] GetValues(int index)
		{
			string[] values = base.GetValues(index);
			if (values == null || values.Length == 0)
			{
				return null;
			}
			return values;
		}

		/// <summary>Tests whether the specified HTTP header can be set for the request.</summary>
		/// <returns>true if the header is restricted; otherwise false.</returns>
		/// <param name="headerName">The header to test. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="headerName" /> is null or <see cref="F:System.String.Empty" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="headerName" /> contains invalid characters. </exception>
		public static bool IsRestricted(string headerName)
		{
			if (headerName == null)
			{
				throw new ArgumentNullException("headerName");
			}
			if (headerName == string.Empty)
			{
				throw new ArgumentException("empty string", "headerName");
			}
			if (!IsHeaderName(headerName))
			{
				throw new ArgumentException("Invalid character in header");
			}
			return restricted.ContainsKey(headerName);
		}

		/// <summary>Tests whether the specified HTTP header can be set for the request or the response.</summary>
		/// <returns>true if the header is restricted; otherwise, false.</returns>
		/// <param name="headerName">The header to test.</param>
		/// <param name="response">Does the Framework test the response or the request?</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="headerName" /> is null or <see cref="F:System.String.Empty" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="headerName" /> contains invalid characters. </exception>
		public static bool IsRestricted(string headerName, bool response)
		{
			if (string.IsNullOrEmpty(headerName))
			{
				throw new ArgumentNullException("headerName");
			}
			if (!IsHeaderName(headerName))
			{
				throw new ArgumentException("Invalid character in header");
			}
			if (response)
			{
				return restricted_response.ContainsKey(headerName);
			}
			return restricted.ContainsKey(headerName);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		public override void OnDeserialization(object sender)
		{
		}

		/// <summary>Removes the specified header from the collection.</summary>
		/// <param name="name">The name of the header to remove from the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null<see cref="F:System.String.Empty" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is a restricted header.-or- <paramref name="name" /> contains invalid characters. </exception>
		public override void Remove(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (internallyCreated && IsRestricted(name))
			{
				throw new ArgumentException("restricted header");
			}
			base.Remove(name);
		}

		/// <summary>Sets the specified header to the specified value.</summary>
		/// <param name="name">The header to set. </param>
		/// <param name="value">The content of the header to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null or <see cref="F:System.String.Empty" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is a restricted header.-or- <paramref name="name" /> or <paramref name="value" /> contain invalid characters. </exception>
		public override void Set(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (internallyCreated && IsRestricted(name))
			{
				throw new ArgumentException("restricted header");
			}
			if (!IsHeaderName(name))
			{
				throw new ArgumentException("invalid header name");
			}
			value = ((value != null) ? value.Trim() : string.Empty);
			if (!IsHeaderValue(value))
			{
				throw new ArgumentException("invalid header value");
			}
			base.Set(name, value);
		}

		/// <summary>Converts the <see cref="T:System.Net.WebHeaderCollection" /> to a byte array..</summary>
		/// <returns>A <see cref="T:System.Byte" /> array holding the header collection.</returns>
		public byte[] ToByteArray()
		{
			return Encoding.UTF8.GetBytes(ToString());
		}

		internal string ToStringMultiValue()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = base.Count;
			for (int i = 0; i < count; i++)
			{
				string key = GetKey(i);
				if (IsMultiValue(key))
				{
					string[] values = GetValues(i);
					foreach (string value in values)
					{
						stringBuilder.Append(key).Append(": ").Append(value)
							.Append("\r\n");
					}
				}
				else
				{
					stringBuilder.Append(key).Append(": ").Append(Get(i))
						.Append("\r\n");
				}
			}
			return stringBuilder.Append("\r\n").ToString();
		}

		/// <summary>Obsolete.</summary>
		/// <returns>The <see cref="T:System.String" /> representation of the collection.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = base.Count;
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append(GetKey(i)).Append(": ").Append(Get(i))
					.Append("\r\n");
			}
			return stringBuilder.Append("\r\n").ToString();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			int count = base.Count;
			serializationInfo.AddValue("Count", count);
			for (int i = 0; i < count; i++)
			{
				serializationInfo.AddValue(i.ToString(), GetKey(i));
				serializationInfo.AddValue((count + i).ToString(), Get(i));
			}
		}

		/// <summary>Get the value of a particular header in the collection, specified by an index into the collection.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the value of the specified header.</returns>
		/// <param name="index">The zero-based index of the key to get from the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative. -or-<paramref name="index" /> exceeds the size of the collection.</exception>
		public override string Get(int index)
		{
			return base.Get(index);
		}

		/// <summary>Get the value of a particular header in the collection, specified by the name of the header.</summary>
		/// <returns>A <see cref="T:System.String" /> holding the value of the specified header.</returns>
		/// <param name="name">The name of the Web header.</param>
		public override string Get(string name)
		{
			return base.Get(name);
		}

		/// <summary>Get the header name at the specified position in the collection.</summary>
		/// <returns>A <see cref="T:System.String" /> holding the header name.</returns>
		/// <param name="index">The zero-based index of the key to get from the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative. -or-<paramref name="index" /> exceeds the size of the collection.</exception>
		public override string GetKey(int index)
		{
			return base.GetKey(index);
		}

		/// <summary>Inserts the specified header with the specified value into the collection.</summary>
		/// <param name="header">The header to add to the collection. </param>
		/// <param name="value">The content of the header. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpRequestHeader" />. </exception>
		public void Add(HttpRequestHeader header, string value)
		{
			Add(RequestHeaderToString(header), value);
		}

		/// <summary>Removes the specified header from the collection.</summary>
		/// <param name="header">The <see cref="T:System.Net.HttpRequestHeader" /> instance to remove from the collection. </param>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpRequestHeader" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Remove(HttpRequestHeader header)
		{
			Remove(RequestHeaderToString(header));
		}

		/// <summary>Sets the specified header to the specified value.</summary>
		/// <param name="header">The <see cref="T:System.Net.HttpRequestHeader" /> value to set. </param>
		/// <param name="value">The content of the header to set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpRequestHeader" />. </exception>
		public void Set(HttpRequestHeader header, string value)
		{
			Set(RequestHeaderToString(header), value);
		}

		/// <summary>Inserts the specified header with the specified value into the collection.</summary>
		/// <param name="header">The header to add to the collection. </param>
		/// <param name="value">The content of the header. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpResponseHeader" />. </exception>
		public void Add(HttpResponseHeader header, string value)
		{
			Add(ResponseHeaderToString(header), value);
		}

		/// <summary>Removes the specified header from the collection.</summary>
		/// <param name="header">The <see cref="T:System.Net.HttpResponseHeader" /> instance to remove from the collection. </param>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpResponseHeader" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Remove(HttpResponseHeader header)
		{
			Remove(ResponseHeaderToString(header));
		}

		/// <summary>Sets the specified header to the specified value.</summary>
		/// <param name="header">The <see cref="T:System.Net.HttpResponseHeader" /> value to set. </param>
		/// <param name="value">The content of the header to set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.WebHeaderCollection" /> instance does not allow instances of <see cref="T:System.Net.HttpResponseHeader" />. </exception>
		public void Set(HttpResponseHeader header, string value)
		{
			Set(ResponseHeaderToString(header), value);
		}

		private string RequestHeaderToString(HttpRequestHeader value)
		{
			switch (value)
			{
			case HttpRequestHeader.CacheControl:
				return "cache-control";
			case HttpRequestHeader.Connection:
				return "connection";
			case HttpRequestHeader.Date:
				return "date";
			case HttpRequestHeader.KeepAlive:
				return "keep-alive";
			case HttpRequestHeader.Pragma:
				return "pragma";
			case HttpRequestHeader.Trailer:
				return "trailer";
			case HttpRequestHeader.TransferEncoding:
				return "transfer-encoding";
			case HttpRequestHeader.Upgrade:
				return "upgrade";
			case HttpRequestHeader.Via:
				return "via";
			case HttpRequestHeader.Warning:
				return "warning";
			case HttpRequestHeader.Allow:
				return "allow";
			case HttpRequestHeader.ContentLength:
				return "content-length";
			case HttpRequestHeader.ContentType:
				return "content-type";
			case HttpRequestHeader.ContentEncoding:
				return "content-encoding";
			case HttpRequestHeader.ContentLanguage:
				return "content-language";
			case HttpRequestHeader.ContentLocation:
				return "content-location";
			case HttpRequestHeader.ContentMd5:
				return "content-md5";
			case HttpRequestHeader.ContentRange:
				return "content-range";
			case HttpRequestHeader.Expires:
				return "expires";
			case HttpRequestHeader.LastModified:
				return "last-modified";
			case HttpRequestHeader.Accept:
				return "accept";
			case HttpRequestHeader.AcceptCharset:
				return "accept-charset";
			case HttpRequestHeader.AcceptEncoding:
				return "accept-encoding";
			case HttpRequestHeader.AcceptLanguage:
				return "accept-language";
			case HttpRequestHeader.Authorization:
				return "authorization";
			case HttpRequestHeader.Cookie:
				return "cookie";
			case HttpRequestHeader.Expect:
				return "expect";
			case HttpRequestHeader.From:
				return "from";
			case HttpRequestHeader.Host:
				return "host";
			case HttpRequestHeader.IfMatch:
				return "if-match";
			case HttpRequestHeader.IfModifiedSince:
				return "if-modified-since";
			case HttpRequestHeader.IfNoneMatch:
				return "if-none-match";
			case HttpRequestHeader.IfRange:
				return "if-range";
			case HttpRequestHeader.IfUnmodifiedSince:
				return "if-unmodified-since";
			case HttpRequestHeader.MaxForwards:
				return "max-forwards";
			case HttpRequestHeader.ProxyAuthorization:
				return "proxy-authorization";
			case HttpRequestHeader.Referer:
				return "referer";
			case HttpRequestHeader.Range:
				return "range";
			case HttpRequestHeader.Te:
				return "te";
			case HttpRequestHeader.Translate:
				return "translate";
			case HttpRequestHeader.UserAgent:
				return "user-agent";
			default:
				throw new InvalidOperationException();
			}
		}

		private string ResponseHeaderToString(HttpResponseHeader value)
		{
			switch (value)
			{
			case HttpResponseHeader.CacheControl:
				return "cache-control";
			case HttpResponseHeader.Connection:
				return "connection";
			case HttpResponseHeader.Date:
				return "date";
			case HttpResponseHeader.KeepAlive:
				return "keep-alive";
			case HttpResponseHeader.Pragma:
				return "pragma";
			case HttpResponseHeader.Trailer:
				return "trailer";
			case HttpResponseHeader.TransferEncoding:
				return "transfer-encoding";
			case HttpResponseHeader.Upgrade:
				return "upgrade";
			case HttpResponseHeader.Via:
				return "via";
			case HttpResponseHeader.Warning:
				return "warning";
			case HttpResponseHeader.Allow:
				return "allow";
			case HttpResponseHeader.ContentLength:
				return "content-length";
			case HttpResponseHeader.ContentType:
				return "content-type";
			case HttpResponseHeader.ContentEncoding:
				return "content-encoding";
			case HttpResponseHeader.ContentLanguage:
				return "content-language";
			case HttpResponseHeader.ContentLocation:
				return "content-location";
			case HttpResponseHeader.ContentMd5:
				return "content-md5";
			case HttpResponseHeader.ContentRange:
				return "content-range";
			case HttpResponseHeader.Expires:
				return "expires";
			case HttpResponseHeader.LastModified:
				return "last-modified";
			case HttpResponseHeader.AcceptRanges:
				return "accept-ranges";
			case HttpResponseHeader.Age:
				return "age";
			case HttpResponseHeader.ETag:
				return "etag";
			case HttpResponseHeader.Location:
				return "location";
			case HttpResponseHeader.ProxyAuthenticate:
				return "proxy-authenticate";
			case HttpResponseHeader.RetryAfter:
				return "RetryAfter";
			case HttpResponseHeader.Server:
				return "server";
			case HttpResponseHeader.SetCookie:
				return "set-cookie";
			case HttpResponseHeader.Vary:
				return "vary";
			case HttpResponseHeader.WwwAuthenticate:
				return "www-authenticate";
			default:
				throw new InvalidOperationException();
			}
		}

		/// <summary>Removes all headers from the collection.</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>Returns an enumerator that can iterate through the <see cref="T:System.Net.WebHeaderCollection" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Net.WebHeaderCollection" />.</returns>
		public override IEnumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

		internal void SetInternal(string header)
		{
			int num = header.IndexOf(':');
			if (num == -1)
			{
				throw new ArgumentException("no colon found", "header");
			}
			SetInternal(header.Substring(0, num), header.Substring(num + 1));
		}

		internal void SetInternal(string name, string value)
		{
			value = ((value != null) ? value.Trim() : string.Empty);
			if (!IsHeaderValue(value))
			{
				throw new ArgumentException("invalid header value");
			}
			if (IsMultiValue(name))
			{
				base.Add(name, value);
				return;
			}
			base.Remove(name);
			base.Set(name, value);
		}

		internal void RemoveAndAdd(string name, string value)
		{
			value = ((value != null) ? value.Trim() : string.Empty);
			base.Remove(name);
			base.Set(name, value);
		}

		internal void RemoveInternal(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			base.Remove(name);
		}

		internal static bool IsMultiValue(string headerName)
		{
			if (headerName == null || headerName == string.Empty)
			{
				return false;
			}
			return multiValue.ContainsKey(headerName);
		}

		internal static bool IsHeaderValue(string value)
		{
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				char c = value[i];
				if (c == '\u007f')
				{
					return false;
				}
				if (c < ' ' && c != '\r' && c != '\n' && c != '\t')
				{
					return false;
				}
				if (c == '\n' && ++i < length)
				{
					c = value[i];
					if (c != ' ' && c != '\t')
					{
						return false;
					}
				}
			}
			return true;
		}

		internal static bool IsHeaderName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return false;
			}
			int length = name.Length;
			for (int i = 0; i < length; i++)
			{
				char c = name[i];
				if (c > '~' || !allowed_chars[c])
				{
					return false;
				}
			}
			return true;
		}
	}
}
