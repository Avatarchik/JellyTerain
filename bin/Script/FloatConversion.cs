namespace UnityEngine.Networking
{
	internal class FloatConversion
	{
		public static float ToSingle(uint value)
		{
			UIntFloat uIntFloat = default(UIntFloat);
			uIntFloat.intValue = value;
			return uIntFloat.floatValue;
		}

		public static double ToDouble(ulong value)
		{
			UIntFloat uIntFloat = default(UIntFloat);
			uIntFloat.longValue = value;
			return uIntFloat.doubleValue;
		}

		public static decimal ToDecimal(ulong value1, ulong value2)
		{
			UIntDecimal uIntDecimal = default(UIntDecimal);
			uIntDecimal.longValue1 = value1;
			uIntDecimal.longValue2 = value2;
			return uIntDecimal.decimalValue;
		}
	}
}
