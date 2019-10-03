namespace System.ComponentModel
{
	/// <summary>Specifies a description for a property or event.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class DescriptionAttribute : Attribute
	{
		private string desc;

		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.DescriptionAttribute" />, which is an empty string (""). This static field is read-only.</summary>
		public static readonly DescriptionAttribute Default = new DescriptionAttribute();

		/// <summary>Gets the description stored in this attribute.</summary>
		/// <returns>The description stored in this attribute.</returns>
		public virtual string Description => DescriptionValue;

		/// <summary>Gets or sets the string stored as the description.</summary>
		/// <returns>The string stored as the description. The default value is an empty string ("").</returns>
		protected string DescriptionValue
		{
			get
			{
				return desc;
			}
			set
			{
				desc = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute" /> class with no parameters.</summary>
		public DescriptionAttribute()
		{
			desc = string.Empty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DescriptionAttribute" /> class with a description.</summary>
		/// <param name="description">The description text. </param>
		public DescriptionAttribute(string name)
		{
			desc = name;
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.DescriptionAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			if (!(obj is DescriptionAttribute))
			{
				return false;
			}
			if (obj == this)
			{
				return true;
			}
			return ((DescriptionAttribute)obj).Description == desc;
		}

		public override int GetHashCode()
		{
			return desc.GetHashCode();
		}

		/// <summary>Returns a value indicating whether this is the default <see cref="T:System.ComponentModel.DescriptionAttribute" /> instance.</summary>
		/// <returns>true, if this is the default <see cref="T:System.ComponentModel.DescriptionAttribute" /> instance; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this == Default;
		}
	}
}
