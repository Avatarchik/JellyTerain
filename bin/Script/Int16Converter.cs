using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 16-bit signed integer objects to and from other representations.</summary>
	public class Int16Converter : BaseNumberConverter
	{
		internal override bool SupportHex => true;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Int16Converter" /> class. </summary>
		public Int16Converter()
		{
			InnerType = typeof(short);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((short)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return short.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToInt16(value, fromBase);
		}
	}
}
