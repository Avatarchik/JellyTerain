using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	[DefaultValue(AppID.Invalid)]
	public enum AppID : ulong
	{
		Invalid = ulong.MaxValue
	}
}
