using System;

namespace UnityEngine
{
	[Flags]
	public enum AdditionalCanvasShaderChannels
	{
		None = 0x0,
		TexCoord1 = 0x1,
		TexCoord2 = 0x2,
		TexCoord3 = 0x4,
		Normal = 0x8,
		Tangent = 0x10
	}
}
