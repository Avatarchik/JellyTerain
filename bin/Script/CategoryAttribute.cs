namespace System.ComponentModel
{
	/// <summary>Specifies the name of the category in which to group the property or event when displayed in a <see cref="T:System.Windows.Forms.PropertyGrid" /> control set to Categorized mode.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class CategoryAttribute : Attribute
	{
		private string category;

		private bool IsLocalized;

		private static volatile CategoryAttribute action;

		private static volatile CategoryAttribute appearance;

		private static volatile CategoryAttribute behaviour;

		private static volatile CategoryAttribute data;

		private static volatile CategoryAttribute def;

		private static volatile CategoryAttribute design;

		private static volatile CategoryAttribute drag_drop;

		private static volatile CategoryAttribute focus;

		private static volatile CategoryAttribute format;

		private static volatile CategoryAttribute key;

		private static volatile CategoryAttribute layout;

		private static volatile CategoryAttribute mouse;

		private static volatile CategoryAttribute window_style;

		private static volatile CategoryAttribute async;

		private static object lockobj = new object();

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Action category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the action category.</returns>
		public static CategoryAttribute Action
		{
			get
			{
				if (action != null)
				{
					return action;
				}
				lock (lockobj)
				{
					if (action == null)
					{
						action = new CategoryAttribute("Action");
					}
				}
				return action;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Appearance category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the appearance category.</returns>
		public static CategoryAttribute Appearance
		{
			get
			{
				if (appearance != null)
				{
					return appearance;
				}
				lock (lockobj)
				{
					if (appearance == null)
					{
						appearance = new CategoryAttribute("Appearance");
					}
				}
				return appearance;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Asynchronous category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the asynchronous category.</returns>
		public static CategoryAttribute Asynchronous
		{
			get
			{
				if (behaviour != null)
				{
					return behaviour;
				}
				lock (lockobj)
				{
					if (async == null)
					{
						async = new CategoryAttribute("Asynchronous");
					}
				}
				return async;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Behavior category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the behavior category.</returns>
		public static CategoryAttribute Behavior
		{
			get
			{
				if (behaviour != null)
				{
					return behaviour;
				}
				lock (lockobj)
				{
					if (behaviour == null)
					{
						behaviour = new CategoryAttribute("Behavior");
					}
				}
				return behaviour;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Data category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the data category.</returns>
		public static CategoryAttribute Data
		{
			get
			{
				if (data != null)
				{
					return data;
				}
				lock (lockobj)
				{
					if (data == null)
					{
						data = new CategoryAttribute("Data");
					}
				}
				return data;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Default category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the default category.</returns>
		public static CategoryAttribute Default
		{
			get
			{
				if (def != null)
				{
					return def;
				}
				lock (lockobj)
				{
					if (def == null)
					{
						def = new CategoryAttribute("Default");
					}
				}
				return def;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Design category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the design category.</returns>
		public static CategoryAttribute Design
		{
			get
			{
				if (design != null)
				{
					return design;
				}
				lock (lockobj)
				{
					if (design == null)
					{
						design = new CategoryAttribute("Design");
					}
				}
				return design;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the DragDrop category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the drag-and-drop category.</returns>
		public static CategoryAttribute DragDrop
		{
			get
			{
				if (drag_drop != null)
				{
					return drag_drop;
				}
				lock (lockobj)
				{
					if (drag_drop == null)
					{
						drag_drop = new CategoryAttribute("DragDrop");
					}
				}
				return drag_drop;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Focus category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the focus category.</returns>
		public static CategoryAttribute Focus
		{
			get
			{
				if (focus != null)
				{
					return focus;
				}
				lock (lockobj)
				{
					if (focus == null)
					{
						focus = new CategoryAttribute("Focus");
					}
				}
				return focus;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Format category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the format category.</returns>
		public static CategoryAttribute Format
		{
			get
			{
				if (format != null)
				{
					return format;
				}
				lock (lockobj)
				{
					if (format == null)
					{
						format = new CategoryAttribute("Format");
					}
				}
				return format;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Key category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the key category.</returns>
		public static CategoryAttribute Key
		{
			get
			{
				if (key != null)
				{
					return key;
				}
				lock (lockobj)
				{
					if (key == null)
					{
						key = new CategoryAttribute("Key");
					}
				}
				return key;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Layout category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the layout category.</returns>
		public static CategoryAttribute Layout
		{
			get
			{
				if (layout != null)
				{
					return layout;
				}
				lock (lockobj)
				{
					if (layout == null)
					{
						layout = new CategoryAttribute("Layout");
					}
				}
				return layout;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Mouse category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the mouse category.</returns>
		public static CategoryAttribute Mouse
		{
			get
			{
				if (mouse != null)
				{
					return mouse;
				}
				lock (lockobj)
				{
					if (mouse == null)
					{
						mouse = new CategoryAttribute("Mouse");
					}
				}
				return mouse;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the WindowStyle category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the window style category.</returns>
		public static CategoryAttribute WindowStyle
		{
			get
			{
				if (window_style != null)
				{
					return window_style;
				}
				lock (lockobj)
				{
					if (window_style == null)
					{
						window_style = new CategoryAttribute("WindowStyle");
					}
				}
				return window_style;
			}
		}

		/// <summary>Gets the name of the category for the property or event that this attribute is applied to.</summary>
		/// <returns>The name of the category for the property or event that this attribute is applied to.</returns>
		public string Category
		{
			get
			{
				if (!IsLocalized)
				{
					IsLocalized = true;
					string localizedString = GetLocalizedString(category);
					if (localizedString != null)
					{
						category = localizedString;
					}
				}
				return category;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CategoryAttribute" /> class using the category name Default.</summary>
		public CategoryAttribute()
		{
			category = "Misc";
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CategoryAttribute" /> class using the specified category name.</summary>
		/// <param name="category">The name of the category. </param>
		public CategoryAttribute(string category)
		{
			this.category = category;
		}

		/// <summary>Looks up the localized name of the specified category.</summary>
		/// <returns>The localized name of the category, or null if a localized name does not exist.</returns>
		/// <param name="value">The identifer for the category to look up. </param>
		protected virtual string GetLocalizedString(string value)
		{
			return Locale.GetText(value);
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.CategoryAttribute" />..</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			if (!(obj is CategoryAttribute))
			{
				return false;
			}
			if (obj == this)
			{
				return true;
			}
			return ((CategoryAttribute)obj).Category == category;
		}

		/// <summary>Returns the hash code for this attribute.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return category.GetHashCode();
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override bool IsDefaultAttribute()
		{
			return category == Default.Category;
		}
	}
}
