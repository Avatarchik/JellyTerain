namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkTransformChild")]
	public class NetworkTransformChild : NetworkBehaviour
	{
		[SerializeField]
		private Transform m_Target;

		[SerializeField]
		private uint m_ChildIndex;

		private NetworkTransform m_Root;

		[SerializeField]
		private float m_SendInterval = 0.1f;

		[SerializeField]
		private NetworkTransform.AxisSyncMode m_SyncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;

		[SerializeField]
		private NetworkTransform.CompressionSyncMode m_RotationSyncCompression = NetworkTransform.CompressionSyncMode.None;

		[SerializeField]
		private float m_MovementThreshold = 0.001f;

		[SerializeField]
		private float m_InterpolateRotation = 0.5f;

		[SerializeField]
		private float m_InterpolateMovement = 0.5f;

		[SerializeField]
		private NetworkTransform.ClientMoveCallback3D m_ClientMoveCallback3D;

		private Vector3 m_TargetSyncPosition;

		private Quaternion m_TargetSyncRotation3D;

		private float m_LastClientSyncTime;

		private float m_LastClientSendTime;

		private Vector3 m_PrevPosition;

		private Quaternion m_PrevRotation;

		private const float k_LocalMovementThreshold = 1E-05f;

		private const float k_LocalRotationThreshold = 1E-05f;

		private NetworkWriter m_LocalTransformWriter;

		public Transform target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
				OnValidate();
			}
		}

		public uint childIndex => m_ChildIndex;

		public float sendInterval
		{
			get
			{
				return m_SendInterval;
			}
			set
			{
				m_SendInterval = value;
			}
		}

		public NetworkTransform.AxisSyncMode syncRotationAxis
		{
			get
			{
				return m_SyncRotationAxis;
			}
			set
			{
				m_SyncRotationAxis = value;
			}
		}

		public NetworkTransform.CompressionSyncMode rotationSyncCompression
		{
			get
			{
				return m_RotationSyncCompression;
			}
			set
			{
				m_RotationSyncCompression = value;
			}
		}

		public float movementThreshold
		{
			get
			{
				return m_MovementThreshold;
			}
			set
			{
				m_MovementThreshold = value;
			}
		}

		public float interpolateRotation
		{
			get
			{
				return m_InterpolateRotation;
			}
			set
			{
				m_InterpolateRotation = value;
			}
		}

		public float interpolateMovement
		{
			get
			{
				return m_InterpolateMovement;
			}
			set
			{
				m_InterpolateMovement = value;
			}
		}

		public NetworkTransform.ClientMoveCallback3D clientMoveCallback3D
		{
			get
			{
				return m_ClientMoveCallback3D;
			}
			set
			{
				m_ClientMoveCallback3D = value;
			}
		}

		public float lastSyncTime => m_LastClientSyncTime;

		public Vector3 targetSyncPosition => m_TargetSyncPosition;

		public Quaternion targetSyncRotation3D => m_TargetSyncRotation3D;

		private void OnValidate()
		{
			if (m_Target != null)
			{
				Transform parent = m_Target.parent;
				if (parent == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkTransformChild target cannot be the root transform.");
					}
					m_Target = null;
					return;
				}
				while (parent.parent != null)
				{
					parent = parent.parent;
				}
				m_Root = parent.gameObject.GetComponent<NetworkTransform>();
				if (m_Root == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkTransformChild root must have NetworkTransform");
					}
					m_Target = null;
					return;
				}
			}
			m_ChildIndex = uint.MaxValue;
			NetworkTransformChild[] components = m_Root.GetComponents<NetworkTransformChild>();
			for (uint num = 0u; num < components.Length; num++)
			{
				if (components[num] == this)
				{
					m_ChildIndex = num;
					break;
				}
			}
			if (m_ChildIndex == uint.MaxValue)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkTransformChild component must be a child in the same hierarchy");
				}
				m_Target = null;
			}
			if (m_SendInterval < 0f)
			{
				m_SendInterval = 0f;
			}
			if (m_SyncRotationAxis < NetworkTransform.AxisSyncMode.None || m_SyncRotationAxis > NetworkTransform.AxisSyncMode.AxisXYZ)
			{
				m_SyncRotationAxis = NetworkTransform.AxisSyncMode.None;
			}
			if (movementThreshold < 0f)
			{
				movementThreshold = 0f;
			}
			if (interpolateRotation < 0f)
			{
				interpolateRotation = 0.01f;
			}
			if (interpolateRotation > 1f)
			{
				interpolateRotation = 1f;
			}
			if (interpolateMovement < 0f)
			{
				interpolateMovement = 0.01f;
			}
			if (interpolateMovement > 1f)
			{
				interpolateMovement = 1f;
			}
		}

		private void Awake()
		{
			m_PrevPosition = m_Target.localPosition;
			m_PrevRotation = m_Target.localRotation;
			if (base.localPlayerAuthority)
			{
				m_LocalTransformWriter = new NetworkWriter();
			}
		}

		public override bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			if (!initialState)
			{
				if (base.syncVarDirtyBits == 0)
				{
					writer.WritePackedUInt32(0u);
					return false;
				}
				writer.WritePackedUInt32(1u);
			}
			SerializeModeTransform(writer);
			return true;
		}

		private void SerializeModeTransform(NetworkWriter writer)
		{
			writer.Write(m_Target.localPosition);
			if (m_SyncRotationAxis != 0)
			{
				NetworkTransform.SerializeRotation3D(writer, m_Target.localRotation, syncRotationAxis, rotationSyncCompression);
			}
			m_PrevPosition = m_Target.localPosition;
			m_PrevRotation = m_Target.localRotation;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if ((!base.isServer || !NetworkServer.localClientActive) && (initialState || reader.ReadPackedUInt32() != 0))
			{
				UnserializeModeTransform(reader, initialState);
				m_LastClientSyncTime = Time.time;
			}
		}

		private void UnserializeModeTransform(NetworkReader reader, bool initialState)
		{
			if (base.hasAuthority)
			{
				reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					NetworkTransform.UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
			else if (base.isServer && m_ClientMoveCallback3D != null)
			{
				Vector3 position = reader.ReadVector3();
				Vector3 velocity = Vector3.zero;
				Quaternion rotation = Quaternion.identity;
				if (syncRotationAxis != 0)
				{
					rotation = NetworkTransform.UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				if (m_ClientMoveCallback3D(ref position, ref velocity, ref rotation))
				{
					m_TargetSyncPosition = position;
					if (syncRotationAxis != 0)
					{
						m_TargetSyncRotation3D = rotation;
					}
				}
			}
			else
			{
				m_TargetSyncPosition = reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					m_TargetSyncRotation3D = NetworkTransform.UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
		}

		private void FixedUpdate()
		{
			if (base.isServer)
			{
				FixedUpdateServer();
			}
			if (base.isClient)
			{
				FixedUpdateClient();
			}
		}

		private void FixedUpdateServer()
		{
			if (base.syncVarDirtyBits != 0 || !NetworkServer.active || !base.isServer || GetNetworkSendInterval() == 0f)
			{
				return;
			}
			float sqrMagnitude = (m_Target.localPosition - m_PrevPosition).sqrMagnitude;
			if (sqrMagnitude < movementThreshold)
			{
				sqrMagnitude = Quaternion.Angle(m_PrevRotation, m_Target.localRotation);
				if (sqrMagnitude < movementThreshold)
				{
					return;
				}
			}
			SetDirtyBit(1u);
		}

		private void FixedUpdateClient()
		{
			if (m_LastClientSyncTime != 0f && (NetworkServer.active || NetworkClient.active) && (base.isServer || base.isClient) && GetNetworkSendInterval() != 0f && !base.hasAuthority && m_LastClientSyncTime != 0f)
			{
				if (m_InterpolateMovement > 0f)
				{
					m_Target.localPosition = Vector3.Lerp(m_Target.localPosition, m_TargetSyncPosition, m_InterpolateMovement);
				}
				else
				{
					m_Target.localPosition = m_TargetSyncPosition;
				}
				if (m_InterpolateRotation > 0f)
				{
					m_Target.localRotation = Quaternion.Slerp(m_Target.localRotation, m_TargetSyncRotation3D, m_InterpolateRotation);
				}
				else
				{
					m_Target.localRotation = m_TargetSyncRotation3D;
				}
			}
		}

		private void Update()
		{
			if (base.hasAuthority && base.localPlayerAuthority && !NetworkServer.active && Time.time - m_LastClientSendTime > GetNetworkSendInterval())
			{
				SendTransform();
				m_LastClientSendTime = Time.time;
			}
		}

		private bool HasMoved()
		{
			float num = 0f;
			num = (m_Target.localPosition - m_PrevPosition).sqrMagnitude;
			if (num > 1E-05f)
			{
				return true;
			}
			num = Quaternion.Angle(m_Target.localRotation, m_PrevRotation);
			if (num > 1E-05f)
			{
				return true;
			}
			return false;
		}

		[Client]
		private void SendTransform()
		{
			if (HasMoved() && ClientScene.readyConnection != null)
			{
				m_LocalTransformWriter.StartMessage(16);
				m_LocalTransformWriter.Write(base.netId);
				m_LocalTransformWriter.WritePackedUInt32(m_ChildIndex);
				SerializeModeTransform(m_LocalTransformWriter);
				m_PrevPosition = m_Target.localPosition;
				m_PrevRotation = m_Target.localRotation;
				m_LocalTransformWriter.FinishMessage();
				ClientScene.readyConnection.SendWriter(m_LocalTransformWriter, GetNetworkChannel());
			}
		}

		internal static void HandleChildTransform(NetworkMessage netMsg)
		{
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			uint num = netMsg.reader.ReadPackedUInt32();
			GameObject gameObject = NetworkServer.FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleChildTransform no gameObject");
				}
				return;
			}
			NetworkTransformChild[] components = gameObject.GetComponents<NetworkTransformChild>();
			if (components == null || components.Length == 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleChildTransform no children");
				}
				return;
			}
			if (num >= components.Length)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleChildTransform childIndex invalid");
				}
				return;
			}
			NetworkTransformChild networkTransformChild = components[num];
			if (networkTransformChild == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleChildTransform null target");
				}
				return;
			}
			if (!networkTransformChild.localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleChildTransform no localPlayerAuthority");
				}
				return;
			}
			if (!netMsg.conn.clientOwnedObjects.Contains(networkInstanceId))
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkTransformChild netId:" + networkInstanceId + " is not for a valid player");
				}
				return;
			}
			networkTransformChild.UnserializeModeTransform(netMsg.reader, initialState: false);
			networkTransformChild.m_LastClientSyncTime = Time.time;
			if (!networkTransformChild.isClient)
			{
				networkTransformChild.m_Target.localPosition = networkTransformChild.m_TargetSyncPosition;
				networkTransformChild.m_Target.localRotation = networkTransformChild.m_TargetSyncRotation3D;
			}
		}

		public override int GetNetworkChannel()
		{
			return 1;
		}

		public override float GetNetworkSendInterval()
		{
			return m_SendInterval;
		}
	}
}
