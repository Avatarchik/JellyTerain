using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Gyroscope
	{
		private int m_GyroIndex;

		public Vector3 rotationRate => rotationRate_Internal(m_GyroIndex);

		public Vector3 rotationRateUnbiased => rotationRateUnbiased_Internal(m_GyroIndex);

		public Vector3 gravity => gravity_Internal(m_GyroIndex);

		public Vector3 userAcceleration => userAcceleration_Internal(m_GyroIndex);

		public Quaternion attitude => attitude_Internal(m_GyroIndex);

		public bool enabled
		{
			get
			{
				return getEnabled_Internal(m_GyroIndex);
			}
			set
			{
				setEnabled_Internal(m_GyroIndex, value);
			}
		}

		public float updateInterval
		{
			get
			{
				return getUpdateInterval_Internal(m_GyroIndex);
			}
			set
			{
				setUpdateInterval_Internal(m_GyroIndex, value);
			}
		}

		internal Gyroscope(int index)
		{
			m_GyroIndex = index;
		}

		private static Vector3 rotationRate_Internal(int idx)
		{
			INTERNAL_CALL_rotationRate_Internal(idx, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_rotationRate_Internal(int idx, out Vector3 value);

		private static Vector3 rotationRateUnbiased_Internal(int idx)
		{
			INTERNAL_CALL_rotationRateUnbiased_Internal(idx, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_rotationRateUnbiased_Internal(int idx, out Vector3 value);

		private static Vector3 gravity_Internal(int idx)
		{
			INTERNAL_CALL_gravity_Internal(idx, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_gravity_Internal(int idx, out Vector3 value);

		private static Vector3 userAcceleration_Internal(int idx)
		{
			INTERNAL_CALL_userAcceleration_Internal(idx, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_userAcceleration_Internal(int idx, out Vector3 value);

		private static Quaternion attitude_Internal(int idx)
		{
			INTERNAL_CALL_attitude_Internal(idx, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_attitude_Internal(int idx, out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool getEnabled_Internal(int idx);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void setEnabled_Internal(int idx, bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float getUpdateInterval_Internal(int idx);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void setUpdateInterval_Internal(int idx, float interval);
	}
}
