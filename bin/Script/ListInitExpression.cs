using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	public sealed class ListInitExpression : Expression
	{
		private NewExpression new_expression;

		private ReadOnlyCollection<ElementInit> initializers;

		public NewExpression NewExpression => new_expression;

		public ReadOnlyCollection<ElementInit> Initializers => initializers;

		internal ListInitExpression(NewExpression new_expression, ReadOnlyCollection<ElementInit> initializers)
			: base(ExpressionType.ListInit, new_expression.Type)
		{
			this.new_expression = new_expression;
			this.initializers = initializers;
		}

		internal override void Emit(EmitContext ec)
		{
			LocalBuilder local = ec.EmitStored(new_expression);
			ec.EmitCollection(initializers, local);
			ec.EmitLoad(local);
		}
	}
}
