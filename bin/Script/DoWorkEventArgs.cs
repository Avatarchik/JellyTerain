namespace System.ComponentModel
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork" /> event handler.</summary>
	public class DoWorkEventArgs : EventArgs
	{
		private object arg;

		private object result;

		private bool cancel;

		/// <summary>Gets a value that represents the argument of an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the argument of an asynchronous operation.</returns>
		public object Argument => arg;

		/// <summary>Gets or sets a value that represents the result of an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the result of an asynchronous operation.</returns>
		public object Result
		{
			get
			{
				return result;
			}
			set
			{
				result = value;
			}
		}

		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DoWorkEventArgs" /> class.</summary>
		/// <param name="argument">Specifies an argument for an asynchronous operation.</param>
		public DoWorkEventArgs(object argument)
		{
			arg = argument;
		}
	}
}
