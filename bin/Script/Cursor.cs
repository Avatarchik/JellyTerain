using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Cursor
	{
		public static bool visible
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static CursorLockMode lockState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		private static void SetCursor(Texture2D texture, CursorMode cursorMode)
		{
			SetCursor(texture, Vector2.zero, cursorMode);
		}

		public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
		{
			INTERNAL_CALL_SetCursor(texture, ref hotspot, cursorMode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetCursor(Texture2D texture, ref Vector2 hotspot, CursorMode cursorMode);
	}
}
