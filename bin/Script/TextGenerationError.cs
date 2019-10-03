using System;

namespace UnityEngine
{
	[Flags]
	internal enum TextGenerationError
	{
		None = 0x0,
		CustomSizeOnNonDynamicFont = 0x1,
		CustomStyleOnNonDynamicFont = 0x2,
		NoFont = 0x4
	}
}
