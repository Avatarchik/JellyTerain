using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.VR
{
	public static class VRDevice
	{
		public static bool isPresent
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("family is deprecated.  Use VRSettings.loadedDeviceName instead.")]
		public static string family
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string model
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static float refreshRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern TrackingSpaceType GetTrackingSpaceType();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool SetTrackingSpaceType(TrackingSpaceType trackingSpaceType);

		public static IntPtr GetNativePtr()
		{
			INTERNAL_CALL_GetNativePtr(out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetNativePtr(out IntPtr value);
	}
}
