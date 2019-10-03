// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class StreamWriter : TextWriter
    {
      // Fields:
  DefaultBufferSize : Int32
  DefaultFileBufferSize : Int32
  MinimumBufferSize : Int32
  internalEncoding : Encoding
  internalStream : Stream
  iflush : Boolean
  byte_buf : Byte[]
  byte_pos : Int32
  decode_buf : Char[]
  decode_pos : Int32
  DisposedAlready : Boolean
  preamble_done : Boolean
  public Null : StreamWriter
      // Properties:
  AutoFlush : Boolean
  BaseStream : Stream
  Encoding : Encoding
      // Events:
      // Methods:
      public VoidIO.StreamWriter::.ctorIO.Stream)
      public VoidIO.StreamWriter::.ctorIO.StreamText.Encoding)
      public VoidIO.StreamWriter::.ctorIO.StreamText.EncodingInt32)
      public VoidIO.StreamWriter::.ctorString)
      public VoidIO.StreamWriter::.ctorStringBoolean)
      public VoidIO.StreamWriter::.ctorStringBooleanText.Encoding)
      public VoidIO.StreamWriter::.ctorStringBooleanText.EncodingInt32)
      VoidIO.StreamWriter::.cctor()
      VoidIO.StreamWriter::InitializeText.EncodingInt32)
      public BooleanIO.StreamWriter::get_AutoFlush()
      public VoidIO.StreamWriter::set_AutoFlushBoolean)
      public IO.StreamIO.StreamWriter::get_BaseStream()
      public Text.EncodingIO.StreamWriter::get_Encoding()
      VoidIO.StreamWriter::DisposeBoolean)
      public VoidIO.StreamWriter::Flush()
      VoidIO.StreamWriter::FlushBytes()
      VoidIO.StreamWriter::Decode()
      public VoidIO.StreamWriter::WriteChar[]Int32Int32)
      VoidIO.StreamWriter::LowLevelWriteChar[]Int32Int32)
      VoidIO.StreamWriter::LowLevelWriteString)
      public VoidIO.StreamWriter::WriteChar)
      public VoidIO.StreamWriter::WriteChar[])
      public VoidIO.StreamWriter::WriteString)
      public VoidIO.StreamWriter::Close()
      VoidIO.StreamWriter::Finalize()
    }
}
