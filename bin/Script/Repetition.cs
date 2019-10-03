namespace System.Text.RegularExpressions.Syntax
{
	internal class Repetition : CompositeExpression
	{
		private int min;

		private int max;

		private bool lazy;

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

		public int Minimum
		{
			get
			{
				return min;
			}
			set
			{
				min = value;
			}
		}

		public int Maximum
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}

		public bool Lazy
		{
			get
			{
				return lazy;
			}
			set
			{
				lazy = value;
			}
		}

		public Repetition(int min, int max, bool lazy)
		{
			base.Expressions.Add(null);
			this.min = min;
			this.max = max;
			this.lazy = lazy;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (Expression.IsComplex())
			{
				LinkRef linkRef = cmp.NewLink();
				cmp.EmitRepeat(min, max, lazy, linkRef);
				Expression.Compile(cmp, reverse);
				cmp.EmitUntil(linkRef);
			}
			else
			{
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitFastRepeat(min, max, lazy, linkRef2);
				Expression.Compile(cmp, reverse);
				cmp.EmitTrue();
				cmp.ResolveLink(linkRef2);
			}
		}

		public override void GetWidth(out int min, out int max)
		{
			Expression.GetWidth(out min, out max);
			min *= this.min;
			if (max == int.MaxValue || this.max == 65535)
			{
				max = int.MaxValue;
			}
			else
			{
				max *= this.max;
			}
		}

		public override AnchorInfo GetAnchorInfo(bool reverse)
		{
			int fixedWidth = GetFixedWidth();
			if (Minimum == 0)
			{
				return new AnchorInfo(this, fixedWidth);
			}
			AnchorInfo anchorInfo = Expression.GetAnchorInfo(reverse);
			if (anchorInfo.IsPosition)
			{
				return new AnchorInfo(this, anchorInfo.Offset, fixedWidth, anchorInfo.Position);
			}
			if (anchorInfo.IsSubstring)
			{
				if (anchorInfo.IsComplete)
				{
					string substring = anchorInfo.Substring;
					StringBuilder stringBuilder = new StringBuilder(substring);
					for (int i = 1; i < Minimum; i++)
					{
						stringBuilder.Append(substring);
					}
					return new AnchorInfo(this, 0, fixedWidth, stringBuilder.ToString(), anchorInfo.IgnoreCase);
				}
				return new AnchorInfo(this, anchorInfo.Offset, fixedWidth, anchorInfo.Substring, anchorInfo.IgnoreCase);
			}
			return new AnchorInfo(this, fixedWidth);
		}
	}
}
