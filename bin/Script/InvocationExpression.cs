using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
	public sealed class InvocationExpression : Expression
	{
		private Expression expression;

		private ReadOnlyCollection<Expression> arguments;

		public Expression Expression => expression;

		public ReadOnlyCollection<Expression> Arguments => arguments;

		internal InvocationExpression(Expression expression, Type type, ReadOnlyCollection<Expression> arguments)
			: base(ExpressionType.Invoke, type)
		{
			this.expression = expression;
			this.arguments = arguments;
		}

		internal override void Emit(EmitContext ec)
		{
			ec.EmitCall(expression, arguments, expression.Type.GetInvokeMethod());
		}
	}
}
