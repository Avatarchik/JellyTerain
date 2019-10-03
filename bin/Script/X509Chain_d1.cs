// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Chain : Object
    {
      // Fields:
  roots : X509CertificateCollection
  certs : X509CertificateCollection
  _root : X509Certificate
  _chain : X509CertificateCollection
  _status : X509ChainStatusFlags
      // Properties:
  Chain : X509CertificateCollection
  Root : X509Certificate
  Status : X509ChainStatusFlags
  TrustAnchors : X509CertificateCollection
      // Events:
      // Methods:
      public Void Mono.Security.X509.X509Chain::.ctor()
      public Void Mono.Security.X509.X509Chain::.ctor(Mono.Security.X509.X509CertificateCollection)
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509Chain::get_Chain()
      public Mono.Security.X509.X509Certificate Mono.Security.X509.X509Chain::get_Root()
      public Mono.Security.X509.X509ChainStatusFlags Mono.Security.X509.X509Chain::get_Status()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509Chain::get_TrustAnchors()
      public Void Mono.Security.X509.X509Chain::set_TrustAnchors(Mono.Security.X509.X509CertificateCollection)
      public Void Mono.Security.X509.X509Chain::LoadCertificate(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.X509Chain::LoadCertificates(Mono.Security.X509.X509CertificateCollection)
      public Mono.Security.X509.X509Certificate Mono.Security.X509.X509Chain::FindByIssuerNameString)
      public Boolean Mono.Security.X509.X509Chain::Build(Mono.Security.X509.X509Certificate)
      public Void Mono.Security.X509.X509Chain::Reset()
      Boolean Mono.Security.X509.X509Chain::IsValid(Mono.Security.X509.X509Certificate)
      Mono.Security.X509.X509Certificate Mono.Security.X509.X509Chain::FindCertificateParent(Mono.Security.X509.X509Certificate)
      Mono.Security.X509.X509Certificate Mono.Security.X509.X509Chain::FindCertificateRoot(Mono.Security.X509.X509Certificate)
      Boolean Mono.Security.X509.X509Chain::IsTrusted(Mono.Security.X509.X509Certificate)
      Boolean Mono.Security.X509.X509Chain::IsParent(Mono.Security.X509.X509Certificate,Mono.Security.X509.X509Certificate)
    }
}
