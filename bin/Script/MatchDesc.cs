using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	internal class MatchDesc : ResponseBase
	{
		public NetworkID networkId
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public int averageEloScore
		{
			get;
			set;
		}

		public int maxSize
		{
			get;
			set;
		}

		public int currentSize
		{
			get;
			set;
		}

		public bool isPrivate
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributes
		{
			get;
			set;
		}

		public NodeID hostNodeId
		{
			get;
			set;
		}

		public List<MatchDirectConnectInfo> directConnectInfos
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-networkId:0x{1},name:{2},averageEloScore:{3},maxSize:{4},currentSize:{5},isPrivate:{6},matchAttributes.Count:{7},hostNodeId:{8},directConnectInfos.Count:{9}", base.ToString(), networkId.ToString("X"), name, averageEloScore, maxSize, currentSize, isPrivate, (matchAttributes != null) ? matchAttributes.Count : 0, hostNodeId, directConnectInfos.Count);
		}

		public override void Parse(object obj)
		{
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				networkId = (NetworkID)ParseJSONUInt64("networkId", obj, dictionary);
				name = ParseJSONString("name", obj, dictionary);
				averageEloScore = ParseJSONInt32("averageEloScore", obj, dictionary);
				maxSize = ParseJSONInt32("maxSize", obj, dictionary);
				currentSize = ParseJSONInt32("currentSize", obj, dictionary);
				isPrivate = ParseJSONBool("isPrivate", obj, dictionary);
				hostNodeId = (NodeID)ParseJSONUInt16("hostNodeId", obj, dictionary);
				directConnectInfos = ParseJSONList<MatchDirectConnectInfo>("directConnectInfos", obj, dictionary);
				return;
			}
			throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
		}
	}
}
