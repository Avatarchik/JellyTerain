using System.Net.Sockets;

namespace System.Net
{
	/// <summary>Identifies a network address. This is an abstract class.</summary>
	[Serializable]
	public abstract class EndPoint
	{
		/// <summary>Gets the address family to which the endpoint belongs.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values.</returns>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to get or set the property when the property is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual AddressFamily AddressFamily
		{
			get
			{
				throw NotImplemented();
			}
		}

		/// <summary>Creates an <see cref="T:System.Net.EndPoint" /> instance from a <see cref="T:System.Net.SocketAddress" /> instance.</summary>
		/// <returns>A new <see cref="T:System.Net.EndPoint" /> instance that is initialized from the specified <see cref="T:System.Net.SocketAddress" /> instance.</returns>
		/// <param name="socketAddress">The socket address that serves as the endpoint for a connection. </param>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual EndPoint Create(SocketAddress address)
		{
			throw NotImplemented();
		}

		/// <summary>Serializes endpoint information into a <see cref="T:System.Net.SocketAddress" /> instance.</summary>
		/// <returns>A <see cref="T:System.Net.SocketAddress" /> instance that contains the endpoint information.</returns>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual SocketAddress Serialize()
		{
			throw NotImplemented();
		}

		private static Exception NotImplemented()
		{
			return new NotImplementedException();
		}
	}
}
