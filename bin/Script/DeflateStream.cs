using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.IO.Compression
{
	/// <summary>Provides methods and properties for compressing and decompressing streams using the Deflate algorithm.</summary>
	public class DeflateStream : Stream
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int UnmanagedReadOrWrite(IntPtr buffer, int length, IntPtr data);

		private delegate int ReadMethod(byte[] array, int offset, int count);

		private delegate void WriteMethod(byte[] array, int offset, int count);

		private const int BufferSize = 4096;

		private const string LIBNAME = "MonoPosixHelper";

		private Stream base_stream;

		private CompressionMode mode;

		private bool leaveOpen;

		private bool disposed;

		private UnmanagedReadOrWrite feeder;

		private IntPtr z_stream;

		private byte[] io_buffer;

		private GCHandle data;

		/// <summary>Gets a reference to the underlying stream.</summary>
		/// <returns>A stream object that represents the underlying stream.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The underlying stream is closed.</exception>
		public Stream BaseStream => base_stream;

		/// <summary>Gets a value indicating whether the stream supports reading while decompressing a file.</summary>
		/// <returns>true if the <see cref="T:System.IO.Compression.CompressionMode" /> value is Decompress, and the underlying stream is opened and supports reading; otherwise, false.</returns>
		public override bool CanRead => !disposed && mode == CompressionMode.Decompress && base_stream.CanRead;

		/// <summary>Gets a value indicating whether the stream supports seeking.</summary>
		/// <returns>false in all cases.</returns>
		public override bool CanSeek => false;

		/// <summary>Gets a value indicating whether the stream supports writing.</summary>
		/// <returns>true if the <see cref="T:System.IO.Compression.CompressionMode" /> value is Compress, and the underlying stream supports writing and is not closed; otherwise, false.</returns>
		public override bool CanWrite => !disposed && mode == CompressionMode.Compress && base_stream.CanWrite;

		/// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>A long value.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>A long value.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class using the specified stream and <see cref="T:System.IO.Compression.CompressionMode" /> value.</summary>
		/// <param name="stream">The stream to compress or decompress.</param>
		/// <param name="mode">One of the <see cref="T:System.IO.Compression.CompressionMode" /> values that indicates the action to take.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="stream" /> access right is ReadOnly and <paramref name="mode" /> value is Compress. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid <see cref="T:System.IO.Compression.CompressionMode" /> value.-or-<see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Compress" />  and <see cref="P:System.IO.Stream.CanWrite" /> is false.-or-<see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Decompress" />  and <see cref="P:System.IO.Stream.CanRead" /> is false.</exception>
		public DeflateStream(Stream compressedStream, CompressionMode mode)
			: this(compressedStream, mode, leaveOpen: false, gzip: false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class using the specified stream and <see cref="T:System.IO.Compression.CompressionMode" /> value, and a value that specifies whether to leave the stream open.</summary>
		/// <param name="stream">The stream to compress or decompress.</param>
		/// <param name="mode">One of the <see cref="T:System.IO.Compression.CompressionMode" /> values that indicates the action to take.</param>
		/// <param name="leaveOpen">true to leave the stream open; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="stream" /> access right is ReadOnly and <paramref name="mode" /> value is Compress. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a valid <see cref="T:System.IO.Compression.CompressionMode" /> value.-or-<see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Compress" />  and <see cref="P:System.IO.Stream.CanWrite" /> is false.-or-<see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Decompress" />  and <see cref="P:System.IO.Stream.CanRead" /> is false.</exception>
		public DeflateStream(Stream compressedStream, CompressionMode mode, bool leaveOpen)
			: this(compressedStream, mode, leaveOpen, gzip: false)
		{
		}

		internal DeflateStream(Stream compressedStream, CompressionMode mode, bool leaveOpen, bool gzip)
		{
			if (compressedStream == null)
			{
				throw new ArgumentNullException("compressedStream");
			}
			if (mode != CompressionMode.Compress && mode != 0)
			{
				throw new ArgumentException("mode");
			}
			data = GCHandle.Alloc(this);
			base_stream = compressedStream;
			feeder = ((mode != CompressionMode.Compress) ? new UnmanagedReadOrWrite(UnmanagedRead) : new UnmanagedReadOrWrite(UnmanagedWrite));
			z_stream = CreateZStream(mode, gzip, feeder, GCHandle.ToIntPtr(data));
			if (z_stream == IntPtr.Zero)
			{
				base_stream = null;
				feeder = null;
				throw new NotImplementedException("Failed to initialize zlib. You probably have an old zlib installed. Version 1.2.0.4 or later is required.");
			}
			this.mode = mode;
			this.leaveOpen = leaveOpen;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Compression.DeflateStream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !disposed)
			{
				disposed = true;
				IntPtr intPtr = z_stream;
				z_stream = IntPtr.Zero;
				int result = 0;
				if (intPtr != IntPtr.Zero)
				{
					result = CloseZStream(intPtr);
				}
				io_buffer = null;
				if (!leaveOpen)
				{
					base_stream?.Close();
					base_stream = null;
				}
				CheckResult(result, "Dispose");
			}
			if (data.IsAllocated)
			{
				data.Free();
				data = default(GCHandle);
			}
			base.Dispose(disposing);
		}

		private static int UnmanagedRead(IntPtr buffer, int length, IntPtr data)
		{
			return (GCHandle.FromIntPtr(data).Target as DeflateStream)?.UnmanagedRead(buffer, length) ?? (-1);
		}

		private unsafe int UnmanagedRead(IntPtr buffer, int length)
		{
			int num = 0;
			int num2 = 1;
			while (length > 0 && num2 > 0)
			{
				if (io_buffer == null)
				{
					io_buffer = new byte[4096];
				}
				int count = Math.Min(length, io_buffer.Length);
				num2 = base_stream.Read(io_buffer, 0, count);
				if (num2 > 0)
				{
					Marshal.Copy(io_buffer, 0, buffer, num2);
					buffer = new IntPtr((byte*)buffer.ToPointer() + num2);
					length -= num2;
					num += num2;
				}
			}
			return num;
		}

		private static int UnmanagedWrite(IntPtr buffer, int length, IntPtr data)
		{
			return (GCHandle.FromIntPtr(data).Target as DeflateStream)?.UnmanagedWrite(buffer, length) ?? (-1);
		}

		private unsafe int UnmanagedWrite(IntPtr buffer, int length)
		{
			int num = 0;
			while (length > 0)
			{
				if (io_buffer == null)
				{
					io_buffer = new byte[4096];
				}
				int num2 = Math.Min(length, io_buffer.Length);
				Marshal.Copy(buffer, io_buffer, 0, num2);
				base_stream.Write(io_buffer, 0, num2);
				buffer = new IntPtr((byte*)buffer.ToPointer() + num2);
				length -= num2;
				num += num2;
			}
			return num;
		}

		private unsafe int ReadInternal(byte[] array, int offset, int count)
		{
			//IL_0021: Incompatible stack types: I vs Ref
			if (count == 0)
			{
				return 0;
			}
			int num = 0;
			fixed (byte* ptr = &((array != null && array.Length != 0) ? ref array[0] : ref *(byte*)null))
			{
				num = ReadZStream(buffer: new IntPtr(ptr + offset), stream: z_stream, length: count);
			}
			CheckResult(num, "ReadInternal");
			return num;
		}

		/// <summary>Reads a specified number of compressed or decompressed bytes into the specified byte array.</summary>
		/// <returns>The number of bytes that were read into the byte array.</returns>
		/// <param name="array">The buffer to store the compressed or decompressed bytes.</param>
		/// <param name="offset">The location in the array to begin reading.</param>
		/// <param name="count">The number of compressed or decompressed bytes to read.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.Compression.CompressionMode" /> value was Compress when the object was created.- or - The underlying stream does not support reading.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is less than zero.-or-<paramref name="array" /> length minus the index starting point is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.IO.InvalidDataException">The data is in an invalid format.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		public override int Read(byte[] dest, int dest_offset, int count)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (dest == null)
			{
				throw new ArgumentNullException("Destination array is null.");
			}
			if (!CanRead)
			{
				throw new InvalidOperationException("Stream does not support reading.");
			}
			int num = dest.Length;
			if (dest_offset < 0 || count < 0)
			{
				throw new ArgumentException("Dest or count is negative.");
			}
			if (dest_offset > num)
			{
				throw new ArgumentException("destination offset is beyond array size");
			}
			if (dest_offset + count > num)
			{
				throw new ArgumentException("Reading would overrun buffer");
			}
			return ReadInternal(dest, dest_offset, count);
		}

		private unsafe void WriteInternal(byte[] array, int offset, int count)
		{
			//IL_0020: Incompatible stack types: I vs Ref
			if (count != 0)
			{
				int num = 0;
				fixed (byte* ptr = &((array != null && array.Length != 0) ? ref array[0] : ref *(byte*)null))
				{
					num = WriteZStream(buffer: new IntPtr(ptr + offset), stream: z_stream, length: count);
				}
				CheckResult(num, "WriteInternal");
			}
		}

		/// <summary>Writes compressed or decompressed bytes to the underlying stream from the specified byte array.</summary>
		/// <param name="array">The buffer that contains the data to compress or decompress.</param>
		/// <param name="offset">The byte offset in <paramref name="array" /> at which the read bytes will be placed.</param>
		/// <param name="count">The maximum number of bytes to compress.</param>
		public override void Write(byte[] src, int src_offset, int count)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			if (src_offset < 0)
			{
				throw new ArgumentOutOfRangeException("src_offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (!CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing");
			}
			WriteInternal(src, src_offset, count);
		}

		private static void CheckResult(int result, string where)
		{
			if (result >= 0)
			{
				return;
			}
			string str;
			switch (result)
			{
			case -1:
				str = "Unknown error";
				break;
			case -2:
				str = "Internal error";
				break;
			case -3:
				str = "Corrupted data";
				break;
			case -4:
				str = "Not enough memory";
				break;
			case -5:
				str = "Internal error (no progress possible)";
				break;
			case -6:
				str = "Invalid version";
				break;
			case -10:
				str = "Invalid argument(s)";
				break;
			case -11:
				str = "IO error";
				break;
			default:
				str = "Unknown error";
				break;
			}
			throw new IOException(str + " " + where);
		}

		/// <summary>Flushes the contents of the internal buffer of the current stream object to the underlying stream.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Flush()
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (CanWrite)
			{
				int result = Flush(z_stream);
				CheckResult(result, "Flush");
			}
		}

		/// <summary>Begins an asynchronous read operation.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous read, which could still be pending.</returns>
		/// <param name="array">The byte array to read the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="array" /> at which to begin writing data read from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="asyncCallback">An optional asynchronous callback, to be called when the read is complete.</param>
		/// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
		/// <exception cref="T:System.IO.IOException">An asynchronous read past the end of the stream was attempted, or a disk error occurred.</exception>
		/// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.DeflateStream" /> implementation does not support the read operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">This call cannot be completed. </exception>
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (!CanRead)
			{
				throw new NotSupportedException("This stream does not support reading");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			if (count + offset > buffer.Length)
			{
				throw new ArgumentException("Buffer too small. count/offset wrong.");
			}
			ReadMethod readMethod = ReadInternal;
			return readMethod.BeginInvoke(buffer, offset, count, cback, state);
		}

		/// <summary>Begins an asynchronous write operation.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous write, which could still be pending.</returns>
		/// <param name="array">The buffer to write data from.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> to begin writing from.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <param name="asyncCallback">An optional asynchronous callback, to be called when the write is complete.</param>
		/// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
		/// <exception cref="T:System.IO.IOException">An asynchronous write past the end of the stream was attempted, or a disk error occurred.</exception>
		/// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.DeflateStream" /> implementation does not support the write operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">The write operation cannot be performed because the stream is closed.</exception>
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (!CanWrite)
			{
				throw new InvalidOperationException("This stream does not support writing");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			if (count + offset > buffer.Length)
			{
				throw new ArgumentException("Buffer too small. count/offset wrong.");
			}
			WriteMethod writeMethod = WriteInternal;
			return writeMethod.BeginInvoke(buffer, offset, count, cback, state);
		}

		/// <summary>Waits for the pending asynchronous read to complete.</summary>
		/// <returns>The number of bytes read from the stream, between zero (0) and the number of bytes you requested. <see cref="T:System.IO.Compression.DeflateStream" /> returns zero (0) only at the end of the stream; otherwise, it blocks until at least one byte is available.</returns>
		/// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.DeflateStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
		/// <exception cref="T:System.SystemException">An exception was thrown during a call to <see cref="M:System.Threading.WaitHandle.WaitOne" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The end call is invalid because asynchronous read operations for this stream are not yet complete.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is null.</exception>
		public override int EndRead(IAsyncResult async_result)
		{
			if (async_result == null)
			{
				throw new ArgumentNullException("async_result");
			}
			AsyncResult asyncResult = async_result as AsyncResult;
			if (asyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "async_result");
			}
			ReadMethod readMethod = asyncResult.AsyncDelegate as ReadMethod;
			if (readMethod == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "async_result");
			}
			return readMethod.EndInvoke(async_result);
		}

		/// <summary>Ends an asynchronous write operation.</summary>
		/// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.DeflateStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
		/// <exception cref="T:System.Exception">An exception was thrown during a call to <see cref="M:System.Threading.WaitHandle.WaitOne" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The end write call is invalid.</exception>
		public override void EndWrite(IAsyncResult async_result)
		{
			if (async_result == null)
			{
				throw new ArgumentNullException("async_result");
			}
			AsyncResult asyncResult = async_result as AsyncResult;
			if (asyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "async_result");
			}
			WriteMethod writeMethod = asyncResult.AsyncDelegate as WriteMethod;
			if (writeMethod == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "async_result");
			}
			writeMethod.EndInvoke(async_result);
		}

		/// <summary>This operation is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>A long value.</returns>
		/// <param name="offset">The location in the stream.</param>
		/// <param name="origin">One of the <see cref="T:System.IO.SeekOrigin" /> values.</param>
		/// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		/// <summary>This operation is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The length of the stream.</param>
		/// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr CreateZStream(CompressionMode compress, bool gzip, UnmanagedReadOrWrite feeder, IntPtr data);

		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl)]
		private static extern int CloseZStream(IntPtr stream);

		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl)]
		private static extern int Flush(IntPtr stream);

		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl)]
		private static extern int ReadZStream(IntPtr stream, IntPtr buffer, int length);

		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl)]
		private static extern int WriteZStream(IntPtr stream, IntPtr buffer, int length);
	}
}
