using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Group : CompositeExpression
	{
		public Expression Expression
		{
			get
			{
				return base.Expressions[0];
			}
			set
			{
				base.Expressions[0] = value;
			}
		}

		public void AppendExpression(Expression e)
		{
			base.Expressions.Add(e);
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			int count = base.Expressions.Count;
			for (int i = 0; i < count; i++)
			{
				Expression expression = (!reverse) ? base.Expressions[i] : base.Expressions[count - i - 1];
				expression.Compile(cmp, reverse);
			}
		}

		public override void GetWidth(out int min, out int max)
		{
			min = 0;
			max = 0;
			foreach (Expression expression in base.Expressions)
			{
				expression.GetWidth(out int min2, out int max2);
				min += min2;
				if (max == int.MaxValue || max2 == int.MaxValue)
				{
					max = int.MaxValue;
				}
				else
				{
					max += max2;
				}
			}
		}

		public override AnchorInfo GetAnchorInfo(bool reverse)
		{
			int fixedWidth = GetFixedWidth();
			ArrayList arrayList = new ArrayList();
			IntervalCollection intervalCollection = new IntervalCollection();
			int num = 0;
			int count = base.Expressions.Count;
			for (int i = 0; i < count; i++)
			{
				Expression expression = (!reverse) ? base.Expressions[i] : base.Expressions[count - i - 1];
				AnchorInfo anchorInfo = expression.GetAnchorInfo(reverse);
				arrayList.Add(anchorInfo);
				if (anchorInfo.IsPosition)
				{
					return new AnchorInfo(this, num + anchorInfo.Offset, fixedWidth, anchorInfo.Position);
				}
				if (anchorInfo.IsSubstring)
				{
					intervalCollection.Add(anchorInfo.GetInterval(num));
				}
				if (anchorInfo.IsUnknownWidth)
				{
					break;
				}
				num += anchorInfo.Width;
			}
			intervalCollection.Normalize();
			Interval interval = Interval.Empty;
			foreach (Interval item in intervalCollection)
			{
				if (item.Size > interval.Size)
				{
					interval = item;
				}
			}
			if (interval.IsEmpty)
			{
				return new AnchorInfo(this, fixedWidth);
			}
			bool flag = false;
			int num2 = 0;
			num = 0;
			for (int j = 0; j < arrayList.Count; j++)
			{
				AnchorInfo anchorInfo2 = (AnchorInfo)arrayList[j];
				if (anchorInfo2.IsSubstring && interval.Contains(anchorInfo2.GetInterval(num)))
				{
					flag |= anchorInfo2.IgnoreCase;
					arrayList[num2++] = anchorInfo2;
				}
				if (anchorInfo2.IsUnknownWidth)
				{
					break;
				}
				num += anchorInfo2.Width;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int k = 0; k < num2; k++)
			{
				AnchorInfo anchorInfo3 = (!reverse) ? ((AnchorInfo)arrayList[k]) : ((AnchorInfo)arrayList[num2 - k - 1]);
				stringBuilder.Append(anchorInfo3.Substring);
			}
			if (stringBuilder.Length == interval.Size)
			{
				return new AnchorInfo(this, interval.low, fixedWidth, stringBuilder.ToString(), flag);
			}
			if (stringBuilder.Length > interval.Size)
			{
				Console.Error.WriteLine("overlapping?");
				return new AnchorInfo(this, fixedWidth);
			}
			throw new SystemException("Shouldn't happen");
		}
	}
}
