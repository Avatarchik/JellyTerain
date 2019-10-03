using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
	[RequiredByNativeCode]
	internal sealed class AnimationOffsetPlayable : AnimationPlayable
	{
		public Vector3 position
		{
			get
			{
				return GetPosition(ref handle);
			}
			set
			{
				SetPosition(ref handle, value);
			}
		}

		public Quaternion rotation
		{
			get
			{
				return GetRotation(ref handle);
			}
			set
			{
				SetRotation(ref handle, value);
			}
		}

		private static Vector3 GetPosition(ref PlayableHandle handle)
		{
			INTERNAL_CALL_GetPosition(ref handle, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPosition(ref PlayableHandle handle, out Vector3 value);

		private static void SetPosition(ref PlayableHandle handle, Vector3 value)
		{
			INTERNAL_CALL_SetPosition(ref handle, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetPosition(ref PlayableHandle handle, ref Vector3 value);

		private static Quaternion GetRotation(ref PlayableHandle handle)
		{
			INTERNAL_CALL_GetRotation(ref handle, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetRotation(ref PlayableHandle handle, out Quaternion value);

		private static void SetRotation(ref PlayableHandle handle, Quaternion value)
		{
			INTERNAL_CALL_SetRotation(ref handle, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetRotation(ref PlayableHandle handle, ref Quaternion value);
	}
}
