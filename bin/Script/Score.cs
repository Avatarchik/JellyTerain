using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.SocialPlatforms.Impl
{
	public class Score : IScore
	{
		private DateTime m_Date;

		private string m_FormattedValue;

		private string m_UserID;

		private int m_Rank;

		public string leaderboardID
		{
			get;
			set;
		}

		public long value
		{
			[CompilerGenerated]
			get
			{
				return value;
			}
			[CompilerGenerated]
			set
			{
				this.value = value;
			}
		}

		public DateTime date => m_Date;

		public string formattedValue => m_FormattedValue;

		public string userID => m_UserID;

		public int rank => m_Rank;

		public Score()
			: this("unkown", -1L)
		{
		}

		public Score(string leaderboardID, long value)
			: this(leaderboardID, value, "0", DateTime.Now, "", -1)
		{
		}

		public Score(string leaderboardID, long value, string userID, DateTime date, string formattedValue, int rank)
		{
			this.leaderboardID = leaderboardID;
			this.value = value;
			m_UserID = userID;
			m_Date = date;
			m_FormattedValue = formattedValue;
			m_Rank = rank;
		}

		public override string ToString()
		{
			return "Rank: '" + m_Rank + "' Value: '" + value + "' Category: '" + leaderboardID + "' PlayerID: '" + m_UserID + "' Date: '" + m_Date;
		}

		public void ReportScore(Action<bool> callback)
		{
			ActivePlatform.Instance.ReportScore(value, leaderboardID, callback);
		}

		public void SetDate(DateTime date)
		{
			m_Date = date;
		}

		public void SetFormattedValue(string value)
		{
			m_FormattedValue = value;
		}

		public void SetUserID(string userID)
		{
			m_UserID = userID;
		}

		public void SetRank(int rank)
		{
			m_Rank = rank;
		}
	}
}
