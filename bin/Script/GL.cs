using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class GL
	{
		public const int TRIANGLES = 4;

		public const int TRIANGLE_STRIP = 5;

		public const int QUADS = 7;

		public const int LINES = 1;

		public const int LINE_STRIP = 2;

		public static Matrix4x4 modelview
		{
			get
			{
				INTERNAL_get_modelview(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_modelview(ref value);
			}
		}

		public static bool wireframe
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool sRGBWrite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool invertCulling
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
		public static extern void Vertex3(float x, float y, float z);

		public static void Vertex(Vector3 v)
		{
			INTERNAL_CALL_Vertex(ref v);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Vertex(ref Vector3 v);

		public static void Color(Color c)
		{
			INTERNAL_CALL_Color(ref c);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Color(ref Color c);

		public static void TexCoord(Vector3 v)
		{
			INTERNAL_CALL_TexCoord(ref v);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_TexCoord(ref Vector3 v);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void TexCoord2(float x, float y);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void TexCoord3(float x, float y, float z);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void MultiTexCoord2(int unit, float x, float y);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void MultiTexCoord3(int unit, float x, float y, float z);

		public static void MultiTexCoord(int unit, Vector3 v)
		{
			INTERNAL_CALL_MultiTexCoord(unit, ref v);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_MultiTexCoord(int unit, ref Vector3 v);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void BeginInternal(int mode);

		public static void Begin(int mode)
		{
			BeginInternal(mode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void End();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void PushMatrix();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void PopMatrix();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void LoadIdentity();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void LoadOrtho();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void LoadPixelMatrix();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void LoadPixelMatrixArgs(float left, float right, float bottom, float top);

		public static void LoadPixelMatrix(float left, float right, float bottom, float top)
		{
			LoadPixelMatrixArgs(left, right, bottom, top);
		}

		public static void Viewport(Rect pixelRect)
		{
			INTERNAL_CALL_Viewport(ref pixelRect);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Viewport(ref Rect pixelRect);

		public static void LoadProjectionMatrix(Matrix4x4 mat)
		{
			INTERNAL_CALL_LoadProjectionMatrix(ref mat);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_LoadProjectionMatrix(ref Matrix4x4 mat);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_get_modelview(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_set_modelview(ref Matrix4x4 value);

		public static void MultMatrix(Matrix4x4 mat)
		{
			INTERNAL_CALL_MultMatrix(ref mat);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_MultMatrix(ref Matrix4x4 mat);

		public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
		{
			INTERNAL_CALL_GetGPUProjectionMatrix(ref proj, renderIntoTexture, out Matrix4x4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetGPUProjectionMatrix(ref Matrix4x4 proj, bool renderIntoTexture, out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use invertCulling property")]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetRevertBackfacing(bool revertBackFaces);

		[ExcludeFromDocs]
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			float depth = 1f;
			Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			Internal_Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		private static void Internal_Clear(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			INTERNAL_CALL_Internal_Clear(clearDepth, clearColor, ref backgroundColor, depth);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_Clear(bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void ClearWithSkybox(bool clearDepth, Camera camera);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Flush();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void InvalidateState();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern void IssuePluginEvent(int eventID);

		public static void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			IssuePluginEventInternal(callback, eventID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void IssuePluginEventInternal(IntPtr callback, int eventID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void RenderTargetBarrier();
	}
}
