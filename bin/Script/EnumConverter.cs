using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Enum" /> objects to and from various other representations.</summary>
	public class EnumConverter : TypeConverter
	{
		private class EnumComparer : IComparer
		{
			int IComparer.Compare(object compareObject1, object compareObject2)
			{
				string text = compareObject1 as string;
				string text2 = compareObject2 as string;
				if (text == null || text2 == null)
				{
					return System.Collections.Comparer.Default.Compare(compareObject1, compareObject2);
				}
				return CultureInfo.InvariantCulture.CompareInfo.Compare(text, text2);
			}
		}

		private Type type;

		private StandardValuesCollection stdValues;

		/// <summary>Gets an <see cref="T:System.Collections.IComparer" /> that can be used to sort the values of the enumeration.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> for sorting the enumeration values.</returns>
		protected virtual IComparer Comparer => new EnumComparer();

		/// <summary>Specifies the type of the enumerator this converter is associated with.</summary>
		/// <returns>The type of the enumerator this converter is associated with.</returns>
		protected Type EnumType => type;

		/// <summary>Gets or sets a <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that specifies the possible values for the enumeration.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that specifies the possible values for the enumeration.</returns>
		protected StandardValuesCollection Values
		{
			get
			{
				return stdValues;
			}
			set
			{
				stdValues = value;
			}
		}

		private bool IsFlags => type.IsDefined(typeof(FlagsAttribute), inherit: false);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EnumConverter" /> class for the given type.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of enumeration to associate with this enumeration converter. </param>
		public EnumConverter(Type type)
		{
			this.type = type;
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
			if (destinationType == typeof(Enum[]))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to the specified destination type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a valid value for the enumeration. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value != null)
			{
				if (value is IConvertible)
				{
					Type underlyingType = Enum.GetUnderlyingType(type);
					if (underlyingType != value.GetType())
					{
						value = ((IConvertible)value).ToType(underlyingType, culture);
					}
				}
				if (!IsFlags && !IsValid(context, value))
				{
					throw CreateValueNotValidException(value);
				}
				return Enum.Format(type, value, "G");
			}
			if (destinationType == typeof(InstanceDescriptor) && value != null)
			{
				string text = ConvertToString(context, culture, value);
				if (IsFlags && text.IndexOf(",") != -1)
				{
					if (value is IConvertible)
					{
						Type underlyingType2 = Enum.GetUnderlyingType(type);
						object obj = ((IConvertible)value).ToType(underlyingType2, culture);
						MethodInfo method = typeof(Enum).GetMethod("ToObject", new Type[2]
						{
							typeof(Type),
							underlyingType2
						});
						return new InstanceDescriptor(method, new object[2]
						{
							type,
							obj
						});
					}
				}
				else
				{
					FieldInfo field = type.GetField(text);
					if (field != null)
					{
						return new InstanceDescriptor(field, null);
					}
				}
			}
			else if (destinationType == typeof(Enum[]) && value != null)
			{
				if (!IsFlags)
				{
					return new Enum[1]
					{
						(Enum)Enum.ToObject(type, value)
					};
				}
				long num = Convert.ToInt64((Enum)value, culture);
				Array values = Enum.GetValues(type);
				long[] array = new long[values.Length];
				for (int i = 0; i < values.Length; i++)
				{
					array[i] = Convert.ToInt64(values.GetValue(i));
				}
				ArrayList arrayList = new ArrayList();
				bool flag = false;
				while (!flag)
				{
					flag = true;
					long[] array2 = array;
					foreach (long num2 in array2)
					{
						if ((num2 != 0L && (num2 & num) == num2) || num2 == num)
						{
							arrayList.Add(Enum.ToObject(type, num2));
							num &= ~num2;
							flag = false;
						}
					}
					if (num == 0L)
					{
						flag = true;
					}
				}
				if (num != 0L)
				{
					arrayList.Add(Enum.ToObject(type, num));
				}
				return arrayList.ToArray(typeof(Enum));
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object in the given source type to an enumeration object using the specified context.</summary>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you wish to convert from. </param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			if (sourceType == typeof(Enum[]))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Converts the specified value object to an enumeration object.</summary>
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
				string text = value as string;
				try
				{
					if (text.IndexOf(',') == -1)
					{
						return Enum.Parse(type, text, ignoreCase: true);
					}
					long num = 0L;
					string[] array = text.Split(',');
					string[] array2 = array;
					foreach (string value2 in array2)
					{
						Enum value3 = (Enum)Enum.Parse(type, value2, ignoreCase: true);
						num |= Convert.ToInt64(value3, culture);
					}
					return Enum.ToObject(type, num);
					IL_009d:;
				}
				catch (Exception innerException)
				{
					throw new FormatException(text + " is not a valid value for " + type.Name, innerException);
					IL_00c2:;
				}
			}
			if (value is Enum[])
			{
				long num2 = 0L;
				Enum[] array3 = (Enum[])value;
				foreach (Enum value4 in array3)
				{
					num2 |= Convert.ToInt64(value4, culture);
				}
				return Enum.ToObject(type, num2);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Gets a value indicating whether the given object value is valid for this type.</summary>
		/// <returns>true if the specified value is valid for this object; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to test. </param>
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return Enum.IsDefined(type, value);
		}

		/// <summary>Gets a value indicating whether this object supports a standard set of values that can be picked from a list using the specified context.</summary>
		/// <returns>true because <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a common set of values the object supports. This method never returns false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Gets a value indicating whether the list of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exclusive list using the specified context.</summary>
		/// <returns>true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exhaustive list of possible values; false if other values are possible.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return !IsFlags;
		}

		/// <summary>Gets a collection of standard values for the data type this validator is designed for.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (stdValues == null)
			{
				Array values = Enum.GetValues(type);
				Array.Sort(values);
				stdValues = new StandardValuesCollection(values);
			}
			return stdValues;
		}

		private ArgumentException CreateValueNotValidException(object value)
		{
			string message = string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not a valid value for the enum '{1}'", value, type.Name);
			return new ArgumentException(message);
		}
	}
}
