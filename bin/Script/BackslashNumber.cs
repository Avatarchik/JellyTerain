using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class BackslashNumber : Reference
	{
		private string literal;

		private bool ecma;

		public BackslashNumber(bool ignore, bool ecma)
			: base(ignore)
		{
			this.ecma = ecma;
		}

		public bool ResolveReference(string num_str, Hashtable groups)
		{
			if (ecma)
			{
				int num = 0;
				for (int i = 1; i < num_str.Length; i++)
				{
					if (groups[num_str.Substring(0, i)] != null)
					{
						num = i;
					}
				}
				if (num != 0)
				{
					base.CapturingGroup = (CapturingGroup)groups[num_str.Substring(0, num)];
					literal = num_str.Substring(num);
					return true;
				}
			}
			else if (num_str.Length == 1)
			{
				return false;
			}
			int ptr = 0;
			int num2 = Parser.ParseOctal(num_str, ref ptr);
			if (num2 == -1)
			{
				return false;
			}
			if (num2 > 255 && ecma)
			{
				num2 /= 8;
				ptr--;
			}
			num2 &= 0xFF;
			literal = (char)num2 + num_str.Substring(ptr);
			return true;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (base.CapturingGroup != null)
			{
				base.Compile(cmp, reverse);
			}
			if (literal != null)
			{
				Literal.CompileLiteral(literal, cmp, base.IgnoreCase, reverse);
			}
		}
	}
}
