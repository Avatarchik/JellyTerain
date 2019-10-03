using System;

namespace UnityEngine.Experimental.Rendering
{
	[Flags]
	public enum RendererConfiguration
	{
		None = 0x0,
		PerObjectLightProbe = 0x1,
		PerObjectReflectionProbes = 0x2,
		PerObjectLightProbeProxyVolume = 0x4,
		PerObjectLightmaps = 0x8,
		ProvideLightIndices = 0x10
	}
}
