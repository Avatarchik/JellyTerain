using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class WheelJoint2D : AnchoredJoint2D
	{
		public JointSuspension2D suspension
		{
			get
			{
				INTERNAL_get_suspension(out JointSuspension2D value);
				return value;
			}
			set
			{
				INTERNAL_set_suspension(ref value);
			}
		}

		public bool useMotor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public JointMotor2D motor
		{
			get
			{
				INTERNAL_get_motor(out JointMotor2D value);
				return value;
			}
			set
			{
				INTERNAL_set_motor(ref value);
			}
		}

		public float jointTranslation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float jointLinearSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float jointSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float jointAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_suspension(out JointSuspension2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_suspension(ref JointSuspension2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_motor(out JointMotor2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_motor(ref JointMotor2D value);

		public float GetMotorTorque(float timeStep)
		{
			return INTERNAL_CALL_GetMotorTorque(this, timeStep);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float INTERNAL_CALL_GetMotorTorque(WheelJoint2D self, float timeStep);
	}
}
