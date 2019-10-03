using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	public abstract class PhraseRecognizer : IDisposable
	{
		public delegate void PhraseRecognizedDelegate(PhraseRecognizedEventArgs args);

		protected IntPtr m_Recognizer;

		public bool IsRunning => m_Recognizer != IntPtr.Zero && IsRunning_Internal(m_Recognizer);

		public event PhraseRecognizedDelegate OnPhraseRecognized;

		internal PhraseRecognizer()
		{
		}

		protected IntPtr CreateFromKeywords(string[] keywords, ConfidenceLevel minimumConfidence)
		{
			INTERNAL_CALL_CreateFromKeywords(this, keywords, minimumConfidence, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CreateFromKeywords(PhraseRecognizer self, string[] keywords, ConfidenceLevel minimumConfidence, out IntPtr value);

		protected IntPtr CreateFromGrammarFile(string grammarFilePath, ConfidenceLevel minimumConfidence)
		{
			INTERNAL_CALL_CreateFromGrammarFile(this, grammarFilePath, minimumConfidence, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CreateFromGrammarFile(PhraseRecognizer self, string grammarFilePath, ConfidenceLevel minimumConfidence, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Start_Internal(IntPtr recognizer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Stop_Internal(IntPtr recognizer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool IsRunning_Internal(IntPtr recognizer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Destroy(IntPtr recognizer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern void DestroyThreaded(IntPtr recognizer);

		~PhraseRecognizer()
		{
			if (m_Recognizer != IntPtr.Zero)
			{
				DestroyThreaded(m_Recognizer);
				m_Recognizer = IntPtr.Zero;
				GC.SuppressFinalize(this);
			}
		}

		public void Start()
		{
			if (!(m_Recognizer == IntPtr.Zero))
			{
				Start_Internal(m_Recognizer);
			}
		}

		public void Stop()
		{
			if (!(m_Recognizer == IntPtr.Zero))
			{
				Stop_Internal(m_Recognizer);
			}
		}

		public void Dispose()
		{
			if (m_Recognizer != IntPtr.Zero)
			{
				Destroy(m_Recognizer);
				m_Recognizer = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		[RequiredByNativeCode]
		private void InvokePhraseRecognizedEvent(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, long phraseStartFileTime, long phraseDurationTicks)
		{
			this.OnPhraseRecognized?.Invoke(new PhraseRecognizedEventArgs(text, confidence, semanticMeanings, DateTime.FromFileTime(phraseStartFileTime), TimeSpan.FromTicks(phraseDurationTicks)));
		}

		[RequiredByNativeCode]
		private unsafe static SemanticMeaning[] MarshalSemanticMeaning(IntPtr keys, IntPtr values, IntPtr valueSizes, int valueCount)
		{
			SemanticMeaning[] array = new SemanticMeaning[valueCount];
			int num = 0;
			for (int i = 0; i < valueCount; i++)
			{
				uint num2 = *(uint*)((byte*)(void*)valueSizes + (long)i * 4L);
				SemanticMeaning semanticMeaning = default(SemanticMeaning);
				semanticMeaning.key = new string((char*)(long)(*(IntPtr*)((byte*)(void*)keys + (long)i * (long)sizeof(char*))));
				semanticMeaning.values = new string[num2];
				SemanticMeaning semanticMeaning2 = semanticMeaning;
				for (int j = 0; j < num2; j++)
				{
					semanticMeaning2.values[j] = new string((char*)(long)(*(IntPtr*)((byte*)(void*)values + (long)(num + j) * (long)sizeof(char*))));
				}
				array[i] = semanticMeaning2;
				num += (int)num2;
			}
			return array;
		}
	}
}
