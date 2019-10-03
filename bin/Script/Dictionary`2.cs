// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Collections.Generic
{
    public class Dictionary`2 : Object
    {
      // Fields:
  INITIAL_SIZE : Int32
  DEFAULT_LOAD_FACTOR : Single
  NO_SLOT : Int32
  HASH_FLAG : Int32
  table : Int32[]
  linkSlots : Link[]
  keySlots : TKey[]
  valueSlots : TValue[]
  touchedSlots : Int32
  emptySlot : Int32
  count : Int32
  threshold : Int32
  hcp : IEqualityComparer`1
  serialization_info : SerializationInfo
  generation : Int32
  <>f__am$cacheB : Transform`1
      // Properties:
  System.Collections.Generic.IDictionary<TKey,TValue>.Keys : ICollection`1
  System.Collections.Generic.IDictionary<TKey,TValue>.Values : ICollection`1
  System.Collections.IDictionary.Keys : ICollection
  System.Collections.IDictionary.Values : ICollection
  System.Collections.IDictionary.IsFixedSize : Boolean
  System.Collections.IDictionary.IsReadOnly : Boolean
  System.Collections.IDictionary.Item : Object
  System.Collections.ICollection.IsSynchronized : Boolean
  System.Collections.ICollection.SyncRoot : Object
  System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly : Boolean
  Count : Int32
  Item : TValue
  Comparer : IEqualityComparer`1
  Keys : KeyCollection
  Values : ValueCollection
      // Events:
      // Methods:
      public VoidCollections.Generic.Dictionary`2::.ctor()
      public VoidCollections.Generic.Dictionary`2::.ctorCollections.Generic.IEqualityComparer`1<TKey>)
      public VoidCollections.Generic.Dictionary`2::.ctorCollections.Generic.IDictionary`2<TKey,TValue>)
      public VoidCollections.Generic.Dictionary`2::.ctorInt32)
      public VoidCollections.Generic.Dictionary`2::.ctorCollections.Generic.IDictionary`2<TKey,TValue>Collections.Generic.IEqualityComparer`1<TKey>)
      public VoidCollections.Generic.Dictionary`2::.ctorInt32Collections.Generic.IEqualityComparer`1<TKey>)
      VoidCollections.Generic.Dictionary`2::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      Collections.Generic.ICollection`1<TKey>Collections.Generic.Dictionary`2::System.Collections.Generic.IDictionary<TKey,TValue>.get_Keys()
      Collections.Generic.ICollection`1<TValue>Collections.Generic.Dictionary`2::System.Collections.Generic.IDictionary<TKey,TValue>.get_Values()
      Collections.ICollectionCollections.Generic.Dictionary`2::System.Collections.IDictionary.get_Keys()
      Collections.ICollectionCollections.Generic.Dictionary`2::System.Collections.IDictionary.get_Values()
      BooleanCollections.Generic.Dictionary`2::System.Collections.IDictionary.get_IsFixedSize()
      BooleanCollections.Generic.Dictionary`2::System.Collections.IDictionary.get_IsReadOnly()
      ObjectCollections.Generic.Dictionary`2::System.Collections.IDictionary.get_ItemObject)
      VoidCollections.Generic.Dictionary`2::System.Collections.IDictionary.set_ItemObjectObject)
      VoidCollections.Generic.Dictionary`2::System.Collections.IDictionary.AddObjectObject)
      BooleanCollections.Generic.Dictionary`2::System.Collections.IDictionary.ContainsObject)
      VoidCollections.Generic.Dictionary`2::System.Collections.IDictionary.RemoveObject)
      BooleanCollections.Generic.Dictionary`2::System.Collections.ICollection.get_IsSynchronized()
      ObjectCollections.Generic.Dictionary`2::System.Collections.ICollection.get_SyncRoot()
      BooleanCollections.Generic.Dictionary`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.get_IsReadOnly()
      VoidCollections.Generic.Dictionary`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.AddCollections.Generic.KeyValuePair`2<TKey,TValue>)
      BooleanCollections.Generic.Dictionary`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.ContainsCollections.Generic.KeyValuePair`2<TKey,TValue>)
      VoidCollections.Generic.Dictionary`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.CopyToCollections.Generic.KeyValuePair`2<TKey,TValue>[]Int32)
      BooleanCollections.Generic.Dictionary`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.RemoveCollections.Generic.KeyValuePair`2<TKey,TValue>)
      VoidCollections.Generic.Dictionary`2::System.Collections.ICollection.CopyToArrayInt32)
      Collections.IEnumeratorCollections.Generic.Dictionary`2::System.Collections.IEnumerable.GetEnumerator()
      Collections.Generic.IEnumerator`1<System.Collections.Generic.KeyValuePair`2<TKey,TValue>>Collections.Generic.Dictionary`2::System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey,TValue>>.GetEnumerator()
      Collections.IDictionaryEnumeratorCollections.Generic.Dictionary`2::System.Collections.IDictionary.GetEnumerator()
      public Int32Collections.Generic.Dictionary`2::get_Count()
      public TValueCollections.Generic.Dictionary`2::get_Item(TKey)
      public VoidCollections.Generic.Dictionary`2::set_Item(TKey,TValue)
      VoidCollections.Generic.Dictionary`2::InitInt32Collections.Generic.IEqualityComparer`1<TKey>)
      VoidCollections.Generic.Dictionary`2::InitArraysInt32)
      VoidCollections.Generic.Dictionary`2::CopyToCheckArrayInt32)
      VoidCollections.Generic.Dictionary`2::Do_CopyTo(TElem[]Int32Collections.Generic.Dictionary`2/Transform`1<TKey,TValue,TRet>)
      Collections.Generic.KeyValuePair`2<TKey,TValue>Collections.Generic.Dictionary`2::make_pair(TKey,TValue)
      TKeyCollections.Generic.Dictionary`2::pick_key(TKey,TValue)
      TValueCollections.Generic.Dictionary`2::pick_value(TKey,TValue)
      VoidCollections.Generic.Dictionary`2::CopyToCollections.Generic.KeyValuePair`2<TKey,TValue>[]Int32)
      VoidCollections.Generic.Dictionary`2::Do_ICollectionCopyToArrayInt32Collections.Generic.Dictionary`2/Transform`1<TKey,TValue,TRet>)
      VoidCollections.Generic.Dictionary`2::Resize()
      public VoidCollections.Generic.Dictionary`2::Add(TKey,TValue)
      public Collections.Generic.IEqualityComparer`1<TKey>Collections.Generic.Dictionary`2::get_Comparer()
      public VoidCollections.Generic.Dictionary`2::Clear()
      public BooleanCollections.Generic.Dictionary`2::ContainsKey(TKey)
      public BooleanCollections.Generic.Dictionary`2::ContainsValue(TValue)
      public VoidCollections.Generic.Dictionary`2::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidCollections.Generic.Dictionary`2::OnDeserializationObject)
      public BooleanCollections.Generic.Dictionary`2::Remove(TKey)
      public BooleanCollections.Generic.Dictionary`2::TryGetValue(TKey,TValue&)
      public Collections.Generic.Dictionary`2/KeyCollection<TKey,TValue>Collections.Generic.Dictionary`2::get_Keys()
      public Collections.Generic.Dictionary`2/ValueCollection<TKey,TValue>Collections.Generic.Dictionary`2::get_Values()
      TKeyCollections.Generic.Dictionary`2::ToTKeyObject)
      TValueCollections.Generic.Dictionary`2::ToTValueObject)
      BooleanCollections.Generic.Dictionary`2::ContainsKeyValuePairCollections.Generic.KeyValuePair`2<TKey,TValue>)
      public Collections.Generic.Dictionary`2/Enumerator<TKey,TValue>Collections.Generic.Dictionary`2::GetEnumerator()
      Collections.DictionaryEntryCollections.Generic.Dictionary`2::<CopyTo>m__0(TKey,TValue)
    }
}
