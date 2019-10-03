// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Serialization
{
    class ObjectRecord : Object
    {
      // Fields:
  public Status : ObjectRecordStatus
  public OriginalObject : Object
  public ObjectInstance : Object
  public ObjectID : Int64
  public Info : SerializationInfo
  public IdOfContainingObj : Int64
  public Surrogate : ISerializationSurrogate
  public SurrogateSelector : ISurrogateSelector
  public Member : MemberInfo
  public ArrayIndex : Int32[]
  public FixupChainAsContainer : BaseFixupRecord
  public FixupChainAsRequired : BaseFixupRecord
  public Next : ObjectRecord
      // Properties:
  IsInstanceReady : Boolean
  IsUnsolvedObjectReference : Boolean
  IsRegistered : Boolean
  HasPendingFixups : Boolean
      // Events:
      // Methods:
      public VoidRuntime.Serialization.ObjectRecord::.ctor()
      public VoidRuntime.Serialization.ObjectRecord::SetMemberValueRuntime.Serialization.ObjectManagerReflection.MemberInfoObject)
      public VoidRuntime.Serialization.ObjectRecord::SetArrayValueRuntime.Serialization.ObjectManagerObjectInt32[])
      public VoidRuntime.Serialization.ObjectRecord::SetMemberValueRuntime.Serialization.ObjectManagerStringObject)
      public BooleanRuntime.Serialization.ObjectRecord::get_IsInstanceReady()
      public BooleanRuntime.Serialization.ObjectRecord::get_IsUnsolvedObjectReference()
      public BooleanRuntime.Serialization.ObjectRecord::get_IsRegistered()
      public BooleanRuntime.Serialization.ObjectRecord::DoFixupsBooleanRuntime.Serialization.ObjectManagerBoolean)
      public VoidRuntime.Serialization.ObjectRecord::RemoveFixupRuntime.Serialization.BaseFixupRecordBoolean)
      VoidRuntime.Serialization.ObjectRecord::UnchainFixupRuntime.Serialization.BaseFixupRecordRuntime.Serialization.BaseFixupRecordBoolean)
      public VoidRuntime.Serialization.ObjectRecord::ChainFixupRuntime.Serialization.BaseFixupRecordBoolean)
      public BooleanRuntime.Serialization.ObjectRecord::LoadDataRuntime.Serialization.ObjectManagerRuntime.Serialization.ISurrogateSelectorRuntime.Serialization.StreamingContext)
      public BooleanRuntime.Serialization.ObjectRecord::get_HasPendingFixups()
    }
}
