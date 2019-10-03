using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 8-bit unsigned integer objects to and from various other representations.</summary>
	public class ByteConverter : BaseNumberConverter
	{
		internal override bool SupportHex => true;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ByteConverter" /> class. </summary>
		public ByteConverter()
		{
			InnerType = typeof(byte);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((byte)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return byte.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToByte(value, fromBase);
		}
	}
}
