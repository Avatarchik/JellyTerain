using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class CompositeCollider2D : Collider2D
	{
		public enum GeometryType
		{
			Outlines,
			Polygons
		}

		public enum GenerationType
		{
			Synchronous,
			Manual
		}

		public GeometryType geometryType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public GenerationType generationType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float vertexDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float edgeRadius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int pathCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int pointCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public void GenerateGeometry()
		{
			INTERNAL_CALL_GenerateGeometry(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GenerateGeometry(CompositeCollider2D self);

		public int GetPathPointCount(int index)
		{
			if (index < 0 || index >= pathCount)
			{
				throw new ArgumentOutOfRangeException("index", $"Path index {index} must be in the range of 0 to {pathCount - 1}.");
			}
			return Internal_GetPathPointCount(index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int Internal_GetPathPointCount(int index);

		public int GetPath(int index, Vector2[] points)
		{
			if (index < 0 || index >= pathCount)
			{
				throw new ArgumentOutOfRangeException("index", $"Path index {index} must be in the range of 0 to {pathCount - 1}.");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			return Internal_GetPath(index, points);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int Internal_GetPath(int index, Vector2[] points);
	}
}
