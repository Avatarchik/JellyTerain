using System.ComponentModel;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		private long received;

		private long sent;

		private long total_recv;

		private long total_send;

		/// <summary>Gets the number of bytes received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes received.</returns>
		public long BytesReceived => received;

		/// <summary>Gets the total number of bytes in a <see cref="T:System.Net.WebClient" /> data upload operation.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes that will be received.</returns>
		public long TotalBytesToReceive => total_recv;

		/// <summary>Gets the number of bytes sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes sent.</returns>
		public long BytesSent => sent;

		/// <summary>Gets the total number of bytes to send.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of bytes that will be sent.</returns>
		public long TotalBytesToSend => total_send;

		internal UploadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, long bytesSent, long totalBytesToSend, int progressPercentage, object userState)
			: base(progressPercentage, userState)
		{
			received = bytesReceived;
			total_recv = totalBytesToReceive;
			sent = bytesSent;
			total_send = totalBytesToSend;
		}
	}
}
