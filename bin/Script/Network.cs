using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class Network
	{
		public static string incomingPassword
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static NetworkLogLevel logLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static NetworkPlayer[] connections
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static NetworkPlayer player
		{
			get
			{
				NetworkPlayer result = default(NetworkPlayer);
				result.index = Internal_GetPlayer();
				return result;
			}
		}

		public static bool isClient
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isServer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static NetworkPeerType peerType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static float sendRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool isMessageQueueRunning
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static double time
		{
			get
			{
				Internal_GetTime(out double t);
				return t;
			}
		}

		public static int minimumAllocatableViewIDs
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("No longer needed. This is now explicitly set in the InitializeServer function call. It is implicitly set when calling Connect depending on if an IP/port combination is used (useNat=false) or a GUID is used(useNat=true).")]
		public static bool useNat
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static string natFacilitatorIP
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int natFacilitatorPort
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static string connectionTesterIP
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int connectionTesterPort
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int maxConnections
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static string proxyIP
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int proxyPort
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool useProxy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static string proxyPassword
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern NetworkConnectionError InitializeServer(int connections, int listenPort, bool useNat);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern NetworkConnectionError Internal_InitializeServerDeprecated(int connections, int listenPort);

		[Obsolete("Use the IntializeServer(connections, listenPort, useNat) function instead")]
		public static NetworkConnectionError InitializeServer(int connections, int listenPort)
		{
			return Internal_InitializeServerDeprecated(connections, listenPort);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void InitializeSecurity();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password);

		[ExcludeFromDocs]
		private static NetworkConnectionError Internal_ConnectToSingleIP(string IP, int remotePort, int localPort)
		{
			string password = "";
			return Internal_ConnectToSingleIP(IP, remotePort, localPort, password);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern NetworkConnectionError Internal_ConnectToGuid(string guid, string password);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort, [DefaultValue("\"\"")] string password);

		[ExcludeFromDocs]
		private static NetworkConnectionError Internal_ConnectToIPs(string[] IP, int remotePort, int localPort)
		{
			string password = "";
			return Internal_ConnectToIPs(IP, remotePort, localPort, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string IP, int remotePort)
		{
			string password = "";
			return Connect(IP, remotePort, password);
		}

		public static NetworkConnectionError Connect(string IP, int remotePort, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToSingleIP(IP, remotePort, 0, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string[] IPs, int remotePort)
		{
			string password = "";
			return Connect(IPs, remotePort, password);
		}

		public static NetworkConnectionError Connect(string[] IPs, int remotePort, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToIPs(IPs, remotePort, 0, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(string GUID)
		{
			string password = "";
			return Connect(GUID, password);
		}

		public static NetworkConnectionError Connect(string GUID, [DefaultValue("\"\"")] string password)
		{
			return Internal_ConnectToGuid(GUID, password);
		}

		[ExcludeFromDocs]
		public static NetworkConnectionError Connect(HostData hostData)
		{
			string password = "";
			return Connect(hostData, password);
		}

		public static NetworkConnectionError Connect(HostData hostData, [DefaultValue("\"\"")] string password)
		{
			if (hostData == null)
			{
				throw new NullReferenceException();
			}
			if (hostData.guid.Length > 0 && hostData.useNat)
			{
				return Connect(hostData.guid, password);
			}
			return Connect(hostData.ip, hostData.port, password);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Disconnect([DefaultValue("200")] int timeout);

		[ExcludeFromDocs]
		public static void Disconnect()
		{
			int timeout = 200;
			Disconnect(timeout);
		}

		public static void CloseConnection(NetworkPlayer target, bool sendDisconnectionNotification)
		{
			INTERNAL_CALL_CloseConnection(ref target, sendDisconnectionNotification);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CloseConnection(ref NetworkPlayer target, bool sendDisconnectionNotification);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetPlayer();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_AllocateViewID(out NetworkViewID viewID);

		public static NetworkViewID AllocateViewID()
		{
			Internal_AllocateViewID(out NetworkViewID viewID);
			return viewID;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object prefab, Vector3 position, Quaternion rotation, int group)
		{
			return INTERNAL_CALL_Instantiate(prefab, ref position, ref rotation, group);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object INTERNAL_CALL_Instantiate(Object prefab, ref Vector3 position, ref Quaternion rotation, int group);

		public static void Destroy(NetworkViewID viewID)
		{
			INTERNAL_CALL_Destroy(ref viewID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Destroy(ref NetworkViewID viewID);

		public static void Destroy(GameObject gameObject)
		{
			if (gameObject != null)
			{
				NetworkView component = gameObject.GetComponent<NetworkView>();
				if (component != null)
				{
					Destroy(component.viewID);
				}
				else
				{
					Debug.LogError("Couldn't destroy game object because no network view is attached to it.", gameObject);
				}
			}
		}

		public static void DestroyPlayerObjects(NetworkPlayer playerID)
		{
			INTERNAL_CALL_DestroyPlayerObjects(ref playerID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_DestroyPlayerObjects(ref NetworkPlayer playerID);

		private static void Internal_RemoveRPCs(NetworkPlayer playerID, NetworkViewID viewID, uint channelMask)
		{
			INTERNAL_CALL_Internal_RemoveRPCs(ref playerID, ref viewID, channelMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_RemoveRPCs(ref NetworkPlayer playerID, ref NetworkViewID viewID, uint channelMask);

		public static void RemoveRPCs(NetworkPlayer playerID)
		{
			Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, uint.MaxValue);
		}

		public static void RemoveRPCs(NetworkPlayer playerID, int group)
		{
			Internal_RemoveRPCs(playerID, NetworkViewID.unassigned, (uint)(1 << group));
		}

		public static void RemoveRPCs(NetworkViewID viewID)
		{
			Internal_RemoveRPCs(NetworkPlayer.unassigned, viewID, uint.MaxValue);
		}

		public static void RemoveRPCsInGroup(int group)
		{
			Internal_RemoveRPCs(NetworkPlayer.unassigned, NetworkViewID.unassigned, (uint)(1 << group));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetLevelPrefix(int prefix);

		public static int GetLastPing(NetworkPlayer player)
		{
			return INTERNAL_CALL_GetLastPing(ref player);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetLastPing(ref NetworkPlayer player);

		public static int GetAveragePing(NetworkPlayer player)
		{
			return INTERNAL_CALL_GetAveragePing(ref player);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetAveragePing(ref NetworkPlayer player);

		public static void SetReceivingEnabled(NetworkPlayer player, int group, bool enabled)
		{
			INTERNAL_CALL_SetReceivingEnabled(ref player, group, enabled);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetReceivingEnabled(ref NetworkPlayer player, int group, bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_SetSendingGlobal(int group, bool enabled);

		private static void Internal_SetSendingSpecific(NetworkPlayer player, int group, bool enabled)
		{
			INTERNAL_CALL_Internal_SetSendingSpecific(ref player, group, enabled);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_SetSendingSpecific(ref NetworkPlayer player, int group, bool enabled);

		public static void SetSendingEnabled(int group, bool enabled)
		{
			Internal_SetSendingGlobal(group, enabled);
		}

		public static void SetSendingEnabled(NetworkPlayer player, int group, bool enabled)
		{
			Internal_SetSendingSpecific(player, group, enabled);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_GetTime(out double t);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern ConnectionTesterStatus TestConnection([DefaultValue("false")] bool forceTest);

		[ExcludeFromDocs]
		public static ConnectionTesterStatus TestConnection()
		{
			bool forceTest = false;
			return TestConnection(forceTest);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern ConnectionTesterStatus TestConnectionNAT([DefaultValue("false")] bool forceTest);

		[ExcludeFromDocs]
		public static ConnectionTesterStatus TestConnectionNAT()
		{
			bool forceTest = false;
			return TestConnectionNAT(forceTest);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool HavePublicAddress();
	}
}
