using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	public static class PhraseRecognitionSystem
	{
		public delegate void ErrorDelegate(SpeechError errorCode);

		public delegate void StatusDelegate(SpeechSystemStatus status);

		[ThreadAndSerializationSafe]
		public static bool isSupported
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static SpeechSystemStatus Status
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static event ErrorDelegate OnError;

		public static event StatusDelegate OnStatusChanged;

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Restart();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Shutdown();

		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeErrorEvent(SpeechError errorCode)
		{
			PhraseRecognitionSystem.OnError?.Invoke(errorCode);
		}

		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeStatusChangedEvent(SpeechSystemStatus status)
		{
			PhraseRecognitionSystem.OnStatusChanged?.Invoke(status);
		}
	}
}
