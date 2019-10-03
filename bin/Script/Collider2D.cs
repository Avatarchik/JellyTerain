using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public class Collider2D : Behaviour
	{
		public float density
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool isTrigger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool usedByEffector
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool usedByComposite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public CompositeCollider2D composite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Vector2 offset
		{
			get
			{
				INTERNAL_get_offset(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_offset(ref value);
			}
		}

		public Rigidbody2D attachedRigidbody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int shapeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		internal ColliderErrorState2D errorState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal bool compositeCapable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
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

		public float friction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float bounciness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_offset(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_offset(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_bounds(out Bounds value);

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
		private static extern bool INTERNAL_CALL_Internal_IsTouching(Collider2D self, Collider2D collider, ref ContactFilter2D contactFilter);

		public bool IsTouching(ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_IsTouching(this, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsTouching(Collider2D self, ref ContactFilter2D contactFilter);

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
		private static extern bool INTERNAL_CALL_OverlapPoint(Collider2D self, ref Vector2 point);

		public int OverlapCollider(ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.OverlapCollider(this, contactFilter, results);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return Raycast(direction, contactFilter, results, distance);
		}

		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_Raycast(direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -1;
			return Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -1;
			float distance = float.PositiveInfinity;
			return Raycast(direction, results, distance, layerMask, minDepth, maxDepth);
		}

		public int Raycast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_Raycast(direction, distance, contactFilter, results);
		}

		private int Internal_Raycast(Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_Raycast(this, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_Raycast(Collider2D self, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance)
		{
			bool ignoreSiblingColliders = true;
			return Cast(direction, contactFilter, results, distance, ignoreSiblingColliders);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			bool ignoreSiblingColliders = true;
			float distance = float.PositiveInfinity;
			return Cast(direction, contactFilter, results, distance, ignoreSiblingColliders);
		}

		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			return Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			bool ignoreSiblingColliders = true;
			return Cast(direction, results, distance, ignoreSiblingColliders);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results)
		{
			bool ignoreSiblingColliders = true;
			float distance = float.PositiveInfinity;
			return Cast(direction, results, distance, ignoreSiblingColliders);
		}

		public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(base.gameObject.layer));
			return Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
		}

		private int Internal_Cast(Vector2 direction, ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_Cast(this, ref direction, ref contactFilter, distance, ignoreSiblingColliders, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_Cast(Collider2D self, ref Vector2 direction, ref ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results);

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

		public ColliderDistance2D Distance(Collider2D collider)
		{
			return Physics2D.Distance(this, collider);
		}
	}
}
