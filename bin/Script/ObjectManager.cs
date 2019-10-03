// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Serialization
{
    public class ObjectManager : Object
    {
      // Fields:
  _objectRecordChain : ObjectRecord
  _lastObjectRecord : ObjectRecord
  _deserializedRecords : ArrayList
  _onDeserializedCallbackRecords : ArrayList
  _objectRecords : Hashtable
  _finalFixup : Boolean
  _selector : ISurrogateSelector
  _context : StreamingContext
  _registeredObjectsCount : Int32
      // Properties:
      // Events:
      // Methods:
      public VoidRuntime.Serialization.ObjectManager::.ctorRuntime.Serialization.ISurrogateSelectorRuntime.Serialization.StreamingContext)
      public VoidRuntime.Serialization.ObjectManager::DoFixups()
      Runtime.Serialization.ObjectRecordRuntime.Serialization.ObjectManager::GetObjectRecordInt64)
      public ObjectRuntime.Serialization.ObjectManager::GetObjectInt64)
      public VoidRuntime.Serialization.ObjectManager::RaiseDeserializationEvent()
      public VoidRuntime.Serialization.ObjectManager::RaiseOnDeserializingEventObject)
      VoidRuntime.Serialization.ObjectManager::RaiseOnDeserializedEventObject)
      VoidRuntime.Serialization.ObjectManager::AddFixupRuntime.Serialization.BaseFixupRecord)
      public VoidRuntime.Serialization.ObjectManager::RecordArrayElementFixupInt64Int32Int64)
      public VoidRuntime.Serialization.ObjectManager::RecordArrayElementFixupInt64Int32[]Int64)
      public VoidRuntime.Serialization.ObjectManager::RecordDelayedFixupInt64StringInt64)
      public VoidRuntime.Serialization.ObjectManager::RecordFixupInt64Reflection.MemberInfoInt64)
      VoidRuntime.Serialization.ObjectManager::RegisterObjectInternalObjectRuntime.Serialization.ObjectRecord)
      public VoidRuntime.Serialization.ObjectManager::RegisterObjectObjectInt64)
      public VoidRuntime.Serialization.ObjectManager::RegisterObjectObjectInt64Runtime.Serialization.SerializationInfo)
      public VoidRuntime.Serialization.ObjectManager::RegisterObjectObjectInt64Runtime.Serialization.SerializationInfoInt64Reflection.MemberInfo)
      public VoidRuntime.Serialization.ObjectManager::RegisterObjectObjectInt64Runtime.Serialization.SerializationInfoInt64Reflection.MemberInfoInt32[])
    }
}
