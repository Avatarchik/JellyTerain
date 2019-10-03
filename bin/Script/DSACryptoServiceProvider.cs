// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class DSACryptoServiceProvider : DSA
    {
      // Fields:
  PROV_DSS_DH : Int32
  store : KeyPairPersistence
  persistKey : Boolean
  persisted : Boolean
  privateKeyExportable : Boolean
  m_disposed : Boolean
  dsa : DSAManaged
  useMachineKeyStore : Boolean
      // Properties:
  KeyExchangeAlgorithm : String
  KeySize : Int32
  PersistKeyInCsp : Boolean
  PublicOnly : Boolean
  SignatureAlgorithm : String
  UseMachineKeyStore : Boolean
  CspKeyContainerInfo : CspKeyContainerInfo
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::.ctor()
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::.ctorSecurity.Cryptography.CspParameters)
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::.ctorInt32)
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::.ctorInt32Security.Cryptography.CspParameters)
      VoidSecurity.Cryptography.DSACryptoServiceProvider::.cctor()
      VoidSecurity.Cryptography.DSACryptoServiceProvider::Finalize()
      public StringSecurity.Cryptography.DSACryptoServiceProvider::get_KeyExchangeAlgorithm()
      public Int32Security.Cryptography.DSACryptoServiceProvider::get_KeySize()
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::get_PersistKeyInCsp()
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::set_PersistKeyInCspBoolean)
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::get_PublicOnly()
      public StringSecurity.Cryptography.DSACryptoServiceProvider::get_SignatureAlgorithm()
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::get_UseMachineKeyStore()
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::set_UseMachineKeyStoreBoolean)
      public Security.Cryptography.DSAParametersSecurity.Cryptography.DSACryptoServiceProvider::ExportParametersBoolean)
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::ImportParametersSecurity.Cryptography.DSAParameters)
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::CreateSignatureByte[])
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::SignDataByte[])
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::SignDataByte[]Int32Int32)
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::SignDataIO.Stream)
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::SignHashByte[]String)
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::VerifyDataByte[]Byte[])
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::VerifyHashByte[]StringByte[])
      public BooleanSecurity.Cryptography.DSACryptoServiceProvider::VerifySignatureByte[]Byte[])
      VoidSecurity.Cryptography.DSACryptoServiceProvider::DisposeBoolean)
      VoidSecurity.Cryptography.DSACryptoServiceProvider::OnKeyGeneratedObjectEventArgs)
      public Security.Cryptography.CspKeyContainerInfoSecurity.Cryptography.DSACryptoServiceProvider::get_CspKeyContainerInfo()
      public Byte[]Security.Cryptography.DSACryptoServiceProvider::ExportCspBlobBoolean)
      public VoidSecurity.Cryptography.DSACryptoServiceProvider::ImportCspBlobByte[])
    }
}
