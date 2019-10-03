// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security
{
    class StrongName : Object
    {
      // Fields:
  rsa : RSA
  publicKey : Byte[]
  keyToken : Byte[]
  tokenAlgorithm : String
  lockObject : Object
  initialized : Boolean
      // Properties:
  CanSign : Boolean
  RSA : RSA
  PublicKey : Byte[]
  PublicKeyToken : Byte[]
  TokenAlgorithm : String
      // Events:
      // Methods:
      public Void Mono.Security.StrongName::.ctor()
      public Void Mono.Security.StrongName::.ctorInt32)
      public Void Mono.Security.StrongName::.ctorByte[])
      public Void Mono.Security.StrongName::.ctorSecurity.Cryptography.RSA)
      Void Mono.Security.StrongName::.cctor()
      Void Mono.Security.StrongName::InvalidateCache()
      public Boolean Mono.Security.StrongName::get_CanSign()
      public Security.Cryptography.RSA Mono.Security.StrongName::get_RSA()
      public Void Mono.Security.StrongName::set_RSASecurity.Cryptography.RSA)
      public Byte[] Mono.Security.StrongName::get_PublicKey()
      public Byte[] Mono.Security.StrongName::get_PublicKeyToken()
      public String Mono.Security.StrongName::get_TokenAlgorithm()
      public Void Mono.Security.StrongName::set_TokenAlgorithmString)
      public Byte[] Mono.Security.StrongName::GetBytes()
      UInt32 Mono.Security.StrongName::RVAtoPositionUInt32Int32Byte[])
      Mono.Security.StrongName/StrongNameSignature Mono.Security.StrongName::StrongHashIO.Stream,Mono.Security.StrongName/StrongNameOptions)
      public Byte[] Mono.Security.StrongName::HashString)
      public Boolean Mono.Security.StrongName::SignString)
      public Boolean Mono.Security.StrongName::VerifyString)
      public Boolean Mono.Security.StrongName::VerifyIO.Stream)
      public Boolean Mono.Security.StrongName::IsAssemblyStrongnamedString)
      public Boolean Mono.Security.StrongName::VerifySignatureByte[]Int32Byte[]Byte[])
      Boolean Mono.Security.StrongName::VerifySecurity.Cryptography.RSAConfiguration.Assemblies.AssemblyHashAlgorithmByte[]Byte[])
    }
}
