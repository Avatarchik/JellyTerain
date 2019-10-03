// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Certificate : Object
    {
      // Fields:
  decoder : ASN1
  m_encodedcert : Byte[]
  m_from : DateTime
  m_until : DateTime
  issuer : ASN1
  m_issuername : String
  m_keyalgo : String
  m_keyalgoparams : Byte[]
  subject : ASN1
  m_subject : String
  m_publickey : Byte[]
  signature : Byte[]
  m_signaturealgo : String
  m_signaturealgoparams : Byte[]
  certhash : Byte[]
  _rsa : RSA
  _dsa : DSA
  version : Int32
  serialnumber : Byte[]
  issuerUniqueID : Byte[]
  subjectUniqueID : Byte[]
  extensions : X509ExtensionCollection
  encoding_error : String
  <>f__switch$map13 : Dictionary`2
  <>f__switch$map14 : Dictionary`2
  <>f__switch$map15 : Dictionary`2
      // Properties:
  DSA : DSA
  Extensions : X509ExtensionCollection
  Hash : Byte[]
  IssuerName : String
  KeyAlgorithm : String
  KeyAlgorithmParameters : Byte[]
  PublicKey : Byte[]
  RSA : RSA
  RawData : Byte[]
  SerialNumber : Byte[]
  Signature : Byte[]
  SignatureAlgorithm : String
  SignatureAlgorithmParameters : Byte[]
  SubjectName : String
  ValidFrom : DateTime
  ValidUntil : DateTime
  Version : Int32
  IsCurrent : Boolean
  IssuerUniqueIdentifier : Byte[]
  SubjectUniqueIdentifier : Byte[]
  IsSelfSigned : Boolean
      // Events:
      // Methods:
      public Void Mono.Security.X509.X509Certificate::.ctorByte[])
      Void Mono.Security.X509.X509Certificate::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      Void Mono.Security.X509.X509Certificate::.cctor()
      Void Mono.Security.X509.X509Certificate::ParseByte[])
      Byte[] Mono.Security.X509.X509Certificate::GetUnsignedBigIntegerByte[])
      public Security.Cryptography.DSA Mono.Security.X509.X509Certificate::get_DSA()
      public Void Mono.Security.X509.X509Certificate::set_DSASecurity.Cryptography.DSA)
      public Mono.Security.X509.X509ExtensionCollection Mono.Security.X509.X509Certificate::get_Extensions()
      public Byte[] Mono.Security.X509.X509Certificate::get_Hash()
      public String Mono.Security.X509.X509Certificate::get_IssuerName()
      public String Mono.Security.X509.X509Certificate::get_KeyAlgorithm()
      public Byte[] Mono.Security.X509.X509Certificate::get_KeyAlgorithmParameters()
      public Void Mono.Security.X509.X509Certificate::set_KeyAlgorithmParametersByte[])
      public Byte[] Mono.Security.X509.X509Certificate::get_PublicKey()
      public Security.Cryptography.RSA Mono.Security.X509.X509Certificate::get_RSA()
      public Void Mono.Security.X509.X509Certificate::set_RSASecurity.Cryptography.RSA)
      public Byte[] Mono.Security.X509.X509Certificate::get_RawData()
      public Byte[] Mono.Security.X509.X509Certificate::get_SerialNumber()
      public Byte[] Mono.Security.X509.X509Certificate::get_Signature()
      public String Mono.Security.X509.X509Certificate::get_SignatureAlgorithm()
      public Byte[] Mono.Security.X509.X509Certificate::get_SignatureAlgorithmParameters()
      public String Mono.Security.X509.X509Certificate::get_SubjectName()
      public DateTime Mono.Security.X509.X509Certificate::get_ValidFrom()
      public DateTime Mono.Security.X509.X509Certificate::get_ValidUntil()
      public Int32 Mono.Security.X509.X509Certificate::get_Version()
      public Boolean Mono.Security.X509.X509Certificate::get_IsCurrent()
      public Boolean Mono.Security.X509.X509Certificate::WasCurrentDateTime)
      public Byte[] Mono.Security.X509.X509Certificate::get_IssuerUniqueIdentifier()
      public Byte[] Mono.Security.X509.X509Certificate::get_SubjectUniqueIdentifier()
      Boolean Mono.Security.X509.X509Certificate::VerifySignatureSecurity.Cryptography.DSA)
      String Mono.Security.X509.X509Certificate::GetHashNameFromOIDString)
      Boolean Mono.Security.X509.X509Certificate::VerifySignatureSecurity.Cryptography.RSA)
      public Boolean Mono.Security.X509.X509Certificate::VerifySignatureSecurity.Cryptography.AsymmetricAlgorithm)
      public Boolean Mono.Security.X509.X509Certificate::CheckSignatureByte[]StringByte[])
      public Boolean Mono.Security.X509.X509Certificate::get_IsSelfSigned()
      public Mono.Security.ASN1 Mono.Security.X509.X509Certificate::GetIssuerName()
      public Mono.Security.ASN1 Mono.Security.X509.X509Certificate::GetSubjectName()
      public Void Mono.Security.X509.X509Certificate::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      Byte[] Mono.Security.X509.X509Certificate::PEMStringByte[])
    }
}
