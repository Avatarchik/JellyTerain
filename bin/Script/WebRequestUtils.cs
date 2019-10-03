using System;
using System.Text.RegularExpressions;
using UnityEngine.Scripting;

namespace UnityEngineInternal
{
	internal static class WebRequestUtils
	{
		private static Regex domainRegex = new Regex("^\\s*\\w+(?:\\.\\w+)+\\s*$");

		[RequiredByNativeCode]
		internal static string RedirectTo(string baseUri, string redirectUri)
		{
			Uri uri = (redirectUri[0] != '/') ? new Uri(redirectUri, UriKind.RelativeOrAbsolute) : new Uri(redirectUri, UriKind.Relative);
			if (uri.IsAbsoluteUri)
			{
				return uri.AbsoluteUri;
			}
			Uri baseUri2 = new Uri(baseUri, UriKind.Absolute);
			Uri uri2 = new Uri(baseUri2, uri);
			return uri2.AbsoluteUri;
		}

		internal static string MakeInitialUrl(string targetUrl, string localUrl)
		{
			Uri uri = new Uri(localUrl);
			if (targetUrl.StartsWith("//"))
			{
				targetUrl = uri.Scheme + ":" + targetUrl;
			}
			if (targetUrl.StartsWith("/"))
			{
				targetUrl = uri.Scheme + "://" + uri.Host + targetUrl;
			}
			if (domainRegex.IsMatch(targetUrl))
			{
				targetUrl = uri.Scheme + "://" + targetUrl;
			}
			Uri uri2 = null;
			try
			{
				uri2 = new Uri(targetUrl);
			}
			catch (FormatException ex)
			{
				try
				{
					uri2 = new Uri(uri, targetUrl);
				}
				catch (FormatException)
				{
					throw ex;
				}
			}
			return (!targetUrl.Contains("%")) ? uri2.AbsoluteUri : uri2.OriginalString;
		}
	}
}
