using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	internal class ListMatchRequest : Request
	{
		[Obsolete("This bool is deprecated in favor of filterOutPrivateMatches")]
		public bool includePasswordMatches;

		public int pageSize
		{
			get;
			set;
		}

		public int pageNum
		{
			get;
			set;
		}

		public string nameFilter
		{
			get;
			set;
		}

		public bool filterOutPrivateMatches
		{
			get;
			set;
		}

		public int eloScore
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterLessThan
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterEqualTo
		{
			get;
			set;
		}

		public Dictionary<string, long> matchAttributeFilterGreaterThan
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-pageSize:{1},pageNum:{2},nameFilter:{3}, filterOutPrivateMatches:{4}, eloScore:{5}, matchAttributeFilterLessThan.Count:{6}, matchAttributeFilterEqualTo.Count:{7}, matchAttributeFilterGreaterThan.Count:{8}", base.ToString(), pageSize, pageNum, nameFilter, filterOutPrivateMatches, eloScore, (matchAttributeFilterLessThan != null) ? matchAttributeFilterLessThan.Count : 0, (matchAttributeFilterEqualTo != null) ? matchAttributeFilterEqualTo.Count : 0, (matchAttributeFilterGreaterThan != null) ? matchAttributeFilterGreaterThan.Count : 0);
		}

		public override bool IsValid()
		{
			int num = (matchAttributeFilterLessThan != null) ? matchAttributeFilterLessThan.Count : 0;
			num += ((matchAttributeFilterEqualTo != null) ? matchAttributeFilterEqualTo.Count : 0);
			num += ((matchAttributeFilterGreaterThan != null) ? matchAttributeFilterGreaterThan.Count : 0);
			return base.IsValid() && (pageSize >= 1 || pageSize <= 1000) && num <= 10;
		}
	}
}
