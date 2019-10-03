using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert <see cref="T:System.DateTime" /> objects to and from various other representations.</summary>
	public class DateTimeConverter : TypeConverter
	{
		/// <summary>Gets a value indicating whether this converter can convert an object in the given source type to a <see cref="T:System.DateTime" /> using the specified context.</summary>
		/// <returns>true if this object can perform the conversion; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you wish to convert from. </param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to. </param>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to a <see cref="T:System.DateTime" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a valid value for the target type. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = (string)value;
				try
				{
					if (text != null && text.Trim().Length == 0)
					{
						return DateTime.MinValue;
					}
					if (culture == null)
					{
						return DateTime.Parse(text);
					}
					DateTimeFormatInfo provider = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
					return DateTime.Parse(text, provider);
					IL_0077:;
				}
				catch
				{
					throw new FormatException(text + " is not a valid DateTime value.");
					IL_008e:;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the given value object to a <see cref="T:System.DateTime" /> using the arguments.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to. </param>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is DateTime)
			{
				DateTime d = (DateTime)value;
				if (destinationType == typeof(string))
				{
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					if (d == DateTime.MinValue)
					{
						return string.Empty;
					}
					DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
					if (culture == CultureInfo.InvariantCulture)
					{
						if (d.Equals(d.Date))
						{
							return d.ToString("yyyy-MM-dd", culture);
						}
						return d.ToString(culture);
					}
					if (d == d.Date)
					{
						return d.ToString(dateTimeFormatInfo.ShortDatePattern, culture);
					}
					return d.ToString(dateTimeFormatInfo.ShortDatePattern + " " + dateTimeFormatInfo.ShortTimePattern, culture);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(DateTime).GetConstructor(new Type[1]
					{
						typeof(long)
					});
					return new InstanceDescriptor(constructor, new object[1]
					{
						d.Ticks
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
