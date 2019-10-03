namespace System.Text.RegularExpressions.Syntax
{
	internal class CaptureAssertion : Assertion
	{
		private ExpressionAssertion alternate;

		private CapturingGroup group;

		private Literal literal;

		public CapturingGroup CapturingGroup
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		private ExpressionAssertion Alternate
		{
			get
			{
				if (alternate == null)
				{
					alternate = new ExpressionAssertion();
					alternate.TrueExpression = base.TrueExpression;
					alternate.FalseExpression = base.FalseExpression;
					alternate.TestExpression = literal;
				}
				return alternate;
			}
		}

		public CaptureAssertion(Literal l)
		{
			literal = l;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (group == null)
			{
				Alternate.Compile(cmp, reverse);
				return;
			}
			int index = group.Index;
			LinkRef linkRef = cmp.NewLink();
			if (base.FalseExpression == null)
			{
				cmp.EmitIfDefined(index, linkRef);
				base.TrueExpression.Compile(cmp, reverse);
			}
			else
			{
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitIfDefined(index, linkRef2);
				base.TrueExpression.Compile(cmp, reverse);
				cmp.EmitJump(linkRef);
				cmp.ResolveLink(linkRef2);
				base.FalseExpression.Compile(cmp, reverse);
			}
			cmp.ResolveLink(linkRef);
		}

		public override bool IsComplex()
		{
			if (group == null)
			{
				return Alternate.IsComplex();
			}
			if (base.TrueExpression != null && base.TrueExpression.IsComplex())
			{
				return true;
			}
			if (base.FalseExpression != null && base.FalseExpression.IsComplex())
			{
				return true;
			}
			return GetFixedWidth() <= 0;
		}
	}
}
