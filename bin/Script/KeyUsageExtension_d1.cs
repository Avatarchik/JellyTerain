// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509.Extensions
{
    class KeyUsageExtension : X509Extension
    {
      // Fields:
  kubits : Int32
      // Properties:
  KeyUsage : KeyUsages
  Name : String
      // Events:
      // Methods:
      public Void Mono.Security.X509.Extensions.KeyUsageExtension::.ctor(Mono.Security.ASN1)
      public Void Mono.Security.X509.Extensions.KeyUsageExtension::.ctor(Mono.Security.X509.X509Extension)
      public Void Mono.Security.X509.Extensions.KeyUsageExtension::.ctor()
      Void Mono.Security.X509.Extensions.KeyUsageExtension::Decode()
      Void Mono.Security.X509.Extensions.KeyUsageExtension::Encode()
      public Mono.Security.X509.Extensions.KeyUsages Mono.Security.X509.Extensions.KeyUsageExtension::get_KeyUsage()
      public Void Mono.Security.X509.Extensions.KeyUsageExtension::set_KeyUsage(Mono.Security.X509.Extensions.KeyUsages)
      public String Mono.Security.X509.Extensions.KeyUsageExtension::get_Name()
      public Boolean Mono.Security.X509.Extensions.KeyUsageExtension::Support(Mono.Security.X509.Extensions.KeyUsages)
      public String Mono.Security.X509.Extensions.KeyUsageExtension::ToString()
    }
}
