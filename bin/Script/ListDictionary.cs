namespace System.Collections.Specialized
{
	/// <summary>Implements IDictionary using a singly linked list. Recommended for collections that typically contain 10 items or less.</summary>
	[Serializable]
	public class ListDictionary : ICollection, IDictionary, IEnumerable
	{
		[Serializable]
		private class DictionaryNode
		{
			public object key;

			public object value;

			public DictionaryNode next;

			public DictionaryNode(object key, object value, DictionaryNode next)
			{
				this.key = key;
				this.value = value;
				this.next = next;
			}
		}

		private class DictionaryNodeEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private ListDictionary dict;

			private bool isAtStart;

			private DictionaryNode current;

			private int version;

			public object Current => Entry;

			private DictionaryNode DictionaryNode
			{
				get
				{
					FailFast();
					if (current == null)
					{
						throw new InvalidOperationException("Enumerator is positioned before the collection's first element or after the last element.");
					}
					return current;
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					object key = DictionaryNode.key;
					return new DictionaryEntry(key, current.value);
				}
			}

			public object Key => DictionaryNode.key;

			public object Value => DictionaryNode.value;

			public DictionaryNodeEnumerator(ListDictionary dict)
			{
				this.dict = dict;
				version = dict.version;
				Reset();
			}

			private void FailFast()
			{
				if (version != dict.version)
				{
					throw new InvalidOperationException("The ListDictionary's contents changed after this enumerator was instantiated.");
				}
			}

			public bool MoveNext()
			{
				FailFast();
				if (current == null && !isAtStart)
				{
					return false;
				}
				current = ((!isAtStart) ? current.next : dict.head);
				isAtStart = false;
				return current != null;
			}

			public void Reset()
			{
				FailFast();
				isAtStart = true;
				current = null;
			}
		}

		private class DictionaryNodeCollection : ICollection, IEnumerable
		{
			private class DictionaryNodeCollectionEnumerator : IEnumerator
			{
				private IDictionaryEnumerator inner;

				private bool isKeyList;

				public object Current => (!isKeyList) ? inner.Value : inner.Key;

				public DictionaryNodeCollectionEnumerator(IDictionaryEnumerator inner, bool isKeyList)
				{
					this.inner = inner;
					this.isKeyList = isKeyList;
				}

				public bool MoveNext()
				{
					return inner.MoveNext();
				}

				public void Reset()
				{
					inner.Reset();
				}
			}

			private ListDictionary dict;

			private bool isKeyList;

			public int Count => dict.Count;

			public bool IsSynchronized => false;

			public object SyncRoot => dict.SyncRoot;

			public DictionaryNodeCollection(ListDictionary dict, bool isKeyList)
			{
				this.dict = dict;
				this.isKeyList = isKeyList;
			}

			public void CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array", "Array cannot be null.");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", "index is less than 0");
				}
				if (index > array.Length)
				{
					throw new IndexOutOfRangeException("index is too large");
				}
				if (Count > array.Length - index)
				{
					throw new ArgumentException("Not enough room in the array");
				}
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						array.SetValue(current, index++);
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}

			public IEnumerator GetEnumerator()
			{
				return new DictionaryNodeCollectionEnumerator(dict.GetEnumerator(), isKeyList);
			}
		}

		private int count;

		private int version;

		private DictionaryNode head;

		private IComparer comparer;

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public int Count => count;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> is synchronized (thread safe).</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsSynchronized => false;

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public object SyncRoot => this;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> has a fixed size.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsFixedSize => false;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> is read-only.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsReadOnly => false;

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new entry using the specified key.</returns>
		/// <param name="key">The key whose value to get or set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public object this[object key]
		{
			get
			{
				return FindEntry(key)?.value;
			}
			set
			{
				DictionaryNode prev;
				DictionaryNode dictionaryNode = FindEntry(key, out prev);
				if (dictionaryNode != null)
				{
					dictionaryNode.value = value;
				}
				else
				{
					AddImpl(key, value, prev);
				}
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public ICollection Keys => new DictionaryNodeCollection(this, isKeyList: true);

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public ICollection Values => new DictionaryNodeCollection(this, isKeyList: false);

		/// <summary>Creates an empty <see cref="T:System.Collections.Specialized.ListDictionary" /> using the default comparer.</summary>
		public ListDictionary()
		{
			count = 0;
			version = 0;
			comparer = null;
			head = null;
		}

		/// <summary>Creates an empty <see cref="T:System.Collections.Specialized.ListDictionary" /> using the specified comparer.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		public ListDictionary(IComparer comparer)
			: this()
		{
			this.comparer = comparer;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryNodeEnumerator(this);
		}

		private DictionaryNode FindEntry(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Attempted lookup for a null key.");
			}
			DictionaryNode next = head;
			if (comparer == null)
			{
				while (next != null && !key.Equals(next.key))
				{
					next = next.next;
				}
			}
			else
			{
				while (next != null && comparer.Compare(key, next.key) != 0)
				{
					next = next.next;
				}
			}
			return next;
		}

		private DictionaryNode FindEntry(object key, out DictionaryNode prev)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Attempted lookup for a null key.");
			}
			DictionaryNode next = head;
			prev = null;
			if (comparer == null)
			{
				while (next != null && !key.Equals(next.key))
				{
					prev = next;
					next = next.next;
				}
			}
			else
			{
				while (next != null && comparer.Compare(key, next.key) != 0)
				{
					prev = next;
					next = next.next;
				}
			}
			return next;
		}

		private void AddImpl(object key, object value, DictionaryNode prev)
		{
			if (prev == null)
			{
				head = new DictionaryNode(key, value, head);
			}
			else
			{
				prev.next = new DictionaryNode(key, value, prev.next);
			}
			count++;
			version++;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Specialized.ListDictionary" /> entries to a one-dimensional <see cref="T:System.Array" /> instance at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Specialized.ListDictionary" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.ListDictionary" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.ListDictionary" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Array cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "index is less than 0");
			}
			if (index > array.Length)
			{
				throw new IndexOutOfRangeException("index is too large");
			}
			if (Count > array.Length - index)
			{
				throw new ArgumentException("Not enough room in the array");
			}
			IDictionaryEnumerator dictionaryEnumerator = GetEnumerator();
			try
			{
				while (dictionaryEnumerator.MoveNext())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)dictionaryEnumerator.Current;
					array.SetValue(dictionaryEntry, index++);
				}
			}
			finally
			{
				IDisposable disposable = dictionaryEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <param name="key">The key of the entry to add. </param>
		/// <param name="value">The value of the entry to add. The value can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An entry with the same key already exists in the <see cref="T:System.Collections.Specialized.ListDictionary" />. </exception>
		public void Add(object key, object value)
		{
			DictionaryNode prev;
			DictionaryNode dictionaryNode = FindEntry(key, out prev);
			if (dictionaryNode != null)
			{
				throw new ArgumentException("key", "Duplicate key in add.");
			}
			AddImpl(key, value, prev);
		}

		/// <summary>Removes all entries from the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		public void Clear()
		{
			head = null;
			count = 0;
			version++;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.ListDictionary" /> contains an entry with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Specialized.ListDictionary" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public bool Contains(object key)
		{
			return FindEntry(key) != null;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryNodeEnumerator(this);
		}

		/// <summary>Removes the entry with the specified key from the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <param name="key">The key of the entry to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public void Remove(object key)
		{
			DictionaryNode prev;
			DictionaryNode dictionaryNode = FindEntry(key, out prev);
			if (dictionaryNode != null)
			{
				if (prev == null)
				{
					head = dictionaryNode.next;
				}
				else
				{
					prev.next = dictionaryNode.next;
				}
				dictionaryNode.value = null;
				count--;
				version++;
			}
		}
	}
}
