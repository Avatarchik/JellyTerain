namespace System.Net
{
	/// <summary>Represents the file compression and decompression encoding format to be used to compress the data received in response to an <see cref="T:System.Net.HttpWebRequest" />.</summary>
	[Flags]
	public enum DecompressionMethods
	{
		/// <summary>Do not use compression.</summary>
		None = 0x0,
		/// <summary>Use the gZip compression-decompression algorithm.</summary>
		GZip = 0x1,
		/// <summary>Use the deflate compression-decompression algorithm.</summary>
		Deflate = 0x2
	}
}
