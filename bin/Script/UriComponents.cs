namespace System
{
	/// <summary>Specifies the parts of a <see cref="T:System.Uri" />.</summary>
	/// <filterpriority>1</filterpriority>
	[Flags]
	public enum UriComponents
	{
		/// <summary>The <see cref="P:System.Uri.Scheme" /> data.</summary>
		Scheme = 0x1,
		/// <summary>The <see cref="P:System.Uri.UserInfo" /> data.</summary>
		UserInfo = 0x2,
		/// <summary>The <see cref="P:System.Uri.Host" /> data.</summary>
		Host = 0x4,
		/// <summary>The <see cref="P:System.Uri.Port" /> data.</summary>
		Port = 0x8,
		/// <summary>The <see cref="P:System.Uri.LocalPath" /> data.</summary>
		Path = 0x10,
		/// <summary>The <see cref="P:System.Uri.Query" /> data.</summary>
		Query = 0x20,
		/// <summary>The <see cref="P:System.Uri.Fragment" /> data.</summary>
		Fragment = 0x40,
		/// <summary>The <see cref="P:System.Uri.Port" /> data. If no port data is in the <see cref="T:System.Uri" /> and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		StrongPort = 0x80,
		/// <summary>Specifies that the delimiter should be included.</summary>
		KeepDelimiter = 0x40000000,
		/// <summary>The <see cref="P:System.Uri.Host" /> and <see cref="P:System.Uri.Port" /> data. If no port data is in the Uri and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		HostAndPort = 0x84,
		/// <summary>The <see cref="P:System.Uri.UserInfo" />, <see cref="P:System.Uri.Host" />, and <see cref="P:System.Uri.Port" /> data. If no port data is in the <see cref="T:System.Uri" /> and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		StrongAuthority = 0x86,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.UserInfo" />, <see cref="P:System.Uri.Host" />, <see cref="P:System.Uri.Port" />, <see cref="P:System.Uri.LocalPath" />, <see cref="P:System.Uri.Query" />, and <see cref="P:System.Uri.Fragment" /> data.</summary>
		AbsoluteUri = 0x7F,
		/// <summary>The <see cref="P:System.Uri.LocalPath" /> and <see cref="P:System.Uri.Query" /> data. Also see <see cref="P:System.Uri.PathAndQuery" />. </summary>
		PathAndQuery = 0x30,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.Host" />, <see cref="P:System.Uri.Port" />, <see cref="P:System.Uri.LocalPath" />, and <see cref="P:System.Uri.Query" /> data.</summary>
		HttpRequestUrl = 0x3D,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.Host" />, and <see cref="P:System.Uri.Port" /> data.</summary>
		SchemeAndServer = 0xD,
		/// <summary>The complete <see cref="T:System.Uri" /> context that is needed for Uri Serializers. The context includes the IPv6 scope.</summary>
		SerializationInfoString = int.MinValue
	}
}
