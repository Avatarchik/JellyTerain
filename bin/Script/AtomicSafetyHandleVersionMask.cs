using System;

namespace UnityEngine
{
	[Flags]
	internal enum AtomicSafetyHandleVersionMask
	{
		Read = 0x1,
		Write = 0x2,
		ReadAndWrite = 0x3,
		WriteInv = -3,
		ReadInv = -2,
		ReadAndWriteInv = -4
	}
}
