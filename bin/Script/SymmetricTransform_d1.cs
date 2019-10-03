// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Cryptography
{
    class SymmetricTransform : Object
    {
      // Fields:
  algo : SymmetricAlgorithm
  encrypt : Boolean
  BlockSizeByte : Int32
  temp : Byte[]
  temp2 : Byte[]
  workBuff : Byte[]
  workout : Byte[]
  FeedBackByte : Int32
  FeedBackIter : Int32
  m_disposed : Boolean
  lastBlock : Boolean
  _rng : RandomNumberGenerator
      // Properties:
  CanTransformMultipleBlocks : Boolean
  CanReuseTransform : Boolean
  InputBlockSize : Int32
  OutputBlockSize : Int32
  KeepLastBlock : Boolean
      // Events:
      // Methods:
      public Void Mono.Security.Cryptography.SymmetricTransform::.ctorSecurity.Cryptography.SymmetricAlgorithmBooleanByte[])
      Void Mono.Security.Cryptography.SymmetricTransform::System.IDisposable.Dispose()
      Void Mono.Security.Cryptography.SymmetricTransform::Finalize()
      Void Mono.Security.Cryptography.SymmetricTransform::DisposeBoolean)
      public Boolean Mono.Security.Cryptography.SymmetricTransform::get_CanTransformMultipleBlocks()
      public Boolean Mono.Security.Cryptography.SymmetricTransform::get_CanReuseTransform()
      public Int32 Mono.Security.Cryptography.SymmetricTransform::get_InputBlockSize()
      public Int32 Mono.Security.Cryptography.SymmetricTransform::get_OutputBlockSize()
      Void Mono.Security.Cryptography.SymmetricTransform::TransformByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::ECBByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::CBCByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::CFBByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::OFBByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::CTSByte[]Byte[])
      Void Mono.Security.Cryptography.SymmetricTransform::CheckInputByte[]Int32Int32)
      public Int32 Mono.Security.Cryptography.SymmetricTransform::TransformBlockByte[]Int32Int32Byte[]Int32)
      Boolean Mono.Security.Cryptography.SymmetricTransform::get_KeepLastBlock()
      Int32 Mono.Security.Cryptography.SymmetricTransform::InternalTransformBlockByte[]Int32Int32Byte[]Int32)
      Void Mono.Security.Cryptography.SymmetricTransform::RandomByte[]Int32Int32)
      Void Mono.Security.Cryptography.SymmetricTransform::ThrowBadPaddingExceptionSecurity.Cryptography.PaddingModeInt32Int32)
      Byte[] Mono.Security.Cryptography.SymmetricTransform::FinalEncryptByte[]Int32Int32)
      Byte[] Mono.Security.Cryptography.SymmetricTransform::FinalDecryptByte[]Int32Int32)
      public Byte[] Mono.Security.Cryptography.SymmetricTransform::TransformFinalBlockByte[]Int32Int32)
    }
}
