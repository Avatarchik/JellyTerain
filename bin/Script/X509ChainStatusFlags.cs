using System;

namespace Mono.Security.X509
{
	[Serializable]
	[Flags]
	public enum X509ChainStatusFlags
	{
		InvalidBasicConstraints = 0x400,
		NoError = 0x0,
		NotSignatureValid = 0x8,
		NotTimeNested = 0x2,
		NotTimeValid = 0x1,
		PartialChain = 0x10000,
		UntrustedRoot = 0x20
	}
}
