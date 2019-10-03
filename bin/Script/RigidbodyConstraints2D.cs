using System;

namespace UnityEngine
{
	[Flags]
	public enum RigidbodyConstraints2D
	{
		None = 0x0,
		FreezePositionX = 0x1,
		FreezePositionY = 0x2,
		FreezeRotation = 0x4,
		FreezePosition = 0x3,
		FreezeAll = 0x7
	}
}
