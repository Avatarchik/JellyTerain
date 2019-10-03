// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Xml
{
    class SmallXmlParser : Object
    {
      // Fields:
  handler : IContentHandler
  reader : TextReader
  elementNames : Stack
  xmlSpaces : Stack
  xmlSpace : String
  buffer : StringBuilder
  nameBuffer : Char[]
  isWhitespace : Boolean
  attributes : AttrListImpl
  line : Int32
  column : Int32
  resetColumn : Boolean
  <>f__switch$map18 : Dictionary`2
      // Properties:
      // Events:
      // Methods:
      public Void Mono.Xml.SmallXmlParser::.ctor()
      Exception Mono.Xml.SmallXmlParser::ErrorString)
      Exception Mono.Xml.SmallXmlParser::UnexpectedEndError()
      Boolean Mono.Xml.SmallXmlParser::IsNameCharCharBoolean)
      Boolean Mono.Xml.SmallXmlParser::IsWhitespaceInt32)
      public Void Mono.Xml.SmallXmlParser::SkipWhitespaces()
      Void Mono.Xml.SmallXmlParser::HandleWhitespaces()
      public Void Mono.Xml.SmallXmlParser::SkipWhitespacesBoolean)
      Int32 Mono.Xml.SmallXmlParser::Peek()
      Int32 Mono.Xml.SmallXmlParser::Read()
      public Void Mono.Xml.SmallXmlParser::ExpectInt32)
      String Mono.Xml.SmallXmlParser::ReadUntilCharBoolean)
      public String Mono.Xml.SmallXmlParser::ReadName()
      public Void Mono.Xml.SmallXmlParser::ParseIO.TextReader,Mono.Xml.SmallXmlParser/IContentHandler)
      Void Mono.Xml.SmallXmlParser::Cleanup()
      public Void Mono.Xml.SmallXmlParser::ReadContent()
      Void Mono.Xml.SmallXmlParser::HandleBufferedContent()
      Void Mono.Xml.SmallXmlParser::ReadCharacters()
      Void Mono.Xml.SmallXmlParser::ReadReference()
      Int32 Mono.Xml.SmallXmlParser::ReadCharacterReference()
      Void Mono.Xml.SmallXmlParser::ReadAttribute(Mono.Xml.SmallXmlParser/AttrListImpl)
      Void Mono.Xml.SmallXmlParser::ReadCDATASection()
      Void Mono.Xml.SmallXmlParser::ReadComment()
    }
}
