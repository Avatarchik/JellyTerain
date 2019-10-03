using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	[RequiredByNativeCode]
	public sealed class AnimationCurve
	{
		internal IntPtr m_Ptr;

		public Keyframe[] keys
		{
			get
			{
				return GetKeys();
			}
			set
			{
				SetKeys(value);
			}
		}

		public Keyframe this[int index] => GetKey_Internal(index);

		public int length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public WrapMode preWrapMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public WrapMode postWrapMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public AnimationCurve(params Keyframe[] keys)
		{
			Init(keys);
		}

		[RequiredByNativeCode]
		public AnimationCurve()
		{
			Init(null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Cleanup();

		~AnimationCurve()
		{
			Cleanup();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float Evaluate(float time);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int AddKey(float time, float value);

		public int AddKey(Keyframe key)
		{
			return AddKey_Internal(key);
		}

		private int AddKey_Internal(Keyframe key)
		{
			return INTERNAL_CALL_AddKey_Internal(this, ref key);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_AddKey_Internal(AnimationCurve self, ref Keyframe key);

		public int MoveKey(int index, Keyframe key)
		{
			return INTERNAL_CALL_MoveKey(this, index, ref key);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_MoveKey(AnimationCurve self, int index, ref Keyframe key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RemoveKey(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetKeys(Keyframe[] keys);

		private Keyframe GetKey_Internal(int index)
		{
			INTERNAL_CALL_GetKey_Internal(this, index, out Keyframe value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetKey_Internal(AnimationCurve self, int index, out Keyframe value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Keyframe[] GetKeys();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SmoothTangents(int index, float weight);

		public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			float num = (valueEnd - valueStart) / (timeEnd - timeStart);
			Keyframe[] keys = new Keyframe[2]
			{
				new Keyframe(timeStart, valueStart, 0f, num),
				new Keyframe(timeEnd, valueEnd, num, 0f)
			};
			return new AnimationCurve(keys);
		}

		public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			Keyframe[] keys = new Keyframe[2]
			{
				new Keyframe(timeStart, valueStart, 0f, 0f),
				new Keyframe(timeEnd, valueEnd, 0f, 0f)
			};
			return new AnimationCurve(keys);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void Init(Keyframe[] keys);
	}
}
