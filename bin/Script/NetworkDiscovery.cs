using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkDiscovery")]
	public class NetworkDiscovery : MonoBehaviour
	{
		private const int k_MaxBroadcastMsgSize = 1024;

		[SerializeField]
		private int m_BroadcastPort = 47777;

		[SerializeField]
		private int m_BroadcastKey = 2222;

		[SerializeField]
		private int m_BroadcastVersion = 1;

		[SerializeField]
		private int m_BroadcastSubVersion = 1;

		[SerializeField]
		private int m_BroadcastInterval = 1000;

		[SerializeField]
		private bool m_UseNetworkManager = false;

		[SerializeField]
		private string m_BroadcastData = "HELLO";

		[SerializeField]
		private bool m_ShowGUI = true;

		[SerializeField]
		private int m_OffsetX;

		[SerializeField]
		private int m_OffsetY;

		private int m_HostId = -1;

		private bool m_Running;

		private bool m_IsServer;

		private bool m_IsClient;

		private byte[] m_MsgOutBuffer;

		private byte[] m_MsgInBuffer;

		private HostTopology m_DefaultTopology;

		private Dictionary<string, NetworkBroadcastResult> m_BroadcastsReceived;

		public int broadcastPort
		{
			get
			{
				return m_BroadcastPort;
			}
			set
			{
				m_BroadcastPort = value;
			}
		}

		public int broadcastKey
		{
			get
			{
				return m_BroadcastKey;
			}
			set
			{
				m_BroadcastKey = value;
			}
		}

		public int broadcastVersion
		{
			get
			{
				return m_BroadcastVersion;
			}
			set
			{
				m_BroadcastVersion = value;
			}
		}

		public int broadcastSubVersion
		{
			get
			{
				return m_BroadcastSubVersion;
			}
			set
			{
				m_BroadcastSubVersion = value;
			}
		}

		public int broadcastInterval
		{
			get
			{
				return m_BroadcastInterval;
			}
			set
			{
				m_BroadcastInterval = value;
			}
		}

		public bool useNetworkManager
		{
			get
			{
				return m_UseNetworkManager;
			}
			set
			{
				m_UseNetworkManager = value;
			}
		}

		public string broadcastData
		{
			get
			{
				return m_BroadcastData;
			}
			set
			{
				m_BroadcastData = value;
				m_MsgOutBuffer = StringToBytes(m_BroadcastData);
				if (m_UseNetworkManager && LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkDiscovery broadcast data changed while using NetworkManager. This can prevent clients from finding the server. The format of the broadcast data must be 'NetworkManager:IPAddress:Port'.");
				}
			}
		}

		public bool showGUI
		{
			get
			{
				return m_ShowGUI;
			}
			set
			{
				m_ShowGUI = value;
			}
		}

		public int offsetX
		{
			get
			{
				return m_OffsetX;
			}
			set
			{
				m_OffsetX = value;
			}
		}

		public int offsetY
		{
			get
			{
				return m_OffsetY;
			}
			set
			{
				m_OffsetY = value;
			}
		}

		public int hostId
		{
			get
			{
				return m_HostId;
			}
			set
			{
				m_HostId = value;
			}
		}

		public bool running
		{
			get
			{
				return m_Running;
			}
			set
			{
				m_Running = value;
			}
		}

		public bool isServer
		{
			get
			{
				return m_IsServer;
			}
			set
			{
				m_IsServer = value;
			}
		}

		public bool isClient
		{
			get
			{
				return m_IsClient;
			}
			set
			{
				m_IsClient = value;
			}
		}

		public Dictionary<string, NetworkBroadcastResult> broadcastsReceived => m_BroadcastsReceived;

		private static byte[] StringToBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		private static string BytesToString(byte[] bytes)
		{
			char[] array = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			return new string(array);
		}

		public bool Initialize()
		{
			if (m_BroadcastData.Length >= 1024)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + 1024);
				}
				return false;
			}
			if (!NetworkTransport.IsStarted)
			{
				NetworkTransport.Init();
			}
			if (m_UseNetworkManager && NetworkManager.singleton != null)
			{
				m_BroadcastData = "NetworkManager:" + NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
				if (LogFilter.logInfo)
				{
					Debug.Log("NetworkDiscovery set broadcast data to:" + m_BroadcastData);
				}
			}
			m_MsgOutBuffer = StringToBytes(m_BroadcastData);
			m_MsgInBuffer = new byte[1024];
			m_BroadcastsReceived = new Dictionary<string, NetworkBroadcastResult>();
			ConnectionConfig connectionConfig = new ConnectionConfig();
			connectionConfig.AddChannel(QosType.Unreliable);
			m_DefaultTopology = new HostTopology(connectionConfig, 1);
			if (m_IsServer)
			{
				StartAsServer();
			}
			if (m_IsClient)
			{
				StartAsClient();
			}
			return true;
		}

		public bool StartAsClient()
		{
			if (m_HostId != -1 || m_Running)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkDiscovery StartAsClient already started");
				}
				return false;
			}
			if (m_MsgInBuffer == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery StartAsClient, NetworkDiscovery is not initialized");
				}
				return false;
			}
			m_HostId = NetworkTransport.AddHost(m_DefaultTopology, m_BroadcastPort);
			if (m_HostId == -1)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery StartAsClient - addHost failed");
				}
				return false;
			}
			NetworkTransport.SetBroadcastCredentials(m_HostId, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, out byte _);
			m_Running = true;
			m_IsClient = true;
			if (LogFilter.logDebug)
			{
				Debug.Log("StartAsClient Discovery listening");
			}
			return true;
		}

		public bool StartAsServer()
		{
			if (m_HostId != -1 || m_Running)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkDiscovery StartAsServer already started");
				}
				return false;
			}
			m_HostId = NetworkTransport.AddHost(m_DefaultTopology, 0);
			if (m_HostId == -1)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery StartAsServer - addHost failed");
				}
				return false;
			}
			if (!NetworkTransport.StartBroadcastDiscovery(m_HostId, m_BroadcastPort, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, m_MsgOutBuffer, m_MsgOutBuffer.Length, m_BroadcastInterval, out byte error))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + error);
				}
				return false;
			}
			m_Running = true;
			m_IsServer = true;
			if (LogFilter.logDebug)
			{
				Debug.Log("StartAsServer Discovery broadcasting");
			}
			Object.DontDestroyOnLoad(base.gameObject);
			return true;
		}

		public void StopBroadcast()
		{
			if (m_HostId == -1)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkDiscovery StopBroadcast not initialized");
				}
				return;
			}
			if (!m_Running)
			{
				Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
				return;
			}
			if (m_IsServer)
			{
				NetworkTransport.StopBroadcastDiscovery();
			}
			NetworkTransport.RemoveHost(m_HostId);
			m_HostId = -1;
			m_Running = false;
			m_IsServer = false;
			m_IsClient = false;
			m_MsgInBuffer = null;
			m_BroadcastsReceived = null;
			if (LogFilter.logDebug)
			{
				Debug.Log("Stopped Discovery broadcasting");
			}
		}

		private void Update()
		{
			if (m_HostId == -1 || m_IsServer)
			{
				return;
			}
			NetworkEventType networkEventType;
			do
			{
				networkEventType = NetworkTransport.ReceiveFromHost(m_HostId, out int _, out int _, m_MsgInBuffer, 1024, out int receivedSize, out byte error);
				if (networkEventType == NetworkEventType.BroadcastEvent)
				{
					NetworkTransport.GetBroadcastConnectionMessage(m_HostId, m_MsgInBuffer, 1024, out receivedSize, out error);
					NetworkTransport.GetBroadcastConnectionInfo(m_HostId, out string address, out int _, out error);
					NetworkBroadcastResult value = default(NetworkBroadcastResult);
					value.serverAddress = address;
					value.broadcastData = new byte[receivedSize];
					Buffer.BlockCopy(m_MsgInBuffer, 0, value.broadcastData, 0, receivedSize);
					m_BroadcastsReceived[address] = value;
					OnReceivedBroadcast(address, BytesToString(m_MsgInBuffer));
				}
			}
			while (networkEventType != NetworkEventType.Nothing);
		}

		private void OnDestroy()
		{
			if (m_IsServer && m_Running && m_HostId != -1)
			{
				NetworkTransport.StopBroadcastDiscovery();
				NetworkTransport.RemoveHost(m_HostId);
			}
			if (m_IsClient && m_Running && m_HostId != -1)
			{
				NetworkTransport.RemoveHost(m_HostId);
			}
		}

		public virtual void OnReceivedBroadcast(string fromAddress, string data)
		{
		}

		private void OnGUI()
		{
			if (!m_ShowGUI)
			{
				return;
			}
			int num = 10 + m_OffsetX;
			int num2 = 40 + m_OffsetY;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				GUI.Box(new Rect(num, num2, 200f, 20f), "( WebGL cannot broadcast )");
				return;
			}
			if (m_MsgInBuffer == null)
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "Initialize Broadcast"))
				{
					Initialize();
				}
				return;
			}
			string str = "";
			if (m_IsServer)
			{
				str = " (server)";
			}
			if (m_IsClient)
			{
				str = " (client)";
			}
			GUI.Label(new Rect(num, num2, 200f, 20f), "initialized" + str);
			num2 += 24;
			if (m_Running)
			{
				if (GUI.Button(new Rect(num, num2, 200f, 20f), "Stop"))
				{
					StopBroadcast();
				}
				num2 += 24;
				if (m_BroadcastsReceived != null)
				{
					foreach (string key in m_BroadcastsReceived.Keys)
					{
						NetworkBroadcastResult networkBroadcastResult = m_BroadcastsReceived[key];
						if (GUI.Button(new Rect(num, num2 + 20, 200f, 20f), "Game at " + key) && m_UseNetworkManager)
						{
							string text = BytesToString(networkBroadcastResult.broadcastData);
							string[] array = text.Split(':');
							if (array.Length == 3 && array[0] == "NetworkManager" && NetworkManager.singleton != null && NetworkManager.singleton.client == null)
							{
								NetworkManager.singleton.networkAddress = array[1];
								NetworkManager.singleton.networkPort = Convert.ToInt32(array[2]);
								NetworkManager.singleton.StartClient();
							}
						}
						num2 += 24;
					}
				}
				return;
			}
			if (GUI.Button(new Rect(num, num2, 200f, 20f), "Start Broadcasting"))
			{
				StartAsServer();
			}
			num2 += 24;
			if (GUI.Button(new Rect(num, num2, 200f, 20f), "Listen for Broadcast"))
			{
				StartAsClient();
			}
			num2 += 24;
		}
	}
}
