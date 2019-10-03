// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Crl : Object
    {
      // Fields:
  issuer : String
  version : Byte
  thisUpdate : DateTime
  nextUpdate : DateTime
  entries : ArrayList
  signatureOID : String
  signature : Byte[]
  extensions : X509ExtensionCollection
  encoded : Byte[]
  hash_value : Byte[]
  <>f__switch$map11 : Dictionary`2
  <>f__switch$map12 : Dictionary`2
      // Properties:
  Entries : ArrayList
  Item : X509CrlEntry
  Item : X509CrlEntry
  Extensions : X509ExtensionCollection
  Hash : Byte[]
  IssuerName : String
  NextUpdate : DateTime
  ThisUpdate : DateTime
  SignatureAlgorithm : String
  Signature : Byte[]
  RawData : Byte[]
  Version : Byte
  IsCurrent : Boolean
      // Events:
      // Methods:
      public Void Mono.Security.X509.X509Crl::.ctorByte[])
      Void Mono.Security.X509.X509Crl::ParseByte[])
      public Collections.ArrayList Mono.Security.X509.X509Crl::get_Entries()
      public Mono.Security.X509.X509Crl/X509CrlEntry Mono.Security.X509.X509Crl::get_ItemInt32)
      public Mono.Security.X509.X509Crl/X509CrlEntry Mono.Security.X509.X509Crl::get_ItemByte[])
      public Mono.Security.X509.X509ExtensionCollection Mono.Security.X509.X509Crl::get_Extensions()
      public Byte[] Mono.Security.X509.X509Crl::get_Hash()
      public String Mono.Security.X509.X509Crl::get_IssuerName()
      public DateTime Mono.Security.X509.X509Crl::get_NextUpdate()
      public DateTime Mono.Security.X509.X509Crl::get_ThisUpdate()
      public String Mono.Security.X509.X509Crl::get_SignatureAlgorithm()
      public Byte[] Mono.Security.X509.X509Crl::get_Signature()
      public Byte[] Mono.Security.X509.X509Crl::get_RawData()
      public Byte Mono.Security.X509.X509Crl::get_Version()
      public Boolean Mono.Security.X509.X509Crl::get_IsCurrent()
      public Boolean Mono.Security.X509.X509Crl::WasCurrentDateTime)
      public Byte[] Mono.Security.X509.X509Crl::GetBytes()
      Boolean Mono.Security.X509.X509Crl::CompareByte[]Byte[])
      public Mono.Security.X509.X509Crl/X509CrlEntry Mono.Security.X509.X509Crl::GetCrlEntry(Mono.Security.X509.X509Certificate)
      public Mono.Security.X509.X509Crl/X509CrlEntry Mono.Security.X509.X509Crl::GetCrlEntryByte[])
      public Boolean Mono.Security.X509.X509Crl::VerifySignature(Mono.Security.X509.X509Certificate)
      String Mono.Security.X509.X509Crl::GetHashName()
      Boolean Mono.Security.X509.X509Crl::VerifySignatureSecurity.Cryptography.DSA)
      Boolean Mono.Security.X509.X509Crl::VerifySignatureSecurity.Cryptography.RSA)
      public Boolean Mono.Security.X509.X509Crl::VerifySignatureSecurity.Cryptography.AsymmetricAlgorithm)
      public Mono.Security.X509.X509Crl Mono.Security.X509.X509Crl::CreateFromFileString)
    }
}
