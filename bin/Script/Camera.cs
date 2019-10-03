using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[RequireComponent(typeof(Transform))]
	public sealed class Camera : Behaviour
	{
		public enum StereoscopicEye
		{
			Left,
			Right
		}

		public enum MonoOrStereoscopicEye
		{
			Left,
			Right,
			Mono
		}

		public delegate void CameraCallback(Camera cam);

		public static CameraCallback onPreCull;

		public static CameraCallback onPreRender;

		public static CameraCallback onPostRender;

		[Obsolete("use Camera.fieldOfView instead.")]
		public float fov
		{
			get
			{
				return fieldOfView;
			}
			set
			{
				fieldOfView = value;
			}
		}

		[Obsolete("use Camera.nearClipPlane instead.")]
		public float near
		{
			get
			{
				return nearClipPlane;
			}
			set
			{
				nearClipPlane = value;
			}
		}

		[Obsolete("use Camera.farClipPlane instead.")]
		public float far
		{
			get
			{
				return farClipPlane;
			}
			set
			{
				farClipPlane = value;
			}
		}

		public float fieldOfView
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float nearClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float farClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RenderingPath renderingPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RenderingPath actualRenderingPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool allowHDR
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("use Camera.allowHDR instead.")]
		public bool hdr
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool forceIntoRenderTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool allowMSAA
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float orthographicSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool orthographic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public OpaqueSortMode opaqueSortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public TransparencySortMode transparencySortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3 transparencySortAxis
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

		public float depth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float aspect
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int cullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		internal static int PreviewCullingLayer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int eventMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Color backgroundColor
		{
			get
			{
				INTERNAL_get_backgroundColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_backgroundColor(ref value);
			}
		}

		public Rect rect
		{
			get
			{
				INTERNAL_get_rect(out Rect value);
				return value;
			}
			set
			{
				INTERNAL_set_rect(ref value);
			}
		}

		public Rect pixelRect
		{
			get
			{
				INTERNAL_get_pixelRect(out Rect value);
				return value;
			}
			set
			{
				INTERNAL_set_pixelRect(ref value);
			}
		}

		public RenderTexture targetTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public RenderTexture activeTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int pixelWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int pixelHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Matrix4x4 cameraToWorldMatrix
		{
			get
			{
				INTERNAL_get_cameraToWorldMatrix(out Matrix4x4 value);
				return value;
			}
		}

		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				INTERNAL_get_worldToCameraMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_worldToCameraMatrix(ref value);
			}
		}

		public Matrix4x4 projectionMatrix
		{
			get
			{
				INTERNAL_get_projectionMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_projectionMatrix(ref value);
			}
		}

		public Matrix4x4 nonJitteredProjectionMatrix
		{
			get
			{
				INTERNAL_get_nonJitteredProjectionMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_nonJitteredProjectionMatrix(ref value);
			}
		}

		public bool useJitteredProjectionMatrixForTransparentRendering
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
		}

		public CameraClearFlags clearFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool stereoEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float stereoSeparation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float stereoConvergence
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public CameraType cameraType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool stereoMirrorMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public StereoTargetEyeMask stereoTargetEye
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public MonoOrStereoscopicEye stereoActiveEye
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int targetDisplay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static Camera main
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static Camera current
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static Camera[] allCameras
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static int allCamerasCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool useOcclusionCulling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Matrix4x4 cullingMatrix
		{
			get
			{
				INTERNAL_get_cullingMatrix(out Matrix4x4 value);
				return value;
			}
			set
			{
				INTERNAL_set_cullingMatrix(ref value);
			}
		}

		public float[] layerCullDistances
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool layerCullSpherical
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public DepthTextureMode depthTextureMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool clearStencilAfterLightingPass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int commandBufferCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern string[] GetCameraBufferWarnings();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_transparencySortAxis(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_transparencySortAxis(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_backgroundColor(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_backgroundColor(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_rect(out Rect value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_rect(ref Rect value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_pixelRect(out Rect value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_pixelRect(ref Rect value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTargetBuffersImpl(out RenderBuffer color, out RenderBuffer depth);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTargetBuffersMRTImpl(RenderBuffer[] color, out RenderBuffer depth);

		public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			SetTargetBuffersImpl(out colorBuffer, out depthBuffer);
		}

		public void SetTargetBuffers(RenderBuffer[] colorBuffer, RenderBuffer depthBuffer)
		{
			SetTargetBuffersMRTImpl(colorBuffer, out depthBuffer);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_cameraToWorldMatrix(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_worldToCameraMatrix(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_worldToCameraMatrix(ref Matrix4x4 value);

		public void ResetWorldToCameraMatrix()
		{
			INTERNAL_CALL_ResetWorldToCameraMatrix(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetWorldToCameraMatrix(Camera self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_projectionMatrix(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_projectionMatrix(ref Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_nonJitteredProjectionMatrix(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_nonJitteredProjectionMatrix(ref Matrix4x4 value);

		public void ResetProjectionMatrix()
		{
			INTERNAL_CALL_ResetProjectionMatrix(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetProjectionMatrix(Camera self);

		public void ResetAspect()
		{
			INTERNAL_CALL_ResetAspect(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetAspect(Camera self);

		[Obsolete("Camera.ResetFieldOfView has been deprecated in Unity 5.6 and will be removed in the future. Please replace it by explicitly setting the camera's FOV to 60 degrees.")]
		public void ResetFieldOfView()
		{
			INTERNAL_CALL_ResetFieldOfView(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetFieldOfView(Camera self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_velocity(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("GetStereoViewMatrices is deprecated. Use GetStereoViewMatrix(StereoscopicEye eye) instead.")]
		[GeneratedByOldBindingsGenerator]
		public extern Matrix4x4[] GetStereoViewMatrices();

		public Matrix4x4 GetStereoViewMatrix(StereoscopicEye eye)
		{
			INTERNAL_CALL_GetStereoViewMatrix(this, eye, out Matrix4x4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetStereoViewMatrix(Camera self, StereoscopicEye eye, out Matrix4x4 value);

		[Obsolete("SetStereoViewMatrices is deprecated. Use SetStereoViewMatrix(StereoscopicEye eye) instead.")]
		public void SetStereoViewMatrices(Matrix4x4 leftMatrix, Matrix4x4 rightMatrix)
		{
			INTERNAL_CALL_SetStereoViewMatrices(this, ref leftMatrix, ref rightMatrix);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetStereoViewMatrices(Camera self, ref Matrix4x4 leftMatrix, ref Matrix4x4 rightMatrix);

		public void SetStereoViewMatrix(StereoscopicEye eye, Matrix4x4 matrix)
		{
			INTERNAL_CALL_SetStereoViewMatrix(this, eye, ref matrix);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetStereoViewMatrix(Camera self, StereoscopicEye eye, ref Matrix4x4 matrix);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ResetStereoViewMatrices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("GetStereoProjectionMatrices is deprecated. Use GetStereoProjectionMatrix(StereoscopicEye eye) instead.")]
		[GeneratedByOldBindingsGenerator]
		public extern Matrix4x4[] GetStereoProjectionMatrices();

		public Matrix4x4 GetStereoProjectionMatrix(StereoscopicEye eye)
		{
			INTERNAL_CALL_GetStereoProjectionMatrix(this, eye, out Matrix4x4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetStereoProjectionMatrix(Camera self, StereoscopicEye eye, out Matrix4x4 value);

		public void SetStereoProjectionMatrix(StereoscopicEye eye, Matrix4x4 matrix)
		{
			INTERNAL_CALL_SetStereoProjectionMatrix(this, eye, ref matrix);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetStereoProjectionMatrix(Camera self, StereoscopicEye eye, ref Matrix4x4 matrix);

		[Obsolete("SetStereoProjectionMatrices is deprecated. Use SetStereoProjectionMatrix(StereoscopicEye eye) instead.")]
		public void SetStereoProjectionMatrices(Matrix4x4 leftMatrix, Matrix4x4 rightMatrix)
		{
			INTERNAL_CALL_SetStereoProjectionMatrices(this, ref leftMatrix, ref rightMatrix);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetStereoProjectionMatrices(Camera self, ref Matrix4x4 leftMatrix, ref Matrix4x4 rightMatrix);

		public void CalculateFrustumCorners(Rect viewport, float z, MonoOrStereoscopicEye eye, Vector3[] outCorners)
		{
			if (outCorners == null)
			{
				throw new ArgumentNullException("outCorners");
			}
			if (outCorners.Length < 4)
			{
				throw new ArgumentException("outCorners minimum size is 4", "outCorners");
			}
			CalculateFrustumCornersInternal(viewport, z, eye, outCorners);
		}

		private void CalculateFrustumCornersInternal(Rect viewport, float z, MonoOrStereoscopicEye eye, Vector3[] outCorners)
		{
			INTERNAL_CALL_CalculateFrustumCornersInternal(this, ref viewport, z, eye, outCorners);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CalculateFrustumCornersInternal(Camera self, ref Rect viewport, float z, MonoOrStereoscopicEye eye, Vector3[] outCorners);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ResetStereoProjectionMatrices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ResetTransparencySortSettings();

		public Vector3 WorldToScreenPoint(Vector3 position)
		{
			INTERNAL_CALL_WorldToScreenPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_WorldToScreenPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Vector3 WorldToViewportPoint(Vector3 position)
		{
			INTERNAL_CALL_WorldToViewportPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_WorldToViewportPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Vector3 ViewportToWorldPoint(Vector3 position)
		{
			INTERNAL_CALL_ViewportToWorldPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ViewportToWorldPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Vector3 ScreenToWorldPoint(Vector3 position)
		{
			INTERNAL_CALL_ScreenToWorldPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ScreenToWorldPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Vector3 ScreenToViewportPoint(Vector3 position)
		{
			INTERNAL_CALL_ScreenToViewportPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ScreenToViewportPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Vector3 ViewportToScreenPoint(Vector3 position)
		{
			INTERNAL_CALL_ViewportToScreenPoint(this, ref position, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ViewportToScreenPoint(Camera self, ref Vector3 position, out Vector3 value);

		public Ray ViewportPointToRay(Vector3 position)
		{
			INTERNAL_CALL_ViewportPointToRay(this, ref position, out Ray value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ViewportPointToRay(Camera self, ref Vector3 position, out Ray value);

		public Ray ScreenPointToRay(Vector3 position)
		{
			INTERNAL_CALL_ScreenPointToRay(this, ref position, out Ray value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ScreenPointToRay(Camera self, ref Vector3 position, out Ray value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetAllCameras(Camera[] cameras);

		[RequiredByNativeCode]
		private static void FireOnPreCull(Camera cam)
		{
			if (onPreCull != null)
			{
				onPreCull(cam);
			}
		}

		[RequiredByNativeCode]
		private static void FireOnPreRender(Camera cam)
		{
			if (onPreRender != null)
			{
				onPreRender(cam);
			}
		}

		[RequiredByNativeCode]
		private static void FireOnPostRender(Camera cam)
		{
			if (onPostRender != null)
			{
				onPostRender(cam);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Render();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RenderWithShader(Shader shader, string replacementTag);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetReplacementShader(Shader shader, string replacementTag);

		public void ResetReplacementShader()
		{
			INTERNAL_CALL_ResetReplacementShader(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetReplacementShader(Camera self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_cullingMatrix(out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_cullingMatrix(ref Matrix4x4 value);

		public void ResetCullingMatrix()
		{
			INTERNAL_CALL_ResetCullingMatrix(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ResetCullingMatrix(Camera self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RenderDontRestore();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetupCurrent(Camera cur);

		[ExcludeFromDocs]
		public bool RenderToCubemap(Cubemap cubemap)
		{
			int faceMask = 63;
			return RenderToCubemap(cubemap, faceMask);
		}

		public bool RenderToCubemap(Cubemap cubemap, [DefaultValue("63")] int faceMask)
		{
			return Internal_RenderToCubemapTexture(cubemap, faceMask);
		}

		[ExcludeFromDocs]
		public bool RenderToCubemap(RenderTexture cubemap)
		{
			int faceMask = 63;
			return RenderToCubemap(cubemap, faceMask);
		}

		public bool RenderToCubemap(RenderTexture cubemap, [DefaultValue("63")] int faceMask)
		{
			return Internal_RenderToCubemapRT(cubemap, faceMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool Internal_RenderToCubemapRT(RenderTexture cubemap, int faceMask);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool Internal_RenderToCubemapTexture(Cubemap cubemap, int faceMask);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void CopyFrom(Camera other);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern bool IsFiltered(GameObject go);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void AddCommandBuffer(CameraEvent evt, CommandBuffer buffer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RemoveCommandBuffer(CameraEvent evt, CommandBuffer buffer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RemoveCommandBuffers(CameraEvent evt);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RemoveAllCommandBuffers();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern CommandBuffer[] GetCommandBuffers(CameraEvent evt);

		internal GameObject RaycastTry(Ray ray, float distance, int layerMask)
		{
			return INTERNAL_CALL_RaycastTry(this, ref ray, distance, layerMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern GameObject INTERNAL_CALL_RaycastTry(Camera self, ref Ray ray, float distance, int layerMask);

		internal GameObject RaycastTry2D(Ray ray, float distance, int layerMask)
		{
			return INTERNAL_CALL_RaycastTry2D(this, ref ray, distance, layerMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern GameObject INTERNAL_CALL_RaycastTry2D(Camera self, ref Ray ray, float distance, int layerMask);

		public Matrix4x4 CalculateObliqueMatrix(Vector4 clipPlane)
		{
			INTERNAL_CALL_CalculateObliqueMatrix(this, ref clipPlane, out Matrix4x4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CalculateObliqueMatrix(Camera self, ref Vector4 clipPlane, out Matrix4x4 value);

		internal void OnlyUsedForTesting1()
		{
		}

		internal void OnlyUsedForTesting2()
		{
		}
	}
}
