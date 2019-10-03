// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Globalization.Unicode
{
    class MSCompatUnicodeTable : Object
    {
      // Fields:
  ResourceVersionSize : Int32
  public MaxExpansionLength : Int32
  ignorableFlags : Byte*
  categories : Byte*
  level1 : Byte*
  level2 : Byte*
  level3 : Byte*
  cjkCHScategory : Byte*
  cjkCHTcategory : Byte*
  cjkJAcategory : Byte*
  cjkKOcategory : Byte*
  cjkCHSlv1 : Byte*
  cjkCHTlv1 : Byte*
  cjkJAlv1 : Byte*
  cjkKOlv1 : Byte*
  cjkKOlv2 : Byte*
  tailoringArr : Char[]
  tailoringInfos : TailoringInfo[]
  forLock : Object
  public isReady : Boolean
  <>f__switch$map2 : Dictionary`2
  <>f__switch$map3 : Dictionary`2
  <>f__switch$map4 : Dictionary`2
      // Properties:
  IsReady : Boolean
      // Events:
      // Methods:
      public Void Mono.Globalization.Unicode.MSCompatUnicodeTable::.ctor()
      Void Mono.Globalization.Unicode.MSCompatUnicodeTable::.cctor()
      public Mono.Globalization.Unicode.TailoringInfo Mono.Globalization.Unicode.MSCompatUnicodeTable::GetTailoringInfoInt32)
      public Void Mono.Globalization.Unicode.MSCompatUnicodeTable::BuildTailoringTablesGlobalization.CultureInfo,Mono.Globalization.Unicode.TailoringInfo,Mono.Globalization.Unicode.Contraction[]&,Mono.Globalization.Unicode.Level2Map[]&)
      Void Mono.Globalization.Unicode.MSCompatUnicodeTable::SetCJKReferencesString,Mono.Globalization.Unicode.CodePointIndexer&Byte*&Byte*&,Mono.Globalization.Unicode.CodePointIndexer&Byte*&)
      public Byte Mono.Globalization.Unicode.MSCompatUnicodeTable::CategoryInt32)
      public Byte Mono.Globalization.Unicode.MSCompatUnicodeTable::Level1Int32)
      public Byte Mono.Globalization.Unicode.MSCompatUnicodeTable::Level2Int32)
      public Byte Mono.Globalization.Unicode.MSCompatUnicodeTable::Level3Int32)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsSortableString)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsSortableInt32)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsIgnorableInt32)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsIgnorableInt32Byte)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsIgnorableSymbolInt32)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsIgnorableNonSpacingInt32)
      public Int32 Mono.Globalization.Unicode.MSCompatUnicodeTable::ToKanaTypeInsensitiveInt32)
      public Int32 Mono.Globalization.Unicode.MSCompatUnicodeTable::ToWidthCompatInt32)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::HasSpecialWeightChar)
      public Byte Mono.Globalization.Unicode.MSCompatUnicodeTable::GetJapaneseDashTypeChar)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsHalfWidthKanaChar)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsHiraganaChar)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::IsJapaneseSmallLetterChar)
      public Boolean Mono.Globalization.Unicode.MSCompatUnicodeTable::get_IsReady()
      IntPtr Mono.Globalization.Unicode.MSCompatUnicodeTable::GetResourceString)
      UInt32 Mono.Globalization.Unicode.MSCompatUnicodeTable::UInt32FromBytePtrByte*UInt32)
      public Void Mono.Globalization.Unicode.MSCompatUnicodeTable::FillCJKString,Mono.Globalization.Unicode.CodePointIndexer&Byte*&Byte*&,Mono.Globalization.Unicode.CodePointIndexer&Byte*&)
      Void Mono.Globalization.Unicode.MSCompatUnicodeTable::FillCJKCoreString,Mono.Globalization.Unicode.CodePointIndexer&Byte*&Byte*&,Mono.Globalization.Unicode.CodePointIndexer&Byte*&)
    }
}
