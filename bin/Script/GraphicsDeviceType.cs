using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	[UsedByNativeCode]
	public enum GraphicsDeviceType
	{
		[Obsolete("OpenGL2 is no longer supported in Unity 5.5+")]
		OpenGL2 = 0,
		Direct3D9 = 1,
		Direct3D11 = 2,
		[Obsolete("PS3 is no longer supported in Unity 5.5+")]
		PlayStation3 = 3,
		Null = 4,
		[Obsolete("Xbox360 is no longer supported in Unity 5.5+")]
		Xbox360 = 6,
		OpenGLES2 = 8,
		OpenGLES3 = 11,
		PlayStationVita = 12,
		PlayStation4 = 13,
		XboxOne = 14,
		PlayStationMobile = 0xF,
		Metal = 0x10,
		OpenGLCore = 17,
		Direct3D12 = 18,
		N3DS = 19,
		Vulkan = 21
	}
}
