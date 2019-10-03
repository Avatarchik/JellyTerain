using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAssetBundle : DownloadHandler
	{
		public AssetBundle assetBundle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public DownloadHandlerAssetBundle(string url, uint crc)
		{
			InternalCreateAssetBundle(url, crc);
		}

		public DownloadHandlerAssetBundle(string url, uint version, uint crc)
		{
			InternalCreateAssetBundle(url, new Hash128(0u, 0u, 0u, version), crc);
		}

		public DownloadHandlerAssetBundle(string url, Hash128 hash, uint crc)
		{
			InternalCreateAssetBundle(url, hash, crc);
		}

		protected override byte[] GetData()
		{
			throw new NotSupportedException("Raw data access is not supported for asset bundles");
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for asset bundles");
		}

		public static AssetBundle GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerAssetBundle>(www).assetBundle;
		}
	}
}
