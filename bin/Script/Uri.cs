// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security
{
    class Uri : Object
    {
      // Fields:
  isUnixFilePath : Boolean
  source : String
  scheme : String
  host : String
  port : Int32
  path : String
  query : String
  fragment : String
  userinfo : String
  isUnc : Boolean
  isOpaquePart : Boolean
  segments : String[]
  userEscaped : Boolean
  cachedAbsoluteUri : String
  cachedToString : String
  cachedLocalPath : String
  cachedHashCode : Int32
  reduce : Boolean
  hexUpperChars : String
  public SchemeDelimiter : String
  public UriSchemeFile : String
  public UriSchemeFtp : String
  public UriSchemeGopher : String
  public UriSchemeHttp : String
  public UriSchemeHttps : String
  public UriSchemeMailto : String
  public UriSchemeNews : String
  public UriSchemeNntp : String
  schemes : UriScheme[]
  <>f__switch$map17 : Dictionary`2
      // Properties:
  AbsolutePath : String
  AbsoluteUri : String
  Authority : String
  Fragment : String
  Host : String
  IsDefaultPort : Boolean
  IsFile : Boolean
  IsLoopback : Boolean
  IsUnc : Boolean
  LocalPath : String
  PathAndQuery : String
  Port : Int32
  Query : String
  Scheme : String
  Segments : String[]
  UserEscaped : Boolean
  UserInfo : String
      // Events:
      // Methods:
      public Void Mono.Security.Uri::.ctorString)
      public Void Mono.Security.Uri::.ctorStringBoolean)
      public Void Mono.Security.Uri::.ctorStringBooleanBoolean)
      public Void Mono.Security.Uri::.ctor(Mono.Security.UriString)
      public Void Mono.Security.Uri::.ctor(Mono.Security.UriStringBoolean)
      Void Mono.Security.Uri::.cctor()
      public String Mono.Security.Uri::get_AbsolutePath()
      public String Mono.Security.Uri::get_AbsoluteUri()
      public String Mono.Security.Uri::get_Authority()
      public String Mono.Security.Uri::get_Fragment()
      public String Mono.Security.Uri::get_Host()
      public Boolean Mono.Security.Uri::get_IsDefaultPort()
      public Boolean Mono.Security.Uri::get_IsFile()
      public Boolean Mono.Security.Uri::get_IsLoopback()
      public Boolean Mono.Security.Uri::get_IsUnc()
      public String Mono.Security.Uri::get_LocalPath()
      public String Mono.Security.Uri::get_PathAndQuery()
      public Int32 Mono.Security.Uri::get_Port()
      public String Mono.Security.Uri::get_Query()
      public String Mono.Security.Uri::get_Scheme()
      public String[] Mono.Security.Uri::get_Segments()
      public Boolean Mono.Security.Uri::get_UserEscaped()
      public String Mono.Security.Uri::get_UserInfo()
      Boolean Mono.Security.Uri::IsIPv4AddressString)
      Boolean Mono.Security.Uri::IsDomainAddressString)
      public Boolean Mono.Security.Uri::CheckSchemeNameString)
      public Boolean Mono.Security.Uri::EqualsObject)
      public Int32 Mono.Security.Uri::GetHashCode()
      public String Mono.Security.Uri::GetLeftPart(Mono.Security.UriPartial)
      public Int32 Mono.Security.Uri::FromHexChar)
      public String Mono.Security.Uri::HexEscapeChar)
      public Char Mono.Security.Uri::HexUnescapeStringInt32&)
      public Boolean Mono.Security.Uri::IsHexDigitChar)
      public Boolean Mono.Security.Uri::IsHexEncodingStringInt32)
      public String Mono.Security.Uri::MakeRelative(Mono.Security.Uri)
      public String Mono.Security.Uri::ToString()
      Void Mono.Security.Uri::Escape()
      String Mono.Security.Uri::EscapeStringString)
      String Mono.Security.Uri::EscapeStringStringBooleanBooleanBoolean)
      Void Mono.Security.Uri::Parse()
      String Mono.Security.Uri::UnescapeString)
      String Mono.Security.Uri::UnescapeStringBoolean)
      Void Mono.Security.Uri::ParseAsWindowsUNCString)
      Void Mono.Security.Uri::ParseAsWindowsAbsoluteFilePathString)
      Void Mono.Security.Uri::ParseAsUnixAbsoluteFilePathString)
      Void Mono.Security.Uri::ParseString)
      String Mono.Security.Uri::ReduceString)
      String Mono.Security.Uri::GetSchemeDelimiterString)
      Int32 Mono.Security.Uri::GetDefaultPortString)
      String Mono.Security.Uri::GetOpaqueWiseSchemeDelimiter()
      Boolean Mono.Security.Uri::IsBadFileSystemCharacterChar)
      Boolean Mono.Security.Uri::IsExcludedCharacterChar)
      Boolean Mono.Security.Uri::IsPredefinedSchemeString)
      Boolean Mono.Security.Uri::IsReservedCharacterChar)
    }
}
