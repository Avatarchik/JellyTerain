using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class GeometryUtility
	{
		private static void Internal_ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix)
		{
			INTERNAL_CALL_Internal_ExtractPlanes(planes, ref worldToProjectionMatrix);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_ExtractPlanes(Plane[] planes, ref Matrix4x4 worldToProjectionMatrix);

		public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
		{
			return INTERNAL_CALL_TestPlanesAABB(planes, ref bounds);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_TestPlanesAABB(Plane[] planes, ref Bounds bounds);

		private static Bounds Internal_CalculateBounds(Vector3[] positions, Matrix4x4 transform)
		{
			INTERNAL_CALL_Internal_CalculateBounds(positions, ref transform, out Bounds value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_CalculateBounds(Vector3[] positions, ref Matrix4x4 transform, out Bounds value);

		public static Plane[] CalculateFrustumPlanes(Camera camera)
		{
			return CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
		}

		public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
		{
			Plane[] array = new Plane[6];
			Internal_ExtractPlanes(array, worldToProjectionMatrix);
			return array;
		}

		public static Bounds CalculateBounds(Vector3[] positions, Matrix4x4 transform)
		{
			if (positions == null)
			{
				throw new ArgumentNullException("positions");
			}
			if (positions.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.", "positions");
			}
			return Internal_CalculateBounds(positions, transform);
		}
	}
}
