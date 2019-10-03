// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Resources
{
    public class ResourceReader : Object
    {
      // Fields:
  reader : BinaryReader
  readerLock : Object
  formatter : IFormatter
  resourceCount : Int32
  typeCount : Int32
  typeNames : String[]
  hashes : Int32[]
  infos : ResourceInfo[]
  dataSectionOffset : Int32
  nameSectionOffset : Int64
  resource_ver : Int32
  cache : ResourceCacheItem[]
  cache_lock : Object
      // Properties:
      // Events:
      // Methods:
      public VoidResources.ResourceReader::.ctorIO.Stream)
      public VoidResources.ResourceReader::.ctorString)
      Collections.IEnumeratorResources.ResourceReader::System.Collections.IEnumerable.GetEnumerator()
      VoidResources.ResourceReader::System.IDisposable.Dispose()
      VoidResources.ResourceReader::ReadHeaders()
      VoidResources.ResourceReader::CreateResourceInfoInt64Resources.ResourceReader/ResourceInfo&)
      Int32Resources.ResourceReader::Read7BitEncodedInt()
      ObjectResources.ResourceReader::ReadValueVer2Int32)
      ObjectResources.ResourceReader::ReadValueVer1Type)
      ObjectResources.ResourceReader::ReadNonPredefinedValueType)
      VoidResources.ResourceReader::LoadResourceValuesResources.ResourceReader/ResourceCacheItem[])
      IO.UnmanagedMemoryStreamResources.ResourceReader::ResourceValueAsStreamStringInt32)
      public VoidResources.ResourceReader::Close()
      public Collections.IDictionaryEnumeratorResources.ResourceReader::GetEnumerator()
      public VoidResources.ResourceReader::GetResourceDataStringString&Byte[]&)
      VoidResources.ResourceReader::GetResourceDataAtInt32String&Byte[]&)
      VoidResources.ResourceReader::DisposeBoolean)
    }
}
