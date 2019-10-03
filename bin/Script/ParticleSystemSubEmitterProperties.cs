using System;

namespace UnityEngine
{
	[Flags]
	public enum ParticleSystemSubEmitterProperties
	{
		InheritNothing = 0x0,
		InheritEverything = 0x7,
		InheritColor = 0x1,
		InheritSize = 0x2,
		InheritRotation = 0x4
	}
}
