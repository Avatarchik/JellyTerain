using System.Net.Sockets;

namespace System.Net
{
	/// <summary>Represents a network endpoint as an IP address and a port number.</summary>
	[Serializable]
	public class IPEndPoint : EndPoint
	{
		/// <summary>Specifies the maximum value that can be assigned to the <see cref="P:System.Net.IPEndPoint.Port" /> property. The MaxPort value is set to 0x0000FFFF. This field is read-only.</summary>
		public const int MaxPort = 65535;

		/// <summary>Specifies the minimum value that can be assigned to the <see cref="P:System.Net.IPEndPoint.Port" /> property. This field is read-only.</summary>
		public const int MinPort = 0;

		private IPAddress address;

		private int port;

		/// <summary>Gets or sets the IP address of the endpoint.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> instance containing the IP address of the endpoint.</returns>
		public IPAddress Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		/// <summary>Gets the Internet Protocol (IP) address family.</summary>
		/// <returns>Returns <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" />.</returns>
		public override AddressFamily AddressFamily => address.AddressFamily;

		/// <summary>Gets or sets the port number of the endpoint.</summary>
		/// <returns>An integer value in the range <see cref="F:System.Net.IPEndPoint.MinPort" /> to <see cref="F:System.Net.IPEndPoint.MaxPort" /> indicating the port number of the endpoint.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value that was specified for a set operation is less than <see cref="F:System.Net.IPEndPoint.MinPort" /> or greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />. </exception>
		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				if (value < 0 || value > 65535)
				{
					throw new ArgumentOutOfRangeException("Invalid port");
				}
				port = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.IPEndPoint" /> class with the specified address and port number.</summary>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" />. </param>
		/// <param name="port">The port number associated with the <paramref name="address" />, or 0 to specify any available port. <paramref name="port" /> is in host order.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than <see cref="F:System.Net.IPEndPoint.MinPort" />.-or- <paramref name="port" /> is greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.-or- <paramref name="address" /> is less than 0 or greater than 0x00000000FFFFFFFF. </exception>
		public IPEndPoint(IPAddress address, int port)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Address = address;
			Port = port;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.IPEndPoint" /> class with the specified address and port number.</summary>
		/// <param name="address">The IP address of the Internet host. </param>
		/// <param name="port">The port number associated with the <paramref name="address" />, or 0 to specify any available port. <paramref name="port" /> is in host order.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than <see cref="F:System.Net.IPEndPoint.MinPort" />.-or- <paramref name="port" /> is greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.-or- <paramref name="address" /> is less than 0 or greater than 0x00000000FFFFFFFF. </exception>
		public IPEndPoint(long iaddr, int port)
		{
			Address = new IPAddress(iaddr);
			Port = port;
		}

		/// <summary>Creates an endpoint from a socket address.</summary>
		/// <returns>An <see cref="T:System.Net.EndPoint" /> instance using the specified socket address.</returns>
		/// <param name="socketAddress">The <see cref="T:System.Net.SocketAddress" /> to use for the endpoint. </param>
		/// <exception cref="T:System.ArgumentException">The AddressFamily of <paramref name="socketAddress" /> is not equal to the AddressFamily of the current instance.-or- <paramref name="socketAddress" />.Size &lt; 8. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override EndPoint Create(SocketAddress socketAddress)
		{
			if (socketAddress == null)
			{
				throw new ArgumentNullException("socketAddress");
			}
			if (socketAddress.Family != AddressFamily)
			{
				throw new ArgumentException("The IPEndPoint was created using " + AddressFamily + " AddressFamily but SocketAddress contains " + socketAddress.Family + " instead, please use the same type.");
			}
			int size = socketAddress.Size;
			AddressFamily family = socketAddress.Family;
			IPEndPoint iPEndPoint = null;
			switch (family)
			{
			case AddressFamily.InterNetwork:
			{
				if (size < 8)
				{
					return null;
				}
				int num = (socketAddress[2] << 8) + socketAddress[3];
				long iaddr = ((long)(int)socketAddress[7] << 24) + ((long)(int)socketAddress[6] << 16) + ((long)(int)socketAddress[5] << 8) + (int)socketAddress[4];
				return new IPEndPoint(iaddr, num);
			}
			case AddressFamily.InterNetworkV6:
			{
				if (size < 28)
				{
					return null;
				}
				int num = (socketAddress[2] << 8) + socketAddress[3];
				int num2 = socketAddress[24] + (socketAddress[25] << 8) + (socketAddress[26] << 16) + (socketAddress[27] << 24);
				ushort[] array = new ushort[8];
				for (int i = 0; i < 8; i++)
				{
					array[i] = (ushort)((socketAddress[8 + i * 2] << 8) + socketAddress[8 + i * 2 + 1]);
				}
				return new IPEndPoint(new IPAddress(array, num2), num);
			}
			default:
				return null;
			}
		}

		/// <summary>Serializes endpoint information into a <see cref="T:System.Net.SocketAddress" /> instance.</summary>
		/// <returns>A <see cref="T:System.Net.SocketAddress" /> instance containing the socket address for the endpoint.</returns>
		public override SocketAddress Serialize()
		{
			SocketAddress socketAddress = null;
			switch (address.AddressFamily)
			{
			case AddressFamily.InterNetwork:
			{
				socketAddress = new SocketAddress(AddressFamily.InterNetwork, 16);
				socketAddress[2] = (byte)((port >> 8) & 0xFF);
				socketAddress[3] = (byte)(port & 0xFF);
				long internalIPv4Address = address.InternalIPv4Address;
				socketAddress[4] = (byte)(internalIPv4Address & 0xFF);
				socketAddress[5] = (byte)((internalIPv4Address >> 8) & 0xFF);
				socketAddress[6] = (byte)((internalIPv4Address >> 16) & 0xFF);
				socketAddress[7] = (byte)((internalIPv4Address >> 24) & 0xFF);
				break;
			}
			case AddressFamily.InterNetworkV6:
			{
				socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
				socketAddress[2] = (byte)((port >> 8) & 0xFF);
				socketAddress[3] = (byte)(port & 0xFF);
				byte[] addressBytes = address.GetAddressBytes();
				for (int i = 0; i < 16; i++)
				{
					socketAddress[8 + i] = addressBytes[i];
				}
				socketAddress[24] = (byte)(address.ScopeId & 0xFF);
				socketAddress[25] = (byte)((address.ScopeId >> 8) & 0xFF);
				socketAddress[26] = (byte)((address.ScopeId >> 16) & 0xFF);
				socketAddress[27] = (byte)((address.ScopeId >> 24) & 0xFF);
				break;
			}
			}
			return socketAddress;
		}

		/// <summary>Returns the IP address and port number of the specified endpoint.</summary>
		/// <returns>A string containing the IP address and the port number of the specified endpoint (for example, 192.168.1.2:80).</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override string ToString()
		{
			return address.ToString() + ":" + port;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Net.IPEndPoint" /> instance.</summary>
		/// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
		/// <param name="comparand">The specified <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Net.IPEndPoint" /> instance.</param>
		public override bool Equals(object obj)
		{
			IPEndPoint iPEndPoint = obj as IPEndPoint;
			return iPEndPoint != null && iPEndPoint.port == port && iPEndPoint.address.Equals(address);
		}

		/// <summary>Returns a hash value for a <see cref="T:System.Net.IPEndPoint" /> instance.</summary>
		/// <returns>An integer hash value.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return address.GetHashCode() + port;
		}
	}
}
