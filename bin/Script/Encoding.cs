// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Text
{
    public class Encoding : Object
    {
      // Fields:
  codePage : Int32
  windows_code_page : Int32
  is_readonly : Boolean
  decoder_fallback : DecoderFallback
  encoder_fallback : EncoderFallback
  i18nAssembly : Assembly
  i18nDisabled : Boolean
  encoding_infos : EncodingInfo[]
  encodings : Object[]
  body_name : String
  encoding_name : String
  header_name : String
  is_mail_news_display : Boolean
  is_mail_news_save : Boolean
  is_browser_save : Boolean
  is_browser_display : Boolean
  web_name : String
  asciiEncoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  bigEndianEncoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  defaultEncoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  utf7Encoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  utf8EncodingWithMarkers : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  utf8EncodingWithoutMarkers : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  unicodeEncoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  isoLatin1Encoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  utf8EncodingUnsafe : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  utf32Encoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  bigEndianUTF32Encoding : Encoding modreq(System.Runtime.CompilerServices.IsVolatile)
  lockobj : Object
      // Properties:
  IsReadOnly : Boolean
  IsSingleByte : Boolean
  DecoderFallback : DecoderFallback
  EncoderFallback : EncoderFallback
  BodyName : String
  CodePage : Int32
  EncodingName : String
  HeaderName : String
  IsBrowserDisplay : Boolean
  IsBrowserSave : Boolean
  IsMailNewsDisplay : Boolean
  IsMailNewsSave : Boolean
  WebName : String
  WindowsCodePage : Int32
  ASCII : Encoding
  BigEndianUnicode : Encoding
  Default : Encoding
  ISOLatin1 : Encoding
  UTF7 : Encoding
  UTF8 : Encoding
  UTF8Unmarked : Encoding
  UTF8UnmarkedUnsafe : Encoding
  Unicode : Encoding
  UTF32 : Encoding
  BigEndianUTF32 : Encoding
      // Events:
      // Methods:
      VoidText.Encoding::.ctor()
      VoidText.Encoding::.ctorInt32)
      VoidText.Encoding::.cctor()
      StringText.Encoding::_String)
      public BooleanText.Encoding::get_IsReadOnly()
      public BooleanText.Encoding::get_IsSingleByte()
      public Text.DecoderFallbackText.Encoding::get_DecoderFallback()
      public VoidText.Encoding::set_DecoderFallbackText.DecoderFallback)
      public Text.EncoderFallbackText.Encoding::get_EncoderFallback()
      public VoidText.Encoding::set_EncoderFallbackText.EncoderFallback)
      VoidText.Encoding::SetFallbackInternalText.EncoderFallbackText.DecoderFallback)
      public Byte[]Text.Encoding::ConvertText.EncodingText.EncodingByte[])
      public Byte[]Text.Encoding::ConvertText.EncodingText.EncodingByte[]Int32Int32)
      public BooleanText.Encoding::EqualsObject)
      public Int32Text.Encoding::GetByteCountChar[]Int32Int32)
      public Int32Text.Encoding::GetByteCountString)
      public Int32Text.Encoding::GetByteCountChar[])
      public Int32Text.Encoding::GetBytesChar[]Int32Int32Byte[]Int32)
      public Int32Text.Encoding::GetBytesStringInt32Int32Byte[]Int32)
      public Byte[]Text.Encoding::GetBytesString)
      public Byte[]Text.Encoding::GetBytesChar[]Int32Int32)
      public Byte[]Text.Encoding::GetBytesChar[])
      public Int32Text.Encoding::GetCharCountByte[]Int32Int32)
      public Int32Text.Encoding::GetCharCountByte[])
      public Int32Text.Encoding::GetCharsByte[]Int32Int32Char[]Int32)
      public Char[]Text.Encoding::GetCharsByte[]Int32Int32)
      public Char[]Text.Encoding::GetCharsByte[])
      public Text.DecoderText.Encoding::GetDecoder()
      public Text.EncoderText.Encoding::GetEncoder()
      ObjectText.Encoding::InvokeI18NStringObject[])
      public Text.EncodingText.Encoding::GetEncodingInt32)
      public ObjectText.Encoding::Clone()
      public Text.EncodingText.Encoding::GetEncodingInt32Text.EncoderFallbackText.DecoderFallback)
      public Text.EncodingText.Encoding::GetEncodingStringText.EncoderFallbackText.DecoderFallback)
      public Text.EncodingInfo[]Text.Encoding::GetEncodings()
      public BooleanText.Encoding::IsAlwaysNormalized()
      public BooleanText.Encoding::IsAlwaysNormalizedText.NormalizationForm)
      public Text.EncodingText.Encoding::GetEncodingString)
      public Int32Text.Encoding::GetHashCode()
      public Int32Text.Encoding::GetMaxByteCountInt32)
      public Int32Text.Encoding::GetMaxCharCountInt32)
      public Byte[]Text.Encoding::GetPreamble()
      public StringText.Encoding::GetStringByte[]Int32Int32)
      public StringText.Encoding::GetStringByte[])
      public StringText.Encoding::get_BodyName()
      public Int32Text.Encoding::get_CodePage()
      public StringText.Encoding::get_EncodingName()
      public StringText.Encoding::get_HeaderName()
      public BooleanText.Encoding::get_IsBrowserDisplay()
      public BooleanText.Encoding::get_IsBrowserSave()
      public BooleanText.Encoding::get_IsMailNewsDisplay()
      public BooleanText.Encoding::get_IsMailNewsSave()
      public StringText.Encoding::get_WebName()
      public Int32Text.Encoding::get_WindowsCodePage()
      public Text.EncodingText.Encoding::get_ASCII()
      public Text.EncodingText.Encoding::get_BigEndianUnicode()
      StringText.Encoding::InternalCodePageInt32&)
      public Text.EncodingText.Encoding::get_Default()
      Text.EncodingText.Encoding::get_ISOLatin1()
      public Text.EncodingText.Encoding::get_UTF7()
      public Text.EncodingText.Encoding::get_UTF8()
      Text.EncodingText.Encoding::get_UTF8Unmarked()
      Text.EncodingText.Encoding::get_UTF8UnmarkedUnsafe()
      public Text.EncodingText.Encoding::get_Unicode()
      public Text.EncodingText.Encoding::get_UTF32()
      Text.EncodingText.Encoding::get_BigEndianUTF32()
      public Int32Text.Encoding::GetByteCountChar*Int32)
      public Int32Text.Encoding::GetCharCountByte*Int32)
      public Int32Text.Encoding::GetCharsByte*Int32Char*Int32)
      public Int32Text.Encoding::GetBytesChar*Int32Byte*Int32)
    }
}
