using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[Serializable]
	public class ConnectionConfig
	{
		private const int g_MinPacketSize = 128;

		[SerializeField]
		private ushort m_PacketSize;

		[SerializeField]
		private ushort m_FragmentSize;

		[SerializeField]
		private uint m_ResendTimeout;

		[SerializeField]
		private uint m_DisconnectTimeout;

		[SerializeField]
		private uint m_ConnectTimeout;

		[SerializeField]
		private uint m_MinUpdateTimeout;

		[SerializeField]
		private uint m_PingTimeout;

		[SerializeField]
		private uint m_ReducedPingTimeout;

		[SerializeField]
		private uint m_AllCostTimeout;

		[SerializeField]
		private byte m_NetworkDropThreshold;

		[SerializeField]
		private byte m_OverflowDropThreshold;

		[SerializeField]
		private byte m_MaxConnectionAttempt;

		[SerializeField]
		private uint m_AckDelay;

		[SerializeField]
		private uint m_SendDelay;

		[SerializeField]
		private ushort m_MaxCombinedReliableMessageSize;

		[SerializeField]
		private ushort m_MaxCombinedReliableMessageCount;

		[SerializeField]
		private ushort m_MaxSentMessageQueueSize;

		[SerializeField]
		private ConnectionAcksType m_AcksType;

		[SerializeField]
		private bool m_UsePlatformSpecificProtocols;

		[SerializeField]
		private uint m_InitialBandwidth;

		[SerializeField]
		private float m_BandwidthPeakFactor;

		[SerializeField]
		private ushort m_WebSocketReceiveBufferMaxSize;

		[SerializeField]
		private uint m_UdpSocketReceiveBufferMaxSize;

		[SerializeField]
		private string m_SSLCertFilePath;

		[SerializeField]
		private string m_SSLPrivateKeyFilePath;

		[SerializeField]
		private string m_SSLCAFilePath;

		[SerializeField]
		internal List<ChannelQOS> m_Channels = new List<ChannelQOS>();

		public ushort PacketSize
		{
			get
			{
				return m_PacketSize;
			}
			set
			{
				m_PacketSize = value;
			}
		}

		public ushort FragmentSize
		{
			get
			{
				return m_FragmentSize;
			}
			set
			{
				m_FragmentSize = value;
			}
		}

		public uint ResendTimeout
		{
			get
			{
				return m_ResendTimeout;
			}
			set
			{
				m_ResendTimeout = value;
			}
		}

		public uint DisconnectTimeout
		{
			get
			{
				return m_DisconnectTimeout;
			}
			set
			{
				m_DisconnectTimeout = value;
			}
		}

		public uint ConnectTimeout
		{
			get
			{
				return m_ConnectTimeout;
			}
			set
			{
				m_ConnectTimeout = value;
			}
		}

		public uint MinUpdateTimeout
		{
			get
			{
				return m_MinUpdateTimeout;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("Minimal update timeout should be > 0");
				}
				m_MinUpdateTimeout = value;
			}
		}

		public uint PingTimeout
		{
			get
			{
				return m_PingTimeout;
			}
			set
			{
				m_PingTimeout = value;
			}
		}

		public uint ReducedPingTimeout
		{
			get
			{
				return m_ReducedPingTimeout;
			}
			set
			{
				m_ReducedPingTimeout = value;
			}
		}

		public uint AllCostTimeout
		{
			get
			{
				return m_AllCostTimeout;
			}
			set
			{
				m_AllCostTimeout = value;
			}
		}

		public byte NetworkDropThreshold
		{
			get
			{
				return m_NetworkDropThreshold;
			}
			set
			{
				m_NetworkDropThreshold = value;
			}
		}

		public byte OverflowDropThreshold
		{
			get
			{
				return m_OverflowDropThreshold;
			}
			set
			{
				m_OverflowDropThreshold = value;
			}
		}

		public byte MaxConnectionAttempt
		{
			get
			{
				return m_MaxConnectionAttempt;
			}
			set
			{
				m_MaxConnectionAttempt = value;
			}
		}

		public uint AckDelay
		{
			get
			{
				return m_AckDelay;
			}
			set
			{
				m_AckDelay = value;
			}
		}

		public uint SendDelay
		{
			get
			{
				return m_SendDelay;
			}
			set
			{
				m_SendDelay = value;
			}
		}

		public ushort MaxCombinedReliableMessageSize
		{
			get
			{
				return m_MaxCombinedReliableMessageSize;
			}
			set
			{
				m_MaxCombinedReliableMessageSize = value;
			}
		}

		public ushort MaxCombinedReliableMessageCount
		{
			get
			{
				return m_MaxCombinedReliableMessageCount;
			}
			set
			{
				m_MaxCombinedReliableMessageCount = value;
			}
		}

		public ushort MaxSentMessageQueueSize
		{
			get
			{
				return m_MaxSentMessageQueueSize;
			}
			set
			{
				m_MaxSentMessageQueueSize = value;
			}
		}

		public ConnectionAcksType AcksType
		{
			get
			{
				return m_AcksType;
			}
			set
			{
				m_AcksType = value;
			}
		}

		[Obsolete("IsAcksLong is deprecated. Use AcksType = ConnectionAcksType.Acks64", false)]
		public bool IsAcksLong
		{
			get
			{
				return m_AcksType != ConnectionAcksType.Acks32;
			}
			set
			{
				if (value && m_AcksType == ConnectionAcksType.Acks32)
				{
					m_AcksType = ConnectionAcksType.Acks64;
				}
				else if (!value)
				{
					m_AcksType = ConnectionAcksType.Acks32;
				}
			}
		}

		public bool UsePlatformSpecificProtocols
		{
			get
			{
				return m_UsePlatformSpecificProtocols;
			}
			set
			{
				if (value && Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.PSP2)
				{
					throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
				}
				m_UsePlatformSpecificProtocols = value;
			}
		}

		public uint InitialBandwidth
		{
			get
			{
				return m_InitialBandwidth;
			}
			set
			{
				m_InitialBandwidth = value;
			}
		}

		public float BandwidthPeakFactor
		{
			get
			{
				return m_BandwidthPeakFactor;
			}
			set
			{
				m_BandwidthPeakFactor = value;
			}
		}

		public ushort WebSocketReceiveBufferMaxSize
		{
			get
			{
				return m_WebSocketReceiveBufferMaxSize;
			}
			set
			{
				m_WebSocketReceiveBufferMaxSize = value;
			}
		}

		public uint UdpSocketReceiveBufferMaxSize
		{
			get
			{
				return m_UdpSocketReceiveBufferMaxSize;
			}
			set
			{
				m_UdpSocketReceiveBufferMaxSize = value;
			}
		}

		public string SSLCertFilePath
		{
			get
			{
				return m_SSLCertFilePath;
			}
			set
			{
				m_SSLCertFilePath = value;
			}
		}

		public string SSLPrivateKeyFilePath
		{
			get
			{
				return m_SSLPrivateKeyFilePath;
			}
			set
			{
				m_SSLPrivateKeyFilePath = value;
			}
		}

		public string SSLCAFilePath
		{
			get
			{
				return m_SSLCAFilePath;
			}
			set
			{
				m_SSLCAFilePath = value;
			}
		}

		public int ChannelCount => m_Channels.Count;

		public List<ChannelQOS> Channels => m_Channels;

		public ConnectionConfig()
		{
			m_PacketSize = 1440;
			m_FragmentSize = 500;
			m_ResendTimeout = 1200u;
			m_DisconnectTimeout = 2000u;
			m_ConnectTimeout = 2000u;
			m_MinUpdateTimeout = 10u;
			m_PingTimeout = 500u;
			m_ReducedPingTimeout = 100u;
			m_AllCostTimeout = 20u;
			m_NetworkDropThreshold = 5;
			m_OverflowDropThreshold = 5;
			m_MaxConnectionAttempt = 10;
			m_AckDelay = 33u;
			m_SendDelay = 10u;
			m_MaxCombinedReliableMessageSize = 100;
			m_MaxCombinedReliableMessageCount = 10;
			m_MaxSentMessageQueueSize = 512;
			m_AcksType = ConnectionAcksType.Acks32;
			m_UsePlatformSpecificProtocols = false;
			m_InitialBandwidth = 0u;
			m_BandwidthPeakFactor = 2f;
			m_WebSocketReceiveBufferMaxSize = 0;
			m_UdpSocketReceiveBufferMaxSize = 0u;
			m_SSLCertFilePath = null;
			m_SSLPrivateKeyFilePath = null;
			m_SSLCAFilePath = null;
		}

		public ConnectionConfig(ConnectionConfig config)
		{
			if (config == null)
			{
				throw new NullReferenceException("config is not defined");
			}
			m_PacketSize = config.m_PacketSize;
			m_FragmentSize = config.m_FragmentSize;
			m_ResendTimeout = config.m_ResendTimeout;
			m_DisconnectTimeout = config.m_DisconnectTimeout;
			m_ConnectTimeout = config.m_ConnectTimeout;
			m_MinUpdateTimeout = config.m_MinUpdateTimeout;
			m_PingTimeout = config.m_PingTimeout;
			m_ReducedPingTimeout = config.m_ReducedPingTimeout;
			m_AllCostTimeout = config.m_AllCostTimeout;
			m_NetworkDropThreshold = config.m_NetworkDropThreshold;
			m_OverflowDropThreshold = config.m_OverflowDropThreshold;
			m_MaxConnectionAttempt = config.m_MaxConnectionAttempt;
			m_AckDelay = config.m_AckDelay;
			m_SendDelay = config.m_SendDelay;
			m_MaxCombinedReliableMessageSize = config.MaxCombinedReliableMessageSize;
			m_MaxCombinedReliableMessageCount = config.m_MaxCombinedReliableMessageCount;
			m_MaxSentMessageQueueSize = config.m_MaxSentMessageQueueSize;
			m_AcksType = config.m_AcksType;
			m_UsePlatformSpecificProtocols = config.m_UsePlatformSpecificProtocols;
			m_InitialBandwidth = config.m_InitialBandwidth;
			if (m_InitialBandwidth == 0)
			{
				m_InitialBandwidth = (uint)(m_PacketSize * 1000) / m_MinUpdateTimeout;
			}
			m_BandwidthPeakFactor = config.m_BandwidthPeakFactor;
			m_WebSocketReceiveBufferMaxSize = config.m_WebSocketReceiveBufferMaxSize;
			m_UdpSocketReceiveBufferMaxSize = config.m_UdpSocketReceiveBufferMaxSize;
			m_SSLCertFilePath = config.m_SSLCertFilePath;
			m_SSLPrivateKeyFilePath = config.m_SSLPrivateKeyFilePath;
			m_SSLCAFilePath = config.m_SSLCAFilePath;
			foreach (ChannelQOS channel in config.m_Channels)
			{
				m_Channels.Add(new ChannelQOS(channel));
			}
		}

		public static void Validate(ConnectionConfig config)
		{
			if (config.m_PacketSize < 128)
			{
				throw new ArgumentOutOfRangeException("PacketSize should be > " + 128.ToString());
			}
			if (config.m_FragmentSize >= config.m_PacketSize - 128)
			{
				throw new ArgumentOutOfRangeException("FragmentSize should be < PacketSize - " + 128.ToString());
			}
			if (config.m_Channels.Count > 255)
			{
				throw new ArgumentOutOfRangeException("Channels number should be less than 256");
			}
		}

		public byte AddChannel(QosType value)
		{
			if (m_Channels.Count > 255)
			{
				throw new ArgumentOutOfRangeException("Channels Count should be less than 256");
			}
			if (!Enum.IsDefined(typeof(QosType), value))
			{
				throw new ArgumentOutOfRangeException("requested qos type doesn't exist: " + (int)value);
			}
			ChannelQOS item = new ChannelQOS(value);
			m_Channels.Add(item);
			return (byte)(m_Channels.Count - 1);
		}

		public QosType GetChannel(byte idx)
		{
			if (idx >= m_Channels.Count)
			{
				throw new ArgumentOutOfRangeException("requested index greater than maximum channels count");
			}
			return m_Channels[idx].QOS;
		}
	}
}
