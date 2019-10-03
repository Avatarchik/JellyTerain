using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GUIStyleState
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly GUIStyle m_SourceStyle;

		[NonSerialized]
		private Texture2D m_Background;

		public Texture2D background
		{
			get
			{
				return GetBackgroundInternal();
			}
			set
			{
				SetBackgroundInternal(value);
				m_Background = value;
			}
		}

		public Color textColor
		{
			get
			{
				INTERNAL_get_textColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_textColor(ref value);
			}
		}

		public GUIStyleState()
		{
			Init();
		}

		private GUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			m_SourceStyle = sourceStyle;
			m_Ptr = source;
		}

		internal static GUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
		{
			GUIStyleState gUIStyleState = new GUIStyleState(sourceStyle, source);
			gUIStyleState.m_Background = gUIStyleState.GetBackgroundInternalFromDeserialization();
			return gUIStyleState;
		}

		internal static GUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			GUIStyleState gUIStyleState = new GUIStyleState(sourceStyle, source);
			gUIStyleState.m_Background = gUIStyleState.GetBackgroundInternal();
			return gUIStyleState;
		}

		~GUIStyleState()
		{
			if (m_SourceStyle == null)
			{
				Cleanup();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Init();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Cleanup();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetBackgroundInternal(Texture2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern Texture2D GetBackgroundInternalFromDeserialization();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Texture2D GetBackgroundInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_textColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_textColor(ref Color value);
	}
}
