using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal sealed class RuntimeUndo
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetTransformParent(Transform transform, Transform newParent, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void RecordObject(Object objectToUndo, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void RecordObjects(Object[] objectsToUndo, string name);
	}
}
