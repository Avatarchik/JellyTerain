using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class InterpreterFactory : IMachineFactory
	{
		private IDictionary mapping;

		private ushort[] pattern;

		private string[] namesMapping;

		private int gap;

		public int GroupCount => pattern[1];

		public int Gap
		{
			get
			{
				return gap;
			}
			set
			{
				gap = value;
			}
		}

		public IDictionary Mapping
		{
			get
			{
				return mapping;
			}
			set
			{
				mapping = value;
			}
		}

		public string[] NamesMapping
		{
			get
			{
				return namesMapping;
			}
			set
			{
				namesMapping = value;
			}
		}

		public InterpreterFactory(ushort[] pattern)
		{
			this.pattern = pattern;
		}

		public IMachine NewInstance()
		{
			return new Interpreter(pattern);
		}
	}
}
