using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class ClusterInput
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetAxis(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetButton(string name);

		public static Vector3 GetTrackerPosition(string name)
		{
			INTERNAL_CALL_GetTrackerPosition(name, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetTrackerPosition(string name, out Vector3 value);

		public static Quaternion GetTrackerRotation(string name)
		{
			INTERNAL_CALL_GetTrackerRotation(name, out Quaternion value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetTrackerRotation(string name, out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetAxis(string name, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetButton(string name, bool value);

		public static void SetTrackerPosition(string name, Vector3 value)
		{
			INTERNAL_CALL_SetTrackerPosition(name, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetTrackerPosition(string name, ref Vector3 value);

		public static void SetTrackerRotation(string name, Quaternion value)
		{
			INTERNAL_CALL_SetTrackerRotation(name, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetTrackerRotation(string name, ref Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool AddInput(string name, string deviceName, string serverUrl, int index, ClusterInputType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool EditInput(string name, string deviceName, string serverUrl, int index, ClusterInputType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CheckConnectionToServer(string name);
	}
}
