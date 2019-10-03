using System;

namespace UnityEngine.Networking
{
	[Serializable]
	public class GlobalConfig
	{
		private const uint g_MaxTimerTimeout = 12000u;

		private const uint g_MaxNetSimulatorTimeout = 12000u;

		private const ushort g_MaxHosts = 128;

		[SerializeField]
		private uint m_ThreadAwakeTimeout;

		[SerializeField]
		private ReactorModel m_ReactorModel;

		[SerializeField]
		private ushort m_ReactorMaximumReceivedMessages;

		[SerializeField]
		private ushort m_ReactorMaximumSentMessages;

		[SerializeField]
		private ushort m_MaxPacketSize;

		[SerializeField]
		private ushort m_MaxHosts;

		[SerializeField]
		private byte m_ThreadPoolSize;

		[SerializeField]
		private uint m_MinTimerTimeout;

		[SerializeField]
		private uint m_MaxTimerTimeout;

		[SerializeField]
		private uint m_MinNetSimulatorTimeout;

		[SerializeField]
		private uint m_MaxNetSimulatorTimeout;

		public uint ThreadAwakeTimeout
		{
			get
			{
				return m_ThreadAwakeTimeout;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("Minimal thread awake timeout should be > 0");
				}
				m_ThreadAwakeTimeout = value;
			}
		}

		public ReactorModel ReactorModel
		{
			get
			{
				return m_ReactorModel;
			}
			set
			{
				m_ReactorModel = value;
			}
		}

		public ushort ReactorMaximumReceivedMessages
		{
			get
			{
				return m_ReactorMaximumReceivedMessages;
			}
			set
			{
				m_ReactorMaximumReceivedMessages = value;
			}
		}

		public ushort ReactorMaximumSentMessages
		{
			get
			{
				return m_ReactorMaximumSentMessages;
			}
			set
			{
				m_ReactorMaximumSentMessages = value;
			}
		}

		public ushort MaxPacketSize
		{
			get
			{
				return m_MaxPacketSize;
			}
			set
			{
				m_MaxPacketSize = value;
			}
		}

		public ushort MaxHosts
		{
			get
			{
				return m_MaxHosts;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("MaxHosts", "Maximum hosts number should be > 0");
				}
				if (value > 128)
				{
					throw new ArgumentOutOfRangeException("MaxHosts", "Maximum hosts number should be <= " + ((ushort)128).ToString());
				}
				m_MaxHosts = value;
			}
		}

		public byte ThreadPoolSize
		{
			get
			{
				return m_ThreadPoolSize;
			}
			set
			{
				m_ThreadPoolSize = value;
			}
		}

		public uint MinTimerTimeout
		{
			get
			{
				return m_MinTimerTimeout;
			}
			set
			{
				if (value > MaxTimerTimeout)
				{
					throw new ArgumentOutOfRangeException("MinTimerTimeout should be < MaxTimerTimeout");
				}
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("MinTimerTimeout should be > 0");
				}
				m_MinTimerTimeout = value;
			}
		}

		public uint MaxTimerTimeout
		{
			get
			{
				return m_MaxTimerTimeout;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("MaxTimerTimeout should be > 0");
				}
				if (value > 12000)
				{
					throw new ArgumentOutOfRangeException("MaxTimerTimeout should be <=" + 12000u.ToString());
				}
				m_MaxTimerTimeout = value;
			}
		}

		public uint MinNetSimulatorTimeout
		{
			get
			{
				return m_MinNetSimulatorTimeout;
			}
			set
			{
				if (value > MaxNetSimulatorTimeout)
				{
					throw new ArgumentOutOfRangeException("MinNetSimulatorTimeout should be < MaxTimerTimeout");
				}
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("MinNetSimulatorTimeout should be > 0");
				}
				m_MinNetSimulatorTimeout = value;
			}
		}

		public uint MaxNetSimulatorTimeout
		{
			get
			{
				return m_MaxNetSimulatorTimeout;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("MaxNetSimulatorTimeout should be > 0");
				}
				if (value > 12000)
				{
					throw new ArgumentOutOfRangeException("MaxNetSimulatorTimeout should be <=" + 12000u.ToString());
				}
				m_MaxNetSimulatorTimeout = value;
			}
		}

		public GlobalConfig()
		{
			m_ThreadAwakeTimeout = 1u;
			m_ReactorModel = ReactorModel.SelectReactor;
			m_ReactorMaximumReceivedMessages = 1024;
			m_ReactorMaximumSentMessages = 1024;
			m_MaxPacketSize = 2000;
			m_MaxHosts = 16;
			m_ThreadPoolSize = 1;
			m_MinTimerTimeout = 1u;
			m_MaxTimerTimeout = 12000u;
			m_MinNetSimulatorTimeout = 1u;
			m_MaxNetSimulatorTimeout = 12000u;
		}
	}
}
