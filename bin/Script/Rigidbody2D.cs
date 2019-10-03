using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class Rigidbody2D : Component
	{
		public Vector2 position
		{
			get
			{
				INTERNAL_get_position(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_position(ref value);
			}
		}

		public float rotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector2 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_velocity(ref value);
			}
		}

		public float angularVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool useAutoMass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float mass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public PhysicsMaterial2D sharedMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector2 centerOfMass
		{
			get
			{
				INTERNAL_get_centerOfMass(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_centerOfMass(ref value);
			}
		}

		public Vector2 worldCenterOfMass
		{
			get
			{
				INTERNAL_get_worldCenterOfMass(out Vector2 value);
				return value;
			}
		}

		public float inertia
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float drag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float gravityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RigidbodyType2D bodyType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool useFullKinematicContacts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool isKinematic
		{
			get
			{
				return bodyType == RigidbodyType2D.Kinematic;
			}
			set
			{
				bodyType = (value ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);
			}
		}

		[Obsolete("The fixedAngle is no longer supported. Use constraints instead.")]
		public bool fixedAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool freezeRotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RigidbodyConstraints2D constraints
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool simulated
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RigidbodyInterpolation2D interpolation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RigidbodySleepMode2D sleepMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public CollisionDetectionMode2D collisionDetectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int attachedColliderCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_position(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_position(ref Vector2 value);

		public void MovePosition(Vector2 position)
		{
			INTERNAL_CALL_MovePosition(this, ref position);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_MovePosition(Rigidbody2D self, ref Vector2 position);

		public void MoveRotation(float angle)
		{
			INTERNAL_CALL_MoveRotation(this, angle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_MoveRotation(Rigidbody2D self, float angle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_velocity(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_velocity(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_centerOfMass(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_centerOfMass(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_worldCenterOfMass(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetDragBehaviour(bool dragged);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsSleeping();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsAwake();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Sleep();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void WakeUp();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetAttachedColliders(Collider2D[] results);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsTouching(Collider2D collider);

		public bool IsTouching(Collider2D collider, ContactFilter2D contactFilter)
		{
			return Internal_IsTouching(collider, contactFilter);
		}

		private bool Internal_IsTouching(Collider2D collider, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_IsTouching(this, collider, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_Internal_IsTouching(Rigidbody2D self, Collider2D collider, ref ContactFilter2D contactFilter);

		public bool IsTouching(ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_IsTouching(this, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsTouching(Rigidbody2D self, ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			int layerMask = -1;
			return IsTouchingLayers(layerMask);
		}

		public bool OverlapPoint(Vector2 point)
		{
			return INTERNAL_CALL_OverlapPoint(this, ref point);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_OverlapPoint(Rigidbody2D self, ref Vector2 point);

		public int OverlapCollider(ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_OverlapCollider(this, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_OverlapCollider(Rigidbody2D self, ref ContactFilter2D contactFilter, Collider2D[] results);

		public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return INTERNAL_CALL_Cast(this, ref direction, results, distance);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_Cast(this, ref direction, results, distance);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Cast(Rigidbody2D self, ref Vector2 direction, RaycastHit2D[] results, float distance);

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return Cast(direction, contactFilter, results, distance);
		}

		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_Cast(direction, distance, contactFilter, results);
		}

		private int Internal_Cast(Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_Cast(this, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_Cast(Rigidbody2D self, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern ColliderDistance2D Distance(Collider2D collider);

		public void AddForce(Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		[ExcludeFromDocs]
		public void AddForce(Vector2 force)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddForce(this, ref force, mode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_AddForce(Rigidbody2D self, ref Vector2 force, ForceMode2D mode);

		public void AddRelativeForce(Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddRelativeForce(this, ref relativeForce, mode);
		}

		[ExcludeFromDocs]
		public void AddRelativeForce(Vector2 relativeForce)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddRelativeForce(this, ref relativeForce, mode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_AddRelativeForce(Rigidbody2D self, ref Vector2 relativeForce, ForceMode2D mode);

		public void AddForceAtPosition(Vector2 force, Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
		{
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector2 force, Vector2 position)
		{
			ForceMode2D mode = ForceMode2D.Force;
			INTERNAL_CALL_AddForceAtPosition(this, ref force, ref position, mode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_AddForceAtPosition(Rigidbody2D self, ref Vector2 force, ref Vector2 position, ForceMode2D mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void AddTorque(float torque, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

		[ExcludeFromDocs]
		public void AddTorque(float torque)
		{
			ForceMode2D mode = ForceMode2D.Force;
			AddTorque(torque, mode);
		}

		public Vector2 GetPoint(Vector2 point)
		{
			Internal_GetPoint(this, point, out Vector2 value);
			return value;
		}

		private static void Internal_GetPoint(Rigidbody2D rigidbody, Vector2 point, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetPoint(rigidbody, ref point, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetPoint(Rigidbody2D rigidbody, ref Vector2 point, out Vector2 value);

		public Vector2 GetRelativePoint(Vector2 relativePoint)
		{
			Internal_GetRelativePoint(this, relativePoint, out Vector2 value);
			return value;
		}

		private static void Internal_GetRelativePoint(Rigidbody2D rigidbody, Vector2 relativePoint, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetRelativePoint(rigidbody, ref relativePoint, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetRelativePoint(Rigidbody2D rigidbody, ref Vector2 relativePoint, out Vector2 value);

		public Vector2 GetVector(Vector2 vector)
		{
			Internal_GetVector(this, vector, out Vector2 value);
			return value;
		}

		private static void Internal_GetVector(Rigidbody2D rigidbody, Vector2 vector, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetVector(rigidbody, ref vector, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetVector(Rigidbody2D rigidbody, ref Vector2 vector, out Vector2 value);

		public Vector2 GetRelativeVector(Vector2 relativeVector)
		{
			Internal_GetRelativeVector(this, relativeVector, out Vector2 value);
			return value;
		}

		private static void Internal_GetRelativeVector(Rigidbody2D rigidbody, Vector2 relativeVector, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetRelativeVector(rigidbody, ref relativeVector, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetRelativeVector(Rigidbody2D rigidbody, ref Vector2 relativeVector, out Vector2 value);

		public Vector2 GetPointVelocity(Vector2 point)
		{
			Internal_GetPointVelocity(this, point, out Vector2 value);
			return value;
		}

		private static void Internal_GetPointVelocity(Rigidbody2D rigidbody, Vector2 point, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetPointVelocity(rigidbody, ref point, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetPointVelocity(Rigidbody2D rigidbody, ref Vector2 point, out Vector2 value);

		public Vector2 GetRelativePointVelocity(Vector2 relativePoint)
		{
			Internal_GetRelativePointVelocity(this, relativePoint, out Vector2 value);
			return value;
		}

		private static void Internal_GetRelativePointVelocity(Rigidbody2D rigidbody, Vector2 relativePoint, out Vector2 value)
		{
			INTERNAL_CALL_Internal_GetRelativePointVelocity(rigidbody, ref relativePoint, out value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetRelativePointVelocity(Rigidbody2D rigidbody, ref Vector2 relativePoint, out Vector2 value);

		public int GetContacts(ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		public int GetContacts(Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}
	}
}
