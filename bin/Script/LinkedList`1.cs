using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	/// <summary>Represents a doubly linked list.</summary>
	/// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	[ComVisible(false)]
	public class LinkedList<T> : IEnumerable<T>, ICollection, IDeserializationCallback, IEnumerable, ICollection<T>, ISerializable
	{
		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private const string VersionKey = "version";

			private const string IndexKey = "index";

			private const string ListKey = "list";

			private LinkedList<T> list;

			private LinkedListNode<T> current;

			private int index;

			private uint version;

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current => Current;

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.LinkedList`1" /> at the current position of the enumerator.</returns>
			public T Current
			{
				get
				{
					if (list == null)
					{
						throw new ObjectDisposedException(null);
					}
					if (current == null)
					{
						throw new InvalidOperationException();
					}
					return current.Value;
				}
			}

			internal Enumerator(LinkedList<T> parent)
			{
				list = parent;
				current = null;
				index = -1;
				version = parent.version;
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection. This class cannot be inherited.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				if (list == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (version != list.version)
				{
					throw new InvalidOperationException("list modified");
				}
				current = null;
				index = -1;
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (list == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (version != list.version)
				{
					throw new InvalidOperationException("list modified");
				}
				if (current == null)
				{
					current = list.first;
				}
				else
				{
					current = current.forward;
					if (current == list.first)
					{
						current = null;
					}
				}
				if (current == null)
				{
					index = -1;
					return false;
				}
				index++;
				return true;
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" />.</summary>
			public void Dispose()
			{
				if (list == null)
				{
					throw new ObjectDisposedException(null);
				}
				current = null;
				list = null;
			}
		}

		private const string DataArrayKey = "DataArray";

		private const string VersionKey = "version";

		private uint count;

		private uint version;

		private object syncRoot;

		internal LinkedListNode<T> first;

		internal SerializationInfo si;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns false.</returns>
		bool ICollection<T>.IsReadOnly => false;

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns false.</returns>
		bool ICollection.IsSynchronized => false;

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns the current instance.</returns>
		object ICollection.SyncRoot => syncRoot;

		/// <summary>Gets the number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public int Count => (int)count;

		/// <summary>Gets the first node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> First => first;

		/// <summary>Gets the last node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> Last => (first == null) ? null : first.back;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is empty.</summary>
		public LinkedList()
		{
			syncRoot = new object();
			first = null;
			count = (version = 0u);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that contains elements copied from the specified <see cref="T:System.Collections.IEnumerable" /> and has sufficient capacity to accommodate the number of elements copied. </summary>
		/// <param name="collection">The <see cref="T:System.Collections.IEnumerable" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public LinkedList(IEnumerable<T> collection)
			: this()
		{
			foreach (T item in collection)
			{
				AddLast(item);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is serializable with the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		protected LinkedList(SerializationInfo info, StreamingContext context)
			: this()
		{
			si = info;
			syncRoot = new object();
		}

		/// <summary>Adds an item at the end of the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		/// <param name="value">The value to add at the end of the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		void ICollection<T>.Add(T value)
		{
			AddLast(value);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			T[] array2 = array as T[];
			if (array2 == null)
			{
				throw new ArgumentException("array");
			}
			CopyTo(array2, index);
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through the linked list as a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the linked list as a collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void VerifyReferencedNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.List != this)
			{
				throw new InvalidOperationException();
			}
		}

		private static void VerifyBlankNode(LinkedListNode<T> newNode)
		{
			if (newNode == null)
			{
				throw new ArgumentNullException("newNode");
			}
			if (newNode.List != null)
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>Adds a new node containing the specified value after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
		/// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			VerifyReferencedNode(node);
			LinkedListNode<T> result = new LinkedListNode<T>(this, value, node, node.forward);
			count++;
			version++;
			return result;
		}

		/// <summary>Adds the specified new node after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert <paramref name="newNode" />.</param>
		/// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			VerifyReferencedNode(node);
			VerifyBlankNode(newNode);
			newNode.InsertBetween(node, node.forward, this);
			count++;
			version++;
		}

		/// <summary>Adds a new node containing the specified value before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
		/// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			VerifyReferencedNode(node);
			LinkedListNode<T> result = new LinkedListNode<T>(this, value, node.back, node);
			count++;
			version++;
			if (node == first)
			{
				first = result;
			}
			return result;
		}

		/// <summary>Adds the specified new node before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert <paramref name="newNode" />.</param>
		/// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			VerifyReferencedNode(node);
			VerifyBlankNode(newNode);
			newNode.InsertBetween(node.back, node, this);
			count++;
			version++;
			if (node == first)
			{
				first = newNode;
			}
		}

		/// <summary>Adds the specified new node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddFirst(LinkedListNode<T> node)
		{
			VerifyBlankNode(node);
			if (first == null)
			{
				node.SelfReference(this);
			}
			else
			{
				node.InsertBetween(first.back, first, this);
			}
			count++;
			version++;
			first = node;
		}

		/// <summary>Adds a new node containing the specified value at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="value">The value to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> result = (first != null) ? new LinkedListNode<T>(this, value, first.back, first) : new LinkedListNode<T>(this, value);
			count++;
			version++;
			first = result;
			return result;
		}

		/// <summary>Adds a new node containing the specified value at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="value">The value to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> result = (first != null) ? new LinkedListNode<T>(this, value, first.back, first) : (first = new LinkedListNode<T>(this, value));
			count++;
			version++;
			return result;
		}

		/// <summary>Adds the specified new node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddLast(LinkedListNode<T> node)
		{
			VerifyBlankNode(node);
			if (first == null)
			{
				node.SelfReference(this);
				first = node;
			}
			else
			{
				node.InsertBetween(first.back, first, this);
			}
			count++;
			version++;
		}

		/// <summary>Removes all nodes from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		public void Clear()
		{
			while (first != null)
			{
				RemoveLast();
			}
		}

		/// <summary>Determines whether a value is in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Collections.Generic.LinkedList`1" />; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />. The value can be null for reference types.</param>
		public bool Contains(T value)
		{
			LinkedListNode<T> forward = first;
			if (forward == null)
			{
				return false;
			}
			do
			{
				if (value.Equals(forward.Value))
				{
					return true;
				}
				forward = forward.forward;
			}
			while (forward != first);
			return false;
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Generic.LinkedList`1" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.LinkedList`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.LinkedList`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if ((uint)index < (uint)array.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("array", "Array is multidimensional");
			}
			if (array.Length - index + array.GetLowerBound(0) < count)
			{
				throw new ArgumentException("number of items exceeds capacity");
			}
			LinkedListNode<T> forward = first;
			if (first != null)
			{
				do
				{
					array[index] = forward.Value;
					index++;
					forward = forward.forward;
				}
				while (forward != first);
			}
		}

		/// <summary>Finds the first node that contains the specified value.</summary>
		/// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> forward = first;
			if (forward == null)
			{
				return null;
			}
			do
			{
				if ((value == null && forward.Value == null) || (value != null && value.Equals(forward.Value)))
				{
					return forward;
				}
				forward = forward.forward;
			}
			while (forward != first);
			return null;
		}

		/// <summary>Finds the last node that contains the specified value.</summary>
		/// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> FindLast(T value)
		{
			LinkedListNode<T> back = first;
			if (back == null)
			{
				return null;
			}
			do
			{
				back = back.back;
				if (value.Equals(back.Value))
				{
					return back;
				}
			}
			while (back != first);
			return null;
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			T[] array = new T[count];
			CopyTo(array, 0);
			info.AddValue("DataArray", array, typeof(T[]));
			info.AddValue("version", version);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.LinkedList`1" /> instance is invalid.</exception>
		public virtual void OnDeserialization(object sender)
		{
			if (si == null)
			{
				return;
			}
			T[] array = (T[])si.GetValue("DataArray", typeof(T[]));
			if (array != null)
			{
				T[] array2 = array;
				foreach (T value in array2)
				{
					AddLast(value);
				}
			}
			version = si.GetUInt32("version");
			si = null;
		}

		/// <summary>Removes the first occurrence of the specified value from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>true if the element containing <paramref name="value" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="value" /> was not found in the original <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		/// <param name="value">The value to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public bool Remove(T value)
		{
			LinkedListNode<T> linkedListNode = Find(value);
			if (linkedListNode == null)
			{
				return false;
			}
			Remove(linkedListNode);
			return true;
		}

		/// <summary>Removes the specified node from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void Remove(LinkedListNode<T> node)
		{
			VerifyReferencedNode(node);
			count--;
			if (count == 0)
			{
				first = null;
			}
			if (node == first)
			{
				first = first.forward;
			}
			version++;
			node.Detach();
		}

		/// <summary>Removes the node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
		public void RemoveFirst()
		{
			if (first != null)
			{
				Remove(first);
			}
		}

		/// <summary>Removes the node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
		public void RemoveLast()
		{
			if (first != null)
			{
				Remove(first.back);
			}
		}
	}
}
