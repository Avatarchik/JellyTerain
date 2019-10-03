namespace System.Collections.Specialized
{
	/// <summary>Implements IDictionary by using a <see cref="T:System.Collections.Specialized.ListDictionary" /> while the collection is small, and then switching to a <see cref="T:System.Collections.Hashtable" /> when the collection gets large.</summary>
	[Serializable]
	public class HybridDictionary : ICollection, IDictionary, IEnumerable
	{
		private const int switchAfter = 10;

		private bool caseInsensitive;

		private Hashtable hashtable;

		private ListDictionary list;

		private IDictionary inner
		{
			get
			{
				object result;
				if (list == null)
				{
					IDictionary dictionary = hashtable;
					result = dictionary;
				}
				else
				{
					result = list;
				}
				return (IDictionary)result;
			}
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.Retrieving the value of this property is an O(1) operation.</returns>
		public int Count => inner.Count;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> has a fixed size.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsFixedSize => false;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> is read-only.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsReadOnly => false;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> is synchronized (thread safe).</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsSynchronized => false;

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new entry using the specified key.</returns>
		/// <param name="key">The key whose value to get or set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public object this[object key]
		{
			get
			{
				return inner[key];
			}
			set
			{
				inner[key] = value;
				if (list != null && Count > 10)
				{
					Switch();
				}
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</returns>
		public ICollection Keys => inner.Keys;

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</returns>
		public object SyncRoot => this;

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</returns>
		public ICollection Values => inner.Values;

		/// <summary>Creates an empty case-sensitive <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		public HybridDictionary()
			: this(0, caseInsensitive: false)
		{
		}

		/// <summary>Creates an empty <see cref="T:System.Collections.Specialized.HybridDictionary" /> with the specified case sensitivity.</summary>
		/// <param name="caseInsensitive">A Boolean that denotes whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> is case-insensitive. </param>
		public HybridDictionary(bool caseInsensitive)
			: this(0, caseInsensitive)
		{
		}

		/// <summary>Creates a case-sensitive <see cref="T:System.Collections.Specialized.HybridDictionary" /> with the specified initial size.</summary>
		/// <param name="initialSize">The approximate number of entries that the <see cref="T:System.Collections.Specialized.HybridDictionary" /> can initially contain. </param>
		public HybridDictionary(int initialSize)
			: this(initialSize, caseInsensitive: false)
		{
		}

		/// <summary>Creates a <see cref="T:System.Collections.Specialized.HybridDictionary" /> with the specified initial size and case sensitivity.</summary>
		/// <param name="initialSize">The approximate number of entries that the <see cref="T:System.Collections.Specialized.HybridDictionary" /> can initially contain. </param>
		/// <param name="caseInsensitive">A Boolean that denotes whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> is case-insensitive. </param>
		public HybridDictionary(int initialSize, bool caseInsensitive)
		{
			this.caseInsensitive = caseInsensitive;
			IComparer comparer = (!caseInsensitive) ? null : CaseInsensitiveComparer.DefaultInvariant;
			IHashCodeProvider hcp = (!caseInsensitive) ? null : CaseInsensitiveHashCodeProvider.DefaultInvariant;
			if (initialSize <= 10)
			{
				list = new ListDictionary(comparer);
			}
			else
			{
				hashtable = new Hashtable(initialSize, hcp, comparer);
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <param name="key">The key of the entry to add. </param>
		/// <param name="value">The value of the entry to add. The value can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An entry with the same key already exists in the <see cref="T:System.Collections.Specialized.HybridDictionary" />. </exception>
		public void Add(object key, object value)
		{
			inner.Add(key, value);
			if (list != null && Count > 10)
			{
				Switch();
			}
		}

		/// <summary>Removes all entries from the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		public void Clear()
		{
			inner.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Specialized.HybridDictionary" /> contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.HybridDictionary" /> contains an entry with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Specialized.HybridDictionary" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public bool Contains(object key)
		{
			return inner.Contains(key);
		}

		/// <summary>Copies the <see cref="T:System.Collections.Specialized.HybridDictionary" /> entries to a one-dimensional <see cref="T:System.Array" /> instance at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Specialized.HybridDictionary" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.HybridDictionary" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.HybridDictionary" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(Array array, int index)
		{
			inner.CopyTo(array, index);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		/// <summary>Removes the entry with the specified key from the <see cref="T:System.Collections.Specialized.HybridDictionary" />.</summary>
		/// <param name="key">The key of the entry to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public void Remove(object key)
		{
			inner.Remove(key);
		}

		private void Switch()
		{
			IComparer comparer = (!caseInsensitive) ? null : CaseInsensitiveComparer.DefaultInvariant;
			IHashCodeProvider hcp = (!caseInsensitive) ? null : CaseInsensitiveHashCodeProvider.DefaultInvariant;
			hashtable = new Hashtable(list, hcp, comparer);
			list.Clear();
			list = null;
		}
	}
}
