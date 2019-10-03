// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509.Extensions
{
    class BasicConstraintsExtension : X509Extension
    {
      // Fields:
  public NoPathLengthConstraint : Int32
  cA : Boolean
  pathLenConstraint : Int32
      // Properties:
  CertificateAuthority : Boolean
  Name : String
  PathLenConstraint : Int32
      // Events:
      // Methods:
      public Void Mono.Security.X509.Extensions.BasicConstraintsExtension::.ctor()
      public Void Mono.Security.X509.Extensions.BasicConstraintsExtension::.ctor(Mono.Security.ASN1)
      public Void Mono.Security.X509.Extensions.BasicConstraintsExtension::.ctor(Mono.Security.X509.X509Extension)
      Void Mono.Security.X509.Extensions.BasicConstraintsExtension::Decode()
      Void Mono.Security.X509.Extensions.BasicConstraintsExtension::Encode()
      public Boolean Mono.Security.X509.Extensions.BasicConstraintsExtension::get_CertificateAuthority()
      public Void Mono.Security.X509.Extensions.BasicConstraintsExtension::set_CertificateAuthorityBoolean)
      public String Mono.Security.X509.Extensions.BasicConstraintsExtension::get_Name()
      public Int32 Mono.Security.X509.Extensions.BasicConstraintsExtension::get_PathLenConstraint()
      public Void Mono.Security.X509.Extensions.BasicConstraintsExtension::set_PathLenConstraintInt32)
      public String Mono.Security.X509.Extensions.BasicConstraintsExtension::ToString()
    }
}
