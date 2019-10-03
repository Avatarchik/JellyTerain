using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
	[RequiredByNativeCode]
	public sealed class AnimationClipPlayable : AnimationPlayable
	{
		public AnimationClip clip => GetAnimationClip(ref handle);

		public float speed
		{
			get
			{
				return GetSpeed(ref handle);
			}
			set
			{
				SetSpeed(ref handle, value);
			}
		}

		public bool applyFootIK
		{
			get
			{
				return GetApplyFootIK(ref handle);
			}
			set
			{
				SetApplyFootIK(ref handle, value);
			}
		}

		internal bool removeStartOffset
		{
			get
			{
				return GetRemoveStartOffset(ref handle);
			}
			set
			{
				SetRemoveStartOffset(ref handle, value);
			}
		}

		private static AnimationClip GetAnimationClip(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetAnimationClip(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimationClip INTERNAL_CALL_GetAnimationClip(ref PlayableHandle handle);

		private static float GetSpeed(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetSpeed(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float INTERNAL_CALL_GetSpeed(ref PlayableHandle handle);

		private static void SetSpeed(ref PlayableHandle handle, float value)
		{
			INTERNAL_CALL_SetSpeed(ref handle, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetSpeed(ref PlayableHandle handle, float value);

		private static bool GetApplyFootIK(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetApplyFootIK(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_GetApplyFootIK(ref PlayableHandle handle);

		private static void SetApplyFootIK(ref PlayableHandle handle, bool value)
		{
			INTERNAL_CALL_SetApplyFootIK(ref handle, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetApplyFootIK(ref PlayableHandle handle, bool value);

		private static bool GetRemoveStartOffset(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetRemoveStartOffset(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_GetRemoveStartOffset(ref PlayableHandle handle);

		private static void SetRemoveStartOffset(ref PlayableHandle handle, bool value)
		{
			INTERNAL_CALL_SetRemoveStartOffset(ref handle, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetRemoveStartOffset(ref PlayableHandle handle, bool value);
	}
}
