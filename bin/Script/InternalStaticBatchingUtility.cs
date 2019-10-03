using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
	internal class InternalStaticBatchingUtility
	{
		internal class SortGO : IComparer
		{
			int IComparer.Compare(object a, object b)
			{
				if (a == b)
				{
					return 0;
				}
				Renderer renderer = GetRenderer(a as GameObject);
				Renderer renderer2 = GetRenderer(b as GameObject);
				int num = GetMaterialId(renderer).CompareTo(GetMaterialId(renderer2));
				if (num == 0)
				{
					num = GetLightmapIndex(renderer).CompareTo(GetLightmapIndex(renderer2));
				}
				return num;
			}

			private static int GetMaterialId(Renderer renderer)
			{
				if (renderer == null || renderer.sharedMaterial == null)
				{
					return 0;
				}
				return renderer.sharedMaterial.GetInstanceID();
			}

			private static int GetLightmapIndex(Renderer renderer)
			{
				if (renderer == null)
				{
					return -1;
				}
				return renderer.lightmapIndex;
			}

			private static Renderer GetRenderer(GameObject go)
			{
				if (go == null)
				{
					return null;
				}
				MeshFilter meshFilter = go.GetComponent(typeof(MeshFilter)) as MeshFilter;
				if (meshFilter == null)
				{
					return null;
				}
				return meshFilter.GetComponent<Renderer>();
			}
		}

		private const int MaxVerticesInBatch = 64000;

		private const string CombinedMeshPrefix = "Combined Mesh";

		public static void CombineRoot(GameObject staticBatchRoot)
		{
			Combine(staticBatchRoot, combineOnlyStatic: false, isEditorPostprocessScene: false);
		}

		public static void Combine(GameObject staticBatchRoot, bool combineOnlyStatic, bool isEditorPostprocessScene)
		{
			GameObject[] array = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
			List<GameObject> list = new List<GameObject>();
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				if ((!(staticBatchRoot != null) || gameObject.transform.IsChildOf(staticBatchRoot.transform)) && (!combineOnlyStatic || gameObject.isStaticBatchable))
				{
					list.Add(gameObject);
				}
			}
			array = list.ToArray();
			CombineGameObjects(array, staticBatchRoot, isEditorPostprocessScene);
		}

		public static void CombineGameObjects(GameObject[] gos, GameObject staticBatchRoot, bool isEditorPostprocessScene)
		{
			Matrix4x4 lhs = Matrix4x4.identity;
			Transform staticBatchRootTransform = null;
			if ((bool)staticBatchRoot)
			{
				lhs = staticBatchRoot.transform.worldToLocalMatrix;
				staticBatchRootTransform = staticBatchRoot.transform;
			}
			int num = 0;
			int num2 = 0;
			List<MeshSubsetCombineUtility.MeshContainer> list = new List<MeshSubsetCombineUtility.MeshContainer>();
			Array.Sort(gos, new SortGO());
			foreach (GameObject gameObject in gos)
			{
				MeshFilter meshFilter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
				if (meshFilter == null)
				{
					continue;
				}
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (sharedMesh == null || (!isEditorPostprocessScene && !sharedMesh.canAccess))
				{
					continue;
				}
				Renderer component = meshFilter.GetComponent<Renderer>();
				if (component == null || !component.enabled || component.staticBatchIndex != 0)
				{
					continue;
				}
				Material[] array = component.sharedMaterials;
				if (array.Any((Material m) => m != null && m.shader != null && m.shader.disableBatching != DisableBatchingType.False))
				{
					continue;
				}
				int vertexCount = sharedMesh.vertexCount;
				if (vertexCount == 0)
				{
					continue;
				}
				MeshRenderer meshRenderer = component as MeshRenderer;
				if (meshRenderer != null && meshRenderer.additionalVertexStreams != null && vertexCount != meshRenderer.additionalVertexStreams.vertexCount)
				{
					continue;
				}
				if (num2 + vertexCount > 64000)
				{
					MakeBatch(list, staticBatchRootTransform, num++);
					list.Clear();
					num2 = 0;
				}
				MeshSubsetCombineUtility.MeshInstance instance = default(MeshSubsetCombineUtility.MeshInstance);
				instance.meshInstanceID = sharedMesh.GetInstanceID();
				instance.rendererInstanceID = component.GetInstanceID();
				if (meshRenderer != null && meshRenderer.additionalVertexStreams != null)
				{
					instance.additionalVertexStreamsMeshInstanceID = meshRenderer.additionalVertexStreams.GetInstanceID();
				}
				instance.transform = lhs * meshFilter.transform.localToWorldMatrix;
				instance.lightmapScaleOffset = component.lightmapScaleOffset;
				instance.realtimeLightmapScaleOffset = component.realtimeLightmapScaleOffset;
				MeshSubsetCombineUtility.MeshContainer item = default(MeshSubsetCombineUtility.MeshContainer);
				item.gameObject = gameObject;
				item.instance = instance;
				item.subMeshInstances = new List<MeshSubsetCombineUtility.SubMeshInstance>();
				list.Add(item);
				if (array.Length > sharedMesh.subMeshCount)
				{
					Debug.LogWarning("Mesh '" + sharedMesh.name + "' has more materials (" + array.Length + ") than subsets (" + sharedMesh.subMeshCount + ")", component);
					Material[] array2 = new Material[sharedMesh.subMeshCount];
					for (int j = 0; j < sharedMesh.subMeshCount; j++)
					{
						array2[j] = component.sharedMaterials[j];
					}
					component.sharedMaterials = array2;
					array = array2;
				}
				for (int k = 0; k < Math.Min(array.Length, sharedMesh.subMeshCount); k++)
				{
					MeshSubsetCombineUtility.SubMeshInstance item2 = default(MeshSubsetCombineUtility.SubMeshInstance);
					item2.meshInstanceID = meshFilter.sharedMesh.GetInstanceID();
					item2.vertexOffset = num2;
					item2.subMeshIndex = k;
					item2.gameObjectInstanceID = gameObject.GetInstanceID();
					item2.transform = instance.transform;
					item.subMeshInstances.Add(item2);
				}
				num2 += sharedMesh.vertexCount;
			}
			MakeBatch(list, staticBatchRootTransform, num);
		}

		private static void MakeBatch(List<MeshSubsetCombineUtility.MeshContainer> meshes, Transform staticBatchRootTransform, int batchIndex)
		{
			if (meshes.Count >= 2)
			{
				List<MeshSubsetCombineUtility.MeshInstance> list = new List<MeshSubsetCombineUtility.MeshInstance>();
				List<MeshSubsetCombineUtility.SubMeshInstance> list2 = new List<MeshSubsetCombineUtility.SubMeshInstance>();
				foreach (MeshSubsetCombineUtility.MeshContainer mesh2 in meshes)
				{
					MeshSubsetCombineUtility.MeshContainer current = mesh2;
					list.Add(current.instance);
					list2.AddRange(current.subMeshInstances);
				}
				string str = "Combined Mesh";
				str = str + " (root: " + ((!(staticBatchRootTransform != null)) ? "scene" : staticBatchRootTransform.name) + ")";
				if (batchIndex > 0)
				{
					str = str + " " + (batchIndex + 1);
				}
				Mesh mesh = StaticBatchingHelper.InternalCombineVertices(list.ToArray(), str);
				StaticBatchingHelper.InternalCombineIndices(list2.ToArray(), mesh);
				int num = 0;
				foreach (MeshSubsetCombineUtility.MeshContainer mesh3 in meshes)
				{
					MeshSubsetCombineUtility.MeshContainer current2 = mesh3;
					MeshFilter meshFilter = (MeshFilter)current2.gameObject.GetComponent(typeof(MeshFilter));
					meshFilter.sharedMesh = mesh;
					int num2 = current2.subMeshInstances.Count();
					Renderer component = current2.gameObject.GetComponent<Renderer>();
					component.SetStaticBatchInfo(num, num2);
					component.staticBatchRootTransform = staticBatchRootTransform;
					component.enabled = false;
					component.enabled = true;
					MeshRenderer meshRenderer = component as MeshRenderer;
					if (meshRenderer != null)
					{
						meshRenderer.additionalVertexStreams = null;
					}
					num += num2;
				}
			}
		}
	}
}
