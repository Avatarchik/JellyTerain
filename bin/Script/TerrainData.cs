using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class TerrainData : Object
	{
		private static readonly int kMaximumResolution = Internal_GetMaximumResolution();

		private static readonly int kMinimumDetailResolutionPerPatch = Internal_GetMinimumDetailResolutionPerPatch();

		private static readonly int kMaximumDetailResolutionPerPatch = Internal_GetMaximumDetailResolutionPerPatch();

		private static readonly int kMaximumDetailPatchCount = Internal_GetMaximumDetailPatchCount();

		private static readonly int kMinimumAlphamapResolution = Internal_GetMinimumAlphamapResolution();

		private static readonly int kMaximumAlphamapResolution = Internal_GetMaximumAlphamapResolution();

		private static readonly int kMinimumBaseMapResolution = Internal_GetMinimumBaseMapResolution();

		private static readonly int kMaximumBaseMapResolution = Internal_GetMaximumBaseMapResolution();

		public int heightmapWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int heightmapHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int heightmapResolution
		{
			get
			{
				return Internal_heightmapResolution;
			}
			set
			{
				int internal_heightmapResolution = value;
				if (value < 0 || value > kMaximumResolution)
				{
					Debug.LogWarning("heightmapResolution is clamped to the range of [0, " + kMaximumResolution + "].");
					internal_heightmapResolution = Math.Min(kMaximumResolution, Math.Max(value, 0));
				}
				Internal_heightmapResolution = internal_heightmapResolution;
			}
		}

		private int Internal_heightmapResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3 heightmapScale
		{
			get
			{
				INTERNAL_get_heightmapScale(out Vector3 value);
				return value;
			}
		}

		public Vector3 size
		{
			get
			{
				INTERNAL_get_size(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_size(ref value);
			}
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		public float thickness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float wavingGrassStrength
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float wavingGrassAmount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float wavingGrassSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Color wavingGrassTint
		{
			get
			{
				INTERNAL_get_wavingGrassTint(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_wavingGrassTint(ref value);
			}
		}

		public int detailWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int detailHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int detailResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal int detailResolutionPerPatch
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public DetailPrototype[] detailPrototypes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public TreeInstance[] treeInstances
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int treeInstanceCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public TreePrototype[] treePrototypes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int alphamapLayers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int alphamapResolution
		{
			get
			{
				return Internal_alphamapResolution;
			}
			set
			{
				int internal_alphamapResolution = value;
				if (value < kMinimumAlphamapResolution || value > kMaximumAlphamapResolution)
				{
					Debug.LogWarning("alphamapResolution is clamped to the range of [" + kMinimumAlphamapResolution + ", " + kMaximumAlphamapResolution + "].");
					internal_alphamapResolution = Math.Min(kMaximumAlphamapResolution, Math.Max(value, kMinimumAlphamapResolution));
				}
				Internal_alphamapResolution = internal_alphamapResolution;
			}
		}

		private int Internal_alphamapResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int alphamapWidth => alphamapResolution;

		public int alphamapHeight => alphamapResolution;

		public int baseMapResolution
		{
			get
			{
				return Internal_baseMapResolution;
			}
			set
			{
				int internal_baseMapResolution = value;
				if (value < kMinimumBaseMapResolution || value > kMaximumBaseMapResolution)
				{
					Debug.LogWarning("baseMapResolution is clamped to the range of [" + kMinimumBaseMapResolution + ", " + kMaximumBaseMapResolution + "].");
					internal_baseMapResolution = Math.Min(kMaximumBaseMapResolution, Math.Max(value, kMinimumBaseMapResolution));
				}
				Internal_baseMapResolution = internal_baseMapResolution;
			}
		}

		private int Internal_baseMapResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		private int alphamapTextureCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Texture2D[] alphamapTextures
		{
			get
			{
				Texture2D[] array = new Texture2D[alphamapTextureCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = GetAlphamapTexture(i);
				}
				return array;
			}
		}

		public SplatPrototype[] splatPrototypes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public TerrainData()
		{
			Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMaximumResolution();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMinimumDetailResolutionPerPatch();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMaximumDetailResolutionPerPatch();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMaximumDetailPatchCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMinimumAlphamapResolution();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMaximumAlphamapResolution();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMinimumBaseMapResolution();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetMaximumBaseMapResolution();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void Internal_Create([Writable] TerrainData terrainData);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern bool HasUser(GameObject user);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void AddUser(GameObject user);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void RemoveUser(GameObject user);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_heightmapScale(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_size(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_size(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_bounds(out Bounds value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float GetHeight(int x, int y);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float GetInterpolatedHeight(float x, float y);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float[,] GetHeights(int xBase, int yBase, int width, int height);

		public void SetHeights(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new NullReferenceException();
			}
			if (xBase + heights.GetLength(1) > heightmapWidth || xBase + heights.GetLength(1) < 0 || yBase + heights.GetLength(0) < 0 || xBase < 0 || yBase < 0 || yBase + heights.GetLength(0) > heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("X or Y base out of bounds. Setting up to {0}x{1} while map size is {2}x{3}", xBase + heights.GetLength(1), yBase + heights.GetLength(0), heightmapWidth, heightmapHeight));
			}
			Internal_SetHeights(xBase, yBase, heights.GetLength(1), heights.GetLength(0), heights);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

		public void SetHeightsDelayLOD(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new ArgumentNullException("heights");
			}
			int length = heights.GetLength(0);
			int length2 = heights.GetLength(1);
			if (xBase < 0 || xBase + length2 < 0 || xBase + length2 > heightmapWidth)
			{
				throw new ArgumentException(UnityString.Format("X out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", xBase, xBase + length2, heightmapWidth));
			}
			if (yBase < 0 || yBase + length < 0 || yBase + length > heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("Y out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", yBase, yBase + length, heightmapHeight));
			}
			Internal_SetHeightsDelayLOD(xBase, yBase, length2, length, heights);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float GetSteepness(float x, float y);

		public Vector3 GetInterpolatedNormal(float x, float y)
		{
			INTERNAL_CALL_GetInterpolatedNormal(this, x, y, out Vector3 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetInterpolatedNormal(TerrainData self, float x, float y, out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern int GetAdjustedSize(int size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_wavingGrassTint(out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_wavingGrassTint(ref Color value);

		public void SetDetailResolution(int detailResolution, int resolutionPerPatch)
		{
			if (detailResolution < 0)
			{
				Debug.LogWarning("detailResolution must not be negative.");
				detailResolution = 0;
			}
			if (resolutionPerPatch < kMinimumDetailResolutionPerPatch || resolutionPerPatch > kMaximumDetailResolutionPerPatch)
			{
				Debug.LogWarning("resolutionPerPatch is clamped to the range of [" + kMinimumDetailResolutionPerPatch + ", " + kMaximumDetailResolutionPerPatch + "].");
				resolutionPerPatch = Math.Min(kMaximumDetailResolutionPerPatch, Math.Max(resolutionPerPatch, kMinimumDetailResolutionPerPatch));
			}
			int num = detailResolution / resolutionPerPatch;
			if (num > kMaximumDetailPatchCount)
			{
				Debug.LogWarning("Patch count (detailResolution / resolutionPerPatch) is clamped to the range of [0, " + kMaximumDetailPatchCount + "].");
				num = Math.Min(kMaximumDetailPatchCount, Math.Max(num, 0));
			}
			Internal_SetDetailResolution(num, resolutionPerPatch);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetDetailResolution(int patchCount, int resolutionPerPatch);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void ResetDirtyDetails();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void RefreshPrototypes();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

		public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
		{
			Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

		public TreeInstance GetTreeInstance(int index)
		{
			INTERNAL_CALL_GetTreeInstance(this, index, out TreeInstance value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetTreeInstance(TerrainData self, int index, out TreeInstance value);

		public void SetTreeInstance(int index, TreeInstance instance)
		{
			INTERNAL_CALL_SetTreeInstance(this, index, ref instance);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetTreeInstance(TerrainData self, int index, ref TreeInstance instance);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void RemoveTreePrototype(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void RecalculateTreePositions();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void RemoveDetailPrototype(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern bool NeedUpgradeScaledTreePrototypes();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void UpgradeScaledTreePrototype();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern float[,,] GetAlphamaps(int x, int y, int width, int height);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[RequiredByNativeCode]
		[GeneratedByOldBindingsGenerator]
		internal extern float GetAlphamapResolutionInternal();

		public void SetAlphamaps(int x, int y, float[,,] map)
		{
			if (map.GetLength(2) != alphamapLayers)
			{
				throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", alphamapLayers));
			}
			Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void RecalculateBasemapIfDirty();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetBasemapDirty(bool dirty);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Texture2D GetAlphamapTexture(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void AddTree(out TreeInstance tree);

		internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			return INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_RemoveTrees(TerrainData self, ref Vector2 position, float radius, int prototypeIndex);
	}
}
