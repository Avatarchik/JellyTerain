using System.Runtime.Serialization;

namespace System.Collections.Specialized
{
	/// <summary>Represents a collection of key/value pairs that are accessible by the key or index.</summary>
	[Serializable]
	public class OrderedDictionary : ICollection, IOrderedDictionary, IDictionary, IDeserializationCallback, IEnumerable, ISerializable
	{
		private class OrderedEntryCollectionEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private IEnumerator listEnumerator;

			public object Current => listEnumerator.Current;

			public DictionaryEntry Entry => (DictionaryEntry)listEnumerator.Current;

			public object Key => Entry.Key;

			public object Value => Entry.Value;

			public OrderedEntryCollectionEnumerator(IEnumerator listEnumerator)
			{
				this.listEnumerator = listEnumerator;
			}

			public bool MoveNext()
			{
				return listEnumerator.MoveNext();
			}

			public void Reset()
			{
				listEnumerator.Reset();
			}
		}

		private class OrderedCollection : ICollection, IEnumerable
		{
			private class OrderedCollectionEnumerator : IEnumerator
			{
				private bool isKeyList;

				private IEnumerator listEnumerator;

				public object Current
				{
					get
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)listEnumerator.Current;
						return (!isKeyList) ? dictionaryEntry.Value : dictionaryEntry.Key;
					}
				}

				public OrderedCollectionEnumerator(IEnumerator listEnumerator, bool isKeyList)
				{
					this.listEnumerator = listEnumerator;
					this.isKeyList = isKeyList;
				}

				public bool MoveNext()
				{
					return listEnumerator.MoveNext();
				}

				public void Reset()
				{
					listEnumerator.Reset();
				}
			}

			private ArrayList list;

			private bool isKeyList;

			public int Count => list.Count;

			public bool IsSynchronized => false;

			public object SyncRoot => list.SyncRoot;

			public OrderedCollection(ArrayList list, bool isKeyList)
			{
				this.list = list;
				this.isKeyList = isKeyList;
			}

			public void CopyTo(Array array, int index)
			{
				for (int i = 0; i < list.Count; i++)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)list[i];
					if (isKeyList)
					{
						array.SetValue(dictionaryEntry.Key, index + i);
					}
					else
					{
						array.SetValue(dictionaryEntry.Value, index + i);
					}
				}
			}

			public IEnumerator GetEnumerator()
			{
				return new OrderedCollectionEnumerator(list.GetEnumerator(), isKeyList);
			}
		}

		private ArrayList list;

		private Hashtable hash;

		private bool readOnly;

		private int initialCapacity;

		private SerializationInfo serializationInfo;

		private IEqualityComparer comparer;

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object is synchronized (thread-safe).</summary>
		/// <returns>This method always returns false.</returns>
		bool ICollection.IsSynchronized => list.IsSynchronized;

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object.</returns>
		object ICollection.SyncRoot => list.SyncRoot;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> has a fixed size; otherwise, false. The default is false.</returns>
		bool IDictionary.IsFixedSize => false;

		/// <summary>Gets the number of key/values pairs contained in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public int Count => list.Count;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only; otherwise, false. The default is false.</returns>
		public bool IsReadOnly => readOnly;

		/// <summary>Gets or sets the value with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new element using the specified key.</returns>
		/// <param name="key">The key of the value to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public object this[object key]
		{
			get
			{
				return hash[key];
			}
			set
			{
				WriteCheck();
				if (hash.Contains(key))
				{
					int index = FindListEntry(key);
					list[index] = new DictionaryEntry(key, value);
				}
				else
				{
					list.Add(new DictionaryEntry(key, value));
				}
				hash[key] = value;
			}
		}

		/// <summary>Gets or sets the value at the specified index.</summary>
		/// <returns>The value of the item at the specified index. </returns>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.OrderedDictionary.Count" />.</exception>
		public object this[int index]
		{
			get
			{
				return ((DictionaryEntry)list[index]).Value;
			}
			set
			{
				WriteCheck();
				DictionaryEntry dictionaryEntry = (DictionaryEntry)list[index];
				dictionaryEntry.Value = value;
				list[index] = dictionaryEntry;
				hash[dictionaryEntry.Key] = value;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> object containing the keys in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the keys in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public ICollection Keys => new OrderedCollection(list, isKeyList: true);

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public ICollection Values => new OrderedCollection(list, isKeyList: false);

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class.</summary>
		public OrderedDictionary()
		{
			list = new ArrayList();
			hash = new Hashtable();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified initial capacity.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection can contain.</param>
		public OrderedDictionary(int capacity)
		{
			initialCapacity = ((capacity >= 0) ? capacity : 0);
			list = new ArrayList(initialCapacity);
			hash = new Hashtable(initialCapacity);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified comparer.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />.</param>
		public OrderedDictionary(IEqualityComparer equalityComparer)
		{
			list = new ArrayList();
			hash = new Hashtable(equalityComparer);
			comparer = equalityComparer;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified initial capacity and comparer.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection can contain.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />.</param>
		public OrderedDictionary(int capacity, IEqualityComparer equalityComparer)
		{
			initialCapacity = ((capacity >= 0) ? capacity : 0);
			list = new ArrayList(initialCapacity);
			hash = new Hashtable(initialCapacity, equalityComparer);
			comparer = equalityComparer;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class that is serializable using the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.OrderedDictionary" />.</param>
		protected OrderedDictionary(SerializationInfo info, StreamingContext context)
		{
			serializationInfo = info;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			if (serializationInfo != null)
			{
				comparer = (IEqualityComparer)serializationInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
				readOnly = serializationInfo.GetBoolean("ReadOnly");
				initialCapacity = serializationInfo.GetInt32("InitialCapacity");
				if (list == null)
				{
					list = new ArrayList();
				}
				else
				{
					list.Clear();
				}
				hash = new Hashtable(comparer);
				object[] array = (object[])serializationInfo.GetValue("ArrayList", typeof(object[]));
				object[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)array2[i];
					hash.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					list.Add(dictionaryEntry);
				}
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object that iterates through the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is invalid.</exception>
		protected virtual void OnDeserialization(object sender)
		{
			((IDeserializationCallback)this).OnDeserialization(sender);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.OrderedDictionary" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("KeyComparer", comparer, typeof(IEqualityComparer));
			info.AddValue("ReadOnly", readOnly);
			info.AddValue("InitialCapacity", initialCapacity);
			object[] array = new object[hash.Count];
			hash.CopyTo(array, 0);
			info.AddValue("ArrayList", array);
		}

		/// <summary>Copies the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> elements to a one-dimensional <see cref="T:System.Array" /> object at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> object that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public void CopyTo(Array array, int index)
		{
			list.CopyTo(array, index);
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection with the lowest available index.</summary>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="value">The value of the entry to add. This value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public void Add(object key, object value)
		{
			WriteCheck();
			hash.Add(key, value);
			list.Add(new DictionaryEntry(key, value));
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public void Clear()
		{
			WriteCheck();
			hash.Clear();
			list.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		public bool Contains(object key)
		{
			return hash.Contains(key);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object that iterates through the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new OrderedEntryCollectionEnumerator(list.GetEnumerator());
		}

		/// <summary>Removes the entry with the specified key from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="key">The key of the entry to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public void Remove(object key)
		{
			WriteCheck();
			if (hash.Contains(key))
			{
				hash.Remove(key);
				int index = FindListEntry(key);
				list.RemoveAt(index);
			}
		}

		private int FindListEntry(object key)
		{
			for (int i = 0; i < list.Count; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)list[i];
				if ((comparer == null) ? dictionaryEntry.Key.Equals(key) : comparer.Equals(dictionaryEntry.Key, key))
				{
					return i;
				}
			}
			return -1;
		}

		private void WriteCheck()
		{
			if (readOnly)
			{
				throw new NotSupportedException("Collection is read only");
			}
		}

		/// <summary>Returns a read-only copy of the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>A read-only copy of the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public OrderedDictionary AsReadOnly()
		{
			OrderedDictionary orderedDictionary = new OrderedDictionary();
			orderedDictionary.list = list;
			orderedDictionary.hash = hash;
			orderedDictionary.comparer = comparer;
			orderedDictionary.readOnly = true;
			return orderedDictionary;
		}

		/// <summary>Inserts a new entry into the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection with the specified key and value at the specified index.</summary>
		/// <param name="index">The zero-based index at which the element should be inserted.</param>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="value">The value of the entry to add. The value can be null.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is out of range.</exception>
		/// <exception cref="T:System.NotSupportedException">This collection is read-only.</exception>
		public void Insert(int index, object key, object value)
		{
			WriteCheck();
			hash.Add(key, value);
			list.Insert(index, new DictionaryEntry(key, value));
		}

		/// <summary>Removes the entry at the specified index from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="index">The zero-based index of the entry to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.- or -<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.OrderedDictionary.Count" />.</exception>
		public void RemoveAt(int index)
		{
			WriteCheck();
			DictionaryEntry dictionaryEntry = (DictionaryEntry)list[index];
			list.RemoveAt(index);
			hash.Remove(dictionaryEntry.Key);
		}
	}
}
