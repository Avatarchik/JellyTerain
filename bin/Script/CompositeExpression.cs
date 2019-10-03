namespace System.Text.RegularExpressions.Syntax
{
	internal abstract class CompositeExpression : Expression
	{
		private ExpressionCollection expressions;

		protected ExpressionCollection Expressions => expressions;

		public CompositeExpression()
		{
			expressions = new ExpressionCollection();
		}

		protected void GetWidth(out int min, out int max, int count)
		{
			min = int.MaxValue;
			max = 0;
			bool flag = true;
			for (int i = 0; i < count; i++)
			{
				Expression expression = Expressions[i];
				if (expression != null)
				{
					flag = false;
					expression.GetWidth(out int min2, out int max2);
					if (min2 < min)
					{
						min = min2;
					}
					if (max2 > max)
					{
						max = max2;
					}
				}
			}
			if (flag)
			{
				min = (max = 0);
			}
		}

		public override bool IsComplex()
		{
			foreach (Expression expression in Expressions)
			{
				if (expression.IsComplex())
				{
					return true;
				}
			}
			return GetFixedWidth() <= 0;
		}
	}
}
