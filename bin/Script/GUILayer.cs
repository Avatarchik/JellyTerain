using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Camera))]
	public sealed class GUILayer : Behaviour
	{
		public GUIElement HitTest(Vector3 screenPosition)
		{
			return INTERNAL_CALL_HitTest(this, ref screenPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern GUIElement INTERNAL_CALL_HitTest(GUILayer self, ref Vector3 screenPosition);
	}
}
