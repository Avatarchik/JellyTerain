namespace System.Text.RegularExpressions.Syntax
{
	internal class Alternation : CompositeExpression
	{
		public ExpressionCollection Alternatives => base.Expressions;

		public void AddAlternative(Expression e)
		{
			Alternatives.Add(e);
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			foreach (Expression alternative in Alternatives)
			{
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitBranch(linkRef2);
				alternative.Compile(cmp, reverse);
				cmp.EmitJump(linkRef);
				cmp.ResolveLink(linkRef2);
				cmp.EmitBranchEnd();
			}
			cmp.EmitFalse();
			cmp.ResolveLink(linkRef);
			cmp.EmitAlternationEnd();
		}

		public override void GetWidth(out int min, out int max)
		{
			GetWidth(out min, out max, Alternatives.Count);
		}
	}
}
