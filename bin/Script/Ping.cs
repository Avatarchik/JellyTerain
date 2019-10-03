using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace System.Net.NetworkInformation
{
	/// <summary>Allows an application to determine whether a remote computer is accessible over the network.</summary>
	[MonoTODO("IPv6 support is missing")]
	public class Ping : Component, IDisposable
	{
		private struct cap_user_header_t
		{
			public uint version;

			public int pid;
		}

		private struct cap_user_data_t
		{
			public uint effective;

			public uint permitted;

			public uint inheritable;
		}

		private class IcmpMessage
		{
			private byte[] bytes;

			public byte Type => bytes[0];

			public byte Code => bytes[1];

			public byte Identifier => (byte)(bytes[4] + (bytes[5] << 8));

			public byte Sequence => (byte)(bytes[6] + (bytes[7] << 8));

			public byte[] Data
			{
				get
				{
					byte[] array = new byte[bytes.Length - 8];
					Buffer.BlockCopy(bytes, 0, array, 0, array.Length);
					return array;
				}
			}

			public IPStatus IPStatus
			{
				get
				{
					switch (Type)
					{
					case 0:
						return IPStatus.Success;
					case 3:
						switch (Code)
						{
						case 0:
							return IPStatus.DestinationNetworkUnreachable;
						case 1:
							return IPStatus.DestinationHostUnreachable;
						case 2:
							return IPStatus.DestinationProhibited;
						case 3:
							return IPStatus.DestinationPortUnreachable;
						case 4:
							return IPStatus.BadOption;
						case 5:
							return IPStatus.BadRoute;
						}
						break;
					case 11:
						switch (Code)
						{
						case 0:
							return IPStatus.TimeExceeded;
						case 1:
							return IPStatus.TtlReassemblyTimeExceeded;
						}
						break;
					case 12:
						return IPStatus.ParameterProblem;
					case 4:
						return IPStatus.SourceQuench;
					case 8:
						return IPStatus.Success;
					}
					return IPStatus.Unknown;
				}
			}

			public IcmpMessage(byte[] bytes, int offset, int size)
			{
				this.bytes = new byte[size];
				Buffer.BlockCopy(bytes, offset, this.bytes, 0, size);
			}

			public IcmpMessage(byte type, byte code, short identifier, short sequence, byte[] data)
			{
				bytes = new byte[data.Length + 8];
				bytes[0] = type;
				bytes[1] = code;
				bytes[4] = (byte)(identifier & 0xFF);
				bytes[5] = (byte)(identifier >> 8);
				bytes[6] = (byte)(sequence & 0xFF);
				bytes[7] = (byte)(sequence >> 8);
				Buffer.BlockCopy(data, 0, bytes, 8, data.Length);
				ushort num = ComputeChecksum(bytes);
				bytes[2] = (byte)(num & 0xFF);
				bytes[3] = (byte)(num >> 8);
			}

			public byte[] GetBytes()
			{
				return bytes;
			}

			private static ushort ComputeChecksum(byte[] data)
			{
				uint num = 0u;
				for (int i = 0; i < data.Length; i += 2)
				{
					ushort num2 = (ushort)((i + 1 < data.Length) ? data[i + 1] : 0);
					num2 = (ushort)(num2 << 8);
					num2 = (ushort)(num2 + data[i]);
					num += num2;
				}
				num = (num >> 16) + (num & 0xFFFF);
				return (ushort)(~num);
			}
		}

		private const int DefaultCount = 1;

		private const int default_timeout = 4000;

		private const int identifier = 1;

		private const uint linux_cap_version = 537333798u;

		private static readonly string[] PingBinPaths;

		private static readonly string PingBinPath;

		private static readonly byte[] default_buffer;

		private static bool canSendPrivileged;

		private BackgroundWorker worker;

		private object user_async_state;

		/// <summary>Occurs when an asynchronous operation to send an Internet Control Message Protocol (ICMP) echo message and receive the corresponding ICMP echo reply message completes or is canceled.</summary>
		public event PingCompletedEventHandler PingCompleted;

		static Ping()
		{
			PingBinPaths = new string[4]
			{
				"/bin/ping",
				"/sbin/ping",
				"/usr/sbin/ping",
				"/system/bin/ping"
			};
			default_buffer = new byte[0];
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				CheckLinuxCapabilities();
				if (!canSendPrivileged && WindowsIdentity.GetCurrent().Name == "root")
				{
					canSendPrivileged = true;
				}
				string[] pingBinPaths = PingBinPaths;
				foreach (string text in pingBinPaths)
				{
					if (File.Exists(text))
					{
						PingBinPath = text;
						break;
					}
				}
			}
			else
			{
				canSendPrivileged = true;
			}
			if (PingBinPath == null)
			{
				PingBinPath = "/bin/ping";
			}
		}

		/// <summary>Releases all resources used by instances of the <see cref="T:System.Net.NetworkInformation.Ping" /> class.</summary>
		void IDisposable.Dispose()
		{
		}

		[DllImport("libc")]
		private static extern int capget(ref cap_user_header_t header, ref cap_user_data_t data);

		private static void CheckLinuxCapabilities()
		{
			try
			{
				cap_user_header_t header = default(cap_user_header_t);
				cap_user_data_t data = default(cap_user_data_t);
				header.version = 537333798u;
				int num = -1;
				try
				{
					num = capget(ref header, ref data);
				}
				catch (Exception)
				{
				}
				if (num != -1)
				{
					canSendPrivileged = ((data.effective & 0x2000) != 0);
				}
			}
			catch
			{
				canSendPrivileged = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.NetworkInformation.Ping.PingCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.NetworkInformation.PingCompletedEventArgs" />  object that contains event data.</param>
		protected void OnPingCompleted(PingCompletedEventArgs e)
		{
			if (this.PingCompleted != null)
			{
				this.PingCompleted(this, e);
			}
			user_async_state = null;
			worker = null;
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message, if one was received, or describes the reason for the failure if no message was received.</returns>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public PingReply Send(IPAddress address)
		{
			return Send(address, 4000);
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer. This method allows you to specify a time-out value for the operation. </summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public PingReply Send(IPAddress address, int timeout)
		{
			return Send(address, timeout, default_buffer);
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received. The method will return <see cref="F:System.Net.NetworkInformation.IPStatus.PacketTooBig" /> if the packet exceeds the Maximum Transmission Unit (MTU).</returns>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.-or-<paramref name="buffer" /> is null, or the <paramref name="buffer" /> size is greater than 65500 bytes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public PingReply Send(IPAddress address, int timeout, byte[] buffer)
		{
			return Send(address, timeout, buffer, new PingOptions());
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received.</returns>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public PingReply Send(string hostNameOrAddress)
		{
			return Send(hostNameOrAddress, 4000);
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This method allows you to specify a time-out value for the operation.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public PingReply Send(string hostNameOrAddress, int timeout)
		{
			return Send(hostNameOrAddress, timeout, default_buffer);
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").-or-<paramref name="buffer" /> is null, or the <paramref name="buffer" /> size is greater than 65500 bytes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
		{
			return Send(hostNameOrAddress, timeout, buffer, new PingOptions());
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP packet.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message if one was received, or provides the reason for the failure if no message was received.</returns>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="options">A <see cref="T:System.Net.NetworkInformation.PingOptions" />  object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is a zero length string.-or-<paramref name="buffer" /> is null, or the <paramref name="buffer" /> size is greater than 65500 bytes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostNameOrAddress);
			return Send(hostAddresses[0], timeout, buffer, options);
		}

		private static IPAddress GetNonLoopbackIP()
		{
			IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (!IPAddress.IsLoopback(iPAddress))
				{
					return iPAddress;
				}
			}
			throw new InvalidOperationException("Could not resolve non-loopback IP address for localhost");
		}

		/// <summary>Attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="T:System.Net.IPAddress" /> and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP echo message packet.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PingReply" /> object that provides information about the ICMP echo reply message, if one was received, or provides the reason for the failure, if no message was received. The method will return <see cref="F:System.Net.NetworkInformation.IPStatus.PacketTooBig" /> if the packet exceeds the Maximum Transmission Unit (MTU).</returns>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="options">A <see cref="T:System.Net.NetworkInformation.PingOptions" />  object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.-or-<paramref name="buffer" /> is null, or the <paramref name="buffer" /> size is greater than 65500 bytes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout", "timeout must be non-negative integer");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException("buffer");
			}
			if (canSendPrivileged)
			{
				return SendPrivileged(address, timeout, buffer, options);
			}
			return SendUnprivileged(address, timeout, buffer, options);
		}

		private PingReply SendPrivileged(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			IPEndPoint iPEndPoint = new IPEndPoint(address, 0);
			IPEndPoint iPEndPoint2 = new IPEndPoint(GetNonLoopbackIP(), 0);
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp))
			{
				if (options != null)
				{
					socket.DontFragment = options.DontFragment;
					socket.Ttl = (short)options.Ttl;
				}
				socket.SendTimeout = timeout;
				socket.ReceiveTimeout = timeout;
				IcmpMessage icmpMessage = new IcmpMessage(8, 0, 1, 0, buffer);
				byte[] bytes = icmpMessage.GetBytes();
				socket.SendBufferSize = bytes.Length;
				socket.SendTo(bytes, bytes.Length, SocketFlags.None, iPEndPoint);
				DateTime now = DateTime.Now;
				bytes = new byte[100];
				while (true)
				{
					EndPoint remote_end = iPEndPoint2;
					int error = 0;
					int num = socket.ReceiveFrom_nochecks_exc(bytes, 0, 100, SocketFlags.None, ref remote_end, throwOnError: false, out error);
					switch (error)
					{
					case 10060:
						return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
					default:
						throw new NotSupportedException($"Unexpected socket error during ping request: {error}");
					case 0:
					{
						long num2 = (long)(DateTime.Now - now).TotalMilliseconds;
						int num3 = (bytes[0] & 0xF) << 2;
						int size = num - num3;
						if (!((IPEndPoint)remote_end).Address.Equals(iPEndPoint.Address))
						{
							long num4 = timeout - num2;
							if (num4 <= 0)
							{
								return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
							}
							socket.ReceiveTimeout = (int)num4;
						}
						else
						{
							IcmpMessage icmpMessage2 = new IcmpMessage(bytes, num3, size);
							if (icmpMessage2.Identifier == 1 && icmpMessage2.Type != 8)
							{
								return new PingReply(address, icmpMessage2.Data, options, num2, icmpMessage2.IPStatus);
							}
							long num5 = timeout - num2;
							if (num5 <= 0)
							{
								return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
							}
							socket.ReceiveTimeout = (int)num5;
						}
						break;
					}
					}
				}
				IL_01ed:
				PingReply result;
				return result;
			}
		}

		private PingReply SendUnprivileged(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			DateTime now = DateTime.Now;
			Process process = new Process();
			string arguments = BuildPingArgs(address, timeout, options);
			long roundtripTime = 0L;
			process.StartInfo.FileName = PingBinPath;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			DateTime utcNow = DateTime.UtcNow;
			try
			{
				process.Start();
				roundtripTime = (long)(DateTime.Now - now).TotalMilliseconds;
				if (!process.WaitForExit(timeout) || (process.HasExited && process.ExitCode == 2))
				{
					return new PingReply(address, buffer, options, roundtripTime, IPStatus.TimedOut);
				}
				if (process.ExitCode == 1)
				{
					return new PingReply(address, buffer, options, roundtripTime, IPStatus.TtlExpired);
				}
			}
			catch (Exception)
			{
				return new PingReply(address, buffer, options, roundtripTime, IPStatus.Unknown);
				IL_00fd:;
			}
			finally
			{
				if (process != null)
				{
					if (!process.HasExited)
					{
						process.Kill();
					}
					process.Dispose();
				}
			}
			return new PingReply(address, buffer, options, roundtripTime, IPStatus.Success);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.-or-<paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, object userToken)
		{
			SendAsync(address, 4000, default_buffer, new PingOptions(), userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="M:System.Net.NetworkInformation.Ping.SendAsync(System.Net.IPAddress,System.Int32,System.Byte[],System.Object)" />  method is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public void SendAsync(IPAddress address, int timeout, object userToken)
		{
			SendAsync(address, 4000, default_buffer, userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer.</summary>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to the <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  method is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SendAsync(IPAddress address, object userToken)
		{
			SendAsync(address, 4000, userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").-or-<paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="hostNameOrAddress" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, object userToken)
		{
			SendAsync(hostNameOrAddress, timeout, buffer, new PingOptions(), userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP packet.</summary>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="buffer">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="options">A <see cref="T:System.Net.NetworkInformation.PingOptions" />  object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").-or-<paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			IPAddress address = Dns.GetHostEntry(hostNameOrAddress).AddressList[0];
			SendAsync(address, timeout, buffer, options, userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation.</summary>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="timeout">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" /> is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="hostNameOrAddress" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public void SendAsync(string hostNameOrAddress, int timeout, object userToken)
		{
			SendAsync(hostNameOrAddress, timeout, default_buffer, userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified computer, and receive a corresponding ICMP echo reply message from that computer.</summary>
		/// <param name="hostNameOrAddress">A <see cref="T:System.String" /> that identifies the computer that is the destination for the ICMP echo message. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null or is an empty string ("").</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="M:System.Net.NetworkInformation.Ping.SendAsync(System.String,System.Object)" />  method is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="hostNameOrAddress" /> could not be resolved to a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SendAsync(string hostNameOrAddress, object userToken)
		{
			SendAsync(hostNameOrAddress, 4000, userToken);
		}

		/// <summary>Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message with the specified data buffer to the computer that has the specified <see cref="T:System.Net.IPAddress" />, and receive a corresponding ICMP echo reply message from that computer. This overload allows you to specify a time-out value for the operation and control fragmentation and Time-to-Live values for the ICMP echo message packet.</summary>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" /> that identifies the computer that is the destination for the ICMP echo message.</param>
		/// <param name="timeout">A <see cref="T:System.Byte" /> array that contains data to be sent with the ICMP echo message and returned in the ICMP echo reply message. The array cannot contain more than 65,500 bytes.</param>
		/// <param name="buffer">An <see cref="T:System.Int32" /> value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
		/// <param name="options">A <see cref="T:System.Net.NetworkInformation.PingOptions" />  object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param>
		/// <param name="userToken">An object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null.-or-<paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">A call to <see cref="Overload:System.Net.NetworkInformation.Ping.SendAsync" />  is in progress.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="address" /> is an IPv6 address and the local computer is running an operating system earlier than Windows 2000. </exception>
		/// <exception cref="T:System.Net.NetworkInformation.PingException">An exception was thrown while sending or receiving the ICMP messages. See the inner exception for the exact exception that was thrown.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="address" /> is not a valid IP address.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.ArgumentException">The size of <paramref name="buffer" /> exceeds 65500 bytes.</exception>
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			if (worker != null)
			{
				throw new InvalidOperationException("Another SendAsync operation is in progress");
			}
			worker = new BackgroundWorker();
			worker.DoWork += delegate(object o, DoWorkEventArgs ea)
			{
				try
				{
					user_async_state = ea.Argument;
					ea.Result = Send(address, timeout, buffer, options);
				}
				catch (Exception ex)
				{
					Exception ex3 = (Exception)(ea.Result = ex);
				}
			};
			worker.WorkerSupportsCancellation = true;
			worker.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs ea)
			{
				OnPingCompleted(new PingCompletedEventArgs(ea.Error, ea.Cancelled, user_async_state, ea.Result as PingReply));
			};
			worker.RunWorkerAsync(userToken);
		}

		/// <summary>Cancels all pending asynchronous requests to send an Internet Control Message Protocol (ICMP) echo message and receives a corresponding ICMP echo reply message.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void SendAsyncCancel()
		{
			if (worker == null)
			{
				throw new InvalidOperationException("SendAsync operation is not in progress");
			}
			worker.CancelAsync();
		}

		private string BuildPingArgs(IPAddress address, int timeout, PingOptions options)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			StringBuilder stringBuilder = new StringBuilder();
			uint num = Convert.ToUInt32(Math.Floor((double)(timeout + 1000) / 1000.0));
			bool flag = Environment.OSVersion.Platform == PlatformID.MacOSX;
			if (!flag)
			{
				stringBuilder.AppendFormat(invariantCulture, "-q -n -c {0} -w {1} -t {2} -M ", 1, num, options.Ttl);
			}
			else
			{
				stringBuilder.AppendFormat(invariantCulture, "-q -n -c {0} -t {1} -o -m {2} ", 1, num, options.Ttl);
			}
			if (!flag)
			{
				stringBuilder.Append((!options.DontFragment) ? "dont " : "do ");
			}
			else if (options.DontFragment)
			{
				stringBuilder.Append("-D ");
			}
			stringBuilder.Append(address.ToString());
			return stringBuilder.ToString();
		}
	}
}
