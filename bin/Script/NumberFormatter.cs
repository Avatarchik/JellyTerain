// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    class NumberFormatter : Object
    {
      // Fields:
  DefaultExpPrecision : Int32
  HundredMillion : Int32
  SeventeenDigitsThreshold : Int64
  ULongDivHundredMillion : UInt64
  ULongModHundredMillion : UInt64
  DoubleBitsExponentShift : Int32
  DoubleBitsExponentMask : Int32
  DoubleBitsMantissaMask : Int64
  DecimalBitsScaleMask : Int32
  SingleDefPrecision : Int32
  DoubleDefPrecision : Int32
  Int8DefPrecision : Int32
  UInt8DefPrecision : Int32
  Int16DefPrecision : Int32
  UInt16DefPrecision : Int32
  Int32DefPrecision : Int32
  UInt32DefPrecision : Int32
  Int64DefPrecision : Int32
  UInt64DefPrecision : Int32
  DecimalDefPrecision : Int32
  TenPowersListLength : Int32
  MinRoundtripVal : Double
  MaxRoundtripVal : Double
  MantissaBitsTable : UInt64*
  TensExponentTable : Int32*
  DigitLowerTable : Char*
  DigitUpperTable : Char*
  TenPowersList : Int64*
  DecHexDigits : Int32*
  _thread : Thread
  _nfi : NumberFormatInfo
  _NaN : Boolean
  _infinity : Boolean
  _isCustomFormat : Boolean
  _specifierIsUpper : Boolean
  _positive : Boolean
  _specifier : Char
  _precision : Int32
  _defPrecision : Int32
  _digitsLen : Int32
  _offset : Int32
  _decPointPos : Int32
  _val1 : UInt32
  _val2 : UInt32
  _val3 : UInt32
  _val4 : UInt32
  _cbuf : Char[]
  _ind : Int32
  threadNumberFormatter : NumberFormatter
      // Properties:
  CurrentCulture : CultureInfo
  IntegerDigits : Int32
  DecimalDigits : Int32
  IsFloatingSource : Boolean
  IsZero : Boolean
  IsZeroInteger : Boolean
      // Events:
      // Methods:
      public VoidNumberFormatter::.ctorThreading.Thread)
      VoidNumberFormatter::.cctor()
      VoidNumberFormatter::GetFormatterTablesUInt64*&Int32*&Char*&Char*&Int64*&Int32*&)
      Int64NumberFormatter::GetTenPowerOfInt32)
      VoidNumberFormatter::InitDecHexDigitsUInt32)
      VoidNumberFormatter::InitDecHexDigitsUInt64)
      VoidNumberFormatter::InitDecHexDigitsUInt32UInt64)
      UInt32NumberFormatter::FastToDecHexInt32)
      UInt32NumberFormatter::ToDecHexInt32)
      Int32NumberFormatter::FastDecHexLenInt32)
      Int32NumberFormatter::DecHexLenUInt32)
      Int32NumberFormatter::DecHexLen()
      Int32NumberFormatter::ScaleOrderInt64)
      Int32NumberFormatter::InitialFloatingPrecision()
      Int32NumberFormatter::ParsePrecisionString)
      VoidNumberFormatter::InitString)
      VoidNumberFormatter::InitHexUInt64)
      VoidNumberFormatter::InitStringInt32Int32)
      VoidNumberFormatter::InitStringUInt32Int32)
      VoidNumberFormatter::InitStringInt64)
      VoidNumberFormatter::InitStringUInt64)
      VoidNumberFormatter::InitStringDoubleInt32)
      VoidNumberFormatter::InitStringDecimal)
      VoidNumberFormatter::ResetCharBufInt32)
      VoidNumberFormatter::ResizeInt32)
      VoidNumberFormatter::AppendChar)
      VoidNumberFormatter::AppendCharInt32)
      VoidNumberFormatter::AppendString)
      Globalization.NumberFormatInfoNumberFormatter::GetNumberFormatInstanceIFormatProvider)
      public VoidNumberFormatter::set_CurrentCultureGlobalization.CultureInfo)
      Int32NumberFormatter::get_IntegerDigits()
      Int32NumberFormatter::get_DecimalDigits()
      BooleanNumberFormatter::get_IsFloatingSource()
      BooleanNumberFormatter::get_IsZero()
      BooleanNumberFormatter::get_IsZeroInteger()
      VoidNumberFormatter::RoundPosInt32)
      BooleanNumberFormatter::RoundDecimalInt32)
      BooleanNumberFormatter::RoundBitsInt32)
      VoidNumberFormatter::RemoveTrailingZeros()
      VoidNumberFormatter::AddOneToDecHex()
      UInt32NumberFormatter::AddOneToDecHexUInt32)
      Int32NumberFormatter::CountTrailingZeros()
      Int32NumberFormatter::CountTrailingZerosUInt32)
      NumberFormatterNumberFormatter::GetInstance()
      VoidNumberFormatter::Release()
      VoidNumberFormatter::SetThreadCurrentCultureGlobalization.CultureInfo)
      public StringNumberFormatter::NumberToStringStringSByteIFormatProvider)
      public StringNumberFormatter::NumberToStringStringByteIFormatProvider)
      public StringNumberFormatter::NumberToStringStringUInt16IFormatProvider)
      public StringNumberFormatter::NumberToStringStringInt16IFormatProvider)
      public StringNumberFormatter::NumberToStringStringUInt32IFormatProvider)
      public StringNumberFormatter::NumberToStringStringInt32IFormatProvider)
      public StringNumberFormatter::NumberToStringStringUInt64IFormatProvider)
      public StringNumberFormatter::NumberToStringStringInt64IFormatProvider)
      public StringNumberFormatter::NumberToStringStringSingleIFormatProvider)
      public StringNumberFormatter::NumberToStringStringDoubleIFormatProvider)
      public StringNumberFormatter::NumberToStringStringDecimalIFormatProvider)
      public StringNumberFormatter::NumberToStringUInt32IFormatProvider)
      public StringNumberFormatter::NumberToStringInt32IFormatProvider)
      public StringNumberFormatter::NumberToStringUInt64IFormatProvider)
      public StringNumberFormatter::NumberToStringInt64IFormatProvider)
      public StringNumberFormatter::NumberToStringSingleIFormatProvider)
      public StringNumberFormatter::NumberToStringDoubleIFormatProvider)
      StringNumberFormatter::FastIntegerToStringInt32IFormatProvider)
      StringNumberFormatter::IntegerToStringStringIFormatProvider)
      StringNumberFormatter::NumberToStringStringGlobalization.NumberFormatInfo)
      public StringNumberFormatter::FormatCurrencyInt32Globalization.NumberFormatInfo)
      StringNumberFormatter::FormatDecimalInt32Globalization.NumberFormatInfo)
      StringNumberFormatter::FormatHexadecimalInt32)
      public StringNumberFormatter::FormatFixedPointInt32Globalization.NumberFormatInfo)
      StringNumberFormatter::FormatRoundtripDoubleGlobalization.NumberFormatInfo)
      StringNumberFormatter::FormatRoundtripSingleGlobalization.NumberFormatInfo)
      StringNumberFormatter::FormatGeneralInt32Globalization.NumberFormatInfo)
      public StringNumberFormatter::FormatNumberInt32Globalization.NumberFormatInfo)
      public StringNumberFormatter::FormatPercentInt32Globalization.NumberFormatInfo)
      public StringNumberFormatter::FormatExponentialInt32Globalization.NumberFormatInfo)
      StringNumberFormatter::FormatExponentialInt32Globalization.NumberFormatInfoInt32)
      public StringNumberFormatter::FormatCustomStringGlobalization.NumberFormatInfo)
      VoidNumberFormatter::ZeroTrimEndText.StringBuilderBoolean)
      BooleanNumberFormatter::IsZeroOnlyText.StringBuilder)
      VoidNumberFormatter::AppendNonNegativeNumberText.StringBuilderInt32)
      VoidNumberFormatter::AppendIntegerStringInt32Text.StringBuilder)
      VoidNumberFormatter::AppendIntegerStringInt32)
      VoidNumberFormatter::AppendDecimalStringInt32Text.StringBuilder)
      VoidNumberFormatter::AppendDecimalStringInt32)
      VoidNumberFormatter::AppendIntegerStringWithGroupSeparatorInt32[]String)
      VoidNumberFormatter::AppendExponentGlobalization.NumberFormatInfoInt32Int32)
      VoidNumberFormatter::AppendOneDigitInt32)
      VoidNumberFormatter::FastAppendDigitsInt32Boolean)
      VoidNumberFormatter::AppendDigitsInt32Int32)
      VoidNumberFormatter::AppendDigitsInt32Int32Text.StringBuilder)
      VoidNumberFormatter::Multiply10Int32)
      VoidNumberFormatter::Divide10Int32)
      NumberFormatterNumberFormatter::GetClone()
    }
}
