using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class MatchInfoSnapshot
	{
		public class MatchInfoDirectConnectSnapshot
		{
			public NodeID nodeId
			{
				get;
				private set;
			}

			public string publicAddress
			{
				get;
				private set;
			}

			public string privateAddress
			{
				get;
				private set;
			}

			public HostPriority hostPriority
			{
				get;
				private set;
			}

			public MatchInfoDirectConnectSnapshot()
			{
			}

			internal MatchInfoDirectConnectSnapshot(MatchDirectConnectInfo matchDirectConnectInfo)
			{
				nodeId = matchDirectConnectInfo.nodeId;
				publicAddress = matchDirectConnectInfo.publicAddress;
				privateAddress = matchDirectConnectInfo.privateAddress;
				hostPriority = matchDirectConnectInfo.hostPriority;
			}
		}

		public NetworkID networkId
		{
			get;
			private set;
		}

		public NodeID hostNodeId
		{
			get;
			private set;
		}

		public string name
		{
			get;
			private set;
		}

		public int averageEloScore
		{
			get;
			private set;
		}

		public int maxSize
		{
			get;
			private set;
		}

		public int currentSize
		{
			get;
			private set;
		}

		public bool isPrivate
		{
			get;
			private set;
		}

		public Dictionary<string, long> matchAttributes
		{
			get;
			private set;
		}

		public List<MatchInfoDirectConnectSnapshot> directConnectInfos
		{
			get;
			private set;
		}

		public MatchInfoSnapshot()
		{
		}

		internal MatchInfoSnapshot(MatchDesc matchDesc)
		{
			networkId = matchDesc.networkId;
			hostNodeId = matchDesc.hostNodeId;
			name = matchDesc.name;
			averageEloScore = matchDesc.averageEloScore;
			maxSize = matchDesc.maxSize;
			currentSize = matchDesc.currentSize;
			isPrivate = matchDesc.isPrivate;
			matchAttributes = matchDesc.matchAttributes;
			directConnectInfos = new List<MatchInfoDirectConnectSnapshot>();
			foreach (MatchDirectConnectInfo directConnectInfo in matchDesc.directConnectInfos)
			{
				directConnectInfos.Add(new MatchInfoDirectConnectSnapshot(directConnectInfo));
			}
		}
	}
}
