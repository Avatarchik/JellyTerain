using System.ComponentModel;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkTransformVisualizer")]
	[RequireComponent(typeof(NetworkTransform))]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class NetworkTransformVisualizer : NetworkBehaviour
	{
		[SerializeField]
		private GameObject m_VisualizerPrefab;

		private NetworkTransform m_NetworkTransform;

		private GameObject m_Visualizer;

		private static Material s_LineMaterial;

		public GameObject visualizerPrefab
		{
			get
			{
				return m_VisualizerPrefab;
			}
			set
			{
				m_VisualizerPrefab = value;
			}
		}

		public override void OnStartClient()
		{
			if (m_VisualizerPrefab != null)
			{
				m_NetworkTransform = GetComponent<NetworkTransform>();
				CreateLineMaterial();
				m_Visualizer = Object.Instantiate(m_VisualizerPrefab, base.transform.position, Quaternion.identity);
			}
		}

		public override void OnStartLocalPlayer()
		{
			if (!(m_Visualizer == null) && (m_NetworkTransform.localPlayerAuthority || base.isServer))
			{
				Object.Destroy(m_Visualizer);
			}
		}

		private void OnDestroy()
		{
			if (m_Visualizer != null)
			{
				Object.Destroy(m_Visualizer);
			}
		}

		[ClientCallback]
		private void FixedUpdate()
		{
			if (!(m_Visualizer == null) && (NetworkServer.active || NetworkClient.active) && (base.isServer || base.isClient) && (!base.hasAuthority || !m_NetworkTransform.localPlayerAuthority))
			{
				m_Visualizer.transform.position = m_NetworkTransform.targetSyncPosition;
				if (m_NetworkTransform.rigidbody3D != null && m_Visualizer.GetComponent<Rigidbody>() != null)
				{
					m_Visualizer.GetComponent<Rigidbody>().velocity = m_NetworkTransform.targetSyncVelocity;
				}
				if (m_NetworkTransform.rigidbody2D != null && m_Visualizer.GetComponent<Rigidbody2D>() != null)
				{
					m_Visualizer.GetComponent<Rigidbody2D>().velocity = m_NetworkTransform.targetSyncVelocity;
				}
				Quaternion rotation = Quaternion.identity;
				if (m_NetworkTransform.rigidbody3D != null)
				{
					rotation = m_NetworkTransform.targetSyncRotation3D;
				}
				if (m_NetworkTransform.rigidbody2D != null)
				{
					rotation = Quaternion.Euler(0f, 0f, m_NetworkTransform.targetSyncRotation2D);
				}
				m_Visualizer.transform.rotation = rotation;
			}
		}

		private void OnRenderObject()
		{
			if (!(m_Visualizer == null) && (!m_NetworkTransform.localPlayerAuthority || !base.hasAuthority) && m_NetworkTransform.lastSyncTime != 0f)
			{
				s_LineMaterial.SetPass(0);
				GL.Begin(1);
				GL.Color(Color.white);
				Vector3 position = base.transform.position;
				float x = position.x;
				Vector3 position2 = base.transform.position;
				float y = position2.y;
				Vector3 position3 = base.transform.position;
				GL.Vertex3(x, y, position3.z);
				Vector3 targetSyncPosition = m_NetworkTransform.targetSyncPosition;
				float x2 = targetSyncPosition.x;
				Vector3 targetSyncPosition2 = m_NetworkTransform.targetSyncPosition;
				float y2 = targetSyncPosition2.y;
				Vector3 targetSyncPosition3 = m_NetworkTransform.targetSyncPosition;
				GL.Vertex3(x2, y2, targetSyncPosition3.z);
				GL.End();
				DrawRotationInterpolation();
			}
		}

		private void DrawRotationInterpolation()
		{
			Quaternion quaternion = Quaternion.identity;
			if (m_NetworkTransform.rigidbody3D != null)
			{
				quaternion = m_NetworkTransform.targetSyncRotation3D;
			}
			if (m_NetworkTransform.rigidbody2D != null)
			{
				quaternion = Quaternion.Euler(0f, 0f, m_NetworkTransform.targetSyncRotation2D);
			}
			if (!(quaternion == Quaternion.identity))
			{
				GL.Begin(1);
				GL.Color(Color.yellow);
				Vector3 position = base.transform.position;
				float x = position.x;
				Vector3 position2 = base.transform.position;
				float y = position2.y;
				Vector3 position3 = base.transform.position;
				GL.Vertex3(x, y, position3.z);
				Vector3 vector = base.transform.position + base.transform.right;
				GL.Vertex3(vector.x, vector.y, vector.z);
				GL.End();
				GL.Begin(1);
				GL.Color(Color.green);
				Vector3 position4 = base.transform.position;
				float x2 = position4.x;
				Vector3 position5 = base.transform.position;
				float y2 = position5.y;
				Vector3 position6 = base.transform.position;
				GL.Vertex3(x2, y2, position6.z);
				Vector3 b = quaternion * Vector3.right;
				Vector3 vector2 = base.transform.position + b;
				GL.Vertex3(vector2.x, vector2.y, vector2.z);
				GL.End();
			}
		}

		private static void CreateLineMaterial()
		{
			if (!s_LineMaterial)
			{
				Shader shader = Shader.Find("Hidden/Internal-Colored");
				if (!shader)
				{
					Debug.LogWarning("Could not find Colored builtin shader");
					return;
				}
				s_LineMaterial = new Material(shader);
				s_LineMaterial.hideFlags = HideFlags.HideAndDontSave;
				s_LineMaterial.SetInt("_ZWrite", 0);
			}
		}
	}
}
