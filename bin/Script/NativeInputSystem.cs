using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Input
{
	public sealed class NativeInputSystem
	{
		public static NativeUpdateCallback onUpdate;

		public static NativeEventCallback onEvents;

		public static NativeDeviceDiscoveredCallback onDeviceDiscovered;

		internal static bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static double zeroEventTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[RequiredByNativeCode]
		internal static void NotifyUpdate(NativeInputUpdateType updateType)
		{
			onUpdate?.Invoke(updateType);
		}

		[RequiredByNativeCode]
		internal static void NotifyEvents(int eventCount, IntPtr eventData)
		{
			onEvents?.Invoke(eventCount, eventData);
		}

		[RequiredByNativeCode]
		internal static void NotifyDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
		{
			onDeviceDiscovered?.Invoke(deviceInfo);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SendInput(ref NativeInputEvent inputEvent);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetDeviceConfiguration(int deviceId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetControlConfiguration(int deviceId, int controlIndex);

		public static void SetPollingFrequency(float hertz)
		{
			if (hertz < 1f)
			{
				throw new ArgumentException("Polling frequency cannot be less than 1Hz");
			}
			SetPollingFrequencyInternal(hertz);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void SetPollingFrequencyInternal(float hertz);
	}
}
