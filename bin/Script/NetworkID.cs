using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	[DefaultValue(NetworkID.Invalid)]
	public enum NetworkID : ulong
	{
		Invalid = ulong.MaxValue
	}
}
