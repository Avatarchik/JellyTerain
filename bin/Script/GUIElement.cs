using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public class GUIElement : Behaviour
	{
		public bool HitTest(Vector3 screenPosition, [DefaultValue("null")] Camera camera)
		{
			return INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
		}

		[ExcludeFromDocs]
		public bool HitTest(Vector3 screenPosition)
		{
			Camera camera = null;
			return INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_HitTest(GUIElement self, ref Vector3 screenPosition, Camera camera);

		public Rect GetScreenRect([DefaultValue("null")] Camera camera)
		{
			INTERNAL_CALL_GetScreenRect(this, camera, out Rect value);
			return value;
		}

		[ExcludeFromDocs]
		public Rect GetScreenRect()
		{
			Camera camera = null;
			INTERNAL_CALL_GetScreenRect(this, camera, out Rect value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetScreenRect(GUIElement self, Camera camera, out Rect value);
	}
}
