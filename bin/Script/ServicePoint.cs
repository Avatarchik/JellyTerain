using System.Collections;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	/// <summary>Provides connection management for HTTP connections.</summary>
	public class ServicePoint
	{
		private Uri uri;

		private int connectionLimit;

		private int maxIdleTime;

		private int currentConnections;

		private DateTime idleSince;

		private Version protocolVersion;

		private X509Certificate certificate;

		private X509Certificate clientCertificate;

		private IPHostEntry host;

		private bool usesProxy;

		private Hashtable groups;

		private bool sendContinue = true;

		private bool useConnect;

		private object locker = new object();

		private object hostE = new object();

		private bool useNagle;

		private BindIPEndPoint endPointCallback;

		/// <summary>Gets the Uniform Resource Identifier (URI) of the server that this <see cref="T:System.Net.ServicePoint" /> object connects to.</summary>
		/// <returns>An instance of the <see cref="T:System.Uri" /> class that contains the URI of the Internet server that this <see cref="T:System.Net.ServicePoint" /> object connects to.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Net.ServicePoint" /> is in host mode.</exception>
		public Uri Address => uri;

		/// <summary>Specifies the delegate to associate a local <see cref="T:System.Net.IPEndPoint" /> with a <see cref="T:System.Net.ServicePoint" />.</summary>
		/// <returns>A delegate that forces a <see cref="T:System.Net.ServicePoint" /> to use a particular local Internet Protocol (IP) address and port number. The default value is null.</returns>
		public BindIPEndPoint BindIPEndPointDelegate
		{
			get
			{
				return endPointCallback;
			}
			set
			{
				endPointCallback = value;
			}
		}

		/// <summary>Gets the certificate received for this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>An instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class that contains the security certificate received for this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		public X509Certificate Certificate => certificate;

		/// <summary>Gets the last client certificate sent to the server.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object that contains the public values of the last client certificate sent to the server.</returns>
		public X509Certificate ClientCertificate => clientCertificate;

		/// <summary>Gets or sets the number of milliseconds after which an active <see cref="T:System.Net.ServicePoint" /> connection is closed.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the number of milliseconds that an active <see cref="T:System.Net.ServicePoint" /> connection remains open. The default is -1, which allows an active <see cref="T:System.Net.ServicePoint" /> connection to stay connected indefinitely. Set this property to 0 to force <see cref="T:System.Net.ServicePoint" /> connections to close after servicing a request.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is a negative number less than -1.</exception>
		[MonoTODO]
		public int ConnectionLeaseTimeout
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets or sets the maximum number of connections allowed on this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum number of connections allowed on this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The connection limit is equal to or less than 0. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ConnectionLimit
		{
			get
			{
				return connectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				connectionLimit = value;
			}
		}

		/// <summary>Gets the connection name. </summary>
		/// <returns>A <see cref="T:System.String" /> that represents the connection name. </returns>
		public string ConnectionName => uri.Scheme;

		/// <summary>Gets the number of open connections associated with this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The number of open connections associated with this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		public int CurrentConnections => currentConnections;

		/// <summary>Gets the date and time that the <see cref="T:System.Net.ServicePoint" /> object was last connected to a host.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that contains the date and time at which the <see cref="T:System.Net.ServicePoint" /> object was last connected.</returns>
		public DateTime IdleSince
		{
			get
			{
				return idleSince;
			}
			internal set
			{
				lock (locker)
				{
					idleSince = value;
				}
			}
		}

		/// <summary>Gets or sets the amount of time a connection associated with the <see cref="T:System.Net.ServicePoint" /> object can remain idle before the connection is closed.</summary>
		/// <returns>The length of time, in milliseconds, that a connection associated with the <see cref="T:System.Net.ServicePoint" /> object can remain idle before it is closed and reused for another connection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePoint.MaxIdleTime" /> is set to less than <see cref="F:System.Threading.Timeout.Infinite" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		public int MaxIdleTime
		{
			get
			{
				return maxIdleTime;
			}
			set
			{
				if (value < -1 || value > int.MaxValue)
				{
					throw new ArgumentOutOfRangeException();
				}
				maxIdleTime = value;
			}
		}

		/// <summary>Gets the version of the HTTP protocol that the <see cref="T:System.Net.ServicePoint" /> object uses.</summary>
		/// <returns>A <see cref="T:System.Version" /> object that contains the HTTP protocol version that the <see cref="T:System.Net.ServicePoint" /> object uses.</returns>
		public virtual Version ProtocolVersion => protocolVersion;

		/// <summary>Gets or sets the size of the receiving buffer for the socket used by this <see cref="T:System.Net.ServicePoint" />.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that contains the size, in bytes, of the receive buffer. The default is 8192.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		[MonoTODO]
		public int ReceiveBufferSize
		{
			get
			{
				throw GetMustImplement();
			}
			set
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Indicates whether the <see cref="T:System.Net.ServicePoint" /> object supports pipelined connections.</summary>
		/// <returns>true if the <see cref="T:System.Net.ServicePoint" /> object supports pipelined connections; otherwise, false.</returns>
		public bool SupportsPipelining => HttpVersion.Version11.Equals(protocolVersion);

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether 100-Continue behavior is used.</summary>
		/// <returns>true to expect 100-Continue responses for POST requests; otherwise, false. The default value is true.</returns>
		public bool Expect100Continue
		{
			get
			{
				return SendContinue;
			}
			set
			{
				SendContinue = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether the Nagle algorithm is used on connections managed by this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>true to use the Nagle algorithm; otherwise, false. The default value is true.</returns>
		public bool UseNagleAlgorithm
		{
			get
			{
				return useNagle;
			}
			set
			{
				useNagle = value;
			}
		}

		internal bool SendContinue
		{
			get
			{
				return sendContinue && (protocolVersion == null || protocolVersion == HttpVersion.Version11);
			}
			set
			{
				sendContinue = value;
			}
		}

		internal bool UsesProxy
		{
			get
			{
				return usesProxy;
			}
			set
			{
				usesProxy = value;
			}
		}

		internal bool UseConnect
		{
			get
			{
				return useConnect;
			}
			set
			{
				useConnect = value;
			}
		}

		internal bool AvailableForRecycling => CurrentConnections == 0 && maxIdleTime != -1 && DateTime.Now >= IdleSince.AddMilliseconds(maxIdleTime);

		internal Hashtable Groups
		{
			get
			{
				if (groups == null)
				{
					groups = new Hashtable();
				}
				return groups;
			}
		}

		internal IPHostEntry HostEntry
		{
			get
			{
				lock (hostE)
				{
					if (host != null)
					{
						return host;
					}
					string text = uri.Host;
					if (uri.HostNameType == UriHostNameType.IPv6 || uri.HostNameType == UriHostNameType.IPv4)
					{
						if (uri.HostNameType == UriHostNameType.IPv6)
						{
							text = text.Substring(1, text.Length - 2);
						}
						host = new IPHostEntry();
						host.AddressList = new IPAddress[1]
						{
							IPAddress.Parse(text)
						};
						return host;
					}
					try
					{
						host = Dns.GetHostByName(text);
					}
					catch
					{
						return null;
						IL_00bd:;
					}
				}
				return host;
			}
		}

		internal ServicePoint(Uri uri, int connectionLimit, int maxIdleTime)
		{
			this.uri = uri;
			this.connectionLimit = connectionLimit;
			this.maxIdleTime = maxIdleTime;
			currentConnections = 0;
			idleSince = DateTime.Now;
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		internal void SetVersion(Version version)
		{
			protocolVersion = version;
		}

		private WebConnectionGroup GetConnectionGroup(string name)
		{
			if (name == null)
			{
				name = string.Empty;
			}
			WebConnectionGroup webConnectionGroup = Groups[name] as WebConnectionGroup;
			if (webConnectionGroup != null)
			{
				return webConnectionGroup;
			}
			webConnectionGroup = new WebConnectionGroup(this, name);
			Groups[name] = webConnectionGroup;
			return webConnectionGroup;
		}

		internal EventHandler SendRequest(HttpWebRequest request, string groupName)
		{
			WebConnection connection;
			lock (locker)
			{
				WebConnectionGroup connectionGroup = GetConnectionGroup(groupName);
				connection = connectionGroup.GetConnection(request);
			}
			return connection.SendRequest(request);
		}

		/// <summary>Removes the specified connection group from this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that indicates whether the connection group was closed.</returns>
		/// <param name="connectionGroupName">The name of the connection group that contains the connections to close and remove from this service point. </param>
		public bool CloseConnectionGroup(string connectionGroupName)
		{
			lock (locker)
			{
				WebConnectionGroup connectionGroup = GetConnectionGroup(connectionGroupName);
				if (connectionGroup != null)
				{
					connectionGroup.Close();
					return true;
				}
			}
			return false;
		}

		internal void IncrementConnection()
		{
			lock (locker)
			{
				currentConnections++;
				idleSince = DateTime.Now.AddMilliseconds(1000000.0);
			}
		}

		internal void DecrementConnection()
		{
			lock (locker)
			{
				currentConnections--;
				if (currentConnections == 0)
				{
					idleSince = DateTime.Now;
				}
			}
		}

		internal void SetCertificates(X509Certificate client, X509Certificate server)
		{
			certificate = server;
			clientCertificate = client;
		}

		internal bool CallEndPointDelegate(Socket sock, IPEndPoint remote)
		{
			if (endPointCallback == null)
			{
				return true;
			}
			int num = 0;
			while (true)
			{
				IPEndPoint iPEndPoint = null;
				try
				{
					iPEndPoint = endPointCallback(this, remote, num);
				}
				catch
				{
					return false;
					IL_0032:;
				}
				if (iPEndPoint == null)
				{
					return true;
				}
				try
				{
					sock.Bind(iPEndPoint);
				}
				catch (SocketException)
				{
					num = checked(num + 1);
					continue;
					IL_0055:;
				}
				break;
			}
			return true;
		}
	}
}
