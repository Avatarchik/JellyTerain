namespace System.Net.Sockets
{
	/// <summary>Specifies socket send and receive behaviors.</summary>
	[Flags]
	public enum SocketFlags
	{
		/// <summary>Use no flags for this call.</summary>
		None = 0x0,
		/// <summary>Process out-of-band data.</summary>
		OutOfBand = 0x1,
		/// <summary>Peek at the incoming message.</summary>
		Peek = 0x2,
		/// <summary>Send without using routing tables.</summary>
		DontRoute = 0x4,
		/// <summary>Provides a standard value for the number of WSABUF structures that are used to send and receive data.</summary>
		MaxIOVectorLength = 0x10,
		/// <summary>The message was too large to fit into the specified buffer and was truncated.</summary>
		Truncated = 0x100,
		/// <summary>Indicates that the control data did not fit into an internal 64-KB buffer and was truncated.</summary>
		ControlDataTruncated = 0x200,
		/// <summary>Indicates a broadcast packet.</summary>
		Broadcast = 0x400,
		/// <summary>Indicates a multicast packet.</summary>
		Multicast = 0x800,
		/// <summary>Partial send or receive for message.</summary>
		Partial = 0x8000
	}
}
