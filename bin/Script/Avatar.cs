using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Avatar : Object
	{
		public bool isValid
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

		private Avatar()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetMuscleMinMax(int muscleId, float min, float max);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetParameter(int parameterId, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern float GetAxisLength(int humanId);

		internal Quaternion GetPreRotation(int humanId)
		{
			INTERNAL_CALL_GetPreRotation(this, humanId, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPreRotation(Avatar self, int humanId, out Quaternion value);

		internal Quaternion GetPostRotation(int humanId)
		{
			INTERNAL_CALL_GetPostRotation(this, humanId, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPostRotation(Avatar self, int humanId, out Quaternion value);

		internal Quaternion GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
		{
			INTERNAL_CALL_GetZYPostQ(this, humanId, ref parentQ, ref q, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetZYPostQ(Avatar self, int humanId, ref Quaternion parentQ, ref Quaternion q, out Quaternion value);

		internal Quaternion GetZYRoll(int humanId, Vector3 uvw)
		{
			INTERNAL_CALL_GetZYRoll(this, humanId, ref uvw, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetZYRoll(Avatar self, int humanId, ref Vector3 uvw, out Quaternion value);

		internal Vector3 GetLimitSign(int humanId)
		{
			INTERNAL_CALL_GetLimitSign(this, humanId, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetLimitSign(Avatar self, int humanId, out Vector3 value);
	}
}
