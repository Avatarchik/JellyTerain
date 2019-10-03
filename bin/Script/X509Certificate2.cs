// Class info from System.dll
// 
using UnityEngine;

namespace System.Security.Cryptography.X509Certificates
{
    public class X509Certificate2 : X509Certificate
    {
      // Fields:
  _archived : Boolean
  _extensions : X509ExtensionCollection
  _name : String
  _serial : String
  _publicKey : PublicKey
  issuer_name : X500DistinguishedName
  subject_name : X500DistinguishedName
  signature_algorithm : Oid
  _cert : X509Certificate
  empty_error : String
  commonName : Byte[]
  email : Byte[]
  signedData : Byte[]
      // Properties:
  Archived : Boolean
  Extensions : X509ExtensionCollection
  FriendlyName : String
  HasPrivateKey : Boolean
  IssuerName : X500DistinguishedName
  NotAfter : DateTime
  NotBefore : DateTime
  PrivateKey : AsymmetricAlgorithm
  PublicKey : PublicKey
  RawData : Byte[]
  SerialNumber : String
  SignatureAlgorithm : Oid
  SubjectName : X500DistinguishedName
  Thumbprint : String
  Version : Int32
  MonoCertificate : X509Certificate
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctor()
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorByte[])
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorByte[]String)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorByte[]Security.SecureString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorByte[]StringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorByte[]Security.SecureStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorStringString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorStringSecurity.SecureString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorStringStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorStringSecurity.SecureStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorIntPtr)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.ctorSecurity.Cryptography.X509Certificates.X509Certificate)
      VoidSecurity.Cryptography.X509Certificates.X509Certificate2::.cctor()
      public BooleanSecurity.Cryptography.X509Certificates.X509Certificate2::get_Archived()
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::set_ArchivedBoolean)
      public Security.Cryptography.X509Certificates.X509ExtensionCollectionSecurity.Cryptography.X509Certificates.X509Certificate2::get_Extensions()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::get_FriendlyName()
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::set_FriendlyNameString)
      public BooleanSecurity.Cryptography.X509Certificates.X509Certificate2::get_HasPrivateKey()
      public Security.Cryptography.X509Certificates.X500DistinguishedNameSecurity.Cryptography.X509Certificates.X509Certificate2::get_IssuerName()
      public DateTimeSecurity.Cryptography.X509Certificates.X509Certificate2::get_NotAfter()
      public DateTimeSecurity.Cryptography.X509Certificates.X509Certificate2::get_NotBefore()
      public Security.Cryptography.AsymmetricAlgorithmSecurity.Cryptography.X509Certificates.X509Certificate2::get_PrivateKey()
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::set_PrivateKeySecurity.Cryptography.AsymmetricAlgorithm)
      public Security.Cryptography.X509Certificates.PublicKeySecurity.Cryptography.X509Certificates.X509Certificate2::get_PublicKey()
      public Byte[]Security.Cryptography.X509Certificates.X509Certificate2::get_RawData()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::get_SerialNumber()
      public Security.Cryptography.OidSecurity.Cryptography.X509Certificates.X509Certificate2::get_SignatureAlgorithm()
      public Security.Cryptography.X509Certificates.X500DistinguishedNameSecurity.Cryptography.X509Certificates.X509Certificate2::get_SubjectName()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::get_Thumbprint()
      public Int32Security.Cryptography.X509Certificates.X509Certificate2::get_Version()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::GetNameInfoSecurity.Cryptography.X509Certificates.X509NameTypeBoolean)
      Mono.Security.ASN1Security.Cryptography.X509Certificates.X509Certificate2::FindByte[],Mono.Security.ASN1)
      StringSecurity.Cryptography.X509Certificates.X509Certificate2::GetValueAsString(Mono.Security.ASN1)
      VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportPkcs12Byte[]String)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportByte[])
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportByte[]StringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportByte[]Security.SecureStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportStringStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::ImportStringSecurity.SecureStringSecurity.Cryptography.X509Certificates.X509KeyStorageFlags)
      Byte[]Security.Cryptography.X509Certificates.X509Certificate2::LoadString)
      public VoidSecurity.Cryptography.X509Certificates.X509Certificate2::Reset()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::ToString()
      public StringSecurity.Cryptography.X509Certificates.X509Certificate2::ToStringBoolean)
      VoidSecurity.Cryptography.X509Certificates.X509Certificate2::AppendBufferText.StringBuilderByte[])
      public BooleanSecurity.Cryptography.X509Certificates.X509Certificate2::Verify()
      public Security.Cryptography.X509Certificates.X509ContentTypeSecurity.Cryptography.X509Certificates.X509Certificate2::GetCertContentTypeByte[])
      public Security.Cryptography.X509Certificates.X509ContentTypeSecurity.Cryptography.X509Certificates.X509Certificate2::GetCertContentTypeString)
      Mono.Security.X509.X509CertificateSecurity.Cryptography.X509Certificates.X509Certificate2::get_MonoCertificate()
    }
}
