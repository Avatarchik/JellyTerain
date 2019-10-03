using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Random
	{
		[Serializable]
		public struct State
		{
			[SerializeField]
			private int s0;

			[SerializeField]
			private int s1;

			[SerializeField]
			private int s2;

			[SerializeField]
			private int s3;
		}

		[Obsolete("Deprecated. Use InitState() function or Random.state property instead.")]
		public static int seed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static State state
		{
			get
			{
				INTERNAL_get_state(out State value);
				return value;
			}
			set
			{
				INTERNAL_set_state(ref value);
			}
		}

		public static float value
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static Vector3 insideUnitSphere
		{
			get
			{
				INTERNAL_get_insideUnitSphere(out Vector3 value);
				return value;
			}
		}

		public static Vector2 insideUnitCircle
		{
			get
			{
				GetRandomUnitCircle(out Vector2 output);
				return output;
			}
		}

		public static Vector3 onUnitSphere
		{
			get
			{
				INTERNAL_get_onUnitSphere(out Vector3 value);
				return value;
			}
		}

		public static Quaternion rotation
		{
			get
			{
				INTERNAL_get_rotation(out Quaternion value);
				return value;
			}
		}

		public static Quaternion rotationUniform
		{
			get
			{
				INTERNAL_get_rotationUniform(out Quaternion value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void InitState(int seed);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_state(out State value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_state(ref State value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern float Range(float min, float max);

		public static int Range(int min, int max)
		{
			return RandomRangeInt(min, max);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int RandomRangeInt(int min, int max);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_insideUnitSphere(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void GetRandomUnitCircle(out Vector2 output);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_onUnitSphere(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_rotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_rotationUniform(out Quaternion value);

		[Obsolete("Use Random.Range instead")]
		public static float RandomRange(float min, float max)
		{
			return Range(min, max);
		}

		[Obsolete("Use Random.Range instead")]
		public static int RandomRange(int min, int max)
		{
			return Range(min, max);
		}

		public static Color ColorHSV()
		{
			return ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
		}

		public static Color ColorHSV(float hueMin, float hueMax)
		{
			return ColorHSV(hueMin, hueMax, 0f, 1f, 0f, 1f, 1f, 1f);
		}

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
		{
			return ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0f, 1f, 1f, 1f);
		}

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
		{
			return ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f);
		}

		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
		{
			float h = Mathf.Lerp(hueMin, hueMax, value);
			float s = Mathf.Lerp(saturationMin, saturationMax, value);
			float v = Mathf.Lerp(valueMin, valueMax, value);
			Color result = Color.HSVToRGB(h, s, v, hdr: true);
			result.a = Mathf.Lerp(alphaMin, alphaMax, value);
			return result;
		}
	}
}
