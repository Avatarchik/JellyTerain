using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Mesh : Object
	{
		internal enum InternalShaderChannel
		{
			Vertex,
			Normal,
			Color,
			TexCoord0,
			TexCoord1,
			TexCoord2,
			TexCoord3,
			Tangent
		}

		internal enum InternalVertexChannelType
		{
			Float = 0,
			Color = 2
		}

		public bool isReadable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal bool canAccess
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int blendShapeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int vertexBufferCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
			set
			{
				INTERNAL_set_bounds(ref value);
			}
		}

		public int vertexCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int subMeshCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public BoneWeight[] boneWeights
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Matrix4x4[] bindposes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3[] vertices
		{
			get
			{
				return GetAllocArrayFromChannel<Vector3>(InternalShaderChannel.Vertex);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.Vertex, value);
			}
		}

		public Vector3[] normals
		{
			get
			{
				return GetAllocArrayFromChannel<Vector3>(InternalShaderChannel.Normal);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.Normal, value);
			}
		}

		public Vector4[] tangents
		{
			get
			{
				return GetAllocArrayFromChannel<Vector4>(InternalShaderChannel.Tangent);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.Tangent, value);
			}
		}

		public Vector2[] uv
		{
			get
			{
				return GetAllocArrayFromChannel<Vector2>(InternalShaderChannel.TexCoord0);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.TexCoord0, value);
			}
		}

		public Vector2[] uv2
		{
			get
			{
				return GetAllocArrayFromChannel<Vector2>(InternalShaderChannel.TexCoord1);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.TexCoord1, value);
			}
		}

		public Vector2[] uv3
		{
			get
			{
				return GetAllocArrayFromChannel<Vector2>(InternalShaderChannel.TexCoord2);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.TexCoord2, value);
			}
		}

		public Vector2[] uv4
		{
			get
			{
				return GetAllocArrayFromChannel<Vector2>(InternalShaderChannel.TexCoord3);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.TexCoord3, value);
			}
		}

		public Color[] colors
		{
			get
			{
				return GetAllocArrayFromChannel<Color>(InternalShaderChannel.Color);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.Color, value);
			}
		}

		public Color32[] colors32
		{
			get
			{
				return GetAllocArrayFromChannel<Color32>(InternalShaderChannel.Color, InternalVertexChannelType.Color, 1);
			}
			set
			{
				SetArrayForChannel(InternalShaderChannel.Color, InternalVertexChannelType.Color, 1, value);
			}
		}

		public int[] triangles
		{
			get
			{
				if (canAccess)
				{
					return GetTrianglesImpl(-1);
				}
				PrintErrorCantAccessMeshForIndices();
				return new int[0];
			}
			set
			{
				if (canAccess)
				{
					SetTrianglesImpl(-1, value, SafeLength(value));
				}
				else
				{
					PrintErrorCantAccessMeshForIndices();
				}
			}
		}

		public Mesh()
		{
			Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] Mesh mono);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Clear([UnityEngine.Internal.DefaultValue("true")] bool keepVertexLayout);

		[ExcludeFromDocs]
		public void Clear()
		{
			bool keepVertexLayout = true;
			Clear(keepVertexLayout);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void PrintErrorCantAccessMesh(InternalShaderChannel channel);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void PrintErrorCantAccessMeshForIndices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void PrintErrorBadSubmeshIndexTriangles();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void PrintErrorBadSubmeshIndexIndices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetArrayForChannelImpl(InternalShaderChannel channel, InternalVertexChannelType format, int dim, Array values, int arraySize);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Array GetAllocArrayFromChannelImpl(InternalShaderChannel channel, InternalVertexChannelType format, int dim);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetArrayFromChannelImpl(InternalShaderChannel channel, InternalVertexChannelType format, int dim, Array values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern bool HasChannel(InternalShaderChannel channel);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void ResizeList(object list, int size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Array ExtractArrayFromList(object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int[] GetTrianglesImpl(int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetTrianglesNonAllocImpl(object values, int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int[] GetIndicesImpl(int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetIndicesNonAllocImpl(object values, int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTrianglesImpl(int submesh, Array triangles, int arraySize, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds);

		[ExcludeFromDocs]
		private void SetTrianglesImpl(int submesh, Array triangles, int arraySize)
		{
			bool calculateBounds = true;
			SetTrianglesImpl(submesh, triangles, arraySize, calculateBounds);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetIndicesImpl(int submesh, MeshTopology topology, Array indices, int arraySize, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds);

		[ExcludeFromDocs]
		private void SetIndicesImpl(int submesh, MeshTopology topology, Array indices, int arraySize)
		{
			bool calculateBounds = true;
			SetIndicesImpl(submesh, topology, indices, arraySize, calculateBounds);
		}

		[ExcludeFromDocs]
		public void SetTriangles(int[] triangles, int submesh)
		{
			bool calculateBounds = true;
			SetTriangles(triangles, submesh, calculateBounds);
		}

		public void SetTriangles(int[] triangles, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds)
		{
			if (CheckCanAccessSubmeshTriangles(submesh))
			{
				SetTrianglesImpl(submesh, triangles, SafeLength(triangles), calculateBounds);
			}
		}

		[ExcludeFromDocs]
		public void SetTriangles(List<int> triangles, int submesh)
		{
			bool calculateBounds = true;
			SetTriangles(triangles, submesh, calculateBounds);
		}

		public void SetTriangles(List<int> triangles, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds)
		{
			if (CheckCanAccessSubmeshTriangles(submesh))
			{
				SetTrianglesImpl(submesh, ExtractArrayFromList(triangles), SafeLength(triangles), calculateBounds);
			}
		}

		[ExcludeFromDocs]
		public void SetIndices(int[] indices, MeshTopology topology, int submesh)
		{
			bool calculateBounds = true;
			SetIndices(indices, topology, submesh, calculateBounds);
		}

		public void SetIndices(int[] indices, MeshTopology topology, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds)
		{
			if (CheckCanAccessSubmeshIndices(submesh))
			{
				SetIndicesImpl(submesh, topology, indices, SafeLength(indices), calculateBounds);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ClearBlendShapes();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetBlendShapeName(int shapeIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetBlendShapeFrameCount(int shapeIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float GetBlendShapeFrameWeight(int shapeIndex, int frameIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void GetBlendShapeFrameVertices(int shapeIndex, int frameIndex, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void AddBlendShapeFrame(string shapeName, float frameWeight, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

		public IntPtr GetNativeVertexBufferPtr(int bufferIndex)
		{
			INTERNAL_CALL_GetNativeVertexBufferPtr(this, bufferIndex, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetNativeVertexBufferPtr(Mesh self, int bufferIndex, out IntPtr value);

		public IntPtr GetNativeIndexBufferPtr()
		{
			INTERNAL_CALL_GetNativeIndexBufferPtr(this, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetNativeIndexBufferPtr(Mesh self, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_bounds(out Bounds value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_bounds(ref Bounds value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RecalculateBounds();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RecalculateNormals();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RecalculateTangents();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is no longer supported (UnityUpgradable)", true)]
		[GeneratedByOldBindingsGenerator]
		public extern void Optimize();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern MeshTopology GetTopology(int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern uint GetIndexStart(int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern uint GetIndexCount(int submesh);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void CombineMeshes(CombineInstance[] combine, [UnityEngine.Internal.DefaultValue("true")] bool mergeSubMeshes, [UnityEngine.Internal.DefaultValue("true")] bool useMatrices, [UnityEngine.Internal.DefaultValue("false")] bool hasLightmapData);

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices)
		{
			bool hasLightmapData = false;
			CombineMeshes(combine, mergeSubMeshes, useMatrices, hasLightmapData);
		}

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
		{
			bool hasLightmapData = false;
			bool useMatrices = true;
			CombineMeshes(combine, mergeSubMeshes, useMatrices, hasLightmapData);
		}

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine)
		{
			bool hasLightmapData = false;
			bool useMatrices = true;
			bool mergeSubMeshes = true;
			CombineMeshes(combine, mergeSubMeshes, useMatrices, hasLightmapData);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetBoneWeightsNonAllocImpl(object values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int GetBindposeCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetBindposesNonAllocImpl(object values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void MarkDynamic();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void UploadMeshData(bool markNoLogerReadable);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetBlendShapeIndex(string blendShapeName);

		internal InternalShaderChannel GetUVChannel(int uvIndex)
		{
			if (uvIndex < 0 || uvIndex > 3)
			{
				throw new ArgumentException("GetUVChannel called for bad uvIndex", "uvIndex");
			}
			return (InternalShaderChannel)(3 + uvIndex);
		}

		internal static int DefaultDimensionForChannel(InternalShaderChannel channel)
		{
			switch (channel)
			{
			case InternalShaderChannel.Vertex:
			case InternalShaderChannel.Normal:
				return 3;
			case InternalShaderChannel.TexCoord0:
			case InternalShaderChannel.TexCoord1:
			case InternalShaderChannel.TexCoord2:
			case InternalShaderChannel.TexCoord3:
				return 2;
			default:
				if (channel == InternalShaderChannel.Tangent || channel == InternalShaderChannel.Color)
				{
					return 4;
				}
				throw new ArgumentException("DefaultDimensionForChannel called for bad channel", "channel");
			}
		}

		private T[] GetAllocArrayFromChannel<T>(InternalShaderChannel channel, InternalVertexChannelType format, int dim)
		{
			if (canAccess)
			{
				if (HasChannel(channel))
				{
					return (T[])GetAllocArrayFromChannelImpl(channel, format, dim);
				}
			}
			else
			{
				PrintErrorCantAccessMesh(channel);
			}
			return new T[0];
		}

		private T[] GetAllocArrayFromChannel<T>(InternalShaderChannel channel)
		{
			return GetAllocArrayFromChannel<T>(channel, InternalVertexChannelType.Float, DefaultDimensionForChannel(channel));
		}

		private int SafeLength(Array values)
		{
			return values?.Length ?? 0;
		}

		private int SafeLength<T>(List<T> values)
		{
			return (values != null) ? values.Count : 0;
		}

		private void SetSizedArrayForChannel(InternalShaderChannel channel, InternalVertexChannelType format, int dim, Array values, int valuesCount)
		{
			if (canAccess)
			{
				SetArrayForChannelImpl(channel, format, dim, values, valuesCount);
			}
			else
			{
				PrintErrorCantAccessMesh(channel);
			}
		}

		private void SetArrayForChannel<T>(InternalShaderChannel channel, InternalVertexChannelType format, int dim, T[] values)
		{
			SetSizedArrayForChannel(channel, format, dim, values, SafeLength(values));
		}

		private void SetArrayForChannel<T>(InternalShaderChannel channel, T[] values)
		{
			SetSizedArrayForChannel(channel, InternalVertexChannelType.Float, DefaultDimensionForChannel(channel), values, SafeLength(values));
		}

		private void SetListForChannel<T>(InternalShaderChannel channel, InternalVertexChannelType format, int dim, List<T> values)
		{
			SetSizedArrayForChannel(channel, format, dim, ExtractArrayFromList(values), SafeLength(values));
		}

		private void SetListForChannel<T>(InternalShaderChannel channel, List<T> values)
		{
			SetSizedArrayForChannel(channel, InternalVertexChannelType.Float, DefaultDimensionForChannel(channel), ExtractArrayFromList(values), SafeLength(values));
		}

		private void GetListForChannel<T>(List<T> buffer, int capacity, InternalShaderChannel channel, int dim)
		{
			GetListForChannel(buffer, capacity, channel, dim, InternalVertexChannelType.Float);
		}

		private void GetListForChannel<T>(List<T> buffer, int capacity, InternalShaderChannel channel, int dim, InternalVertexChannelType channelType)
		{
			buffer.Clear();
			if (!canAccess)
			{
				PrintErrorCantAccessMesh(channel);
			}
			else if (HasChannel(channel))
			{
				PrepareUserBuffer(buffer, capacity);
				GetArrayFromChannelImpl(channel, channelType, dim, ExtractArrayFromList(buffer));
			}
		}

		private void PrepareUserBuffer<T>(List<T> buffer, int capacity)
		{
			buffer.Clear();
			if (buffer.Capacity < capacity)
			{
				buffer.Capacity = capacity;
			}
			ResizeList(buffer, capacity);
		}

		public void GetVertices(List<Vector3> vertices)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("The result vertices list cannot be null.", "vertices");
			}
			GetListForChannel(vertices, vertexCount, InternalShaderChannel.Vertex, DefaultDimensionForChannel(InternalShaderChannel.Vertex));
		}

		public void SetVertices(List<Vector3> inVertices)
		{
			SetListForChannel(InternalShaderChannel.Vertex, inVertices);
		}

		public void GetNormals(List<Vector3> normals)
		{
			if (normals == null)
			{
				throw new ArgumentNullException("The result normals list cannot be null.", "normals");
			}
			GetListForChannel(normals, vertexCount, InternalShaderChannel.Normal, DefaultDimensionForChannel(InternalShaderChannel.Normal));
		}

		public void SetNormals(List<Vector3> inNormals)
		{
			SetListForChannel(InternalShaderChannel.Normal, inNormals);
		}

		public void GetTangents(List<Vector4> tangents)
		{
			if (tangents == null)
			{
				throw new ArgumentNullException("The result tangents list cannot be null.", "tangents");
			}
			GetListForChannel(tangents, vertexCount, InternalShaderChannel.Tangent, DefaultDimensionForChannel(InternalShaderChannel.Tangent));
		}

		public void SetTangents(List<Vector4> inTangents)
		{
			SetListForChannel(InternalShaderChannel.Tangent, inTangents);
		}

		public void GetColors(List<Color> colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("The result colors list cannot be null.", "colors");
			}
			GetListForChannel(colors, vertexCount, InternalShaderChannel.Color, DefaultDimensionForChannel(InternalShaderChannel.Color));
		}

		public void SetColors(List<Color> inColors)
		{
			SetListForChannel(InternalShaderChannel.Color, inColors);
		}

		public void GetColors(List<Color32> colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("The result colors list cannot be null.", "colors");
			}
			GetListForChannel(colors, vertexCount, InternalShaderChannel.Color, 1, InternalVertexChannelType.Color);
		}

		public void SetColors(List<Color32> inColors)
		{
			SetListForChannel(InternalShaderChannel.Color, InternalVertexChannelType.Color, 1, inColors);
		}

		private void SetUvsImpl<T>(int uvIndex, int dim, List<T> uvs)
		{
			if (uvIndex < 0 || uvIndex > 3)
			{
				Debug.LogError("The uv index is invalid (must be in [0..3]");
			}
			else
			{
				SetListForChannel(GetUVChannel(uvIndex), InternalVertexChannelType.Float, dim, uvs);
			}
		}

		public void SetUVs(int channel, List<Vector2> uvs)
		{
			SetUvsImpl(channel, 2, uvs);
		}

		public void SetUVs(int channel, List<Vector3> uvs)
		{
			SetUvsImpl(channel, 3, uvs);
		}

		public void SetUVs(int channel, List<Vector4> uvs)
		{
			SetUvsImpl(channel, 4, uvs);
		}

		private void GetUVsImpl<T>(int uvIndex, List<T> uvs, int dim)
		{
			if (uvs == null)
			{
				throw new ArgumentNullException("The result uvs list cannot be null.", "uvs");
			}
			if (uvIndex < 0 || uvIndex > 3)
			{
				throw new IndexOutOfRangeException("Specified uv index is out of range. Must be in the range [0, 3].");
			}
			GetListForChannel(uvs, vertexCount, GetUVChannel(uvIndex), dim);
		}

		public void GetUVs(int channel, List<Vector2> uvs)
		{
			GetUVsImpl(channel, uvs, 2);
		}

		public void GetUVs(int channel, List<Vector3> uvs)
		{
			GetUVsImpl(channel, uvs, 3);
		}

		public void GetUVs(int channel, List<Vector4> uvs)
		{
			GetUVsImpl(channel, uvs, 4);
		}

		private bool CheckCanAccessSubmesh(int submesh, bool errorAboutTriangles)
		{
			if (!canAccess)
			{
				PrintErrorCantAccessMeshForIndices();
				return false;
			}
			if (submesh < 0 || submesh >= subMeshCount)
			{
				if (errorAboutTriangles)
				{
					PrintErrorBadSubmeshIndexTriangles();
				}
				else
				{
					PrintErrorBadSubmeshIndexIndices();
				}
				return false;
			}
			return true;
		}

		private bool CheckCanAccessSubmeshTriangles(int submesh)
		{
			return CheckCanAccessSubmesh(submesh, errorAboutTriangles: true);
		}

		private bool CheckCanAccessSubmeshIndices(int submesh)
		{
			return CheckCanAccessSubmesh(submesh, errorAboutTriangles: false);
		}

		public int[] GetTriangles(int submesh)
		{
			return (!CheckCanAccessSubmeshTriangles(submesh)) ? new int[0] : GetTrianglesImpl(submesh);
		}

		public void GetTriangles(List<int> triangles, int submesh)
		{
			if (triangles == null)
			{
				throw new ArgumentNullException("The result triangles list cannot be null.", "triangles");
			}
			if (submesh < 0 || submesh >= subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			PrepareUserBuffer(triangles, (int)GetIndexCount(submesh));
			GetTrianglesNonAllocImpl(triangles, submesh);
		}

		public int[] GetIndices(int submesh)
		{
			return (!CheckCanAccessSubmeshIndices(submesh)) ? new int[0] : GetIndicesImpl(submesh);
		}

		public void GetIndices(List<int> indices, int submesh)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("The result indices list cannot be null.", "indices");
			}
			if (submesh < 0 || submesh >= subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			PrepareUserBuffer(indices, (int)GetIndexCount(submesh));
			indices.Clear();
			GetIndicesNonAllocImpl(indices, submesh);
		}

		public void GetBindposes(List<Matrix4x4> bindposes)
		{
			if (bindposes == null)
			{
				throw new ArgumentNullException("The result bindposes list cannot be null.", "bindposes");
			}
			PrepareUserBuffer(bindposes, GetBindposeCount());
			GetBindposesNonAllocImpl(bindposes);
		}

		public void GetBoneWeights(List<BoneWeight> boneWeights)
		{
			if (boneWeights == null)
			{
				throw new ArgumentNullException("The result boneWeights list cannot be null.", "boneWeights");
			}
			PrepareUserBuffer(boneWeights, vertexCount);
			GetBoneWeightsNonAllocImpl(boneWeights);
		}
	}
}
