// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Authenticode
{
    class AuthenticodeBase : Object
    {
      // Fields:
  public spcIndirectDataContext : String
  fileblock : Byte[]
  fs : FileStream
  blockNo : Int32
  blockLength : Int32
  peOffset : Int32
  dirSecurityOffset : Int32
  dirSecuritySize : Int32
  coffSymbolTableOffset : Int32
      // Properties:
  PEOffset : Int32
  CoffSymbolTableOffset : Int32
  SecurityOffset : Int32
      // Events:
      // Methods:
      public Void Mono.Security.Authenticode.AuthenticodeBase::.ctor()
      Int32 Mono.Security.Authenticode.AuthenticodeBase::get_PEOffset()
      Int32 Mono.Security.Authenticode.AuthenticodeBase::get_CoffSymbolTableOffset()
      Int32 Mono.Security.Authenticode.AuthenticodeBase::get_SecurityOffset()
      Void Mono.Security.Authenticode.AuthenticodeBase::OpenString)
      Void Mono.Security.Authenticode.AuthenticodeBase::Close()
      Boolean Mono.Security.Authenticode.AuthenticodeBase::ReadFirstBlock()
      Byte[] Mono.Security.Authenticode.AuthenticodeBase::GetSecurityEntry()
      Byte[] Mono.Security.Authenticode.AuthenticodeBase::GetHashSecurity.Cryptography.HashAlgorithm)
      Byte[] Mono.Security.Authenticode.AuthenticodeBase::HashFileStringString)
    }
}
