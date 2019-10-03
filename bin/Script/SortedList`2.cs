// Class info from System.dll
// 
using UnityEngine;

namespace System.Collections.Generic
{
    public class SortedList`2 : Object
    {
      // Fields:
  INITIAL_SIZE : Int32
  inUse : Int32
  modificationCount : Int32
  table : KeyValuePair`2[]
  comparer : IComparer`1
  defaultCapacity : Int32
      // Properties:
  System.Collections.ICollection.IsSynchronized : Boolean
  System.Collections.ICollection.SyncRoot : Object
  System.Collections.IDictionary.IsFixedSize : Boolean
  System.Collections.IDictionary.IsReadOnly : Boolean
  System.Collections.IDictionary.Item : Object
  System.Collections.IDictionary.Keys : ICollection
  System.Collections.IDictionary.Values : ICollection
  System.Collections.Generic.IDictionary<TKey,TValue>.Keys : ICollection`1
  System.Collections.Generic.IDictionary<TKey,TValue>.Values : ICollection`1
  System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.IsReadOnly : Boolean
  Count : Int32
  Item : TValue
  Capacity : Int32
  Keys : IList`1
  Values : IList`1
  Comparer : IComparer`1
      // Events:
      // Methods:
      public VoidCollections.Generic.SortedList`2::.ctor()
      public VoidCollections.Generic.SortedList`2::.ctorInt32)
      public VoidCollections.Generic.SortedList`2::.ctorInt32Collections.Generic.IComparer`1<TKey>)
      public VoidCollections.Generic.SortedList`2::.ctorCollections.Generic.IComparer`1<TKey>)
      public VoidCollections.Generic.SortedList`2::.ctorCollections.Generic.IDictionary`2<TKey,TValue>)
      public VoidCollections.Generic.SortedList`2::.ctorCollections.Generic.IDictionary`2<TKey,TValue>Collections.Generic.IComparer`1<TKey>)
      VoidCollections.Generic.SortedList`2::.cctor()
      BooleanCollections.Generic.SortedList`2::System.Collections.ICollection.get_IsSynchronized()
      ObjectCollections.Generic.SortedList`2::System.Collections.ICollection.get_SyncRoot()
      BooleanCollections.Generic.SortedList`2::System.Collections.IDictionary.get_IsFixedSize()
      BooleanCollections.Generic.SortedList`2::System.Collections.IDictionary.get_IsReadOnly()
      ObjectCollections.Generic.SortedList`2::System.Collections.IDictionary.get_ItemObject)
      VoidCollections.Generic.SortedList`2::System.Collections.IDictionary.set_ItemObjectObject)
      Collections.ICollectionCollections.Generic.SortedList`2::System.Collections.IDictionary.get_Keys()
      Collections.ICollectionCollections.Generic.SortedList`2::System.Collections.IDictionary.get_Values()
      Collections.Generic.ICollection`1<TKey>Collections.Generic.SortedList`2::System.Collections.Generic.IDictionary<TKey,TValue>.get_Keys()
      Collections.Generic.ICollection`1<TValue>Collections.Generic.SortedList`2::System.Collections.Generic.IDictionary<TKey,TValue>.get_Values()
      BooleanCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.get_IsReadOnly()
      VoidCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.Clear()
      VoidCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.CopyToCollections.Generic.KeyValuePair`2<TKey,TValue>[]Int32)
      VoidCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.AddCollections.Generic.KeyValuePair`2<TKey,TValue>)
      BooleanCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.ContainsCollections.Generic.KeyValuePair`2<TKey,TValue>)
      BooleanCollections.Generic.SortedList`2::System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey,TValue>>.RemoveCollections.Generic.KeyValuePair`2<TKey,TValue>)
      Collections.Generic.IEnumerator`1<System.Collections.Generic.KeyValuePair`2<TKey,TValue>>Collections.Generic.SortedList`2::System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey,TValue>>.GetEnumerator()
      Collections.IEnumeratorCollections.Generic.SortedList`2::System.Collections.IEnumerable.GetEnumerator()
      VoidCollections.Generic.SortedList`2::System.Collections.IDictionary.AddObjectObject)
      BooleanCollections.Generic.SortedList`2::System.Collections.IDictionary.ContainsObject)
      Collections.IDictionaryEnumeratorCollections.Generic.SortedList`2::System.Collections.IDictionary.GetEnumerator()
      VoidCollections.Generic.SortedList`2::System.Collections.IDictionary.RemoveObject)
      VoidCollections.Generic.SortedList`2::System.Collections.ICollection.CopyToArrayInt32)
      public Int32Collections.Generic.SortedList`2::get_Count()
      public TValueCollections.Generic.SortedList`2::get_Item(TKey)
      public VoidCollections.Generic.SortedList`2::set_Item(TKey,TValue)
      public Int32Collections.Generic.SortedList`2::get_Capacity()
      public VoidCollections.Generic.SortedList`2::set_CapacityInt32)
      public Collections.Generic.IList`1<TKey>Collections.Generic.SortedList`2::get_Keys()
      public Collections.Generic.IList`1<TValue>Collections.Generic.SortedList`2::get_Values()
      public Collections.Generic.IComparer`1<TKey>Collections.Generic.SortedList`2::get_Comparer()
      public VoidCollections.Generic.SortedList`2::Add(TKey,TValue)
      public BooleanCollections.Generic.SortedList`2::ContainsKey(TKey)
      public Collections.Generic.IEnumerator`1<System.Collections.Generic.KeyValuePair`2<TKey,TValue>>Collections.Generic.SortedList`2::GetEnumerator()
      public BooleanCollections.Generic.SortedList`2::Remove(TKey)
      public VoidCollections.Generic.SortedList`2::Clear()
      public VoidCollections.Generic.SortedList`2::RemoveAtInt32)
      public Int32Collections.Generic.SortedList`2::IndexOfKey(TKey)
      public Int32Collections.Generic.SortedList`2::IndexOfValue(TValue)
      public BooleanCollections.Generic.SortedList`2::ContainsValue(TValue)
      public VoidCollections.Generic.SortedList`2::TrimExcess()
      public BooleanCollections.Generic.SortedList`2::TryGetValue(TKey,TValue&)
      VoidCollections.Generic.SortedList`2::EnsureCapacityInt32Int32)
      VoidCollections.Generic.SortedList`2::PutImpl(TKey,TValueBoolean)
      VoidCollections.Generic.SortedList`2::InitCollections.Generic.IComparer`1<TKey>Int32Boolean)
      VoidCollections.Generic.SortedList`2::CopyToArrayArrayInt32Collections.Generic.SortedList`2/EnumeratorMode<TKey,TValue>)
      Int32Collections.Generic.SortedList`2::Find(TKey)
      TKeyCollections.Generic.SortedList`2::ToKeyObject)
      TValueCollections.Generic.SortedList`2::ToValueObject)
      TKeyCollections.Generic.SortedList`2::KeyAtInt32)
      TValueCollections.Generic.SortedList`2::ValueAtInt32)
    }
}
