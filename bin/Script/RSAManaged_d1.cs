// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Cryptography
{
    class RSAManaged : RSA
    {
      // Fields:
  defaultKeySize : Int32
  isCRTpossible : Boolean
  keyBlinding : Boolean
  keypairGenerated : Boolean
  m_disposed : Boolean
  d : BigInteger
  p : BigInteger
  q : BigInteger
  dp : BigInteger
  dq : BigInteger
  qInv : BigInteger
  n : BigInteger
  e : BigInteger
  KeyGenerated : KeyGeneratedEventHandler
      // Properties:
  KeySize : Int32
  KeyExchangeAlgorithm : String
  PublicOnly : Boolean
  SignatureAlgorithm : String
  UseKeyBlinding : Boolean
  IsCrtPossible : Boolean
      // Events:
      KeyGenerated : KeyGeneratedEventHandler
      // Methods:
      public Void Mono.Security.Cryptography.RSAManaged::.ctor()
      public Void Mono.Security.Cryptography.RSAManaged::.ctorInt32)
      public Void Mono.Security.Cryptography.RSAManaged::add_KeyGenerated(Mono.Security.Cryptography.RSAManaged/KeyGeneratedEventHandler)
      public Void Mono.Security.Cryptography.RSAManaged::remove_KeyGenerated(Mono.Security.Cryptography.RSAManaged/KeyGeneratedEventHandler)
      Void Mono.Security.Cryptography.RSAManaged::Finalize()
      Void Mono.Security.Cryptography.RSAManaged::GenerateKeyPair()
      public Int32 Mono.Security.Cryptography.RSAManaged::get_KeySize()
      public String Mono.Security.Cryptography.RSAManaged::get_KeyExchangeAlgorithm()
      public Boolean Mono.Security.Cryptography.RSAManaged::get_PublicOnly()
      public String Mono.Security.Cryptography.RSAManaged::get_SignatureAlgorithm()
      public Byte[] Mono.Security.Cryptography.RSAManaged::DecryptValueByte[])
      public Byte[] Mono.Security.Cryptography.RSAManaged::EncryptValueByte[])
      public Security.Cryptography.RSAParameters Mono.Security.Cryptography.RSAManaged::ExportParametersBoolean)
      public Void Mono.Security.Cryptography.RSAManaged::ImportParametersSecurity.Cryptography.RSAParameters)
      Void Mono.Security.Cryptography.RSAManaged::DisposeBoolean)
      public String Mono.Security.Cryptography.RSAManaged::ToXmlStringBoolean)
      public Boolean Mono.Security.Cryptography.RSAManaged::get_UseKeyBlinding()
      public Void Mono.Security.Cryptography.RSAManaged::set_UseKeyBlindingBoolean)
      public Boolean Mono.Security.Cryptography.RSAManaged::get_IsCrtPossible()
      Byte[] Mono.Security.Cryptography.RSAManaged::GetPaddedValue(Mono.Math.BigIntegerInt32)
    }
}
