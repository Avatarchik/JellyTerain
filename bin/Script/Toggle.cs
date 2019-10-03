using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Toggle", 31)]
	[RequireComponent(typeof(RectTransform))]
	public class Toggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEventSystemHandler
	{
		public enum ToggleTransition
		{
			None,
			Fade
		}

		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		public ToggleTransition toggleTransition = ToggleTransition.Fade;

		public Graphic graphic;

		[SerializeField]
		private ToggleGroup m_Group;

		public ToggleEvent onValueChanged = new ToggleEvent();

		[FormerlySerializedAs("m_IsActive")]
		[Tooltip("Is the toggle currently on or off?")]
		[SerializeField]
		private bool m_IsOn;

		public ToggleGroup group
		{
			get
			{
				return m_Group;
			}
			set
			{
				m_Group = value;
				SetToggleGroup(m_Group, setMemberValue: true);
				PlayEffect(instant: true);
			}
		}

		public bool isOn
		{
			get
			{
				return m_IsOn;
			}
			set
			{
				Set(value);
			}
		}

		protected Toggle()
		{
		}

		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetToggleGroup(m_Group, setMemberValue: false);
			PlayEffect(instant: true);
		}

		protected override void OnDisable()
		{
			SetToggleGroup(null, setMemberValue: false);
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			if (graphic != null)
			{
				Color color = graphic.canvasRenderer.GetColor();
				bool flag = !Mathf.Approximately(color.a, 0f);
				if (m_IsOn != flag)
				{
					m_IsOn = flag;
					Set(!flag);
				}
			}
			base.OnDidApplyAnimationProperties();
		}

		private void SetToggleGroup(ToggleGroup newGroup, bool setMemberValue)
		{
			ToggleGroup group = m_Group;
			if (m_Group != null)
			{
				m_Group.UnregisterToggle(this);
			}
			if (setMemberValue)
			{
				m_Group = newGroup;
			}
			if (newGroup != null && IsActive())
			{
				newGroup.RegisterToggle(this);
			}
			if (newGroup != null && newGroup != group && isOn && IsActive())
			{
				newGroup.NotifyToggleOn(this);
			}
		}

		private void Set(bool value)
		{
			Set(value, sendCallback: true);
		}

		private void Set(bool value, bool sendCallback)
		{
			if (m_IsOn != value)
			{
				m_IsOn = value;
				if (m_Group != null && IsActive() && (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff)))
				{
					m_IsOn = true;
					m_Group.NotifyToggleOn(this);
				}
				PlayEffect(toggleTransition == ToggleTransition.None);
				if (sendCallback)
				{
					onValueChanged.Invoke(m_IsOn);
				}
			}
		}

		private void PlayEffect(bool instant)
		{
			if (!(graphic == null))
			{
				graphic.CrossFadeAlpha((!m_IsOn) ? 0f : 1f, (!instant) ? 0.1f : 0f, ignoreTimeScale: true);
			}
		}

		protected override void Start()
		{
			PlayEffect(instant: true);
		}

		private void InternalToggle()
		{
			if (IsActive() && IsInteractable())
			{
				isOn = !isOn;
			}
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				InternalToggle();
			}
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			InternalToggle();
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}
	}
}
