// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class PasswordDeriveBytes : DeriveBytes
    {
      // Fields:
  HashNameValue : String
  SaltValue : Byte[]
  IterationsValue : Int32
  hash : HashAlgorithm
  state : Int32
  password : Byte[]
  initial : Byte[]
  output : Byte[]
  position : Int32
  hashnumber : Int32
      // Properties:
  HashName : String
  IterationCount : Int32
  Salt : Byte[]
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorStringByte[])
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorStringByte[]Security.Cryptography.CspParameters)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorStringByte[]StringInt32)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorStringByte[]StringInt32Security.Cryptography.CspParameters)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorByte[]Byte[])
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorByte[]Byte[]Security.Cryptography.CspParameters)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorByte[]Byte[]StringInt32)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::.ctorByte[]Byte[]StringInt32Security.Cryptography.CspParameters)
      VoidSecurity.Cryptography.PasswordDeriveBytes::Finalize()
      VoidSecurity.Cryptography.PasswordDeriveBytes::PrepareStringByte[]StringInt32)
      VoidSecurity.Cryptography.PasswordDeriveBytes::PrepareByte[]Byte[]StringInt32)
      public StringSecurity.Cryptography.PasswordDeriveBytes::get_HashName()
      public VoidSecurity.Cryptography.PasswordDeriveBytes::set_HashNameString)
      public Int32Security.Cryptography.PasswordDeriveBytes::get_IterationCount()
      public VoidSecurity.Cryptography.PasswordDeriveBytes::set_IterationCountInt32)
      public Byte[]Security.Cryptography.PasswordDeriveBytes::get_Salt()
      public VoidSecurity.Cryptography.PasswordDeriveBytes::set_SaltByte[])
      public Byte[]Security.Cryptography.PasswordDeriveBytes::CryptDeriveKeyStringStringInt32Byte[])
      public Byte[]Security.Cryptography.PasswordDeriveBytes::GetBytesInt32)
      public VoidSecurity.Cryptography.PasswordDeriveBytes::Reset()
    }
}
