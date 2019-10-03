using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkProximityChecker")]
	[RequireComponent(typeof(NetworkIdentity))]
	public class NetworkProximityChecker : NetworkBehaviour
	{
		public enum CheckMethod
		{
			Physics3D,
			Physics2D
		}

		public int visRange = 10;

		public float visUpdateInterval = 1f;

		public CheckMethod checkMethod = CheckMethod.Physics3D;

		public bool forceHidden = false;

		private float m_VisUpdateTime;

		private void Update()
		{
			if (NetworkServer.active && Time.time - m_VisUpdateTime > visUpdateInterval)
			{
				GetComponent<NetworkIdentity>().RebuildObservers(initialize: false);
				m_VisUpdateTime = Time.time;
			}
		}

		public override bool OnCheckObserver(NetworkConnection newObserver)
		{
			if (forceHidden)
			{
				return false;
			}
			GameObject gameObject = null;
			for (int i = 0; i < newObserver.playerControllers.Count; i++)
			{
				PlayerController playerController = newObserver.playerControllers[i];
				if (playerController != null && playerController.gameObject != null)
				{
					gameObject = playerController.gameObject;
					break;
				}
			}
			if (gameObject == null)
			{
				return false;
			}
			Vector3 position = gameObject.transform.position;
			return (position - base.transform.position).magnitude < (float)visRange;
		}

		public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initial)
		{
			if (forceHidden)
			{
				NetworkIdentity component = GetComponent<NetworkIdentity>();
				if (component.connectionToClient != null)
				{
					observers.Add(component.connectionToClient);
				}
				return true;
			}
			switch (checkMethod)
			{
			case CheckMethod.Physics3D:
			{
				Collider[] array2 = Physics.OverlapSphere(base.transform.position, visRange);
				foreach (Collider collider in array2)
				{
					NetworkIdentity component3 = collider.GetComponent<NetworkIdentity>();
					if (component3 != null && component3.connectionToClient != null)
					{
						observers.Add(component3.connectionToClient);
					}
				}
				return true;
			}
			case CheckMethod.Physics2D:
			{
				Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, visRange);
				foreach (Collider2D collider2D in array)
				{
					NetworkIdentity component2 = collider2D.GetComponent<NetworkIdentity>();
					if (component2 != null && component2.connectionToClient != null)
					{
						observers.Add(component2.connectionToClient);
					}
				}
				return true;
			}
			default:
				return false;
			}
		}

		public override void OnSetLocalVisibility(bool vis)
		{
			SetVis(base.gameObject, vis);
		}

		private static void SetVis(GameObject go, bool vis)
		{
			Renderer[] components = go.GetComponents<Renderer>();
			foreach (Renderer renderer in components)
			{
				renderer.enabled = vis;
			}
			for (int j = 0; j < go.transform.childCount; j++)
			{
				Transform child = go.transform.GetChild(j);
				SetVis(child.gameObject, vis);
			}
		}
	}
}
