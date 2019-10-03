using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkIdentity")]
	public sealed class NetworkIdentity : MonoBehaviour
	{
		public delegate void ClientAuthorityCallback(NetworkConnection conn, NetworkIdentity uv, bool authorityState);

		[SerializeField]
		private NetworkSceneId m_SceneId;

		[SerializeField]
		private NetworkHash128 m_AssetId;

		[SerializeField]
		private bool m_ServerOnly;

		[SerializeField]
		private bool m_LocalPlayerAuthority;

		private bool m_IsClient;

		private bool m_IsServer;

		private bool m_HasAuthority;

		private NetworkInstanceId m_NetId;

		private bool m_IsLocalPlayer;

		private NetworkConnection m_ConnectionToServer;

		private NetworkConnection m_ConnectionToClient;

		private short m_PlayerId = -1;

		private NetworkBehaviour[] m_NetworkBehaviours;

		private HashSet<int> m_ObserverConnections;

		private List<NetworkConnection> m_Observers;

		private NetworkConnection m_ClientAuthorityOwner;

		private bool m_Reset = false;

		private static uint s_NextNetworkId = 1u;

		private static NetworkWriter s_UpdateWriter = new NetworkWriter();

		public static ClientAuthorityCallback clientAuthorityCallback;

		public bool isClient => m_IsClient;

		public bool isServer
		{
			get
			{
				if (!m_IsServer)
				{
					return false;
				}
				return NetworkServer.active && m_IsServer;
			}
		}

		public bool hasAuthority => m_HasAuthority;

		public NetworkInstanceId netId => m_NetId;

		public NetworkSceneId sceneId => m_SceneId;

		public bool serverOnly
		{
			get
			{
				return m_ServerOnly;
			}
			set
			{
				m_ServerOnly = value;
			}
		}

		public bool localPlayerAuthority
		{
			get
			{
				return m_LocalPlayerAuthority;
			}
			set
			{
				m_LocalPlayerAuthority = value;
			}
		}

		public NetworkConnection clientAuthorityOwner => m_ClientAuthorityOwner;

		public NetworkHash128 assetId => m_AssetId;

		public bool isLocalPlayer => m_IsLocalPlayer;

		public short playerControllerId => m_PlayerId;

		public NetworkConnection connectionToServer => m_ConnectionToServer;

		public NetworkConnection connectionToClient => m_ConnectionToClient;

		public ReadOnlyCollection<NetworkConnection> observers
		{
			get
			{
				if (m_Observers == null)
				{
					return null;
				}
				return new ReadOnlyCollection<NetworkConnection>(m_Observers);
			}
		}

		internal void SetDynamicAssetId(NetworkHash128 newAssetId)
		{
			if (!m_AssetId.IsValid() || m_AssetId.Equals(newAssetId))
			{
				m_AssetId = newAssetId;
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("SetDynamicAssetId object already has an assetId <" + m_AssetId + ">");
			}
		}

		internal void SetClientOwner(NetworkConnection conn)
		{
			if (m_ClientAuthorityOwner != null && LogFilter.logError)
			{
				Debug.LogError("SetClientOwner m_ClientAuthorityOwner already set!");
			}
			m_ClientAuthorityOwner = conn;
			m_ClientAuthorityOwner.AddOwnedObject(this);
		}

		internal void ClearClientOwner()
		{
			m_ClientAuthorityOwner = null;
		}

		internal void ForceAuthority(bool authority)
		{
			if (m_HasAuthority != authority)
			{
				m_HasAuthority = authority;
				if (authority)
				{
					OnStartAuthority();
				}
				else
				{
					OnStopAuthority();
				}
			}
		}

		internal static NetworkInstanceId GetNextNetworkId()
		{
			uint value = s_NextNetworkId;
			s_NextNetworkId++;
			return new NetworkInstanceId(value);
		}

		private void CacheBehaviours()
		{
			if (m_NetworkBehaviours == null)
			{
				m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
			}
		}

		internal static void AddNetworkId(uint id)
		{
			if (id >= s_NextNetworkId)
			{
				s_NextNetworkId = id + 1;
			}
		}

		internal void SetNetworkInstanceId(NetworkInstanceId newNetId)
		{
			m_NetId = newNetId;
			if (newNetId.Value == 0)
			{
				m_IsServer = false;
			}
		}

		public void ForceSceneId(int newSceneId)
		{
			m_SceneId = new NetworkSceneId((uint)newSceneId);
		}

		internal void UpdateClientServer(bool isClientFlag, bool isServerFlag)
		{
			m_IsClient |= isClientFlag;
			m_IsServer |= isServerFlag;
		}

		internal void SetNotLocalPlayer()
		{
			m_IsLocalPlayer = false;
			if (!NetworkServer.active || !NetworkServer.localClientActive)
			{
				m_HasAuthority = false;
			}
		}

		internal void RemoveObserverInternal(NetworkConnection conn)
		{
			if (m_Observers != null)
			{
				m_Observers.Remove(conn);
				m_ObserverConnections.Remove(conn.connectionId);
			}
		}

		private void OnDestroy()
		{
			if (m_IsServer && NetworkServer.active)
			{
				NetworkServer.Destroy(base.gameObject);
			}
		}

		internal void OnStartServer(bool allowNonZeroNetId)
		{
			if (m_IsServer)
			{
				return;
			}
			m_IsServer = true;
			if (m_LocalPlayerAuthority)
			{
				m_HasAuthority = false;
			}
			else
			{
				m_HasAuthority = true;
			}
			m_Observers = new List<NetworkConnection>();
			m_ObserverConnections = new HashSet<int>();
			CacheBehaviours();
			if (netId.IsEmpty())
			{
				m_NetId = GetNextNetworkId();
			}
			else if (!allowNonZeroNetId)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Object has non-zero netId " + netId + " for " + base.gameObject);
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("OnStartServer " + base.gameObject + " GUID:" + netId);
			}
			NetworkServer.instance.SetLocalObjectOnServer(netId, base.gameObject);
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnStartServer();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStartServer:" + ex.Message + " " + ex.StackTrace);
				}
			}
			if (NetworkClient.active && NetworkServer.localClientActive)
			{
				ClientScene.SetLocalObject(netId, base.gameObject);
				OnStartClient();
			}
			if (m_HasAuthority)
			{
				OnStartAuthority();
			}
		}

		internal void OnStartClient()
		{
			if (!m_IsClient)
			{
				m_IsClient = true;
			}
			CacheBehaviours();
			if (LogFilter.logDev)
			{
				Debug.Log("OnStartClient " + base.gameObject + " GUID:" + netId + " localPlayerAuthority:" + localPlayerAuthority);
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.PreStartClient();
					networkBehaviour.OnStartClient();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStartClient:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnStartAuthority()
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnStartAuthority();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStartAuthority:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnStopAuthority()
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnStopAuthority();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnStopAuthority:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal void OnSetLocalVisibility(bool vis)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					networkBehaviour.OnSetLocalVisibility(vis);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnSetLocalVisibility:" + ex.Message + " " + ex.StackTrace);
				}
			}
		}

		internal bool OnCheckObserver(NetworkConnection conn)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				try
				{
					if (!networkBehaviour.OnCheckObserver(conn))
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnCheckObserver:" + ex.Message + " " + ex.StackTrace);
				}
			}
			return true;
		}

		internal void UNetSerializeAllVars(NetworkWriter writer)
		{
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnSerialize(writer, initialState: true);
			}
		}

		internal void HandleClientAuthority(bool authority)
		{
			if (!localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleClientAuthority " + base.gameObject + " does not have localPlayerAuthority");
				}
			}
			else
			{
				ForceAuthority(authority);
			}
		}

		private bool GetInvokeComponent(int cmdHash, Type invokeClass, out NetworkBehaviour invokeComponent)
		{
			NetworkBehaviour networkBehaviour = null;
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour2 = m_NetworkBehaviours[i];
				if (networkBehaviour2.GetType() == invokeClass || networkBehaviour2.GetType().IsSubclassOf(invokeClass))
				{
					networkBehaviour = networkBehaviour2;
					break;
				}
			}
			if (networkBehaviour == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no behaviour for incoming [" + cmdHashHandlerName + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
				invokeComponent = null;
				return false;
			}
			invokeComponent = networkBehaviour;
			return true;
		}

		internal void HandleSyncEvent(int cmdHash, NetworkReader reader)
		{
			Type invokeClass;
			NetworkBehaviour.CmdDelegate invokeFunction;
			NetworkBehaviour invokeComponent;
			if (base.gameObject == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncEvent [" + cmdHashHandlerName + "] received for deleted object [netId=" + netId + "]");
				}
			}
			else if (!NetworkBehaviour.GetInvokerForHashSyncEvent(cmdHash, out invokeClass, out invokeFunction))
			{
				string cmdHashHandlerName2 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no receiver for incoming [" + cmdHashHandlerName2 + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
			}
			else if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
			{
				string cmdHashHandlerName3 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncEvent [" + cmdHashHandlerName3 + "] handler not found [netId=" + netId + "]");
				}
			}
			else
			{
				invokeFunction(invokeComponent, reader);
			}
		}

		internal void HandleSyncList(int cmdHash, NetworkReader reader)
		{
			Type invokeClass;
			NetworkBehaviour.CmdDelegate invokeFunction;
			NetworkBehaviour invokeComponent;
			if (base.gameObject == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncList [" + cmdHashHandlerName + "] received for deleted object [netId=" + netId + "]");
				}
			}
			else if (!NetworkBehaviour.GetInvokerForHashSyncList(cmdHash, out invokeClass, out invokeFunction))
			{
				string cmdHashHandlerName2 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no receiver for incoming [" + cmdHashHandlerName2 + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
			}
			else if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
			{
				string cmdHashHandlerName3 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SyncList [" + cmdHashHandlerName3 + "] handler not found [netId=" + netId + "]");
				}
			}
			else
			{
				invokeFunction(invokeComponent, reader);
			}
		}

		internal void HandleCommand(int cmdHash, NetworkReader reader)
		{
			Type invokeClass;
			NetworkBehaviour.CmdDelegate invokeFunction;
			NetworkBehaviour invokeComponent;
			if (base.gameObject == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command [" + cmdHashHandlerName + "] received for deleted object [netId=" + netId + "]");
				}
			}
			else if (!NetworkBehaviour.GetInvokerForHashCommand(cmdHash, out invokeClass, out invokeFunction))
			{
				string cmdHashHandlerName2 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no receiver for incoming [" + cmdHashHandlerName2 + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
			}
			else if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
			{
				string cmdHashHandlerName3 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command [" + cmdHashHandlerName3 + "] handler not found [netId=" + netId + "]");
				}
			}
			else
			{
				invokeFunction(invokeComponent, reader);
			}
		}

		internal void HandleRPC(int cmdHash, NetworkReader reader)
		{
			Type invokeClass;
			NetworkBehaviour.CmdDelegate invokeFunction;
			NetworkBehaviour invokeComponent;
			if (base.gameObject == null)
			{
				string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientRpc [" + cmdHashHandlerName + "] received for deleted object [netId=" + netId + "]");
				}
			}
			else if (!NetworkBehaviour.GetInvokerForHashClientRpc(cmdHash, out invokeClass, out invokeFunction))
			{
				string cmdHashHandlerName2 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logError)
				{
					Debug.LogError("Found no receiver for incoming [" + cmdHashHandlerName2 + "] on " + base.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "].");
				}
			}
			else if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
			{
				string cmdHashHandlerName3 = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientRpc [" + cmdHashHandlerName3 + "] handler not found [netId=" + netId + "]");
				}
			}
			else
			{
				invokeFunction(invokeComponent, reader);
			}
		}

		internal void UNetUpdate()
		{
			uint num = 0u;
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				int dirtyChannel = networkBehaviour.GetDirtyChannel();
				if (dirtyChannel != -1)
				{
					num = (uint)((int)num | (1 << dirtyChannel));
				}
			}
			if (num == 0)
			{
				return;
			}
			for (int j = 0; j < NetworkServer.numChannels; j++)
			{
				if (((int)num & (1 << j)) == 0)
				{
					continue;
				}
				s_UpdateWriter.StartMessage(8);
				s_UpdateWriter.Write(netId);
				bool flag = false;
				for (int k = 0; k < m_NetworkBehaviours.Length; k++)
				{
					short position = s_UpdateWriter.Position;
					NetworkBehaviour networkBehaviour2 = m_NetworkBehaviours[k];
					if (networkBehaviour2.GetDirtyChannel() != j)
					{
						networkBehaviour2.OnSerialize(s_UpdateWriter, initialState: false);
						continue;
					}
					if (networkBehaviour2.OnSerialize(s_UpdateWriter, initialState: false))
					{
						networkBehaviour2.ClearAllDirtyBits();
						flag = true;
					}
					if (s_UpdateWriter.Position - position > NetworkServer.maxPacketSize && LogFilter.logWarn)
					{
						Debug.LogWarning("Large state update of " + (s_UpdateWriter.Position - position) + " bytes for netId:" + netId + " from script:" + networkBehaviour2);
					}
				}
				if (flag)
				{
					s_UpdateWriter.FinishMessage();
					NetworkServer.SendWriterToReady(base.gameObject, s_UpdateWriter, j);
				}
			}
		}

		internal void OnUpdateVars(NetworkReader reader, bool initialState)
		{
			if (initialState && m_NetworkBehaviours == null)
			{
				m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnDeserialize(reader, initialState);
			}
		}

		internal void SetLocalPlayer(short localPlayerControllerId)
		{
			m_IsLocalPlayer = true;
			m_PlayerId = localPlayerControllerId;
			bool hasAuthority = m_HasAuthority;
			if (localPlayerAuthority)
			{
				m_HasAuthority = true;
			}
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				networkBehaviour.OnStartLocalPlayer();
				if (localPlayerAuthority && !hasAuthority)
				{
					networkBehaviour.OnStartAuthority();
				}
			}
		}

		internal void SetConnectionToServer(NetworkConnection conn)
		{
			m_ConnectionToServer = conn;
		}

		internal void SetConnectionToClient(NetworkConnection conn, short newPlayerControllerId)
		{
			m_PlayerId = newPlayerControllerId;
			m_ConnectionToClient = conn;
		}

		internal void OnNetworkDestroy()
		{
			int num = 0;
			while (m_NetworkBehaviours != null && num < m_NetworkBehaviours.Length)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[num];
				networkBehaviour.OnNetworkDestroy();
				num++;
			}
			m_IsServer = false;
		}

		internal void ClearObservers()
		{
			if (m_Observers != null)
			{
				int count = m_Observers.Count;
				for (int i = 0; i < count; i++)
				{
					NetworkConnection networkConnection = m_Observers[i];
					networkConnection.RemoveFromVisList(this, isDestroyed: true);
				}
				m_Observers.Clear();
				m_ObserverConnections.Clear();
			}
		}

		internal void AddObserver(NetworkConnection conn)
		{
			if (m_Observers == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddObserver for " + base.gameObject + " observer list is null");
				}
				return;
			}
			if (m_ObserverConnections.Contains(conn.connectionId))
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("Duplicate observer " + conn.address + " added for " + base.gameObject);
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("Added observer " + conn.address + " added for " + base.gameObject);
			}
			m_Observers.Add(conn);
			m_ObserverConnections.Add(conn.connectionId);
			conn.AddToVisList(this);
		}

		internal void RemoveObserver(NetworkConnection conn)
		{
			if (m_Observers != null)
			{
				m_Observers.Remove(conn);
				m_ObserverConnections.Remove(conn.connectionId);
				conn.RemoveFromVisList(this, isDestroyed: false);
			}
		}

		public void RebuildObservers(bool initialize)
		{
			if (m_Observers == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			HashSet<NetworkConnection> hashSet = new HashSet<NetworkConnection>();
			HashSet<NetworkConnection> hashSet2 = new HashSet<NetworkConnection>(m_Observers);
			for (int i = 0; i < m_NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = m_NetworkBehaviours[i];
				flag2 |= networkBehaviour.OnRebuildObservers(hashSet, initialize);
			}
			if (!flag2)
			{
				if (!initialize)
				{
					return;
				}
				for (int j = 0; j < NetworkServer.connections.Count; j++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[j];
					if (networkConnection != null && networkConnection.isReady)
					{
						AddObserver(networkConnection);
					}
				}
				for (int k = 0; k < NetworkServer.localConnections.Count; k++)
				{
					NetworkConnection networkConnection2 = NetworkServer.localConnections[k];
					if (networkConnection2 != null && networkConnection2.isReady)
					{
						AddObserver(networkConnection2);
					}
				}
				return;
			}
			foreach (NetworkConnection item in hashSet)
			{
				if (item != null)
				{
					if (!item.isReady)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("Observer is not ready for " + base.gameObject + " " + item);
						}
					}
					else if (initialize || !hashSet2.Contains(item))
					{
						item.AddToVisList(this);
						if (LogFilter.logDebug)
						{
							Debug.Log("New Observer for " + base.gameObject + " " + item);
						}
						flag = true;
					}
				}
			}
			foreach (NetworkConnection item2 in hashSet2)
			{
				if (!hashSet.Contains(item2))
				{
					item2.RemoveFromVisList(this, isDestroyed: false);
					if (LogFilter.logDebug)
					{
						Debug.Log("Removed Observer for " + base.gameObject + " " + item2);
					}
					flag = true;
				}
			}
			if (initialize)
			{
				for (int l = 0; l < NetworkServer.localConnections.Count; l++)
				{
					if (!hashSet.Contains(NetworkServer.localConnections[l]))
					{
						OnSetLocalVisibility(vis: false);
					}
				}
			}
			if (flag)
			{
				m_Observers = new List<NetworkConnection>(hashSet);
				m_ObserverConnections.Clear();
				for (int m = 0; m < m_Observers.Count; m++)
				{
					m_ObserverConnections.Add(m_Observers[m].connectionId);
				}
			}
		}

		public bool RemoveClientAuthority(NetworkConnection conn)
		{
			if (!isServer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority can only be call on the server for spawned objects.");
				}
				return false;
			}
			if (connectionToClient != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority cannot remove authority for a player object");
				}
				return false;
			}
			if (m_ClientAuthorityOwner == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority for " + base.gameObject + " has no clientAuthority owner.");
				}
				return false;
			}
			if (m_ClientAuthorityOwner != conn)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveClientAuthority for " + base.gameObject + " has different owner.");
				}
				return false;
			}
			m_ClientAuthorityOwner.RemoveOwnedObject(this);
			m_ClientAuthorityOwner = null;
			ForceAuthority(authority: true);
			ClientAuthorityMessage clientAuthorityMessage = new ClientAuthorityMessage();
			clientAuthorityMessage.netId = netId;
			clientAuthorityMessage.authority = false;
			conn.Send(15, clientAuthorityMessage);
			if (clientAuthorityCallback != null)
			{
				clientAuthorityCallback(conn, this, authorityState: false);
			}
			return true;
		}

		public bool AssignClientAuthority(NetworkConnection conn)
		{
			if (!isServer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority can only be call on the server for spawned objects.");
				}
				return false;
			}
			if (!localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority can only be used for NetworkIdentity component with LocalPlayerAuthority set.");
				}
				return false;
			}
			if (m_ClientAuthorityOwner != null && conn != m_ClientAuthorityOwner)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority for " + base.gameObject + " already has an owner. Use RemoveClientAuthority() first.");
				}
				return false;
			}
			if (conn == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AssignClientAuthority for " + base.gameObject + " owner cannot be null. Use RemoveClientAuthority() instead.");
				}
				return false;
			}
			m_ClientAuthorityOwner = conn;
			m_ClientAuthorityOwner.AddOwnedObject(this);
			ForceAuthority(authority: false);
			ClientAuthorityMessage clientAuthorityMessage = new ClientAuthorityMessage();
			clientAuthorityMessage.netId = netId;
			clientAuthorityMessage.authority = true;
			conn.Send(15, clientAuthorityMessage);
			if (clientAuthorityCallback != null)
			{
				clientAuthorityCallback(conn, this, authorityState: true);
			}
			return true;
		}

		internal void MarkForReset()
		{
			m_Reset = true;
		}

		internal void Reset()
		{
			if (m_Reset)
			{
				m_Reset = false;
				m_IsServer = false;
				m_IsClient = false;
				m_HasAuthority = false;
				m_NetId = NetworkInstanceId.Zero;
				m_IsLocalPlayer = false;
				m_ConnectionToServer = null;
				m_ConnectionToClient = null;
				m_PlayerId = -1;
				m_NetworkBehaviours = null;
				ClearObservers();
				m_ClientAuthorityOwner = null;
			}
		}

		internal static void UNetStaticUpdate()
		{
			NetworkServer.Update();
			NetworkClient.UpdateClients();
			NetworkManager.UpdateScene();
		}
	}
}
