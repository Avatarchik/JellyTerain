using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class BillboardAsset : Object
	{
		public float width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float bottom
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int imageCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int vertexCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int indexCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Material material
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public BillboardAsset()
		{
			Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] BillboardAsset obj);

		public void GetImageTexCoords(List<Vector4> imageTexCoords)
		{
			if (imageTexCoords == null)
			{
				throw new ArgumentNullException("imageTexCoords");
			}
			GetImageTexCoordsInternal(imageTexCoords);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Vector4[] GetImageTexCoords();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetImageTexCoordsInternal(object list);

		public void SetImageTexCoords(List<Vector4> imageTexCoords)
		{
			if (imageTexCoords == null)
			{
				throw new ArgumentNullException("imageTexCoords");
			}
			SetImageTexCoordsInternalList(imageTexCoords);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetImageTexCoords(Vector4[] imageTexCoords);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetImageTexCoordsInternalList(object list);

		public void GetVertices(List<Vector2> vertices)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("vertices");
			}
			GetVerticesInternal(vertices);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Vector2[] GetVertices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetVerticesInternal(object list);

		public void SetVertices(List<Vector2> vertices)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("vertices");
			}
			SetVerticesInternalList(vertices);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetVertices(Vector2[] vertices);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetVerticesInternalList(object list);

		public void GetIndices(List<ushort> indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			GetIndicesInternal(indices);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern ushort[] GetIndices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetIndicesInternal(object list);

		public void SetIndices(List<ushort> indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			SetIndicesInternalList(indices);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetIndices(ushort[] indices);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetIndicesInternalList(object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera);
	}
}
