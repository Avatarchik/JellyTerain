using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.VR
{
	public static class VRStats
	{
		[Obsolete("gpuTimeLastFrame is deprecated. Use VRStats.TryGetGPUTimeLastFrame instead.")]
		public static float gpuTimeLastFrame
		{
			get
			{
				if (TryGetGPUTimeLastFrame(out float gpuTimeLastFrame))
				{
					return gpuTimeLastFrame;
				}
				return 0f;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool TryGetGPUTimeLastFrame(out float gpuTimeLastFrame);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool TryGetDroppedFrameCount(out int droppedFrameCount);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool TryGetFramePresentCount(out int framePresentCount);
	}
}
