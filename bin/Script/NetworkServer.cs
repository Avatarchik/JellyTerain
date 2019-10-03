using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	public sealed class NetworkServer
	{
		private class ServerSimpleWrapper : NetworkServerSimple
		{
			private NetworkServer m_Server;

			public ServerSimpleWrapper(NetworkServer server)
			{
				m_Server = server;
			}

			public override void OnConnectError(int connectionId, byte error)
			{
				m_Server.GenerateConnectError(error);
			}

			public override void OnDataError(NetworkConnection conn, byte error)
			{
				m_Server.GenerateDataError(conn, error);
			}

			public override void OnDisconnectError(NetworkConnection conn, byte error)
			{
				m_Server.GenerateDisconnectError(conn, error);
			}

			public override void OnConnected(NetworkConnection conn)
			{
				m_Server.OnConnected(conn);
			}

			public override void OnDisconnected(NetworkConnection conn)
			{
				m_Server.OnDisconnected(conn);
			}

			public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
			{
				m_Server.OnData(conn, receivedSize, channelId);
			}
		}

		private static bool s_Active;

		private static volatile NetworkServer s_Instance;

		private static object s_Sync = new Object();

		private static bool m_DontListen;

		private bool m_LocalClientActive;

		private List<NetworkConnection> m_LocalConnectionsFakeList = new List<NetworkConnection>();

		private ULocalConnectionToClient m_LocalConnection = null;

		private NetworkScene m_NetworkScene;

		private HashSet<int> m_ExternalConnections;

		private ServerSimpleWrapper m_SimpleServerSimple;

		private float m_MaxDelay = 0.1f;

		private HashSet<NetworkInstanceId> m_RemoveList;

		private int m_RemoveListCount;

		private const int k_RemoveListInterval = 100;

		internal static ushort maxPacketSize;

		private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache4;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache5;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache6;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache7;

		[CompilerGenerated]
		private static NetworkMessageDelegate _003C_003Ef__mg_0024cache8;

		public static List<NetworkConnection> localConnections => instance.m_LocalConnectionsFakeList;

		public static int listenPort => instance.m_SimpleServerSimple.listenPort;

		public static int serverHostId => instance.m_SimpleServerSimple.serverHostId;

		public static ReadOnlyCollection<NetworkConnection> connections => instance.m_SimpleServerSimple.connections;

		public static Dictionary<short, NetworkMessageDelegate> handlers => instance.m_SimpleServerSimple.handlers;

		public static HostTopology hostTopology => instance.m_SimpleServerSimple.hostTopology;

		public static Dictionary<NetworkInstanceId, NetworkIdentity> objects => instance.m_NetworkScene.localObjects;

		[Obsolete("Moved to NetworkMigrationManager")]
		public static bool sendPeerInfo
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public static bool dontListen
		{
			get
			{
				return m_DontListen;
			}
			set
			{
				m_DontListen = value;
			}
		}

		public static bool useWebSockets
		{
			get
			{
				return instance.m_SimpleServerSimple.useWebSockets;
			}
			set
			{
				instance.m_SimpleServerSimple.useWebSockets = value;
			}
		}

		internal static NetworkServer instance
		{
			get
			{
				if (s_Instance == null)
				{
					lock (s_Sync)
					{
						if (s_Instance == null)
						{
							s_Instance = new NetworkServer();
						}
					}
				}
				return s_Instance;
			}
		}

		public static bool active => s_Active;

		public static bool localClientActive => instance.m_LocalClientActive;

		public static int numChannels => instance.m_SimpleServerSimple.hostTopology.DefaultConfig.ChannelCount;

		public static float maxDelay
		{
			get
			{
				return instance.m_MaxDelay;
			}
			set
			{
				instance.InternalSetMaxDelay(value);
			}
		}

		public static Type networkConnectionClass => instance.m_SimpleServerSimple.networkConnectionClass;

		private NetworkServer()
		{
			NetworkTransport.Init();
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer Created version " + Version.Current);
			}
			m_RemoveList = new HashSet<NetworkInstanceId>();
			m_ExternalConnections = new HashSet<int>();
			m_NetworkScene = new NetworkScene();
			m_SimpleServerSimple = new ServerSimpleWrapper(this);
		}

		public static void SetNetworkConnectionClass<T>() where T : NetworkConnection
		{
			instance.m_SimpleServerSimple.SetNetworkConnectionClass<T>();
		}

		public static bool Configure(ConnectionConfig config, int maxConnections)
		{
			return instance.m_SimpleServerSimple.Configure(config, maxConnections);
		}

		public static bool Configure(HostTopology topology)
		{
			return instance.m_SimpleServerSimple.Configure(topology);
		}

		public static void Reset()
		{
			NetworkTransport.Shutdown();
			NetworkTransport.Init();
			s_Instance = null;
			s_Active = false;
		}

		public static void Shutdown()
		{
			if (s_Instance != null)
			{
				s_Instance.InternalDisconnectAll();
				if (!m_DontListen)
				{
					s_Instance.m_SimpleServerSimple.Stop();
				}
				s_Instance = null;
			}
			m_DontListen = false;
			s_Active = false;
		}

		public static bool Listen(MatchInfo matchInfo, int listenPort)
		{
			if (!matchInfo.usingRelay)
			{
				return instance.InternalListen(null, listenPort);
			}
			instance.InternalListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
			return true;
		}

		internal void RegisterMessageHandlers()
		{
			m_SimpleServerSimple.RegisterHandlerSafe(35, OnClientReadyMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(5, OnCommandMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(6, NetworkTransform.HandleTransform);
			m_SimpleServerSimple.RegisterHandlerSafe(16, NetworkTransformChild.HandleChildTransform);
			m_SimpleServerSimple.RegisterHandlerSafe(38, OnRemovePlayerMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(40, NetworkAnimator.OnAnimationServerMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(41, NetworkAnimator.OnAnimationParametersServerMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(42, NetworkAnimator.OnAnimationTriggerServerMessage);
			m_SimpleServerSimple.RegisterHandlerSafe(17, NetworkConnection.OnFragment);
			maxPacketSize = hostTopology.DefaultConfig.PacketSize;
		}

		public static void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
		{
			instance.InternalListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
		}

		private void InternalListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
		{
			m_SimpleServerSimple.ListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
			s_Active = true;
			RegisterMessageHandlers();
		}

		public static bool Listen(int serverPort)
		{
			return instance.InternalListen(null, serverPort);
		}

		public static bool Listen(string ipAddress, int serverPort)
		{
			return instance.InternalListen(ipAddress, serverPort);
		}

		internal bool InternalListen(string ipAddress, int serverPort)
		{
			if (m_DontListen)
			{
				m_SimpleServerSimple.Initialize();
			}
			else if (!m_SimpleServerSimple.Listen(ipAddress, serverPort))
			{
				return false;
			}
			maxPacketSize = hostTopology.DefaultConfig.PacketSize;
			s_Active = true;
			RegisterMessageHandlers();
			return true;
		}

		public static NetworkClient BecomeHost(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
		{
			return instance.BecomeHostInternal(oldClient, port, matchInfo, oldConnectionId, peers);
		}

		internal NetworkClient BecomeHostInternal(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
		{
			if (s_Active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("BecomeHost already a server.");
				}
				return null;
			}
			if (!NetworkClient.active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("BecomeHost NetworkClient not active.");
				}
				return null;
			}
			Configure(hostTopology);
			if (matchInfo == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("BecomeHost Listen on " + port);
				}
				if (!Listen(port))
				{
					if (LogFilter.logError)
					{
						Debug.LogError("BecomeHost bind failed.");
					}
					return null;
				}
			}
			else
			{
				if (LogFilter.logDev)
				{
					Debug.Log("BecomeHost match:" + matchInfo.networkId);
				}
				ListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
			}
			foreach (NetworkIdentity value in ClientScene.objects.Values)
			{
				if (!(value == null) && !(value.gameObject == null))
				{
					NetworkIdentity.AddNetworkId(value.netId.Value);
					m_NetworkScene.SetLocalObject(value.netId, value.gameObject, isClient: false, isServer: false);
					value.OnStartServer(allowNonZeroNetId: true);
				}
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer BecomeHost done. oldConnectionId:" + oldConnectionId);
			}
			RegisterMessageHandlers();
			if (!NetworkClient.RemoveClient(oldClient) && LogFilter.logError)
			{
				Debug.LogError("BecomeHost failed to remove client");
			}
			if (LogFilter.logDev)
			{
				Debug.Log("BecomeHost localClient ready");
			}
			NetworkClient networkClient = ClientScene.ReconnectLocalServer();
			ClientScene.Ready(networkClient.connection);
			ClientScene.SetReconnectId(oldConnectionId, peers);
			ClientScene.AddPlayer(ClientScene.readyConnection, 0);
			return networkClient;
		}

		private void InternalSetMaxDelay(float seconds)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				connections[i]?.SetMaxDelay(seconds);
			}
			m_MaxDelay = seconds;
		}

		internal int AddLocalClient(LocalClient localClient)
		{
			if (m_LocalConnectionsFakeList.Count != 0)
			{
				Debug.LogError("Local Connection already exists");
				return -1;
			}
			m_LocalConnection = new ULocalConnectionToClient(localClient);
			m_LocalConnection.connectionId = 0;
			m_SimpleServerSimple.SetConnectionAtIndex(m_LocalConnection);
			m_LocalConnectionsFakeList.Add(m_LocalConnection);
			m_LocalConnection.InvokeHandlerNoData(32);
			return 0;
		}

		internal void RemoveLocalClient(NetworkConnection localClientConnection)
		{
			for (int i = 0; i < m_LocalConnectionsFakeList.Count; i++)
			{
				if (m_LocalConnectionsFakeList[i].connectionId == localClientConnection.connectionId)
				{
					m_LocalConnectionsFakeList.RemoveAt(i);
					break;
				}
			}
			if (m_LocalConnection != null)
			{
				m_LocalConnection.Disconnect();
				m_LocalConnection.Dispose();
				m_LocalConnection = null;
			}
			m_LocalClientActive = false;
			m_SimpleServerSimple.RemoveConnectionAtIndex(0);
		}

		internal void SetLocalObjectOnServer(NetworkInstanceId netId, GameObject obj)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("SetLocalObjectOnServer " + netId + " " + obj);
			}
			m_NetworkScene.SetLocalObject(netId, obj, isClient: false, isServer: true);
		}

		internal void ActivateLocalClientScene()
		{
			if (!m_LocalClientActive)
			{
				m_LocalClientActive = true;
				foreach (NetworkIdentity value in objects.Values)
				{
					if (!value.isClient)
					{
						if (LogFilter.logDev)
						{
							Debug.Log("ActivateClientScene " + value.netId + " " + value.gameObject);
						}
						ClientScene.SetLocalObject(value.netId, value.gameObject);
						value.OnStartClient();
					}
				}
			}
		}

		public static bool SendToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToAll msgType:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.Send(msgType, msg);
				}
			}
			return flag;
		}

		private static bool SendToObservers(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToObservers id:" + msgType);
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkConnection networkConnection = component.observers[i];
				flag &= networkConnection.Send(msgType, msg);
			}
			return flag;
		}

		public static bool SendToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < connections.Count; i++)
				{
					NetworkConnection networkConnection = connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.Send(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.Send(msgType, msg);
				}
			}
			return flag;
		}

		public static void SendWriterToReady(GameObject contextObj, NetworkWriter writer, int channelId)
		{
			if (writer.AsArraySegment().Count > 32767)
			{
				throw new UnityException("NetworkWriter used buffer is too big!");
			}
			SendBytesToReady(contextObj, writer.AsArraySegment().Array, writer.AsArraySegment().Count, channelId);
		}

		public static void SendBytesToReady(GameObject contextObj, byte[] buffer, int numBytes, int channelId)
		{
			if (contextObj == null)
			{
				bool flag = true;
				for (int i = 0; i < connections.Count; i++)
				{
					NetworkConnection networkConnection = connections[i];
					if (networkConnection != null && networkConnection.isReady && !networkConnection.SendBytes(buffer, numBytes, channelId))
					{
						flag = false;
					}
				}
				if (!flag && LogFilter.logWarn)
				{
					Debug.LogWarning("SendBytesToReady failed");
				}
			}
			else
			{
				NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
				try
				{
					bool flag2 = true;
					int count = component.observers.Count;
					for (int j = 0; j < count; j++)
					{
						NetworkConnection networkConnection2 = component.observers[j];
						if (networkConnection2.isReady && !networkConnection2.SendBytes(buffer, numBytes, channelId))
						{
							flag2 = false;
						}
					}
					if (!flag2 && LogFilter.logWarn)
					{
						Debug.LogWarning("SendBytesToReady failed for " + contextObj);
					}
				}
				catch (NullReferenceException)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("SendBytesToReady object " + contextObj + " has not been spawned");
					}
				}
			}
		}

		public static void SendBytesToPlayer(GameObject player, byte[] buffer, int numBytes, int channelId)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection == null)
				{
					continue;
				}
				for (int j = 0; j < networkConnection.playerControllers.Count; j++)
				{
					if (networkConnection.playerControllers[j].IsValid && networkConnection.playerControllers[j].gameObject == player)
					{
						networkConnection.SendBytes(buffer, numBytes, channelId);
						break;
					}
				}
			}
		}

		public static bool SendUnreliableToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToAll msgType:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendUnreliableToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < connections.Count; i++)
				{
					NetworkConnection networkConnection = connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendUnreliable(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendByChannelToAll(short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToAll id:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static bool SendByChannelToReady(GameObject contextObj, short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToReady msgType:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < connections.Count; i++)
				{
					NetworkConnection networkConnection = connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendByChannel(msgType, msg, channelId);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static void DisconnectAll()
		{
			instance.InternalDisconnectAll();
		}

		internal void InternalDisconnectAll()
		{
			m_SimpleServerSimple.DisconnectAllConnections();
			if (m_LocalConnection != null)
			{
				m_LocalConnection.Disconnect();
				m_LocalConnection.Dispose();
				m_LocalConnection = null;
			}
			s_Active = false;
			m_LocalClientActive = false;
		}

		internal static void Update()
		{
			if (s_Instance != null)
			{
				s_Instance.InternalUpdate();
			}
		}

		private void UpdateServerObjects()
		{
			foreach (NetworkIdentity value in objects.Values)
			{
				try
				{
					value.UNetUpdate();
				}
				catch (NullReferenceException)
				{
				}
				catch (MissingReferenceException)
				{
				}
			}
			if (m_RemoveListCount++ % 100 == 0)
			{
				CheckForNullObjects();
			}
		}

		private void CheckForNullObjects()
		{
			foreach (NetworkInstanceId key in objects.Keys)
			{
				NetworkIdentity networkIdentity = objects[key];
				if (networkIdentity == null || networkIdentity.gameObject == null)
				{
					m_RemoveList.Add(key);
				}
			}
			if (m_RemoveList.Count > 0)
			{
				foreach (NetworkInstanceId remove in m_RemoveList)
				{
					objects.Remove(remove);
				}
				m_RemoveList.Clear();
			}
		}

		internal void InternalUpdate()
		{
			m_SimpleServerSimple.Update();
			if (m_DontListen)
			{
				m_SimpleServerSimple.UpdateConnections();
			}
			UpdateServerObjects();
		}

		private void OnConnected(NetworkConnection conn)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Server accepted client:" + conn.connectionId);
			}
			conn.SetMaxDelay(m_MaxDelay);
			conn.InvokeHandlerNoData(32);
			SendCrc(conn);
		}

		private void OnDisconnected(NetworkConnection conn)
		{
			conn.InvokeHandlerNoData(33);
			for (int i = 0; i < conn.playerControllers.Count; i++)
			{
				if (conn.playerControllers[i].gameObject != null && LogFilter.logWarn)
				{
					Debug.LogWarning("Player not destroyed when connection disconnected.");
				}
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Server lost client:" + conn.connectionId);
			}
			conn.RemoveObservers();
			conn.Dispose();
		}

		private void OnData(NetworkConnection conn, int receivedSize, int channelId)
		{
			conn.TransportReceive(m_SimpleServerSimple.messageBuffer, receivedSize, channelId);
		}

		private void GenerateConnectError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Connect Error: " + error);
			}
			GenerateError(null, error);
		}

		private void GenerateDataError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Data Error: " + (NetworkError)error);
			}
			GenerateError(conn, error);
		}

		private void GenerateDisconnectError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Disconnect Error: " + (NetworkError)error + " conn:[" + conn + "]:" + conn.connectionId);
			}
			GenerateError(conn, error);
		}

		private void GenerateError(NetworkConnection conn, int error)
		{
			if (handlers.ContainsKey(34))
			{
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.errorCode = error;
				NetworkWriter writer = new NetworkWriter();
				errorMessage.Serialize(writer);
				NetworkReader reader = new NetworkReader(writer);
				conn.InvokeHandler(34, reader, 0);
			}
		}

		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			instance.m_SimpleServerSimple.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			instance.m_SimpleServerSimple.UnregisterHandler(msgType);
		}

		public static void ClearHandlers()
		{
			instance.m_SimpleServerSimple.ClearHandlers();
		}

		public static void ClearSpawners()
		{
			NetworkScene.ClearSpawners();
		}

		public static void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					networkConnection.GetStatsOut(out int numMsgs2, out int numBufferedMsgs2, out int numBytes2, out int lastBufferedPerSecond2);
					numMsgs += numMsgs2;
					numBufferedMsgs += numBufferedMsgs2;
					numBytes += numBytes2;
					lastBufferedPerSecond += lastBufferedPerSecond2;
				}
			}
		}

		public static void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					networkConnection.GetStatsIn(out int numMsgs2, out int numBytes2);
					numMsgs += numMsgs2;
					numBytes += numBytes2;
				}
			}
		}

		public static void SendToClientOfPlayer(GameObject player, short msgType, MessageBase msg)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection == null)
				{
					continue;
				}
				for (int j = 0; j < networkConnection.playerControllers.Count; j++)
				{
					if (networkConnection.playerControllers[j].IsValid && networkConnection.playerControllers[j].gameObject == player)
					{
						networkConnection.Send(msgType, msg);
						return;
					}
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to player object '" + player.name + ", not found in connection list");
			}
		}

		public static void SendToClient(int connectionId, short msgType, MessageBase msg)
		{
			if (connectionId < connections.Count)
			{
				NetworkConnection networkConnection = connections[connectionId];
				if (networkConnection != null)
				{
					networkConnection.Send(msgType, msg);
					return;
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to connection ID '" + connectionId + ", not found in connection list");
			}
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			if (GetNetworkIdentity(player, out NetworkIdentity view))
			{
				view.SetDynamicAssetId(assetId);
			}
			return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			if (GetNetworkIdentity(player, out NetworkIdentity view))
			{
				view.SetDynamicAssetId(assetId);
			}
			return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		internal bool InternalAddPlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			if (!GetNetworkIdentity(playerGameObject, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			view.Reset();
			if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			PlayerController playerController = null;
			GameObject x = null;
			if (conn.GetPlayerController(playerControllerId, out playerController))
			{
				x = playerController.gameObject;
			}
			if (x != null)
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: player object already exists for playerControllerId of " + playerControllerId);
				}
				return false;
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			view.SetConnectionToClient(conn, playerController2.playerControllerId);
			SetClientReady(conn);
			if (SetupLocalPlayerForConnection(conn, view, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Adding new playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId);
			}
			FinishPlayerForConnection(conn, view, playerGameObject);
			if (view.localPlayerAuthority)
			{
				view.SetClientOwner(conn);
			}
			return true;
		}

		private static bool CheckPlayerControllerIdForConnection(NetworkConnection conn, short playerControllerId)
		{
			if (playerControllerId < 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddPlayer: playerControllerId of " + playerControllerId + " is negative");
				}
				return false;
			}
			if (playerControllerId > 32)
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: playerControllerId of " + playerControllerId + " is too high. max is " + 32);
				}
				return false;
			}
			if (playerControllerId > 16 && LogFilter.logWarn)
			{
				Debug.LogWarning("AddPlayer: playerControllerId of " + playerControllerId + " is unusually high");
			}
			return true;
		}

		private bool SetupLocalPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, PlayerController newPlayerController)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer SetupLocalPlayerForConnection netID:" + uv.netId);
			}
			ULocalConnectionToClient uLocalConnectionToClient = conn as ULocalConnectionToClient;
			if (uLocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer AddPlayer handling ULocalConnectionToClient");
				}
				if (uv.netId.IsEmpty())
				{
					uv.OnStartServer(allowNonZeroNetId: true);
				}
				uv.RebuildObservers(initialize: true);
				SendSpawnMessage(uv, null);
				uLocalConnectionToClient.localClient.AddLocalPlayer(newPlayerController);
				uv.SetClientOwner(conn);
				uv.ForceAuthority(authority: true);
				uv.SetLocalPlayer(newPlayerController.playerControllerId);
				return true;
			}
			return false;
		}

		private static void FinishPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, GameObject playerGameObject)
		{
			if (uv.netId.IsEmpty())
			{
				Spawn(playerGameObject);
			}
			OwnerMessage ownerMessage = new OwnerMessage();
			ownerMessage.netId = uv.netId;
			ownerMessage.playerControllerId = uv.playerControllerId;
			conn.Send(4, ownerMessage);
		}

		internal bool InternalReplacePlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			if (!GetNetworkIdentity(playerGameObject, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer");
			}
			if (conn.GetPlayerController(playerControllerId, out PlayerController playerController))
			{
				playerController.unetView.SetNotLocalPlayer();
				playerController.unetView.ClearClientOwner();
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			view.SetConnectionToClient(conn, playerController2.playerControllerId);
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer setup local");
			}
			if (SetupLocalPlayerForConnection(conn, view, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Replacing playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId);
			}
			FinishPlayerForConnection(conn, view, playerGameObject);
			if (view.localPlayerAuthority)
			{
				view.SetClientOwner(conn);
			}
			return true;
		}

		private static bool GetNetworkIdentity(GameObject go, out NetworkIdentity view)
		{
			view = go.GetComponent<NetworkIdentity>();
			if (view == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("UNET failure. GameObject doesn't have NetworkIdentity.");
				}
				return false;
			}
			return true;
		}

		public static void SetClientReady(NetworkConnection conn)
		{
			instance.SetClientReadyInternal(conn);
		}

		internal void SetClientReadyInternal(NetworkConnection conn)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("SetClientReadyInternal for conn:" + conn.connectionId);
			}
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("SetClientReady conn " + conn.connectionId + " already ready");
				}
				return;
			}
			if (conn.playerControllers.Count == 0 && LogFilter.logDebug)
			{
				Debug.LogWarning("Ready with no player object");
			}
			conn.isReady = true;
			ULocalConnectionToClient uLocalConnectionToClient = conn as ULocalConnectionToClient;
			if (uLocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer Ready handling ULocalConnectionToClient");
				}
				foreach (NetworkIdentity value in objects.Values)
				{
					if (value != null && value.gameObject != null)
					{
						if (value.OnCheckObserver(conn))
						{
							value.AddObserver(conn);
						}
						if (!value.isClient)
						{
							if (LogFilter.logDev)
							{
								Debug.Log("LocalClient.SetSpawnObject calling OnStartClient");
							}
							value.OnStartClient();
						}
					}
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Spawning " + objects.Count + " objects for conn " + conn.connectionId);
			}
			ObjectSpawnFinishedMessage objectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();
			objectSpawnFinishedMessage.state = 0u;
			conn.Send(12, objectSpawnFinishedMessage);
			foreach (NetworkIdentity value2 in objects.Values)
			{
				if (value2 == null)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("Invalid object found in server local object list (null NetworkIdentity).");
					}
				}
				else if (value2.gameObject.activeSelf)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("Sending spawn message for current server objects name='" + value2.gameObject.name + "' netId=" + value2.netId);
					}
					if (value2.OnCheckObserver(conn))
					{
						value2.AddObserver(conn);
					}
				}
			}
			objectSpawnFinishedMessage.state = 1u;
			conn.Send(12, objectSpawnFinishedMessage);
		}

		internal static void ShowForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			if (conn.isReady)
			{
				instance.SendSpawnMessage(uv, conn);
			}
		}

		internal static void HideForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
			objectDestroyMessage.netId = uv.netId;
			conn.Send(13, objectDestroyMessage);
		}

		public static void SetAllClientsNotReady()
		{
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					SetClientNotReady(networkConnection);
				}
			}
		}

		public static void SetClientNotReady(NetworkConnection conn)
		{
			instance.InternalSetClientNotReady(conn);
		}

		internal void InternalSetClientNotReady(NetworkConnection conn)
		{
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("PlayerNotReady " + conn);
				}
				conn.isReady = false;
				conn.RemoveObservers();
				NotReadyMessage msg = new NotReadyMessage();
				conn.Send(36, msg);
			}
		}

		private static void OnClientReadyMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Default handler for ready message from " + netMsg.conn);
			}
			SetClientReady(netMsg.conn);
		}

		private static void OnRemovePlayerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_RemovePlayerMessage);
			PlayerController playerController = null;
			netMsg.conn.GetPlayerController(s_RemovePlayerMessage.playerControllerId, out playerController);
			if (playerController != null)
			{
				netMsg.conn.RemovePlayerController(s_RemovePlayerMessage.playerControllerId);
				Destroy(playerController.gameObject);
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Received remove player message but could not find the player ID: " + s_RemovePlayerMessage.playerControllerId);
			}
		}

		private static void OnCommandMessage(NetworkMessage netMsg)
		{
			int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			GameObject gameObject = FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Instance not found when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			NetworkIdentity component = gameObject.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkIdentity deleted when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			bool flag = false;
			for (int i = 0; i < netMsg.conn.playerControllers.Count; i++)
			{
				PlayerController playerController = netMsg.conn.playerControllers[i];
				if (playerController.gameObject != null && playerController.gameObject.GetComponent<NetworkIdentity>().netId == component.netId)
				{
					flag = true;
					break;
				}
			}
			if (!flag && component.clientAuthorityOwner != netMsg.conn)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command for object without authority [netId=" + networkInstanceId + "]");
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("OnCommandMessage for netId=" + networkInstanceId + " conn=" + netMsg.conn);
			}
			component.HandleCommand(cmdHash, netMsg.reader);
		}

		internal void SpawnObject(GameObject obj)
		{
			if (!active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SpawnObject for " + obj + ", NetworkServer is not active. Cannot spawn objects without an active server.");
				}
				return;
			}
			if (!GetNetworkIdentity(obj, out NetworkIdentity view))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SpawnObject " + obj + " has no NetworkIdentity. Please add a NetworkIdentity to " + obj);
				}
				return;
			}
			view.Reset();
			view.OnStartServer(allowNonZeroNetId: false);
			if (LogFilter.logDebug)
			{
				Debug.Log("SpawnObject instance ID " + view.netId + " asset ID " + view.assetId);
			}
			view.RebuildObservers(initialize: true);
		}

		internal void SendSpawnMessage(NetworkIdentity uv, NetworkConnection conn)
		{
			if (uv.serverOnly)
			{
				return;
			}
			if (uv.sceneId.IsEmpty())
			{
				ObjectSpawnMessage objectSpawnMessage = new ObjectSpawnMessage();
				objectSpawnMessage.netId = uv.netId;
				objectSpawnMessage.assetId = uv.assetId;
				objectSpawnMessage.position = uv.transform.position;
				objectSpawnMessage.rotation = uv.transform.rotation;
				NetworkWriter networkWriter = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter);
				if (networkWriter.Position > 0)
				{
					objectSpawnMessage.payload = networkWriter.ToArray();
				}
				if (conn != null)
				{
					conn.Send(3, objectSpawnMessage);
				}
				else
				{
					SendToReady(uv.gameObject, 3, objectSpawnMessage);
				}
			}
			else
			{
				ObjectSpawnSceneMessage objectSpawnSceneMessage = new ObjectSpawnSceneMessage();
				objectSpawnSceneMessage.netId = uv.netId;
				objectSpawnSceneMessage.sceneId = uv.sceneId;
				objectSpawnSceneMessage.position = uv.transform.position;
				NetworkWriter networkWriter2 = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter2);
				if (networkWriter2.Position > 0)
				{
					objectSpawnSceneMessage.payload = networkWriter2.ToArray();
				}
				if (conn != null)
				{
					conn.Send(10, objectSpawnSceneMessage);
				}
				else
				{
					SendToReady(uv.gameObject, 3, objectSpawnSceneMessage);
				}
			}
		}

		public static void DestroyPlayersForConnection(NetworkConnection conn)
		{
			if (conn.playerControllers.Count == 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Empty player list given to NetworkServer.Destroy(), nothing to do.");
				}
				return;
			}
			if (conn.clientOwnedObjects != null)
			{
				HashSet<NetworkInstanceId> hashSet = new HashSet<NetworkInstanceId>(conn.clientOwnedObjects);
				foreach (NetworkInstanceId item in hashSet)
				{
					GameObject gameObject = FindLocalObject(item);
					if (gameObject != null)
					{
						DestroyObject(gameObject);
					}
				}
			}
			for (int i = 0; i < conn.playerControllers.Count; i++)
			{
				PlayerController playerController = conn.playerControllers[i];
				if (playerController.IsValid)
				{
					if (!(playerController.unetView == null))
					{
						DestroyObject(playerController.unetView, destroyServerObject: true);
					}
					playerController.gameObject = null;
				}
			}
			conn.playerControllers.Clear();
		}

		private static void UnSpawnObject(GameObject obj)
		{
			NetworkIdentity view;
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer UnspawnObject is null");
				}
			}
			else if (GetNetworkIdentity(obj, out view))
			{
				UnSpawnObject(view);
			}
		}

		private static void UnSpawnObject(NetworkIdentity uv)
		{
			DestroyObject(uv, destroyServerObject: false);
		}

		private static void DestroyObject(GameObject obj)
		{
			NetworkIdentity view;
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer DestroyObject is null");
				}
			}
			else if (GetNetworkIdentity(obj, out view))
			{
				DestroyObject(view, destroyServerObject: true);
			}
		}

		private static void DestroyObject(NetworkIdentity uv, bool destroyServerObject)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("DestroyObject instance:" + uv.netId);
			}
			if (objects.ContainsKey(uv.netId))
			{
				objects.Remove(uv.netId);
			}
			if (uv.clientAuthorityOwner != null)
			{
				uv.clientAuthorityOwner.RemoveOwnedObject(uv);
			}
			ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
			objectDestroyMessage.netId = uv.netId;
			SendToObservers(uv.gameObject, 1, objectDestroyMessage);
			uv.ClearObservers();
			if (NetworkClient.active && instance.m_LocalClientActive)
			{
				uv.OnNetworkDestroy();
				ClientScene.SetLocalObject(objectDestroyMessage.netId, null);
			}
			if (destroyServerObject)
			{
				Object.Destroy(uv.gameObject);
			}
			uv.MarkForReset();
		}

		public static void ClearLocalObjects()
		{
			objects.Clear();
		}

		public static void Spawn(GameObject obj)
		{
			if (VerifyCanSpawn(obj))
			{
				instance.SpawnObject(obj);
			}
		}

		private static bool CheckForPrefab(GameObject obj)
		{
			return false;
		}

		private static bool VerifyCanSpawn(GameObject obj)
		{
			if (CheckForPrefab(obj))
			{
				Debug.LogErrorFormat("GameObject {0} is a prefab, it can't be spawned. This will cause errors in builds.", obj.name);
				return false;
			}
			return true;
		}

		public static bool SpawnWithClientAuthority(GameObject obj, GameObject player)
		{
			NetworkIdentity component = player.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object has no NetworkIdentity");
				return false;
			}
			if (component.connectionToClient == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object is not a player.");
				return false;
			}
			return SpawnWithClientAuthority(obj, component.connectionToClient);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkConnection conn)
		{
			if (!conn.isReady)
			{
				Debug.LogError("SpawnWithClientAuthority NetworkConnection is not ready!");
				return false;
			}
			Spawn(obj);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			if (component == null || !component.isServer)
			{
				return false;
			}
			return component.AssignClientAuthority(conn);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkHash128 assetId, NetworkConnection conn)
		{
			Spawn(obj, assetId);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			if (component == null || !component.isServer)
			{
				return false;
			}
			return component.AssignClientAuthority(conn);
		}

		public static void Spawn(GameObject obj, NetworkHash128 assetId)
		{
			if (VerifyCanSpawn(obj))
			{
				if (GetNetworkIdentity(obj, out NetworkIdentity view))
				{
					view.SetDynamicAssetId(assetId);
				}
				instance.SpawnObject(obj);
			}
		}

		public static void Destroy(GameObject obj)
		{
			DestroyObject(obj);
		}

		public static void UnSpawn(GameObject obj)
		{
			UnSpawnObject(obj);
		}

		internal bool InvokeBytes(ULocalConnectionToServer conn, byte[] buffer, int numBytes, int channelId)
		{
			NetworkReader networkReader = new NetworkReader(buffer);
			networkReader.ReadInt16();
			short num = networkReader.ReadInt16();
			if (handlers.ContainsKey(num) && m_LocalConnection != null)
			{
				m_LocalConnection.InvokeHandler(num, networkReader, channelId);
				return true;
			}
			return false;
		}

		internal bool InvokeHandlerOnServer(ULocalConnectionToServer conn, short msgType, MessageBase msg, int channelId)
		{
			if (handlers.ContainsKey(msgType) && m_LocalConnection != null)
			{
				NetworkWriter writer = new NetworkWriter();
				msg.Serialize(writer);
				NetworkReader reader = new NetworkReader(writer);
				m_LocalConnection.InvokeHandler(msgType, reader, channelId);
				return true;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Local invoke: Failed to find local connection to invoke handler on [connectionId=" + conn.connectionId + "] for MsgId:" + msgType);
			}
			return false;
		}

		public static GameObject FindLocalObject(NetworkInstanceId netId)
		{
			return instance.m_NetworkScene.FindLocalObject(netId);
		}

		public static Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
		{
			Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
			for (int i = 0; i < connections.Count; i++)
			{
				NetworkConnection networkConnection = connections[i];
				if (networkConnection != null)
				{
					foreach (short key in networkConnection.packetStats.Keys)
					{
						if (dictionary.ContainsKey(key))
						{
							NetworkConnection.PacketStat packetStat = dictionary[key];
							packetStat.count += networkConnection.packetStats[key].count;
							packetStat.bytes += networkConnection.packetStats[key].bytes;
							dictionary[key] = packetStat;
						}
						else
						{
							dictionary[key] = new NetworkConnection.PacketStat(networkConnection.packetStats[key]);
						}
					}
				}
			}
			return dictionary;
		}

		public static void ResetConnectionStats()
		{
			for (int i = 0; i < connections.Count; i++)
			{
				connections[i]?.ResetStats();
			}
		}

		public static bool AddExternalConnection(NetworkConnection conn)
		{
			return instance.AddExternalConnectionInternal(conn);
		}

		private bool AddExternalConnectionInternal(NetworkConnection conn)
		{
			if (conn.connectionId < 0)
			{
				return false;
			}
			if (conn.connectionId < connections.Count && connections[conn.connectionId] != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddExternalConnection failed, already connection for id:" + conn.connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("AddExternalConnection external connection " + conn.connectionId);
			}
			m_SimpleServerSimple.SetConnectionAtIndex(conn);
			m_ExternalConnections.Add(conn.connectionId);
			conn.InvokeHandlerNoData(32);
			return true;
		}

		public static void RemoveExternalConnection(int connectionId)
		{
			instance.RemoveExternalConnectionInternal(connectionId);
		}

		private bool RemoveExternalConnectionInternal(int connectionId)
		{
			if (!m_ExternalConnections.Contains(connectionId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveExternalConnection failed, no connection for id:" + connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RemoveExternalConnection external connection " + connectionId);
			}
			m_SimpleServerSimple.FindConnection(connectionId)?.RemoveObservers();
			m_SimpleServerSimple.RemoveConnectionAtIndex(connectionId);
			return true;
		}

		public static bool SpawnObjects()
		{
			if (active)
			{
				NetworkIdentity[] array = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
				foreach (NetworkIdentity networkIdentity in array)
				{
					if (networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity.sceneId.IsEmpty())
					{
						if (LogFilter.logDebug)
						{
							Debug.Log("SpawnObjects sceneId:" + networkIdentity.sceneId + " name:" + networkIdentity.gameObject.name);
						}
						networkIdentity.gameObject.SetActive(value: true);
					}
				}
				foreach (NetworkIdentity networkIdentity2 in array)
				{
					if (networkIdentity2.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity2.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity2.sceneId.IsEmpty() && !networkIdentity2.isServer && !(networkIdentity2.gameObject == null))
					{
						Spawn(networkIdentity2.gameObject);
						networkIdentity2.ForceAuthority(authority: true);
					}
				}
			}
			return true;
		}

		private static void SendCrc(NetworkConnection targetConnection)
		{
			if (NetworkCRC.singleton != null && NetworkCRC.scriptCRCCheck)
			{
				CRCMessage cRCMessage = new CRCMessage();
				List<CRCMessageEntry> list = new List<CRCMessageEntry>();
				foreach (string key in NetworkCRC.singleton.scripts.Keys)
				{
					CRCMessageEntry item = default(CRCMessageEntry);
					item.name = key;
					item.channel = (byte)NetworkCRC.singleton.scripts[key];
					list.Add(item);
				}
				cRCMessage.scripts = list.ToArray();
				targetConnection.Send(14, cRCMessage);
			}
		}

		[Obsolete("moved to NetworkMigrationManager")]
		public void SendNetworkInfo(NetworkConnection targetConnection)
		{
		}
	}
}
