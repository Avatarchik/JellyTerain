// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Serialization.Formatters.Binary
{
    class ObjectWriter : Object
    {
      // Fields:
  _idGenerator : ObjectIDGenerator
  _cachedMetadata : Hashtable
  _pendingObjects : Queue
  _assemblyCache : Hashtable
  _cachedTypes : Hashtable
  CorlibAssembly : Assembly
  CorlibAssemblyName : String
  _surrogateSelector : ISurrogateSelector
  _context : StreamingContext
  _assemblyFormat : FormatterAssemblyStyle
  _typeFormat : FormatterTypeStyle
  arrayBuffer : Byte[]
  ArrayBufferLength : Int32
  _manager : SerializationObjectManager
      // Properties:
      // Events:
      // Methods:
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::.ctorRuntime.Serialization.ISurrogateSelectorRuntime.Serialization.StreamingContextRuntime.Serialization.Formatters.FormatterAssemblyStyleRuntime.Serialization.Formatters.FormatterTypeStyle)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::.cctor()
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteObjectGraphIO.BinaryWriterObjectRuntime.Remoting.Messaging.Header[])
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::QueueObjectObject)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteQueuedObjectsIO.BinaryWriter)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteObjectInstanceIO.BinaryWriterObjectBoolean)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteSerializationEndIO.BinaryWriter)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteObjectIO.BinaryWriterInt64Object)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::GetObjectDataObjectRuntime.Serialization.Formatters.Binary.TypeMetadata&Object&)
      Runtime.Serialization.Formatters.Binary.TypeMetadataRuntime.Serialization.Formatters.Binary.ObjectWriter::CreateMemberTypeMetadataType)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteArrayIO.BinaryWriterInt64Array)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteGenericArrayIO.BinaryWriterInt64Array)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteObjectArrayIO.BinaryWriterInt64Array)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteStringArrayIO.BinaryWriterInt64Array)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WritePrimitiveTypeArrayIO.BinaryWriterInt64Array)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::BlockWriteIO.BinaryWriterArrayInt32)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteSingleDimensionArrayElementsIO.BinaryWriterArrayType)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteNullFillerIO.BinaryWriterInt32)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteObjectReferenceIO.BinaryWriterInt64)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteValueIO.BinaryWriterTypeObject)
      VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteStringIO.BinaryWriterInt64String)
      public Int32Runtime.Serialization.Formatters.Binary.ObjectWriter::WriteAssemblyIO.BinaryWriterReflection.Assembly)
      public Int32Runtime.Serialization.Formatters.Binary.ObjectWriter::WriteAssemblyNameIO.BinaryWriterString)
      public Int32Runtime.Serialization.Formatters.Binary.ObjectWriter::GetAssemblyIdReflection.Assembly)
      public Int32Runtime.Serialization.Formatters.Binary.ObjectWriter::GetAssemblyNameIdString)
      Int32Runtime.Serialization.Formatters.Binary.ObjectWriter::RegisterAssemblyStringBoolean&)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WritePrimitiveValueIO.BinaryWriterObject)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteTypeCodeIO.BinaryWriterType)
      public Runtime.Serialization.Formatters.Binary.TypeTagRuntime.Serialization.Formatters.Binary.ObjectWriter::GetTypeTagType)
      public VoidRuntime.Serialization.Formatters.Binary.ObjectWriter::WriteTypeSpecIO.BinaryWriterType)
    }
}
