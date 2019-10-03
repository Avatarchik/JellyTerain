// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class HashAlgorithm : Object
    {
      // Fields:
  HashValue : Byte[]
  HashSizeValue : Int32
  State : Int32
  disposed : Boolean
      // Properties:
  CanTransformMultipleBlocks : Boolean
  CanReuseTransform : Boolean
  Hash : Byte[]
  HashSize : Int32
  InputBlockSize : Int32
  OutputBlockSize : Int32
      // Events:
      // Methods:
      VoidSecurity.Cryptography.HashAlgorithm::.ctor()
      VoidSecurity.Cryptography.HashAlgorithm::System.IDisposable.Dispose()
      public BooleanSecurity.Cryptography.HashAlgorithm::get_CanTransformMultipleBlocks()
      public BooleanSecurity.Cryptography.HashAlgorithm::get_CanReuseTransform()
      public VoidSecurity.Cryptography.HashAlgorithm::Clear()
      public Byte[]Security.Cryptography.HashAlgorithm::ComputeHashByte[])
      public Byte[]Security.Cryptography.HashAlgorithm::ComputeHashByte[]Int32Int32)
      public Byte[]Security.Cryptography.HashAlgorithm::ComputeHashIO.Stream)
      public Security.Cryptography.HashAlgorithmSecurity.Cryptography.HashAlgorithm::Create()
      public Security.Cryptography.HashAlgorithmSecurity.Cryptography.HashAlgorithm::CreateString)
      public Byte[]Security.Cryptography.HashAlgorithm::get_Hash()
      VoidSecurity.Cryptography.HashAlgorithm::HashCoreByte[]Int32Int32)
      Byte[]Security.Cryptography.HashAlgorithm::HashFinal()
      public Int32Security.Cryptography.HashAlgorithm::get_HashSize()
      public VoidSecurity.Cryptography.HashAlgorithm::Initialize()
      VoidSecurity.Cryptography.HashAlgorithm::DisposeBoolean)
      public Int32Security.Cryptography.HashAlgorithm::get_InputBlockSize()
      public Int32Security.Cryptography.HashAlgorithm::get_OutputBlockSize()
      public Int32Security.Cryptography.HashAlgorithm::TransformBlockByte[]Int32Int32Byte[]Int32)
      public Byte[]Security.Cryptography.HashAlgorithm::TransformFinalBlockByte[]Int32Int32)
    }
}
