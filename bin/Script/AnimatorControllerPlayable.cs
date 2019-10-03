using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
	[RequiredByNativeCode]
	public class AnimatorControllerPlayable : AnimationPlayable
	{
		public int layerCount => GetLayerCountInternal(ref handle);

		public int parameterCount => GetParameterCountInternal(ref handle);

		public static implicit operator PlayableHandle(AnimatorControllerPlayable b)
		{
			return b.handle;
		}

		private static RuntimeAnimatorController GetAnimatorControllerInternal(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetAnimatorControllerInternal(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RuntimeAnimatorController INTERNAL_CALL_GetAnimatorControllerInternal(ref PlayableHandle handle);

		public float GetFloat(string name)
		{
			return GetFloatString(ref handle, name);
		}

		public float GetFloat(int id)
		{
			return GetFloatID(ref handle, id);
		}

		public void SetFloat(string name, float value)
		{
			SetFloatString(ref handle, name, value);
		}

		public void SetFloat(int id, float value)
		{
			SetFloatID(ref handle, id, value);
		}

		public bool GetBool(string name)
		{
			return GetBoolString(ref handle, name);
		}

		public bool GetBool(int id)
		{
			return GetBoolID(ref handle, id);
		}

		public void SetBool(string name, bool value)
		{
			SetBoolString(ref handle, name, value);
		}

		public void SetBool(int id, bool value)
		{
			SetBoolID(ref handle, id, value);
		}

		public int GetInteger(string name)
		{
			return GetIntegerString(ref handle, name);
		}

		public int GetInteger(int id)
		{
			return GetIntegerID(ref handle, id);
		}

		public void SetInteger(string name, int value)
		{
			SetIntegerString(ref handle, name, value);
		}

		public void SetInteger(int id, int value)
		{
			SetIntegerID(ref handle, id, value);
		}

		public void SetTrigger(string name)
		{
			SetTriggerString(ref handle, name);
		}

		public void SetTrigger(int id)
		{
			SetTriggerID(ref handle, id);
		}

		public void ResetTrigger(string name)
		{
			ResetTriggerString(ref handle, name);
		}

		public void ResetTrigger(int id)
		{
			ResetTriggerID(ref handle, id);
		}

		public bool IsParameterControlledByCurve(string name)
		{
			return IsParameterControlledByCurveString(ref handle, name);
		}

		public bool IsParameterControlledByCurve(int id)
		{
			return IsParameterControlledByCurveID(ref handle, id);
		}

		private static int GetLayerCountInternal(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetLayerCountInternal(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetLayerCountInternal(ref PlayableHandle handle);

		private static string GetLayerNameInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetLayerNameInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern string INTERNAL_CALL_GetLayerNameInternal(ref PlayableHandle handle, int layerIndex);

		public string GetLayerName(int layerIndex)
		{
			return GetLayerNameInternal(ref handle, layerIndex);
		}

		private static int GetLayerIndexInternal(ref PlayableHandle handle, string layerName)
		{
			return INTERNAL_CALL_GetLayerIndexInternal(ref handle, layerName);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetLayerIndexInternal(ref PlayableHandle handle, string layerName);

		public int GetLayerIndex(string layerName)
		{
			return GetLayerIndexInternal(ref handle, layerName);
		}

		private static float GetLayerWeightInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetLayerWeightInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float INTERNAL_CALL_GetLayerWeightInternal(ref PlayableHandle handle, int layerIndex);

		public float GetLayerWeight(int layerIndex)
		{
			return GetLayerWeightInternal(ref handle, layerIndex);
		}

		private static void SetLayerWeightInternal(ref PlayableHandle handle, int layerIndex, float weight)
		{
			INTERNAL_CALL_SetLayerWeightInternal(ref handle, layerIndex, weight);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetLayerWeightInternal(ref PlayableHandle handle, int layerIndex, float weight);

		public void SetLayerWeight(int layerIndex, float weight)
		{
			SetLayerWeightInternal(ref handle, layerIndex, weight);
		}

		private static AnimatorStateInfo GetCurrentAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetCurrentAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorStateInfo INTERNAL_CALL_GetCurrentAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex);

		public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
		{
			return GetCurrentAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorStateInfo GetNextAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetNextAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorStateInfo INTERNAL_CALL_GetNextAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex);

		public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
		{
			return GetNextAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorTransitionInfo GetAnimatorTransitionInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetAnimatorTransitionInfoInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorTransitionInfo INTERNAL_CALL_GetAnimatorTransitionInfoInternal(ref PlayableHandle handle, int layerIndex);

		public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
		{
			return GetAnimatorTransitionInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorClipInfo[] GetCurrentAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetCurrentAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorClipInfo[] INTERNAL_CALL_GetCurrentAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex);

		public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex)
		{
			return GetCurrentAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			GetAnimatorClipInfoInternal(ref handle, layerIndex, isCurrent: true, clips);
		}

		public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			GetAnimatorClipInfoInternal(ref handle, layerIndex, isCurrent: false, clips);
		}

		private void GetAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex, bool isCurrent, object clips)
		{
			INTERNAL_CALL_GetAnimatorClipInfoInternal(this, ref handle, layerIndex, isCurrent, clips);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetAnimatorClipInfoInternal(AnimatorControllerPlayable self, ref PlayableHandle handle, int layerIndex, bool isCurrent, object clips);

		private static int GetAnimatorClipInfoCountInternal(ref PlayableHandle handle, int layerIndex, bool current)
		{
			return INTERNAL_CALL_GetAnimatorClipInfoCountInternal(ref handle, layerIndex, current);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetAnimatorClipInfoCountInternal(ref PlayableHandle handle, int layerIndex, bool current);

		public int GetCurrentAnimatorClipInfoCount(int layerIndex)
		{
			return GetAnimatorClipInfoCountInternal(ref handle, layerIndex, current: true);
		}

		public int GetNextAnimatorClipInfoCount(int layerIndex)
		{
			return GetAnimatorClipInfoCountInternal(ref handle, layerIndex, current: false);
		}

		private static AnimatorClipInfo[] GetNextAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_GetNextAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorClipInfo[] INTERNAL_CALL_GetNextAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex);

		public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex)
		{
			return GetNextAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		internal string ResolveHash(int hash)
		{
			return ResolveHashInternal(ref handle, hash);
		}

		private static string ResolveHashInternal(ref PlayableHandle handle, int hash)
		{
			return INTERNAL_CALL_ResolveHashInternal(ref handle, hash);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern string INTERNAL_CALL_ResolveHashInternal(ref PlayableHandle handle, int hash);

		private static bool IsInTransitionInternal(ref PlayableHandle handle, int layerIndex)
		{
			return INTERNAL_CALL_IsInTransitionInternal(ref handle, layerIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsInTransitionInternal(ref PlayableHandle handle, int layerIndex);

		public bool IsInTransition(int layerIndex)
		{
			return IsInTransitionInternal(ref handle, layerIndex);
		}

		private static int GetParameterCountInternal(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetParameterCountInternal(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetParameterCountInternal(ref PlayableHandle handle);

		private static AnimatorControllerParameter[] GetParametersArrayInternal(ref PlayableHandle handle)
		{
			return INTERNAL_CALL_GetParametersArrayInternal(ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern AnimatorControllerParameter[] INTERNAL_CALL_GetParametersArrayInternal(ref PlayableHandle handle);

		public AnimatorControllerParameter GetParameter(int index)
		{
			AnimatorControllerParameter[] parametersArrayInternal = GetParametersArrayInternal(ref handle);
			if (index < 0 && index >= parametersArrayInternal.Length)
			{
				throw new IndexOutOfRangeException("index");
			}
			return parametersArrayInternal[index];
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int StringToHash(string name);

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		public void CrossFadeInFixedTime(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			CrossFadeInFixedTimeInternal(ref handle, StringToHash(stateName), transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		[ExcludeFromDocs]
		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float fixedTime);

		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			CrossFade(stateName, transitionDuration, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			CrossFade(stateName, transitionDuration, layer, normalizedTime);
		}

		public void CrossFade(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			CrossFadeInternal(ref handle, StringToHash(stateName), transitionDuration, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
		}

		public void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float normalizedTime);

		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName, int layer)
		{
			float fixedTime = float.NegativeInfinity;
			PlayInFixedTime(stateName, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName)
		{
			float fixedTime = float.NegativeInfinity;
			int layer = -1;
			PlayInFixedTime(stateName, layer, fixedTime);
		}

		public void PlayInFixedTime(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			PlayInFixedTimeInternal(ref handle, StringToHash(stateName), layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash, int layer)
		{
			float fixedTime = float.NegativeInfinity;
			PlayInFixedTime(stateNameHash, layer, fixedTime);
		}

		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash)
		{
			float fixedTime = float.NegativeInfinity;
			int layer = -1;
			PlayInFixedTime(stateNameHash, layer, fixedTime);
		}

		public void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			PlayInFixedTimeInternal(ref handle, stateNameHash, layer, fixedTime);
		}

		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, fixedTime);
		}

		[ExcludeFromDocs]
		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, int layer)
		{
			float fixedTime = float.NegativeInfinity;
			INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, fixedTime);
		}

		[ExcludeFromDocs]
		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash)
		{
			float fixedTime = float.NegativeInfinity;
			int layer = -1;
			INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, fixedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, int layer, float fixedTime);

		[ExcludeFromDocs]
		public void Play(string stateName, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			Play(stateName, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void Play(string stateName)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			Play(stateName, layer, normalizedTime);
		}

		public void Play(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			PlayInternal(ref handle, StringToHash(stateName), layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void Play(int stateNameHash, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			Play(stateNameHash, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		public void Play(int stateNameHash)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			Play(stateNameHash, layer, normalizedTime);
		}

		public void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			PlayInternal(ref handle, stateNameHash, layer, normalizedTime);
		}

		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash, int layer)
		{
			float normalizedTime = float.NegativeInfinity;
			INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, normalizedTime);
		}

		[ExcludeFromDocs]
		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash)
		{
			float normalizedTime = float.NegativeInfinity;
			int layer = -1;
			INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, normalizedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_PlayInternal(ref PlayableHandle handle, int stateNameHash, int layer, float normalizedTime);

		public bool HasState(int layerIndex, int stateID)
		{
			return HasStateInternal(ref handle, layerIndex, stateID);
		}

		private static bool HasStateInternal(ref PlayableHandle handle, int layerIndex, int stateID)
		{
			return INTERNAL_CALL_HasStateInternal(ref handle, layerIndex, stateID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_HasStateInternal(ref PlayableHandle handle, int layerIndex, int stateID);

		private static void SetFloatString(ref PlayableHandle handle, string name, float value)
		{
			INTERNAL_CALL_SetFloatString(ref handle, name, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetFloatString(ref PlayableHandle handle, string name, float value);

		private static void SetFloatID(ref PlayableHandle handle, int id, float value)
		{
			INTERNAL_CALL_SetFloatID(ref handle, id, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetFloatID(ref PlayableHandle handle, int id, float value);

		private static float GetFloatString(ref PlayableHandle handle, string name)
		{
			return INTERNAL_CALL_GetFloatString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float INTERNAL_CALL_GetFloatString(ref PlayableHandle handle, string name);

		private static float GetFloatID(ref PlayableHandle handle, int id)
		{
			return INTERNAL_CALL_GetFloatID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float INTERNAL_CALL_GetFloatID(ref PlayableHandle handle, int id);

		private static void SetBoolString(ref PlayableHandle handle, string name, bool value)
		{
			INTERNAL_CALL_SetBoolString(ref handle, name, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetBoolString(ref PlayableHandle handle, string name, bool value);

		private static void SetBoolID(ref PlayableHandle handle, int id, bool value)
		{
			INTERNAL_CALL_SetBoolID(ref handle, id, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetBoolID(ref PlayableHandle handle, int id, bool value);

		private static bool GetBoolString(ref PlayableHandle handle, string name)
		{
			return INTERNAL_CALL_GetBoolString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_GetBoolString(ref PlayableHandle handle, string name);

		private static bool GetBoolID(ref PlayableHandle handle, int id)
		{
			return INTERNAL_CALL_GetBoolID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_GetBoolID(ref PlayableHandle handle, int id);

		private static void SetIntegerString(ref PlayableHandle handle, string name, int value)
		{
			INTERNAL_CALL_SetIntegerString(ref handle, name, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetIntegerString(ref PlayableHandle handle, string name, int value);

		private static void SetIntegerID(ref PlayableHandle handle, int id, int value)
		{
			INTERNAL_CALL_SetIntegerID(ref handle, id, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetIntegerID(ref PlayableHandle handle, int id, int value);

		private static int GetIntegerString(ref PlayableHandle handle, string name)
		{
			return INTERNAL_CALL_GetIntegerString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetIntegerString(ref PlayableHandle handle, string name);

		private static int GetIntegerID(ref PlayableHandle handle, int id)
		{
			return INTERNAL_CALL_GetIntegerID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetIntegerID(ref PlayableHandle handle, int id);

		private static void SetTriggerString(ref PlayableHandle handle, string name)
		{
			INTERNAL_CALL_SetTriggerString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetTriggerString(ref PlayableHandle handle, string name);

		private static void SetTriggerID(ref PlayableHandle handle, int id)
		{
			INTERNAL_CALL_SetTriggerID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetTriggerID(ref PlayableHandle handle, int id);

		private static void ResetTriggerString(ref PlayableHandle handle, string name)
		{
			INTERNAL_CALL_ResetTriggerString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetTriggerString(ref PlayableHandle handle, string name);

		private static void ResetTriggerID(ref PlayableHandle handle, int id)
		{
			INTERNAL_CALL_ResetTriggerID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetTriggerID(ref PlayableHandle handle, int id);

		private static bool IsParameterControlledByCurveString(ref PlayableHandle handle, string name)
		{
			return INTERNAL_CALL_IsParameterControlledByCurveString(ref handle, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsParameterControlledByCurveString(ref PlayableHandle handle, string name);

		private static bool IsParameterControlledByCurveID(ref PlayableHandle handle, int id)
		{
			return INTERNAL_CALL_IsParameterControlledByCurveID(ref handle, id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsParameterControlledByCurveID(ref PlayableHandle handle, int id);
	}
}
