// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Authenticode
{
    class AuthenticodeDeformatter : AuthenticodeBase
    {
      // Fields:
  filename : String
  hash : Byte[]
  coll : X509CertificateCollection
  signedHash : ASN1
  timestamp : DateTime
  signingCertificate : X509Certificate
  reason : Int32
  trustedRoot : Boolean
  trustedTimestampRoot : Boolean
  entry : Byte[]
  signerChain : X509Chain
  timestampChain : X509Chain
  <>f__switch$map5 : Dictionary`2
  <>f__switch$map6 : Dictionary`2
  <>f__switch$map7 : Dictionary`2
      // Properties:
  FileName : String
  Hash : Byte[]
  Reason : Int32
  Signature : Byte[]
  Timestamp : DateTime
  Certificates : X509CertificateCollection
  SigningCertificate : X509Certificate
      // Events:
      // Methods:
      public Void Mono.Security.Authenticode.AuthenticodeDeformatter::.ctor()
      public Void Mono.Security.Authenticode.AuthenticodeDeformatter::.ctorString)
      public String Mono.Security.Authenticode.AuthenticodeDeformatter::get_FileName()
      public Void Mono.Security.Authenticode.AuthenticodeDeformatter::set_FileNameString)
      public Byte[] Mono.Security.Authenticode.AuthenticodeDeformatter::get_Hash()
      public Int32 Mono.Security.Authenticode.AuthenticodeDeformatter::get_Reason()
      public Boolean Mono.Security.Authenticode.AuthenticodeDeformatter::IsTrusted()
      public Byte[] Mono.Security.Authenticode.AuthenticodeDeformatter::get_Signature()
      public DateTime Mono.Security.Authenticode.AuthenticodeDeformatter::get_Timestamp()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.Authenticode.AuthenticodeDeformatter::get_Certificates()
      public Mono.Security.X509.X509Certificate Mono.Security.Authenticode.AuthenticodeDeformatter::get_SigningCertificate()
      Boolean Mono.Security.Authenticode.AuthenticodeDeformatter::CheckSignatureString)
      Boolean Mono.Security.Authenticode.AuthenticodeDeformatter::CompareIssuerSerialStringByte[],Mono.Security.X509.X509Certificate)
      Boolean Mono.Security.Authenticode.AuthenticodeDeformatter::VerifySignature(Mono.Security.PKCS7/SignedDataByte[]Security.Cryptography.HashAlgorithm)
      Boolean Mono.Security.Authenticode.AuthenticodeDeformatter::VerifyCounterSignature(Mono.Security.PKCS7/SignerInfoByte[])
      Void Mono.Security.Authenticode.AuthenticodeDeformatter::Reset()
    }
}
