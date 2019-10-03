using System;

namespace Unity.Bindings
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
	internal class NativeConditionalAttribute : Attribute
	{
		public string Condition
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public NativeConditionalAttribute(string condition)
		{
			Condition = condition;
			Enabled = true;
		}

		public NativeConditionalAttribute(string condition, bool enabled)
		{
			Condition = condition;
			Enabled = enabled;
		}

		public NativeConditionalAttribute(bool enabled)
		{
			Condition = null;
			Enabled = enabled;
		}
	}
}
