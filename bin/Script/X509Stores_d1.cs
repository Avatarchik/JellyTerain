// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Stores : Object
    {
      // Fields:
  _storePath : String
  _personal : X509Store
  _other : X509Store
  _intermediate : X509Store
  _trusted : X509Store
  _untrusted : X509Store
      // Properties:
  Personal : X509Store
  OtherPeople : X509Store
  IntermediateCA : X509Store
  TrustedRoot : X509Store
  Untrusted : X509Store
      // Events:
      // Methods:
      Void Mono.Security.X509.X509Stores::.ctorString)
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::get_Personal()
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::get_OtherPeople()
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::get_IntermediateCA()
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::get_TrustedRoot()
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::get_Untrusted()
      public Void Mono.Security.X509.X509Stores::Clear()
      public Mono.Security.X509.X509Store Mono.Security.X509.X509Stores::OpenStringBoolean)
    }
}
