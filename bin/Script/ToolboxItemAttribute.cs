namespace System.ComponentModel
{
	/// <summary>Represents an attribute of a toolbox item.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class ToolboxItemAttribute : Attribute
	{
		private const string defaultItemType = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and sets the type to the default, <see cref="T:System.Drawing.Design.ToolboxItem" />. This field is read-only.</summary>
		public static readonly ToolboxItemAttribute Default = new ToolboxItemAttribute("System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and sets the type to null. This field is read-only.</summary>
		public static readonly ToolboxItemAttribute None = new ToolboxItemAttribute(defaultType: false);

		private Type itemType;

		private string itemTypeName;

		/// <summary>Gets or sets the type of the toolbox item.</summary>
		/// <returns>The type of the toolbox item.</returns>
		/// <exception cref="T:System.ArgumentException">The type cannot be found. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Type ToolboxItemType
		{
			get
			{
				if (itemType == null && itemTypeName != null)
				{
					try
					{
						itemType = Type.GetType(itemTypeName, throwOnError: true);
					}
					catch (Exception innerException)
					{
						throw new ArgumentException("Failed to create ToolboxItem of type: " + itemTypeName, innerException);
						IL_0045:;
					}
				}
				return itemType;
			}
		}

		/// <summary>Gets or sets the name of the type of the current <see cref="T:System.Drawing.Design.ToolboxItem" />.</summary>
		/// <returns>The fully qualified type name of the current toolbox item.</returns>
		public string ToolboxItemTypeName
		{
			get
			{
				if (itemTypeName == null)
				{
					if (itemType == null)
					{
						return string.Empty;
					}
					itemTypeName = itemType.AssemblyQualifiedName;
				}
				return itemTypeName;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and specifies whether to use default initialization values.</summary>
		/// <param name="defaultType">true to create a toolbox item attribute for a default type; false to associate no default toolbox item support for this attribute. </param>
		public ToolboxItemAttribute(bool defaultType)
		{
			if (defaultType)
			{
				itemTypeName = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class using the specified name of the type.</summary>
		/// <param name="toolboxItemTypeName">The names of the type of the toolbox item and of the assembly that contains the type. </param>
		public ToolboxItemAttribute(string toolboxItemName)
		{
			itemTypeName = toolboxItemName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class using the specified type of the toolbox item.</summary>
		/// <param name="toolboxItemType">The type of the toolbox item. </param>
		public ToolboxItemAttribute(Type toolboxItemType)
		{
			itemType = toolboxItemType;
		}

		/// <param name="obj">The object to compare.</param>
		public override bool Equals(object o)
		{
			ToolboxItemAttribute toolboxItemAttribute = o as ToolboxItemAttribute;
			if (toolboxItemAttribute == null)
			{
				return false;
			}
			return toolboxItemAttribute.ToolboxItemTypeName == ToolboxItemTypeName;
		}

		public override int GetHashCode()
		{
			if (itemTypeName != null)
			{
				return itemTypeName.GetHashCode();
			}
			return base.GetHashCode();
		}

		/// <summary>Gets a value indicating whether the current value of the attribute is the default value for the attribute.</summary>
		/// <returns>true if the current value of the attribute is the default; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return Equals(Default);
		}
	}
}
