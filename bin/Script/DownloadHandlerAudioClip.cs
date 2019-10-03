using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAudioClip : DownloadHandler
	{
		public AudioClip audioClip
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[RequiredByNativeCode]
		public DownloadHandlerAudioClip(string url, AudioType audioType)
		{
			InternalCreateAudioClip(url, audioType);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalCreateAudioClip(string url, AudioType audioType);

		protected override byte[] GetData()
		{
			return InternalGetData();
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for audio clips");
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern byte[] InternalGetData();

		public static AudioClip GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerAudioClip>(www).audioClip;
		}
	}
}
