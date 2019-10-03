using System.Collections.Generic;
using System.Threading;

namespace System.Net.Sockets
{
	/// <summary>Represents an asynchronous socket operation.</summary>
	public class SocketAsyncEventArgs : EventArgs, IDisposable
	{
		private IList<ArraySegment<byte>> _bufferList;

		private Socket curSocket;

		/// <summary>Gets or sets the socket to use or the socket created for accepting a connection with an asynchronous socket method.</summary>
		/// <returns>The <see cref="T:System.Net.Sockets.Socket" /> to use or the socket created for accepting a connection with an asynchronous socket method.</returns>
		public Socket AcceptSocket
		{
			get;
			set;
		}

		/// <summary>Gets the data buffer to use with an asynchronous socket method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array that represents the data buffer to use with an asynchronous socket method.</returns>
		public byte[] Buffer
		{
			get;
			private set;
		}

		/// <summary>Gets or sets an array of data buffers to use with an asynchronous socket method.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that represents an array of data buffers to use with an asynchronous socket method.</returns>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified on a set operation. This exception occurs if a value other than null is passed and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null.</exception>
		[MonoTODO("not supported in all cases")]
		public IList<ArraySegment<byte>> BufferList
		{
			get
			{
				return _bufferList;
			}
			set
			{
				if (Buffer != null && value != null)
				{
					throw new ArgumentException("Buffer and BufferList properties cannot both be non-null.");
				}
				_bufferList = value;
			}
		}

		/// <summary>Gets the number of bytes transferred in the socket operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the number of bytes transferred in the socket operation.</returns>
		public int BytesTransferred
		{
			get;
			private set;
		}

		/// <summary>Gets the maximum amount of data, in bytes, to send or receive in an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the maximum amount of data, in bytes, to send or receive.</returns>
		public int Count
		{
			get;
			private set;
		}

		/// <summary>Gets or sets a value that specifies if socket can be reused after a disconnect operation.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> that specifies if socket can be reused after a disconnect operation.</returns>
		public bool DisconnectReuseSocket
		{
			get;
			set;
		}

		/// <summary>Gets the type of socket operation most recently performed with this context object.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketAsyncOperation" /> instance that indicates the type of socket operation most recently performed with this context object.</returns>
		public SocketAsyncOperation LastOperation
		{
			get;
			private set;
		}

		/// <summary>Gets the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</returns>
		public int Offset
		{
			get;
			private set;
		}

		/// <summary>Gets or sets the remote IP endpoint for an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Net.EndPoint" /> that represents the remote IP endpoint for an asynchronous operation.</returns>
		public EndPoint RemoteEndPoint
		{
			get;
			set;
		}

		/// <summary>Gets or sets the size, in bytes, of the data block used in the send operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the data block used in the send operation.</returns>
		[MonoTODO("unused property")]
		public int SendPacketsSendSize
		{
			get;
			set;
		}

		/// <summary>Gets or sets the result of the asynchronous socket operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketError" /> that represents the result of the asynchronous socket operation.</returns>
		public SocketError SocketError
		{
			get;
			set;
		}

		/// <summary>Gets the results of an asynchronous socket operation or sets the behavior of an asynchronous operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketFlags" /> that represents the results of an asynchronous socket operation.</returns>
		public SocketFlags SocketFlags
		{
			get;
			set;
		}

		/// <summary>Gets or sets a user or application object associated with this asynchronous socket operation.</summary>
		/// <returns>An object that represents the user or application object associated with this asynchronous socket operation.</returns>
		public object UserToken
		{
			get;
			set;
		}

		public Socket ConnectSocket
		{
			get
			{
				SocketError socketError = SocketError;
				if (socketError == SocketError.AccessDenied)
				{
					return null;
				}
				return curSocket;
			}
		}

		internal bool PolicyRestricted
		{
			get;
			private set;
		}

		/// <summary>The event used to complete an asynchronous operation.</summary>
		public event EventHandler<SocketAsyncEventArgs> Completed;

		internal SocketAsyncEventArgs(bool policy)
			: this()
		{
			PolicyRestricted = policy;
		}

		/// <summary>Creates an empty <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance.</summary>
		/// <exception cref="T:System.NotSupportedException">The platform is not supported. </exception>
		public SocketAsyncEventArgs()
		{
			AcceptSocket = null;
			Buffer = null;
			BufferList = null;
			BytesTransferred = 0;
			Count = 0;
			DisconnectReuseSocket = false;
			LastOperation = SocketAsyncOperation.None;
			Offset = 0;
			RemoteEndPoint = null;
			SendPacketsSendSize = -1;
			SocketError = SocketError.Success;
			SocketFlags = SocketFlags.None;
			UserToken = null;
		}

		/// <summary>Frees resources used by the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> class.</summary>
		~SocketAsyncEventArgs()
		{
			Dispose(disposing: false);
		}

		private void Dispose(bool disposing)
		{
			AcceptSocket?.Close();
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance and optionally disposes of the managed resources.</summary>
		public void Dispose()
		{
			Dispose(disposing: true);
		}

		/// <summary>Represents a method that is called when an asynchronous operation completes.</summary>
		/// <param name="e">The event that is signaled.</param>
		protected virtual void OnCompleted(SocketAsyncEventArgs e)
		{
			e?.Completed?.Invoke(e.curSocket, e);
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is also not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		public void SetBuffer(int offset, int count)
		{
			SetBufferInternal(Buffer, offset, count);
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is also not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		public void SetBuffer(byte[] buffer, int offset, int count)
		{
			SetBufferInternal(buffer, offset, count);
		}

		private void SetBufferInternal(byte[] buffer, int offset, int count)
		{
			if (buffer != null)
			{
				if (BufferList != null)
				{
					throw new ArgumentException("Buffer and BufferList properties cannot both be non-null.");
				}
				int num = buffer.Length;
				if (offset < 0 || (offset != 0 && offset >= num))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > num - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				Count = count;
				Offset = offset;
			}
			Buffer = buffer;
		}

		private void ReceiveCallback()
		{
			SocketError = SocketError.Success;
			LastOperation = SocketAsyncOperation.Receive;
			SocketError error = SocketError.Success;
			if (!curSocket.Connected)
			{
				SocketError = SocketError.NotConnected;
			}
			else
			{
				try
				{
					BytesTransferred = curSocket.Receive_nochecks(Buffer, Offset, Count, SocketFlags, out error);
				}
				finally
				{
					SocketError = error;
					OnCompleted(this);
				}
			}
		}

		private void ConnectCallback()
		{
			LastOperation = SocketAsyncOperation.Connect;
			SocketError socketError = SocketError.AccessDenied;
			try
			{
				socketError = TryConnect(RemoteEndPoint);
			}
			finally
			{
				SocketError = socketError;
				OnCompleted(this);
			}
		}

		private SocketError TryConnect(EndPoint endpoint)
		{
			curSocket.Connected = false;
			SocketError result = SocketError.Success;
			try
			{
				curSocket.seed_endpoint = endpoint;
				curSocket.Connect(endpoint);
				curSocket.Connected = true;
				return result;
			}
			catch (SocketException ex)
			{
				return ex.SocketErrorCode;
			}
		}

		private void SendCallback()
		{
			SocketError = SocketError.Success;
			LastOperation = SocketAsyncOperation.Send;
			SocketError error = SocketError.Success;
			if (!curSocket.Connected)
			{
				SocketError = SocketError.NotConnected;
			}
			else
			{
				try
				{
					if (Buffer != null)
					{
						BytesTransferred = curSocket.Send_nochecks(Buffer, Offset, Count, SocketFlags.None, out error);
					}
					else if (BufferList != null)
					{
						BytesTransferred = 0;
						foreach (ArraySegment<byte> buffer in BufferList)
						{
							BytesTransferred += curSocket.Send_nochecks(buffer.Array, buffer.Offset, buffer.Count, SocketFlags.None, out error);
							if (error != 0)
							{
								break;
							}
						}
					}
				}
				finally
				{
					SocketError = error;
					OnCompleted(this);
				}
			}
		}

		internal void DoOperation(SocketAsyncOperation operation, Socket socket)
		{
			curSocket = socket;
			ThreadStart start;
			switch (operation)
			{
			case SocketAsyncOperation.Receive:
				start = ReceiveCallback;
				break;
			case SocketAsyncOperation.Connect:
				start = ConnectCallback;
				break;
			case SocketAsyncOperation.Send:
				start = SendCallback;
				break;
			default:
				throw new NotSupportedException();
			}
			Thread thread = new Thread(start);
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
