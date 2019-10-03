using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Net
{
	/// <summary>Provides a collection container for instances of the <see cref="T:System.Net.Cookie" /> class.</summary>
	[Serializable]
	public sealed class CookieCollection : ICollection, IEnumerable
	{
		private sealed class CookieCollectionComparer : IComparer<Cookie>
		{
			public int Compare(Cookie x, Cookie y)
			{
				if (x == null || y == null)
				{
					return 0;
				}
				int num = x.Name.Length + x.Value.Length;
				int num2 = y.Name.Length + y.Value.Length;
				return num - num2;
			}
		}

		private List<Cookie> list = new List<Cookie>();

		private static CookieCollectionComparer Comparer = new CookieCollectionComparer();

		internal IList<Cookie> List => list;

		/// <summary>Gets the number of cookies contained in a <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <returns>The number of cookies contained in a <see cref="T:System.Net.CookieCollection" />.</returns>
		public int Count => list.Count;

		/// <summary>Gets a value that indicates whether access to a <see cref="T:System.Net.CookieCollection" /> is thread safe.</summary>
		/// <returns>true if access to the <see cref="T:System.Net.CookieCollection" /> is thread safe; otherwise, false. The default is false.</returns>
		public bool IsSynchronized => false;

		/// <summary>Gets an object that you can use to synchronize access to the <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <returns>An object that you can use to synchronize access to the <see cref="T:System.Net.CookieCollection" />.</returns>
		public object SyncRoot => this;

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Net.CookieCollection" /> is read-only.</summary>
		/// <returns>true if this is a read-only <see cref="T:System.Net.CookieCollection" />; otherwise, false. The default is true.</returns>
		public bool IsReadOnly => true;

		/// <summary>Gets the <see cref="T:System.Net.Cookie" /> with a specific index from a <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <returns>A <see cref="T:System.Net.Cookie" /> with a specific index from a <see cref="T:System.Net.CookieCollection" />.</returns>
		/// <param name="index">The zero-based index of the <see cref="T:System.Net.Cookie" /> to be found. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0 or <paramref name="index" /> is greater than or equal to <see cref="P:System.Net.CookieCollection.Count" />. </exception>
		public Cookie this[int index]
		{
			get
			{
				if (index < 0 || index >= list.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return list[index];
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.Cookie" /> with a specific name from a <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <returns>The <see cref="T:System.Net.Cookie" /> with a specific name from a <see cref="T:System.Net.CookieCollection" />.</returns>
		/// <param name="name">The name of the <see cref="T:System.Net.Cookie" /> to be found. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		public Cookie this[string name]
		{
			get
			{
				foreach (Cookie item in list)
				{
					if (string.Compare(item.Name, name, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
					{
						return item;
					}
				}
				return null;
			}
		}

		/// <summary>Copies the elements of a <see cref="T:System.Net.CookieCollection" /> to an instance of the <see cref="T:System.Array" /> class, starting at a particular index.</summary>
		/// <param name="array">The target <see cref="T:System.Array" /> to which the <see cref="T:System.Net.CookieCollection" /> will be copied. </param>
		/// <param name="index">The zero-based index in the target <see cref="T:System.Array" /> where copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in this <see cref="T:System.Net.CookieCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The elements in this <see cref="T:System.Net.CookieCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(Array array, int index)
		{
			((ICollection)list).CopyTo(array, index);
		}

		/// <summary>Copies the elements of this <see cref="T:System.Net.CookieCollection" /> to a <see cref="T:System.Net.Cookie" /> array starting at the specified index of the target array.</summary>
		/// <param name="array">The target <see cref="T:System.Net.Cookie" /> array to which the <see cref="T:System.Net.CookieCollection" /> will be copied.</param>
		/// <param name="index">The zero-based index in the target <see cref="T:System.Array" /> where copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in this <see cref="T:System.Net.CookieCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The elements in this <see cref="T:System.Net.CookieCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(Cookie[] array, int index)
		{
			list.CopyTo(array, index);
		}

		/// <summary>Gets an enumerator that can iterate through a <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <returns>An instance of an implementation of an <see cref="T:System.Collections.IEnumerator" /> interface that can iterate through a <see cref="T:System.Net.CookieCollection" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>Adds a <see cref="T:System.Net.Cookie" /> to a <see cref="T:System.Net.CookieCollection" />.</summary>
		/// <param name="cookie">The <see cref="T:System.Net.Cookie" /> to be added to a <see cref="T:System.Net.CookieCollection" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookie" /> is null. </exception>
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			int num = SearchCookie(cookie);
			if (num == -1)
			{
				list.Add(cookie);
			}
			else
			{
				list[num] = cookie;
			}
		}

		internal void Sort()
		{
			if (list.Count > 0)
			{
				list.Sort(Comparer);
			}
		}

		private int SearchCookie(Cookie cookie)
		{
			string name = cookie.Name;
			string domain = cookie.Domain;
			string path = cookie.Path;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				Cookie cookie2 = list[num];
				if (cookie2.Version == cookie.Version && string.Compare(domain, cookie2.Domain, ignoreCase: true, CultureInfo.InvariantCulture) == 0 && string.Compare(name, cookie2.Name, ignoreCase: true, CultureInfo.InvariantCulture) == 0 && string.Compare(path, cookie2.Path, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
				{
					return num;
				}
			}
			return -1;
		}

		/// <summary>Adds the contents of a <see cref="T:System.Net.CookieCollection" /> to the current instance.</summary>
		/// <param name="cookies">The <see cref="T:System.Net.CookieCollection" /> to be added. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cookies" /> is null. </exception>
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (Cookie cooky in cookies)
			{
				Add(cooky);
			}
		}
	}
}
