using System;

namespace UnityEngine.SocialPlatforms.Impl
{
	public class Leaderboard : ILeaderboard
	{
		private bool m_Loading;

		private IScore m_LocalUserScore;

		private uint m_MaxRange;

		private IScore[] m_Scores;

		private string m_Title;

		private string[] m_UserIDs;

		public bool loading => ActivePlatform.Instance.GetLoading(this);

		public string id
		{
			get;
			set;
		}

		public UserScope userScope
		{
			get;
			set;
		}

		public Range range
		{
			get;
			set;
		}

		public TimeScope timeScope
		{
			get;
			set;
		}

		public IScore localUserScore => m_LocalUserScore;

		public uint maxRange => m_MaxRange;

		public IScore[] scores => m_Scores;

		public string title => m_Title;

		public Leaderboard()
		{
			id = "Invalid";
			range = new Range(1, 10);
			userScope = UserScope.Global;
			timeScope = TimeScope.AllTime;
			m_Loading = false;
			m_LocalUserScore = new Score("Invalid", 0L);
			m_MaxRange = 0u;
			m_Scores = new Score[0];
			m_Title = "Invalid";
			m_UserIDs = new string[0];
		}

		public void SetUserFilter(string[] userIDs)
		{
			m_UserIDs = userIDs;
		}

		public override string ToString()
		{
			object[] obj = new object[20]
			{
				"ID: '",
				id,
				"' Title: '",
				m_Title,
				"' Loading: '",
				m_Loading,
				"' Range: [",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			Range range = this.range;
			obj[7] = range.from;
			obj[8] = ",";
			Range range2 = this.range;
			obj[9] = range2.count;
			obj[10] = "] MaxRange: '";
			obj[11] = m_MaxRange;
			obj[12] = "' Scores: '";
			obj[13] = m_Scores.Length;
			obj[14] = "' UserScope: '";
			obj[15] = userScope;
			obj[16] = "' TimeScope: '";
			obj[17] = timeScope;
			obj[18] = "' UserFilter: '";
			obj[19] = m_UserIDs.Length;
			return string.Concat(obj);
		}

		public void LoadScores(Action<bool> callback)
		{
			ActivePlatform.Instance.LoadScores(this, callback);
		}

		public void SetLocalUserScore(IScore score)
		{
			m_LocalUserScore = score;
		}

		public void SetMaxRange(uint maxRange)
		{
			m_MaxRange = maxRange;
		}

		public void SetScores(IScore[] scores)
		{
			m_Scores = scores;
		}

		public void SetTitle(string title)
		{
			m_Title = title;
		}

		public string[] GetUserFilter()
		{
			return m_UserIDs;
		}
	}
}
