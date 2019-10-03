namespace System.ComponentModel
{
	/// <summary>Allows coordination of initialization for a component and its dependent properties.</summary>
	public interface ISupportInitializeNotification : ISupportInitialize
	{
		/// <summary>Gets a value indicating whether the component is initialized.</summary>
		/// <returns>true to indicate the component has completed initialization; otherwise, false. </returns>
		bool IsInitialized
		{
			get;
		}

		/// <summary>Occurs when initialization of the component is completed.</summary>
		event EventHandler Initialized;
	}
}
