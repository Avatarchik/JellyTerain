using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[Serializable]
	public class HostTopology
	{
		[SerializeField]
		private ConnectionConfig m_DefConfig = null;

		[SerializeField]
		private int m_MaxDefConnections = 0;

		[SerializeField]
		private List<ConnectionConfig> m_SpecialConnections = new List<ConnectionConfig>();

		[SerializeField]
		private ushort m_ReceivedMessagePoolSize = 1024;

		[SerializeField]
		private ushort m_SentMessagePoolSize = 1024;

		[SerializeField]
		private float m_MessagePoolSizeGrowthFactor = 0.75f;

		public ConnectionConfig DefaultConfig => m_DefConfig;

		public int MaxDefaultConnections => m_MaxDefConnections;

		public int SpecialConnectionConfigsCount => m_SpecialConnections.Count;

		public List<ConnectionConfig> SpecialConnectionConfigs => m_SpecialConnections;

		public ushort ReceivedMessagePoolSize
		{
			get
			{
				return m_ReceivedMessagePoolSize;
			}
			set
			{
				m_ReceivedMessagePoolSize = value;
			}
		}

		public ushort SentMessagePoolSize
		{
			get
			{
				return m_SentMessagePoolSize;
			}
			set
			{
				m_SentMessagePoolSize = value;
			}
		}

		public float MessagePoolSizeGrowthFactor
		{
			get
			{
				return m_MessagePoolSizeGrowthFactor;
			}
			set
			{
				if ((double)value <= 0.5 || (double)value > 1.0)
				{
					throw new ArgumentException("pool growth factor should be varied between 0.5 and 1.0");
				}
				m_MessagePoolSizeGrowthFactor = value;
			}
		}

		public HostTopology(ConnectionConfig defaultConfig, int maxDefaultConnections)
		{
			if (defaultConfig == null)
			{
				throw new NullReferenceException("config is not defined");
			}
			if (maxDefaultConnections <= 0)
			{
				throw new ArgumentOutOfRangeException("maxDefaultConnections", "Number of connections should be > 0");
			}
			if (maxDefaultConnections >= 65535)
			{
				throw new ArgumentOutOfRangeException("maxDefaultConnections", "Number of connections should be < 65535");
			}
			ConnectionConfig.Validate(defaultConfig);
			m_DefConfig = new ConnectionConfig(defaultConfig);
			m_MaxDefConnections = maxDefaultConnections;
		}

		private HostTopology()
		{
		}

		public ConnectionConfig GetSpecialConnectionConfig(int i)
		{
			if (i > m_SpecialConnections.Count || i == 0)
			{
				throw new ArgumentException("special configuration index is out of valid range");
			}
			return m_SpecialConnections[i - 1];
		}

		public int AddSpecialConnectionConfig(ConnectionConfig config)
		{
			m_SpecialConnections.Add(new ConnectionConfig(config));
			return m_SpecialConnections.Count;
		}
	}
}
