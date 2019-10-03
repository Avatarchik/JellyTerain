// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class RSACryptoServiceProvider : RSA
    {
      // Fields:
  PROV_RSA_FULL : Int32
  store : KeyPairPersistence
  persistKey : Boolean
  persisted : Boolean
  privateKeyExportable : Boolean
  m_disposed : Boolean
  rsa : RSAManaged
  useMachineKeyStore : Boolean
  <>f__switch$map29 : Dictionary`2
      // Properties:
  UseMachineKeyStore : Boolean
  KeyExchangeAlgorithm : String
  KeySize : Int32
  PersistKeyInCsp : Boolean
  PublicOnly : Boolean
  SignatureAlgorithm : String
  CspKeyContainerInfo : CspKeyContainerInfo
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::.ctor()
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::.ctorSecurity.Cryptography.CspParameters)
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::.ctorInt32)
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::.ctorInt32Security.Cryptography.CspParameters)
      VoidSecurity.Cryptography.RSACryptoServiceProvider::.cctor()
      VoidSecurity.Cryptography.RSACryptoServiceProvider::CommonInt32Security.Cryptography.CspParameters)
      public BooleanSecurity.Cryptography.RSACryptoServiceProvider::get_UseMachineKeyStore()
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::set_UseMachineKeyStoreBoolean)
      VoidSecurity.Cryptography.RSACryptoServiceProvider::Finalize()
      public StringSecurity.Cryptography.RSACryptoServiceProvider::get_KeyExchangeAlgorithm()
      public Int32Security.Cryptography.RSACryptoServiceProvider::get_KeySize()
      public BooleanSecurity.Cryptography.RSACryptoServiceProvider::get_PersistKeyInCsp()
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::set_PersistKeyInCspBoolean)
      public BooleanSecurity.Cryptography.RSACryptoServiceProvider::get_PublicOnly()
      public StringSecurity.Cryptography.RSACryptoServiceProvider::get_SignatureAlgorithm()
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::DecryptByte[]Boolean)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::DecryptValueByte[])
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::EncryptByte[]Boolean)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::EncryptValueByte[])
      public Security.Cryptography.RSAParametersSecurity.Cryptography.RSACryptoServiceProvider::ExportParametersBoolean)
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::ImportParametersSecurity.Cryptography.RSAParameters)
      Security.Cryptography.HashAlgorithmSecurity.Cryptography.RSACryptoServiceProvider::GetHashObject)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::SignDataByte[]Object)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::SignDataIO.StreamObject)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::SignDataByte[]Int32Int32Object)
      StringSecurity.Cryptography.RSACryptoServiceProvider::GetHashNameFromOIDString)
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::SignHashByte[]String)
      public BooleanSecurity.Cryptography.RSACryptoServiceProvider::VerifyDataByte[]ObjectByte[])
      public BooleanSecurity.Cryptography.RSACryptoServiceProvider::VerifyHashByte[]StringByte[])
      VoidSecurity.Cryptography.RSACryptoServiceProvider::DisposeBoolean)
      VoidSecurity.Cryptography.RSACryptoServiceProvider::OnKeyGeneratedObjectEventArgs)
      public Security.Cryptography.CspKeyContainerInfoSecurity.Cryptography.RSACryptoServiceProvider::get_CspKeyContainerInfo()
      public Byte[]Security.Cryptography.RSACryptoServiceProvider::ExportCspBlobBoolean)
      public VoidSecurity.Cryptography.RSACryptoServiceProvider::ImportCspBlobByte[])
    }
}
