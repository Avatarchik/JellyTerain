namespace System.IO
{
	/// <summary>Changes that might occur to a file or directory.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum WatcherChangeTypes
	{
		/// <summary>The creation, deletion, change, or renaming of a file or folder.</summary>
		All = 0xF,
		/// <summary>The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.</summary>
		Changed = 0x4,
		/// <summary>The creation of a file or folder.</summary>
		Created = 0x1,
		/// <summary>The deletion of a file or folder.</summary>
		Deleted = 0x2,
		/// <summary>The renaming of a file or folder.</summary>
		Renamed = 0x8
	}
}
