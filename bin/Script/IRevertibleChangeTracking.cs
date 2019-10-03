namespace System.ComponentModel
{
	/// <summary>Provides support for rolling back the changes</summary>
	public interface IRevertibleChangeTracking : IChangeTracking
	{
		/// <summary>Resets the object’s state to unchanged by rejecting the modifications.</summary>
		void RejectChanges();
	}
}
