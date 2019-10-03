using System.Collections;

namespace System.ComponentModel
{
	/// <summary>Represents a collection of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects.</summary>
	public class PropertyDescriptorCollection : ICollection, IDictionary, IEnumerable, IList
	{
		/// <summary>Specifies an empty collection that you can use instead of creating a new one with no items. This static field is read-only.</summary>
		public static readonly PropertyDescriptorCollection Empty = new PropertyDescriptorCollection(null, readOnly: true);

		private ArrayList properties;

		private bool readOnly;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, false.</returns>
		bool IDictionary.IsFixedSize => ((IList)this).IsFixedSize;

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>true if the collection has a fixed size; otherwise, false.</returns>
		bool IList.IsFixedSize => readOnly;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, false.</returns>
		bool IDictionary.IsReadOnly => ((IList)this).IsReadOnly;

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>true if the collection is read-only; otherwise, false.</returns>
		bool IList.IsReadOnly => readOnly;

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>true if access to the collection is synchronized (thread safe); otherwise, false.</returns>
		bool ICollection.IsSynchronized => false;

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements contained in the collection.</returns>
		int ICollection.Count => Count;

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		object ICollection.SyncRoot => null;

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Keys
		{
			get
			{
				string[] array = new string[properties.Count];
				int num = 0;
				foreach (PropertyDescriptor property in properties)
				{
					array[num++] = property.Name;
				}
				return array;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Values => (ICollection)properties.Clone();

		/// <summary>Gets or sets the element with the specified key. </summary>
		/// <returns>The element with the specified key.</returns>
		/// <param name="key">The key of the element to get or set. </param>
		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
				{
					return null;
				}
				return this[(string)key];
			}
			set
			{
				if (readOnly)
				{
					throw new NotSupportedException();
				}
				if (!(key is string) || !(value is PropertyDescriptor))
				{
					throw new ArgumentException();
				}
				int num = properties.IndexOf(value);
				if (num == -1)
				{
					Add((PropertyDescriptor)value);
				}
				else
				{
					properties[num] = value;
				}
			}
		}

		/// <summary>Gets or sets an item from the collection at a specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.ComponentModel.PropertyDescriptor" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is less than 0. -or-<paramref name="index" /> is equal to or greater than <see cref="P:System.ComponentModel.EventDescriptorCollection.Count" />.</exception>
		object IList.this[int index]
		{
			get
			{
				return properties[index];
			}
			set
			{
				if (readOnly)
				{
					throw new NotSupportedException();
				}
				properties[index] = value;
			}
		}

		/// <summary>Gets the number of property descriptors in the collection.</summary>
		/// <returns>The number of property descriptors in the collection.</returns>
		public int Count => properties.Count;

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> with the specified name.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> with the specified name, or null if the property does not exist.</returns>
		/// <param name="name">The name of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to get from the collection. </param>
		public virtual PropertyDescriptor this[string s] => Find(s, ignoreCase: false);

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> at the specified index number.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> with the specified index number.</returns>
		/// <param name="index">The zero-based index of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to get or set. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The <paramref name="index" /> parameter is not a valid index for <see cref="P:System.ComponentModel.PropertyDescriptorCollection.Item(System.Int32)" />. </exception>
		public virtual PropertyDescriptor this[int index] => (PropertyDescriptor)properties[index];

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> class.</summary>
		/// <param name="properties">An array of type <see cref="T:System.ComponentModel.PropertyDescriptor" /> that provides the properties for this collection. </param>
		public PropertyDescriptorCollection(PropertyDescriptor[] properties)
		{
			this.properties = new ArrayList();
			if (properties != null)
			{
				this.properties.AddRange(properties);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> class, which is optionally read-only.</summary>
		/// <param name="properties">An array of type <see cref="T:System.ComponentModel.PropertyDescriptor" /> that provides the properties for this collection.</param>
		/// <param name="readOnly">If true, specifies that the collection cannot be modified.</param>
		public PropertyDescriptorCollection(PropertyDescriptor[] properties, bool readOnly)
			: this(properties)
		{
			this.readOnly = readOnly;
		}

		private PropertyDescriptorCollection()
		{
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.</summary>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <param name="value">The item to add to the collection.</param>
		int IList.Add(object value)
		{
			return Add((PropertyDescriptor)value);
		}

		/// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The <see cref="T:System.Object" /> to use as the key of the element to add.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to use as the value of the element to add.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is null.</exception>
		void IDictionary.Add(object key, object value)
		{
			if (!(value is PropertyDescriptor))
			{
				throw new ArgumentException("value");
			}
			Add((PropertyDescriptor)value);
		}

		/// <summary>Removes all items from the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Clear()
		{
			Clear();
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.IDictionary" />. </summary>
		void IDictionary.Clear()
		{
			Clear();
		}

		/// <summary>Determines whether the collection contains a specific value.</summary>
		/// <returns>true if the item is found in the collection; otherwise, false.</returns>
		/// <param name="value">The item to locate in the collection.</param>
		bool IList.Contains(object value)
		{
			return Contains((PropertyDescriptor)value);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
		bool IDictionary.Contains(object value)
		{
			return Contains((PropertyDescriptor)value);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Returns an enumerator for this class.</summary>
		/// <returns>An enumerator of type <see cref="T:System.Collections.IEnumerator" />.</returns>
		[MonoTODO]
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines the index of a specified item in the collection.</summary>
		/// <returns>The index of <paramref name="value" /> if found in the list, otherwise -1.</returns>
		/// <param name="value">The item to locate in the collection.</param>
		int IList.IndexOf(object value)
		{
			return IndexOf((PropertyDescriptor)value);
		}

		/// <summary>Inserts an item into the collection at a specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The item to insert into the collection.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Insert(int index, object value)
		{
			Insert(index, (PropertyDescriptor)value);
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />. </summary>
		/// <param name="key">The key of the element to remove.</param>
		void IDictionary.Remove(object value)
		{
			Remove((PropertyDescriptor)value);
		}

		/// <summary>Removes the first occurrence of a specified value from the collection.</summary>
		/// <param name="value">The item to remove from the collection.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Remove(object value)
		{
			Remove((PropertyDescriptor)value);
		}

		/// <summary>Removes the item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		/// <summary>Adds the specified <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the collection.</summary>
		/// <returns>The index of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that was added to the collection.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to add to the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public int Add(PropertyDescriptor value)
		{
			if (readOnly)
			{
				throw new NotSupportedException();
			}
			properties.Add(value);
			return properties.Count - 1;
		}

		/// <summary>Removes all <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects from the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Clear()
		{
			if (readOnly)
			{
				throw new NotSupportedException();
			}
			properties.Clear();
		}

		/// <summary>Returns whether the collection contains the given <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <returns>true if the collection contains the given <see cref="T:System.ComponentModel.PropertyDescriptor" />; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to find in the collection. </param>
		public bool Contains(PropertyDescriptor value)
		{
			return properties.Contains(value);
		}

		/// <summary>Copies the entire collection to an array, starting at the specified index number.</summary>
		/// <param name="array">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to copy elements of the collection to. </param>
		/// <param name="index">The index of the <paramref name="array" /> parameter at which copying begins. </param>
		public void CopyTo(Array array, int index)
		{
			properties.CopyTo(array, index);
		}

		/// <summary>Returns the <see cref="T:System.ComponentModel.PropertyDescriptor" /> with the specified name, using a Boolean to indicate whether to ignore case.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> with the specified name, or null if the property does not exist.</returns>
		/// <param name="name">The name of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to return from the collection. </param>
		/// <param name="ignoreCase">true if you want to ignore the case of the property name; otherwise, false. </param>
		public virtual PropertyDescriptor Find(string name, bool ignoreCase)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[i];
				if (ignoreCase)
				{
					if (string.Compare(name, propertyDescriptor.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return propertyDescriptor;
					}
				}
				else if (string.Compare(name, propertyDescriptor.Name, StringComparison.Ordinal) == 0)
				{
					return propertyDescriptor;
				}
			}
			return null;
		}

		/// <summary>Returns an enumerator for this class.</summary>
		/// <returns>An enumerator of type <see cref="T:System.Collections.IEnumerator" />.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return properties.GetEnumerator();
		}

		/// <summary>Returns the index of the given <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <returns>The index of the given <see cref="T:System.ComponentModel.PropertyDescriptor" />.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to return the index of. </param>
		public int IndexOf(PropertyDescriptor value)
		{
			return properties.IndexOf(value);
		}

		/// <summary>Adds the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the collection at the specified index number.</summary>
		/// <param name="index">The index at which to add the <paramref name="value" /> parameter to the collection. </param>
		/// <param name="value">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to add to the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Insert(int index, PropertyDescriptor value)
		{
			if (readOnly)
			{
				throw new NotSupportedException();
			}
			properties.Insert(index, value);
		}

		/// <summary>Removes the specified <see cref="T:System.ComponentModel.PropertyDescriptor" /> from the collection.</summary>
		/// <param name="value">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to remove from the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Remove(PropertyDescriptor value)
		{
			if (readOnly)
			{
				throw new NotSupportedException();
			}
			properties.Remove(value);
		}

		/// <summary>Removes the <see cref="T:System.ComponentModel.PropertyDescriptor" /> at the specified index from the collection.</summary>
		/// <param name="index">The index of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to remove from the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void RemoveAt(int index)
		{
			if (readOnly)
			{
				throw new NotSupportedException();
			}
			properties.RemoveAt(index);
		}

		private PropertyDescriptorCollection CloneCollection()
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection();
			propertyDescriptorCollection.properties = (ArrayList)properties.Clone();
			return propertyDescriptorCollection;
		}

		/// <summary>Sorts the members of this collection, using the default sort for this collection, which is usually alphabetical.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the sorted <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects.</returns>
		public virtual PropertyDescriptorCollection Sort()
		{
			PropertyDescriptorCollection propertyDescriptorCollection = CloneCollection();
			propertyDescriptorCollection.InternalSort((IComparer)null);
			return propertyDescriptorCollection;
		}

		/// <summary>Sorts the members of this collection, using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the sorted <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects.</returns>
		/// <param name="comparer">A comparer to use to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		public virtual PropertyDescriptorCollection Sort(IComparer comparer)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = CloneCollection();
			propertyDescriptorCollection.InternalSort(comparer);
			return propertyDescriptorCollection;
		}

		/// <summary>Sorts the members of this collection. The specified order is applied first, followed by the default sort for this collection, which is usually alphabetical.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the sorted <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects.</returns>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		public virtual PropertyDescriptorCollection Sort(string[] order)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = CloneCollection();
			propertyDescriptorCollection.InternalSort(order);
			return propertyDescriptorCollection;
		}

		/// <summary>Sorts the members of this collection. The specified order is applied first, followed by the sort using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the sorted <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects.</returns>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		/// <param name="comparer">A comparer to use to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		public virtual PropertyDescriptorCollection Sort(string[] order, IComparer comparer)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = CloneCollection();
			if (order != null)
			{
				ArrayList arrayList = propertyDescriptorCollection.ExtractItems(order);
				propertyDescriptorCollection.InternalSort(comparer);
				arrayList.AddRange(propertyDescriptorCollection.properties);
				propertyDescriptorCollection.properties = arrayList;
			}
			else
			{
				propertyDescriptorCollection.InternalSort(comparer);
			}
			return propertyDescriptorCollection;
		}

		/// <summary>Sorts the members of this collection, using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="sorter">A comparer to use to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		protected void InternalSort(IComparer ic)
		{
			if (ic == null)
			{
				ic = MemberDescriptor.DefaultComparer;
			}
			properties.Sort(ic);
		}

		/// <summary>Sorts the members of this collection. The specified order is applied first, followed by the default sort for this collection, which is usually alphabetical.</summary>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects in this collection. </param>
		protected void InternalSort(string[] order)
		{
			if (order != null)
			{
				ArrayList arrayList = ExtractItems(order);
				InternalSort((IComparer)null);
				arrayList.AddRange(properties);
				properties = arrayList;
			}
			else
			{
				InternalSort((IComparer)null);
			}
		}

		private ArrayList ExtractItems(string[] names)
		{
			ArrayList arrayList = new ArrayList(properties.Count);
			object[] array = new object[names.Length];
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[i];
				int num = Array.IndexOf(names, propertyDescriptor.Name);
				if (num != -1)
				{
					array[num] = propertyDescriptor;
					properties.RemoveAt(i);
					i--;
				}
			}
			object[] array2 = array;
			foreach (object obj in array2)
			{
				if (obj != null)
				{
					arrayList.Add(obj);
				}
			}
			return arrayList;
		}

		internal PropertyDescriptorCollection Filter(Attribute[] attributes)
		{
			ArrayList arrayList = new ArrayList();
			foreach (PropertyDescriptor property in properties)
			{
				if (property.Attributes.Contains(attributes))
				{
					arrayList.Add(property);
				}
			}
			PropertyDescriptor[] array = new PropertyDescriptor[arrayList.Count];
			arrayList.CopyTo(array);
			return new PropertyDescriptorCollection(array, readOnly: true);
		}
	}
}
