using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	[DefaultValue(NetworkAccessLevel.Invalid)]
	public enum NetworkAccessLevel : ulong
	{
		Invalid = 0uL,
		User = 1uL,
		Owner = 2uL,
		Admin = 4uL
	}
}
