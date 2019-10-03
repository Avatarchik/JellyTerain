// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Globalization.Unicode
{
    class SortKeyBuffer : Object
    {
      // Fields:
  l1 : Int32
  l2 : Int32
  l3 : Int32
  l4s : Int32
  l4t : Int32
  l4k : Int32
  l4w : Int32
  l5 : Int32
  l1b : Byte[]
  l2b : Byte[]
  l3b : Byte[]
  l4sb : Byte[]
  l4tb : Byte[]
  l4kb : Byte[]
  l4wb : Byte[]
  l5b : Byte[]
  source : String
  processLevel2 : Boolean
  frenchSort : Boolean
  frenchSorted : Boolean
  lcid : Int32
  options : CompareOptions
      // Properties:
      // Events:
      // Methods:
      public Void Mono.Globalization.Unicode.SortKeyBuffer::.ctorInt32)
      public Void Mono.Globalization.Unicode.SortKeyBuffer::Reset()
      Void Mono.Globalization.Unicode.SortKeyBuffer::ClearBuffer()
      Void Mono.Globalization.Unicode.SortKeyBuffer::InitializeGlobalization.CompareOptionsInt32StringBoolean)
      Void Mono.Globalization.Unicode.SortKeyBuffer::AppendCJKExtensionByteByte)
      Void Mono.Globalization.Unicode.SortKeyBuffer::AppendKanaByteByteByteByteBooleanByteBooleanBoolean)
      Void Mono.Globalization.Unicode.SortKeyBuffer::AppendNormalByteByteByteByte)
      Void Mono.Globalization.Unicode.SortKeyBuffer::AppendLevel5ByteByte)
      Void Mono.Globalization.Unicode.SortKeyBuffer::AppendBufferPrimitiveByteByte[]&Int32&)
      public Globalization.SortKey Mono.Globalization.Unicode.SortKeyBuffer::GetResultAndReset()
      Int32 Mono.Globalization.Unicode.SortKeyBuffer::GetOptimizedLengthByte[]Int32Byte)
      public Globalization.SortKey Mono.Globalization.Unicode.SortKeyBuffer::GetResult()
    }
}
