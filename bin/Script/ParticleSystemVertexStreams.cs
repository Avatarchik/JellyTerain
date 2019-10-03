using System;

namespace UnityEngine
{
	[Flags]
	[Obsolete("ParticleSystemVertexStreams is deprecated. Please use ParticleSystemVertexStream instead.")]
	public enum ParticleSystemVertexStreams
	{
		Position = 0x1,
		Normal = 0x2,
		Tangent = 0x4,
		Color = 0x8,
		UV = 0x10,
		UV2BlendAndFrame = 0x20,
		CenterAndVertexID = 0x40,
		Size = 0x80,
		Rotation = 0x100,
		Velocity = 0x200,
		Lifetime = 0x400,
		Custom1 = 0x800,
		Custom2 = 0x1000,
		Random = 0x2000,
		None = 0x0,
		All = int.MaxValue
	}
}
