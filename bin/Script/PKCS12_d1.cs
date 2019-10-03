// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class PKCS12 : Object
    {
      // Fields:
  public pbeWithSHAAnd128BitRC4 : String
  public pbeWithSHAAnd40BitRC4 : String
  public pbeWithSHAAnd3KeyTripleDESCBC : String
  public pbeWithSHAAnd2KeyTripleDESCBC : String
  public pbeWithSHAAnd128BitRC2CBC : String
  public pbeWithSHAAnd40BitRC2CBC : String
  public keyBag : String
  public pkcs8ShroudedKeyBag : String
  public certBag : String
  public crlBag : String
  public secretBag : String
  public safeContentsBag : String
  public x509Certificate : String
  public sdsiCertificate : String
  public x509Crl : String
  public CryptoApiPasswordLimit : Int32
  recommendedIterationCount : Int32
  _password : Byte[]
  _keyBags : ArrayList
  _secretBags : ArrayList
  _certs : X509CertificateCollection
  _keyBagsChanged : Boolean
  _secretBagsChanged : Boolean
  _certsChanged : Boolean
  _iterations : Int32
  _safeBags : ArrayList
  _rng : RandomNumberGenerator
  password_max_length : Int32
  <>f__switch$map8 : Dictionary`2
  <>f__switch$map9 : Dictionary`2
  <>f__switch$mapA : Dictionary`2
  <>f__switch$mapB : Dictionary`2
  <>f__switch$mapC : Dictionary`2
  <>f__switch$mapD : Dictionary`2
  <>f__switch$mapE : Dictionary`2
  <>f__switch$mapF : Dictionary`2
      // Properties:
  Password : String
  IterationCount : Int32
  Keys : ArrayList
  Secrets : ArrayList
  Certificates : X509CertificateCollection
  RNG : RandomNumberGenerator
  MaximumPasswordLength : Int32
      // Events:
      // Methods:
      public Void Mono.Security.X509.PKCS12::.ctor()
      public Void Mono.Security.X509.PKCS12::.ctorByte[])
      public Void Mono.Security.X509.PKCS12::.ctorByte[]String)
      public Void Mono.Security.X509.PKCS12::.ctorByte[]Byte[])
      Void Mono.Security.X509.PKCS12::.cctor()
      Void Mono.Security.X509.PKCS12::DecodeByte[])
      Void Mono.Security.X509.PKCS12::Finalize()
      public Void Mono.Security.X509.PKCS12::set_PasswordString)
      public Int32 Mono.Security.X509.PKCS12::get_IterationCount()
      public Void Mono.Security.X509.PKCS12::set_IterationCountInt32)
      public Collections.ArrayList Mono.Security.X509.PKCS12::get_Keys()
      public Collections.ArrayList Mono.Security.X509.PKCS12::get_Secrets()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.PKCS12::get_Certificates()
      Security.Cryptography.RandomNumberGenerator Mono.Security.X509.PKCS12::get_RNG()
      Boolean Mono.Security.X509.PKCS12::CompareByte[]Byte[])
      Security.Cryptography.SymmetricAlgorithm Mono.Security.X509.PKCS12::GetSymmetricAlgorithmStringByte[]Int32)
      public Byte[] Mono.Security.X509.PKCS12::DecryptStringByte[]Int32Byte[])
      public Byte[] Mono.Security.X509.PKCS12::Decrypt(Mono.Security.PKCS7/EncryptedData)
      public Byte[] Mono.Security.X509.PKCS12::EncryptStringByte[]Int32Byte[])
      Security.Cryptography.DSAParameters Mono.Security.X509.PKCS12::GetExistingParametersBoolean&)
      Void Mono.Security.X509.PKCS12::AddPrivateKey(Mono.Security.Cryptography.PKCS8/PrivateKeyInfo)
      Void Mono.Security.X509.PKCS12::ReadSafeBag(Mono.Security.ASN1)
      Mono.Security.ASN1 Mono.Security.X509.PKCS12::Pkcs8ShroudedKeyBagSafeBagSecurity.Cryptography.AsymmetricAlgorithmCollections.IDictionary)
      Mono.Security.ASN1 Mono.Security.X509.PKCS12::KeyBagSafeBagSecurity.Cryptography.AsymmetricAlgorithmCollections.IDictionary)
      Mono.Security.ASN1 Mono.Security.X509.PKCS12::SecretBagSafeBagByte[]Collections.IDictionary)
      Mono.Security.ASN1 Mono.Security.X509.PKCS12::CertificateSafeBag(Mono.Security.X509.X509CertificateCollections.IDictionary)
      Byte[] Mono.Security.X509.PKCS12::MACByte[]Byte[]Int32Byte[])
      public Byte[] Mono.Security.X509.PKCS12::GetBytes()
      Mono.Security.PKCS7/ContentInfo Mono.Security.X509.PKCS12::EncryptedContentInfo(Mono.Security.ASN1String)
      public Void Mono.Security.X509.PKCS12::AddCertificate(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.PKCS12::AddCertificate(Mono.Security.X509.X509CertificateCollections.IDictionary)
      public Void Mono.Security.X509.PKCS12::RemoveCertificate(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.PKCS12::RemoveCertificate(Mono.Security.X509.X509CertificateCollections.IDictionary)
      Boolean Mono.Security.X509.PKCS12::CompareAsymmetricAlgorithmSecurity.Cryptography.AsymmetricAlgorithmSecurity.Cryptography.AsymmetricAlgorithm)
      public Void Mono.Security.X509.PKCS12::AddPkcs8ShroudedKeyBagSecurity.Cryptography.AsymmetricAlgorithm)
      public Void Mono.Security.X509.PKCS12::AddPkcs8ShroudedKeyBagSecurity.Cryptography.AsymmetricAlgorithmCollections.IDictionary)
      public Void Mono.Security.X509.PKCS12::RemovePkcs8ShroudedKeyBagSecurity.Cryptography.AsymmetricAlgorithm)
      public Void Mono.Security.X509.PKCS12::AddKeyBagSecurity.Cryptography.AsymmetricAlgorithm)
      public Void Mono.Security.X509.PKCS12::AddKeyBagSecurity.Cryptography.AsymmetricAlgorithmCollections.IDictionary)
      public Void Mono.Security.X509.PKCS12::RemoveKeyBagSecurity.Cryptography.AsymmetricAlgorithm)
      public Void Mono.Security.X509.PKCS12::AddSecretBagByte[])
      public Void Mono.Security.X509.PKCS12::AddSecretBagByte[]Collections.IDictionary)
      public Void Mono.Security.X509.PKCS12::RemoveSecretBagByte[])
      public Security.Cryptography.AsymmetricAlgorithm Mono.Security.X509.PKCS12::GetAsymmetricAlgorithmCollections.IDictionary)
      public Byte[] Mono.Security.X509.PKCS12::GetSecretCollections.IDictionary)
      public Mono.Security.X509.X509Certificate Mono.Security.X509.PKCS12::GetCertificateCollections.IDictionary)
      public Collections.IDictionary Mono.Security.X509.PKCS12::GetAttributesSecurity.Cryptography.AsymmetricAlgorithm)
      public Collections.IDictionary Mono.Security.X509.PKCS12::GetAttributes(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.PKCS12::SaveToFileString)
      public Object Mono.Security.X509.PKCS12::Clone()
      public Int32 Mono.Security.X509.PKCS12::get_MaximumPasswordLength()
      public Void Mono.Security.X509.PKCS12::set_MaximumPasswordLengthInt32)
      Byte[] Mono.Security.X509.PKCS12::LoadFileString)
      public Mono.Security.X509.PKCS12 Mono.Security.X509.PKCS12::LoadFromFileString)
      public Mono.Security.X509.PKCS12 Mono.Security.X509.PKCS12::LoadFromFileStringString)
    }
}
