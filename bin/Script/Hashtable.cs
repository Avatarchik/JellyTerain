// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Collections
{
    public class Hashtable : Object
    {
      // Fields:
  CHAIN_MARKER : Int32
  inUse : Int32
  modificationCount : Int32
  loadFactor : Single
  table : Slot[]
  hashes : Int32[]
  threshold : Int32
  hashKeys : HashKeys
  hashValues : HashValues
  hcpRef : IHashCodeProvider
  comparerRef : IComparer
  serializationInfo : SerializationInfo
  equalityComparer : IEqualityComparer
  primeTbl : Int32[]
      // Properties:
  comparer : IComparer
  hcp : IHashCodeProvider
  EqualityComparer : IEqualityComparer
  Count : Int32
  IsSynchronized : Boolean
  SyncRoot : Object
  IsFixedSize : Boolean
  IsReadOnly : Boolean
  Keys : ICollection
  Values : ICollection
  Item : Object
      // Events:
      // Methods:
      public VoidCollections.Hashtable::.ctor()
      public VoidCollections.Hashtable::.ctorInt32SingleCollections.IHashCodeProviderCollections.IComparer)
      public VoidCollections.Hashtable::.ctorInt32Single)
      public VoidCollections.Hashtable::.ctorInt32)
      VoidCollections.Hashtable::.ctorCollections.Hashtable)
      public VoidCollections.Hashtable::.ctorInt32Collections.IHashCodeProviderCollections.IComparer)
      public VoidCollections.Hashtable::.ctorCollections.IDictionarySingleCollections.IHashCodeProviderCollections.IComparer)
      public VoidCollections.Hashtable::.ctorCollections.IDictionarySingle)
      public VoidCollections.Hashtable::.ctorCollections.IDictionary)
      public VoidCollections.Hashtable::.ctorCollections.IDictionaryCollections.IHashCodeProviderCollections.IComparer)
      public VoidCollections.Hashtable::.ctorCollections.IHashCodeProviderCollections.IComparer)
      public VoidCollections.Hashtable::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidCollections.Hashtable::.ctorCollections.IDictionaryCollections.IEqualityComparer)
      public VoidCollections.Hashtable::.ctorCollections.IDictionarySingleCollections.IEqualityComparer)
      public VoidCollections.Hashtable::.ctorCollections.IEqualityComparer)
      public VoidCollections.Hashtable::.ctorInt32Collections.IEqualityComparer)
      public VoidCollections.Hashtable::.ctorInt32SingleCollections.IEqualityComparer)
      VoidCollections.Hashtable::.cctor()
      Collections.IEnumeratorCollections.Hashtable::System.Collections.IEnumerable.GetEnumerator()
      VoidCollections.Hashtable::set_comparerCollections.IComparer)
      Collections.IComparerCollections.Hashtable::get_comparer()
      VoidCollections.Hashtable::set_hcpCollections.IHashCodeProvider)
      Collections.IHashCodeProviderCollections.Hashtable::get_hcp()
      Collections.IEqualityComparerCollections.Hashtable::get_EqualityComparer()
      public Int32Collections.Hashtable::get_Count()
      public BooleanCollections.Hashtable::get_IsSynchronized()
      public ObjectCollections.Hashtable::get_SyncRoot()
      public BooleanCollections.Hashtable::get_IsFixedSize()
      public BooleanCollections.Hashtable::get_IsReadOnly()
      public Collections.ICollectionCollections.Hashtable::get_Keys()
      public Collections.ICollectionCollections.Hashtable::get_Values()
      public ObjectCollections.Hashtable::get_ItemObject)
      public VoidCollections.Hashtable::set_ItemObjectObject)
      public VoidCollections.Hashtable::CopyToArrayInt32)
      public VoidCollections.Hashtable::AddObjectObject)
      public VoidCollections.Hashtable::Clear()
      public BooleanCollections.Hashtable::ContainsObject)
      public Collections.IDictionaryEnumeratorCollections.Hashtable::GetEnumerator()
      public VoidCollections.Hashtable::RemoveObject)
      public BooleanCollections.Hashtable::ContainsKeyObject)
      public BooleanCollections.Hashtable::ContainsValueObject)
      public ObjectCollections.Hashtable::Clone()
      public VoidCollections.Hashtable::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidCollections.Hashtable::OnDeserializationObject)
      public Collections.HashtableCollections.Hashtable::SynchronizedCollections.Hashtable)
      Int32Collections.Hashtable::GetHashObject)
      BooleanCollections.Hashtable::KeyEqualsObjectObject)
      VoidCollections.Hashtable::AdjustThreshold()
      VoidCollections.Hashtable::SetTableCollections.Hashtable/Slot[]Int32[])
      Int32Collections.Hashtable::FindObject)
      VoidCollections.Hashtable::Rehash()
      VoidCollections.Hashtable::PutImplObjectObjectBoolean)
      VoidCollections.Hashtable::CopyToArrayArrayInt32Collections.Hashtable/EnumeratorMode)
      BooleanCollections.Hashtable::TestPrimeInt32)
      Int32Collections.Hashtable::CalcPrimeInt32)
      Int32Collections.Hashtable::ToPrimeInt32)
    }
}
