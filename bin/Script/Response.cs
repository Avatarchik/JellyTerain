using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	internal abstract class Response : ResponseBase, IResponse
	{
		public bool success
		{
			get;
			private set;
		}

		public string extendedInfo
		{
			get;
			private set;
		}

		public void SetSuccess()
		{
			success = true;
			extendedInfo = "";
		}

		public void SetFailure(string info)
		{
			success = false;
			extendedInfo += info;
		}

		public override string ToString()
		{
			return UnityString.Format("[{0}]-success:{1}-extendedInfo:{2}", base.ToString(), success, extendedInfo);
		}

		public override void Parse(object obj)
		{
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				success = ParseJSONBool("success", obj, dictionary);
				extendedInfo = ParseJSONString("extendedInfo", obj, dictionary);
				if (!success)
				{
					throw new FormatException("FAILURE Returned from server: " + extendedInfo);
				}
			}
		}
	}
}
