using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.VR
{
	public static class InputTracking
	{
		public static bool disablePositionalTracking
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static Vector3 GetLocalPosition(VRNode node)
		{
			INTERNAL_CALL_GetLocalPosition(node, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetLocalPosition(VRNode node, out Vector3 value);

		public static Quaternion GetLocalRotation(VRNode node)
		{
			INTERNAL_CALL_GetLocalRotation(node, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetLocalRotation(VRNode node, out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Recenter();
	}
}
