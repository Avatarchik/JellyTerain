using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Director;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public sealed class Animator : Behaviour
	{
		public bool isOptimizable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isHuman
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool hasRootMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal bool isRootPositionOrRotationControlledByCurves
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float humanScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isInitialized
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Vector3 deltaPosition
		{
			get
			{
				INTERNAL_get_deltaPosition(out Vector3 value);
				return value;
			}
		}

		public Quaternion deltaRotation
		{
			get
			{
				INTERNAL_get_deltaRotation(out Quaternion value);
				return value;
			}
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
		}

		public Vector3 angularVelocity
		{
			get
			{
				INTERNAL_get_angularVelocity(out Vector3 value);
				return value;
			}
		}

		public Vector3 rootPosition
		{
			get
			{
				INTERNAL_get_rootPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_rootPosition(ref value);
			}
		}

		public Quaternion rootRotation
		{
			get
			{
				INTERNAL_get_rootRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_rootRotation(ref value);
			}
		}

		public bool applyRootMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool linearVelocityBlending
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("Use Animator.updateMode instead")]
		public bool animatePhysics
		{
			get
			{
				return updateMode == AnimatorUpdateMode.AnimatePhysics;
			}
			set
			{
				updateMode = (value ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
			}
		}

		public AnimatorUpdateMode updateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool hasTransformHierarchy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal bool allowConstantClipSamplingOptimization
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float gravityWeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Vector3 bodyPosition
		{
			get
			{
				CheckIfInIKPass();
				return GetBodyPositionInternal();
			}
			set
			{
				CheckIfInIKPass();
				SetBodyPositionInternal(value);
			}
		}

		public Quaternion bodyRotation
		{
			get
			{
				CheckIfInIKPass();
				return GetBodyRotationInternal();
			}
			set
			{
				CheckIfInIKPass();
				SetBodyRotationInternal(value);
			}
		}

		public bool stabilizeFeet
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int layerCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public AnimatorControllerParameter[] parameters
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int parameterCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float feetPivotActive
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float pivotWeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Vector3 pivotPosition
		{
			get
			{
				INTERNAL_get_pivotPosition(out Vector3 value);
				return value;
			}
		}

		public bool isMatchingTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float speed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3 targetPosition
		{
			get
			{
				INTERNAL_get_targetPosition(out Vector3 value);
				return value;
			}
		}

		public Quaternion targetRotation
		{
			get
			{
				INTERNAL_get_targetRotation(out Quaternion value);
				return value;
			}
		}

		internal Transform avatarRoot
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public AnimatorCullingMode cullingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float playbackTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float recorderStartTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float recorderStopTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public AnimatorRecorderMode recorderMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public RuntimeAnimatorController runtimeAnimatorController
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool hasBoundPlayables
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Avatar avatar
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public PlayableGraph playableGraph
		{
			get
			{
				PlayableGraph graph = default(PlayableGraph);
				InternalGetCurrentGraph(ref graph);
				return graph;
			}
		}

		public bool layersAffectMassCenter
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float leftFeetBottomHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float rightFeetBottomHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool logWarnings
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool fireEvents
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float GetFloat(string name)
		{
			return GetFloatString(name);
		}

		public float GetFloat(int id)
		{
			return GetFloatID(id);
		}

		public void SetFloat(string name, float value)
		{
			SetFloatString(name, value);
		}

		public void SetFloat(string name, float value, float dampTime, float deltaTime)
		{
			SetFloatStringDamp(name, value, dampTime, deltaTime);
		}

		public void SetFloat(int id, float value)
		{
			SetFloatID(id, value);
		}

		public void SetFloat(int id, float value, float dampTime, float deltaTime)
		{
			SetFloatIDDamp(id, value, dampTime, deltaTime);
		}

		public bool GetBool(string name)
		{
			return GetBoolString(name);
		}

		public bool GetBool(int id)
		{
			return GetBoolID(id);
		}

		public void SetBool(string name, bool value)
		{
			SetBoolString(name, value);
		}

		public void SetBool(int id, bool value)
		{
			SetBoolID(id, value);
		}

		public int GetInteger(string name)
		{
			return GetIntegerString(name);
		}

		public int GetInteger(int id)
		{
			return GetIntegerID(id);
		}

		public void SetInteger(string name, int value)
		{
			SetIntegerString(name, value);
		}

		public void SetInteger(int id, int value)
		{
			SetIntegerID(id, value);
		}

		public void SetTrigger(string name)
		{
			SetTriggerString(name);
		}

		public void SetTrigger(int id)
		{
			SetTriggerID(id);
		}

		public void ResetTrigger(string name)
		{
			ResetTriggerString(name);
		}

		public void ResetTrigger(int id)
		{
			ResetTriggerID(id);
		}

		public bool IsParameterControlledByCurve(string name)
		{
			return IsParameterControlledByCurveString(name);
		}

		public bool IsParameterControlledByCurve(int id)
		{
			return IsParameterControlledByCurveID(id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_deltaPosition(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_deltaRotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_velocity(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularVelocity(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_rootPosition(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_rootPosition(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_rootRotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_rootRotation(ref Quaternion value);

		internal Vector3 GetBodyPositionInternal()
		{
			INTERNAL_CALL_GetBodyPositionInternal(this, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetBodyPositionInternal(Animator self, out Vector3 value);

		internal void SetBodyPositionInternal(Vector3 bodyPosition)
		{
			INTERNAL_CALL_SetBodyPositionInternal(this, ref bodyPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetBodyPositionInternal(Animator self, ref Vector3 bodyPosition);

		internal Quaternion GetBodyRotationInternal()
		{
			INTERNAL_CALL_GetBodyRotationInternal(this, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetBodyRotationInternal(Animator self, out Quaternion value);

		internal void SetBodyRotationInternal(Quaternion bodyRotation)
		{
			INTERNAL_CALL_SetBodyRotationInternal(this, ref bodyRotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetBodyRotationInternal(Animator self, ref Quaternion bodyRotation);

		public Vector3 GetIKPosition(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKPositionInternal(goal);
		}

		internal Vector3 GetIKPositionInternal(AvatarIKGoal goal)
		{
			INTERNAL_CALL_GetIKPositionInternal(this, goal, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetIKPositionInternal(Animator self, AvatarIKGoal goal, out Vector3 value);

		public void SetIKPosition(AvatarIKGoal goal, Vector3 goalPosition)
		{
			CheckIfInIKPass();
			SetIKPositionInternal(goal, goalPosition);
		}

		internal void SetIKPositionInternal(AvatarIKGoal goal, Vector3 goalPosition)
		{
			INTERNAL_CALL_SetIKPositionInternal(this, goal, ref goalPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetIKPositionInternal(Animator self, AvatarIKGoal goal, ref Vector3 goalPosition);

		public Quaternion GetIKRotation(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKRotationInternal(goal);
		}

		internal Quaternion GetIKRotationInternal(AvatarIKGoal goal)
		{
			INTERNAL_CALL_GetIKRotationInternal(this, goal, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetIKRotationInternal(Animator self, AvatarIKGoal goal, out Quaternion value);

		public void SetIKRotation(AvatarIKGoal goal, Quaternion goalRotation)
		{
			CheckIfInIKPass();
			SetIKRotationInternal(goal, goalRotation);
		}

		internal void SetIKRotationInternal(AvatarIKGoal goal, Quaternion goalRotation)
		{
			INTERNAL_CALL_SetIKRotationInternal(this, goal, ref goalRotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetIKRotationInternal(Animator self, AvatarIKGoal goal, ref Quaternion goalRotation);

		public float GetIKPositionWeight(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKPositionWeightInternal(goal);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern float GetIKPositionWeightInternal(AvatarIKGoal goal);

		public void SetIKPositionWeight(AvatarIKGoal goal, float value)
		{
			CheckIfInIKPass();
			SetIKPositionWeightInternal(goal, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetIKPositionWeightInternal(AvatarIKGoal goal, float value);

		public float GetIKRotationWeight(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKRotationWeightInternal(goal);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern float GetIKRotationWeightInternal(AvatarIKGoal goal);

		public void SetIKRotationWeight(AvatarIKGoal goal, float value)
		{
			CheckIfInIKPass();
			SetIKRotationWeightInternal(goal, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetIKRotationWeightInternal(AvatarIKGoal goal, float value);

		public Vector3 GetIKHintPosition(AvatarIKHint hint)
		{
			CheckIfInIKPass();
			return GetIKHintPositionInternal(hint);
		}

		internal Vector3 GetIKHintPositionInternal(AvatarIKHint hint)
		{
			INTERNAL_CALL_GetIKHintPositionInternal(this, hint, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetIKHintPositionInternal(Animator self, AvatarIKHint hint, out Vector3 value);

		public void SetIKHintPosition(AvatarIKHint hint, Vector3 hintPosition)
		{
			CheckIfInIKPass();
			SetIKHintPositionInternal(hint, hintPosition);
		}

		internal void SetIKHintPositionInternal(AvatarIKHint hint, Vector3 hintPosition)
		{
			INTERNAL_CALL_SetIKHintPositionInternal(this, hint, ref hintPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetIKHintPositionInternal(Animator self, AvatarIKHint hint, ref Vector3 hintPosition);

		public float GetIKHintPositionWeight(AvatarIKHint hint)
		{
			CheckIfInIKPass();
			return GetHintWeightPositionInternal(hint);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern float GetHintWeightPositionInternal(AvatarIKHint hint);

		public void SetIKHintPositionWeight(AvatarIKHint hint, float value)
		{
			CheckIfInIKPass();
			SetIKHintPositionWeightInternal(hint, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetIKHintPositionWeightInternal(AvatarIKHint hint, float value);

		public void SetLookAtPosition(Vector3 lookAtPosition)
		{
			CheckIfInIKPass();
			SetLookAtPositionInternal(lookAtPosition);
		}

		internal void SetLookAtPositionInternal(Vector3 lookAtPosition)
		{
			INTERNAL_CALL_SetLookAtPositionInternal(this, ref lookAtPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetLookAtPositionInternal(Animator self, ref Vector3 lookAtPosition);

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		public void SetLookAtWeight(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight)
		{
			CheckIfInIKPass();
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetLookAtWeightInternal(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight);

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		public void SetBoneLocalRotation(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			CheckIfInIKPass();
			SetBoneLocalRotationInternal((int)humanBoneId, rotation);
		}

		internal void SetBoneLocalRotationInternal(int humanBoneId, Quaternion rotation)
		{
			INTERNAL_CALL_SetBoneLocalRotationInternal(this, humanBoneId, ref rotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetBoneLocalRotationInternal(Animator self, int humanBoneId, ref Quaternion rotation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern ScriptableObject GetBehaviour(Type type);

		public T GetBehaviour<T>() where T : StateMachineBehaviour
		{
			return GetBehaviour(typeof(T)) as T;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern ScriptableObject[] GetBehaviours(Type type);

		internal static T[] ConvertStateMachineBehaviour<T>(ScriptableObject[] rawObjects) where T : StateMachineBehaviour
		{
			if (rawObjects == null)
			{
				return null;
			}
			T[] array = new T[rawObjects.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)rawObjects[i];
			}
			return array;
		}

		public T[] GetBehaviours<T>() where T : StateMachineBehaviour
		{
			return ConvertStateMachineBehaviour<T>(GetBehaviours(typeof(T)));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetLayerName(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetLayerIndex(string layerName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float GetLayerWeight(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetLayerWeight(int layerIndex, float weight);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetCurrentAnimatorClipInfoCount(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex);

		public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			GetAnimatorClipInfoInternal(layerIndex, isCurrent: true, clips);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetAnimatorClipInfoInternal(int layerIndex, bool isCurrent, object clips);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetNextAnimatorClipInfoCount(int layerIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex);

		public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			GetAnimatorClipInfoInternal(layerIndex, isCurrent: false, clips);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsInTransition(int layerIndex);

		public AnimatorControllerParameter GetParameter(int index)
		{
			AnimatorControllerParameter[] parameters = this.parameters;
			if (index < 0 && index >= this.parameters.Length)
			{
				throw new IndexOutOfRangeException("Index must be between 0 and " + this.parameters.Length);
			}
			return parameters[index];
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_pivotPosition(out Vector3 value);

		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime, [DefaultValue("1")] float targetNormalizedTime)
		{
			INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		[ExcludeFromDocs]
		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime)
		{
			float targetNormalizedTime = 1f;
			INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_MatchTarget(Animator self, ref Vector3 matchPosition, ref Quaternion matchRotation, AvatarTarget targetBodyPart, ref MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InterruptMatchTarget([DefaultValue("true")] bool completeMatch);

		[ExcludeFromDocs]
		public void InterruptMatchTarget()
		{
			bool completeMatch = true;
			InterruptMatchTarget(completeMatch);
		}

		[Obsolete("ForceStateNormalizedTime is deprecated. Please use Play or CrossFade instead.")]
		public void ForceStateNormalizedTime(float normalizedTime)
		{
			Play(0, 0, normalizedTime);
		}

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
			CrossFadeInFixedTime(StringToHash(stateName), transitionDuration, layer, fixedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime);

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
			CrossFade(StringToHash(stateName), transitionDuration, layer, normalizedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

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
			PlayInFixedTime(StringToHash(stateName), layer, fixedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime);

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
			Play(StringToHash(stateName), layer, normalizedTime);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

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

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetTarget(AvatarTarget targetIndex, float targetNormalizedTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetPosition(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetRotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("use mask and layers to control subset of transfroms in a skeleton", true)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsControlled(Transform transform);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern bool IsBoneTransform(Transform transform);

		public Transform GetBoneTransform(HumanBodyBones humanBoneId)
		{
			return GetBoneTransformInternal((int)humanBoneId);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern Transform GetBoneTransformInternal(int humanBoneId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void StartPlayback();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void StopPlayback();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void StartRecording(int frameCount);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void StopRecording();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool HasState(int layerIndex, int stateID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int StringToHash(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern string GetStats();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalGetCurrentGraph(ref PlayableGraph graph);

		private void CheckIfInIKPass()
		{
			if (logWarnings && !CheckIfInIKPassInternal())
			{
				Debug.LogWarning("Setting and getting Body Position/Rotation, IK Goals, Lookat and BoneLocalRotation should only be done in OnAnimatorIK or OnStateIK");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool CheckIfInIKPassInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatString(string name, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatID(int id, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern float GetFloatString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern float GetFloatID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetBoolString(string name, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetBoolID(int id, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool GetBoolString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool GetBoolID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetIntegerString(string name, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetIntegerID(int id, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int GetIntegerString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int GetIntegerID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTriggerString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTriggerID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void ResetTriggerString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void ResetTriggerID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool IsParameterControlledByCurveString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool IsParameterControlledByCurveID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatStringDamp(string name, float value, float dampTime, float deltaTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatIDDamp(int id, float value, float dampTime, float deltaTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Update(float deltaTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Rebind();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ApplyBuiltinRootMotion();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern string ResolveHash(int hash);

		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(string name)
		{
			return Vector3.zero;
		}

		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(int id)
		{
			return Vector3.zero;
		}

		[Obsolete("SetVector is deprecated.")]
		public void SetVector(string name, Vector3 value)
		{
		}

		[Obsolete("SetVector is deprecated.")]
		public void SetVector(int id, Vector3 value)
		{
		}

		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(string name)
		{
			return Quaternion.identity;
		}

		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(int id)
		{
			return Quaternion.identity;
		}

		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(string name, Quaternion value)
		{
		}

		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(int id, Quaternion value)
		{
		}
	}
}
