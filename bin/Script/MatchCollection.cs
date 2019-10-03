using System.Collections;

namespace System.Text.RegularExpressions
{
	/// <summary>Represents the set of successful matches found by iteratively applying a regular expression pattern to the input string.</summary>
	[Serializable]
	public class MatchCollection : ICollection, IEnumerable
	{
		private class Enumerator : IEnumerator
		{
			private int index;

			private MatchCollection coll;

			object IEnumerator.Current
			{
				get
				{
					if (index < 0)
					{
						throw new InvalidOperationException("'Current' called before 'MoveNext()'");
					}
					if (index > coll.list.Count)
					{
						throw new SystemException("MatchCollection in invalid state");
					}
					if (index == coll.list.Count && !coll.current.Success)
					{
						throw new InvalidOperationException("'Current' called after 'MoveNext()' returned false");
					}
					return (index >= coll.list.Count) ? coll.current : coll.list[index];
				}
			}

			internal Enumerator(MatchCollection coll)
			{
				this.coll = coll;
				index = -1;
			}

			void IEnumerator.Reset()
			{
				index = -1;
			}

			bool IEnumerator.MoveNext()
			{
				if (index > coll.list.Count)
				{
					throw new SystemException("MatchCollection in invalid state");
				}
				if (index == coll.list.Count && !coll.current.Success)
				{
					return false;
				}
				return coll.TryToGet(++index);
			}
		}

		private Match current;

		private ArrayList list;

		/// <summary>Gets the number of matches.</summary>
		/// <returns>The number of matches.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public int Count => FullList.Count;

		/// <summary>Gets a value that indicates whether the collection is read only.</summary>
		/// <returns>This value of this property is always true.</returns>
		public bool IsReadOnly => true;

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread-safe).</summary>
		/// <returns>The value of this property is always false.</returns>
		public bool IsSynchronized => false;

		/// <summary>Gets an individual member of the collection.</summary>
		/// <returns>The captured substring at position <paramref name="i" /> in the collection.</returns>
		/// <param name="i">Index into the Match collection. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="i" /> is less than 0 or greater than or equal to <see cref="P:System.Text.RegularExpressions.MatchCollection.Count" />. </exception>
		public virtual Match this[int i]
		{
			get
			{
				if (i < 0 || !TryToGet(i))
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return (i >= list.Count) ? current : ((Match)list[i]);
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection. This property always returns the object itself.</returns>
		public object SyncRoot => list;

		private ICollection FullList
		{
			get
			{
				if (TryToGet(int.MaxValue))
				{
					throw new SystemException("too many matches");
				}
				return list;
			}
		}

		internal MatchCollection(Match start)
		{
			current = start;
			list = new ArrayList();
		}

		/// <summary>Copies all the elements of the collection to the given array starting at the given index.</summary>
		/// <param name="array">The array the collection is to be copied into. </param>
		/// <param name="arrayIndex">The position in the array where copying is to begin. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is a multi-dimensional array.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />.-or-<paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.GroupCollection.Count" /> is outside the bounds of <paramref name="array" />.</exception>
		public void CopyTo(Array array, int index)
		{
			FullList.CopyTo(array, index);
		}

		/// <summary>Provides an enumerator in the same order as <see cref="P:System.Text.RegularExpressions.MatchCollection.Item(System.Int32)" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that contains all Match objects within the MatchCollection.</returns>
		public IEnumerator GetEnumerator()
		{
			object result;
			if (current.Success)
			{
				IEnumerator enumerator = new Enumerator(this);
				result = enumerator;
			}
			else
			{
				result = list.GetEnumerator();
			}
			return (IEnumerator)result;
		}

		private bool TryToGet(int i)
		{
			while (i > list.Count && current.Success)
			{
				list.Add(current);
				current = current.NextMatch();
			}
			return i < list.Count || current.Success;
		}
	}
}
