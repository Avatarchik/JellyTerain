using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal sealed class PlayerConnectionInternal : IPlayerEditorConnectionNative
	{
		void IPlayerEditorConnectionNative.SendMessage(Guid messageId, byte[] data, int playerId)
		{
			if (messageId == Guid.Empty)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			SendMessage(messageId.ToString("N"), data, playerId);
		}

		void IPlayerEditorConnectionNative.RegisterInternal(Guid messageId)
		{
			RegisterInternal(messageId.ToString("N"));
		}

		void IPlayerEditorConnectionNative.UnregisterInternal(Guid messageId)
		{
			UnregisterInternal(messageId.ToString("N"));
		}

		void IPlayerEditorConnectionNative.Initialize()
		{
			Initialize();
		}

		bool IPlayerEditorConnectionNative.IsConnected()
		{
			return IsConnected();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsConnected();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Initialize();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void RegisterInternal(string messageId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void UnregisterInternal(string messageId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SendMessage(string messageId, byte[] data, int playerId);
	}
}
