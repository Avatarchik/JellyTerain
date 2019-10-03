// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security
{
    class ASN1 : Object
    {
      // Fields:
  m_nTag : Byte
  m_aValue : Byte[]
  elist : ArrayList
      // Properties:
  Count : Int32
  Tag : Byte
  Length : Int32
  Value : Byte[]
  Item : ASN1
      // Events:
      // Methods:
      public Void Mono.Security.ASN1::.ctor()
      public Void Mono.Security.ASN1::.ctorByte)
      public Void Mono.Security.ASN1::.ctorByteByte[])
      public Void Mono.Security.ASN1::.ctorByte[])
      public Int32 Mono.Security.ASN1::get_Count()
      public Byte Mono.Security.ASN1::get_Tag()
      public Int32 Mono.Security.ASN1::get_Length()
      public Byte[] Mono.Security.ASN1::get_Value()
      public Void Mono.Security.ASN1::set_ValueByte[])
      Boolean Mono.Security.ASN1::CompareArrayByte[]Byte[])
      public Boolean Mono.Security.ASN1::EqualsByte[])
      public Boolean Mono.Security.ASN1::CompareValueByte[])
      public Mono.Security.ASN1 Mono.Security.ASN1::Add(Mono.Security.ASN1)
      public Byte[] Mono.Security.ASN1::GetBytes()
      Void Mono.Security.ASN1::DecodeByte[]Int32&Int32)
      Void Mono.Security.ASN1::DecodeTLVByte[]Int32&Byte&Int32&Byte[]&)
      public Mono.Security.ASN1 Mono.Security.ASN1::get_ItemInt32)
      public Mono.Security.ASN1 Mono.Security.ASN1::ElementInt32Byte)
      public String Mono.Security.ASN1::ToString()
      public Void Mono.Security.ASN1::SaveToFileString)
    }
}
