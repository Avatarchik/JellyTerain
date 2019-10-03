using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class Physics2D
	{
		public const int IgnoreRaycastLayer = 4;

		public const int DefaultRaycastLayers = -5;

		public const int AllLayers = -1;

		private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

		[ThreadAndSerializationSafe]
		public static int velocityIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int positionIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static Vector2 gravity
		{
			get
			{
				INTERNAL_get_gravity(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_gravity(ref value);
			}
		}

		public static bool queriesHitTriggers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool queriesStartInColliders
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool changeStopsCallbacks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool callbacksOnDisable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float velocityThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float maxLinearCorrection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float maxAngularCorrection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float maxTranslationSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float maxRotationSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float defaultContactOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float baumgarteScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float baumgarteTOIScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float timeToSleep
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float linearSleepTolerance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float angularSleepTolerance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool alwaysShowColliders
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool showColliderSleep
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool showColliderContacts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool showColliderAABB
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float contactArrowScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static Color colliderAwakeColor
		{
			get
			{
				INTERNAL_get_colliderAwakeColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_colliderAwakeColor(ref value);
			}
		}

		public static Color colliderAsleepColor
		{
			get
			{
				INTERNAL_get_colliderAsleepColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_colliderAsleepColor(ref value);
			}
		}

		public static Color colliderContactColor
		{
			get
			{
				INTERNAL_get_colliderContactColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_colliderContactColor(ref value);
			}
		}

		public static Color colliderAABBColor
		{
			get
			{
				INTERNAL_get_colliderAABBColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_colliderAABBColor(ref value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_gravity(out Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_gravity(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_colliderAwakeColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_colliderAwakeColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_colliderAsleepColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_colliderAsleepColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_colliderContactColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_colliderContactColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_colliderAABBColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_colliderAABBColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void IgnoreCollision(Collider2D collider1, Collider2D collider2, [DefaultValue("true")] bool ignore);

		[ExcludeFromDocs]
		public static void IgnoreCollision(Collider2D collider1, Collider2D collider2)
		{
			bool ignore = true;
			IgnoreCollision(collider1, collider2, ignore);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetIgnoreCollision(Collider2D collider1, Collider2D collider2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void IgnoreLayerCollision(int layer1, int layer2, [DefaultValue("true")] bool ignore);

		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			bool ignore = true;
			IgnoreLayerCollision(layer1, layer2, ignore);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetIgnoreLayerCollision(int layer1, int layer2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetLayerCollisionMask(int layer, int layerMask);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetLayerCollisionMask(int layer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsTouching(Collider2D collider1, Collider2D collider2);

		public static bool IsTouching(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter)
		{
			return Internal_IsTouching(collider1, collider2, contactFilter);
		}

		private static bool Internal_IsTouching(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_IsTouching(collider1, collider2, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_Internal_IsTouching(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter);

		public static bool IsTouching(Collider2D collider, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_IsTouching(collider, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsTouching(Collider2D collider, ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsTouchingLayers(Collider2D collider, [DefaultValue("AllLayers")] int layerMask);

		[ExcludeFromDocs]
		public static bool IsTouchingLayers(Collider2D collider)
		{
			int layerMask = -1;
			return IsTouchingLayers(collider, layerMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern ColliderDistance2D Distance(Collider2D colliderA, Collider2D colliderB);

		internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
		{
			foreach (Rigidbody2D item in m_LastDisabledRigidbody2D)
			{
				if (item != null)
				{
					item.SetDragBehaviour(dragged: false);
				}
			}
			m_LastDisabledRigidbody2D.Clear();
			if (!dragging)
			{
				return;
			}
			foreach (GameObject gameObject in objs)
			{
				Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(includeInactive: false);
				Rigidbody2D[] array = componentsInChildren;
				foreach (Rigidbody2D rigidbody2D in array)
				{
					m_LastDisabledRigidbody2D.Add(rigidbody2D);
					rigidbody2D.SetDragBehaviour(dragged: true);
				}
			}
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return Linecast(start, end, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			Internal_Linecast(start, end, contactFilter, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return LinecastAll(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return LinecastAll(start, end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return LinecastAll(start, end, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_LinecastAll(start, end, contactFilter);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return LinecastNonAlloc(start, end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return LinecastNonAlloc(start, end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return LinecastNonAlloc(start, end, results, layerMask, minDepth, maxDepth);
		}

		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_LinecastNonAlloc(start, end, contactFilter, results);
		}

		public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Internal_LinecastNonAlloc(start, end, contactFilter, results);
		}

		private static void Internal_Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_Linecast(ref start, ref end, ref contactFilter, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_Linecast(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_LinecastAll(Vector2 start, Vector2 end, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_LinecastAll(ref start, ref end, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_LinecastAll(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter);

		private static int Internal_LinecastNonAlloc(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_LinecastNonAlloc(ref start, ref end, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_LinecastNonAlloc(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			Internal_Raycast(origin, direction, distance, contactFilter, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_RaycastAll(origin, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_RaycastNonAlloc(origin, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return Raycast(origin, direction, contactFilter, results, distance);
		}

		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_RaycastNonAlloc(origin, direction, distance, contactFilter, results);
		}

		private static void Internal_Raycast(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_Raycast(ref origin, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_Raycast(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_RaycastAll(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_RaycastAll(ref origin, ref direction, distance, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_RaycastAll(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_RaycastNonAlloc(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_RaycastNonAlloc(ref origin, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_RaycastNonAlloc(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			Internal_CircleCast(origin, radius, direction, distance, contactFilter, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_CircleCastAll(origin, radius, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_CircleCastNonAlloc(origin, radius, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return CircleCast(origin, radius, direction, contactFilter, results, distance);
		}

		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_CircleCastNonAlloc(origin, radius, direction, distance, contactFilter, results);
		}

		private static void Internal_CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_CircleCast(ref origin, radius, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_CircleCast(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_CircleCastAll(ref origin, radius, ref direction, distance, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_CircleCastAll(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_CircleCastNonAlloc(ref origin, radius, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_CircleCastNonAlloc(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			Internal_BoxCast(origin, size, angle, direction, distance, contactFilter, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_BoxCastAll(origin, size, angle, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_BoxCastNonAlloc(origin, size, angle, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return BoxCast(origin, size, angle, direction, contactFilter, results, distance);
		}

		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_BoxCastNonAlloc(origin, size, angle, direction, distance, contactFilter, results);
		}

		private static void Internal_BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_BoxCast(ref origin, ref size, angle, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_BoxCast(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_BoxCastAll(ref origin, ref size, angle, ref direction, distance, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_BoxCastAll(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_BoxCastNonAlloc(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			Internal_CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
		}

		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth);
		}

		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float distance = float.PositiveInfinity;
			return CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, distance);
		}

		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Internal_CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		private static void Internal_CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_CapsuleCast(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_CapsuleCast(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_CapsuleCastAll(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_CapsuleCastAll(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return INTERNAL_CALL_Internal_CapsuleCastNonAlloc(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_CapsuleCastNonAlloc(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		private static void Internal_GetRayIntersection(Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit)
		{
			INTERNAL_CALL_Internal_GetRayIntersection(ref ray, distance, layerMask, out raycastHit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_GetRayIntersection(ref Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit);

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray, float distance)
		{
			int layerMask = -5;
			return GetRayIntersection(ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return GetRayIntersection(ray, distance, layerMask);
		}

		public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			Internal_GetRayIntersection(ray, distance, layerMask, out RaycastHit2D raycastHit);
			return raycastHit;
		}

		[RequiredByNativeCode]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance)
		{
			int layerMask = -5;
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern RaycastHit2D[] INTERNAL_CALL_GetRayIntersectionAll(ref Ray ray, float distance, int layerMask);

		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance)
		{
			int layerMask = -5;
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results)
		{
			int layerMask = -5;
			float distance = float.PositiveInfinity;
			return INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetRayIntersectionNonAlloc(ref Ray ray, RaycastHit2D[] results, float distance, int layerMask);

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapPoint(point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapPoint(point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapPoint(point, layerMask, minDepth, maxDepth);
		}

		public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapPoint(point, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapPointAll(point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapPointAll(point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapPointAll(point, layerMask, minDepth, maxDepth);
		}

		public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapPointAll(point, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapPointNonAlloc(point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapPointNonAlloc(point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapPointNonAlloc(point, results, layerMask, minDepth, maxDepth);
		}

		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapPointNonAlloc(point, contactFilter, results);
		}

		public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Internal_OverlapPointNonAlloc(point, contactFilter, results);
		}

		private static Collider2D Internal_OverlapPoint(Vector2 point, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapPoint(ref point, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapPoint(ref Vector2 point, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapPointAll(Vector2 point, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapPointAll(ref point, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapPointAll(ref Vector2 point, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapPointNonAlloc(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_Internal_OverlapPointNonAlloc(ref point, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_OverlapPointNonAlloc(ref Vector2 point, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCircle(point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCircle(point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCircle(point, radius, layerMask, minDepth, maxDepth);
		}

		public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCircle(point, radius, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCircleAll(point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCircleAll(point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCircleAll(point, radius, layerMask, minDepth, maxDepth);
		}

		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCircleAll(point, radius, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, maxDepth);
		}

		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCircleNonAlloc(point, radius, contactFilter, results);
		}

		public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Internal_OverlapCircleNonAlloc(point, radius, contactFilter, results);
		}

		private static Collider2D Internal_OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapCircle(ref point, radius, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapCircle(ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapCircleAll(Vector2 point, float radius, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapCircleAll(ref point, radius, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapCircleAll(ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapCircleNonAlloc(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_Internal_OverlapCircleNonAlloc(ref point, radius, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_OverlapCircleNonAlloc(ref Vector2 point, float radius, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapBox(point, size, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapBox(point, size, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapBox(point, size, angle, layerMask, minDepth, maxDepth);
		}

		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapBox(point, size, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapBoxAll(point, size, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapBoxAll(point, size, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapBoxAll(point, size, angle, layerMask, minDepth, maxDepth);
		}

		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapBoxAll(point, size, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, maxDepth);
		}

		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapBoxNonAlloc(point, size, angle, contactFilter, results);
		}

		public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Internal_OverlapBoxNonAlloc(point, size, angle, contactFilter, results);
		}

		private static Collider2D Internal_OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapBox(ref point, ref size, angle, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapBox(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapBoxAll(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapBoxAll(ref point, ref size, angle, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapBoxAll(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_Internal_OverlapBoxNonAlloc(ref point, ref size, angle, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_OverlapBoxNonAlloc(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapArea(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapArea(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapArea(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapArea(pointA, pointB, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapAreaAll(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapAreaAll(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapAreaAll(pointA, pointB, layerMask, minDepth, maxDepth);
		}

		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapAreaAll(pointA, pointB, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, maxDepth);
		}

		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapAreaNonAlloc(pointA, pointB, contactFilter, results);
		}

		public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Internal_OverlapAreaNonAlloc(pointA, pointB, contactFilter, results);
		}

		private static Collider2D Internal_OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapArea(ref pointA, ref pointB, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapArea(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapAreaAll(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapAreaAll(ref pointA, ref pointB, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapAreaAll(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_Internal_OverlapAreaNonAlloc(ref pointA, ref pointB, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_OverlapAreaNonAlloc(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCapsule(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCapsule(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCapsule(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCapsule(point, size, direction, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, maxDepth);
		}

		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCapsuleAll(point, size, direction, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			float maxDepth = float.PositiveInfinity;
			return OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			return OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results)
		{
			float maxDepth = float.PositiveInfinity;
			float minDepth = float.NegativeInfinity;
			int layerMask = -5;
			return OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, maxDepth);
		}

		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Internal_OverlapCapsuleNonAlloc(point, size, direction, angle, contactFilter, results);
		}

		public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Internal_OverlapCapsuleNonAlloc(point, size, direction, angle, contactFilter, results);
		}

		private static Collider2D Internal_OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapCapsule(ref point, ref size, direction, angle, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapCapsule(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
		{
			return INTERNAL_CALL_Internal_OverlapCapsuleAll(ref point, ref size, direction, angle, ref contactFilter);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapCapsuleAll(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_Internal_OverlapCapsuleNonAlloc(ref point, ref size, direction, angle, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_OverlapCapsuleNonAlloc(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter, Collider2D[] results);

		public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_OverlapCollider(collider, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_OverlapCollider(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern Rigidbody2D GetRigidbodyFromInstanceID(int instanceID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern Collider2D GetColliderFromInstanceID(int instanceID);

		private static int GetColliderContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] results)
		{
			return INTERNAL_CALL_GetColliderContacts(collider, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetColliderContacts(Collider2D collider, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		private static int GetRigidbodyContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] results)
		{
			return INTERNAL_CALL_GetRigidbodyContacts(rigidbody, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetRigidbodyContacts(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		private static int GetColliderContactsCollidersOnly(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_GetColliderContactsCollidersOnly(collider, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetColliderContactsCollidersOnly(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

		private static int GetRigidbodyContactsCollidersOnly(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return INTERNAL_CALL_GetRigidbodyContactsCollidersOnly(rigidbody, ref contactFilter, results);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_GetRigidbodyContactsCollidersOnly(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, Collider2D[] results);

		public static int GetContacts(Collider2D collider, ContactPoint2D[] contacts)
		{
			return GetColliderContacts(collider, default(ContactFilter2D).NoFilter(), contacts);
		}

		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return GetColliderContacts(collider, contactFilter, contacts);
		}

		public static int GetContacts(Collider2D collider, Collider2D[] colliders)
		{
			return GetColliderContactsCollidersOnly(collider, default(ContactFilter2D).NoFilter(), colliders);
		}

		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return GetColliderContactsCollidersOnly(collider, contactFilter, colliders);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactPoint2D[] contacts)
		{
			return GetRigidbodyContacts(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return GetRigidbodyContacts(rigidbody, contactFilter, contacts);
		}

		public static int GetContacts(Rigidbody2D rigidbody, Collider2D[] colliders)
		{
			return GetRigidbodyContactsCollidersOnly(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return GetRigidbodyContactsCollidersOnly(rigidbody, contactFilter, colliders);
		}
	}
}
