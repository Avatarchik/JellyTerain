using System;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkTransform")]
	public class NetworkTransform : NetworkBehaviour
	{
		public enum TransformSyncMode
		{
			SyncNone,
			SyncTransform,
			SyncRigidbody2D,
			SyncRigidbody3D,
			SyncCharacterController
		}

		public enum AxisSyncMode
		{
			None,
			AxisX,
			AxisY,
			AxisZ,
			AxisXY,
			AxisXZ,
			AxisYZ,
			AxisXYZ
		}

		public enum CompressionSyncMode
		{
			None,
			Low,
			High
		}

		public delegate bool ClientMoveCallback3D(ref Vector3 position, ref Vector3 velocity, ref Quaternion rotation);

		public delegate bool ClientMoveCallback2D(ref Vector2 position, ref Vector2 velocity, ref float rotation);

		[SerializeField]
		private TransformSyncMode m_TransformSyncMode = TransformSyncMode.SyncNone;

		[SerializeField]
		private float m_SendInterval = 0.1f;

		[SerializeField]
		private AxisSyncMode m_SyncRotationAxis = AxisSyncMode.AxisXYZ;

		[SerializeField]
		private CompressionSyncMode m_RotationSyncCompression = CompressionSyncMode.None;

		[SerializeField]
		private bool m_SyncSpin;

		[SerializeField]
		private float m_MovementTheshold = 0.001f;

		[SerializeField]
		private float m_VelocityThreshold = 0.0001f;

		[SerializeField]
		private float m_SnapThreshold = 5f;

		[SerializeField]
		private float m_InterpolateRotation = 1f;

		[SerializeField]
		private float m_InterpolateMovement = 1f;

		[SerializeField]
		private ClientMoveCallback3D m_ClientMoveCallback3D;

		[SerializeField]
		private ClientMoveCallback2D m_ClientMoveCallback2D;

		private Rigidbody m_RigidBody3D;

		private Rigidbody2D m_RigidBody2D;

		private CharacterController m_CharacterController;

		private bool m_Grounded = true;

		private Vector3 m_TargetSyncPosition;

		private Vector3 m_TargetSyncVelocity;

		private Vector3 m_FixedPosDiff;

		private Quaternion m_TargetSyncRotation3D;

		private Vector3 m_TargetSyncAngularVelocity3D;

		private float m_TargetSyncRotation2D;

		private float m_TargetSyncAngularVelocity2D;

		private float m_LastClientSyncTime;

		private float m_LastClientSendTime;

		private Vector3 m_PrevPosition;

		private Quaternion m_PrevRotation;

		private float m_PrevRotation2D;

		private float m_PrevVelocity;

		private const float k_LocalMovementThreshold = 1E-05f;

		private const float k_LocalRotationThreshold = 1E-05f;

		private const float k_LocalVelocityThreshold = 1E-05f;

		private const float k_MoveAheadRatio = 0.1f;

		private NetworkWriter m_LocalTransformWriter;

		public TransformSyncMode transformSyncMode
		{
			get
			{
				return m_TransformSyncMode;
			}
			set
			{
				m_TransformSyncMode = value;
			}
		}

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

		public AxisSyncMode syncRotationAxis
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

		public CompressionSyncMode rotationSyncCompression
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

		public bool syncSpin
		{
			get
			{
				return m_SyncSpin;
			}
			set
			{
				m_SyncSpin = value;
			}
		}

		public float movementTheshold
		{
			get
			{
				return m_MovementTheshold;
			}
			set
			{
				m_MovementTheshold = value;
			}
		}

		public float velocityThreshold
		{
			get
			{
				return m_VelocityThreshold;
			}
			set
			{
				m_VelocityThreshold = value;
			}
		}

		public float snapThreshold
		{
			get
			{
				return m_SnapThreshold;
			}
			set
			{
				m_SnapThreshold = value;
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

		public ClientMoveCallback3D clientMoveCallback3D
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

		public ClientMoveCallback2D clientMoveCallback2D
		{
			get
			{
				return m_ClientMoveCallback2D;
			}
			set
			{
				m_ClientMoveCallback2D = value;
			}
		}

		public CharacterController characterContoller => m_CharacterController;

		public Rigidbody rigidbody3D => m_RigidBody3D;

		public Rigidbody2D rigidbody2D => m_RigidBody2D;

		public float lastSyncTime => m_LastClientSyncTime;

		public Vector3 targetSyncPosition => m_TargetSyncPosition;

		public Vector3 targetSyncVelocity => m_TargetSyncVelocity;

		public Quaternion targetSyncRotation3D => m_TargetSyncRotation3D;

		public float targetSyncRotation2D => m_TargetSyncRotation2D;

		public bool grounded
		{
			get
			{
				return m_Grounded;
			}
			set
			{
				m_Grounded = value;
			}
		}

		private void OnValidate()
		{
			if (m_TransformSyncMode < TransformSyncMode.SyncNone || m_TransformSyncMode > TransformSyncMode.SyncCharacterController)
			{
				m_TransformSyncMode = TransformSyncMode.SyncTransform;
			}
			if (m_SendInterval < 0f)
			{
				m_SendInterval = 0f;
			}
			if (m_SyncRotationAxis < AxisSyncMode.None || m_SyncRotationAxis > AxisSyncMode.AxisXYZ)
			{
				m_SyncRotationAxis = AxisSyncMode.None;
			}
			if (m_MovementTheshold < 0f)
			{
				m_MovementTheshold = 0f;
			}
			if (m_VelocityThreshold < 0f)
			{
				m_VelocityThreshold = 0f;
			}
			if (m_SnapThreshold < 0f)
			{
				m_SnapThreshold = 0.01f;
			}
			if (m_InterpolateRotation < 0f)
			{
				m_InterpolateRotation = 0.01f;
			}
			if (m_InterpolateMovement < 0f)
			{
				m_InterpolateMovement = 0.01f;
			}
		}

		private void Awake()
		{
			m_RigidBody3D = GetComponent<Rigidbody>();
			m_RigidBody2D = GetComponent<Rigidbody2D>();
			m_CharacterController = GetComponent<CharacterController>();
			m_PrevPosition = base.transform.position;
			m_PrevRotation = base.transform.rotation;
			m_PrevVelocity = 0f;
			if (base.localPlayerAuthority)
			{
				m_LocalTransformWriter = new NetworkWriter();
			}
		}

		public override void OnStartServer()
		{
			m_LastClientSyncTime = 0f;
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
			switch (transformSyncMode)
			{
			case TransformSyncMode.SyncNone:
				return false;
			case TransformSyncMode.SyncTransform:
				SerializeModeTransform(writer);
				break;
			case TransformSyncMode.SyncRigidbody3D:
				SerializeMode3D(writer);
				break;
			case TransformSyncMode.SyncRigidbody2D:
				SerializeMode2D(writer);
				break;
			case TransformSyncMode.SyncCharacterController:
				SerializeModeCharacterController(writer);
				break;
			}
			return true;
		}

		private void SerializeModeTransform(NetworkWriter writer)
		{
			writer.Write(base.transform.position);
			if (m_SyncRotationAxis != 0)
			{
				SerializeRotation3D(writer, base.transform.rotation, syncRotationAxis, rotationSyncCompression);
			}
			m_PrevPosition = base.transform.position;
			m_PrevRotation = base.transform.rotation;
			m_PrevVelocity = 0f;
		}

		private void VerifySerializeComponentExists()
		{
			bool flag = false;
			Type type = null;
			switch (transformSyncMode)
			{
			case TransformSyncMode.SyncCharacterController:
				if (!m_CharacterController)
				{
					flag = true;
					type = typeof(CharacterController);
				}
				break;
			case TransformSyncMode.SyncRigidbody2D:
				if (!m_RigidBody2D)
				{
					flag = true;
					type = typeof(Rigidbody2D);
				}
				break;
			case TransformSyncMode.SyncRigidbody3D:
				if (!m_RigidBody3D)
				{
					flag = true;
					type = typeof(Rigidbody);
				}
				break;
			}
			if (flag && type != null)
			{
				throw new InvalidOperationException($"transformSyncMode set to {transformSyncMode} but no {type.Name} component was found, did you call NetworkServer.Spawn on a prefab?");
			}
		}

		private void SerializeMode3D(NetworkWriter writer)
		{
			VerifySerializeComponentExists();
			if (base.isServer && m_LastClientSyncTime != 0f)
			{
				writer.Write(m_TargetSyncPosition);
				SerializeVelocity3D(writer, m_TargetSyncVelocity, CompressionSyncMode.None);
				if (syncRotationAxis != 0)
				{
					SerializeRotation3D(writer, m_TargetSyncRotation3D, syncRotationAxis, rotationSyncCompression);
				}
			}
			else
			{
				writer.Write(m_RigidBody3D.position);
				SerializeVelocity3D(writer, m_RigidBody3D.velocity, CompressionSyncMode.None);
				if (syncRotationAxis != 0)
				{
					SerializeRotation3D(writer, m_RigidBody3D.rotation, syncRotationAxis, rotationSyncCompression);
				}
			}
			if (m_SyncSpin)
			{
				SerializeSpin3D(writer, m_RigidBody3D.angularVelocity, syncRotationAxis, rotationSyncCompression);
			}
			m_PrevPosition = m_RigidBody3D.position;
			m_PrevRotation = base.transform.rotation;
			m_PrevVelocity = m_RigidBody3D.velocity.sqrMagnitude;
		}

		private void SerializeModeCharacterController(NetworkWriter writer)
		{
			VerifySerializeComponentExists();
			if (base.isServer && m_LastClientSyncTime != 0f)
			{
				writer.Write(m_TargetSyncPosition);
				if (syncRotationAxis != 0)
				{
					SerializeRotation3D(writer, m_TargetSyncRotation3D, syncRotationAxis, rotationSyncCompression);
				}
			}
			else
			{
				writer.Write(base.transform.position);
				if (syncRotationAxis != 0)
				{
					SerializeRotation3D(writer, base.transform.rotation, syncRotationAxis, rotationSyncCompression);
				}
			}
			m_PrevPosition = base.transform.position;
			m_PrevRotation = base.transform.rotation;
			m_PrevVelocity = 0f;
		}

		private void SerializeMode2D(NetworkWriter writer)
		{
			VerifySerializeComponentExists();
			if (base.isServer && m_LastClientSyncTime != 0f)
			{
				writer.Write((Vector2)m_TargetSyncPosition);
				SerializeVelocity2D(writer, m_TargetSyncVelocity, CompressionSyncMode.None);
				if (syncRotationAxis != 0)
				{
					float num = m_TargetSyncRotation2D % 360f;
					if (num < 0f)
					{
						num += 360f;
					}
					SerializeRotation2D(writer, num, rotationSyncCompression);
				}
			}
			else
			{
				writer.Write(m_RigidBody2D.position);
				SerializeVelocity2D(writer, m_RigidBody2D.velocity, CompressionSyncMode.None);
				if (syncRotationAxis != 0)
				{
					float num2 = m_RigidBody2D.rotation % 360f;
					if (num2 < 0f)
					{
						num2 += 360f;
					}
					SerializeRotation2D(writer, num2, rotationSyncCompression);
				}
			}
			if (m_SyncSpin)
			{
				SerializeSpin2D(writer, m_RigidBody2D.angularVelocity, rotationSyncCompression);
			}
			m_PrevPosition = m_RigidBody2D.position;
			m_PrevRotation = base.transform.rotation;
			m_PrevVelocity = m_RigidBody2D.velocity.sqrMagnitude;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if ((!base.isServer || !NetworkServer.localClientActive) && (initialState || reader.ReadPackedUInt32() != 0))
			{
				switch (transformSyncMode)
				{
				case TransformSyncMode.SyncNone:
					return;
				case TransformSyncMode.SyncTransform:
					UnserializeModeTransform(reader, initialState);
					break;
				case TransformSyncMode.SyncRigidbody3D:
					UnserializeMode3D(reader, initialState);
					break;
				case TransformSyncMode.SyncRigidbody2D:
					UnserializeMode2D(reader, initialState);
					break;
				case TransformSyncMode.SyncCharacterController:
					UnserializeModeCharacterController(reader, initialState);
					break;
				}
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
					UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
			else if (base.isServer && m_ClientMoveCallback3D != null)
			{
				Vector3 position = reader.ReadVector3();
				Vector3 velocity = Vector3.zero;
				Quaternion rotation = Quaternion.identity;
				if (syncRotationAxis != 0)
				{
					rotation = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				if (m_ClientMoveCallback3D(ref position, ref velocity, ref rotation))
				{
					base.transform.position = position;
					if (syncRotationAxis != 0)
					{
						base.transform.rotation = rotation;
					}
				}
			}
			else
			{
				base.transform.position = reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					base.transform.rotation = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
		}

		private void UnserializeMode3D(NetworkReader reader, bool initialState)
		{
			if (base.hasAuthority)
			{
				reader.ReadVector3();
				reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				if (syncSpin)
				{
					UnserializeSpin3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				return;
			}
			if (base.isServer && m_ClientMoveCallback3D != null)
			{
				Vector3 position = reader.ReadVector3();
				Vector3 velocity = reader.ReadVector3();
				Quaternion rotation = Quaternion.identity;
				if (syncRotationAxis != 0)
				{
					rotation = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				if (!m_ClientMoveCallback3D(ref position, ref velocity, ref rotation))
				{
					return;
				}
				m_TargetSyncPosition = position;
				m_TargetSyncVelocity = velocity;
				if (syncRotationAxis != 0)
				{
					m_TargetSyncRotation3D = rotation;
				}
			}
			else
			{
				m_TargetSyncPosition = reader.ReadVector3();
				m_TargetSyncVelocity = reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					m_TargetSyncRotation3D = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
			if (syncSpin)
			{
				m_TargetSyncAngularVelocity3D = UnserializeSpin3D(reader, syncRotationAxis, rotationSyncCompression);
			}
			if (m_RigidBody3D == null)
			{
				return;
			}
			if (base.isServer && !base.isClient)
			{
				m_RigidBody3D.MovePosition(m_TargetSyncPosition);
				m_RigidBody3D.MoveRotation(m_TargetSyncRotation3D);
				m_RigidBody3D.velocity = m_TargetSyncVelocity;
				return;
			}
			if (GetNetworkSendInterval() == 0f)
			{
				m_RigidBody3D.MovePosition(m_TargetSyncPosition);
				m_RigidBody3D.velocity = m_TargetSyncVelocity;
				if (syncRotationAxis != 0)
				{
					m_RigidBody3D.MoveRotation(m_TargetSyncRotation3D);
				}
				if (syncSpin)
				{
					m_RigidBody3D.angularVelocity = m_TargetSyncAngularVelocity3D;
				}
				return;
			}
			float magnitude = (m_RigidBody3D.position - m_TargetSyncPosition).magnitude;
			if (magnitude > snapThreshold)
			{
				m_RigidBody3D.position = m_TargetSyncPosition;
				m_RigidBody3D.velocity = m_TargetSyncVelocity;
			}
			if (interpolateRotation == 0f && syncRotationAxis != 0)
			{
				m_RigidBody3D.rotation = m_TargetSyncRotation3D;
				if (syncSpin)
				{
					m_RigidBody3D.angularVelocity = m_TargetSyncAngularVelocity3D;
				}
			}
			if (m_InterpolateMovement == 0f)
			{
				m_RigidBody3D.position = m_TargetSyncPosition;
			}
			if (initialState && syncRotationAxis != 0)
			{
				m_RigidBody3D.rotation = m_TargetSyncRotation3D;
			}
		}

		private void UnserializeMode2D(NetworkReader reader, bool initialState)
		{
			if (base.hasAuthority)
			{
				reader.ReadVector2();
				reader.ReadVector2();
				if (syncRotationAxis != 0)
				{
					UnserializeRotation2D(reader, rotationSyncCompression);
				}
				if (syncSpin)
				{
					UnserializeSpin2D(reader, rotationSyncCompression);
				}
			}
			else
			{
				if (m_RigidBody2D == null)
				{
					return;
				}
				if (base.isServer && m_ClientMoveCallback2D != null)
				{
					Vector2 position = reader.ReadVector2();
					Vector2 velocity = reader.ReadVector2();
					float rotation = 0f;
					if (syncRotationAxis != 0)
					{
						rotation = UnserializeRotation2D(reader, rotationSyncCompression);
					}
					if (!m_ClientMoveCallback2D(ref position, ref velocity, ref rotation))
					{
						return;
					}
					m_TargetSyncPosition = position;
					m_TargetSyncVelocity = velocity;
					if (syncRotationAxis != 0)
					{
						m_TargetSyncRotation2D = rotation;
					}
				}
				else
				{
					m_TargetSyncPosition = reader.ReadVector2();
					m_TargetSyncVelocity = reader.ReadVector2();
					if (syncRotationAxis != 0)
					{
						m_TargetSyncRotation2D = UnserializeRotation2D(reader, rotationSyncCompression);
					}
				}
				if (syncSpin)
				{
					m_TargetSyncAngularVelocity2D = UnserializeSpin2D(reader, rotationSyncCompression);
				}
				if (base.isServer && !base.isClient)
				{
					base.transform.position = m_TargetSyncPosition;
					m_RigidBody2D.MoveRotation(m_TargetSyncRotation2D);
					m_RigidBody2D.velocity = m_TargetSyncVelocity;
					return;
				}
				if (GetNetworkSendInterval() == 0f)
				{
					base.transform.position = m_TargetSyncPosition;
					m_RigidBody2D.velocity = m_TargetSyncVelocity;
					if (syncRotationAxis != 0)
					{
						m_RigidBody2D.MoveRotation(m_TargetSyncRotation2D);
					}
					if (syncSpin)
					{
						m_RigidBody2D.angularVelocity = m_TargetSyncAngularVelocity2D;
					}
					return;
				}
				float magnitude = (m_RigidBody2D.position - (Vector2)m_TargetSyncPosition).magnitude;
				if (magnitude > snapThreshold)
				{
					m_RigidBody2D.position = m_TargetSyncPosition;
					m_RigidBody2D.velocity = m_TargetSyncVelocity;
				}
				if (interpolateRotation == 0f && syncRotationAxis != 0)
				{
					m_RigidBody2D.rotation = m_TargetSyncRotation2D;
					if (syncSpin)
					{
						m_RigidBody2D.angularVelocity = m_TargetSyncAngularVelocity2D;
					}
				}
				if (m_InterpolateMovement == 0f)
				{
					m_RigidBody2D.position = m_TargetSyncPosition;
				}
				if (initialState)
				{
					m_RigidBody2D.rotation = m_TargetSyncRotation2D;
				}
			}
		}

		private void UnserializeModeCharacterController(NetworkReader reader, bool initialState)
		{
			if (base.hasAuthority)
			{
				reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				return;
			}
			if (base.isServer && m_ClientMoveCallback3D != null)
			{
				Vector3 position = reader.ReadVector3();
				Quaternion rotation = Quaternion.identity;
				if (syncRotationAxis != 0)
				{
					rotation = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
				if (m_CharacterController == null)
				{
					return;
				}
				Vector3 velocity = m_CharacterController.velocity;
				if (!m_ClientMoveCallback3D(ref position, ref velocity, ref rotation))
				{
					return;
				}
				m_TargetSyncPosition = position;
				m_TargetSyncVelocity = velocity;
				if (syncRotationAxis != 0)
				{
					m_TargetSyncRotation3D = rotation;
				}
			}
			else
			{
				m_TargetSyncPosition = reader.ReadVector3();
				if (syncRotationAxis != 0)
				{
					m_TargetSyncRotation3D = UnserializeRotation3D(reader, syncRotationAxis, rotationSyncCompression);
				}
			}
			if (m_CharacterController == null)
			{
				return;
			}
			Vector3 a = m_TargetSyncPosition - base.transform.position;
			Vector3 a2 = a / GetNetworkSendInterval();
			m_FixedPosDiff = a2 * Time.fixedDeltaTime;
			if (base.isServer && !base.isClient)
			{
				base.transform.position = m_TargetSyncPosition;
				base.transform.rotation = m_TargetSyncRotation3D;
				return;
			}
			if (GetNetworkSendInterval() == 0f)
			{
				base.transform.position = m_TargetSyncPosition;
				if (syncRotationAxis != 0)
				{
					base.transform.rotation = m_TargetSyncRotation3D;
				}
				return;
			}
			float magnitude = (base.transform.position - m_TargetSyncPosition).magnitude;
			if (magnitude > snapThreshold)
			{
				base.transform.position = m_TargetSyncPosition;
			}
			if (interpolateRotation == 0f && syncRotationAxis != 0)
			{
				base.transform.rotation = m_TargetSyncRotation3D;
			}
			if (m_InterpolateMovement == 0f)
			{
				base.transform.position = m_TargetSyncPosition;
			}
			if (initialState && syncRotationAxis != 0)
			{
				base.transform.rotation = m_TargetSyncRotation3D;
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
			float magnitude = (base.transform.position - m_PrevPosition).magnitude;
			if (magnitude < movementTheshold)
			{
				magnitude = Quaternion.Angle(m_PrevRotation, base.transform.rotation);
				if (magnitude < movementTheshold && !CheckVelocityChanged())
				{
					return;
				}
			}
			SetDirtyBit(1u);
		}

		private bool CheckVelocityChanged()
		{
			switch (transformSyncMode)
			{
			case TransformSyncMode.SyncRigidbody2D:
				if ((bool)m_RigidBody2D && m_VelocityThreshold > 0f)
				{
					return Mathf.Abs(m_RigidBody2D.velocity.sqrMagnitude - m_PrevVelocity) >= m_VelocityThreshold;
				}
				return false;
			case TransformSyncMode.SyncRigidbody3D:
				if ((bool)m_RigidBody3D && m_VelocityThreshold > 0f)
				{
					return Mathf.Abs(m_RigidBody3D.velocity.sqrMagnitude - m_PrevVelocity) >= m_VelocityThreshold;
				}
				return false;
			default:
				return false;
			}
		}

		private void FixedUpdateClient()
		{
			if (m_LastClientSyncTime != 0f && (NetworkServer.active || NetworkClient.active) && (base.isServer || base.isClient) && GetNetworkSendInterval() != 0f && !base.hasAuthority)
			{
				switch (transformSyncMode)
				{
				case TransformSyncMode.SyncNone:
					break;
				case TransformSyncMode.SyncTransform:
					break;
				case TransformSyncMode.SyncRigidbody3D:
					InterpolateTransformMode3D();
					break;
				case TransformSyncMode.SyncRigidbody2D:
					InterpolateTransformMode2D();
					break;
				case TransformSyncMode.SyncCharacterController:
					InterpolateTransformModeCharacterController();
					break;
				}
			}
		}

		private void InterpolateTransformMode3D()
		{
			if (m_InterpolateMovement != 0f)
			{
				Vector3 velocity = (m_TargetSyncPosition - m_RigidBody3D.position) * m_InterpolateMovement / GetNetworkSendInterval();
				m_RigidBody3D.velocity = velocity;
			}
			if (interpolateRotation != 0f)
			{
				m_RigidBody3D.MoveRotation(Quaternion.Slerp(m_RigidBody3D.rotation, m_TargetSyncRotation3D, Time.fixedDeltaTime * interpolateRotation));
			}
			m_TargetSyncPosition += m_TargetSyncVelocity * Time.fixedDeltaTime * 0.1f;
		}

		private void InterpolateTransformModeCharacterController()
		{
			if (!(m_FixedPosDiff == Vector3.zero) || !(m_TargetSyncRotation3D == base.transform.rotation))
			{
				if (m_InterpolateMovement != 0f)
				{
					m_CharacterController.Move(m_FixedPosDiff * m_InterpolateMovement);
				}
				if (interpolateRotation != 0f)
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, m_TargetSyncRotation3D, Time.fixedDeltaTime * interpolateRotation * 10f);
				}
				if (Time.time - m_LastClientSyncTime > GetNetworkSendInterval())
				{
					m_FixedPosDiff = Vector3.zero;
					Vector3 motion = m_TargetSyncPosition - base.transform.position;
					m_CharacterController.Move(motion);
				}
			}
		}

		private void InterpolateTransformMode2D()
		{
			if (m_InterpolateMovement != 0f)
			{
				Vector2 velocity = m_RigidBody2D.velocity;
				Vector2 velocity2 = ((Vector2)m_TargetSyncPosition - m_RigidBody2D.position) * m_InterpolateMovement / GetNetworkSendInterval();
				if (!m_Grounded && velocity2.y < 0f)
				{
					velocity2.y = velocity.y;
				}
				m_RigidBody2D.velocity = velocity2;
			}
			if (interpolateRotation != 0f)
			{
				float num = m_RigidBody2D.rotation % 360f;
				if (num < 0f)
				{
					num += 360f;
				}
				Quaternion quaternion = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, 0f, m_TargetSyncRotation2D), Time.fixedDeltaTime * interpolateRotation / GetNetworkSendInterval());
				Rigidbody2D rigidBody2D = m_RigidBody2D;
				Vector3 eulerAngles = quaternion.eulerAngles;
				rigidBody2D.MoveRotation(eulerAngles.z);
				m_TargetSyncRotation2D += m_TargetSyncAngularVelocity2D * Time.fixedDeltaTime * 0.1f;
			}
			m_TargetSyncPosition += m_TargetSyncVelocity * Time.fixedDeltaTime * 0.1f;
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
			num = ((m_RigidBody3D != null) ? (m_RigidBody3D.position - m_PrevPosition).magnitude : ((!(m_RigidBody2D != null)) ? (base.transform.position - m_PrevPosition).magnitude : (m_RigidBody2D.position - (Vector2)m_PrevPosition).magnitude));
			if (num > 1E-05f)
			{
				return true;
			}
			num = ((m_RigidBody3D != null) ? Quaternion.Angle(m_RigidBody3D.rotation, m_PrevRotation) : ((!(m_RigidBody2D != null)) ? Quaternion.Angle(base.transform.rotation, m_PrevRotation) : Math.Abs(m_RigidBody2D.rotation - m_PrevRotation2D)));
			if (num > 1E-05f)
			{
				return true;
			}
			if (m_RigidBody3D != null)
			{
				num = Mathf.Abs(m_RigidBody3D.velocity.sqrMagnitude - m_PrevVelocity);
			}
			else if (m_RigidBody2D != null)
			{
				num = Mathf.Abs(m_RigidBody2D.velocity.sqrMagnitude - m_PrevVelocity);
			}
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
				m_LocalTransformWriter.StartMessage(6);
				m_LocalTransformWriter.Write(base.netId);
				switch (transformSyncMode)
				{
				case TransformSyncMode.SyncNone:
					return;
				case TransformSyncMode.SyncTransform:
					SerializeModeTransform(m_LocalTransformWriter);
					break;
				case TransformSyncMode.SyncRigidbody3D:
					SerializeMode3D(m_LocalTransformWriter);
					break;
				case TransformSyncMode.SyncRigidbody2D:
					SerializeMode2D(m_LocalTransformWriter);
					break;
				case TransformSyncMode.SyncCharacterController:
					SerializeModeCharacterController(m_LocalTransformWriter);
					break;
				}
				if (m_RigidBody3D != null)
				{
					m_PrevPosition = m_RigidBody3D.position;
					m_PrevRotation = m_RigidBody3D.rotation;
					m_PrevVelocity = m_RigidBody3D.velocity.sqrMagnitude;
				}
				else if (m_RigidBody2D != null)
				{
					m_PrevPosition = m_RigidBody2D.position;
					m_PrevRotation2D = m_RigidBody2D.rotation;
					m_PrevVelocity = m_RigidBody2D.velocity.sqrMagnitude;
				}
				else
				{
					m_PrevPosition = base.transform.position;
					m_PrevRotation = base.transform.rotation;
				}
				m_LocalTransformWriter.FinishMessage();
				ClientScene.readyConnection.SendWriter(m_LocalTransformWriter, GetNetworkChannel());
			}
		}

		public static void HandleTransform(NetworkMessage netMsg)
		{
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			GameObject gameObject = NetworkServer.FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleTransform no gameObject");
				}
				return;
			}
			NetworkTransform component = gameObject.GetComponent<NetworkTransform>();
			if (component == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleTransform null target");
				}
			}
			else if (!component.localPlayerAuthority)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleTransform no localPlayerAuthority");
				}
			}
			else if (netMsg.conn.clientOwnedObjects == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("HandleTransform object not owned by connection");
				}
			}
			else if (netMsg.conn.clientOwnedObjects.Contains(networkInstanceId))
			{
				switch (component.transformSyncMode)
				{
				case TransformSyncMode.SyncNone:
					return;
				case TransformSyncMode.SyncTransform:
					component.UnserializeModeTransform(netMsg.reader, initialState: false);
					break;
				case TransformSyncMode.SyncRigidbody3D:
					component.UnserializeMode3D(netMsg.reader, initialState: false);
					break;
				case TransformSyncMode.SyncRigidbody2D:
					component.UnserializeMode2D(netMsg.reader, initialState: false);
					break;
				case TransformSyncMode.SyncCharacterController:
					component.UnserializeModeCharacterController(netMsg.reader, initialState: false);
					break;
				}
				component.m_LastClientSyncTime = Time.time;
			}
			else if (LogFilter.logWarn)
			{
				Debug.LogWarning("HandleTransform netId:" + networkInstanceId + " is not for a valid player");
			}
		}

		private static void WriteAngle(NetworkWriter writer, float angle, CompressionSyncMode compression)
		{
			switch (compression)
			{
			case CompressionSyncMode.None:
				writer.Write(angle);
				break;
			case CompressionSyncMode.Low:
				writer.Write((short)angle);
				break;
			case CompressionSyncMode.High:
				writer.Write((short)angle);
				break;
			}
		}

		private static float ReadAngle(NetworkReader reader, CompressionSyncMode compression)
		{
			switch (compression)
			{
			case CompressionSyncMode.None:
				return reader.ReadSingle();
			case CompressionSyncMode.Low:
				return reader.ReadInt16();
			case CompressionSyncMode.High:
				return reader.ReadInt16();
			default:
				return 0f;
			}
		}

		public static void SerializeVelocity3D(NetworkWriter writer, Vector3 velocity, CompressionSyncMode compression)
		{
			writer.Write(velocity);
		}

		public static void SerializeVelocity2D(NetworkWriter writer, Vector2 velocity, CompressionSyncMode compression)
		{
			writer.Write(velocity);
		}

		public static void SerializeRotation3D(NetworkWriter writer, Quaternion rot, AxisSyncMode mode, CompressionSyncMode compression)
		{
			switch (mode)
			{
			case AxisSyncMode.None:
				break;
			case AxisSyncMode.AxisX:
			{
				Vector3 eulerAngles12 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles12.x, compression);
				break;
			}
			case AxisSyncMode.AxisY:
			{
				Vector3 eulerAngles11 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles11.y, compression);
				break;
			}
			case AxisSyncMode.AxisZ:
			{
				Vector3 eulerAngles10 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles10.z, compression);
				break;
			}
			case AxisSyncMode.AxisXY:
			{
				Vector3 eulerAngles8 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles8.x, compression);
				Vector3 eulerAngles9 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles9.y, compression);
				break;
			}
			case AxisSyncMode.AxisXZ:
			{
				Vector3 eulerAngles6 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles6.x, compression);
				Vector3 eulerAngles7 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles7.z, compression);
				break;
			}
			case AxisSyncMode.AxisYZ:
			{
				Vector3 eulerAngles4 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles4.y, compression);
				Vector3 eulerAngles5 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles5.z, compression);
				break;
			}
			case AxisSyncMode.AxisXYZ:
			{
				Vector3 eulerAngles = rot.eulerAngles;
				WriteAngle(writer, eulerAngles.x, compression);
				Vector3 eulerAngles2 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles2.y, compression);
				Vector3 eulerAngles3 = rot.eulerAngles;
				WriteAngle(writer, eulerAngles3.z, compression);
				break;
			}
			}
		}

		public static void SerializeRotation2D(NetworkWriter writer, float rot, CompressionSyncMode compression)
		{
			WriteAngle(writer, rot, compression);
		}

		public static void SerializeSpin3D(NetworkWriter writer, Vector3 angularVelocity, AxisSyncMode mode, CompressionSyncMode compression)
		{
			switch (mode)
			{
			case AxisSyncMode.None:
				break;
			case AxisSyncMode.AxisX:
				WriteAngle(writer, angularVelocity.x, compression);
				break;
			case AxisSyncMode.AxisY:
				WriteAngle(writer, angularVelocity.y, compression);
				break;
			case AxisSyncMode.AxisZ:
				WriteAngle(writer, angularVelocity.z, compression);
				break;
			case AxisSyncMode.AxisXY:
				WriteAngle(writer, angularVelocity.x, compression);
				WriteAngle(writer, angularVelocity.y, compression);
				break;
			case AxisSyncMode.AxisXZ:
				WriteAngle(writer, angularVelocity.x, compression);
				WriteAngle(writer, angularVelocity.z, compression);
				break;
			case AxisSyncMode.AxisYZ:
				WriteAngle(writer, angularVelocity.y, compression);
				WriteAngle(writer, angularVelocity.z, compression);
				break;
			case AxisSyncMode.AxisXYZ:
				WriteAngle(writer, angularVelocity.x, compression);
				WriteAngle(writer, angularVelocity.y, compression);
				WriteAngle(writer, angularVelocity.z, compression);
				break;
			}
		}

		public static void SerializeSpin2D(NetworkWriter writer, float angularVelocity, CompressionSyncMode compression)
		{
			WriteAngle(writer, angularVelocity, compression);
		}

		public static Vector3 UnserializeVelocity3D(NetworkReader reader, CompressionSyncMode compression)
		{
			return reader.ReadVector3();
		}

		public static Vector3 UnserializeVelocity2D(NetworkReader reader, CompressionSyncMode compression)
		{
			return reader.ReadVector2();
		}

		public static Quaternion UnserializeRotation3D(NetworkReader reader, AxisSyncMode mode, CompressionSyncMode compression)
		{
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			switch (mode)
			{
			case AxisSyncMode.AxisX:
				zero.Set(ReadAngle(reader, compression), 0f, 0f);
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisY:
				zero.Set(0f, ReadAngle(reader, compression), 0f);
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisZ:
				zero.Set(0f, 0f, ReadAngle(reader, compression));
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisXY:
				zero.Set(ReadAngle(reader, compression), ReadAngle(reader, compression), 0f);
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisXZ:
				zero.Set(ReadAngle(reader, compression), 0f, ReadAngle(reader, compression));
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisYZ:
				zero.Set(0f, ReadAngle(reader, compression), ReadAngle(reader, compression));
				identity.eulerAngles = zero;
				break;
			case AxisSyncMode.AxisXYZ:
				zero.Set(ReadAngle(reader, compression), ReadAngle(reader, compression), ReadAngle(reader, compression));
				identity.eulerAngles = zero;
				break;
			}
			return identity;
		}

		public static float UnserializeRotation2D(NetworkReader reader, CompressionSyncMode compression)
		{
			return ReadAngle(reader, compression);
		}

		public static Vector3 UnserializeSpin3D(NetworkReader reader, AxisSyncMode mode, CompressionSyncMode compression)
		{
			Vector3 zero = Vector3.zero;
			switch (mode)
			{
			case AxisSyncMode.AxisX:
				zero.Set(ReadAngle(reader, compression), 0f, 0f);
				break;
			case AxisSyncMode.AxisY:
				zero.Set(0f, ReadAngle(reader, compression), 0f);
				break;
			case AxisSyncMode.AxisZ:
				zero.Set(0f, 0f, ReadAngle(reader, compression));
				break;
			case AxisSyncMode.AxisXY:
				zero.Set(ReadAngle(reader, compression), ReadAngle(reader, compression), 0f);
				break;
			case AxisSyncMode.AxisXZ:
				zero.Set(ReadAngle(reader, compression), 0f, ReadAngle(reader, compression));
				break;
			case AxisSyncMode.AxisYZ:
				zero.Set(0f, ReadAngle(reader, compression), ReadAngle(reader, compression));
				break;
			case AxisSyncMode.AxisXYZ:
				zero.Set(ReadAngle(reader, compression), ReadAngle(reader, compression), ReadAngle(reader, compression));
				break;
			}
			return zero;
		}

		public static float UnserializeSpin2D(NetworkReader reader, CompressionSyncMode compression)
		{
			return ReadAngle(reader, compression);
		}

		public override int GetNetworkChannel()
		{
			return 1;
		}

		public override float GetNetworkSendInterval()
		{
			return m_SendInterval;
		}

		public override void OnStartAuthority()
		{
			m_LastClientSyncTime = 0f;
		}
	}
}
