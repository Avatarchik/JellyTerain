namespace System.Text.RegularExpressions.Syntax
{
	internal class BalancingGroup : CapturingGroup
	{
		private CapturingGroup balance;

		public CapturingGroup Balance
		{
			get
			{
				return balance;
			}
			set
			{
				balance = value;
			}
		}

		public BalancingGroup()
		{
			balance = null;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			cmp.EmitBalanceStart(base.Index, balance.Index, base.IsNamed, linkRef);
			int count = base.Expressions.Count;
			for (int i = 0; i < count; i++)
			{
				Expression expression = (!reverse) ? base.Expressions[i] : base.Expressions[count - i - 1];
				expression.Compile(cmp, reverse);
			}
			cmp.EmitBalance();
			cmp.ResolveLink(linkRef);
		}
	}
}
