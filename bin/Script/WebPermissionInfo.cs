using System.Text.RegularExpressions;

namespace System.Net
{
	internal class WebPermissionInfo
	{
		private WebPermissionInfoType _type;

		private object _info;

		public string Info
		{
			get
			{
				if (_type == WebPermissionInfoType.InfoRegex)
				{
					return null;
				}
				return (string)_info;
			}
		}

		public WebPermissionInfo(WebPermissionInfoType type, string info)
		{
			_type = type;
			_info = info;
		}

		public WebPermissionInfo(Regex regex)
		{
			_type = WebPermissionInfoType.InfoRegex;
			_info = regex;
		}
	}
}
