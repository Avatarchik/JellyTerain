// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    class SHA1Internal : Object
    {
      // Fields:
  BLOCK_SIZE_BYTES : Int32
  HASH_SIZE_BYTES : Int32
  _H : UInt32[]
  count : UInt64
  _ProcessingBuffer : Byte[]
  _ProcessingBufferCount : Int32
  buff : UInt32[]
      // Properties:
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.SHA1Internal::.ctor()
      public VoidSecurity.Cryptography.SHA1Internal::HashCoreByte[]Int32Int32)
      public Byte[]Security.Cryptography.SHA1Internal::HashFinal()
      public VoidSecurity.Cryptography.SHA1Internal::Initialize()
      VoidSecurity.Cryptography.SHA1Internal::ProcessBlockByte[]UInt32)
      VoidSecurity.Cryptography.SHA1Internal::InitialiseBuffUInt32[]Byte[]UInt32)
      VoidSecurity.Cryptography.SHA1Internal::FillBuffUInt32[])
      VoidSecurity.Cryptography.SHA1Internal::ProcessFinalBlockByte[]Int32Int32)
      VoidSecurity.Cryptography.SHA1Internal::AddLengthUInt64Byte[]Int32)
    }
}
