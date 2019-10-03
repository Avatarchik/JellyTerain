using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class NetworkView : Behaviour
	{
		public Component observed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public NetworkStateSynchronization stateSynchronization
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public NetworkViewID viewID
		{
			get
			{
				Internal_GetViewID(out NetworkViewID viewID);
				return viewID;
			}
			set
			{
				Internal_SetViewID(value);
			}
		}

		public int group
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool isMine => viewID.isMine;

		public NetworkPlayer owner => viewID.owner;

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_RPC(NetworkView view, string name, RPCMode mode, object[] args);

		private static void Internal_RPC_Target(NetworkView view, string name, NetworkPlayer target, object[] args)
		{
			INTERNAL_CALL_Internal_RPC_Target(view, name, ref target, args);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_RPC_Target(NetworkView view, string name, ref NetworkPlayer target, object[] args);

		[Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
		public void RPC(string name, RPCMode mode, params object[] args)
		{
			Internal_RPC(this, name, mode, args);
		}

		[Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
		public void RPC(string name, NetworkPlayer target, params object[] args)
		{
			Internal_RPC_Target(this, name, target, args);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_GetViewID(out NetworkViewID viewID);

		private void Internal_SetViewID(NetworkViewID viewID)
		{
			INTERNAL_CALL_Internal_SetViewID(this, ref viewID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_SetViewID(NetworkView self, ref NetworkViewID viewID);

		public bool SetScope(NetworkPlayer player, bool relevancy)
		{
			return INTERNAL_CALL_SetScope(this, ref player, relevancy);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_SetScope(NetworkView self, ref NetworkPlayer player, bool relevancy);

		public static NetworkView Find(NetworkViewID viewID)
		{
			return INTERNAL_CALL_Find(ref viewID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern NetworkView INTERNAL_CALL_Find(ref NetworkViewID viewID);
	}
}
