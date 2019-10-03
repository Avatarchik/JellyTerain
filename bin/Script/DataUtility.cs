using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Sprites
{
	public sealed class DataUtility
	{
		public static Vector4 GetInnerUV(Sprite sprite)
		{
			INTERNAL_CALL_GetInnerUV(sprite, out Vector4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetInnerUV(Sprite sprite, out Vector4 value);

		public static Vector4 GetOuterUV(Sprite sprite)
		{
			INTERNAL_CALL_GetOuterUV(sprite, out Vector4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetOuterUV(Sprite sprite, out Vector4 value);

		public static Vector4 GetPadding(Sprite sprite)
		{
			INTERNAL_CALL_GetPadding(sprite, out Vector4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPadding(Sprite sprite, out Vector4 value);

		public static Vector2 GetMinSize(Sprite sprite)
		{
			Internal_GetMinSize(sprite, out Vector2 output);
			return output;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_GetMinSize(Sprite sprite, out Vector2 output);
	}
}
