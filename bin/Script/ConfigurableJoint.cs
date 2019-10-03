using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class ConfigurableJoint : Joint
	{
		public Vector3 secondaryAxis
		{
			get
			{
				INTERNAL_get_secondaryAxis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_secondaryAxis(ref value);
			}
		}

		public ConfigurableJointMotion xMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ConfigurableJointMotion yMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ConfigurableJointMotion zMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ConfigurableJointMotion angularXMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ConfigurableJointMotion angularYMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ConfigurableJointMotion angularZMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public SoftJointLimitSpring linearLimitSpring
		{
			get
			{
				INTERNAL_get_linearLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_linearLimitSpring(ref value);
			}
		}

		public SoftJointLimitSpring angularXLimitSpring
		{
			get
			{
				INTERNAL_get_angularXLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_angularXLimitSpring(ref value);
			}
		}

		public SoftJointLimitSpring angularYZLimitSpring
		{
			get
			{
				INTERNAL_get_angularYZLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYZLimitSpring(ref value);
			}
		}

		public SoftJointLimit linearLimit
		{
			get
			{
				INTERNAL_get_linearLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_linearLimit(ref value);
			}
		}

		public SoftJointLimit lowAngularXLimit
		{
			get
			{
				INTERNAL_get_lowAngularXLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_lowAngularXLimit(ref value);
			}
		}

		public SoftJointLimit highAngularXLimit
		{
			get
			{
				INTERNAL_get_highAngularXLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_highAngularXLimit(ref value);
			}
		}

		public SoftJointLimit angularYLimit
		{
			get
			{
				INTERNAL_get_angularYLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYLimit(ref value);
			}
		}

		public SoftJointLimit angularZLimit
		{
			get
			{
				INTERNAL_get_angularZLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_angularZLimit(ref value);
			}
		}

		public Vector3 targetPosition
		{
			get
			{
				INTERNAL_get_targetPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetPosition(ref value);
			}
		}

		public Vector3 targetVelocity
		{
			get
			{
				INTERNAL_get_targetVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetVelocity(ref value);
			}
		}

		public JointDrive xDrive
		{
			get
			{
				INTERNAL_get_xDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_xDrive(ref value);
			}
		}

		public JointDrive yDrive
		{
			get
			{
				INTERNAL_get_yDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_yDrive(ref value);
			}
		}

		public JointDrive zDrive
		{
			get
			{
				INTERNAL_get_zDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_zDrive(ref value);
			}
		}

		public Quaternion targetRotation
		{
			get
			{
				INTERNAL_get_targetRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_targetRotation(ref value);
			}
		}

		public Vector3 targetAngularVelocity
		{
			get
			{
				INTERNAL_get_targetAngularVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetAngularVelocity(ref value);
			}
		}

		public RotationDriveMode rotationDriveMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public JointDrive angularXDrive
		{
			get
			{
				INTERNAL_get_angularXDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_angularXDrive(ref value);
			}
		}

		public JointDrive angularYZDrive
		{
			get
			{
				INTERNAL_get_angularYZDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYZDrive(ref value);
			}
		}

		public JointDrive slerpDrive
		{
			get
			{
				INTERNAL_get_slerpDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_slerpDrive(ref value);
			}
		}

		public JointProjectionMode projectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float projectionDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float projectionAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool configuredInWorldSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool swapBodies
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_secondaryAxis(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_secondaryAxis(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_linearLimitSpring(out SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_linearLimitSpring(ref SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularXLimitSpring(out SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularXLimitSpring(ref SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularYZLimitSpring(out SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularYZLimitSpring(ref SoftJointLimitSpring value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_linearLimit(out SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_linearLimit(ref SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_lowAngularXLimit(out SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_lowAngularXLimit(ref SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_highAngularXLimit(out SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_highAngularXLimit(ref SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularYLimit(out SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularYLimit(ref SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularZLimit(out SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularZLimit(ref SoftJointLimit value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetPosition(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_targetPosition(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetVelocity(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_targetVelocity(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_xDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_xDrive(ref JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_yDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_yDrive(ref JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_zDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_zDrive(ref JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetRotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_targetRotation(ref Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_targetAngularVelocity(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_targetAngularVelocity(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularXDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularXDrive(ref JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_angularYZDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_angularYZDrive(ref JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_slerpDrive(out JointDrive value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_slerpDrive(ref JointDrive value);
	}
}
