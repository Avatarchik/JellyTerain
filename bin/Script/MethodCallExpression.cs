using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
	public sealed class MethodCallExpression : Expression
	{
		private Expression obj;

		private MethodInfo method;

		private ReadOnlyCollection<Expression> arguments;

		public Expression Object => obj;

		public MethodInfo Method => method;

		public ReadOnlyCollection<Expression> Arguments => arguments;

		internal MethodCallExpression(MethodInfo method, ReadOnlyCollection<Expression> arguments)
			: base(ExpressionType.Call, method.ReturnType)
		{
			this.method = method;
			this.arguments = arguments;
		}

		internal MethodCallExpression(Expression obj, MethodInfo method, ReadOnlyCollection<Expression> arguments)
			: base(ExpressionType.Call, method.ReturnType)
		{
			this.obj = obj;
			this.method = method;
			this.arguments = arguments;
		}

		internal override void Emit(EmitContext ec)
		{
			ec.EmitCall(obj, arguments, method);
		}
	}
}
