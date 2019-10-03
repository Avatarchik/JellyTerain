// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Cryptography
{
    class DSAManaged : DSA
    {
      // Fields:
  defaultKeySize : Int32
  keypairGenerated : Boolean
  m_disposed : Boolean
  p : BigInteger
  q : BigInteger
  g : BigInteger
  x : BigInteger
  y : BigInteger
  j : BigInteger
  seed : BigInteger
  counter : Int32
  j_missing : Boolean
  rng : RandomNumberGenerator
  KeyGenerated : KeyGeneratedEventHandler
      // Properties:
  Random : RandomNumberGenerator
  KeySize : Int32
  KeyExchangeAlgorithm : String
  PublicOnly : Boolean
  SignatureAlgorithm : String
      // Events:
      KeyGenerated : KeyGeneratedEventHandler
      // Methods:
      public Void Mono.Security.Cryptography.DSAManaged::.ctor()
      public Void Mono.Security.Cryptography.DSAManaged::.ctorInt32)
      public Void Mono.Security.Cryptography.DSAManaged::add_KeyGenerated(Mono.Security.Cryptography.DSAManaged/KeyGeneratedEventHandler)
      public Void Mono.Security.Cryptography.DSAManaged::remove_KeyGenerated(Mono.Security.Cryptography.DSAManaged/KeyGeneratedEventHandler)
      Void Mono.Security.Cryptography.DSAManaged::Finalize()
      Void Mono.Security.Cryptography.DSAManaged::Generate()
      Void Mono.Security.Cryptography.DSAManaged::GenerateKeyPair()
      Void Mono.Security.Cryptography.DSAManaged::addByte[]Byte[]Int32)
      Void Mono.Security.Cryptography.DSAManaged::GenerateParamsInt32)
      Security.Cryptography.RandomNumberGenerator Mono.Security.Cryptography.DSAManaged::get_Random()
      public Int32 Mono.Security.Cryptography.DSAManaged::get_KeySize()
      public String Mono.Security.Cryptography.DSAManaged::get_KeyExchangeAlgorithm()
      public Boolean Mono.Security.Cryptography.DSAManaged::get_PublicOnly()
      public String Mono.Security.Cryptography.DSAManaged::get_SignatureAlgorithm()
      Byte[] Mono.Security.Cryptography.DSAManaged::NormalizeArrayByte[])
      public Security.Cryptography.DSAParameters Mono.Security.Cryptography.DSAManaged::ExportParametersBoolean)
      public Void Mono.Security.Cryptography.DSAManaged::ImportParametersSecurity.Cryptography.DSAParameters)
      public Byte[] Mono.Security.Cryptography.DSAManaged::CreateSignatureByte[])
      public Boolean Mono.Security.Cryptography.DSAManaged::VerifySignatureByte[]Byte[])
      Void Mono.Security.Cryptography.DSAManaged::DisposeBoolean)
    }
}
