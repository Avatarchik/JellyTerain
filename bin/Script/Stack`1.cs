using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	/// <summary>Represents a variable size last-in-first-out (LIFO) collection of instances of the same arbitrary type.</summary>
	/// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	[ComVisible(false)]
	public class Stack<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		[Serializable]
		public struct Enumerator : IEnumerator, IDisposable, IEnumerator<T>
		{
			private const int NOT_STARTED = -2;

			private const int FINISHED = -1;

			private Stack<T> parent;

			private int idx;

			private int _version;

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current => Current;

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.Stack`1" /> at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			public T Current
			{
				get
				{
					if (idx < 0)
					{
						throw new InvalidOperationException();
					}
					return parent._array[idx];
				}
			}

			internal Enumerator(Stack<T> t)
			{
				parent = t;
				idx = -2;
				_version = t._version;
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection. This class cannot be inherited.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				if (_version != parent._version)
				{
					throw new InvalidOperationException();
				}
				idx = -2;
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Stack`1.Enumerator" />.</summary>
			public void Dispose()
			{
				idx = -2;
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (_version != parent._version)
				{
					throw new InvalidOperationException();
				}
				if (idx == -2)
				{
					idx = parent._size;
				}
				return idx != -1 && --idx != -1;
			}
		}

		private const int INITIAL_SIZE = 16;

		private T[] _array;

		private int _size;

		private int _version;

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Stack`1" />, this property always returns false.</returns>
		bool ICollection.IsSynchronized => false;

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Stack`1" />, this property always returns the current instance.</returns>
		object ICollection.SyncRoot => this;

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
		public int Count => _size;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that is empty and has the default initial capacity.</summary>
		public Stack()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that is empty and has the specified initial capacity or the default initial capacity, whichever is greater.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Stack`1" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		public Stack(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			_array = new T[count];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
		/// <param name="collection">The collection to copy elements from.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public Stack(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				_size = collection2.Count;
				_array = new T[_size];
				collection2.CopyTo(_array, 0);
			}
			else
			{
				foreach (T item in collection)
				{
					Push(item);
				}
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		void ICollection.CopyTo(Array dest, int idx)
		{
			try
			{
				if (_array != null)
				{
					_array.CopyTo(dest, idx);
					Array.Reverse(dest, idx, _size);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException();
				IL_0031:;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Removes all objects from the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <filterpriority>1</filterpriority>
		public void Clear()
		{
			if (_array != null)
			{
				Array.Clear(_array, 0, _array.Length);
			}
			_size = 0;
			_version++;
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.Stack`1" />; otherwise, false.</returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.Stack`1" />. The value can be null for reference types.</param>
		public bool Contains(T t)
		{
			return _array != null && Array.IndexOf(_array, t, 0, _size) != -1;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Generic.Stack`1" /> to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Stack`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Stack`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] dest, int idx)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (idx < 0)
			{
				throw new ArgumentOutOfRangeException("idx");
			}
			if (_array != null)
			{
				Array.Copy(_array, 0, dest, idx, _size);
				Array.Reverse(dest, idx, _size);
			}
		}

		/// <summary>Returns the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" /> without removing it.</summary>
		/// <returns>The object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</exception>
		public T Peek()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException();
			}
			return _array[_size - 1];
		}

		/// <summary>Removes and returns the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <returns>The object removed from the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</exception>
		public T Pop()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException();
			}
			_version++;
			T result = _array[--_size];
			_array[_size] = default(T);
			return result;
		}

		/// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <param name="item">The object to push onto the <see cref="T:System.Collections.Generic.Stack`1" />. The value can be null for reference types.</param>
		public void Push(T t)
		{
			if (_array == null || _size == _array.Length)
			{
				Array.Resize(ref _array, (_size != 0) ? (2 * _size) : 16);
			}
			_version++;
			_array[_size++] = t;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Generic.Stack`1" /> to a new array.</summary>
		/// <returns>A new array containing copies of the elements of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
		public T[] ToArray()
		{
			T[] array = new T[_size];
			CopyTo(array, 0);
			return array;
		}

		/// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Generic.Stack`1" />, if that number is less than 90 percent of current capacity.</summary>
		public void TrimExcess()
		{
			if (_array != null && (double)_size < (double)_array.Length * 0.9)
			{
				Array.Resize(ref _array, _size);
			}
			_version++;
		}

		/// <summary>Returns an enumerator for the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.Stack`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}
