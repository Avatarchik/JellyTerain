using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.Networking
{
	[RequireComponent(typeof(NetworkIdentity))]
	[AddComponentMenu("")]
	public class NetworkBehaviour : MonoBehaviour
	{
		public delegate void CmdDelegate(NetworkBehaviour obj, NetworkReader reader);

		protected delegate void EventDelegate(List<Delegate> targets, NetworkReader reader);

		protected enum UNetInvokeType
		{
			Command,
			ClientRpc,
			SyncEvent,
			SyncList
		}

		protected class Invoker
		{
			public UNetInvokeType invokeType;

			public Type invokeClass;

			public CmdDelegate invokeFunction;

			public string DebugString()
			{
				return invokeType + ":" + invokeClass + ":" + invokeFunction.GetMethodName();
			}
		}

		private uint m_SyncVarDirtyBits;

		private float m_LastSendTime;

		private bool m_SyncVarGuard;

		private const float k_DefaultSendInterval = 0.1f;

		private NetworkIdentity m_MyView;

		private static Dictionary<int, Invoker> s_CmdHandlerDelegates = new Dictionary<int, Invoker>();

		public bool localPlayerAuthority => myView.localPlayerAuthority;

		public bool isServer => myView.isServer;

		public bool isClient => myView.isClient;

		public bool isLocalPlayer => myView.isLocalPlayer;

		public bool hasAuthority => myView.hasAuthority;

		public NetworkInstanceId netId => myView.netId;

		public NetworkConnection connectionToServer => myView.connectionToServer;

		public NetworkConnection connectionToClient => myView.connectionToClient;

		public short playerControllerId => myView.playerControllerId;

		protected uint syncVarDirtyBits => m_SyncVarDirtyBits;

		protected bool syncVarHookGuard
		{
			get
			{
				return m_SyncVarGuard;
			}
			set
			{
				m_SyncVarGuard = value;
			}
		}

		private NetworkIdentity myView
		{
			get
			{
				if (m_MyView == null)
				{
					m_MyView = GetComponent<NetworkIdentity>();
					if (m_MyView == null && LogFilter.logError)
					{
						Debug.LogError("There is no NetworkIdentity on this object. Please add one.");
					}
					return m_MyView;
				}
				return m_MyView;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SendCommandInternal(NetworkWriter writer, int channelId, string cmdName)
		{
			if (!isLocalPlayer && !hasAuthority)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Trying to send command for object without authority.");
				}
			}
			else if (ClientScene.readyConnection == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Send command attempted with no client running [client=" + connectionToServer + "].");
				}
			}
			else
			{
				writer.FinishMessage();
				ClientScene.readyConnection.SendWriter(writer, channelId);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool InvokeCommand(int cmdHash, NetworkReader reader)
		{
			if (InvokeCommandDelegate(cmdHash, reader))
			{
				return true;
			}
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SendRPCInternal(NetworkWriter writer, int channelId, string rpcName)
		{
			if (!isServer)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ClientRpc call on un-spawned object");
				}
			}
			else
			{
				writer.FinishMessage();
				NetworkServer.SendWriterToReady(base.gameObject, writer, channelId);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SendTargetRPCInternal(NetworkConnection conn, NetworkWriter writer, int channelId, string rpcName)
		{
			if (!isServer)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("TargetRpc call on un-spawned object");
				}
			}
			else
			{
				writer.FinishMessage();
				conn.SendWriter(writer, channelId);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool InvokeRPC(int cmdHash, NetworkReader reader)
		{
			if (InvokeRpcDelegate(cmdHash, reader))
			{
				return true;
			}
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SendEventInternal(NetworkWriter writer, int channelId, string eventName)
		{
			if (!NetworkServer.active)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SendEvent no server?");
				}
			}
			else
			{
				writer.FinishMessage();
				NetworkServer.SendWriterToReady(base.gameObject, writer, channelId);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool InvokeSyncEvent(int cmdHash, NetworkReader reader)
		{
			if (InvokeSyncEventDelegate(cmdHash, reader))
			{
				return true;
			}
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool InvokeSyncList(int cmdHash, NetworkReader reader)
		{
			if (InvokeSyncListDelegate(cmdHash, reader))
			{
				return true;
			}
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected static void RegisterCommandDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				Invoker invoker = new Invoker();
				invoker.invokeType = UNetInvokeType.Command;
				invoker.invokeClass = invokeClass;
				invoker.invokeFunction = func;
				s_CmdHandlerDelegates[cmdHash] = invoker;
				if (LogFilter.logDev)
				{
					Debug.Log("RegisterCommandDelegate hash:" + cmdHash + " " + func.GetMethodName());
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected static void RegisterRpcDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				Invoker invoker = new Invoker();
				invoker.invokeType = UNetInvokeType.ClientRpc;
				invoker.invokeClass = invokeClass;
				invoker.invokeFunction = func;
				s_CmdHandlerDelegates[cmdHash] = invoker;
				if (LogFilter.logDev)
				{
					Debug.Log("RegisterRpcDelegate hash:" + cmdHash + " " + func.GetMethodName());
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected static void RegisterEventDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				Invoker invoker = new Invoker();
				invoker.invokeType = UNetInvokeType.SyncEvent;
				invoker.invokeClass = invokeClass;
				invoker.invokeFunction = func;
				s_CmdHandlerDelegates[cmdHash] = invoker;
				if (LogFilter.logDev)
				{
					Debug.Log("RegisterEventDelegate hash:" + cmdHash + " " + func.GetMethodName());
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected static void RegisterSyncListDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				Invoker invoker = new Invoker();
				invoker.invokeType = UNetInvokeType.SyncList;
				invoker.invokeClass = invokeClass;
				invoker.invokeFunction = func;
				s_CmdHandlerDelegates[cmdHash] = invoker;
				if (LogFilter.logDev)
				{
					Debug.Log("RegisterSyncListDelegate hash:" + cmdHash + " " + func.GetMethodName());
				}
			}
		}

		internal static string GetInvoker(int cmdHash)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return null;
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			return invoker.DebugString();
		}

		internal static bool GetInvokerForHashCommand(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
		{
			return GetInvokerForHash(cmdHash, UNetInvokeType.Command, out invokeClass, out invokeFunction);
		}

		internal static bool GetInvokerForHashClientRpc(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
		{
			return GetInvokerForHash(cmdHash, UNetInvokeType.ClientRpc, out invokeClass, out invokeFunction);
		}

		internal static bool GetInvokerForHashSyncList(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
		{
			return GetInvokerForHash(cmdHash, UNetInvokeType.SyncList, out invokeClass, out invokeFunction);
		}

		internal static bool GetInvokerForHashSyncEvent(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
		{
			return GetInvokerForHash(cmdHash, UNetInvokeType.SyncEvent, out invokeClass, out invokeFunction);
		}

		private static bool GetInvokerForHash(int cmdHash, UNetInvokeType invokeType, out Type invokeClass, out CmdDelegate invokeFunction)
		{
			Invoker value = null;
			if (!s_CmdHandlerDelegates.TryGetValue(cmdHash, out value))
			{
				if (LogFilter.logDev)
				{
					Debug.Log("GetInvokerForHash hash:" + cmdHash + " not found");
				}
				invokeClass = null;
				invokeFunction = null;
				return false;
			}
			if (value == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("GetInvokerForHash hash:" + cmdHash + " invoker null");
				}
				invokeClass = null;
				invokeFunction = null;
				return false;
			}
			if (value.invokeType != invokeType)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("GetInvokerForHash hash:" + cmdHash + " mismatched invokeType");
				}
				invokeClass = null;
				invokeFunction = null;
				return false;
			}
			invokeClass = value.invokeClass;
			invokeFunction = value.invokeFunction;
			return true;
		}

		internal static void DumpInvokers()
		{
			Debug.Log("DumpInvokers size:" + s_CmdHandlerDelegates.Count);
			foreach (KeyValuePair<int, Invoker> s_CmdHandlerDelegate in s_CmdHandlerDelegates)
			{
				Debug.Log("  Invoker:" + s_CmdHandlerDelegate.Value.invokeClass + ":" + s_CmdHandlerDelegate.Value.invokeFunction.GetMethodName() + " " + s_CmdHandlerDelegate.Value.invokeType + " " + s_CmdHandlerDelegate.Key);
			}
		}

		internal bool ContainsCommandDelegate(int cmdHash)
		{
			return s_CmdHandlerDelegates.ContainsKey(cmdHash);
		}

		internal bool InvokeCommandDelegate(int cmdHash, NetworkReader reader)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return false;
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			if (invoker.invokeType != 0)
			{
				return false;
			}
			if (GetType() != invoker.invokeClass && !GetType().IsSubclassOf(invoker.invokeClass))
			{
				return false;
			}
			invoker.invokeFunction(this, reader);
			return true;
		}

		internal bool InvokeRpcDelegate(int cmdHash, NetworkReader reader)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return false;
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			if (invoker.invokeType != UNetInvokeType.ClientRpc)
			{
				return false;
			}
			if (GetType() != invoker.invokeClass && !GetType().IsSubclassOf(invoker.invokeClass))
			{
				return false;
			}
			invoker.invokeFunction(this, reader);
			return true;
		}

		internal bool InvokeSyncEventDelegate(int cmdHash, NetworkReader reader)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return false;
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			if (invoker.invokeType != UNetInvokeType.SyncEvent)
			{
				return false;
			}
			invoker.invokeFunction(this, reader);
			return true;
		}

		internal bool InvokeSyncListDelegate(int cmdHash, NetworkReader reader)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return false;
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			if (invoker.invokeType != UNetInvokeType.SyncList)
			{
				return false;
			}
			if (GetType() != invoker.invokeClass)
			{
				return false;
			}
			invoker.invokeFunction(this, reader);
			return true;
		}

		internal static string GetCmdHashHandlerName(int cmdHash)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return cmdHash.ToString();
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			return invoker.invokeType + ":" + invoker.invokeFunction.GetMethodName();
		}

		private static string GetCmdHashPrefixName(int cmdHash, string prefix)
		{
			if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
			{
				return cmdHash.ToString();
			}
			Invoker invoker = s_CmdHandlerDelegates[cmdHash];
			string text = invoker.invokeFunction.GetMethodName();
			int num = text.IndexOf(prefix);
			if (num > -1)
			{
				text = text.Substring(prefix.Length);
			}
			return text;
		}

		internal static string GetCmdHashCmdName(int cmdHash)
		{
			return GetCmdHashPrefixName(cmdHash, "InvokeCmd");
		}

		internal static string GetCmdHashRpcName(int cmdHash)
		{
			return GetCmdHashPrefixName(cmdHash, "InvokeRpc");
		}

		internal static string GetCmdHashEventName(int cmdHash)
		{
			return GetCmdHashPrefixName(cmdHash, "InvokeSyncEvent");
		}

		internal static string GetCmdHashListName(int cmdHash)
		{
			return GetCmdHashPrefixName(cmdHash, "InvokeSyncList");
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SetSyncVarGameObject(GameObject newGameObject, ref GameObject gameObjectField, uint dirtyBit, ref NetworkInstanceId netIdField)
		{
			if (m_SyncVarGuard)
			{
				return;
			}
			NetworkInstanceId networkInstanceId = default(NetworkInstanceId);
			if (newGameObject != null)
			{
				NetworkIdentity component = newGameObject.GetComponent<NetworkIdentity>();
				if (component != null)
				{
					networkInstanceId = component.netId;
					if (networkInstanceId.IsEmpty() && LogFilter.logWarn)
					{
						Debug.LogWarning("SetSyncVarGameObject GameObject " + newGameObject + " has a zero netId. Maybe it is not spawned yet?");
					}
				}
			}
			NetworkInstanceId networkInstanceId2 = default(NetworkInstanceId);
			if (gameObjectField != null)
			{
				networkInstanceId2 = gameObjectField.GetComponent<NetworkIdentity>().netId;
			}
			if (networkInstanceId != networkInstanceId2)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("SetSyncVar GameObject " + GetType().Name + " bit [" + dirtyBit + "] netfieldId:" + networkInstanceId2 + "->" + networkInstanceId);
				}
				SetDirtyBit(dirtyBit);
				gameObjectField = newGameObject;
				netIdField = networkInstanceId;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SetSyncVar<T>(T value, ref T fieldValue, uint dirtyBit)
		{
			bool flag = false;
			if (value == null)
			{
				if (fieldValue != null)
				{
					flag = true;
				}
			}
			else
			{
				flag = !value.Equals(fieldValue);
			}
			if (flag)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("SetSyncVar " + GetType().Name + " bit [" + dirtyBit + "] " + fieldValue + "->" + value);
				}
				SetDirtyBit(dirtyBit);
				fieldValue = value;
			}
		}

		public void SetDirtyBit(uint dirtyBit)
		{
			m_SyncVarDirtyBits |= dirtyBit;
		}

		public void ClearAllDirtyBits()
		{
			m_LastSendTime = Time.time;
			m_SyncVarDirtyBits = 0u;
		}

		internal int GetDirtyChannel()
		{
			if (Time.time - m_LastSendTime > GetNetworkSendInterval() && m_SyncVarDirtyBits != 0)
			{
				return GetNetworkChannel();
			}
			return -1;
		}

		public virtual bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			if (!initialState)
			{
				writer.WritePackedUInt32(0u);
			}
			return false;
		}

		public virtual void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (!initialState)
			{
				reader.ReadPackedUInt32();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void PreStartClient()
		{
		}

		public virtual void OnNetworkDestroy()
		{
		}

		public virtual void OnStartServer()
		{
		}

		public virtual void OnStartClient()
		{
		}

		public virtual void OnStartLocalPlayer()
		{
		}

		public virtual void OnStartAuthority()
		{
		}

		public virtual void OnStopAuthority()
		{
		}

		public virtual bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
		{
			return false;
		}

		public virtual void OnSetLocalVisibility(bool vis)
		{
		}

		public virtual bool OnCheckObserver(NetworkConnection conn)
		{
			return true;
		}

		public virtual int GetNetworkChannel()
		{
			return 0;
		}

		public virtual float GetNetworkSendInterval()
		{
			return 0.1f;
		}
	}
}
