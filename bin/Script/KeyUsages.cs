using System;

namespace Mono.Security.X509.Extensions
{
	[Flags]
	public enum KeyUsages
	{
		digitalSignature = 0x80,
		nonRepudiation = 0x40,
		keyEncipherment = 0x20,
		dataEncipherment = 0x10,
		keyAgreement = 0x8,
		keyCertSign = 0x4,
		cRLSign = 0x2,
		encipherOnly = 0x1,
		decipherOnly = 0x800,
		none = 0x0
	}
}
