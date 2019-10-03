using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	public sealed class GraphicsSettings : Object
	{
		public static RenderPipelineAsset renderPipelineAsset
		{
			get
			{
				return INTERNAL_renderPipelineAsset as RenderPipelineAsset;
			}
			set
			{
				INTERNAL_renderPipelineAsset = value;
			}
		}

		private static ScriptableObject INTERNAL_renderPipelineAsset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static TransparencySortMode transparencySortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static Vector3 transparencySortAxis
		{
			get
			{
				INTERNAL_get_transparencySortAxis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_transparencySortAxis(ref value);
			}
		}

		public static bool lightsUseLinearIntensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool lightsUseColorTemperature
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetShaderMode(BuiltinShaderType type, BuiltinShaderMode mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern BuiltinShaderMode GetShaderMode(BuiltinShaderType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetCustomShader(BuiltinShaderType type, Shader shader);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern Shader GetCustomShader(BuiltinShaderType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern Object GetGraphicsSettings();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_transparencySortAxis(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_transparencySortAxis(ref Vector3 value);
	}
}
