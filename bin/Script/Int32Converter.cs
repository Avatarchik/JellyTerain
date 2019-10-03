using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 32-bit signed integer objects to and from other representations.</summary>
	public class Int32Converter : BaseNumberConverter
	{
		internal override bool SupportHex => true;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Int32Converter" /> class. </summary>
		public Int32Converter()
		{
			InnerType = typeof(int);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((int)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return int.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToInt32(value, fromBase);
		}
	}
}
