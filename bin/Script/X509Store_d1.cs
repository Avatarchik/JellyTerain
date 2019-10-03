// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Store : Object
    {
      // Fields:
  _storePath : String
  _certificates : X509CertificateCollection
  _crls : ArrayList
  _crl : Boolean
  _name : String
      // Properties:
  Certificates : X509CertificateCollection
  Crls : ArrayList
  Name : String
      // Events:
      // Methods:
      Void Mono.Security.X509.X509Store::.ctorStringBoolean)
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509Store::get_Certificates()
      public Collections.ArrayList Mono.Security.X509.X509Store::get_Crls()
      public String Mono.Security.X509.X509Store::get_Name()
      public Void Mono.Security.X509.X509Store::Clear()
      public Void Mono.Security.X509.X509Store::Import(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.X509Store::Import(Mono.Security.X509.X509Crl)
      public Void Mono.Security.X509.X509Store::Remove(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.X509Store::Remove(Mono.Security.X509.X509Crl)
      String Mono.Security.X509.X509Store::GetUniqueName(Mono.Security.X509.X509Certificate)
      String Mono.Security.X509.X509Store::GetUniqueName(Mono.Security.X509.X509Crl)
      Byte[] Mono.Security.X509.X509Store::GetUniqueName(Mono.Security.X509.X509ExtensionCollection)
      String Mono.Security.X509.X509Store::GetUniqueNameStringByte[]String)
      Byte[] Mono.Security.X509.X509Store::LoadString)
      Mono.Security.X509.X509Certificate Mono.Security.X509.X509Store::LoadCertificateString)
      Mono.Security.X509.X509Crl Mono.Security.X509.X509Store::LoadCrlString)
      Boolean Mono.Security.X509.X509Store::CheckStoreStringBoolean)
      Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509Store::BuildCertificatesCollectionString)
      Collections.ArrayList Mono.Security.X509.X509Store::BuildCrlsCollectionString)
    }
}
