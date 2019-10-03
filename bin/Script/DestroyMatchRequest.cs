using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	internal class DestroyMatchRequest : Request
	{
		public NetworkID networkId
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-networkId:0x{1}", base.ToString(), networkId.ToString("X"));
		}

		public override bool IsValid()
		{
			return base.IsValid() && networkId != NetworkID.Invalid;
		}
	}
}
