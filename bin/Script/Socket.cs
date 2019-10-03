using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net.Sockets
{
	/// <summary>Implements the Berkeley sockets interface.</summary>
	public class Socket : IDisposable
	{
		private enum SocketOperation
		{
			Accept,
			Connect,
			Receive,
			ReceiveFrom,
			Send,
			SendTo,
			UsedInManaged1,
			UsedInManaged2,
			UsedInProcess,
			UsedInConsole2,
			Disconnect,
			AcceptReceive,
			ReceiveGeneric,
			SendGeneric
		}

		private struct WSABUF
		{
			public int len;

			public IntPtr buf;
		}

		[StructLayout(LayoutKind.Sequential)]
		private sealed class SocketAsyncResult : IAsyncResult
		{
			public Socket Sock;

			public IntPtr handle;

			private object state;

			private AsyncCallback callback;

			private WaitHandle waithandle;

			private Exception delayedException;

			public EndPoint EndPoint;

			public byte[] Buffer;

			public int Offset;

			public int Size;

			public SocketFlags SockFlags;

			public Socket AcceptSocket;

			public IPAddress[] Addresses;

			public int Port;

			public IList<ArraySegment<byte>> Buffers;

			public bool ReuseSocket;

			private Socket acc_socket;

			private int total;

			private bool completed_sync;

			private bool completed;

			public bool blocking;

			internal int error;

			private SocketOperation operation;

			public object ares;

			public int EndCalled;

			public object AsyncState => state;

			public WaitHandle AsyncWaitHandle
			{
				get
				{
					lock (this)
					{
						if (waithandle == null)
						{
							waithandle = new ManualResetEvent(completed);
						}
					}
					return waithandle;
				}
				set
				{
					waithandle = value;
				}
			}

			public bool CompletedSynchronously => completed_sync;

			public bool IsCompleted
			{
				get
				{
					return completed;
				}
				set
				{
					completed = value;
					lock (this)
					{
						if (waithandle != null && value)
						{
							((ManualResetEvent)waithandle).Set();
						}
					}
				}
			}

			public Socket Socket => acc_socket;

			public int Total
			{
				get
				{
					return total;
				}
				set
				{
					total = value;
				}
			}

			public SocketError ErrorCode
			{
				get
				{
					SocketException ex = delayedException as SocketException;
					if (ex != null)
					{
						return ex.SocketErrorCode;
					}
					if (error != 0)
					{
						return (SocketError)error;
					}
					return SocketError.Success;
				}
			}

			public SocketAsyncResult(Socket sock, object state, AsyncCallback callback, SocketOperation operation)
			{
				Sock = sock;
				blocking = sock.blocking;
				handle = sock.socket;
				this.state = state;
				this.callback = callback;
				this.operation = operation;
				SockFlags = SocketFlags.None;
			}

			public void CheckIfThrowDelayedException()
			{
				if (delayedException != null)
				{
					Sock.connected = false;
					throw delayedException;
				}
				if (error != 0)
				{
					Sock.connected = false;
					throw new SocketException(error);
				}
			}

			private void CompleteAllOnDispose(Queue queue)
			{
				object[] array = queue.ToArray();
				queue.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					SocketAsyncResult @object = (SocketAsyncResult)array[i];
					WaitCallback callBack = @object.CompleteDisposed;
					ThreadPool.QueueUserWorkItem(callBack, null);
				}
				if (array.Length == 0)
				{
					Buffer = null;
				}
			}

			private void CompleteDisposed(object unused)
			{
				Complete();
			}

			public void Complete()
			{
				if (operation != SocketOperation.Receive && Sock.disposed)
				{
					delayedException = new ObjectDisposedException(Sock.GetType().ToString());
				}
				IsCompleted = true;
				Queue queue = null;
				if (operation == SocketOperation.Receive || operation == SocketOperation.ReceiveFrom || operation == SocketOperation.ReceiveGeneric)
				{
					queue = Sock.readQ;
				}
				else if (operation == SocketOperation.Send || operation == SocketOperation.SendTo || operation == SocketOperation.SendGeneric)
				{
					queue = Sock.writeQ;
				}
				if (queue != null)
				{
					SocketAsyncCall socketAsyncCall = null;
					SocketAsyncResult socketAsyncResult = null;
					lock (queue)
					{
						queue.Dequeue();
						if (queue.Count > 0)
						{
							socketAsyncResult = (SocketAsyncResult)queue.Peek();
							if (!Sock.disposed)
							{
								Worker worker = new Worker(socketAsyncResult);
								socketAsyncCall = GetDelegate(worker, socketAsyncResult.operation);
							}
							else
							{
								CompleteAllOnDispose(queue);
							}
						}
					}
					socketAsyncCall?.BeginInvoke(null, socketAsyncResult);
				}
				if (callback != null)
				{
					callback(this);
				}
				Buffer = null;
			}

			private SocketAsyncCall GetDelegate(Worker worker, SocketOperation op)
			{
				switch (op)
				{
				case SocketOperation.Receive:
					return worker.Receive;
				case SocketOperation.ReceiveFrom:
					return worker.ReceiveFrom;
				case SocketOperation.Send:
					return worker.Send;
				case SocketOperation.SendTo:
					return worker.SendTo;
				default:
					return null;
				}
			}

			public void Complete(bool synch)
			{
				completed_sync = synch;
				Complete();
			}

			public void Complete(int total)
			{
				this.total = total;
				Complete();
			}

			public void Complete(Exception e, bool synch)
			{
				completed_sync = synch;
				delayedException = e;
				Complete();
			}

			public void Complete(Exception e)
			{
				delayedException = e;
				Complete();
			}

			public void Complete(Socket s)
			{
				acc_socket = s;
				Complete();
			}

			public void Complete(Socket s, int total)
			{
				acc_socket = s;
				this.total = total;
				Complete();
			}
		}

		private sealed class Worker
		{
			private SocketAsyncResult result;

			private bool requireSocketSecurity;

			private int send_so_far;

			public Worker(SocketAsyncResult ares)
				: this(ares, requireSocketSecurity: true)
			{
			}

			public Worker(SocketAsyncResult ares, bool requireSocketSecurity)
			{
				result = ares;
				this.requireSocketSecurity = requireSocketSecurity;
			}

			public void Accept()
			{
				Socket socket = null;
				try
				{
					socket = result.Sock.Accept();
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_002a:;
				}
				result.Complete(socket);
			}

			public void AcceptReceive()
			{
				Socket socket = null;
				try
				{
					if (result.AcceptSocket == null)
					{
						socket = result.Sock.Accept();
					}
					else
					{
						socket = result.AcceptSocket;
						result.Sock.Accept(socket);
					}
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_005c:;
				}
				int total = 0;
				if (result.Size > 0)
				{
					try
					{
						total = socket.Receive_nochecks(result.Buffer, result.Offset, result.Size, result.SockFlags, out SocketError _);
					}
					catch (Exception e2)
					{
						result.Complete(e2);
						return;
						IL_00c2:;
					}
				}
				result.Complete(socket, total);
			}

			public void Connect()
			{
				if (result.EndPoint != null)
				{
					try
					{
						if (!result.Sock.Blocking)
						{
							result.Sock.Poll(-1, SelectMode.SelectWrite, out int socket_error);
							if (socket_error != 0)
							{
								result.Complete(new SocketException(socket_error));
								return;
							}
							result.Sock.connected = true;
						}
						else
						{
							result.Sock.seed_endpoint = result.EndPoint;
							result.Sock.Connect(result.EndPoint, requireSocketSecurity);
							result.Sock.connected = true;
						}
					}
					catch (Exception e)
					{
						result.Complete(e);
						return;
						IL_00d5:;
					}
					result.Complete();
				}
				else if (result.Addresses != null)
				{
					int error = 10036;
					IPAddress[] addresses = result.Addresses;
					foreach (IPAddress address in addresses)
					{
						IPEndPoint iPEndPoint = new IPEndPoint(address, result.Port);
						SocketAddress sa = iPEndPoint.Serialize();
						try
						{
							Connect_internal(result.Sock.socket, sa, out error, requireSocketSecurity);
						}
						catch (Exception e2)
						{
							result.Complete(e2);
							return;
							IL_016f:;
						}
						switch (error)
						{
						case 0:
							result.Sock.connected = true;
							result.Sock.seed_endpoint = iPEndPoint;
							result.Complete();
							return;
						case 10035:
						case 10036:
							if (!result.Sock.Blocking)
							{
								result.Sock.Poll(-1, SelectMode.SelectWrite, out int socket_error2);
								if (socket_error2 == 0)
								{
									result.Sock.connected = true;
									result.Sock.seed_endpoint = iPEndPoint;
									result.Complete();
									return;
								}
							}
							break;
						}
					}
					result.Complete(new SocketException(error));
				}
				else
				{
					result.Complete(new SocketException(10049));
				}
			}

			public void Disconnect()
			{
				try
				{
					result.Sock.Disconnect(result.ReuseSocket);
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_0032:;
				}
				result.Complete();
			}

			public void Receive()
			{
				result.Complete();
			}

			public void ReceiveFrom()
			{
				int num = 0;
				try
				{
					num = result.Sock.ReceiveFrom_nochecks(result.Buffer, result.Offset, result.Size, result.SockFlags, ref result.EndPoint);
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_0061:;
				}
				result.Complete(num);
			}

			public void ReceiveGeneric()
			{
				int num = 0;
				try
				{
					num = result.Sock.Receive(result.Buffers, result.SockFlags, out SocketError _);
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_0042:;
				}
				result.Complete(num);
			}

			private void UpdateSendValues(int last_sent)
			{
				if (result.error == 0)
				{
					send_so_far += last_sent;
					result.Offset += last_sent;
					result.Size -= last_sent;
				}
			}

			public void Send()
			{
				if (result.error == 0)
				{
					UpdateSendValues(result.Total);
					if (result.Sock.disposed)
					{
						result.Complete();
						return;
					}
					if (result.Size > 0)
					{
						SocketAsyncCall socketAsyncCall = Send;
						socketAsyncCall.BeginInvoke(null, result);
						return;
					}
					result.Total = send_so_far;
				}
				result.Complete();
			}

			public void SendTo()
			{
				int num = 0;
				try
				{
					num = result.Sock.SendTo_nochecks(result.Buffer, result.Offset, result.Size, result.SockFlags, result.EndPoint);
					UpdateSendValues(num);
					if (result.Size > 0)
					{
						SocketAsyncCall socketAsyncCall = SendTo;
						socketAsyncCall.BeginInvoke(null, result);
						return;
					}
					result.Total = send_so_far;
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_00aa:;
				}
				result.Complete();
			}

			public void SendGeneric()
			{
				int num = 0;
				try
				{
					num = result.Sock.Send(result.Buffers, result.SockFlags, out SocketError _);
				}
				catch (Exception e)
				{
					result.Complete(e);
					return;
					IL_0042:;
				}
				result.Complete(num);
			}
		}

		private sealed class SendFileAsyncResult : IAsyncResult
		{
			private IAsyncResult ares;

			private SendFileHandler d;

			public object AsyncState => ares.AsyncState;

			public WaitHandle AsyncWaitHandle => ares.AsyncWaitHandle;

			public bool CompletedSynchronously => ares.CompletedSynchronously;

			public bool IsCompleted => ares.IsCompleted;

			public SendFileHandler Delegate => d;

			public IAsyncResult Original => ares;

			public SendFileAsyncResult(SendFileHandler d, IAsyncResult ares)
			{
				this.d = d;
				this.ares = ares;
			}
		}

		private delegate void SocketAsyncCall();

		private delegate void SendFileHandler(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);

		private Queue readQ = new Queue(2);

		private Queue writeQ = new Queue(2);

		private bool islistening;

		private bool useoverlappedIO;

		private readonly int MinListenPort = 7100;

		private readonly int MaxListenPort = 7150;

		private static int ipv4Supported;

		private static int ipv6Supported;

		private int linger_timeout;

		private IntPtr socket;

		private AddressFamily address_family;

		private SocketType socket_type;

		private ProtocolType protocol_type;

		internal bool blocking = true;

		private Thread blocking_thread;

		private bool isbound;

		private static int current_bind_count;

		private readonly int max_bind_count = 50;

		private bool connected;

		private bool closed;

		internal bool disposed;

		internal EndPoint seed_endpoint;

		private static MethodInfo check_socket_policy;

		/// <summary>Gets the amount of data that has been received from the network and is available to be read.</summary>
		/// <returns>The number of bytes of data received from the network and available to be read.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public int Available
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				int error;
				int result = Available_internal(socket, out error);
				if (error != 0)
				{
					throw new SocketException(error);
				}
				return result;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> allows Internet Protocol (IP) datagrams to be fragmented.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> allows datagram fragmentation; otherwise, false. The default is true.</returns>
		/// <exception cref="T:System.NotSupportedException">This property can be set only for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool DontFragment
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					return (int)GetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment) != 0;
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					return (int)GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DontFragment) != 0;
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, value ? 1 : 0);
					return;
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DontFragment, value ? 1 : 0);
					return;
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> can send or receive broadcast packets.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> allows broadcast packets; otherwise, false. The default is false.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">This option is valid for a datagram socket only. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool EnableBroadcast
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (protocol_type != ProtocolType.Udp)
				{
					throw new SocketException(10042);
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast) != 0;
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (protocol_type != ProtocolType.Udp)
				{
					throw new SocketException(10042);
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> allows only one process to bind to a port.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> allows only one socket to bind to a specific port; otherwise, false. The default is true for Windows Server 2003 and Windows XP Service Pack 2, and false for all other versions.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> has been called for this <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool ExclusiveAddressUse
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse) != 0;
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (isbound)
				{
					throw new InvalidOperationException("Bind has already been called for this socket");
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, value ? 1 : 0);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Net.Sockets.Socket" /> is bound to a specific local port.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> is bound to a local port; otherwise, false.</returns>
		public bool IsBound => isbound;

		/// <summary>Gets or sets a value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> will delay closing a socket in an attempt to send all pending data.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.LingerOption" /> that specifies how to linger while closing a socket.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public LingerOption LingerState
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (LingerOption)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		/// <summary>Gets or sets a value that specifies whether outgoing multicast packets are delivered to the sending application.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> receives outgoing multicast packets; otherwise, false.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool MulticastLoopback
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (protocol_type == ProtocolType.Tcp)
				{
					throw new SocketException(10042);
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					return (int)GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback) != 0;
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					return (int)GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback) != 0;
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (protocol_type == ProtocolType.Tcp)
				{
					throw new SocketException(10042);
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, value ? 1 : 0);
					return;
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, value ? 1 : 0);
					return;
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
		}

		/// <summary>Specifies whether the socket should only use Overlapped I/O mode.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> uses only overlapped I/O; otherwise, false. The default is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The socket has been bound to a completion port.</exception>
		[MonoTODO("This doesn't do anything on Mono yet")]
		public bool UseOnlyOverlappedIO
		{
			get
			{
				return useoverlappedIO;
			}
			set
			{
				useoverlappedIO = value;
			}
		}

		/// <summary>Gets the operating system handle for the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the operating system handle for the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IntPtr Handle => socket;

		/// <summary>Gets the local endpoint.</summary>
		/// <returns>The <see cref="T:System.Net.EndPoint" /> that the <see cref="T:System.Net.Sockets.Socket" /> is using for communications.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public EndPoint LocalEndPoint
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (seed_endpoint == null)
				{
					return null;
				}
				int error;
				SocketAddress address = LocalEndPoint_internal(socket, out error);
				if (error != 0)
				{
					throw new SocketException(error);
				}
				return seed_endpoint.Create(address);
			}
		}

		/// <summary>Gets the type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</returns>
		public SocketType SocketType => socket_type;

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Sockets.Socket.Send" /> call will time out.</summary>
		/// <returns>The time-out value, in milliseconds. If you set the property with a value between 1 and 499, the value will be changed to 500. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than -1.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTimeout
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value", "The value specified for a set operation is less than -1");
				}
				if (value == -1)
				{
					value = 0;
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Sockets.Socket.Receive" /> call will time out.</summary>
		/// <returns>The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than -1.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveTimeout
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value", "The value specified for a set operation is less than -1");
				}
				if (value == -1)
				{
					value = 0;
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		/// <summary>Gets a value indicating whether IPv4 support is available and enabled on the current host.</summary>
		/// <returns>true if the current host supports the IPv4 protocol; otherwise, false.</returns>
		public static bool SupportsIPv4
		{
			get
			{
				CheckProtocolSupport();
				return ipv4Supported == 1;
			}
		}

		/// <summary>Gets a value that indicates whether the Framework supports IPv6 for certain obsolete <see cref="T:System.Net.Dns" /> members.</summary>
		/// <returns>true if the Framework supports IPv6 for certain obsolete <see cref="T:System.Net.Dns" /> methods; otherwise, false.</returns>
		[Obsolete("Use OSSupportsIPv6 instead")]
		public static bool SupportsIPv6
		{
			get
			{
				CheckProtocolSupport();
				return ipv6Supported == 1;
			}
		}

		public static bool OSSupportsIPv4
		{
			get
			{
				CheckProtocolSupport();
				return ipv4Supported == 1;
			}
		}

		/// <summary>Indicates whether the underlying operating system and network adaptors support Internet Protocol version 6 (IPv6).</summary>
		/// <returns>true if the operating system and network adaptors support the IPv6 protocol; otherwise, false.</returns>
		public static bool OSSupportsIPv6
		{
			get
			{
				CheckProtocolSupport();
				return ipv6Supported == 1;
			}
		}

		/// <summary>Gets the address family of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values.</returns>
		public AddressFamily AddressFamily => address_family;

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Net.Sockets.Socket" /> is in blocking mode.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> will block; otherwise, false. The default is true.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool Blocking
		{
			get
			{
				return blocking;
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				Blocking_internal(socket, value, out int error);
				if (error != 0)
				{
					throw new SocketException(error);
				}
				blocking = value;
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Net.Sockets.Socket" /> is connected to a remote host as of the last <see cref="Overload:System.Net.Sockets.Socket.Send" /> or <see cref="Overload:System.Net.Sockets.Socket.Receive" /> operation.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.Socket" /> was connected to a remote resource as of the most recent operation; otherwise, false.</returns>
		public bool Connected
		{
			get
			{
				return connected;
			}
			internal set
			{
				connected = value;
			}
		}

		/// <summary>Gets the protocol type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</returns>
		public ProtocolType ProtocolType => protocol_type;

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the stream <see cref="T:System.Net.Sockets.Socket" /> is using the Nagle algorithm.</summary>
		/// <returns>false if the <see cref="T:System.Net.Sockets.Socket" /> uses the Nagle algorithm; otherwise, true. The default is false.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool NoDelay
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				ThrowIfUpd();
				return (int)GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				ThrowIfUpd();
				SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a value that specifies the size of the receive buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the receive buffer. The default is 8192.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than 0.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveBufferSize
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", "The value specified for a set operation is less than zero");
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the size of the send buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the send buffer. The default is 8192.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than 0.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendBufferSize
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				return (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", "The value specified for a set operation is less than zero");
				}
				SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the Time To Live (TTL) value of Internet Protocol (IP) packets sent by the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The TTL value.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The TTL value can't be set to a negative number.</exception>
		/// <exception cref="T:System.NotSupportedException">This property can only be retrieved or set for a socket in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> address family.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. This error is also returned when an attempt was made to set TTL to a value higher than 255.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public short Ttl
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					return (short)(int)GetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress);
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					return (short)(int)GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.HopLimit);
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
			set
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (address_family == AddressFamily.InterNetwork)
				{
					SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, value);
					return;
				}
				if (address_family == AddressFamily.InterNetworkV6)
				{
					SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.HopLimit, value);
					return;
				}
				throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
			}
		}

		/// <summary>Gets the remote endpoint.</summary>
		/// <returns>The <see cref="T:System.Net.EndPoint" /> with which the <see cref="T:System.Net.Sockets.Socket" /> is communicating.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public EndPoint RemoteEndPoint
		{
			get
			{
				if (disposed && closed)
				{
					throw new ObjectDisposedException(GetType().ToString());
				}
				if (seed_endpoint == null)
				{
					return null;
				}
				int error;
				SocketAddress address = RemoteEndPoint_internal(socket, out error);
				if (error != 0)
				{
					throw new SocketException(error);
				}
				return seed_endpoint.Create(address);
			}
		}

		private Socket(AddressFamily family, SocketType type, ProtocolType proto, IntPtr sock)
		{
			address_family = family;
			socket_type = type;
			protocol_type = proto;
			socket = sock;
			connected = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.Socket" /> class using the specified value returned from <see cref="M:System.Net.Sockets.Socket.DuplicateAndClose(System.Int32)" />.</summary>
		/// <param name="socketInformation">The socket information returned by <see cref="M:System.Net.Sockets.Socket.DuplicateAndClose(System.Int32)" />.</param>
		[MonoTODO]
		public Socket(SocketInformation socketInformation)
		{
			throw new NotImplementedException("SocketInformation not figured out yet");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.Socket" /> class using the specified address family, socket type and protocol.</summary>
		/// <param name="addressFamily">One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values. </param>
		/// <param name="socketType">One of the <see cref="T:System.Net.Sockets.SocketType" /> values. </param>
		/// <param name="protocolType">One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">The combination of <paramref name="addressFamily" />, <paramref name="socketType" />, and <paramref name="protocolType" /> results in an invalid socket. </exception>
		public Socket(AddressFamily family, SocketType type, ProtocolType proto)
		{
			if (family == AddressFamily.Unspecified)
			{
				throw new ArgumentException("family");
			}
			address_family = family;
			socket_type = type;
			protocol_type = proto;
			socket = Socket_internal(family, type, proto, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
		}

		static Socket()
		{
			ipv4Supported = -1;
			ipv6Supported = -1;
			CheckProtocolSupport();
		}

		private static void AddSockets(ArrayList sockets, IList list, string name)
		{
			if (list != null)
			{
				foreach (Socket item in list)
				{
					if (item == null)
					{
						throw new ArgumentNullException("name", "Contains a null element");
					}
					sockets.Add(item);
				}
			}
			sockets.Add(null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Select_internal(ref Socket[] sockets, int microSeconds, out int error);

		/// <summary>Determines the status of one or more sockets.</summary>
		/// <param name="checkRead">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for readability. </param>
		/// <param name="checkWrite">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for writability. </param>
		/// <param name="checkError">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for errors. </param>
		/// <param name="microSeconds">The time-out value, in microseconds. A -1 value indicates an infinite time-out.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="checkRead" /> parameter is null or empty.-and- The <paramref name="checkWrite" /> parameter is null or empty -and- The <paramref name="checkError" /> parameter is null or empty. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
		{
			ArrayList arrayList = new ArrayList();
			AddSockets(arrayList, checkRead, "checkRead");
			AddSockets(arrayList, checkWrite, "checkWrite");
			AddSockets(arrayList, checkError, "checkError");
			if (arrayList.Count == 3)
			{
				throw new ArgumentNullException("checkRead, checkWrite, checkError", "All the lists are null or empty.");
			}
			Socket[] sockets = (Socket[])arrayList.ToArray(typeof(Socket));
			Select_internal(ref sockets, microSeconds, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			if (sockets == null)
			{
				checkRead?.Clear();
				checkWrite?.Clear();
				checkError?.Clear();
				return;
			}
			int num = 0;
			int num2 = sockets.Length;
			IList list = checkRead;
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				Socket socket = sockets[i];
				if (socket == null)
				{
					if (list != null)
					{
						int num4 = list.Count - num3;
						for (int j = 0; j < num4; j++)
						{
							list.RemoveAt(num3);
						}
					}
					list = ((num != 0) ? checkError : checkWrite);
					num3 = 0;
					num++;
					continue;
				}
				if (num == 1 && list == checkWrite && !socket.connected && (int)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error) == 0)
				{
					socket.connected = true;
				}
				if (list != null && num3 < list.Count)
				{
					Socket socket2;
					while ((socket2 = (Socket)list[num3]) != socket)
					{
						list.RemoveAt(num3);
					}
				}
				num3++;
			}
		}

		private void SocketDefaults()
		{
			try
			{
				if (address_family == AddressFamily.InterNetwork)
				{
					DontFragment = false;
				}
			}
			catch (SocketException)
			{
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Available_internal(IntPtr socket, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SocketAddress LocalEndPoint_internal(IntPtr socket, out int error);

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.Returns false if the I/O operation completed synchronously. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentException">An argument is not valid. This exception occurs if the buffer provided is not large enough. The buffer must be at least 2 * (sizeof(SOCKADDR_STORAGE + 16) bytes. This exception also occurs if multiple buffers are specified, the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument is out of range. The exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Count" /> is less than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">An invalid operation was requested. This exception occurs if the accepting <see cref="T:System.Net.Sockets.Socket" /> is not listening for connections or the accepted socket is bound. You must call the <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> method before calling the <see cref="M:System.Net.Sockets.Socket.AcceptAsync(System.Net.Sockets.SocketAsyncEventArgs)" /> method.This exception also occurs if the socket is already connected or a socket operation was already in progress using the specified <paramref name="e" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public bool AcceptAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!IsBound)
			{
				throw new InvalidOperationException("You must call the Bind method before performing this operation.");
			}
			if (!islistening)
			{
				throw new InvalidOperationException("You must call the Listen method before performing this operation.");
			}
			if (e.BufferList != null)
			{
				throw new ArgumentException("Multiple buffers cannot be used with this method.");
			}
			if (e.Count < 0)
			{
				throw new ArgumentOutOfRangeException("e.Count");
			}
			Socket acceptSocket = e.AcceptSocket;
			if (acceptSocket != null)
			{
				if (acceptSocket.IsBound || acceptSocket.Connected)
				{
					throw new InvalidOperationException("AcceptSocket: The socket must not be bound or connected.");
				}
			}
			else
			{
				e.AcceptSocket = new Socket(AddressFamily, SocketType, ProtocolType);
			}
			try
			{
				e.DoOperation(SocketAsyncOperation.Accept, this);
			}
			catch
			{
				((IDisposable)e).Dispose();
				throw;
				IL_00e6:;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Accept_internal(IntPtr sock, out int error, bool blocking);

		/// <summary>Creates a new <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.Accept" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public Socket Accept()
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			int error = 0;
			IntPtr sock = (IntPtr)(-1);
			blocking_thread = Thread.CurrentThread;
			try
			{
				sock = Accept_internal(this.socket, out error, blocking);
			}
			catch (ThreadAbortException)
			{
				if (disposed)
				{
					Thread.ResetAbort();
					error = 10004;
				}
			}
			finally
			{
				blocking_thread = null;
			}
			if (error != 0)
			{
				throw new SocketException(error);
			}
			Socket socket = new Socket(AddressFamily, SocketType, ProtocolType, sock);
			socket.seed_endpoint = seed_endpoint;
			socket.Blocking = Blocking;
			return socket;
		}

		internal void Accept(Socket acceptSocket)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			int error = 0;
			IntPtr intPtr = (IntPtr)(-1);
			blocking_thread = Thread.CurrentThread;
			try
			{
				intPtr = Accept_internal(socket, out error, blocking);
			}
			catch (ThreadAbortException)
			{
				if (disposed)
				{
					Thread.ResetAbort();
					error = 10004;
				}
			}
			finally
			{
				blocking_thread = null;
			}
			if (error != 0)
			{
				throw new SocketException(error);
			}
			acceptSocket.address_family = AddressFamily;
			acceptSocket.socket_type = SocketType;
			acceptSocket.protocol_type = ProtocolType;
			acceptSocket.socket = intPtr;
			acceptSocket.connected = true;
			acceptSocket.seed_endpoint = seed_endpoint;
			acceptSocket.Blocking = Blocking;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.-or- The accepted socket is bound. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IAsyncResult BeginAccept(AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!isbound || !islistening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Accept);
			Worker @object = new Worker(socketAsyncResult);
			SocketAsyncCall socketAsyncCall = @object.Accept;
			socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt and receives the first block of data sent by the client application.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
		/// <param name="receiveSize">The number of bytes to accept from the sender. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.-or- The accepted socket is bound. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (receiveSize < 0)
			{
				throw new ArgumentOutOfRangeException("receiveSize", "receiveSize is less than zero");
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.AcceptReceive);
			Worker @object = new Worker(socketAsyncResult);
			SocketAsyncCall socketAsyncCall = @object.AcceptReceive;
			socketAsyncResult.Buffer = new byte[receiveSize];
			socketAsyncResult.Offset = 0;
			socketAsyncResult.Size = receiveSize;
			socketAsyncResult.SockFlags = SocketFlags.None;
			socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt from a specified socket and receives the first block of data sent by the client application.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> object creation.</returns>
		/// <param name="acceptSocket">The accepted <see cref="T:System.Net.Sockets.Socket" /> object. This value may be null. </param>
		/// <param name="receiveSize">The maximum number of bytes to receive. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.-or- The accepted socket is bound. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (receiveSize < 0)
			{
				throw new ArgumentOutOfRangeException("receiveSize", "receiveSize is less than zero");
			}
			if (acceptSocket != null)
			{
				if (acceptSocket.disposed && acceptSocket.closed)
				{
					throw new ObjectDisposedException(acceptSocket.GetType().ToString());
				}
				if (acceptSocket.IsBound)
				{
					throw new InvalidOperationException();
				}
				if (acceptSocket.ProtocolType != ProtocolType.Tcp)
				{
					throw new SocketException(10022);
				}
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.AcceptReceive);
			Worker @object = new Worker(socketAsyncResult);
			SocketAsyncCall socketAsyncCall = @object.AcceptReceive;
			socketAsyncResult.Buffer = new byte[receiveSize];
			socketAsyncResult.Offset = 0;
			socketAsyncResult.Size = receiveSize;
			socketAsyncResult.SockFlags = SocketFlags.None;
			socketAsyncResult.AcceptSocket = acceptSocket;
			socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous request for a remote host connection.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote host. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(EndPoint end_point, AsyncCallback callback, object state)
		{
			return BeginConnect(end_point, callback, state, bypassSocketSecurity: false);
		}

		internal IAsyncResult BeginConnect(EndPoint end_point, AsyncCallback callback, object state, bool bypassSocketSecurity)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (end_point == null)
			{
				throw new ArgumentNullException("end_point");
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Connect);
			socketAsyncResult.EndPoint = end_point;
			if (end_point is IPEndPoint)
			{
				IPEndPoint iPEndPoint = (IPEndPoint)end_point;
				if (iPEndPoint.Address.Equals(IPAddress.Any) || iPEndPoint.Address.Equals(IPAddress.IPv6Any))
				{
					socketAsyncResult.Complete(new SocketException(10049), synch: true);
					return socketAsyncResult;
				}
			}
			int error = 0;
			if (!blocking)
			{
				SocketAddress sa = end_point.Serialize();
				Connect_internal(socket, sa, out error);
				switch (error)
				{
				case 0:
					connected = true;
					socketAsyncResult.Complete(synch: true);
					break;
				default:
					connected = false;
					socketAsyncResult.Complete(new SocketException(error), synch: true);
					break;
				case 10035:
				case 10036:
					break;
				}
			}
			if (blocking || error == 10036 || error == 10035)
			{
				connected = false;
				Worker @object = new Worker(socketAsyncResult, bypassSocketSecurity);
				SocketAsyncCall socketAsyncCall = @object.Connect;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			}
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> and a port number.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <param name="address">The <see cref="T:System.Net.IPAddress" /> of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Net.Sockets.Socket" /> is not in the socket family.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.ToString().Length == 0)
			{
				throw new ArgumentException("The length of the IP address is zero");
			}
			if (islistening)
			{
				throw new InvalidOperationException();
			}
			IPEndPoint end_point = new IPEndPoint(address, port);
			return BeginConnect(end_point, callback, state);
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> array and a port number.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connections.</returns>
		/// <param name="addresses">At least one <see cref="T:System.Net.IPAddress" />, designating the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addresses" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets that use <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (AddressFamily != AddressFamily.InterNetwork && AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");
			}
			if (islistening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Connect);
			socketAsyncResult.Addresses = addresses;
			socketAsyncResult.Port = port;
			connected = false;
			Worker @object = new Worker(socketAsyncResult);
			SocketAsyncCall socketAsyncCall = @object.Connect;
			socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by a host name and a port number.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (address_family != AddressFamily.InterNetwork && address_family != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException("This method is valid only for sockets in the InterNetwork and InterNetworkV6 families");
			}
			if (islistening)
			{
				throw new InvalidOperationException();
			}
			IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
			return BeginConnect(hostAddresses, port, callback, state);
		}

		/// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous operation.</returns>
		/// <param name="reuseSocket">true if this socket can be reused after the connection is closed; otherwise, false. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Disconnect);
			socketAsyncResult.ReuseSocket = reuseSocket;
			Worker @object = new Worker(socketAsyncResult);
			SocketAsyncCall socketAsyncCall = @object.Disconnect;
			socketAsyncCall.BeginInvoke(null, socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socket_flags, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			lock (readQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Receive);
				socketAsyncResult.Buffer = buffer;
				socketAsyncResult.Offset = offset;
				socketAsyncResult.Size = size;
				socketAsyncResult.SockFlags = socket_flags;
				readQ.Enqueue(socketAsyncResult);
				if (readQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.Receive;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The location in <paramref name="buffer" /> to store the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags flags, out SocketError error, AsyncCallback callback, object state)
		{
			error = SocketError.Success;
			return BeginReceive(buffer, offset, size, flags, callback, state);
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			lock (readQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.ReceiveGeneric);
				socketAsyncResult.Buffers = buffers;
				socketAsyncResult.SockFlags = socketFlags;
				readQ.Enqueue(socketAsyncResult);
				if (readQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.ReceiveGeneric;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			errorCode = SocketError.Success;
			return BeginReceive(buffers, socketFlags, callback, state);
		}

		/// <summary>Begins to asynchronously receive data from a specified network device.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socket_flags, ref EndPoint remote_end, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be >= 0");
			}
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "size must be >= 0");
			}
			if (offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset, size", "offset + size exceeds the buffer length");
			}
			lock (readQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.ReceiveFrom);
				socketAsyncResult.Buffer = buffer;
				socketAsyncResult.Offset = offset;
				socketAsyncResult.Size = size;
				socketAsyncResult.SockFlags = socket_flags;
				socketAsyncResult.EndPoint = remote_end;
				readQ.Enqueue(socketAsyncResult);
				if (readQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.ReceiveFrom;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		/// <summary>Begins to asynchronously receive the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information..</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data.</param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		[MonoTODO]
		public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			throw new NotImplementedException();
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is less than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socket_flags, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be >= 0");
			}
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "size must be >= 0");
			}
			if (offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset, size", "offset + size exceeds the buffer length");
			}
			if (!connected)
			{
				throw new SocketException(10057);
			}
			lock (writeQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.Send);
				socketAsyncResult.Buffer = buffer;
				socketAsyncResult.Offset = offset;
				socketAsyncResult.Size = size;
				socketAsyncResult.SockFlags = socket_flags;
				writeQ.Enqueue(socketAsyncResult);
				if (writeQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.Send;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is less than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (!connected)
			{
				errorCode = SocketError.NotConnected;
				throw new SocketException((int)errorCode);
			}
			errorCode = SocketError.Success;
			return BeginSend(buffer, offset, size, socketFlags, callback, state);
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (!connected)
			{
				throw new SocketException(10057);
			}
			lock (writeQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.SendGeneric);
				socketAsyncResult.Buffers = buffers;
				socketAsyncResult.SockFlags = socketFlags;
				writeQ.Enqueue(socketAsyncResult);
				if (writeQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.SendGeneric;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			if (!connected)
			{
				errorCode = SocketError.NotConnected;
				throw new SocketException((int)errorCode);
			}
			errorCode = SocketError.Success;
			return BeginSend(buffers, socketFlags, callback, state);
		}

		/// <summary>Sends the file <paramref name="fileName" /> to a connected <see cref="T:System.Net.Sockets.Socket" /> object using the <see cref="F:System.Net.Sockets.TransmitFileOptions.UseDefaultWorkerThread" /> flag.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous send.</returns>
		/// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be null. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The socket is not connected to a remote host. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!connected)
			{
				throw new NotSupportedException();
			}
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}
			return BeginSendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread, callback, state);
		}

		/// <summary>Sends a file and buffers of data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous operation.</returns>
		/// <param name="fileName">A string that contains the path and name of the file to be sent. This parameter can be null. </param>
		/// <param name="preBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent before the file is sent. This parameter can be null. </param>
		/// <param name="postBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent after the file is sent. This parameter can be null. </param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values. </param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate to be invoked when this operation completes. This parameter can be null. </param>
		/// <param name="state">A user-defined object that contains state information for this request. This parameter can be null. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is not Windows NT or later.- or - The socket is not connected to a remote host. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found. </exception>
		public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!connected)
			{
				throw new NotSupportedException();
			}
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}
			SendFileHandler sendFileHandler = SendFile;
			return new SendFileAsyncResult(sendFileHandler, sendFileHandler.BeginInvoke(fileName, preBuffer, postBuffer, flags, callback, state));
		}

		/// <summary>Sends data asynchronously to a specific remote host.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send. </param>
		/// <param name="offset">The zero-based position in <paramref name="buffer" /> at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device. </param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socket_flags, EndPoint remote_end, AsyncCallback callback, object state)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be >= 0");
			}
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "size must be >= 0");
			}
			if (offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset, size", "offset + size exceeds the buffer length");
			}
			lock (writeQ)
			{
				SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, state, callback, SocketOperation.SendTo);
				socketAsyncResult.Buffer = buffer;
				socketAsyncResult.Offset = offset;
				socketAsyncResult.Size = size;
				socketAsyncResult.SockFlags = socket_flags;
				socketAsyncResult.EndPoint = remote_end;
				writeQ.Enqueue(socketAsyncResult);
				if (writeQ.Count != 1)
				{
					return socketAsyncResult;
				}
				Worker @object = new Worker(socketAsyncResult);
				SocketAsyncCall socketAsyncCall = @object.SendTo;
				socketAsyncCall.BeginInvoke(null, socketAsyncResult);
				return socketAsyncResult;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Bind_internal(IntPtr sock, SocketAddress sa, out int error);

		/// <summary>Associates a <see cref="T:System.Net.Sockets.Socket" /> with a local endpoint.</summary>
		/// <param name="localEP">The local <see cref="T:System.Net.EndPoint" /> to associate with the <see cref="T:System.Net.Sockets.Socket" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Bind(EndPoint local_end)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (local_end == null)
			{
				throw new ArgumentNullException("local_end");
			}
			if (Environment.SocketSecurityEnabled && current_bind_count >= max_bind_count)
			{
				throw new SecurityException("Too many sockets are bound, maximum count in the webplayer is " + max_bind_count);
			}
			Bind_internal(socket, local_end.Serialize(), out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			if (error == 0)
			{
				isbound = true;
			}
			if (Environment.SocketSecurityEnabled)
			{
				current_bind_count++;
			}
			seed_endpoint = local_end;
		}

		/// <summary>Begins an asynchronous request for a remote host connection.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation. </returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentException">An argument is not valid. This exception occurs if multiple buffers are specified, the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is not null. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is listening or a socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method. This exception also occurs if the local endpoint and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> are not the same address family.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		public bool ConnectAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (islistening)
			{
				throw new InvalidOperationException("You may not perform this operation after calling the Listen method.");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP", "Value cannot be null.");
			}
			if (e.BufferList != null)
			{
				throw new ArgumentException("Multiple buffers cannot be used with this method.");
			}
			e.DoOperation(SocketAsyncOperation.Connect, this);
			return true;
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by an IP address and a port number.</summary>
		/// <param name="address">The IP address of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(IPAddress address, int port)
		{
			Connect(new IPEndPoint(address, port));
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by an array of IP addresses and a port number.</summary>
		/// <param name="addresses">The IP addresses of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addresses" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(IPAddress[] addresses, int port)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (AddressFamily != AddressFamily.InterNetwork && AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");
			}
			if (islistening)
			{
				throw new InvalidOperationException();
			}
			int error = 0;
			foreach (IPAddress address in addresses)
			{
				IPEndPoint iPEndPoint = new IPEndPoint(address, port);
				SocketAddress sa = iPEndPoint.Serialize();
				Connect_internal(socket, sa, out error);
				switch (error)
				{
				case 0:
					connected = true;
					seed_endpoint = iPEndPoint;
					return;
				case 10035:
				case 10036:
					if (!blocking)
					{
						Poll(-1, SelectMode.SelectWrite);
						error = (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
						if (error == 0)
						{
							connected = true;
							seed_endpoint = iPEndPoint;
							return;
						}
					}
					break;
				}
			}
			if (error != 0)
			{
				throw new SocketException(error);
			}
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by a host name and a port number.</summary>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(string host, int port)
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
			Connect(hostAddresses, port);
		}

		/// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. </exception>
		public bool DisconnectAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			e.DoOperation(SocketAsyncOperation.Disconnect, this);
			return true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Disconnect_internal(IntPtr sock, bool reuse, out int error);

		/// <summary>Closes the socket connection and allows reuse of the socket.</summary>
		/// <param name="reuseSocket">true if this socket can be reused after the current connection is closed; otherwise, false. </param>
		/// <exception cref="T:System.PlatformNotSupportedException">This method requires Windows 2000 or earlier, or the exception will be thrown.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Disconnect(bool reuseSocket)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			int error = 0;
			Disconnect_internal(socket, reuseSocket, out error);
			switch (error)
			{
			case 50:
				throw new PlatformNotSupportedException();
			default:
				throw new SocketException(error);
			case 0:
				connected = false;
				if (!reuseSocket)
				{
				}
				break;
			}
		}

		/// <summary>Duplicates the socket reference for the target process, and closes the socket for this process.</summary>
		/// <returns>The socket reference to be passed to the target process.</returns>
		/// <param name="targetProcessId">The ID of the target process where a duplicate of the socket reference is created.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="targetProcessID" /> is not a valid process id.-or- Duplication of the socket reference failed. </exception>
		[MonoTODO("Not implemented")]
		public SocketInformation DuplicateAndClose(int targetProcessId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> to handle remote host communication.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation as well as any user defined data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called. </exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public Socket EndAccept(IAsyncResult result)
		{
			byte[] buffer;
			int bytesTransferred;
			return EndAccept(out buffer, out bytesTransferred, result);
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data transferred.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" /> See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
		{
			int bytesTransferred;
			return EndAccept(out buffer, out bytesTransferred, asyncResult);
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data and the number of bytes transferred.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred. </param>
		/// <param name="bytesTransferred">The number of bytes transferred. </param>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data. </param>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SocketAsyncResult socketAsyncResult = asyncResult as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndAccept");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			buffer = socketAsyncResult.Buffer;
			bytesTransferred = socketAsyncResult.Total;
			return socketAsyncResult.Socket;
		}

		/// <summary>Ends a pending asynchronous connection request.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginConnect(System.Net.EndPoint,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndConnect(System.IAsyncResult)" /> was previously called for the asynchronous connection. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void EndConnect(IAsyncResult result)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			SocketAsyncResult socketAsyncResult = result as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "result");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndConnect");
			}
			if (!result.IsCompleted)
			{
				result.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
		}

		/// <summary>Ends a pending asynchronous disconnect request.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information and any user-defined data for this asynchronous operation. </param>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginDisconnect(System.Boolean,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndDisconnect(System.IAsyncResult)" /> was previously called for the asynchronous connection. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.Net.WebException">The disconnect request has timed out. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void EndDisconnect(IAsyncResult asyncResult)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SocketAsyncResult socketAsyncResult = asyncResult as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndDisconnect");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
		}

		/// <summary>Ends a pending asynchronous read.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceive(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> was previously called for the asynchronous read. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int EndReceive(IAsyncResult result)
		{
			SocketError errorCode;
			return EndReceive(result, out errorCode);
		}

		/// <summary>Ends a pending asynchronous read.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceive(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> was previously called for the asynchronous read. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SocketAsyncResult socketAsyncResult = asyncResult as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndReceive");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			errorCode = socketAsyncResult.ErrorCode;
			socketAsyncResult.CheckIfThrowDelayedException();
			return socketAsyncResult.Total;
		}

		/// <summary>Ends a pending asynchronous read from a specific endpoint.</summary>
		/// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
		/// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceiveFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint@,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceiveFrom(System.IAsyncResult,System.Net.EndPoint@)" /> was previously called for the asynchronous read. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int EndReceiveFrom(IAsyncResult result, ref EndPoint end_point)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			SocketAsyncResult socketAsyncResult = result as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "result");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndReceiveFrom");
			}
			if (!result.IsCompleted)
			{
				result.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			end_point = socketAsyncResult.EndPoint;
			return socketAsyncResult.Total;
		}

		/// <summary>Ends a pending asynchronous read from a specific endpoint. This method also reveals more information about the packet than <see cref="M:System.Net.Sockets.Socket.EndReceiveFrom(System.IAsyncResult,System.Net.EndPoint@)" />.</summary>
		/// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values for the received packet.</param>
		/// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />.</param>
		/// <param name="ipPacketInformation">The <see cref="T:System.Net.IPAddress" /> and interface of the received packet.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null-or- <paramref name="endPoint" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint@,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> was previously called for the asynchronous read. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[MonoTODO]
		public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			SocketAsyncResult socketAsyncResult = asyncResult as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndReceiveMessageFrom");
			}
			throw new NotImplementedException();
		}

		/// <summary>Ends a pending asynchronous send.</summary>
		/// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSend(System.IAsyncResult)" /> was previously called for the asynchronous send. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int EndSend(IAsyncResult result)
		{
			SocketError errorCode;
			return EndSend(result, out errorCode);
		}

		/// <summary>Ends a pending asynchronous send.</summary>
		/// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSend(System.IAsyncResult)" /> was previously called for the asynchronous send. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SocketAsyncResult socketAsyncResult = asyncResult as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "result");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndSend");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			errorCode = socketAsyncResult.ErrorCode;
			socketAsyncResult.CheckIfThrowDelayedException();
			return socketAsyncResult.Total;
		}

		/// <summary>Ends a pending asynchronous send of a file.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation. </param>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSendFile(System.String,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSendFile(System.IAsyncResult)" /> was previously called for the asynchronous <see cref="M:System.Net.Sockets.Socket.BeginSendFile(System.String,System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void EndSendFile(IAsyncResult asyncResult)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SendFileAsyncResult sendFileAsyncResult = asyncResult as SendFileAsyncResult;
			if (sendFileAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			sendFileAsyncResult.Delegate.EndInvoke(sendFileAsyncResult.Original);
		}

		private Exception InvalidAsyncOp(string method)
		{
			return new InvalidOperationException(method + " can only be called once per asynchronous operation");
		}

		/// <summary>Ends a pending asynchronous send to a specific location.</summary>
		/// <returns>If successful, the number of bytes sent; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSendTo(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint,System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSendTo(System.IAsyncResult)" /> was previously called for the asynchronous send. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int EndSendTo(IAsyncResult result)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			SocketAsyncResult socketAsyncResult = result as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "result");
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw InvalidAsyncOp("EndSendTo");
			}
			if (!result.IsCompleted)
			{
				result.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			return socketAsyncResult.Total;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSocketOption_arr_internal(IntPtr socket, SocketOptionLevel level, SocketOptionName name, ref byte[] byte_val, out int error);

		/// <summary>Returns the specified <see cref="T:System.Net.Sockets.Socket" /> option setting, represented as a byte array.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionValue">An array of type <see cref="T:System.Byte" /> that is to receive the option setting. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. - or -In .NET Compact Framework applications, the Windows CE default buffer space is set to 32768 bytes. You can change the per socket buffer space by calling <see cref="Overload:System.Net.Sockets.Socket.SetSocketOption" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (optionValue == null)
			{
				throw new SocketException(10014, "Error trying to dereference an invalid pointer");
			}
			GetSocketOption_arr_internal(socket, optionLevel, optionName, ref optionValue, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
		}

		/// <summary>Returns the value of the specified <see cref="T:System.Net.Sockets.Socket" /> option in an array.</summary>
		/// <returns>An array of type <see cref="T:System.Byte" /> that contains the value of the socket option.</returns>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionLength">The length, in bytes, of the expected return value. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. - or -In .NET Compact Framework applications, the Windows CE default buffer space is set to 32768 bytes. You can change the per socket buffer space by calling <see cref="Overload:System.Net.Sockets.Socket.SetSocketOption" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int length)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			byte[] byte_val = new byte[length];
			GetSocketOption_arr_internal(socket, optionLevel, optionName, ref byte_val, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			return byte_val;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int WSAIoctl(IntPtr sock, int ioctl_code, byte[] input, byte[] output, out int error);

		/// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using numerical control codes.</summary>
		/// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
		/// <param name="ioControlCode">An <see cref="T:System.Int32" /> value that specifies the control code of the operation to perform. </param>
		/// <param name="optionInValue">A <see cref="T:System.Byte" /> array that contains the input data required by the operation. </param>
		/// <param name="optionOutValue">A <see cref="T:System.Byte" /> array that contains the output data returned by the operation. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to change the blocking mode without using the <see cref="P:System.Net.Sockets.Socket.Blocking" /> property. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int IOControl(int ioctl_code, byte[] in_value, byte[] out_value)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			int error;
			int num = WSAIoctl(socket, ioctl_code, in_value, out_value, out error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			if (num == -1)
			{
				throw new InvalidOperationException("Must use Blocking property instead.");
			}
			return num;
		}

		/// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using the <see cref="T:System.Net.Sockets.IOControlCode" /> enumeration to specify control codes.</summary>
		/// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
		/// <param name="ioControlCode">A <see cref="T:System.Net.Sockets.IOControlCode" /> value that specifies the control code of the operation to perform. </param>
		/// <param name="optionInValue">An array of type <see cref="T:System.Byte" /> that contains the input data required by the operation. </param>
		/// <param name="optionOutValue">An array of type <see cref="T:System.Byte" /> that contains the output data returned by the operation. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to change the blocking mode without using the <see cref="P:System.Net.Sockets.Socket.Blocking" /> property. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO]
		public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
		{
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Listen_internal(IntPtr sock, int backlog, out int error);

		/// <summary>Places a <see cref="T:System.Net.Sockets.Socket" /> in a listening state.</summary>
		/// <param name="backlog">The maximum length of the pending connections queue. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Listen(int backlog)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!isbound)
			{
				throw new SocketException(10022);
			}
			if (Environment.SocketSecurityEnabled)
			{
				SecurityException ex = new SecurityException("Listening on TCP sockets is not allowed in the webplayer");
				Console.WriteLine("Throwing the following securityexception: " + ex);
				throw ex;
			}
			Listen_internal(socket, backlog, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			islistening = true;
		}

		/// <summary>Determines the status of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The status of the <see cref="T:System.Net.Sockets.Socket" /> based on the polling mode value passed in the <paramref name="mode" /> parameter.Mode Return Value <see cref="F:System.Net.Sockets.SelectMode.SelectRead" />true if <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> has been called and a connection is pending; -or- true if data is available for reading; -or- true if the connection has been closed, reset, or terminated; otherwise, returns false. <see cref="F:System.Net.Sockets.SelectMode.SelectWrite" />true, if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" />, and the connection has succeeded; -or- true if data can be sent; otherwise, returns false. <see cref="F:System.Net.Sockets.SelectMode.SelectError" />true if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" /> that does not block, and the connection has failed; -or- true if <see cref="F:System.Net.Sockets.SocketOptionName.OutOfBandInline" /> is not set and out-of-band data is available; otherwise, returns false. </returns>
		/// <param name="microSeconds">The time to wait for a response, in microseconds. </param>
		/// <param name="mode">One of the <see cref="T:System.Net.Sockets.SelectMode" /> values. </param>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="mode" /> parameter is not one of the <see cref="T:System.Net.Sockets.SelectMode" /> values. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks below. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool Poll(int time_us, SelectMode mode)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (mode != 0 && mode != SelectMode.SelectWrite && mode != SelectMode.SelectError)
			{
				throw new NotSupportedException("'mode' parameter is not valid.");
			}
			int error;
			bool flag = Poll_internal(socket, mode, time_us, out error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			if (mode == SelectMode.SelectWrite && flag && !connected && (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error) == 0)
			{
				connected = true;
			}
			return flag;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Receive(byte[] buffer)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			SocketError error;
			int result = Receive_nochecks(buffer, 0, buffer.Length, SocketFlags.None, out error);
			if (error != 0)
			{
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Receive(byte[] buffer, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			SocketError error;
			int result = Receive_nochecks(buffer, 0, buffer.Length, flags, out error);
			if (error != 0)
			{
				if (error == SocketError.WouldBlock && blocking)
				{
					throw new SocketException((int)error, "Operation timed out.");
				}
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Receives the specified number of bytes of data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> exceeds the size of <paramref name="buffer" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Receive(byte[] buffer, int size, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (size < 0 || size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			SocketError error;
			int result = Receive_nochecks(buffer, 0, size, flags, out error);
			if (error != 0)
			{
				if (error == SocketError.WouldBlock && blocking)
				{
					throw new SocketException((int)error, "Operation timed out.");
				}
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Receives the specified number of bytes from a bound <see cref="T:System.Net.Sockets.Socket" /> into the specified offset position of the receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
		/// <param name="offset">The location in <paramref name="buffer" /> to store the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Receive(byte[] buffer, int offset, int size, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			SocketError error;
			int result = Receive_nochecks(buffer, offset, size, flags, out error);
			if (error != 0)
			{
				if (error == SocketError.WouldBlock && blocking)
				{
					throw new SocketException((int)error, "Operation timed out.");
				}
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property is not set.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		public int Receive(byte[] buffer, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return Receive_nochecks(buffer, offset, size, flags, out error);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Receive_internal(IntPtr sock, WSABUF[] bufarray, SocketFlags flags, out int error);

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int Receive(IList<ArraySegment<byte>> buffers)
		{
			SocketError errorCode;
			int result = Receive(buffers, SocketFlags.None, out errorCode);
			if (errorCode != 0)
			{
				throw new SocketException((int)errorCode);
			}
			return result;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null.-or-<paramref name="buffers" />.Count is zero.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError errorCode;
			int result = Receive(buffers, socketFlags, out errorCode);
			if (errorCode != 0)
			{
				throw new SocketException((int)errorCode);
			}
			return result;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null.-or-<paramref name="buffers" />.Count is zero.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffers == null || buffers.Count == 0)
			{
				throw new ArgumentNullException("buffers");
			}
			int count = buffers.Count;
			WSABUF[] array = new WSABUF[count];
			GCHandle[] array2 = new GCHandle[count];
			for (int i = 0; i < count; i++)
			{
				ArraySegment<byte> arraySegment = buffers[i];
				array2[i] = GCHandle.Alloc(arraySegment.Array, GCHandleType.Pinned);
				array[i].len = arraySegment.Count;
				array[i].buf = Marshal.UnsafeAddrOfPinnedArrayElement(arraySegment.Array, arraySegment.Offset);
			}
			int result;
			int error;
			try
			{
				result = Receive_internal(socket, array, socketFlags, out error);
			}
			finally
			{
				for (int j = 0; j < count; j++)
				{
					if (array2[j].IsAllocated)
					{
						array2[j].Free();
					}
				}
			}
			errorCode = (SocketError)error;
			return result;
		}

		/// <summary>Begins to asynchronously receive data from a specified network device.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. </exception>
		public bool ReceiveFromAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (e.BufferList != null)
			{
				throw new NotSupportedException("Mono doesn't support using BufferList at this point.");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP", "Value cannot be null.");
			}
			e.DoOperation(SocketAsyncOperation.ReceiveFrom, this);
			return true;
		}

		/// <summary>Receives a datagram into the data buffer and stores the endpoint.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			return ReceiveFrom_nochecks(buffer, 0, buffer.Length, SocketFlags.None, ref remoteEP);
		}

		/// <summary>Receives a datagram into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveFrom(byte[] buffer, SocketFlags flags, ref EndPoint remoteEP)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			return ReceiveFrom_nochecks(buffer, 0, buffer.Length, flags, ref remoteEP);
		}

		/// <summary>Receives the specified number of bytes into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveFrom(byte[] buffer, int size, SocketFlags flags, ref EndPoint remoteEP)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (size < 0 || size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return ReceiveFrom_nochecks(buffer, 0, size, flags, ref remoteEP);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int RecvFrom_internal(IntPtr sock, byte[] buffer, int offset, int count, SocketFlags flags, ref SocketAddress sockaddr, out int error);

		/// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data. </param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data. </param>
		/// <param name="size">The number of bytes to receive. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of the <paramref name="buffer" /> minus the value of the offset parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.-or- An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags flags, ref EndPoint remoteEP)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return ReceiveFrom_nochecks(buffer, offset, size, flags, ref remoteEP);
		}

		internal int ReceiveFrom_nochecks(byte[] buf, int offset, int size, SocketFlags flags, ref EndPoint remote_end)
		{
			int error;
			return ReceiveFrom_nochecks_exc(buf, offset, size, flags, ref remote_end, throwOnError: true, out error);
		}

		internal int ReceiveFrom_nochecks_exc(byte[] buf, int offset, int size, SocketFlags flags, ref EndPoint remote_end, bool throwOnError, out int error)
		{
			SocketAddress sockaddr = remote_end.Serialize();
			int result = RecvFrom_internal(socket, buf, offset, size, flags, ref sockaddr, out error);
			SocketError socketError = (SocketError)error;
			if (socketError != 0)
			{
				if (socketError != SocketError.WouldBlock && socketError != SocketError.InProgress)
				{
					connected = false;
				}
				else if (socketError == SocketError.WouldBlock && blocking)
				{
					if (throwOnError)
					{
						throw new SocketException(10060, "Operation timed out");
					}
					error = 10060;
					return 0;
				}
				if (throwOnError)
				{
					throw new SocketException(error);
				}
				return 0;
			}
			if (Environment.SocketSecurityEnabled && !CheckEndPoint(sockaddr))
			{
				buf.Initialize();
				throw new SecurityException("Unable to connect, as no valid crossdomain policy was found");
			}
			connected = true;
			isbound = true;
			if (sockaddr != null)
			{
				remote_end = remote_end.Create(sockaddr);
			}
			seed_endpoint = remote_end;
			return result;
		}

		/// <summary>Begins to asynchronously receive the specified number of bytes of data into the specified location in the data buffer, using the specified <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. </exception>
		[MonoTODO("Not implemented")]
		public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			throw new NotImplementedException();
		}

		/// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <returns>The number of bytes received.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <param name="ipPacketInformation">An <see cref="T:System.Net.Sockets.IPPacketInformation" /> holding address and interface information.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.- or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of the <paramref name="buffer" /> minus the value of the offset parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.-or- The .NET Framework is running on an AMD 64-bit processor.-or- An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		[MonoTODO("Not implemented")]
		public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			throw new NotImplementedException();
		}

		/// <summary>Sends a collection of files or in memory data buffers asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in the <see cref="P:System.Net.Sockets.SendPacketsElement.FilePath" /> property was not found. </exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method. This exception also occurs if the <see cref="T:System.Net.Sockets.Socket" /> is not connected to a remote host. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">A connectionless <see cref="T:System.Net.Sockets.Socket" /> is being used and the file being sent exceeds the maximum packet size of the underlying transport.</exception>
		[MonoTODO("Not implemented")]
		public bool SendPacketsAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			throw new NotImplementedException();
		}

		/// <summary>Sends data to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Send(byte[] buf)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buf == null)
			{
				throw new ArgumentNullException("buf");
			}
			SocketError error;
			int result = Send_nochecks(buf, 0, buf.Length, SocketFlags.None, out error);
			if (error != 0)
			{
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Sends data to a connected <see cref="T:System.Net.Sockets.Socket" /> using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Send(byte[] buf, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buf == null)
			{
				throw new ArgumentNullException("buf");
			}
			SocketError error;
			int result = Send_nochecks(buf, 0, buf.Length, flags, out error);
			if (error != 0)
			{
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> is less than 0 or exceeds the size of the buffer. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- An operating system error occurs while accessing the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Send(byte[] buf, int size, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buf == null)
			{
				throw new ArgumentNullException("buf");
			}
			if (size < 0 || size > buf.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			SocketError error;
			int result = Send_nochecks(buf, 0, size, flags, out error);
			if (error != 0)
			{
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="offset">The position in the data buffer at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int Send(byte[] buf, int offset, int size, SocketFlags flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buf == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buf.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buf.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			SocketError error;
			int result = Send_nochecks(buf, offset, size, flags, out error);
			if (error != 0)
			{
				throw new SocketException((int)error);
			}
			return result;
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" /></summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="offset">The position in the data buffer at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int Send(byte[] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buf == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buf.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buf.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return Send_nochecks(buf, offset, size, flags, out error);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Send_internal(IntPtr sock, WSABUF[] bufarray, SocketFlags flags, out int error);

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int Send(IList<ArraySegment<byte>> buffers)
		{
			SocketError errorCode;
			int result = Send(buffers, SocketFlags.None, out errorCode);
			if (errorCode != 0)
			{
				throw new SocketException((int)errorCode);
			}
			return result;
		}

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError errorCode;
			int result = Send(buffers, socketFlags, out errorCode);
			if (errorCode != 0)
			{
				throw new SocketException((int)errorCode);
			}
			return result;
		}

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		[CLSCompliant(false)]
		public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException("Buffer is empty", "buffers");
			}
			int count = buffers.Count;
			WSABUF[] array = new WSABUF[count];
			GCHandle[] array2 = new GCHandle[count];
			for (int i = 0; i < count; i++)
			{
				ArraySegment<byte> arraySegment = buffers[i];
				array2[i] = GCHandle.Alloc(arraySegment.Array, GCHandleType.Pinned);
				array[i].len = arraySegment.Count;
				array[i].buf = Marshal.UnsafeAddrOfPinnedArrayElement(arraySegment.Array, arraySegment.Offset);
			}
			int result;
			int error;
			try
			{
				result = Send_internal(socket, array, socketFlags, out error);
			}
			finally
			{
				for (int j = 0; j < count; j++)
				{
					if (array2[j].IsAllocated)
					{
						array2[j].Free();
					}
				}
			}
			errorCode = (SocketError)error;
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SendFile(IntPtr sock, string filename, byte[] pre_buffer, byte[] post_buffer, TransmitFileOptions flags);

		/// <summary>Sends the file <paramref name="fileName" /> to a connected <see cref="T:System.Net.Sockets.Socket" /> object with the <see cref="F:System.Net.Sockets.TransmitFileOptions.UseDefaultWorkerThread" /> transmit flag.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the path and name of the file to be sent. This parameter can be null. </param>
		/// <exception cref="T:System.NotSupportedException">The socket is not connected to a remote host. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> object is not in blocking mode and cannot accept this synchronous call. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SendFile(string fileName)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!connected)
			{
				throw new NotSupportedException();
			}
			if (!blocking)
			{
				throw new InvalidOperationException();
			}
			SendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread);
		}

		/// <summary>Sends the file <paramref name="fileName" /> and buffers of data to a connected <see cref="T:System.Net.Sockets.Socket" /> object using the specified <see cref="T:System.Net.Sockets.TransmitFileOptions" /> value.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the path and name of the file to be sent. This parameter can be null. </param>
		/// <param name="preBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent before the file is sent. This parameter can be null. </param>
		/// <param name="postBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent after the file is sent. This parameter can be null. </param>
		/// <param name="flags">One or more of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values. </param>
		/// <exception cref="T:System.NotSupportedException">The operating system is not Windows NT or later.- or - The socket is not connected to a remote host. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> object is not in blocking mode and cannot accept this synchronous call. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!connected)
			{
				throw new NotSupportedException();
			}
			if (!blocking)
			{
				throw new InvalidOperationException();
			}
			if (!SendFile(socket, fileName, preBuffer, postBuffer, flags))
			{
				SocketException ex = new SocketException();
				if (ex.ErrorCode == 2 || ex.ErrorCode == 3)
				{
					throw new FileNotFoundException();
				}
				throw ex;
			}
		}

		/// <summary>Sends data asynchronously to a specific remote host.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">The protocol specified is connection-oriented, but the <see cref="T:System.Net.Sockets.Socket" /> is not yet connected.</exception>
		public bool SendToAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP", "Value cannot be null.");
			}
			e.DoOperation(SocketAsyncOperation.SendTo, this);
			return true;
		}

		/// <summary>Sends data to the specified endpoint.</summary>
		/// <returns>The number of bytes sent.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination for the data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTo(byte[] buffer, EndPoint remote_end)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remote_end == null)
			{
				throw new ArgumentNullException("remote_end");
			}
			return SendTo_nochecks(buffer, 0, buffer.Length, SocketFlags.None, remote_end);
		}

		/// <summary>Sends data to a specific endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTo(byte[] buffer, SocketFlags flags, EndPoint remote_end)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remote_end == null)
			{
				throw new ArgumentNullException("remote_end");
			}
			return SendTo_nochecks(buffer, 0, buffer.Length, flags, remote_end);
		}

		/// <summary>Sends the specified number of bytes of data to the specified endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="size" /> exceeds the size of <paramref name="buffer" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTo(byte[] buffer, int size, SocketFlags flags, EndPoint remote_end)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remote_end == null)
			{
				throw new ArgumentNullException("remote_end");
			}
			if (size < 0 || size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return SendTo_nochecks(buffer, 0, size, flags, remote_end);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SendTo_internal_real(IntPtr sock, byte[] buffer, int offset, int count, SocketFlags flags, SocketAddress sa, out int error);

		private static int SendTo_internal(IntPtr sock, byte[] buffer, int offset, int count, SocketFlags flags, SocketAddress sa, out int error)
		{
			if (Environment.SocketSecurityEnabled && !CheckEndPoint(sa))
			{
				SecurityException ex = new SecurityException("SendTo request refused by Unity webplayer security model");
				Console.WriteLine("Throwing the following security exception: " + ex);
				throw ex;
			}
			return SendTo_internal_real(sock, buffer, offset, count, flags, sa, out error);
		}

		/// <summary>Sends the specified number of bytes of data to the specified endpoint, starting at the specified location in the buffer, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <returns>The number of bytes sent.</returns>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent. </param>
		/// <param name="offset">The position in the data buffer at which to begin sending data. </param>
		/// <param name="size">The number of bytes to send. </param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values. </param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.-or- <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.-or- <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.-or- <paramref name="size" /> is less than 0.-or- <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.-or- An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTo(byte[] buffer, int offset, int size, SocketFlags flags, EndPoint remote_end)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remote_end == null)
			{
				throw new ArgumentNullException("remote_end");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || offset + size > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return SendTo_nochecks(buffer, offset, size, flags, remote_end);
		}

		internal int SendTo_nochecks(byte[] buffer, int offset, int size, SocketFlags flags, EndPoint remote_end)
		{
			SocketAddress sa = remote_end.Serialize();
			int error;
			int result = SendTo_internal(socket, buffer, offset, size, flags, sa, out error);
			SocketError socketError = (SocketError)error;
			if (socketError != 0)
			{
				if (socketError != SocketError.WouldBlock && socketError != SocketError.InProgress)
				{
					connected = false;
				}
				throw new SocketException(error);
			}
			connected = true;
			isbound = true;
			seed_endpoint = remote_end;
			return result;
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified value, represented as a byte array.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionValue">An array of type <see cref="T:System.Byte" /> that represents the value of the option. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (optionValue == null)
			{
				throw new SocketException(10014, "Error trying to dereference an invalid pointer");
			}
			SetSocketOption_internal(socket, optionLevel, optionName, null, optionValue, 0, out int error);
			switch (error)
			{
			case 0:
				break;
			case 10022:
				throw new ArgumentException();
			default:
				throw new SocketException(error);
			}
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified value, represented as an object.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionValue">A <see cref="T:System.Net.Sockets.LingerOption" /> or <see cref="T:System.Net.Sockets.MulticastOption" /> that contains the value of the option. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="optionValue" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (optionValue == null)
			{
				throw new ArgumentNullException("optionValue");
			}
			int error;
			if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
			{
				LingerOption lingerOption = optionValue as LingerOption;
				if (lingerOption == null)
				{
					throw new ArgumentException("A 'LingerOption' value must be specified.", "optionValue");
				}
				SetSocketOption_internal(socket, optionLevel, optionName, lingerOption, null, 0, out error);
			}
			else if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
			{
				MulticastOption multicastOption = optionValue as MulticastOption;
				if (multicastOption == null)
				{
					throw new ArgumentException("A 'MulticastOption' value must be specified.", "optionValue");
				}
				SetSocketOption_internal(socket, optionLevel, optionName, multicastOption, null, 0, out error);
			}
			else
			{
				if (optionLevel != SocketOptionLevel.IPv6 || (optionName != SocketOptionName.AddMembership && optionName != SocketOptionName.DropMembership))
				{
					throw new ArgumentException("Invalid value specified.", "optionValue");
				}
				IPv6MulticastOption pv6MulticastOption = optionValue as IPv6MulticastOption;
				if (pv6MulticastOption == null)
				{
					throw new ArgumentException("A 'IPv6MulticastOption' value must be specified.", "optionValue");
				}
				SetSocketOption_internal(socket, optionLevel, optionName, pv6MulticastOption, null, 0, out error);
			}
			switch (error)
			{
			case 0:
				break;
			case 10022:
				throw new ArgumentException();
			default:
				throw new SocketException(error);
			}
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified <see cref="T:System.Boolean" /> value.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionValue">The value of the option, represented as a <see cref="T:System.Boolean" />. </param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			int int_val = optionValue ? 1 : 0;
			SetSocketOption_internal(socket, optionLevel, optionName, null, null, int_val, out int error);
			switch (error)
			{
			case 0:
				break;
			case 10022:
				throw new ArgumentException();
			default:
				throw new SocketException(error);
			}
		}

		internal static void CheckProtocolSupport()
		{
			if (ipv4Supported == -1)
			{
				try
				{
					Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					socket.Close();
					ipv4Supported = 1;
				}
				catch
				{
					ipv4Supported = 0;
				}
			}
			if (ipv6Supported == -1 && ipv6Supported != 0)
			{
				try
				{
					Socket socket2 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
					socket2.Close();
					ipv6Supported = 1;
				}
				catch
				{
					ipv6Supported = 0;
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr Socket_internal(AddressFamily family, SocketType type, ProtocolType proto, out int error);

		/// <summary>Frees resources used by the <see cref="T:System.Net.Sockets.Socket" /> class.</summary>
		~Socket()
		{
			Dispose(explicitDisposing: false);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Blocking_internal(IntPtr socket, bool block, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SocketAddress RemoteEndPoint_internal(IntPtr socket, out int error);

		private void Linger(IntPtr handle)
		{
			if (!connected || linger_timeout <= 0)
			{
				return;
			}
			Shutdown_internal(handle, SocketShutdown.Receive, out int error);
			if (error != 0)
			{
				return;
			}
			int num = linger_timeout / 1000;
			int num2 = linger_timeout % 1000;
			if (num2 > 0)
			{
				Poll_internal(handle, SelectMode.SelectRead, num2 * 1000, out error);
				if (error != 0)
				{
					return;
				}
			}
			if (num > 0)
			{
				LingerOption obj_val = new LingerOption(enable: true, num);
				SetSocketOption_internal(handle, SocketOptionLevel.Socket, SocketOptionName.Linger, obj_val, null, 0, out error);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.Socket" />, and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources. </param>
		protected virtual void Dispose(bool explicitDisposing)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			bool flag = connected;
			connected = false;
			if ((int)socket != -1)
			{
				if (Environment.SocketSecurityEnabled && current_bind_count > 0)
				{
					current_bind_count--;
				}
				closed = true;
				IntPtr handle = socket;
				socket = (IntPtr)(-1);
				Thread thread = blocking_thread;
				if (thread != null)
				{
					thread.Abort();
					blocking_thread = null;
				}
				if (flag)
				{
					Linger(handle);
				}
				Close_internal(handle, out int error);
				if (error != 0)
				{
					throw new SocketException(error);
				}
			}
		}

		public void Dispose()
		{
			Dispose(explicitDisposing: true);
			GC.SuppressFinalize(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Close_internal(IntPtr socket, out int error);

		/// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Close()
		{
			linger_timeout = 0;
			((IDisposable)this).Dispose();
		}

		/// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources with a specified timeout to allow queued data to be sent.</summary>
		/// <param name="timeout">Wait up to <paramref name="timeout" /> seconds to send any remaining data, then close the socket.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Close(int timeout)
		{
			linger_timeout = timeout;
			((IDisposable)this).Dispose();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Connect_internal_real(IntPtr sock, SocketAddress sa, out int error);

		private static void Connect_internal(IntPtr sock, SocketAddress sa, out int error)
		{
			Connect_internal(sock, sa, out error, requireSocketPolicyFile: true);
		}

		private static void Connect_internal(IntPtr sock, SocketAddress sa, out int error, bool requireSocketPolicyFile)
		{
			if (requireSocketPolicyFile && !CheckEndPoint(sa))
			{
				throw new SecurityException("Unable to connect, as no valid crossdomain policy was found");
			}
			Connect_internal_real(sock, sa, out error);
		}

		internal static bool CheckEndPoint(SocketAddress sa)
		{
			if (!Environment.SocketSecurityEnabled)
			{
				return true;
			}
			try
			{
				IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Loopback, 123);
				IPEndPoint iPEndPoint2 = (IPEndPoint)iPEndPoint.Create(sa);
				if (check_socket_policy == null)
				{
					check_socket_policy = GetUnityCrossDomainHelperMethod("CheckSocketEndPoint");
				}
				return (bool)check_socket_policy.Invoke(null, new object[2]
				{
					iPEndPoint2.Address.ToString(),
					iPEndPoint2.Port
				});
				IL_0077:
				bool result;
				return result;
			}
			catch (Exception arg)
			{
				Console.WriteLine("Unexpected error while trying to CheckEndPoint() : " + arg);
				return false;
				IL_0094:
				bool result;
				return result;
			}
		}

		private static MethodInfo GetUnityCrossDomainHelperMethod(string methodname)
		{
			Type type = Type.GetType("UnityEngine.UnityCrossDomainHelper, CrossDomainPolicyParser, Version=1.0.0.0, Culture=neutral");
			if (type == null)
			{
				throw new SecurityException("Cant find type UnityCrossDomainHelper");
			}
			MethodInfo method = type.GetMethod(methodname);
			if (method == null)
			{
				throw new SecurityException("Cant find " + methodname);
			}
			return method;
		}

		/// <summary>Establishes a connection to a remote host.</summary>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="remoteEP" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />ing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(EndPoint remoteEP)
		{
			Connect(remoteEP, requireSocketPolicy: true);
		}

		internal void Connect(EndPoint remoteEP, bool requireSocketPolicy)
		{
			SocketAddress socketAddress = null;
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			IPEndPoint iPEndPoint = remoteEP as IPEndPoint;
			if (iPEndPoint != null && (iPEndPoint.Address.Equals(IPAddress.Any) || iPEndPoint.Address.Equals(IPAddress.IPv6Any)))
			{
				throw new SocketException(10049);
			}
			if (islistening)
			{
				throw new InvalidOperationException();
			}
			socketAddress = remoteEP.Serialize();
			int error = 0;
			blocking_thread = Thread.CurrentThread;
			try
			{
				Connect_internal(socket, socketAddress, out error, requireSocketPolicy);
			}
			catch (ThreadAbortException)
			{
				if (disposed)
				{
					Thread.ResetAbort();
					error = 10004;
				}
			}
			finally
			{
				blocking_thread = null;
			}
			if (error != 0)
			{
				throw new SocketException(error);
			}
			connected = true;
			isbound = true;
			seed_endpoint = remoteEP;
		}

		/// <summary>Begins an asynchronous request to receive data from a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentException">An argument was invalid. The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> or <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> properties on the <paramref name="e" /> parameter must reference valid buffers. One or the other of these properties may be set, but not both at the same time.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		public bool ReceiveAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (e.BufferList != null)
			{
				throw new NotSupportedException("Mono doesn't support using BufferList at this point.");
			}
			e.DoOperation(SocketAsyncOperation.Receive, this);
			return true;
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <returns>Returns true if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation. Returns false if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> or <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> properties on the <paramref name="e" /> parameter must reference valid buffers. One or the other of these properties may be set, but not both at the same time.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">The <see cref="T:System.Net.Sockets.Socket" /> is not yet connected or was not obtained via an <see cref="M:System.Net.Sockets.Socket.Accept" />, <see cref="M:System.Net.Sockets.Socket.AcceptAsync(System.Net.Sockets.SocketAsyncEventArgs)" />,or <see cref="Overload:System.Net.Sockets.Socket.BeginAccept" />, method.</exception>
		public bool SendAsync(SocketAsyncEventArgs e)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (e.Buffer == null && e.BufferList == null)
			{
				throw new ArgumentException("Either e.Buffer or e.BufferList must be valid buffers.");
			}
			e.DoOperation(SocketAsyncOperation.Send, this);
			return true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Poll_internal(IntPtr socket, SelectMode mode, int timeout, out int error);

		internal bool Poll(int time_us, SelectMode mode, out int socket_error)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (mode != 0 && mode != SelectMode.SelectWrite && mode != SelectMode.SelectError)
			{
				throw new NotSupportedException("'mode' parameter is not valid.");
			}
			int error;
			bool flag = Poll_internal(socket, mode, time_us, out error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			socket_error = (int)GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
			if (mode == SelectMode.SelectWrite && flag && socket_error == 0)
			{
				connected = true;
			}
			return flag;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Receive_internal(IntPtr sock, byte[] buffer, int offset, int count, SocketFlags flags, out int error);

		internal int Receive_nochecks(byte[] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (protocol_type == ProtocolType.Udp && Environment.SocketSecurityEnabled)
			{
				IPAddress address = IPAddress.Any;
				if (address_family == AddressFamily.InterNetworkV6)
				{
					address = IPAddress.IPv6Any;
				}
				EndPoint remote_end = new IPEndPoint(address, 0);
				int error2 = 0;
				int result = ReceiveFrom_nochecks_exc(buf, offset, size, flags, ref remote_end, throwOnError: false, out error2);
				error = (SocketError)error2;
				return result;
			}
			int error3;
			int result2 = Receive_internal(socket, buf, offset, size, flags, out error3);
			error = (SocketError)error3;
			if (error != 0 && error != SocketError.WouldBlock && error != SocketError.InProgress)
			{
				connected = false;
			}
			else
			{
				connected = true;
			}
			return result2;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSocketOption_obj_internal(IntPtr socket, SocketOptionLevel level, SocketOptionName name, out object obj_val, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Send_internal(IntPtr sock, byte[] buf, int offset, int count, SocketFlags flags, out int error);

		internal int Send_nochecks(byte[] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (size == 0)
			{
				error = SocketError.Success;
				return 0;
			}
			int error2;
			int result = Send_internal(socket, buf, offset, size, flags, out error2);
			error = (SocketError)error2;
			if (error != 0 && error != SocketError.WouldBlock && error != SocketError.InProgress)
			{
				connected = false;
			}
			else
			{
				connected = true;
			}
			return result;
		}

		/// <summary>Returns the value of a specified <see cref="T:System.Net.Sockets.Socket" /> option, represented as an object.</summary>
		/// <returns>An object that represents the value of the option. When the <paramref name="optionName" /> parameter is set to <see cref="F:System.Net.Sockets.SocketOptionName.Linger" /> the return value is an instance of the <see cref="T:System.Net.Sockets.LingerOption" /> class. When <paramref name="optionName" /> is set to <see cref="F:System.Net.Sockets.SocketOptionName.AddMembership" /> or <see cref="F:System.Net.Sockets.SocketOptionName.DropMembership" />, the return value is an instance of the <see cref="T:System.Net.Sockets.MulticastOption" /> class. When <paramref name="optionName" /> is any other value, the return value is an integer.</returns>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.-or-<paramref name="optionName" /> was set to the unsupported value <see cref="F:System.Net.Sockets.SocketOptionName.MaxConnections" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			GetSocketOption_obj_internal(socket, optionLevel, optionName, out object obj_val, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
			switch (optionName)
			{
			case SocketOptionName.Linger:
				return (LingerOption)obj_val;
			case SocketOptionName.AddMembership:
			case SocketOptionName.DropMembership:
				return (MulticastOption)obj_val;
			default:
				if (obj_val is int)
				{
					return (int)obj_val;
				}
				return obj_val;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Shutdown_internal(IntPtr socket, SocketShutdown how, out int error);

		/// <summary>Disables sends and receives on a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="how">One of the <see cref="T:System.Net.Sockets.SocketShutdown" /> values that specifies the operation that will no longer be allowed. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Shutdown(SocketShutdown how)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			if (!connected)
			{
				throw new SocketException(10057);
			}
			Shutdown_internal(socket, how, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSocketOption_internal(IntPtr socket, SocketOptionLevel level, SocketOptionName name, object obj_val, byte[] byte_val, int int_val, out int error);

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified integer value.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values. </param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values. </param>
		/// <param name="optionValue">A value of the option. </param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
		{
			if (disposed && closed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
			SetSocketOption_internal(socket, optionLevel, optionName, null, null, optionValue, out int error);
			if (error != 0)
			{
				throw new SocketException(error);
			}
		}

		private void ThrowIfUpd()
		{
		}
	}
}
