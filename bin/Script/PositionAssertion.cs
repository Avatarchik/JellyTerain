namespace System.Text.RegularExpressions.Syntax
{
	internal class PositionAssertion : Expression
	{
		private Position pos;

		public Position Position
		{
			get
			{
				return pos;
			}
			set
			{
				pos = value;
			}
		}

		public PositionAssertion(Position pos)
		{
			this.pos = pos;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			cmp.EmitPosition(pos);
		}

		public override void GetWidth(out int min, out int max)
		{
			min = (max = 0);
		}

		public override bool IsComplex()
		{
			return false;
		}

		public override AnchorInfo GetAnchorInfo(bool revers)
		{
			switch (pos)
			{
			case System.Text.RegularExpressions.Position.StartOfString:
			case System.Text.RegularExpressions.Position.StartOfLine:
			case System.Text.RegularExpressions.Position.StartOfScan:
				return new AnchorInfo(this, 0, 0, pos);
			default:
				return new AnchorInfo(this, 0);
			}
		}
	}
}
