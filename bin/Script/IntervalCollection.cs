using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class IntervalCollection : ICollection, IEnumerable
	{
		private class Enumerator : IEnumerator
		{
			private IList list;

			private int ptr;

			public object Current
			{
				get
				{
					if (ptr >= list.Count)
					{
						throw new InvalidOperationException();
					}
					return list[ptr];
				}
			}

			public Enumerator(IList list)
			{
				this.list = list;
				Reset();
			}

			public bool MoveNext()
			{
				if (ptr > list.Count)
				{
					throw new InvalidOperationException();
				}
				return ++ptr < list.Count;
			}

			public void Reset()
			{
				ptr = -1;
			}
		}

		public delegate double CostDelegate(Interval i);

		private ArrayList intervals;

		public Interval this[int i]
		{
			get
			{
				return (Interval)intervals[i];
			}
			set
			{
				intervals[i] = value;
			}
		}

		public int Count => intervals.Count;

		public bool IsSynchronized => false;

		public object SyncRoot => intervals;

		public IntervalCollection()
		{
			intervals = new ArrayList();
		}

		public void Add(Interval i)
		{
			intervals.Add(i);
		}

		public void Clear()
		{
			intervals.Clear();
		}

		public void Sort()
		{
			intervals.Sort();
		}

		public void Normalize()
		{
			intervals.Sort();
			int num = 0;
			while (num < intervals.Count - 1)
			{
				Interval interval = (Interval)intervals[num];
				Interval i = (Interval)intervals[num + 1];
				if (!interval.IsDisjoint(i) || interval.IsAdjacent(i))
				{
					interval.Merge(i);
					intervals[num] = interval;
					intervals.RemoveAt(num + 1);
				}
				else
				{
					num++;
				}
			}
		}

		public IntervalCollection GetMetaCollection(CostDelegate cost_del)
		{
			IntervalCollection intervalCollection = new IntervalCollection();
			Normalize();
			Optimize(0, Count - 1, intervalCollection, cost_del);
			intervalCollection.intervals.Sort();
			return intervalCollection;
		}

		private void Optimize(int begin, int end, IntervalCollection meta, CostDelegate cost_del)
		{
			Interval i = default(Interval);
			i.contiguous = false;
			int num = -1;
			int num2 = -1;
			double num3 = 0.0;
			for (int j = begin; j <= end; j++)
			{
				Interval interval = this[j];
				i.low = interval.low;
				double num4 = 0.0;
				for (int k = j; k <= end; k++)
				{
					Interval interval2 = this[k];
					i.high = interval2.high;
					num4 += cost_del(this[k]);
					double num5 = cost_del(i);
					if (num5 < num4 && num4 > num3)
					{
						num = j;
						num2 = k;
						num3 = num4;
					}
				}
			}
			if (num < 0)
			{
				for (int l = begin; l <= end; l++)
				{
					meta.Add(this[l]);
				}
				return;
			}
			Interval interval3 = this[num];
			i.low = interval3.low;
			Interval interval4 = this[num2];
			i.high = interval4.high;
			meta.Add(i);
			if (num > begin)
			{
				Optimize(begin, num - 1, meta, cost_del);
			}
			if (num2 < end)
			{
				Optimize(num2 + 1, end, meta, cost_del);
			}
		}

		public void CopyTo(Array array, int index)
		{
			foreach (Interval interval in intervals)
			{
				if (index > array.Length)
				{
					break;
				}
				array.SetValue(interval, index++);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return new Enumerator(intervals);
		}
	}
}
