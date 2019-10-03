using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 64-bit unsigned integer objects to and from other representations.</summary>
	public class UInt64Converter : BaseNumberConverter
	{
		internal override bool SupportHex => true;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.UInt64Converter" /> class. </summary>
		public UInt64Converter()
		{
			InnerType = typeof(ulong);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((ulong)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return ulong.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToUInt64(value, fromBase);
		}
	}
}
