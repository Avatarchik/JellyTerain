namespace System.IO
{
	[Flags]
	internal enum InotifyMask : uint
	{
		Access = 0x1,
		Modify = 0x2,
		Attrib = 0x4,
		CloseWrite = 0x8,
		CloseNoWrite = 0x10,
		Open = 0x20,
		MovedFrom = 0x40,
		MovedTo = 0x80,
		Create = 0x100,
		Delete = 0x200,
		DeleteSelf = 0x400,
		MoveSelf = 0x800,
		BaseEvents = 0xFFF,
		Umount = 0x2000,
		Overflow = 0x4000,
		Ignored = 0x8000,
		OnlyDir = 0x1000000,
		DontFollow = 0x2000000,
		AddMask = 0x20000000,
		Directory = 0x40000000,
		OneShot = 0x80000000
	}
}
