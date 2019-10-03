namespace System.Text.RegularExpressions.Syntax
{
	internal abstract class Expression
	{
		public abstract void Compile(ICompiler cmp, bool reverse);

		public abstract void GetWidth(out int min, out int max);

		public int GetFixedWidth()
		{
			GetWidth(out int min, out int max);
			if (min == max)
			{
				return min;
			}
			return -1;
		}

		public virtual AnchorInfo GetAnchorInfo(bool reverse)
		{
			return new AnchorInfo(this, GetFixedWidth());
		}

		public abstract bool IsComplex();
	}
}
