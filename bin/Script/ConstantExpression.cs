using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	public sealed class ConstantExpression : Expression
	{
		private object value;

		public object Value => value;

		internal ConstantExpression(object value, Type type)
			: base(ExpressionType.Constant, type)
		{
			this.value = value;
		}

		internal override void Emit(EmitContext ec)
		{
			if (base.Type.IsNullable())
			{
				EmitNullableConstant(ec, base.Type, value);
			}
			else
			{
				EmitConstant(ec, base.Type, value);
			}
		}

		private void EmitNullableConstant(EmitContext ec, Type type, object value)
		{
			if (value == null)
			{
				ILGenerator ig = ec.ig;
				LocalBuilder local = ig.DeclareLocal(type);
				ig.Emit(OpCodes.Ldloca, local);
				ig.Emit(OpCodes.Initobj, type);
				ig.Emit(OpCodes.Ldloc, local);
			}
			else
			{
				EmitConstant(ec, type.GetFirstGenericArgument(), value);
				ec.EmitNullableNew(type);
			}
		}

		private void EmitConstant(EmitContext ec, Type type, object value)
		{
			ILGenerator ig = ec.ig;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Byte:
				ig.Emit(OpCodes.Ldc_I4, (int)(byte)value);
				break;
			case TypeCode.SByte:
				ig.Emit(OpCodes.Ldc_I4, (int)(sbyte)value);
				break;
			case TypeCode.Int16:
				ig.Emit(OpCodes.Ldc_I4, (int)(short)value);
				break;
			case TypeCode.UInt16:
				ig.Emit(OpCodes.Ldc_I4, (ushort)value);
				break;
			case TypeCode.Int32:
				ig.Emit(OpCodes.Ldc_I4, (int)value);
				break;
			case TypeCode.UInt32:
				ig.Emit(OpCodes.Ldc_I4, (int)(uint)Value);
				break;
			case TypeCode.Int64:
				ig.Emit(OpCodes.Ldc_I8, (long)value);
				break;
			case TypeCode.UInt64:
				ig.Emit(OpCodes.Ldc_I8, (long)(ulong)value);
				break;
			case TypeCode.Boolean:
				if ((bool)Value)
				{
					ig.Emit(OpCodes.Ldc_I4_1);
				}
				else
				{
					ec.ig.Emit(OpCodes.Ldc_I4_0);
				}
				break;
			case TypeCode.Char:
				ig.Emit(OpCodes.Ldc_I4, (char)value);
				break;
			case TypeCode.Single:
				ig.Emit(OpCodes.Ldc_R4, (float)value);
				break;
			case TypeCode.Double:
				ig.Emit(OpCodes.Ldc_R8, (double)value);
				break;
			case TypeCode.Decimal:
			{
				decimal num = (decimal)value;
				int[] bits = decimal.GetBits(num);
				int num2 = (bits[3] >> 16) & 0xFF;
				Type typeFromHandle = typeof(int);
				if (num2 == 0 && num <= 2147483647m && num >= -2147483648m)
				{
					ig.Emit(OpCodes.Ldc_I4, (int)num);
					ig.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[1]
					{
						typeFromHandle
					}));
					break;
				}
				ig.Emit(OpCodes.Ldc_I4, bits[0]);
				ig.Emit(OpCodes.Ldc_I4, bits[1]);
				ig.Emit(OpCodes.Ldc_I4, bits[2]);
				ig.Emit(OpCodes.Ldc_I4, bits[3] >> 31);
				ig.Emit(OpCodes.Ldc_I4, num2);
				ig.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[5]
				{
					typeFromHandle,
					typeFromHandle,
					typeFromHandle,
					typeof(bool),
					typeof(byte)
				}));
				break;
			}
			case TypeCode.DateTime:
			{
				DateTime dateTime = (DateTime)value;
				LocalBuilder local = ig.DeclareLocal(typeof(DateTime));
				ig.Emit(OpCodes.Ldloca, local);
				ig.Emit(OpCodes.Ldc_I8, dateTime.Ticks);
				ig.Emit(OpCodes.Ldc_I4, (int)dateTime.Kind);
				ig.Emit(OpCodes.Call, typeof(DateTime).GetConstructor(new Type[2]
				{
					typeof(long),
					typeof(DateTimeKind)
				}));
				ig.Emit(OpCodes.Ldloc, local);
				break;
			}
			case TypeCode.DBNull:
				ig.Emit(OpCodes.Ldsfld, typeof(DBNull).GetField("Value", BindingFlags.Static | BindingFlags.Public));
				break;
			case TypeCode.String:
				EmitIfNotNull(ec, delegate(EmitContext c)
				{
					c.ig.Emit(OpCodes.Ldstr, (string)value);
				});
				break;
			case TypeCode.Object:
				EmitIfNotNull(ec, delegate(EmitContext c)
				{
					c.EmitReadGlobal(value);
				});
				break;
			default:
				throw new NotImplementedException($"No support for constants of type {base.Type} yet");
			}
		}

		private void EmitIfNotNull(EmitContext ec, Action<EmitContext> emit)
		{
			if (value == null)
			{
				ec.ig.Emit(OpCodes.Ldnull);
			}
			else
			{
				emit(ec);
			}
		}
	}
}
