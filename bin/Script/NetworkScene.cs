using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class NetworkScene
	{
		private Dictionary<NetworkInstanceId, NetworkIdentity> m_LocalObjects = new Dictionary<NetworkInstanceId, NetworkIdentity>();

		private static Dictionary<NetworkHash128, GameObject> s_GuidToPrefab = new Dictionary<NetworkHash128, GameObject>();

		private static Dictionary<NetworkHash128, SpawnDelegate> s_SpawnHandlers = new Dictionary<NetworkHash128, SpawnDelegate>();

		private static Dictionary<NetworkHash128, UnSpawnDelegate> s_UnspawnHandlers = new Dictionary<NetworkHash128, UnSpawnDelegate>();

		internal Dictionary<NetworkInstanceId, NetworkIdentity> localObjects => m_LocalObjects;

		internal static Dictionary<NetworkHash128, GameObject> guidToPrefab => s_GuidToPrefab;

		internal static Dictionary<NetworkHash128, SpawnDelegate> spawnHandlers => s_SpawnHandlers;

		internal static Dictionary<NetworkHash128, UnSpawnDelegate> unspawnHandlers => s_UnspawnHandlers;

		internal void Shutdown()
		{
			ClearLocalObjects();
			ClearSpawners();
		}

		internal void SetLocalObject(NetworkInstanceId netId, GameObject obj, bool isClient, bool isServer)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("SetLocalObject " + netId + " " + obj);
			}
			if (obj == null)
			{
				m_LocalObjects[netId] = null;
				return;
			}
			NetworkIdentity networkIdentity = null;
			if (m_LocalObjects.ContainsKey(netId))
			{
				networkIdentity = m_LocalObjects[netId];
			}
			if (networkIdentity == null)
			{
				networkIdentity = obj.GetComponent<NetworkIdentity>();
				m_LocalObjects[netId] = networkIdentity;
			}
			networkIdentity.UpdateClientServer(isClient, isServer);
		}

		internal GameObject FindLocalObject(NetworkInstanceId netId)
		{
			if (m_LocalObjects.ContainsKey(netId))
			{
				NetworkIdentity networkIdentity = m_LocalObjects[netId];
				if (networkIdentity != null)
				{
					return networkIdentity.gameObject;
				}
			}
			return null;
		}

		internal bool GetNetworkIdentity(NetworkInstanceId netId, out NetworkIdentity uv)
		{
			if (m_LocalObjects.ContainsKey(netId) && m_LocalObjects[netId] != null)
			{
				uv = m_LocalObjects[netId];
				return true;
			}
			uv = null;
			return false;
		}

		internal bool RemoveLocalObject(NetworkInstanceId netId)
		{
			return m_LocalObjects.Remove(netId);
		}

		internal bool RemoveLocalObjectAndDestroy(NetworkInstanceId netId)
		{
			if (m_LocalObjects.ContainsKey(netId))
			{
				NetworkIdentity networkIdentity = m_LocalObjects[netId];
				Object.Destroy(networkIdentity.gameObject);
				return m_LocalObjects.Remove(netId);
			}
			return false;
		}

		internal void ClearLocalObjects()
		{
			m_LocalObjects.Clear();
		}

		internal static void RegisterPrefab(GameObject prefab, NetworkHash128 newAssetId)
		{
			NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
			if ((bool)component)
			{
				component.SetDynamicAssetId(newAssetId);
				if (LogFilter.logDebug)
				{
					Debug.Log("Registering prefab '" + prefab.name + "' as asset:" + component.assetId);
				}
				s_GuidToPrefab[component.assetId] = prefab;
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component");
			}
		}

		internal static void RegisterPrefab(GameObject prefab)
		{
			NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
			if ((bool)component)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("Registering prefab '" + prefab.name + "' as asset:" + component.assetId);
				}
				s_GuidToPrefab[component.assetId] = prefab;
				NetworkIdentity[] componentsInChildren = prefab.GetComponentsInChildren<NetworkIdentity>();
				if (componentsInChildren.Length > 1 && LogFilter.logWarn)
				{
					Debug.LogWarning("The prefab '" + prefab.name + "' has multiple NetworkIdentity components. There can only be one NetworkIdentity on a prefab, and it must be on the root object.");
				}
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component");
			}
		}

		internal static bool GetPrefab(NetworkHash128 assetId, out GameObject prefab)
		{
			if (!assetId.IsValid())
			{
				prefab = null;
				return false;
			}
			if (s_GuidToPrefab.ContainsKey(assetId) && s_GuidToPrefab[assetId] != null)
			{
				prefab = s_GuidToPrefab[assetId];
				return true;
			}
			prefab = null;
			return false;
		}

		internal static void ClearSpawners()
		{
			s_GuidToPrefab.Clear();
			s_SpawnHandlers.Clear();
			s_UnspawnHandlers.Clear();
		}

		public static void UnregisterSpawnHandler(NetworkHash128 assetId)
		{
			s_SpawnHandlers.Remove(assetId);
			s_UnspawnHandlers.Remove(assetId);
		}

		internal static void RegisterSpawnHandler(NetworkHash128 assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (spawnHandler == null || unspawnHandler == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterSpawnHandler custom spawn function null for " + assetId);
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RegisterSpawnHandler asset '" + assetId + "' " + spawnHandler.GetMethodName() + "/" + unspawnHandler.GetMethodName());
			}
			s_SpawnHandlers[assetId] = spawnHandler;
			s_UnspawnHandlers[assetId] = unspawnHandler;
		}

		internal static void UnregisterPrefab(GameObject prefab)
		{
			NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Could not unregister '" + prefab.name + "' since it contains no NetworkIdentity component");
				}
			}
			else
			{
				s_SpawnHandlers.Remove(component.assetId);
				s_UnspawnHandlers.Remove(component.assetId);
			}
		}

		internal static void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component");
				}
				return;
			}
			if (spawnHandler == null || unspawnHandler == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterPrefab custom spawn function null for " + component.assetId);
				}
				return;
			}
			if (!component.assetId.IsValid())
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterPrefab game object " + prefab.name + " has no prefab. Use RegisterSpawnHandler() instead?");
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Registering custom prefab '" + prefab.name + "' as asset:" + component.assetId + " " + spawnHandler.GetMethodName() + "/" + unspawnHandler.GetMethodName());
			}
			s_SpawnHandlers[component.assetId] = spawnHandler;
			s_UnspawnHandlers[component.assetId] = unspawnHandler;
		}

		internal static bool GetSpawnHandler(NetworkHash128 assetId, out SpawnDelegate handler)
		{
			if (s_SpawnHandlers.ContainsKey(assetId))
			{
				handler = s_SpawnHandlers[assetId];
				return true;
			}
			handler = null;
			return false;
		}

		internal static bool InvokeUnSpawnHandler(NetworkHash128 assetId, GameObject obj)
		{
			if (s_UnspawnHandlers.ContainsKey(assetId) && s_UnspawnHandlers[assetId] != null)
			{
				UnSpawnDelegate unSpawnDelegate = s_UnspawnHandlers[assetId];
				unSpawnDelegate(obj);
				return true;
			}
			return false;
		}

		internal void DestroyAllClientObjects()
		{
			foreach (NetworkInstanceId key in m_LocalObjects.Keys)
			{
				NetworkIdentity networkIdentity = m_LocalObjects[key];
				if (networkIdentity != null && networkIdentity.gameObject != null && !InvokeUnSpawnHandler(networkIdentity.assetId, networkIdentity.gameObject))
				{
					if (networkIdentity.sceneId.IsEmpty())
					{
						Object.Destroy(networkIdentity.gameObject);
					}
					else
					{
						networkIdentity.MarkForReset();
						networkIdentity.gameObject.SetActive(value: false);
					}
				}
			}
			ClearLocalObjects();
		}

		internal void DumpAllClientObjects()
		{
			foreach (NetworkInstanceId key in m_LocalObjects.Keys)
			{
				NetworkIdentity networkIdentity = m_LocalObjects[key];
				if (networkIdentity != null)
				{
					Debug.Log("ID:" + key + " OBJ:" + networkIdentity.gameObject + " AS:" + networkIdentity.assetId);
				}
				else
				{
					Debug.Log("ID:" + key + " OBJ: null");
				}
			}
		}
	}
}
