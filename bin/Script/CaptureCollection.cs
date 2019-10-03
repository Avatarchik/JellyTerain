using System.Collections;

namespace System.Text.RegularExpressions
{
	/// <summary>Represents the set of captures made by a single capturing group.</summary>
	[Serializable]
	public class CaptureCollection : ICollection, IEnumerable
	{
		private Capture[] list;

		/// <summary>Gets the number of substrings captured by the group.</summary>
		/// <returns>The number of items in the <see cref="T:System.Text.RegularExpressions.CaptureCollection" />.</returns>
		public int Count => list.Length;

		/// <summary>Gets a value that indicates whether the collection is read only.</summary>
		/// <returns>true in all cases.</returns>
		public bool IsReadOnly => true;

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread-safe).</summary>
		/// <returns>false in all cases.</returns>
		public bool IsSynchronized => false;

		/// <summary>Gets an individual member of the collection.</summary>
		/// <returns>The captured substring at position <paramref name="i" /> in the collection.</returns>
		/// <param name="i">Index into the capture collection. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="i" /> is less than 0 or greater than <see cref="P:System.Text.RegularExpressions.CaptureCollection.Count" />. </exception>
		public Capture this[int i]
		{
			get
			{
				if (i < 0 || i >= Count)
				{
					throw new ArgumentOutOfRangeException("Index is out of range");
				}
				return list[i];
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		public object SyncRoot => list;

		internal CaptureCollection(int n)
		{
			list = new Capture[n];
		}

		internal void SetValue(Capture cap, int i)
		{
			list[i] = cap;
		}

		/// <summary>Copies all the elements of the collection to the given array beginning at the given index.</summary>
		/// <param name="array">The array the collection is to be copied into. </param>
		/// <param name="arrayIndex">The position in the destination array where copying is to begin. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array " />is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />. -or-<paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.CaptureCollection.Count" /> is outside the bounds of <paramref name="array" />. </exception>
		public void CopyTo(Array array, int index)
		{
			list.CopyTo(array, index);
		}

		/// <summary>Provides an enumerator that iterates through the collection.</summary>
		/// <returns>An object that contains all <see cref="T:System.Text.RegularExpressions.Capture" /> objects within the <see cref="T:System.Text.RegularExpressions.CaptureCollection" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}
