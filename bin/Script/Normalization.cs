// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Globalization.Unicode
{
    class Normalization : Object
    {
      // Fields:
  public NoNfd : Int32
  public NoNfkd : Int32
  public NoNfc : Int32
  public MaybeNfc : Int32
  public NoNfkc : Int32
  public MaybeNfkc : Int32
  public FullCompositionExclusion : Int32
  public IsUnsafe : Int32
  HangulSBase : Int32
  HangulLBase : Int32
  HangulVBase : Int32
  HangulTBase : Int32
  HangulLCount : Int32
  HangulVCount : Int32
  HangulTCount : Int32
  HangulNCount : Int32
  HangulSCount : Int32
  props : Byte*
  mappedChars : Int32*
  charMapIndex : Int16*
  helperIndex : Int16*
  mapIdxToComposite : UInt16*
  combiningClass : Byte*
  forLock : Object
  public isReady : Boolean
      // Properties:
  IsReady : Boolean
      // Events:
      // Methods:
      public Void Mono.Globalization.Unicode.Normalization::.ctor()
      Void Mono.Globalization.Unicode.Normalization::.cctor()
      UInt32 Mono.Globalization.Unicode.Normalization::PropValueInt32)
      Int32 Mono.Globalization.Unicode.Normalization::CharMapIdxInt32)
      Int32 Mono.Globalization.Unicode.Normalization::GetNormalizedStringLengthInt32)
      Byte Mono.Globalization.Unicode.Normalization::GetCombiningClassInt32)
      Int32 Mono.Globalization.Unicode.Normalization::GetPrimaryCompositeFromMapIndexInt32)
      Int32 Mono.Globalization.Unicode.Normalization::GetPrimaryCompositeHelperIndexInt32)
      Int32 Mono.Globalization.Unicode.Normalization::GetPrimaryCompositeCharIndexObjectInt32)
      String Mono.Globalization.Unicode.Normalization::ComposeStringInt32)
      Text.StringBuilder Mono.Globalization.Unicode.Normalization::CombineStringInt32Int32)
      Boolean Mono.Globalization.Unicode.Normalization::CanBePrimaryCompositeInt32)
      Void Mono.Globalization.Unicode.Normalization::CombineText.StringBuilderInt32Int32)
      Int32 Mono.Globalization.Unicode.Normalization::GetPrimaryCompositeMapIndexObjectInt32Int32)
      String Mono.Globalization.Unicode.Normalization::DecomposeStringInt32)
      Void Mono.Globalization.Unicode.Normalization::DecomposeStringText.StringBuilder&Int32)
      Void Mono.Globalization.Unicode.Normalization::ReorderCanonicalStringText.StringBuilder&Int32)
      Void Mono.Globalization.Unicode.Normalization::DecomposeCharText.StringBuilder&Int32[]&StringInt32Int32&)
      public Mono.Globalization.Unicode.NormalizationCheck Mono.Globalization.Unicode.Normalization::QuickCheckCharInt32)
      Boolean Mono.Globalization.Unicode.Normalization::GetCanonicalHangulInt32Int32[]Int32)
      public Void Mono.Globalization.Unicode.Normalization::GetCanonicalInt32Int32[]Int32)
      public Boolean Mono.Globalization.Unicode.Normalization::IsNormalizedStringInt32)
      public String Mono.Globalization.Unicode.Normalization::NormalizeStringInt32)
      public Boolean Mono.Globalization.Unicode.Normalization::get_IsReady()
      Void Mono.Globalization.Unicode.Normalization::load_normalization_resourceIntPtr&IntPtr&IntPtr&IntPtr&IntPtr&IntPtr&)
    }
}
