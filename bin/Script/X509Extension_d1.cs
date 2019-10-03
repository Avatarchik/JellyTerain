// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509Extension : Object
    {
      // Fields:
  extnOid : String
  extnCritical : Boolean
  extnValue : ASN1
      // Properties:
  ASN1 : ASN1
  Oid : String
  Critical : Boolean
  Name : String
  Value : ASN1
      // Events:
      // Methods:
      Void Mono.Security.X509.X509Extension::.ctor()
      public Void Mono.Security.X509.X509Extension::.ctor(Mono.Security.ASN1)
      public Void Mono.Security.X509.X509Extension::.ctor(Mono.Security.X509.X509Extension)
      Void Mono.Security.X509.X509Extension::Decode()
      Void Mono.Security.X509.X509Extension::Encode()
      public Mono.Security.ASN1 Mono.Security.X509.X509Extension::get_ASN1()
      public String Mono.Security.X509.X509Extension::get_Oid()
      public Boolean Mono.Security.X509.X509Extension::get_Critical()
      public Void Mono.Security.X509.X509Extension::set_CriticalBoolean)
      public String Mono.Security.X509.X509Extension::get_Name()
      public Mono.Security.ASN1 Mono.Security.X509.X509Extension::get_Value()
      public Boolean Mono.Security.X509.X509Extension::EqualsObject)
      public Byte[] Mono.Security.X509.X509Extension::GetBytes()
      public Int32 Mono.Security.X509.X509Extension::GetHashCode()
      Void Mono.Security.X509.X509Extension::WriteLineText.StringBuilderInt32Int32)
      public String Mono.Security.X509.X509Extension::ToString()
    }
}
