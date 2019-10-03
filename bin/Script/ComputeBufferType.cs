using System;

namespace UnityEngine
{
	[Flags]
	public enum ComputeBufferType
	{
		Default = 0x0,
		Raw = 0x1,
		Append = 0x2,
		Counter = 0x4,
		[Obsolete("Enum member DrawIndirect has been deprecated. Use IndirectArguments instead (UnityUpgradable) -> IndirectArguments", false)]
		DrawIndirect = 0x100,
		IndirectArguments = 0x100,
		GPUMemory = 0x200
	}
}
