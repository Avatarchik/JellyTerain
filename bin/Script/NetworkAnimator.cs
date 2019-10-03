using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkAnimator")]
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(Animator))]
	public class NetworkAnimator : NetworkBehaviour
	{
		[SerializeField]
		private Animator m_Animator;

		[SerializeField]
		private uint m_ParameterSendBits;

		private static AnimationMessage s_AnimationMessage = new AnimationMessage();

		private static AnimationParametersMessage s_AnimationParametersMessage = new AnimationParametersMessage();

		private static AnimationTriggerMessage s_AnimationTriggerMessage = new AnimationTriggerMessage();

		private int m_AnimationHash;

		private int m_TransitionHash;

		private NetworkWriter m_ParameterWriter;

		private float m_SendTimer;

		public string param0;

		public string param1;

		public string param2;

		public string param3;

		public string param4;

		public string param5;

		public Animator animator
		{
			get
			{
				return m_Animator;
			}
			set
			{
				m_Animator = value;
				ResetParameterOptions();
			}
		}

		public void SetParameterAutoSend(int index, bool value)
		{
			if (value)
			{
				m_ParameterSendBits |= (uint)(1 << index);
			}
			else
			{
				m_ParameterSendBits &= (uint)(~(1 << index));
			}
		}

		public bool GetParameterAutoSend(int index)
		{
			return ((int)m_ParameterSendBits & (1 << index)) != 0;
		}

		internal void ResetParameterOptions()
		{
			Debug.Log("ResetParameterOptions");
			m_ParameterSendBits = 0u;
		}

		public override void OnStartAuthority()
		{
			m_ParameterWriter = new NetworkWriter();
		}

		private void FixedUpdate()
		{
			if (m_ParameterWriter == null)
			{
				return;
			}
			CheckSendRate();
			if (CheckAnimStateChanged(out int stateHash, out float normalizedTime))
			{
				AnimationMessage animationMessage = new AnimationMessage();
				animationMessage.netId = base.netId;
				animationMessage.stateHash = stateHash;
				animationMessage.normalizedTime = normalizedTime;
				m_ParameterWriter.SeekZero();
				WriteParameters(m_ParameterWriter, autoSend: false);
				animationMessage.parameters = m_ParameterWriter.ToArray();
				if (base.hasAuthority && ClientScene.readyConnection != null)
				{
					ClientScene.readyConnection.Send(40, animationMessage);
				}
				else if (base.isServer && !base.localPlayerAuthority)
				{
					NetworkServer.SendToReady(base.gameObject, 40, animationMessage);
				}
			}
		}

		private bool CheckAnimStateChanged(out int stateHash, out float normalizedTime)
		{
			stateHash = 0;
			normalizedTime = 0f;
			if (m_Animator.IsInTransition(0))
			{
				AnimatorTransitionInfo animatorTransitionInfo = m_Animator.GetAnimatorTransitionInfo(0);
				if (animatorTransitionInfo.fullPathHash != m_TransitionHash)
				{
					m_TransitionHash = animatorTransitionInfo.fullPathHash;
					m_AnimationHash = 0;
					return true;
				}
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.fullPathHash != m_AnimationHash)
			{
				if (m_AnimationHash != 0)
				{
					stateHash = currentAnimatorStateInfo.fullPathHash;
					normalizedTime = currentAnimatorStateInfo.normalizedTime;
				}
				m_TransitionHash = 0;
				m_AnimationHash = currentAnimatorStateInfo.fullPathHash;
				return true;
			}
			return false;
		}

		private void CheckSendRate()
		{
			if (GetNetworkSendInterval() != 0f && m_SendTimer < Time.time)
			{
				m_SendTimer = Time.time + GetNetworkSendInterval();
				AnimationParametersMessage animationParametersMessage = new AnimationParametersMessage();
				animationParametersMessage.netId = base.netId;
				m_ParameterWriter.SeekZero();
				WriteParameters(m_ParameterWriter, autoSend: true);
				animationParametersMessage.parameters = m_ParameterWriter.ToArray();
				if (base.hasAuthority && ClientScene.readyConnection != null)
				{
					ClientScene.readyConnection.Send(41, animationParametersMessage);
				}
				else if (base.isServer && !base.localPlayerAuthority)
				{
					NetworkServer.SendToReady(base.gameObject, 41, animationParametersMessage);
				}
			}
		}

		private void SetSendTrackingParam(string p, int i)
		{
			p = "Sent Param: " + p;
			if (i == 0)
			{
				param0 = p;
			}
			if (i == 1)
			{
				param1 = p;
			}
			if (i == 2)
			{
				param2 = p;
			}
			if (i == 3)
			{
				param3 = p;
			}
			if (i == 4)
			{
				param4 = p;
			}
			if (i == 5)
			{
				param5 = p;
			}
		}

		private void SetRecvTrackingParam(string p, int i)
		{
			p = "Recv Param: " + p;
			if (i == 0)
			{
				param0 = p;
			}
			if (i == 1)
			{
				param1 = p;
			}
			if (i == 2)
			{
				param2 = p;
			}
			if (i == 3)
			{
				param3 = p;
			}
			if (i == 4)
			{
				param4 = p;
			}
			if (i == 5)
			{
				param5 = p;
			}
		}

		internal void HandleAnimMsg(AnimationMessage msg, NetworkReader reader)
		{
			if (!base.hasAuthority)
			{
				if (msg.stateHash != 0)
				{
					m_Animator.Play(msg.stateHash, 0, msg.normalizedTime);
				}
				ReadParameters(reader, autoSend: false);
			}
		}

		internal void HandleAnimParamsMsg(AnimationParametersMessage msg, NetworkReader reader)
		{
			if (!base.hasAuthority)
			{
				ReadParameters(reader, autoSend: true);
			}
		}

		internal void HandleAnimTriggerMsg(int hash)
		{
			m_Animator.SetTrigger(hash);
		}

		private void WriteParameters(NetworkWriter writer, bool autoSend)
		{
			for (int i = 0; i < m_Animator.parameters.Length; i++)
			{
				if (!autoSend || GetParameterAutoSend(i))
				{
					AnimatorControllerParameter animatorControllerParameter = m_Animator.parameters[i];
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Int)
					{
						writer.WritePackedUInt32((uint)m_Animator.GetInteger(animatorControllerParameter.nameHash));
						SetSendTrackingParam(animatorControllerParameter.name + ":" + m_Animator.GetInteger(animatorControllerParameter.nameHash), i);
					}
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						writer.Write(m_Animator.GetFloat(animatorControllerParameter.nameHash));
						SetSendTrackingParam(animatorControllerParameter.name + ":" + m_Animator.GetFloat(animatorControllerParameter.nameHash), i);
					}
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						writer.Write(m_Animator.GetBool(animatorControllerParameter.nameHash));
						SetSendTrackingParam(animatorControllerParameter.name + ":" + m_Animator.GetBool(animatorControllerParameter.nameHash), i);
					}
				}
			}
		}

		private void ReadParameters(NetworkReader reader, bool autoSend)
		{
			for (int i = 0; i < m_Animator.parameters.Length; i++)
			{
				if (!autoSend || GetParameterAutoSend(i))
				{
					AnimatorControllerParameter animatorControllerParameter = m_Animator.parameters[i];
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Int)
					{
						int num = (int)reader.ReadPackedUInt32();
						m_Animator.SetInteger(animatorControllerParameter.nameHash, num);
						SetRecvTrackingParam(animatorControllerParameter.name + ":" + num, i);
					}
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						float num2 = reader.ReadSingle();
						m_Animator.SetFloat(animatorControllerParameter.nameHash, num2);
						SetRecvTrackingParam(animatorControllerParameter.name + ":" + num2, i);
					}
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						bool flag = reader.ReadBoolean();
						m_Animator.SetBool(animatorControllerParameter.nameHash, flag);
						SetRecvTrackingParam(animatorControllerParameter.name + ":" + flag, i);
					}
				}
			}
		}

		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			if (forceAll)
			{
				if (m_Animator.IsInTransition(0))
				{
					AnimatorStateInfo nextAnimatorStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
					writer.Write(nextAnimatorStateInfo.fullPathHash);
					writer.Write(nextAnimatorStateInfo.normalizedTime);
				}
				else
				{
					AnimatorStateInfo currentAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
					writer.Write(currentAnimatorStateInfo.fullPathHash);
					writer.Write(currentAnimatorStateInfo.normalizedTime);
				}
				WriteParameters(writer, autoSend: false);
				return true;
			}
			return false;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				int stateNameHash = reader.ReadInt32();
				float normalizedTime = reader.ReadSingle();
				ReadParameters(reader, autoSend: false);
				m_Animator.Play(stateNameHash, 0, normalizedTime);
			}
		}

		public void SetTrigger(string triggerName)
		{
			SetTrigger(Animator.StringToHash(triggerName));
		}

		public void SetTrigger(int hash)
		{
			AnimationTriggerMessage animationTriggerMessage = new AnimationTriggerMessage();
			animationTriggerMessage.netId = base.netId;
			animationTriggerMessage.hash = hash;
			if (base.hasAuthority && base.localPlayerAuthority)
			{
				if (NetworkClient.allClients.Count > 0)
				{
					ClientScene.readyConnection?.Send(42, animationTriggerMessage);
				}
			}
			else if (base.isServer && !base.localPlayerAuthority)
			{
				NetworkServer.SendToReady(base.gameObject, 42, animationTriggerMessage);
			}
		}

		internal static void OnAnimationServerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationMessage);
			if (LogFilter.logDev)
			{
				Debug.Log("OnAnimationMessage for netId=" + s_AnimationMessage.netId + " conn=" + netMsg.conn);
			}
			GameObject gameObject = NetworkServer.FindLocalObject(s_AnimationMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					NetworkReader reader = new NetworkReader(s_AnimationMessage.parameters);
					component.HandleAnimMsg(s_AnimationMessage, reader);
					NetworkServer.SendToReady(gameObject, 40, s_AnimationMessage);
				}
			}
		}

		internal static void OnAnimationParametersServerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationParametersMessage);
			if (LogFilter.logDev)
			{
				Debug.Log("OnAnimationParametersMessage for netId=" + s_AnimationParametersMessage.netId + " conn=" + netMsg.conn);
			}
			GameObject gameObject = NetworkServer.FindLocalObject(s_AnimationParametersMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					NetworkReader reader = new NetworkReader(s_AnimationParametersMessage.parameters);
					component.HandleAnimParamsMsg(s_AnimationParametersMessage, reader);
					NetworkServer.SendToReady(gameObject, 41, s_AnimationParametersMessage);
				}
			}
		}

		internal static void OnAnimationTriggerServerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationTriggerMessage);
			if (LogFilter.logDev)
			{
				Debug.Log("OnAnimationTriggerMessage for netId=" + s_AnimationTriggerMessage.netId + " conn=" + netMsg.conn);
			}
			GameObject gameObject = NetworkServer.FindLocalObject(s_AnimationTriggerMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					component.HandleAnimTriggerMsg(s_AnimationTriggerMessage.hash);
					NetworkServer.SendToReady(gameObject, 42, s_AnimationTriggerMessage);
				}
			}
		}

		internal static void OnAnimationClientMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationMessage);
			GameObject gameObject = ClientScene.FindLocalObject(s_AnimationMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					NetworkReader reader = new NetworkReader(s_AnimationMessage.parameters);
					component.HandleAnimMsg(s_AnimationMessage, reader);
				}
			}
		}

		internal static void OnAnimationParametersClientMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationParametersMessage);
			GameObject gameObject = ClientScene.FindLocalObject(s_AnimationParametersMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					NetworkReader reader = new NetworkReader(s_AnimationParametersMessage.parameters);
					component.HandleAnimParamsMsg(s_AnimationParametersMessage, reader);
				}
			}
		}

		internal static void OnAnimationTriggerClientMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage(s_AnimationTriggerMessage);
			GameObject gameObject = ClientScene.FindLocalObject(s_AnimationTriggerMessage.netId);
			if (!(gameObject == null))
			{
				NetworkAnimator component = gameObject.GetComponent<NetworkAnimator>();
				if (component != null)
				{
					component.HandleAnimTriggerMsg(s_AnimationTriggerMessage.hash);
				}
			}
		}
	}
}
