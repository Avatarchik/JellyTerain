using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class ExpressionCollection : CollectionBase
	{
		public Expression this[int i]
		{
			get
			{
				return (Expression)base.List[i];
			}
			set
			{
				base.List[i] = value;
			}
		}

		public void Add(Expression e)
		{
			base.List.Add(e);
		}

		protected override void OnValidate(object o)
		{
		}
	}
}
