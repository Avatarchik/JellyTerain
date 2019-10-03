using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Globalization.CultureInfo" /> objects to and from various other representations.</summary>
	public class CultureInfoConverter : TypeConverter
	{
		private class CultureInfoComparer : IComparer
		{
			public int Compare(object first, object second)
			{
				if (first == null)
				{
					if (second == null)
					{
						return 0;
					}
					return -1;
				}
				if (second == null)
				{
					return 1;
				}
				return string.Compare(((CultureInfo)first).DisplayName, ((CultureInfo)second).DisplayName, ignoreCase: false, CultureInfo.CurrentCulture);
			}
		}

		private StandardValuesCollection _standardValues;

		/// <summary>Gets a value indicating whether this converter can convert an object in the given source type to a <see cref="T:System.Globalization.CultureInfo" /> using the specified context.</summary>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
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
			if (destinationType == typeof(string))
			{
				return true;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified value object to a <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture to which to convert.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> specifies a culture that is not valid. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				if (string.Compare(text, "(Default)", ignoreCase: false) == 0)
				{
					return CultureInfo.InvariantCulture;
				}
				try
				{
					return new CultureInfo(text);
					IL_0031:;
				}
				catch
				{
					CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
					foreach (CultureInfo cultureInfo in cultures)
					{
						if (string.Compare(cultureInfo.DisplayName, 0, text, 0, text.Length, ignoreCase: true) == 0)
						{
							return cultureInfo;
						}
					}
				}
				throw new ArgumentException($"Culture {value} cannot be converted to a CultureInfo or is not available in this environment.");
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the given value object to the specified destination type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture to which to convert.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationType" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value != null && value is CultureInfo)
				{
					if (value == CultureInfo.InvariantCulture)
					{
						return "(Default)";
					}
					return ((CultureInfo)value).DisplayName;
				}
				return "(Default)";
			}
			if (destinationType == typeof(InstanceDescriptor) && value is CultureInfo)
			{
				CultureInfo cultureInfo = (CultureInfo)value;
				ConstructorInfo constructor = typeof(CultureInfo).GetConstructor(new Type[1]
				{
					typeof(int)
				});
				return new InstanceDescriptor(constructor, new object[1]
				{
					cultureInfo.LCID
				});
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Gets a collection of standard values for a <see cref="T:System.Globalization.CultureInfo" /> object using the specified context.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> containing a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (_standardValues == null)
			{
				CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
				Array.Sort(cultures, new CultureInfoComparer());
				CultureInfo[] array = new CultureInfo[cultures.Length + 1];
				array[0] = CultureInfo.InvariantCulture;
				Array.Copy(cultures, 0, array, 1, cultures.Length);
				_standardValues = new StandardValuesCollection(array);
			}
			return _standardValues;
		}

		/// <summary>Gets a value indicating whether the list of standard values returned from <see cref="M:System.ComponentModel.CultureInfoConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> is an exhaustive list.</summary>
		/// <returns>false because the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from <see cref="M:System.ComponentModel.CultureInfoConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> is not an exhaustive list of possible values (that is, other values are possible). This method never returns true.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>Gets a value indicating whether this object supports a standard set of values that can be picked from a list using the specified context.</summary>
		/// <returns>true because <see cref="M:System.ComponentModel.CultureInfoConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> should be called to find a common set of values the object supports. This method never returns false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
