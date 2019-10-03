using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	[UsedByNativeCode]
	public sealed class RectOffset
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly object m_SourceStyle;

		public int left
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int right
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int top
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int bottom
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int horizontal
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int vertical
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public RectOffset()
		{
			Init();
		}

		internal RectOffset(object sourceStyle, IntPtr source)
		{
			m_SourceStyle = sourceStyle;
			m_Ptr = source;
		}

		public RectOffset(int left, int right, int top, int bottom)
		{
			Init();
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Init();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Cleanup();

		public Rect Add(Rect rect)
		{
			INTERNAL_CALL_Add(this, ref rect, out Rect value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Add(RectOffset self, ref Rect rect, out Rect value);

		public Rect Remove(Rect rect)
		{
			INTERNAL_CALL_Remove(this, ref rect, out Rect value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Remove(RectOffset self, ref Rect rect, out Rect value);

		~RectOffset()
		{
			if (m_SourceStyle == null)
			{
				Cleanup();
			}
		}

		public override string ToString()
		{
			return UnityString.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", left, right, top, bottom);
		}
	}
}
