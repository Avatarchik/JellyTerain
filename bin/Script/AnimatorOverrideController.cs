using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class AnimatorOverrideController : RuntimeAnimatorController
	{
		public RuntimeAnimatorController runtimeAnimatorController
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public AnimationClip this[string name]
		{
			get
			{
				return Internal_GetClipByName(name, returnEffectiveClip: true);
			}
			set
			{
				Internal_SetClipByName(name, value);
			}
		}

		public AnimationClip this[AnimationClip clip]
		{
			get
			{
				return Internal_GetClip(clip, returnEffectiveClip: true);
			}
			set
			{
				Internal_SetClip(clip, value);
			}
		}

		public int overridesCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("clips property is deprecated. Use AnimatorOverrideController.GetOverrides and AnimatorOverrideController.ApplyOverrides instead.")]
		public AnimationClipPair[] clips
		{
			get
			{
				int overridesCount = this.overridesCount;
				AnimationClipPair[] array = new AnimationClipPair[overridesCount];
				for (int i = 0; i < overridesCount; i++)
				{
					array[i] = new AnimationClipPair();
					array[i].originalClip = Internal_GetOriginalClip(i);
					array[i].overrideClip = Internal_GetOverrideClip(array[i].originalClip);
				}
				return array;
			}
			set
			{
				for (int i = 0; i < value.Length; i++)
				{
					Internal_SetClip(value[i].originalClip, value[i].overrideClip, notify: false);
				}
				SendNotification();
			}
		}

		public AnimatorOverrideController()
		{
			Internal_CreateAnimatorOverrideController(this, null);
		}

		public AnimatorOverrideController(RuntimeAnimatorController controller)
		{
			Internal_CreateAnimatorOverrideController(this, controller);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_CreateAnimatorOverrideController([Writable] AnimatorOverrideController self, RuntimeAnimatorController controller);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetClipByName(string name, AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern AnimationClip Internal_GetClip(AnimationClip originalClip, bool returnEffectiveClip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip, [DefaultValue("true")] bool notify);

		[ExcludeFromDocs]
		private void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip)
		{
			bool notify = true;
			Internal_SetClip(originalClip, overrideClip, notify);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SendNotification();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern AnimationClip Internal_GetOriginalClip(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern AnimationClip Internal_GetOverrideClip(AnimationClip originalClip);

		public void GetOverrides(List<KeyValuePair<AnimationClip, AnimationClip>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			int overridesCount = this.overridesCount;
			if (overrides.Capacity < overridesCount)
			{
				overrides.Capacity = overridesCount;
			}
			overrides.Clear();
			for (int i = 0; i < overridesCount; i++)
			{
				AnimationClip animationClip = Internal_GetOriginalClip(i);
				overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, Internal_GetOverrideClip(animationClip)));
			}
		}

		public void ApplyOverrides(IList<KeyValuePair<AnimationClip, AnimationClip>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				Internal_SetClip(overrides[i].Key, overrides[i].Value, notify: false);
			}
			SendNotification();
		}
	}
}
