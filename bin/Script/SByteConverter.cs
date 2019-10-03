using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 8-bit unsigned integer objects to and from a string.</summary>
	public class SByteConverter : BaseNumberConverter
	{
		internal override bool SupportHex => true;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.SByteConverter" /> class. </summary>
		public SByteConverter()
		{
			InnerType = typeof(sbyte);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((sbyte)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return sbyte.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToSByte(value, fromBase);
		}
	}
}
