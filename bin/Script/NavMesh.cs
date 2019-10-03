using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	[MovedFrom("UnityEngine")]
	public static class NavMesh
	{
		public delegate void OnNavMeshPreUpdate();

		public static OnNavMeshPreUpdate onPreUpdate;

		public const int AllAreas = -1;

		public static float avoidancePredictionTime
		{
			get
			{
				return GetAvoidancePredictionTime();
			}
			set
			{
				SetAvoidancePredictionTime(value);
			}
		}

		public static int pathfindingIterationsPerFrame
		{
			get
			{
				return GetPathfindingIterationsPerFrame();
			}
			set
			{
				SetPathfindingIterationsPerFrame(value);
			}
		}

		[RequiredByNativeCode]
		private static void Internal_CallOnNavMeshPreUpdate()
		{
			if (onPreUpdate != null)
			{
				onPreUpdate();
			}
		}

		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int areaMask)
		{
			return INTERNAL_CALL_Raycast(ref sourcePosition, ref targetPosition, out hit, areaMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_Raycast(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int areaMask);

		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			path.ClearCorners();
			return CalculatePathInternal(sourcePosition, targetPosition, areaMask, path);
		}

		internal static bool CalculatePathInternal(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			return INTERNAL_CALL_CalculatePathInternal(ref sourcePosition, ref targetPosition, areaMask, path);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_CalculatePathInternal(ref Vector3 sourcePosition, ref Vector3 targetPosition, int areaMask, NavMeshPath path);

		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, int areaMask)
		{
			return INTERNAL_CALL_FindClosestEdge(ref sourcePosition, out hit, areaMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_FindClosestEdge(ref Vector3 sourcePosition, out NavMeshHit hit, int areaMask);

		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
		{
			return INTERNAL_CALL_SamplePosition(ref sourcePosition, out hit, maxDistance, areaMask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_SamplePosition(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use SetAreaCost instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetLayerCost(int layer, float cost);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use GetAreaCost instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetLayerCost(int layer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use GetAreaFromName instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetNavMeshLayerFromName(string layerName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetAreaCost(int areaIndex, float cost);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetAreaCost(int areaIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetAreaFromName(string areaName);

		public static NavMeshTriangulation CalculateTriangulation()
		{
			return (NavMeshTriangulation)TriangulateInternal();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern object TriangulateInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("use NavMesh.CalculateTriangulation () instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern void Triangulate(out Vector3[] vertices, out int[] indices);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("AddOffMeshLinks has no effect and is deprecated.")]
		[GeneratedByOldBindingsGenerator]
		public static extern void AddOffMeshLinks();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("RestoreNavMesh has no effect and is deprecated.")]
		[GeneratedByOldBindingsGenerator]
		public static extern void RestoreNavMesh();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void SetAvoidancePredictionTime(float t);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern float GetAvoidancePredictionTime();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void SetPathfindingIterationsPerFrame(int iter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern int GetPathfindingIterationsPerFrame();

		public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData)
		{
			if (navMeshData == null)
			{
				throw new ArgumentNullException("navMeshData");
			}
			NavMeshDataInstance result = default(NavMeshDataInstance);
			result.id = AddNavMeshDataInternal(navMeshData);
			return result;
		}

		public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
		{
			if (navMeshData == null)
			{
				throw new ArgumentNullException("navMeshData");
			}
			NavMeshDataInstance result = default(NavMeshDataInstance);
			result.id = AddNavMeshDataTransformedInternal(navMeshData, position, rotation);
			return result;
		}

		public static void RemoveNavMeshData(NavMeshDataInstance handle)
		{
			RemoveNavMeshDataInternal(handle.id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool IsValidNavMeshDataHandle(int handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool IsValidLinkHandle(int handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern Object InternalGetOwner(int dataID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool InternalSetOwner(int dataID, int ownerID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern Object InternalGetLinkOwner(int linkID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool InternalSetLinkOwner(int linkID, int ownerID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern int AddNavMeshDataInternal(NavMeshData navMeshData);

		internal static int AddNavMeshDataTransformedInternal(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
		{
			return INTERNAL_CALL_AddNavMeshDataTransformedInternal(navMeshData, ref position, ref rotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_AddNavMeshDataTransformedInternal(NavMeshData navMeshData, ref Vector3 position, ref Quaternion rotation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void RemoveNavMeshDataInternal(int handle);

		public static NavMeshLinkInstance AddLink(NavMeshLinkData link)
		{
			NavMeshLinkInstance result = default(NavMeshLinkInstance);
			result.id = AddLinkInternal(link, Vector3.zero, Quaternion.identity);
			return result;
		}

		public static NavMeshLinkInstance AddLink(NavMeshLinkData link, Vector3 position, Quaternion rotation)
		{
			NavMeshLinkInstance result = default(NavMeshLinkInstance);
			result.id = AddLinkInternal(link, position, rotation);
			return result;
		}

		public static void RemoveLink(NavMeshLinkInstance handle)
		{
			RemoveLinkInternal(handle.id);
		}

		internal static int AddLinkInternal(NavMeshLinkData link, Vector3 position, Quaternion rotation)
		{
			return INTERNAL_CALL_AddLinkInternal(ref link, ref position, ref rotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_AddLinkInternal(ref NavMeshLinkData link, ref Vector3 position, ref Quaternion rotation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void RemoveLinkInternal(int handle);

		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, NavMeshQueryFilter filter)
		{
			return SamplePositionFilter(sourcePosition, out hit, maxDistance, filter.agentTypeID, filter.areaMask);
		}

		private static bool SamplePositionFilter(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask)
		{
			return INTERNAL_CALL_SamplePositionFilter(ref sourcePosition, out hit, maxDistance, type, mask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_SamplePositionFilter(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask);

		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, NavMeshQueryFilter filter)
		{
			return FindClosestEdgeFilter(sourcePosition, out hit, filter.agentTypeID, filter.areaMask);
		}

		private static bool FindClosestEdgeFilter(Vector3 sourcePosition, out NavMeshHit hit, int type, int mask)
		{
			return INTERNAL_CALL_FindClosestEdgeFilter(ref sourcePosition, out hit, type, mask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_FindClosestEdgeFilter(ref Vector3 sourcePosition, out NavMeshHit hit, int type, int mask);

		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, NavMeshQueryFilter filter)
		{
			return RaycastFilter(sourcePosition, targetPosition, out hit, filter.agentTypeID, filter.areaMask);
		}

		private static bool RaycastFilter(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int type, int mask)
		{
			return INTERNAL_CALL_RaycastFilter(ref sourcePosition, ref targetPosition, out hit, type, mask);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_RaycastFilter(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int type, int mask);

		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter filter, NavMeshPath path)
		{
			path.ClearCorners();
			return CalculatePathFilterInternal(sourcePosition, targetPosition, path, filter.agentTypeID, filter.areaMask, filter.costs);
		}

		internal static bool CalculatePathFilterInternal(Vector3 sourcePosition, Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs)
		{
			return INTERNAL_CALL_CalculatePathFilterInternal(ref sourcePosition, ref targetPosition, path, type, mask, costs);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_CalculatePathFilterInternal(ref Vector3 sourcePosition, ref Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs);

		public static NavMeshBuildSettings CreateSettings()
		{
			INTERNAL_CALL_CreateSettings(out NavMeshBuildSettings value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CreateSettings(out NavMeshBuildSettings value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void RemoveSettings(int agentTypeID);

		public static NavMeshBuildSettings GetSettingsByID(int agentTypeID)
		{
			INTERNAL_CALL_GetSettingsByID(agentTypeID, out NavMeshBuildSettings value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetSettingsByID(int agentTypeID, out NavMeshBuildSettings value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetSettingsCount();

		public static NavMeshBuildSettings GetSettingsByIndex(int index)
		{
			INTERNAL_CALL_GetSettingsByIndex(index, out NavMeshBuildSettings value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetSettingsByIndex(int index, out NavMeshBuildSettings value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetSettingsNameFromID(int agentTypeID);
	}
}
