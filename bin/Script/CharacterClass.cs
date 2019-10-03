using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class CharacterClass : Expression
	{
		private const int distance_between_upper_and_lower_case = 32;

		private static Interval upper_case_characters = new Interval(65, 90);

		private bool negate;

		private bool ignore;

		private BitArray pos_cats;

		private BitArray neg_cats;

		private IntervalCollection intervals;

		public bool Negate
		{
			get
			{
				return negate;
			}
			set
			{
				negate = value;
			}
		}

		public bool IgnoreCase
		{
			get
			{
				return ignore;
			}
			set
			{
				ignore = value;
			}
		}

		public CharacterClass(bool negate, bool ignore)
		{
			this.negate = negate;
			this.ignore = ignore;
			intervals = new IntervalCollection();
			int length = 144;
			pos_cats = new BitArray(length);
			neg_cats = new BitArray(length);
		}

		public CharacterClass(Category cat, bool negate)
			: this(negate: false, ignore: false)
		{
			AddCategory(cat, negate);
		}

		public void AddCategory(Category cat, bool negate)
		{
			if (negate)
			{
				neg_cats[(int)cat] = true;
			}
			else
			{
				pos_cats[(int)cat] = true;
			}
		}

		public void AddCharacter(char c)
		{
			AddRange(c, c);
		}

		public void AddRange(char lo, char hi)
		{
			Interval i = new Interval(lo, hi);
			if (ignore)
			{
				if (upper_case_characters.Intersects(i))
				{
					Interval i2;
					if (i.low < upper_case_characters.low)
					{
						i2 = new Interval(upper_case_characters.low + 32, i.high + 32);
						i.high = upper_case_characters.low - 1;
					}
					else
					{
						i2 = new Interval(i.low + 32, upper_case_characters.high + 32);
						i.low = upper_case_characters.high + 1;
					}
					intervals.Add(i2);
				}
				else if (upper_case_characters.Contains(i))
				{
					i.high += 32;
					i.low += 32;
				}
			}
			intervals.Add(i);
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			IntervalCollection metaCollection = intervals.GetMetaCollection(GetIntervalCost);
			int num = metaCollection.Count;
			for (int i = 0; i < pos_cats.Length; i++)
			{
				if (pos_cats[i] || neg_cats[i])
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			LinkRef linkRef = cmp.NewLink();
			if (num > 1)
			{
				cmp.EmitIn(linkRef);
			}
			foreach (Interval item in metaCollection)
			{
				Interval interval = item;
				if (interval.IsDiscontiguous)
				{
					BitArray bitArray = new BitArray(interval.Size);
					foreach (Interval interval2 in intervals)
					{
						Interval i2 = interval2;
						if (interval.Contains(i2))
						{
							for (int j = i2.low; j <= i2.high; j++)
							{
								bitArray[j - interval.low] = true;
							}
						}
					}
					cmp.EmitSet((char)interval.low, bitArray, negate, ignore, reverse);
				}
				else if (interval.IsSingleton)
				{
					cmp.EmitCharacter((char)interval.low, negate, ignore, reverse);
				}
				else
				{
					cmp.EmitRange((char)interval.low, (char)interval.high, negate, ignore, reverse);
				}
			}
			for (int k = 0; k < pos_cats.Length; k++)
			{
				if (pos_cats[k])
				{
					if (neg_cats[k])
					{
						cmp.EmitCategory(Category.AnySingleline, negate, reverse);
					}
					else
					{
						cmp.EmitCategory((Category)k, negate, reverse);
					}
				}
				else if (neg_cats[k])
				{
					cmp.EmitNotCategory((Category)k, negate, reverse);
				}
			}
			if (num > 1)
			{
				if (negate)
				{
					cmp.EmitTrue();
				}
				else
				{
					cmp.EmitFalse();
				}
				cmp.ResolveLink(linkRef);
			}
		}

		public override void GetWidth(out int min, out int max)
		{
			min = (max = 1);
		}

		public override bool IsComplex()
		{
			return false;
		}

		private static double GetIntervalCost(Interval i)
		{
			if (i.IsDiscontiguous)
			{
				return 3 + (i.Size + 15 >> 4);
			}
			if (i.IsSingleton)
			{
				return 2.0;
			}
			return 3.0;
		}
	}
}
