using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.VR
{
	public static class VRSettings
	{
		public static bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool isDeviceActive
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool showDeviceView
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static float renderScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int eyeTextureWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static int eyeTextureHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static float renderViewportScale
		{
			get
			{
				return renderViewportScaleInternal;
			}
			set
			{
				if (value < 0f || value > 1f)
				{
					throw new ArgumentOutOfRangeException("value", "Render viewport scale should be between 0 and 1.");
				}
				renderViewportScaleInternal = value;
			}
		}

		internal static float renderViewportScaleInternal
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("loadedDevice is deprecated.  Use loadedDeviceName and LoadDeviceByName instead.")]
		public static VRDeviceType loadedDevice
		{
			get
			{
				VRDeviceType result = VRDeviceType.Unknown;
				try
				{
					result = (VRDeviceType)Enum.Parse(typeof(VRDeviceType), loadedDeviceName, ignoreCase: true);
				}
				catch (Exception)
				{
				}
				return result;
			}
			set
			{
				LoadDeviceByName(value.ToString());
			}
		}

		public static string loadedDeviceName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string[] supportedDevices
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static void LoadDeviceByName(string deviceName)
		{
			LoadDeviceByName(new string[1]
			{
				deviceName
			});
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void LoadDeviceByName(string[] prioritizedDeviceNameList);
	}
}
