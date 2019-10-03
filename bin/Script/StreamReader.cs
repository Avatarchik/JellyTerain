// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class StreamReader : TextReader
    {
      // Fields:
  DefaultBufferSize : Int32
  DefaultFileBufferSize : Int32
  MinimumBufferSize : Int32
  input_buffer : Byte[]
  decoded_buffer : Char[]
  decoded_count : Int32
  pos : Int32
  buffer_size : Int32
  do_checks : Int32
  encoding : Encoding
  decoder : Decoder
  base_stream : Stream
  mayBlock : Boolean
  line_builder : StringBuilder
  public Null : StreamReader
  foundCR : Boolean
      // Properties:
  BaseStream : Stream
  CurrentEncoding : Encoding
  EndOfStream : Boolean
      // Events:
      // Methods:
      VoidIO.StreamReader::.ctor()
      public VoidIO.StreamReader::.ctorIO.Stream)
      public VoidIO.StreamReader::.ctorIO.StreamBoolean)
      public VoidIO.StreamReader::.ctorIO.StreamText.Encoding)
      public VoidIO.StreamReader::.ctorIO.StreamText.EncodingBoolean)
      public VoidIO.StreamReader::.ctorIO.StreamText.EncodingBooleanInt32)
      public VoidIO.StreamReader::.ctorString)
      public VoidIO.StreamReader::.ctorStringBoolean)
      public VoidIO.StreamReader::.ctorStringText.Encoding)
      public VoidIO.StreamReader::.ctorStringText.EncodingBoolean)
      public VoidIO.StreamReader::.ctorStringText.EncodingBooleanInt32)
      VoidIO.StreamReader::.cctor()
      VoidIO.StreamReader::InitializeIO.StreamText.EncodingBooleanInt32)
      public IO.StreamIO.StreamReader::get_BaseStream()
      public Text.EncodingIO.StreamReader::get_CurrentEncoding()
      public BooleanIO.StreamReader::get_EndOfStream()
      public VoidIO.StreamReader::Close()
      VoidIO.StreamReader::DisposeBoolean)
      Int32IO.StreamReader::DoChecksInt32)
      public VoidIO.StreamReader::DiscardBufferedData()
      Int32IO.StreamReader::ReadBuffer()
      public Int32IO.StreamReader::Peek()
      BooleanIO.StreamReader::DataAvailable()
      public Int32IO.StreamReader::Read()
      public Int32IO.StreamReader::ReadChar[]Int32Int32)
      Int32IO.StreamReader::FindNextEOL()
      public StringIO.StreamReader::ReadLine()
      public StringIO.StreamReader::ReadToEnd()
    }
}
