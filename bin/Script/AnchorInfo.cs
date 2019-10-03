namespace System.Text.RegularExpressions.Syntax
{
	internal class AnchorInfo
	{
		private Expression expr;

		private Position pos;

		private int offset;

		private string str;

		private int width;

		private bool ignore;

		public Expression Expression => expr;

		public int Offset => offset;

		public int Width => width;

		public int Length => (str != null) ? str.Length : 0;

		public bool IsUnknownWidth => width < 0;

		public bool IsComplete => Length == Width;

		public string Substring => str;

		public bool IgnoreCase => ignore;

		public Position Position => pos;

		public bool IsSubstring => str != null;

		public bool IsPosition => pos != System.Text.RegularExpressions.Position.Any;

		public AnchorInfo(Expression expr, int width)
		{
			this.expr = expr;
			offset = 0;
			this.width = width;
			str = null;
			ignore = false;
			pos = System.Text.RegularExpressions.Position.Any;
		}

		public AnchorInfo(Expression expr, int offset, int width, string str, bool ignore)
		{
			this.expr = expr;
			this.offset = offset;
			this.width = width;
			this.str = ((!ignore) ? str : str.ToLower());
			this.ignore = ignore;
			pos = System.Text.RegularExpressions.Position.Any;
		}

		public AnchorInfo(Expression expr, int offset, int width, Position pos)
		{
			this.expr = expr;
			this.offset = offset;
			this.width = width;
			this.pos = pos;
			str = null;
			ignore = false;
		}

		public Interval GetInterval()
		{
			return GetInterval(0);
		}

		public Interval GetInterval(int start)
		{
			if (!IsSubstring)
			{
				return Interval.Empty;
			}
			return new Interval(start + Offset, start + Offset + Length - 1);
		}
	}
}
