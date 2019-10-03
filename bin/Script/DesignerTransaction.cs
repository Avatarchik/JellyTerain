namespace System.ComponentModel.Design
{
	/// <summary>Provides a way to group a series of design-time actions to improve performance and enable most types of changes to be undone.</summary>
	public abstract class DesignerTransaction : IDisposable
	{
		private string description;

		private bool committed;

		private bool canceled;

		/// <summary>Gets a value indicating whether the transaction was canceled.</summary>
		/// <returns>true if the transaction was canceled; otherwise, false.</returns>
		public bool Canceled => canceled;

		/// <summary>Gets a value indicating whether the transaction was committed.</summary>
		/// <returns>true if the transaction was committed; otherwise, false.</returns>
		public bool Committed => committed;

		/// <summary>Gets a description for the transaction.</summary>
		/// <returns>A description for the transaction.</returns>
		public string Description => description;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> class with no description.</summary>
		protected DesignerTransaction()
			: this(string.Empty)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> class using the specified transaction description.</summary>
		/// <param name="description">A description for this transaction. </param>
		protected DesignerTransaction(string description)
		{
			this.description = description;
			committed = false;
			canceled = false;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.ComponentModel.Design.DesignerTransaction" />. </summary>
		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			Cancel();
			if (disposing)
			{
				GC.SuppressFinalize(true);
			}
		}

		/// <summary>Raises the Cancel event.</summary>
		protected abstract void OnCancel();

		/// <summary>Performs the actual work of committing a transaction.</summary>
		protected abstract void OnCommit();

		/// <summary>Cancels the transaction and attempts to roll back the changes made by the events of the transaction.</summary>
		public void Cancel()
		{
			if (!Canceled && !Committed)
			{
				canceled = true;
				OnCancel();
			}
		}

		/// <summary>Commits this transaction.</summary>
		public void Commit()
		{
			if (!Canceled && !Committed)
			{
				committed = true;
				OnCommit();
			}
		}

		/// <summary>Releases the resources associated with this object. This override commits this transaction if it was not already committed.</summary>
		~DesignerTransaction()
		{
			Dispose(disposing: false);
		}
	}
}
