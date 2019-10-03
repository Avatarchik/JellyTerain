using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class ParticleSystem : Component
	{
		public struct Burst
		{
			private float m_Time;

			private short m_MinCount;

			private short m_MaxCount;

			private int m_RepeatCount;

			private float m_RepeatInterval;

			public float time
			{
				get
				{
					return m_Time;
				}
				set
				{
					m_Time = value;
				}
			}

			public short minCount
			{
				get
				{
					return m_MinCount;
				}
				set
				{
					m_MinCount = value;
				}
			}

			public short maxCount
			{
				get
				{
					return m_MaxCount;
				}
				set
				{
					m_MaxCount = value;
				}
			}

			public int cycleCount
			{
				get
				{
					return m_RepeatCount + 1;
				}
				set
				{
					m_RepeatCount = value - 1;
				}
			}

			public float repeatInterval
			{
				get
				{
					return m_RepeatInterval;
				}
				set
				{
					m_RepeatInterval = value;
				}
			}

			public Burst(float _time, short _count)
			{
				m_Time = _time;
				m_MinCount = _count;
				m_MaxCount = _count;
				m_RepeatCount = 0;
				m_RepeatInterval = 0f;
			}

			public Burst(float _time, short _minCount, short _maxCount)
			{
				m_Time = _time;
				m_MinCount = _minCount;
				m_MaxCount = _maxCount;
				m_RepeatCount = 0;
				m_RepeatInterval = 0f;
			}

			public Burst(float _time, short _minCount, short _maxCount, int _cycleCount, float _repeatInterval)
			{
				m_Time = _time;
				m_MinCount = _minCount;
				m_MaxCount = _maxCount;
				m_RepeatCount = _cycleCount - 1;
				m_RepeatInterval = _repeatInterval;
			}
		}

		public struct MinMaxCurve
		{
			private ParticleSystemCurveMode m_Mode;

			private float m_CurveMultiplier;

			private AnimationCurve m_CurveMin;

			private AnimationCurve m_CurveMax;

			private float m_ConstantMin;

			private float m_ConstantMax;

			public ParticleSystemCurveMode mode
			{
				get
				{
					return m_Mode;
				}
				set
				{
					m_Mode = value;
				}
			}

			[Obsolete("Please use MinMaxCurve.curveMultiplier instead. (UnityUpgradable) -> UnityEngine.ParticleSystem/MinMaxCurve.curveMultiplier")]
			public float curveScalar
			{
				get
				{
					return m_CurveMultiplier;
				}
				set
				{
					m_CurveMultiplier = value;
				}
			}

			public float curveMultiplier
			{
				get
				{
					return m_CurveMultiplier;
				}
				set
				{
					m_CurveMultiplier = value;
				}
			}

			public AnimationCurve curveMax
			{
				get
				{
					return m_CurveMax;
				}
				set
				{
					m_CurveMax = value;
				}
			}

			public AnimationCurve curveMin
			{
				get
				{
					return m_CurveMin;
				}
				set
				{
					m_CurveMin = value;
				}
			}

			public float constantMax
			{
				get
				{
					return m_ConstantMax;
				}
				set
				{
					m_ConstantMax = value;
				}
			}

			public float constantMin
			{
				get
				{
					return m_ConstantMin;
				}
				set
				{
					m_ConstantMin = value;
				}
			}

			public float constant
			{
				get
				{
					return m_ConstantMax;
				}
				set
				{
					m_ConstantMax = value;
				}
			}

			public AnimationCurve curve
			{
				get
				{
					return m_CurveMax;
				}
				set
				{
					m_CurveMax = value;
				}
			}

			public MinMaxCurve(float constant)
			{
				m_Mode = ParticleSystemCurveMode.Constant;
				m_CurveMultiplier = 0f;
				m_CurveMin = null;
				m_CurveMax = null;
				m_ConstantMin = 0f;
				m_ConstantMax = constant;
			}

			public MinMaxCurve(float multiplier, AnimationCurve curve)
			{
				m_Mode = ParticleSystemCurveMode.Curve;
				m_CurveMultiplier = multiplier;
				m_CurveMin = null;
				m_CurveMax = curve;
				m_ConstantMin = 0f;
				m_ConstantMax = 0f;
			}

			public MinMaxCurve(float multiplier, AnimationCurve min, AnimationCurve max)
			{
				m_Mode = ParticleSystemCurveMode.TwoCurves;
				m_CurveMultiplier = multiplier;
				m_CurveMin = min;
				m_CurveMax = max;
				m_ConstantMin = 0f;
				m_ConstantMax = 0f;
			}

			public MinMaxCurve(float min, float max)
			{
				m_Mode = ParticleSystemCurveMode.TwoConstants;
				m_CurveMultiplier = 0f;
				m_CurveMin = null;
				m_CurveMax = null;
				m_ConstantMin = min;
				m_ConstantMax = max;
			}

			public float Evaluate(float time)
			{
				return Evaluate(time, 1f);
			}

			public float Evaluate(float time, float lerpFactor)
			{
				time = Mathf.Clamp(time, 0f, 1f);
				lerpFactor = Mathf.Clamp(lerpFactor, 0f, 1f);
				if (m_Mode == ParticleSystemCurveMode.Constant)
				{
					return m_ConstantMax;
				}
				if (m_Mode == ParticleSystemCurveMode.TwoConstants)
				{
					return Mathf.Lerp(m_ConstantMin, m_ConstantMax, lerpFactor);
				}
				float num = m_CurveMax.Evaluate(time) * m_CurveMultiplier;
				if (m_Mode == ParticleSystemCurveMode.TwoCurves)
				{
					return Mathf.Lerp(m_CurveMin.Evaluate(time) * m_CurveMultiplier, num, lerpFactor);
				}
				return num;
			}

			public static implicit operator MinMaxCurve(float constant)
			{
				return new MinMaxCurve(constant);
			}
		}

		public struct MinMaxGradient
		{
			private ParticleSystemGradientMode m_Mode;

			private Gradient m_GradientMin;

			private Gradient m_GradientMax;

			private Color m_ColorMin;

			private Color m_ColorMax;

			public ParticleSystemGradientMode mode
			{
				get
				{
					return m_Mode;
				}
				set
				{
					m_Mode = value;
				}
			}

			public Gradient gradientMax
			{
				get
				{
					return m_GradientMax;
				}
				set
				{
					m_GradientMax = value;
				}
			}

			public Gradient gradientMin
			{
				get
				{
					return m_GradientMin;
				}
				set
				{
					m_GradientMin = value;
				}
			}

			public Color colorMax
			{
				get
				{
					return m_ColorMax;
				}
				set
				{
					m_ColorMax = value;
				}
			}

			public Color colorMin
			{
				get
				{
					return m_ColorMin;
				}
				set
				{
					m_ColorMin = value;
				}
			}

			public Color color
			{
				get
				{
					return m_ColorMax;
				}
				set
				{
					m_ColorMax = value;
				}
			}

			public Gradient gradient
			{
				get
				{
					return m_GradientMax;
				}
				set
				{
					m_GradientMax = value;
				}
			}

			public MinMaxGradient(Color color)
			{
				m_Mode = ParticleSystemGradientMode.Color;
				m_GradientMin = null;
				m_GradientMax = null;
				m_ColorMin = Color.black;
				m_ColorMax = color;
			}

			public MinMaxGradient(Gradient gradient)
			{
				m_Mode = ParticleSystemGradientMode.Gradient;
				m_GradientMin = null;
				m_GradientMax = gradient;
				m_ColorMin = Color.black;
				m_ColorMax = Color.black;
			}

			public MinMaxGradient(Color min, Color max)
			{
				m_Mode = ParticleSystemGradientMode.TwoColors;
				m_GradientMin = null;
				m_GradientMax = null;
				m_ColorMin = min;
				m_ColorMax = max;
			}

			public MinMaxGradient(Gradient min, Gradient max)
			{
				m_Mode = ParticleSystemGradientMode.TwoGradients;
				m_GradientMin = min;
				m_GradientMax = max;
				m_ColorMin = Color.black;
				m_ColorMax = Color.black;
			}

			public Color Evaluate(float time)
			{
				return Evaluate(time, 1f);
			}

			public Color Evaluate(float time, float lerpFactor)
			{
				time = Mathf.Clamp(time, 0f, 1f);
				lerpFactor = Mathf.Clamp(lerpFactor, 0f, 1f);
				if (m_Mode == ParticleSystemGradientMode.Color)
				{
					return m_ColorMax;
				}
				if (m_Mode == ParticleSystemGradientMode.TwoColors)
				{
					return Color.Lerp(m_ColorMin, m_ColorMax, lerpFactor);
				}
				Color color = m_GradientMax.Evaluate(time);
				if (m_Mode == ParticleSystemGradientMode.TwoGradients)
				{
					return Color.Lerp(m_GradientMin.Evaluate(time), color, lerpFactor);
				}
				return color;
			}

			public static implicit operator MinMaxGradient(Color color)
			{
				return new MinMaxGradient(color);
			}

			public static implicit operator MinMaxGradient(Gradient gradient)
			{
				return new MinMaxGradient(gradient);
			}
		}

		public struct MainModule
		{
			private ParticleSystem m_ParticleSystem;

			public float duration
			{
				get
				{
					return GetDuration(m_ParticleSystem);
				}
				set
				{
					SetDuration(m_ParticleSystem, value);
				}
			}

			public bool loop
			{
				get
				{
					return GetLoop(m_ParticleSystem);
				}
				set
				{
					SetLoop(m_ParticleSystem, value);
				}
			}

			public bool prewarm
			{
				get
				{
					return GetPrewarm(m_ParticleSystem);
				}
				set
				{
					SetPrewarm(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startDelay
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartDelay(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartDelay(m_ParticleSystem, ref value);
				}
			}

			public float startDelayMultiplier
			{
				get
				{
					return GetStartDelayMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartDelayMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startLifetime
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartLifetime(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartLifetime(m_ParticleSystem, ref value);
				}
			}

			public float startLifetimeMultiplier
			{
				get
				{
					return GetStartLifetimeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartLifetimeMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startSpeed
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartSpeed(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartSpeed(m_ParticleSystem, ref value);
				}
			}

			public float startSpeedMultiplier
			{
				get
				{
					return GetStartSpeedMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartSpeedMultiplier(m_ParticleSystem, value);
				}
			}

			public bool startSize3D
			{
				get
				{
					return GetStartSize3D(m_ParticleSystem);
				}
				set
				{
					SetStartSize3D(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startSize
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartSizeX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartSizeX(m_ParticleSystem, ref value);
				}
			}

			public float startSizeMultiplier
			{
				get
				{
					return GetStartSizeXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartSizeXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startSizeX
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartSizeX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartSizeX(m_ParticleSystem, ref value);
				}
			}

			public float startSizeXMultiplier
			{
				get
				{
					return GetStartSizeXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartSizeXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startSizeY
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartSizeY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartSizeY(m_ParticleSystem, ref value);
				}
			}

			public float startSizeYMultiplier
			{
				get
				{
					return GetStartSizeYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartSizeYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startSizeZ
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartSizeZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartSizeZ(m_ParticleSystem, ref value);
				}
			}

			public float startSizeZMultiplier
			{
				get
				{
					return GetStartSizeZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartSizeZMultiplier(m_ParticleSystem, value);
				}
			}

			public bool startRotation3D
			{
				get
				{
					return GetStartRotation3D(m_ParticleSystem);
				}
				set
				{
					SetStartRotation3D(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startRotation
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartRotationZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartRotationZ(m_ParticleSystem, ref value);
				}
			}

			public float startRotationMultiplier
			{
				get
				{
					return GetStartRotationZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartRotationZMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startRotationX
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartRotationX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartRotationX(m_ParticleSystem, ref value);
				}
			}

			public float startRotationXMultiplier
			{
				get
				{
					return GetStartRotationXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartRotationXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startRotationY
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartRotationY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartRotationY(m_ParticleSystem, ref value);
				}
			}

			public float startRotationYMultiplier
			{
				get
				{
					return GetStartRotationYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartRotationYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startRotationZ
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartRotationZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartRotationZ(m_ParticleSystem, ref value);
				}
			}

			public float startRotationZMultiplier
			{
				get
				{
					return GetStartRotationZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartRotationZMultiplier(m_ParticleSystem, value);
				}
			}

			public float randomizeRotationDirection
			{
				get
				{
					return GetRandomizeRotationDirection(m_ParticleSystem);
				}
				set
				{
					SetRandomizeRotationDirection(m_ParticleSystem, value);
				}
			}

			public MinMaxGradient startColor
			{
				get
				{
					MinMaxGradient gradient = default(MinMaxGradient);
					GetStartColor(m_ParticleSystem, ref gradient);
					return gradient;
				}
				set
				{
					SetStartColor(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve gravityModifier
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetGravityModifier(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetGravityModifier(m_ParticleSystem, ref value);
				}
			}

			public float gravityModifierMultiplier
			{
				get
				{
					return GetGravityModifierMultiplier(m_ParticleSystem);
				}
				set
				{
					SetGravityModifierMultiplier(m_ParticleSystem, value);
				}
			}

			public ParticleSystemSimulationSpace simulationSpace
			{
				get
				{
					return GetSimulationSpace(m_ParticleSystem);
				}
				set
				{
					SetSimulationSpace(m_ParticleSystem, value);
				}
			}

			public Transform customSimulationSpace
			{
				get
				{
					return GetCustomSimulationSpace(m_ParticleSystem);
				}
				set
				{
					SetCustomSimulationSpace(m_ParticleSystem, value);
				}
			}

			public float simulationSpeed
			{
				get
				{
					return GetSimulationSpeed(m_ParticleSystem);
				}
				set
				{
					SetSimulationSpeed(m_ParticleSystem, value);
				}
			}

			public ParticleSystemScalingMode scalingMode
			{
				get
				{
					return GetScalingMode(m_ParticleSystem);
				}
				set
				{
					SetScalingMode(m_ParticleSystem, value);
				}
			}

			public bool playOnAwake
			{
				get
				{
					return GetPlayOnAwake(m_ParticleSystem);
				}
				set
				{
					SetPlayOnAwake(m_ParticleSystem, value);
				}
			}

			public int maxParticles
			{
				get
				{
					return GetMaxParticles(m_ParticleSystem);
				}
				set
				{
					SetMaxParticles(m_ParticleSystem, value);
				}
			}

			internal MainModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDuration(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetDuration(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLoop(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetLoop(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetPrewarm(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetPrewarm(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartDelay(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartDelay(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartDelayMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartDelayMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartLifetime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartLifetime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartLifetimeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartLifetimeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSpeedMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartSpeedMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSize3D(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetStartSize3D(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartSizeX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartSizeXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartSizeY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartSizeYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartSizeZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartSizeZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartSizeZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotation3D(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetStartRotation3D(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartRotationX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartRotationXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartRotationY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartRotationYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartRotationZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartRotationZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartRotationZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRandomizeRotationDirection(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRandomizeRotationDirection(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartColor(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartColor(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetGravityModifier(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetGravityModifier(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetGravityModifierMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetGravityModifierMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSimulationSpace(ParticleSystem system, ParticleSystemSimulationSpace value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystemSimulationSpace GetSimulationSpace(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCustomSimulationSpace(ParticleSystem system, Transform value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern Transform GetCustomSimulationSpace(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSimulationSpeed(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetSimulationSpeed(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetScalingMode(ParticleSystem system, ParticleSystemScalingMode value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystemScalingMode GetScalingMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetPlayOnAwake(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetPlayOnAwake(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMaxParticles(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMaxParticles(ParticleSystem system);
		}

		public struct EmissionModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve rateOverTime
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRateOverTime(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRateOverTime(m_ParticleSystem, ref value);
				}
			}

			public float rateOverTimeMultiplier
			{
				get
				{
					return GetRateOverTimeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRateOverTimeMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve rateOverDistance
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRateOverDistance(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRateOverDistance(m_ParticleSystem, ref value);
				}
			}

			public float rateOverDistanceMultiplier
			{
				get
				{
					return GetRateOverDistanceMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRateOverDistanceMultiplier(m_ParticleSystem, value);
				}
			}

			public int burstCount => GetBurstCount(m_ParticleSystem);

			[Obsolete("ParticleSystemEmissionType no longer does anything. Time and Distance based emission are now both always active.")]
			public ParticleSystemEmissionType type
			{
				get
				{
					return ParticleSystemEmissionType.Time;
				}
				set
				{
				}
			}

			[Obsolete("rate property is deprecated. Use rateOverTime or rateOverDistance instead.")]
			public MinMaxCurve rate
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRateOverTime(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRateOverTime(m_ParticleSystem, ref value);
				}
			}

			[Obsolete("rateMultiplier property is deprecated. Use rateOverTimeMultiplier or rateOverDistanceMultiplier instead.")]
			public float rateMultiplier
			{
				get
				{
					return GetRateOverTimeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRateOverTimeMultiplier(m_ParticleSystem, value);
				}
			}

			internal EmissionModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			public void SetBursts(Burst[] bursts)
			{
				SetBursts(m_ParticleSystem, bursts, bursts.Length);
			}

			public void SetBursts(Burst[] bursts, int size)
			{
				SetBursts(m_ParticleSystem, bursts, size);
			}

			public int GetBursts(Burst[] bursts)
			{
				return GetBursts(m_ParticleSystem, bursts);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetBurstCount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRateOverTime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRateOverTime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRateOverTimeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRateOverTimeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRateOverDistance(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRateOverDistance(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRateOverDistanceMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRateOverDistanceMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetBursts(ParticleSystem system, Burst[] bursts, int size);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetBursts(ParticleSystem system, Burst[] bursts);
		}

		public struct ShapeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public ParticleSystemShapeType shapeType
			{
				get
				{
					return (ParticleSystemShapeType)GetShapeType(m_ParticleSystem);
				}
				set
				{
					SetShapeType(m_ParticleSystem, (int)value);
				}
			}

			public float randomDirectionAmount
			{
				get
				{
					return GetRandomDirectionAmount(m_ParticleSystem);
				}
				set
				{
					SetRandomDirectionAmount(m_ParticleSystem, value);
				}
			}

			public float sphericalDirectionAmount
			{
				get
				{
					return GetSphericalDirectionAmount(m_ParticleSystem);
				}
				set
				{
					SetSphericalDirectionAmount(m_ParticleSystem, value);
				}
			}

			public bool alignToDirection
			{
				get
				{
					return GetAlignToDirection(m_ParticleSystem);
				}
				set
				{
					SetAlignToDirection(m_ParticleSystem, value);
				}
			}

			public float radius
			{
				get
				{
					return GetRadius(m_ParticleSystem);
				}
				set
				{
					SetRadius(m_ParticleSystem, value);
				}
			}

			public ParticleSystemShapeMultiModeValue radiusMode
			{
				get
				{
					return (ParticleSystemShapeMultiModeValue)GetRadiusMode(m_ParticleSystem);
				}
				set
				{
					SetRadiusMode(m_ParticleSystem, (int)value);
				}
			}

			public float radiusSpread
			{
				get
				{
					return GetRadiusSpread(m_ParticleSystem);
				}
				set
				{
					SetRadiusSpread(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve radiusSpeed
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRadiusSpeed(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRadiusSpeed(m_ParticleSystem, ref value);
				}
			}

			public float radiusSpeedMultiplier
			{
				get
				{
					return GetRadiusSpeedMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRadiusSpeedMultiplier(m_ParticleSystem, value);
				}
			}

			public float angle
			{
				get
				{
					return GetAngle(m_ParticleSystem);
				}
				set
				{
					SetAngle(m_ParticleSystem, value);
				}
			}

			public float length
			{
				get
				{
					return GetLength(m_ParticleSystem);
				}
				set
				{
					SetLength(m_ParticleSystem, value);
				}
			}

			public Vector3 box
			{
				get
				{
					return GetBox(m_ParticleSystem);
				}
				set
				{
					SetBox(m_ParticleSystem, value);
				}
			}

			public ParticleSystemMeshShapeType meshShapeType
			{
				get
				{
					return (ParticleSystemMeshShapeType)GetMeshShapeType(m_ParticleSystem);
				}
				set
				{
					SetMeshShapeType(m_ParticleSystem, (int)value);
				}
			}

			public Mesh mesh
			{
				get
				{
					return GetMesh(m_ParticleSystem);
				}
				set
				{
					SetMesh(m_ParticleSystem, value);
				}
			}

			public MeshRenderer meshRenderer
			{
				get
				{
					return GetMeshRenderer(m_ParticleSystem);
				}
				set
				{
					SetMeshRenderer(m_ParticleSystem, value);
				}
			}

			public SkinnedMeshRenderer skinnedMeshRenderer
			{
				get
				{
					return GetSkinnedMeshRenderer(m_ParticleSystem);
				}
				set
				{
					SetSkinnedMeshRenderer(m_ParticleSystem, value);
				}
			}

			public bool useMeshMaterialIndex
			{
				get
				{
					return GetUseMeshMaterialIndex(m_ParticleSystem);
				}
				set
				{
					SetUseMeshMaterialIndex(m_ParticleSystem, value);
				}
			}

			public int meshMaterialIndex
			{
				get
				{
					return GetMeshMaterialIndex(m_ParticleSystem);
				}
				set
				{
					SetMeshMaterialIndex(m_ParticleSystem, value);
				}
			}

			public bool useMeshColors
			{
				get
				{
					return GetUseMeshColors(m_ParticleSystem);
				}
				set
				{
					SetUseMeshColors(m_ParticleSystem, value);
				}
			}

			public float normalOffset
			{
				get
				{
					return GetNormalOffset(m_ParticleSystem);
				}
				set
				{
					SetNormalOffset(m_ParticleSystem, value);
				}
			}

			public float meshScale
			{
				get
				{
					return GetMeshScale(m_ParticleSystem);
				}
				set
				{
					SetMeshScale(m_ParticleSystem, value);
				}
			}

			public float arc
			{
				get
				{
					return GetArc(m_ParticleSystem);
				}
				set
				{
					SetArc(m_ParticleSystem, value);
				}
			}

			public ParticleSystemShapeMultiModeValue arcMode
			{
				get
				{
					return (ParticleSystemShapeMultiModeValue)GetArcMode(m_ParticleSystem);
				}
				set
				{
					SetArcMode(m_ParticleSystem, (int)value);
				}
			}

			public float arcSpread
			{
				get
				{
					return GetArcSpread(m_ParticleSystem);
				}
				set
				{
					SetArcSpread(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve arcSpeed
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetArcSpeed(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetArcSpeed(m_ParticleSystem, ref value);
				}
			}

			public float arcSpeedMultiplier
			{
				get
				{
					return GetArcSpeedMultiplier(m_ParticleSystem);
				}
				set
				{
					SetArcSpeedMultiplier(m_ParticleSystem, value);
				}
			}

			[Obsolete("randomDirection property is deprecated. Use randomDirectionAmount instead.")]
			public bool randomDirection
			{
				get
				{
					return GetRandomDirectionAmount(m_ParticleSystem) >= 0.5f;
				}
				set
				{
					SetRandomDirectionAmount(m_ParticleSystem, 1f);
				}
			}

			internal ShapeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetShapeType(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetShapeType(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRandomDirectionAmount(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRandomDirectionAmount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSphericalDirectionAmount(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetSphericalDirectionAmount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetAlignToDirection(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetAlignToDirection(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadius(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRadius(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusMode(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetRadiusMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusSpread(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRadiusSpread(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRadiusSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusSpeedMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRadiusSpeedMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetAngle(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetAngle(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLength(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetLength(ParticleSystem system);

			private static void SetBox(ParticleSystem system, Vector3 value)
			{
				INTERNAL_CALL_SetBox(system, ref value);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_SetBox(ParticleSystem system, ref Vector3 value);

			private static Vector3 GetBox(ParticleSystem system)
			{
				INTERNAL_CALL_GetBox(system, out Vector3 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetBox(ParticleSystem system, out Vector3 value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMeshShapeType(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMeshShapeType(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMesh(ParticleSystem system, Mesh value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern Mesh GetMesh(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMeshRenderer(ParticleSystem system, MeshRenderer value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern MeshRenderer GetMeshRenderer(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSkinnedMeshRenderer(ParticleSystem system, SkinnedMeshRenderer value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern SkinnedMeshRenderer GetSkinnedMeshRenderer(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUseMeshMaterialIndex(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUseMeshMaterialIndex(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMeshMaterialIndex(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMeshMaterialIndex(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUseMeshColors(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUseMeshColors(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetNormalOffset(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetNormalOffset(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMeshScale(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMeshScale(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetArc(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetArc(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetArcMode(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetArcMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetArcSpread(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetArcSpread(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetArcSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetArcSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetArcSpeedMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetArcSpeedMultiplier(ParticleSystem system);
		}

		public struct VelocityOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public ParticleSystemSimulationSpace space
			{
				get
				{
					return GetWorldSpace(m_ParticleSystem) ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
				}
				set
				{
					SetWorldSpace(m_ParticleSystem, value == ParticleSystemSimulationSpace.World);
				}
			}

			internal VelocityOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWorldSpace(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetWorldSpace(ParticleSystem system);
		}

		public struct LimitVelocityOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve limitX
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float limitXMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve limitY
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public float limitYMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve limitZ
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float limitZMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve limit
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetMagnitude(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetMagnitude(m_ParticleSystem, ref value);
				}
			}

			public float limitMultiplier
			{
				get
				{
					return GetMagnitudeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetMagnitudeMultiplier(m_ParticleSystem, value);
				}
			}

			public float dampen
			{
				get
				{
					return GetDampen(m_ParticleSystem);
				}
				set
				{
					SetDampen(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			public ParticleSystemSimulationSpace space
			{
				get
				{
					return GetWorldSpace(m_ParticleSystem) ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
				}
				set
				{
					SetWorldSpace(m_ParticleSystem, value == ParticleSystemSimulationSpace.World);
				}
			}

			internal LimitVelocityOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMagnitude(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetMagnitude(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMagnitudeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMagnitudeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDampen(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetDampen(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWorldSpace(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetWorldSpace(ParticleSystem system);
		}

		public struct InheritVelocityModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public ParticleSystemInheritVelocityMode mode
			{
				get
				{
					return (ParticleSystemInheritVelocityMode)GetMode(m_ParticleSystem);
				}
				set
				{
					SetMode(m_ParticleSystem, (int)value);
				}
			}

			public MinMaxCurve curve
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetCurve(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetCurve(m_ParticleSystem, ref value);
				}
			}

			public float curveMultiplier
			{
				get
				{
					return GetCurveMultiplier(m_ParticleSystem);
				}
				set
				{
					SetCurveMultiplier(m_ParticleSystem, value);
				}
			}

			internal InheritVelocityModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMode(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCurve(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetCurve(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCurveMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetCurveMultiplier(ParticleSystem system);
		}

		public struct ForceOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public ParticleSystemSimulationSpace space
			{
				get
				{
					return GetWorldSpace(m_ParticleSystem) ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
				}
				set
				{
					SetWorldSpace(m_ParticleSystem, value == ParticleSystemSimulationSpace.World);
				}
			}

			public bool randomized
			{
				get
				{
					return GetRandomized(m_ParticleSystem);
				}
				set
				{
					SetRandomized(m_ParticleSystem, value);
				}
			}

			internal ForceOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWorldSpace(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetWorldSpace(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRandomized(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetRandomized(ParticleSystem system);
		}

		public struct ColorOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxGradient color
			{
				get
				{
					MinMaxGradient gradient = default(MinMaxGradient);
					GetColor(m_ParticleSystem, ref gradient);
					return gradient;
				}
				set
				{
					SetColor(m_ParticleSystem, ref value);
				}
			}

			internal ColorOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetColor(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetColor(ParticleSystem system, ref MinMaxGradient gradient);
		}

		public struct ColorBySpeedModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxGradient color
			{
				get
				{
					MinMaxGradient gradient = default(MinMaxGradient);
					GetColor(m_ParticleSystem, ref gradient);
					return gradient;
				}
				set
				{
					SetColor(m_ParticleSystem, ref value);
				}
			}

			public Vector2 range
			{
				get
				{
					return GetRange(m_ParticleSystem);
				}
				set
				{
					SetRange(m_ParticleSystem, value);
				}
			}

			internal ColorBySpeedModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetColor(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetColor(ParticleSystem system, ref MinMaxGradient gradient);

			private static void SetRange(ParticleSystem system, Vector2 value)
			{
				INTERNAL_CALL_SetRange(system, ref value);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_SetRange(ParticleSystem system, ref Vector2 value);

			private static Vector2 GetRange(ParticleSystem system)
			{
				INTERNAL_CALL_GetRange(system, out Vector2 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetRange(ParticleSystem system, out Vector2 value);
		}

		public struct SizeOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve size
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float sizeMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			internal SizeOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);
		}

		public struct SizeBySpeedModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve size
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float sizeMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			public Vector2 range
			{
				get
				{
					return GetRange(m_ParticleSystem);
				}
				set
				{
					SetRange(m_ParticleSystem, value);
				}
			}

			internal SizeBySpeedModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);

			private static void SetRange(ParticleSystem system, Vector2 value)
			{
				INTERNAL_CALL_SetRange(system, ref value);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_SetRange(ParticleSystem system, ref Vector2 value);

			private static Vector2 GetRange(ParticleSystem system)
			{
				INTERNAL_CALL_GetRange(system, out Vector2 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetRange(ParticleSystem system, out Vector2 value);
		}

		public struct RotationOverLifetimeModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			internal RotationOverLifetimeModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);
		}

		public struct RotationBySpeedModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve x
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetX(m_ParticleSystem, ref value);
				}
			}

			public float xMultiplier
			{
				get
				{
					return GetXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve y
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetY(m_ParticleSystem, ref value);
				}
			}

			public float yMultiplier
			{
				get
				{
					return GetYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve z
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetZ(m_ParticleSystem, ref value);
				}
			}

			public float zMultiplier
			{
				get
				{
					return GetZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetZMultiplier(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			public Vector2 range
			{
				get
				{
					return GetRange(m_ParticleSystem);
				}
				set
				{
					SetRange(m_ParticleSystem, value);
				}
			}

			internal RotationBySpeedModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);

			private static void SetRange(ParticleSystem system, Vector2 value)
			{
				INTERNAL_CALL_SetRange(system, ref value);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_SetRange(ParticleSystem system, ref Vector2 value);

			private static Vector2 GetRange(ParticleSystem system)
			{
				INTERNAL_CALL_GetRange(system, out Vector2 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetRange(ParticleSystem system, out Vector2 value);
		}

		public struct ExternalForcesModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public float multiplier
			{
				get
				{
					return GetMultiplier(m_ParticleSystem);
				}
				set
				{
					SetMultiplier(m_ParticleSystem, value);
				}
			}

			internal ExternalForcesModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMultiplier(ParticleSystem system);
		}

		public struct NoiseModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public bool separateAxes
			{
				get
				{
					return GetSeparateAxes(m_ParticleSystem);
				}
				set
				{
					SetSeparateAxes(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve strength
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStrengthX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStrengthX(m_ParticleSystem, ref value);
				}
			}

			public float strengthMultiplier
			{
				get
				{
					return GetStrengthXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStrengthXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve strengthX
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStrengthX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStrengthX(m_ParticleSystem, ref value);
				}
			}

			public float strengthXMultiplier
			{
				get
				{
					return GetStrengthXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStrengthXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve strengthY
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStrengthY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStrengthY(m_ParticleSystem, ref value);
				}
			}

			public float strengthYMultiplier
			{
				get
				{
					return GetStrengthYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStrengthYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve strengthZ
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStrengthZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStrengthZ(m_ParticleSystem, ref value);
				}
			}

			public float strengthZMultiplier
			{
				get
				{
					return GetStrengthZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStrengthZMultiplier(m_ParticleSystem, value);
				}
			}

			public float frequency
			{
				get
				{
					return GetFrequency(m_ParticleSystem);
				}
				set
				{
					SetFrequency(m_ParticleSystem, value);
				}
			}

			public bool damping
			{
				get
				{
					return GetDamping(m_ParticleSystem);
				}
				set
				{
					SetDamping(m_ParticleSystem, value);
				}
			}

			public int octaveCount
			{
				get
				{
					return GetOctaveCount(m_ParticleSystem);
				}
				set
				{
					SetOctaveCount(m_ParticleSystem, value);
				}
			}

			public float octaveMultiplier
			{
				get
				{
					return GetOctaveMultiplier(m_ParticleSystem);
				}
				set
				{
					SetOctaveMultiplier(m_ParticleSystem, value);
				}
			}

			public float octaveScale
			{
				get
				{
					return GetOctaveScale(m_ParticleSystem);
				}
				set
				{
					SetOctaveScale(m_ParticleSystem, value);
				}
			}

			public ParticleSystemNoiseQuality quality
			{
				get
				{
					return (ParticleSystemNoiseQuality)GetQuality(m_ParticleSystem);
				}
				set
				{
					SetQuality(m_ParticleSystem, (int)value);
				}
			}

			public MinMaxCurve scrollSpeed
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetScrollSpeed(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetScrollSpeed(m_ParticleSystem, ref value);
				}
			}

			public float scrollSpeedMultiplier
			{
				get
				{
					return GetScrollSpeedMultiplier(m_ParticleSystem);
				}
				set
				{
					SetScrollSpeedMultiplier(m_ParticleSystem, value);
				}
			}

			public bool remapEnabled
			{
				get
				{
					return GetRemapEnabled(m_ParticleSystem);
				}
				set
				{
					SetRemapEnabled(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve remap
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRemapX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRemapX(m_ParticleSystem, ref value);
				}
			}

			public float remapMultiplier
			{
				get
				{
					return GetRemapXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRemapXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve remapX
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRemapX(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRemapX(m_ParticleSystem, ref value);
				}
			}

			public float remapXMultiplier
			{
				get
				{
					return GetRemapXMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRemapXMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve remapY
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRemapY(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRemapY(m_ParticleSystem, ref value);
				}
			}

			public float remapYMultiplier
			{
				get
				{
					return GetRemapYMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRemapYMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve remapZ
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRemapZ(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRemapZ(m_ParticleSystem, ref value);
				}
			}

			public float remapZMultiplier
			{
				get
				{
					return GetRemapZMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRemapZMultiplier(m_ParticleSystem, value);
				}
			}

			internal NoiseModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSeparateAxes(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSeparateAxes(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStrengthX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStrengthY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStrengthZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStrengthXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStrengthYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStrengthZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStrengthZMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetFrequency(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetFrequency(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDamping(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetDamping(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetOctaveCount(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetOctaveCount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetOctaveMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetOctaveMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetOctaveScale(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetOctaveScale(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetQuality(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetQuality(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetScrollSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetScrollSpeed(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetScrollSpeedMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetScrollSpeedMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetRemapEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRemapX(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRemapY(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRemapZ(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapXMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRemapXMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapYMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRemapYMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRemapZMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRemapZMultiplier(ParticleSystem system);
		}

		public struct CollisionModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public ParticleSystemCollisionType type
			{
				get
				{
					return (ParticleSystemCollisionType)GetType(m_ParticleSystem);
				}
				set
				{
					SetType(m_ParticleSystem, (int)value);
				}
			}

			public ParticleSystemCollisionMode mode
			{
				get
				{
					return (ParticleSystemCollisionMode)GetMode(m_ParticleSystem);
				}
				set
				{
					SetMode(m_ParticleSystem, (int)value);
				}
			}

			public MinMaxCurve dampen
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetDampen(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetDampen(m_ParticleSystem, ref value);
				}
			}

			public float dampenMultiplier
			{
				get
				{
					return GetDampenMultiplier(m_ParticleSystem);
				}
				set
				{
					SetDampenMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve bounce
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetBounce(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetBounce(m_ParticleSystem, ref value);
				}
			}

			public float bounceMultiplier
			{
				get
				{
					return GetBounceMultiplier(m_ParticleSystem);
				}
				set
				{
					SetBounceMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve lifetimeLoss
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetLifetimeLoss(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetLifetimeLoss(m_ParticleSystem, ref value);
				}
			}

			public float lifetimeLossMultiplier
			{
				get
				{
					return GetLifetimeLossMultiplier(m_ParticleSystem);
				}
				set
				{
					SetLifetimeLossMultiplier(m_ParticleSystem, value);
				}
			}

			public float minKillSpeed
			{
				get
				{
					return GetMinKillSpeed(m_ParticleSystem);
				}
				set
				{
					SetMinKillSpeed(m_ParticleSystem, value);
				}
			}

			public float maxKillSpeed
			{
				get
				{
					return GetMaxKillSpeed(m_ParticleSystem);
				}
				set
				{
					SetMaxKillSpeed(m_ParticleSystem, value);
				}
			}

			public LayerMask collidesWith
			{
				get
				{
					return GetCollidesWith(m_ParticleSystem);
				}
				set
				{
					SetCollidesWith(m_ParticleSystem, value);
				}
			}

			public bool enableDynamicColliders
			{
				get
				{
					return GetEnableDynamicColliders(m_ParticleSystem);
				}
				set
				{
					SetEnableDynamicColliders(m_ParticleSystem, value);
				}
			}

			public bool enableInteriorCollisions
			{
				get
				{
					return GetEnableInteriorCollisions(m_ParticleSystem);
				}
				set
				{
					SetEnableInteriorCollisions(m_ParticleSystem, value);
				}
			}

			public int maxCollisionShapes
			{
				get
				{
					return GetMaxCollisionShapes(m_ParticleSystem);
				}
				set
				{
					SetMaxCollisionShapes(m_ParticleSystem, value);
				}
			}

			public ParticleSystemCollisionQuality quality
			{
				get
				{
					return (ParticleSystemCollisionQuality)GetQuality(m_ParticleSystem);
				}
				set
				{
					SetQuality(m_ParticleSystem, (int)value);
				}
			}

			public float voxelSize
			{
				get
				{
					return GetVoxelSize(m_ParticleSystem);
				}
				set
				{
					SetVoxelSize(m_ParticleSystem, value);
				}
			}

			public float radiusScale
			{
				get
				{
					return GetRadiusScale(m_ParticleSystem);
				}
				set
				{
					SetRadiusScale(m_ParticleSystem, value);
				}
			}

			public bool sendCollisionMessages
			{
				get
				{
					return GetUsesCollisionMessages(m_ParticleSystem);
				}
				set
				{
					SetUsesCollisionMessages(m_ParticleSystem, value);
				}
			}

			public int maxPlaneCount => GetMaxPlaneCount(m_ParticleSystem);

			internal CollisionModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			public void SetPlane(int index, Transform transform)
			{
				SetPlane(m_ParticleSystem, index, transform);
			}

			public Transform GetPlane(int index)
			{
				return GetPlane(m_ParticleSystem, index);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetType(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetType(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMode(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDampen(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetDampen(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDampenMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetDampenMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetBounce(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetBounce(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetBounceMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetBounceMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLifetimeLoss(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetLifetimeLoss(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLifetimeLossMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetLifetimeLossMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMinKillSpeed(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMinKillSpeed(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMaxKillSpeed(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMaxKillSpeed(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCollidesWith(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetCollidesWith(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnableDynamicColliders(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnableDynamicColliders(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnableInteriorCollisions(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnableInteriorCollisions(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMaxCollisionShapes(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMaxCollisionShapes(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetQuality(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetQuality(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetVoxelSize(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetVoxelSize(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusScale(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRadiusScale(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUsesCollisionMessages(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUsesCollisionMessages(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetPlane(ParticleSystem system, int index, Transform transform);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern Transform GetPlane(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMaxPlaneCount(ParticleSystem system);
		}

		public struct TriggerModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public ParticleSystemOverlapAction inside
			{
				get
				{
					return (ParticleSystemOverlapAction)GetInside(m_ParticleSystem);
				}
				set
				{
					SetInside(m_ParticleSystem, (int)value);
				}
			}

			public ParticleSystemOverlapAction outside
			{
				get
				{
					return (ParticleSystemOverlapAction)GetOutside(m_ParticleSystem);
				}
				set
				{
					SetOutside(m_ParticleSystem, (int)value);
				}
			}

			public ParticleSystemOverlapAction enter
			{
				get
				{
					return (ParticleSystemOverlapAction)GetEnter(m_ParticleSystem);
				}
				set
				{
					SetEnter(m_ParticleSystem, (int)value);
				}
			}

			public ParticleSystemOverlapAction exit
			{
				get
				{
					return (ParticleSystemOverlapAction)GetExit(m_ParticleSystem);
				}
				set
				{
					SetExit(m_ParticleSystem, (int)value);
				}
			}

			public float radiusScale
			{
				get
				{
					return GetRadiusScale(m_ParticleSystem);
				}
				set
				{
					SetRadiusScale(m_ParticleSystem, value);
				}
			}

			public int maxColliderCount => GetMaxColliderCount(m_ParticleSystem);

			internal TriggerModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			public void SetCollider(int index, Component collider)
			{
				SetCollider(m_ParticleSystem, index, collider);
			}

			public Component GetCollider(int index)
			{
				return GetCollider(m_ParticleSystem, index);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetInside(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetInside(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetOutside(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetOutside(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnter(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetEnter(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetExit(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetExit(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRadiusScale(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRadiusScale(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCollider(ParticleSystem system, int index, Component collider);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern Component GetCollider(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMaxColliderCount(ParticleSystem system);
		}

		public struct SubEmittersModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public int subEmittersCount => GetSubEmittersCount(m_ParticleSystem);

			[Obsolete("birth0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem birth0
			{
				get
				{
					return GetBirth(m_ParticleSystem, 0);
				}
				set
				{
					SetBirth(m_ParticleSystem, 0, value);
				}
			}

			[Obsolete("birth1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem birth1
			{
				get
				{
					return GetBirth(m_ParticleSystem, 1);
				}
				set
				{
					SetBirth(m_ParticleSystem, 1, value);
				}
			}

			[Obsolete("collision0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem collision0
			{
				get
				{
					return GetCollision(m_ParticleSystem, 0);
				}
				set
				{
					SetCollision(m_ParticleSystem, 0, value);
				}
			}

			[Obsolete("collision1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem collision1
			{
				get
				{
					return GetCollision(m_ParticleSystem, 1);
				}
				set
				{
					SetCollision(m_ParticleSystem, 1, value);
				}
			}

			[Obsolete("death0 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem death0
			{
				get
				{
					return GetDeath(m_ParticleSystem, 0);
				}
				set
				{
					SetDeath(m_ParticleSystem, 0, value);
				}
			}

			[Obsolete("death1 property is deprecated. Use AddSubEmitter, RemoveSubEmitter, SetSubEmitterSystem and GetSubEmitterSystem instead.")]
			public ParticleSystem death1
			{
				get
				{
					return GetDeath(m_ParticleSystem, 1);
				}
				set
				{
					SetDeath(m_ParticleSystem, 1, value);
				}
			}

			internal SubEmittersModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			public void AddSubEmitter(ParticleSystem subEmitter, ParticleSystemSubEmitterType type, ParticleSystemSubEmitterProperties properties)
			{
				AddSubEmitter(m_ParticleSystem, subEmitter, (int)type, (int)properties);
			}

			public void RemoveSubEmitter(int index)
			{
				RemoveSubEmitter(m_ParticleSystem, index);
			}

			public void SetSubEmitterSystem(int index, ParticleSystem subEmitter)
			{
				SetSubEmitterSystem(m_ParticleSystem, index, subEmitter);
			}

			public void SetSubEmitterType(int index, ParticleSystemSubEmitterType type)
			{
				SetSubEmitterType(m_ParticleSystem, index, (int)type);
			}

			public void SetSubEmitterProperties(int index, ParticleSystemSubEmitterProperties properties)
			{
				SetSubEmitterProperties(m_ParticleSystem, index, (int)properties);
			}

			public ParticleSystem GetSubEmitterSystem(int index)
			{
				return GetSubEmitterSystem(m_ParticleSystem, index);
			}

			public ParticleSystemSubEmitterType GetSubEmitterType(int index)
			{
				return (ParticleSystemSubEmitterType)GetSubEmitterType(m_ParticleSystem, index);
			}

			public ParticleSystemSubEmitterProperties GetSubEmitterProperties(int index)
			{
				return (ParticleSystemSubEmitterProperties)GetSubEmitterProperties(m_ParticleSystem, index);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetSubEmittersCount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetBirth(ParticleSystem system, int index, ParticleSystem value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystem GetBirth(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCollision(ParticleSystem system, int index, ParticleSystem value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystem GetCollision(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDeath(ParticleSystem system, int index, ParticleSystem value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystem GetDeath(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void AddSubEmitter(ParticleSystem system, ParticleSystem subEmitter, int type, int properties);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void RemoveSubEmitter(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSubEmitterSystem(ParticleSystem system, int index, ParticleSystem subEmitter);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSubEmitterType(ParticleSystem system, int index, int type);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSubEmitterProperties(ParticleSystem system, int index, int properties);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern ParticleSystem GetSubEmitterSystem(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetSubEmitterType(ParticleSystem system, int index);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetSubEmitterProperties(ParticleSystem system, int index);
		}

		public struct TextureSheetAnimationModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public int numTilesX
			{
				get
				{
					return GetNumTilesX(m_ParticleSystem);
				}
				set
				{
					SetNumTilesX(m_ParticleSystem, value);
				}
			}

			public int numTilesY
			{
				get
				{
					return GetNumTilesY(m_ParticleSystem);
				}
				set
				{
					SetNumTilesY(m_ParticleSystem, value);
				}
			}

			public ParticleSystemAnimationType animation
			{
				get
				{
					return (ParticleSystemAnimationType)GetAnimationType(m_ParticleSystem);
				}
				set
				{
					SetAnimationType(m_ParticleSystem, (int)value);
				}
			}

			public bool useRandomRow
			{
				get
				{
					return GetUseRandomRow(m_ParticleSystem);
				}
				set
				{
					SetUseRandomRow(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve frameOverTime
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetFrameOverTime(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetFrameOverTime(m_ParticleSystem, ref value);
				}
			}

			public float frameOverTimeMultiplier
			{
				get
				{
					return GetFrameOverTimeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetFrameOverTimeMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve startFrame
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetStartFrame(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetStartFrame(m_ParticleSystem, ref value);
				}
			}

			public float startFrameMultiplier
			{
				get
				{
					return GetStartFrameMultiplier(m_ParticleSystem);
				}
				set
				{
					SetStartFrameMultiplier(m_ParticleSystem, value);
				}
			}

			public int cycleCount
			{
				get
				{
					return GetCycleCount(m_ParticleSystem);
				}
				set
				{
					SetCycleCount(m_ParticleSystem, value);
				}
			}

			public int rowIndex
			{
				get
				{
					return GetRowIndex(m_ParticleSystem);
				}
				set
				{
					SetRowIndex(m_ParticleSystem, value);
				}
			}

			public UVChannelFlags uvChannelMask
			{
				get
				{
					return (UVChannelFlags)GetUVChannelMask(m_ParticleSystem);
				}
				set
				{
					SetUVChannelMask(m_ParticleSystem, (int)value);
				}
			}

			public float flipU
			{
				get
				{
					return GetFlipU(m_ParticleSystem);
				}
				set
				{
					SetFlipU(m_ParticleSystem, value);
				}
			}

			public float flipV
			{
				get
				{
					return GetFlipV(m_ParticleSystem);
				}
				set
				{
					SetFlipV(m_ParticleSystem, value);
				}
			}

			internal TextureSheetAnimationModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetNumTilesX(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetNumTilesX(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetNumTilesY(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetNumTilesY(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetAnimationType(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetAnimationType(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUseRandomRow(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUseRandomRow(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetFrameOverTime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetFrameOverTime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetFrameOverTimeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetFrameOverTimeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartFrame(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetStartFrame(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetStartFrameMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetStartFrameMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetCycleCount(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetCycleCount(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRowIndex(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetRowIndex(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUVChannelMask(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetUVChannelMask(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetFlipU(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetFlipU(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetFlipV(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetFlipV(ParticleSystem system);
		}

		public struct LightsModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public float ratio
			{
				get
				{
					return GetRatio(m_ParticleSystem);
				}
				set
				{
					SetRatio(m_ParticleSystem, value);
				}
			}

			public bool useRandomDistribution
			{
				get
				{
					return GetUseRandomDistribution(m_ParticleSystem);
				}
				set
				{
					SetUseRandomDistribution(m_ParticleSystem, value);
				}
			}

			public Light light
			{
				get
				{
					return GetLightPrefab(m_ParticleSystem);
				}
				set
				{
					SetLightPrefab(m_ParticleSystem, value);
				}
			}

			public bool useParticleColor
			{
				get
				{
					return GetUseParticleColor(m_ParticleSystem);
				}
				set
				{
					SetUseParticleColor(m_ParticleSystem, value);
				}
			}

			public bool sizeAffectsRange
			{
				get
				{
					return GetSizeAffectsRange(m_ParticleSystem);
				}
				set
				{
					SetSizeAffectsRange(m_ParticleSystem, value);
				}
			}

			public bool alphaAffectsIntensity
			{
				get
				{
					return GetAlphaAffectsIntensity(m_ParticleSystem);
				}
				set
				{
					SetAlphaAffectsIntensity(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve range
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetRange(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetRange(m_ParticleSystem, ref value);
				}
			}

			public float rangeMultiplier
			{
				get
				{
					return GetRangeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetRangeMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve intensity
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetIntensity(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetIntensity(m_ParticleSystem, ref value);
				}
			}

			public float intensityMultiplier
			{
				get
				{
					return GetIntensityMultiplier(m_ParticleSystem);
				}
				set
				{
					SetIntensityMultiplier(m_ParticleSystem, value);
				}
			}

			public int maxLights
			{
				get
				{
					return GetMaxLights(m_ParticleSystem);
				}
				set
				{
					SetMaxLights(m_ParticleSystem, value);
				}
			}

			internal LightsModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRatio(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRatio(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUseRandomDistribution(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUseRandomDistribution(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLightPrefab(ParticleSystem system, Light value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern Light GetLightPrefab(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetUseParticleColor(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetUseParticleColor(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSizeAffectsRange(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSizeAffectsRange(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetAlphaAffectsIntensity(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetAlphaAffectsIntensity(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRange(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetRange(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRangeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRangeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetIntensity(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetIntensity(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetIntensityMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetIntensityMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMaxLights(ParticleSystem system, int value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMaxLights(ParticleSystem system);
		}

		public struct TrailModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			public float ratio
			{
				get
				{
					return GetRatio(m_ParticleSystem);
				}
				set
				{
					SetRatio(m_ParticleSystem, value);
				}
			}

			public MinMaxCurve lifetime
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetLifetime(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetLifetime(m_ParticleSystem, ref value);
				}
			}

			public float lifetimeMultiplier
			{
				get
				{
					return GetLifetimeMultiplier(m_ParticleSystem);
				}
				set
				{
					SetLifetimeMultiplier(m_ParticleSystem, value);
				}
			}

			public float minVertexDistance
			{
				get
				{
					return GetMinVertexDistance(m_ParticleSystem);
				}
				set
				{
					SetMinVertexDistance(m_ParticleSystem, value);
				}
			}

			public ParticleSystemTrailTextureMode textureMode
			{
				get
				{
					return (ParticleSystemTrailTextureMode)GetTextureMode(m_ParticleSystem);
				}
				set
				{
					SetTextureMode(m_ParticleSystem, (float)value);
				}
			}

			public bool worldSpace
			{
				get
				{
					return GetWorldSpace(m_ParticleSystem);
				}
				set
				{
					SetWorldSpace(m_ParticleSystem, value);
				}
			}

			public bool dieWithParticles
			{
				get
				{
					return GetDieWithParticles(m_ParticleSystem);
				}
				set
				{
					SetDieWithParticles(m_ParticleSystem, value);
				}
			}

			public bool sizeAffectsWidth
			{
				get
				{
					return GetSizeAffectsWidth(m_ParticleSystem);
				}
				set
				{
					SetSizeAffectsWidth(m_ParticleSystem, value);
				}
			}

			public bool sizeAffectsLifetime
			{
				get
				{
					return GetSizeAffectsLifetime(m_ParticleSystem);
				}
				set
				{
					SetSizeAffectsLifetime(m_ParticleSystem, value);
				}
			}

			public bool inheritParticleColor
			{
				get
				{
					return GetInheritParticleColor(m_ParticleSystem);
				}
				set
				{
					SetInheritParticleColor(m_ParticleSystem, value);
				}
			}

			public MinMaxGradient colorOverLifetime
			{
				get
				{
					MinMaxGradient gradient = default(MinMaxGradient);
					GetColorOverLifetime(m_ParticleSystem, ref gradient);
					return gradient;
				}
				set
				{
					SetColorOverLifetime(m_ParticleSystem, ref value);
				}
			}

			public MinMaxCurve widthOverTrail
			{
				get
				{
					MinMaxCurve curve = default(MinMaxCurve);
					GetWidthOverTrail(m_ParticleSystem, ref curve);
					return curve;
				}
				set
				{
					SetWidthOverTrail(m_ParticleSystem, ref value);
				}
			}

			public float widthOverTrailMultiplier
			{
				get
				{
					return GetWidthOverTrailMultiplier(m_ParticleSystem);
				}
				set
				{
					SetWidthOverTrailMultiplier(m_ParticleSystem, value);
				}
			}

			public MinMaxGradient colorOverTrail
			{
				get
				{
					MinMaxGradient gradient = default(MinMaxGradient);
					GetColorOverTrail(m_ParticleSystem, ref gradient);
					return gradient;
				}
				set
				{
					SetColorOverTrail(m_ParticleSystem, ref value);
				}
			}

			internal TrailModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetRatio(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetRatio(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLifetime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetLifetime(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetLifetimeMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetLifetimeMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMinVertexDistance(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetMinVertexDistance(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetTextureMode(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetTextureMode(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWorldSpace(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetWorldSpace(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetDieWithParticles(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetDieWithParticles(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSizeAffectsWidth(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSizeAffectsWidth(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetSizeAffectsLifetime(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetSizeAffectsLifetime(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetInheritParticleColor(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetInheritParticleColor(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetColorOverLifetime(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetColorOverLifetime(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWidthOverTrail(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetWidthOverTrail(ParticleSystem system, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetWidthOverTrailMultiplier(ParticleSystem system, float value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetWidthOverTrailMultiplier(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetColorOverTrail(ParticleSystem system, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetColorOverTrail(ParticleSystem system, ref MinMaxGradient gradient);
		}

		public struct CustomDataModule
		{
			private ParticleSystem m_ParticleSystem;

			public bool enabled
			{
				get
				{
					return GetEnabled(m_ParticleSystem);
				}
				set
				{
					SetEnabled(m_ParticleSystem, value);
				}
			}

			internal CustomDataModule(ParticleSystem particleSystem)
			{
				m_ParticleSystem = particleSystem;
			}

			public void SetMode(ParticleSystemCustomData stream, ParticleSystemCustomDataMode mode)
			{
				SetMode(m_ParticleSystem, (int)stream, (int)mode);
			}

			public ParticleSystemCustomDataMode GetMode(ParticleSystemCustomData stream)
			{
				return (ParticleSystemCustomDataMode)GetMode(m_ParticleSystem, (int)stream);
			}

			public void SetVectorComponentCount(ParticleSystemCustomData stream, int count)
			{
				SetVectorComponentCount(m_ParticleSystem, (int)stream, count);
			}

			public int GetVectorComponentCount(ParticleSystemCustomData stream)
			{
				return GetVectorComponentCount(m_ParticleSystem, (int)stream);
			}

			public void SetVector(ParticleSystemCustomData stream, int component, MinMaxCurve curve)
			{
				SetVector(m_ParticleSystem, (int)stream, component, ref curve);
			}

			public MinMaxCurve GetVector(ParticleSystemCustomData stream, int component)
			{
				MinMaxCurve curve = default(MinMaxCurve);
				GetVector(m_ParticleSystem, (int)stream, component, ref curve);
				return curve;
			}

			public void SetColor(ParticleSystemCustomData stream, MinMaxGradient gradient)
			{
				SetColor(m_ParticleSystem, (int)stream, ref gradient);
			}

			public MinMaxGradient GetColor(ParticleSystemCustomData stream)
			{
				MinMaxGradient gradient = default(MinMaxGradient);
				GetColor(m_ParticleSystem, (int)stream, ref gradient);
				return gradient;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetEnabled(ParticleSystem system, bool value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern bool GetEnabled(ParticleSystem system);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetMode(ParticleSystem system, int stream, int mode);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetVectorComponentCount(ParticleSystem system, int stream, int count);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetVector(ParticleSystem system, int stream, int component, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void SetColor(ParticleSystem system, int stream, ref MinMaxGradient gradient);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetMode(ParticleSystem system, int stream);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern int GetVectorComponentCount(ParticleSystem system, int stream);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetVector(ParticleSystem system, int stream, int component, ref MinMaxCurve curve);

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void GetColor(ParticleSystem system, int stream, ref MinMaxGradient gradient);
		}

		[RequiredByNativeCode("particleSystemParticle", Optional = true)]
		public struct Particle
		{
			private Vector3 m_Position;

			private Vector3 m_Velocity;

			private Vector3 m_AnimatedVelocity;

			private Vector3 m_InitialVelocity;

			private Vector3 m_AxisOfRotation;

			private Vector3 m_Rotation;

			private Vector3 m_AngularVelocity;

			private Vector3 m_StartSize;

			private Color32 m_StartColor;

			private uint m_RandomSeed;

			private float m_Lifetime;

			private float m_StartLifetime;

			private float m_EmitAccumulator0;

			private float m_EmitAccumulator1;

			public Vector3 position
			{
				get
				{
					return m_Position;
				}
				set
				{
					m_Position = value;
				}
			}

			public Vector3 velocity
			{
				get
				{
					return m_Velocity;
				}
				set
				{
					m_Velocity = value;
				}
			}

			[Obsolete("Please use Particle.remainingLifetime instead. (UnityUpgradable) -> UnityEngine.ParticleSystem/Particle.remainingLifetime")]
			public float lifetime
			{
				get
				{
					return m_Lifetime;
				}
				set
				{
					m_Lifetime = value;
				}
			}

			public float remainingLifetime
			{
				get
				{
					return m_Lifetime;
				}
				set
				{
					m_Lifetime = value;
				}
			}

			public float startLifetime
			{
				get
				{
					return m_StartLifetime;
				}
				set
				{
					m_StartLifetime = value;
				}
			}

			public float startSize
			{
				get
				{
					return m_StartSize.x;
				}
				set
				{
					m_StartSize = new Vector3(value, value, value);
				}
			}

			public Vector3 startSize3D
			{
				get
				{
					return m_StartSize;
				}
				set
				{
					m_StartSize = value;
				}
			}

			public Vector3 axisOfRotation
			{
				get
				{
					return m_AxisOfRotation;
				}
				set
				{
					m_AxisOfRotation = value;
				}
			}

			public float rotation
			{
				get
				{
					return m_Rotation.z * 57.29578f;
				}
				set
				{
					m_Rotation = new Vector3(0f, 0f, value * ((float)Math.PI / 180f));
				}
			}

			public Vector3 rotation3D
			{
				get
				{
					return m_Rotation * 57.29578f;
				}
				set
				{
					m_Rotation = value * ((float)Math.PI / 180f);
				}
			}

			public float angularVelocity
			{
				get
				{
					return m_AngularVelocity.z * 57.29578f;
				}
				set
				{
					m_AngularVelocity.z = value * ((float)Math.PI / 180f);
				}
			}

			public Vector3 angularVelocity3D
			{
				get
				{
					return m_AngularVelocity * 57.29578f;
				}
				set
				{
					m_AngularVelocity = value * ((float)Math.PI / 180f);
				}
			}

			public Color32 startColor
			{
				get
				{
					return m_StartColor;
				}
				set
				{
					m_StartColor = value;
				}
			}

			[Obsolete("randomValue property is deprecated. Use randomSeed instead to control random behavior of particles.")]
			public float randomValue
			{
				get
				{
					return BitConverter.ToSingle(BitConverter.GetBytes(m_RandomSeed), 0);
				}
				set
				{
					m_RandomSeed = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
				}
			}

			public uint randomSeed
			{
				get
				{
					return m_RandomSeed;
				}
				set
				{
					m_RandomSeed = value;
				}
			}

			[Obsolete("size property is deprecated. Use startSize or GetCurrentSize() instead.")]
			public float size
			{
				get
				{
					return m_StartSize.x;
				}
				set
				{
					m_StartSize = new Vector3(value, value, value);
				}
			}

			[Obsolete("color property is deprecated. Use startColor or GetCurrentColor() instead.")]
			public Color32 color
			{
				get
				{
					return m_StartColor;
				}
				set
				{
					m_StartColor = value;
				}
			}

			public float GetCurrentSize(ParticleSystem system)
			{
				return GetCurrentSize(system, ref this);
			}

			public Vector3 GetCurrentSize3D(ParticleSystem system)
			{
				return GetCurrentSize3D(system, ref this);
			}

			public Color32 GetCurrentColor(ParticleSystem system)
			{
				return GetCurrentColor(system, ref this);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern float GetCurrentSize(ParticleSystem system, ref Particle particle);

			private static Vector3 GetCurrentSize3D(ParticleSystem system, ref Particle particle)
			{
				INTERNAL_CALL_GetCurrentSize3D(system, ref particle, out Vector3 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetCurrentSize3D(ParticleSystem system, ref Particle particle, out Vector3 value);

			private static Color32 GetCurrentColor(ParticleSystem system, ref Particle particle)
			{
				INTERNAL_CALL_GetCurrentColor(system, ref particle, out Color32 value);
				return value;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			private static extern void INTERNAL_CALL_GetCurrentColor(ParticleSystem system, ref Particle particle, out Color32 value);
		}

		public struct EmitParams
		{
			internal Particle m_Particle;

			internal bool m_PositionSet;

			internal bool m_VelocitySet;

			internal bool m_AxisOfRotationSet;

			internal bool m_RotationSet;

			internal bool m_AngularVelocitySet;

			internal bool m_StartSizeSet;

			internal bool m_StartColorSet;

			internal bool m_RandomSeedSet;

			internal bool m_StartLifetimeSet;

			internal bool m_ApplyShapeToPosition;

			public Vector3 position
			{
				get
				{
					return m_Particle.position;
				}
				set
				{
					m_Particle.position = value;
					m_PositionSet = true;
				}
			}

			public bool applyShapeToPosition
			{
				get
				{
					return m_ApplyShapeToPosition;
				}
				set
				{
					m_ApplyShapeToPosition = value;
				}
			}

			public Vector3 velocity
			{
				get
				{
					return m_Particle.velocity;
				}
				set
				{
					m_Particle.velocity = value;
					m_VelocitySet = true;
				}
			}

			public float startLifetime
			{
				get
				{
					return m_Particle.startLifetime;
				}
				set
				{
					m_Particle.startLifetime = value;
					m_StartLifetimeSet = true;
				}
			}

			public float startSize
			{
				get
				{
					return m_Particle.startSize;
				}
				set
				{
					m_Particle.startSize = value;
					m_StartSizeSet = true;
				}
			}

			public Vector3 startSize3D
			{
				get
				{
					return m_Particle.startSize3D;
				}
				set
				{
					m_Particle.startSize3D = value;
					m_StartSizeSet = true;
				}
			}

			public Vector3 axisOfRotation
			{
				get
				{
					return m_Particle.axisOfRotation;
				}
				set
				{
					m_Particle.axisOfRotation = value;
					m_AxisOfRotationSet = true;
				}
			}

			public float rotation
			{
				get
				{
					return m_Particle.rotation;
				}
				set
				{
					m_Particle.rotation = value;
					m_RotationSet = true;
				}
			}

			public Vector3 rotation3D
			{
				get
				{
					return m_Particle.rotation3D;
				}
				set
				{
					m_Particle.rotation3D = value;
					m_RotationSet = true;
				}
			}

			public float angularVelocity
			{
				get
				{
					return m_Particle.angularVelocity;
				}
				set
				{
					m_Particle.angularVelocity = value;
					m_AngularVelocitySet = true;
				}
			}

			public Vector3 angularVelocity3D
			{
				get
				{
					return m_Particle.angularVelocity3D;
				}
				set
				{
					m_Particle.angularVelocity3D = value;
					m_AngularVelocitySet = true;
				}
			}

			public Color32 startColor
			{
				get
				{
					return m_Particle.startColor;
				}
				set
				{
					m_Particle.startColor = value;
					m_StartColorSet = true;
				}
			}

			public uint randomSeed
			{
				get
				{
					return m_Particle.randomSeed;
				}
				set
				{
					m_Particle.randomSeed = value;
					m_RandomSeedSet = true;
				}
			}

			public void ResetPosition()
			{
				m_PositionSet = false;
			}

			public void ResetVelocity()
			{
				m_VelocitySet = false;
			}

			public void ResetAxisOfRotation()
			{
				m_AxisOfRotationSet = false;
			}

			public void ResetRotation()
			{
				m_RotationSet = false;
			}

			public void ResetAngularVelocity()
			{
				m_AngularVelocitySet = false;
			}

			public void ResetStartSize()
			{
				m_StartSizeSet = false;
			}

			public void ResetStartColor()
			{
				m_StartColorSet = false;
			}

			public void ResetRandomSeed()
			{
				m_RandomSeedSet = false;
			}

			public void ResetStartLifetime()
			{
				m_StartLifetimeSet = false;
			}
		}

		internal delegate bool IteratorDelegate(ParticleSystem ps);

		[Obsolete("startDelay property is deprecated. Use main.startDelay or main.startDelayMultiplier instead.")]
		public float startDelay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool isPlaying
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isEmitting
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isStopped
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isPaused
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("loop property is deprecated. Use main.loop instead.")]
		public bool loop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("playOnAwake property is deprecated. Use main.playOnAwake instead.")]
		public bool playOnAwake
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("duration property is deprecated. Use main.duration instead.")]
		public float duration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("playbackSpeed property is deprecated. Use main.simulationSpeed instead.")]
		public float playbackSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int particleCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("enableEmission property is deprecated. Use emission.enabled instead.")]
		public bool enableEmission
		{
			get
			{
				return emission.enabled;
			}
			set
			{
				emission.enabled = value;
			}
		}

		[Obsolete("emissionRate property is deprecated. Use emission.rateOverTime, emission.rateOverDistance, emission.rateOverTimeMultiplier or emission.rateOverDistanceMultiplier instead.")]
		public float emissionRate
		{
			get
			{
				return emission.rateOverTimeMultiplier;
			}
			set
			{
				emission.rateOverTime = value;
			}
		}

		[Obsolete("startSpeed property is deprecated. Use main.startSpeed or main.startSpeedMultiplier instead.")]
		public float startSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("startSize property is deprecated. Use main.startSize or main.startSizeMultiplier instead.")]
		public float startSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("startColor property is deprecated. Use main.startColor instead.")]
		public Color startColor
		{
			get
			{
				INTERNAL_get_startColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_startColor(ref value);
			}
		}

		[Obsolete("startRotation property is deprecated. Use main.startRotation or main.startRotationMultiplier instead.")]
		public float startRotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("startRotation3D property is deprecated. Use main.startRotationX, main.startRotationY and main.startRotationZ instead. (Or main.startRotationXMultiplier, main.startRotationYMultiplier and main.startRotationZMultiplier).")]
		public Vector3 startRotation3D
		{
			get
			{
				INTERNAL_get_startRotation3D(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_startRotation3D(ref value);
			}
		}

		[Obsolete("startLifetime property is deprecated. Use main.startLifetime or main.startLifetimeMultiplier instead.")]
		public float startLifetime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("gravityModifier property is deprecated. Use main.gravityModifier or main.gravityModifierMultiplier instead.")]
		public float gravityModifier
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("maxParticles property is deprecated. Use main.maxParticles instead.")]
		public int maxParticles
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("simulationSpace property is deprecated. Use main.simulationSpace instead.")]
		public ParticleSystemSimulationSpace simulationSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("scalingMode property is deprecated. Use main.scalingMode instead.")]
		public ParticleSystemScalingMode scalingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public uint randomSeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool useAutoRandomSeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public MainModule main => new MainModule(this);

		public EmissionModule emission => new EmissionModule(this);

		public ShapeModule shape => new ShapeModule(this);

		public VelocityOverLifetimeModule velocityOverLifetime => new VelocityOverLifetimeModule(this);

		public LimitVelocityOverLifetimeModule limitVelocityOverLifetime => new LimitVelocityOverLifetimeModule(this);

		public InheritVelocityModule inheritVelocity => new InheritVelocityModule(this);

		public ForceOverLifetimeModule forceOverLifetime => new ForceOverLifetimeModule(this);

		public ColorOverLifetimeModule colorOverLifetime => new ColorOverLifetimeModule(this);

		public ColorBySpeedModule colorBySpeed => new ColorBySpeedModule(this);

		public SizeOverLifetimeModule sizeOverLifetime => new SizeOverLifetimeModule(this);

		public SizeBySpeedModule sizeBySpeed => new SizeBySpeedModule(this);

		public RotationOverLifetimeModule rotationOverLifetime => new RotationOverLifetimeModule(this);

		public RotationBySpeedModule rotationBySpeed => new RotationBySpeedModule(this);

		public ExternalForcesModule externalForces => new ExternalForcesModule(this);

		public NoiseModule noise => new NoiseModule(this);

		public CollisionModule collision => new CollisionModule(this);

		public TriggerModule trigger => new TriggerModule(this);

		public SubEmittersModule subEmitters => new SubEmittersModule(this);

		public TextureSheetAnimationModule textureSheetAnimation => new TextureSheetAnimationModule(this);

		public LightsModule lights => new LightsModule(this);

		public TrailModule trails => new TrailModule(this);

		public CustomDataModule customData => new CustomDataModule(this);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_startColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_startColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_startRotation3D(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_startRotation3D(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetParticles(Particle[] particles, int size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetParticles(Particle[] particles);

		public void SetCustomParticleData(List<Vector4> customData, ParticleSystemCustomData streamIndex)
		{
			SetCustomParticleDataInternal(customData, (int)streamIndex);
		}

		public int GetCustomParticleData(List<Vector4> customData, ParticleSystemCustomData streamIndex)
		{
			return GetCustomParticleDataInternal(customData, (int)streamIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetCustomParticleDataInternal(object customData, int streamIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern int GetCustomParticleDataInternal(object customData, int streamIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_Simulate(ParticleSystem self, float t, bool restart, bool fixedTimeStep);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_Play(ParticleSystem self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_Stop(ParticleSystem self, ParticleSystemStopBehavior stopBehavior);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_Pause(ParticleSystem self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_Clear(ParticleSystem self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool Internal_IsAlive(ParticleSystem self);

		[ExcludeFromDocs]
		public void Simulate(float t, bool withChildren, bool restart)
		{
			bool fixedTimeStep = true;
			Simulate(t, withChildren, restart, fixedTimeStep);
		}

		[ExcludeFromDocs]
		public void Simulate(float t, bool withChildren)
		{
			bool fixedTimeStep = true;
			bool restart = true;
			Simulate(t, withChildren, restart, fixedTimeStep);
		}

		[ExcludeFromDocs]
		public void Simulate(float t)
		{
			bool fixedTimeStep = true;
			bool restart = true;
			bool withChildren = true;
			Simulate(t, withChildren, restart, fixedTimeStep);
		}

		public void Simulate(float t, [DefaultValue("true")] bool withChildren, [DefaultValue("true")] bool restart, [DefaultValue("true")] bool fixedTimeStep)
		{
			IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_Simulate(ps, t, restart, fixedTimeStep));
		}

		[ExcludeFromDocs]
		public void Play()
		{
			bool withChildren = true;
			Play(withChildren);
		}

		public void Play([DefaultValue("true")] bool withChildren)
		{
			IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_Play(ps));
		}

		[ExcludeFromDocs]
		public void Stop(bool withChildren)
		{
			ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting;
			Stop(withChildren, stopBehavior);
		}

		[ExcludeFromDocs]
		public void Stop()
		{
			ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting;
			bool withChildren = true;
			Stop(withChildren, stopBehavior);
		}

		public void Stop([DefaultValue("true")] bool withChildren, [DefaultValue("ParticleSystemStopBehavior.StopEmitting")] ParticleSystemStopBehavior stopBehavior)
		{
			IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_Stop(ps, stopBehavior));
		}

		[ExcludeFromDocs]
		public void Pause()
		{
			bool withChildren = true;
			Pause(withChildren);
		}

		public void Pause([DefaultValue("true")] bool withChildren)
		{
			IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_Pause(ps));
		}

		[ExcludeFromDocs]
		public void Clear()
		{
			bool withChildren = true;
			Clear(withChildren);
		}

		public void Clear([DefaultValue("true")] bool withChildren)
		{
			IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_Clear(ps));
		}

		[ExcludeFromDocs]
		public bool IsAlive()
		{
			bool withChildren = true;
			return IsAlive(withChildren);
		}

		public bool IsAlive([DefaultValue("true")] bool withChildren)
		{
			return IterateParticleSystems(withChildren, (ParticleSystem ps) => Internal_IsAlive(ps));
		}

		public void Emit(int count)
		{
			INTERNAL_CALL_Emit(this, count);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Emit(ParticleSystem self, int count);

		[Obsolete("Emit with specific parameters is deprecated. Pass a ParticleSystem.EmitParams parameter instead, which allows you to override some/all of the emission properties")]
		public void Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
		{
			Particle particle = default(Particle);
			particle.position = position;
			particle.velocity = velocity;
			particle.lifetime = lifetime;
			particle.startLifetime = lifetime;
			particle.startSize = size;
			particle.rotation3D = Vector3.zero;
			particle.angularVelocity3D = Vector3.zero;
			particle.startColor = color;
			particle.randomSeed = 5u;
			Internal_EmitOld(ref particle);
		}

		[Obsolete("Emit with a single particle structure is deprecated. Pass a ParticleSystem.EmitParams parameter instead, which allows you to override some/all of the emission properties")]
		public void Emit(Particle particle)
		{
			Internal_EmitOld(ref particle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_EmitOld(ref Particle particle);

		public void Emit(EmitParams emitParams, int count)
		{
			Internal_Emit(ref emitParams, count);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_Emit(ref EmitParams emitParams, int count);

		internal bool IterateParticleSystems(bool recurse, IteratorDelegate func)
		{
			bool flag = func(this);
			if (recurse)
			{
				flag |= IterateParticleSystemsRecursive(base.transform, func);
			}
			return flag;
		}

		private static bool IterateParticleSystemsRecursive(Transform transform, IteratorDelegate func)
		{
			bool flag = false;
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				ParticleSystem component = child.gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					flag = func(component);
					if (flag)
					{
						break;
					}
					IterateParticleSystemsRecursive(child, func);
				}
			}
			return flag;
		}
	}
}
