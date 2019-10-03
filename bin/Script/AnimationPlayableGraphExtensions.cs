using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
	public static class AnimationPlayableGraphExtensions
	{
		public static AnimationPlayableOutput CreateAnimationOutput(this PlayableGraph graph, string name, Animator target)
		{
			AnimationPlayableOutput result = default(AnimationPlayableOutput);
			if (!InternalCreateAnimationOutput(ref graph, name, out result.m_Output))
			{
				return AnimationPlayableOutput.Null;
			}
			result.target = target;
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool InternalCreateAnimationOutput(ref PlayableGraph graph, string name, out PlayableOutput output);

		internal static void SyncUpdateAndTimeMode(this PlayableGraph graph, Animator animator)
		{
			InternalSyncUpdateAndTimeMode(ref graph, animator);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void InternalSyncUpdateAndTimeMode(ref PlayableGraph graph, Animator animator);

		public static PlayableHandle CreateAnimationClipPlayable(this PlayableGraph graph, AnimationClip clip)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimationClipPlayable(ref graph, clip, ref handle))
			{
				return PlayableHandle.Null;
			}
			return handle;
		}

		private static bool InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref graph, clip, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle);

		[ExcludeFromDocs]
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph, int inputCount)
		{
			bool normalizeWeights = false;
			return graph.CreateAnimationMixerPlayable(inputCount, normalizeWeights);
		}

		[ExcludeFromDocs]
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph)
		{
			bool normalizeWeights = false;
			int inputCount = 0;
			return graph.CreateAnimationMixerPlayable(inputCount, normalizeWeights);
		}

		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph, [DefaultValue("0")] int inputCount, [DefaultValue("false")] bool normalizeWeights)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimationMixerPlayable(ref graph, inputCount, normalizeWeights, ref handle))
			{
				return PlayableHandle.Null;
			}
			handle.inputCount = inputCount;
			return handle;
		}

		private static bool InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref graph, inputCount, normalizeWeights, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle);

		public static PlayableHandle CreateAnimatorControllerPlayable(this PlayableGraph graph, RuntimeAnimatorController controller)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimatorControllerPlayable(ref graph, controller, ref handle))
			{
				return PlayableHandle.Null;
			}
			return handle;
		}

		private static bool InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref graph, controller, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle);

		internal static PlayableHandle CreateAnimationOffsetPlayable(this PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimationOffsetPlayable(ref graph, position, rotation, ref handle))
			{
				return PlayableHandle.Null;
			}
			handle.inputCount = inputCount;
			return handle;
		}

		private static bool InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref graph, ref position, ref rotation, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle);

		internal static PlayableHandle CreateAnimationMotionXToDeltaPlayable(this PlayableGraph graph)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimationMotionXToDeltaPlayable(ref graph, ref handle))
			{
				return PlayableHandle.Null;
			}
			handle.inputCount = 1;
			return handle;
		}

		private static bool InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref graph, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle);

		[ExcludeFromDocs]
		internal static PlayableHandle CreateAnimationLayerMixerPlayable(this PlayableGraph graph)
		{
			int inputCount = 0;
			return graph.CreateAnimationLayerMixerPlayable(inputCount);
		}

		internal static PlayableHandle CreateAnimationLayerMixerPlayable(this PlayableGraph graph, [DefaultValue("0")] int inputCount)
		{
			PlayableHandle handle = PlayableHandle.Null;
			if (!InternalCreateAnimationLayerMixerPlayable(ref graph, ref handle))
			{
				return PlayableHandle.Null;
			}
			handle.inputCount = inputCount;
			return handle;
		}

		private static bool InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref graph, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutput output);

		public static void DestroyOutput(this PlayableGraph graph, AnimationPlayableOutput output)
		{
			InternalDestroyOutput(ref graph, ref output.m_Output);
		}

		public static int GetAnimationOutputCount(this PlayableGraph graph)
		{
			return InternalAnimationOutputCount(ref graph);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int InternalAnimationOutputCount(ref PlayableGraph graph);

		public static AnimationPlayableOutput GetAnimationOutput(this PlayableGraph graph, int index)
		{
			AnimationPlayableOutput result = default(AnimationPlayableOutput);
			if (!InternalGetAnimationOutput(ref graph, index, out result.m_Output))
			{
				return AnimationPlayableOutput.Null;
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool InternalGetAnimationOutput(ref PlayableGraph graph, int index, out PlayableOutput output);
	}
}
