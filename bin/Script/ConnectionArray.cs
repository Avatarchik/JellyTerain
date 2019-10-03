using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class ConnectionArray
	{
		private List<NetworkConnection> m_LocalConnections;

		private List<NetworkConnection> m_Connections;

		internal List<NetworkConnection> localConnections => m_LocalConnections;

		internal List<NetworkConnection> connections => m_Connections;

		public int Count => m_Connections.Count;

		public int LocalIndex => -m_LocalConnections.Count;

		public ConnectionArray()
		{
			m_Connections = new List<NetworkConnection>();
			m_LocalConnections = new List<NetworkConnection>();
		}

		public int Add(int connId, NetworkConnection conn)
		{
			if (connId < 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Add bad id " + connId);
				}
				return -1;
			}
			if (connId < m_Connections.Count && m_Connections[connId] != null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Add dupe at " + connId);
				}
				return -1;
			}
			while (connId > m_Connections.Count - 1)
			{
				m_Connections.Add(null);
			}
			m_Connections[connId] = conn;
			return connId;
		}

		public NetworkConnection Get(int connId)
		{
			if (connId < 0)
			{
				return m_LocalConnections[Mathf.Abs(connId) - 1];
			}
			if (connId < 0 || connId > m_Connections.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Get invalid index " + connId);
				}
				return null;
			}
			return m_Connections[connId];
		}

		public NetworkConnection GetUnsafe(int connId)
		{
			if (connId < 0 || connId > m_Connections.Count)
			{
				return null;
			}
			return m_Connections[connId];
		}

		public void Remove(int connId)
		{
			if (connId < 0)
			{
				m_LocalConnections[Mathf.Abs(connId) - 1] = null;
			}
			else if (connId < 0 || connId > m_Connections.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Remove invalid index " + connId);
				}
			}
			else
			{
				m_Connections[connId] = null;
			}
		}

		public int AddLocal(NetworkConnection conn)
		{
			m_LocalConnections.Add(conn);
			return conn.connectionId = -m_LocalConnections.Count;
		}

		public bool ContainsPlayer(GameObject player, out NetworkConnection conn)
		{
			conn = null;
			if (player == null)
			{
				return false;
			}
			for (int i = LocalIndex; i < m_Connections.Count; i++)
			{
				conn = Get(i);
				if (conn == null)
				{
					continue;
				}
				for (int j = 0; j < conn.playerControllers.Count; j++)
				{
					if (conn.playerControllers[j].IsValid && conn.playerControllers[j].gameObject == player)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
