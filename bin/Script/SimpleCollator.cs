// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Globalization.Unicode
{
    class SimpleCollator : Object
    {
      // Fields:
  UnsafeFlagLength : Int32
  QuickCheckDisabled : Boolean
  invariant : SimpleCollator
  textInfo : TextInfo
  frenchSort : Boolean
  cjkCatTable : Byte*
  cjkLv1Table : Byte*
  cjkIndexer : CodePointIndexer
  cjkLv2Table : Byte*
  cjkLv2Indexer : CodePointIndexer
  lcid : Int32
  contractions : Contraction[]
  level2Maps : Level2Map[]
  unsafeFlags : Byte[]
      // Properties:
      // Events:
      // Methods:
      public Void Mono.Globalization.Unicode.SimpleCollator::.ctorGlobalization.CultureInfo)
      Void Mono.Globalization.Unicode.SimpleCollator::.cctor()
      Void Mono.Globalization.Unicode.SimpleCollator::SetCJKTableGlobalization.CultureInfo,Mono.Globalization.Unicode.CodePointIndexer&Byte*&Byte*&,Mono.Globalization.Unicode.CodePointIndexer&Byte*&)
      Globalization.CultureInfo Mono.Globalization.Unicode.SimpleCollator::GetNeutralCultureGlobalization.CultureInfo)
      Byte Mono.Globalization.Unicode.SimpleCollator::CategoryInt32)
      Byte Mono.Globalization.Unicode.SimpleCollator::Level1Int32)
      Byte Mono.Globalization.Unicode.SimpleCollator::Level2Int32,Mono.Globalization.Unicode.SimpleCollator/ExtenderType)
      Boolean Mono.Globalization.Unicode.SimpleCollator::IsHalfKanaInt32Globalization.CompareOptions)
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetContractionStringInt32Int32)
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetContractionStringInt32Int32,Mono.Globalization.Unicode.Contraction[])
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetTailContractionStringInt32Int32)
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetTailContractionStringInt32Int32,Mono.Globalization.Unicode.Contraction[])
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetContractionChar)
      Mono.Globalization.Unicode.Contraction Mono.Globalization.Unicode.SimpleCollator::GetContractionChar,Mono.Globalization.Unicode.Contraction[])
      Int32 Mono.Globalization.Unicode.SimpleCollator::FilterOptionsInt32Globalization.CompareOptions)
      Mono.Globalization.Unicode.SimpleCollator/ExtenderType Mono.Globalization.Unicode.SimpleCollator::GetExtenderTypeInt32)
      Byte Mono.Globalization.Unicode.SimpleCollator::ToDashTypeValue(Mono.Globalization.Unicode.SimpleCollator/ExtenderTypeGlobalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::FilterExtenderInt32,Mono.Globalization.Unicode.SimpleCollator/ExtenderTypeGlobalization.CompareOptions)
      Boolean Mono.Globalization.Unicode.SimpleCollator::IsIgnorableInt32Globalization.CompareOptions)
      Boolean Mono.Globalization.Unicode.SimpleCollator::IsSafeInt32)
      public Globalization.SortKey Mono.Globalization.Unicode.SimpleCollator::GetSortKeyString)
      public Globalization.SortKey Mono.Globalization.Unicode.SimpleCollator::GetSortKeyStringGlobalization.CompareOptions)
      public Globalization.SortKey Mono.Globalization.Unicode.SimpleCollator::GetSortKeyStringInt32Int32Globalization.CompareOptions)
      Void Mono.Globalization.Unicode.SimpleCollator::GetSortKeyStringInt32Int32,Mono.Globalization.Unicode.SortKeyBufferGlobalization.CompareOptions)
      Void Mono.Globalization.Unicode.SimpleCollator::FillSortKeyRawInt32,Mono.Globalization.Unicode.SimpleCollator/ExtenderType,Mono.Globalization.Unicode.SortKeyBufferGlobalization.CompareOptions)
      Void Mono.Globalization.Unicode.SimpleCollator::FillSurrogateSortKeyRawInt32,Mono.Globalization.Unicode.SortKeyBuffer)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::CompareStringString)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::CompareStringStringGlobalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::CompareOrdinalStringInt32Int32StringInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::CompareQuickStringInt32Int32StringInt32Int32Boolean&Boolean&Boolean)
      Int32 Mono.Globalization.Unicode.SimpleCollator::CompareOrdinalIgnoreCaseStringInt32Int32StringInt32Int32)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::CompareStringInt32Int32StringInt32Int32Globalization.CompareOptions)
      Void Mono.Globalization.Unicode.SimpleCollator::ClearBufferByte*Int32)
      Boolean Mono.Globalization.Unicode.SimpleCollator::QuickCheckPossibleStringInt32Int32StringInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::CompareInternalStringInt32Int32StringInt32Int32Boolean&Boolean&BooleanBoolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Int32 Mono.Globalization.Unicode.SimpleCollator::CompareFlagPairBooleanBoolean)
      public Boolean Mono.Globalization.Unicode.SimpleCollator::IsPrefixStringStringGlobalization.CompareOptions)
      public Boolean Mono.Globalization.Unicode.SimpleCollator::IsPrefixStringStringInt32Int32Globalization.CompareOptions)
      Boolean Mono.Globalization.Unicode.SimpleCollator::IsPrefixStringStringInt32Int32Boolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      public Boolean Mono.Globalization.Unicode.SimpleCollator::IsSuffixStringStringGlobalization.CompareOptions)
      public Boolean Mono.Globalization.Unicode.SimpleCollator::IsSuffixStringStringInt32Int32Globalization.CompareOptions)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfStringStringGlobalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::QuickIndexOfStringStringInt32Int32Boolean&)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfStringStringInt32Int32Globalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfOrdinalStringStringInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfOrdinalIgnoreCaseStringStringInt32Int32)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfStringCharGlobalization.CompareOptions)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfStringCharInt32Int32Globalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfOrdinalStringCharInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfOrdinalIgnoreCaseStringCharInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfSortKeyStringInt32Int32Byte*CharInt32Boolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Int32 Mono.Globalization.Unicode.SimpleCollator::IndexOfStringStringInt32Int32Byte*,Mono.Globalization.Unicode.SimpleCollator/Context&)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfStringStringGlobalization.CompareOptions)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfStringStringInt32Int32Globalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfOrdinalStringStringInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfOrdinalIgnoreCaseStringStringInt32Int32)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfStringCharGlobalization.CompareOptions)
      public Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfStringCharInt32Int32Globalization.CompareOptions)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfOrdinalStringCharInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfOrdinalIgnoreCaseStringCharInt32Int32)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfSortKeyStringInt32Int32Int32Byte*Int32Boolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Int32 Mono.Globalization.Unicode.SimpleCollator::LastIndexOfStringStringInt32Int32Byte*,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Boolean Mono.Globalization.Unicode.SimpleCollator::MatchesForwardStringInt32&Int32Int32Byte*Boolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Boolean Mono.Globalization.Unicode.SimpleCollator::MatchesForwardCoreStringInt32&Int32Int32Byte*Boolean,Mono.Globalization.Unicode.SimpleCollator/ExtenderType,Mono.Globalization.Unicode.Contraction&,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Boolean Mono.Globalization.Unicode.SimpleCollator::MatchesPrimitiveGlobalization.CompareOptionsByte*Int32,Mono.Globalization.Unicode.SimpleCollator/ExtenderTypeByte*Int32Boolean)
      Boolean Mono.Globalization.Unicode.SimpleCollator::MatchesBackwardStringInt32&Int32Int32Int32Byte*Boolean,Mono.Globalization.Unicode.SimpleCollator/Context&)
      Boolean Mono.Globalization.Unicode.SimpleCollator::MatchesBackwardCoreStringInt32&Int32Int32Int32Byte*Boolean,Mono.Globalization.Unicode.SimpleCollator/ExtenderType,Mono.Globalization.Unicode.Contraction&,Mono.Globalization.Unicode.SimpleCollator/Context&)
    }
}
