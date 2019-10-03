using System;

namespace Mono.Security.Protocol.Tls.Handshake
{
	[Serializable]
	internal enum HandshakeType : byte
	{
		HelloRequest = 0,
		ClientHello = 1,
		ServerHello = 2,
		Certificate = 11,
		ServerKeyExchange = 12,
		CertificateRequest = 13,
		ServerHelloDone = 14,
		CertificateVerify = 0xF,
		ClientKeyExchange = 0x10,
		Finished = 20,
		None = byte.MaxValue
	}
}
