using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public static class TerrainExtensions
	{
		public static void UpdateGIMaterials(this Terrain terrain)
		{
			if (terrain.terrainData == null)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			UpdateGIMaterialsForTerrain(terrain, new Rect(0f, 0f, 1f, 1f));
		}

		public static void UpdateGIMaterials(this Terrain terrain, int x, int y, int width, int height)
		{
			if (terrain.terrainData == null)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			float num = terrain.terrainData.alphamapWidth;
			float num2 = terrain.terrainData.alphamapHeight;
			UpdateGIMaterialsForTerrain(terrain, new Rect((float)x / num, (float)y / num2, (float)width / num, (float)height / num2));
		}

		internal static void UpdateGIMaterialsForTerrain(Terrain terrain, Rect uvBounds)
		{
			INTERNAL_CALL_UpdateGIMaterialsForTerrain(terrain, ref uvBounds);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_UpdateGIMaterialsForTerrain(Terrain terrain, ref Rect uvBounds);
	}
}
