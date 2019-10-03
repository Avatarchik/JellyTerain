using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Slider", 33)]
	[RequireComponent(typeof(RectTransform))]
	public class Slider : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement, IEventSystemHandler
	{
		public enum Direction
		{
			LeftToRight,
			RightToLeft,
			BottomToTop,
			TopToBottom
		}

		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		private enum Axis
		{
			Horizontal,
			Vertical
		}

		[SerializeField]
		private RectTransform m_FillRect;

		[SerializeField]
		private RectTransform m_HandleRect;

		[Space]
		[SerializeField]
		private Direction m_Direction = Direction.LeftToRight;

		[SerializeField]
		private float m_MinValue = 0f;

		[SerializeField]
		private float m_MaxValue = 1f;

		[SerializeField]
		private bool m_WholeNumbers = false;

		[SerializeField]
		protected float m_Value;

		[Space]
		[SerializeField]
		private SliderEvent m_OnValueChanged = new SliderEvent();

		private Image m_FillImage;

		private Transform m_FillTransform;

		private RectTransform m_FillContainerRect;

		private Transform m_HandleTransform;

		private RectTransform m_HandleContainerRect;

		private Vector2 m_Offset = Vector2.zero;

		private DrivenRectTransformTracker m_Tracker;

		public RectTransform fillRect
		{
			get
			{
				return m_FillRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_FillRect, value))
				{
					UpdateCachedReferences();
					UpdateVisuals();
				}
			}
		}

		public RectTransform handleRect
		{
			get
			{
				return m_HandleRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_HandleRect, value))
				{
					UpdateCachedReferences();
					UpdateVisuals();
				}
			}
		}

		public Direction direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_Direction, value))
				{
					UpdateVisuals();
				}
			}
		}

		public float minValue
		{
			get
			{
				return m_MinValue;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_MinValue, value))
				{
					Set(m_Value);
					UpdateVisuals();
				}
			}
		}

		public float maxValue
		{
			get
			{
				return m_MaxValue;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_MaxValue, value))
				{
					Set(m_Value);
					UpdateVisuals();
				}
			}
		}

		public bool wholeNumbers
		{
			get
			{
				return m_WholeNumbers;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_WholeNumbers, value))
				{
					Set(m_Value);
					UpdateVisuals();
				}
			}
		}

		public virtual float value
		{
			get
			{
				if (wholeNumbers)
				{
					return Mathf.Round(m_Value);
				}
				return m_Value;
			}
			set
			{
				Set(value);
			}
		}

		public float normalizedValue
		{
			get
			{
				if (Mathf.Approximately(minValue, maxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(minValue, maxValue, value);
			}
			set
			{
				this.value = Mathf.Lerp(minValue, maxValue, value);
			}
		}

		public SliderEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		private float stepSize => (!wholeNumbers) ? ((maxValue - minValue) * 0.1f) : 1f;

		private Axis axis => (m_Direction != 0 && m_Direction != Direction.RightToLeft) ? Axis.Vertical : Axis.Horizontal;

		private bool reverseValue => m_Direction == Direction.RightToLeft || m_Direction == Direction.TopToBottom;

		protected Slider()
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
			UpdateCachedReferences();
			Set(m_Value, sendCallback: false);
			UpdateVisuals();
		}

		protected override void OnDisable()
		{
			m_Tracker.Clear();
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			m_Value = ClampValue(m_Value);
			float num = normalizedValue;
			if (m_FillContainerRect != null)
			{
				num = ((!(m_FillImage != null) || m_FillImage.type != Image.Type.Filled) ? ((!reverseValue) ? m_FillRect.anchorMax[(int)axis] : (1f - m_FillRect.anchorMin[(int)axis])) : m_FillImage.fillAmount);
			}
			else if (m_HandleContainerRect != null)
			{
				num = ((!reverseValue) ? m_HandleRect.anchorMin[(int)axis] : (1f - m_HandleRect.anchorMin[(int)axis]));
			}
			UpdateVisuals();
			if (num != normalizedValue)
			{
				onValueChanged.Invoke(m_Value);
			}
		}

		private void UpdateCachedReferences()
		{
			if ((bool)m_FillRect)
			{
				m_FillTransform = m_FillRect.transform;
				m_FillImage = m_FillRect.GetComponent<Image>();
				if (m_FillTransform.parent != null)
				{
					m_FillContainerRect = m_FillTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				m_FillContainerRect = null;
				m_FillImage = null;
			}
			if ((bool)m_HandleRect)
			{
				m_HandleTransform = m_HandleRect.transform;
				if (m_HandleTransform.parent != null)
				{
					m_HandleContainerRect = m_HandleTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				m_HandleContainerRect = null;
			}
		}

		private float ClampValue(float input)
		{
			float num = Mathf.Clamp(input, minValue, maxValue);
			if (wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		private void Set(float input)
		{
			Set(input, sendCallback: true);
		}

		protected virtual void Set(float input, bool sendCallback)
		{
			float num = ClampValue(input);
			if (m_Value != num)
			{
				m_Value = num;
				UpdateVisuals();
				if (sendCallback)
				{
					m_OnValueChanged.Invoke(num);
				}
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (IsActive())
			{
				UpdateVisuals();
			}
		}

		private void UpdateVisuals()
		{
			m_Tracker.Clear();
			if (m_FillContainerRect != null)
			{
				m_Tracker.Add(this, m_FillRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				if (m_FillImage != null && m_FillImage.type == Image.Type.Filled)
				{
					m_FillImage.fillAmount = normalizedValue;
				}
				else if (reverseValue)
				{
					zero[(int)this.axis] = 1f - normalizedValue;
				}
				else
				{
					one[(int)this.axis] = normalizedValue;
				}
				m_FillRect.anchorMin = zero;
				m_FillRect.anchorMax = one;
			}
			if (m_HandleContainerRect != null)
			{
				m_Tracker.Add(this, m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero2 = Vector2.zero;
				Vector2 one2 = Vector2.one;
				Axis axis = this.axis;
				float value = (!reverseValue) ? normalizedValue : (1f - normalizedValue);
				one2[(int)this.axis] = value;
				zero2[(int)axis] = value;
				m_HandleRect.anchorMin = zero2;
				m_HandleRect.anchorMax = one2;
			}
		}

		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform rectTransform = m_HandleContainerRect ?? m_FillContainerRect;
			if (rectTransform != null && rectTransform.rect.size[(int)axis] > 0f && RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, cam, out Vector2 localPoint))
			{
				localPoint -= rectTransform.rect.position;
				float num = Mathf.Clamp01((localPoint - m_Offset)[(int)axis] / rectTransform.rect.size[(int)axis]);
				normalizedValue = ((!reverseValue) ? num : (1f - num));
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			m_Offset = Vector2.zero;
			if (m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.position, eventData.enterEventCamera))
			{
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
				{
					m_Offset = localPoint;
				}
			}
			else
			{
				UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		public override void OnMove(AxisEventData eventData)
		{
			if (!IsActive() || !IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (axis == Axis.Horizontal && FindSelectableOnLeft() == null)
				{
					Set((!reverseValue) ? (value - stepSize) : (value + stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Right:
				if (axis == Axis.Horizontal && FindSelectableOnRight() == null)
				{
					Set((!reverseValue) ? (value + stepSize) : (value - stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Up:
				if (axis == Axis.Vertical && FindSelectableOnUp() == null)
				{
					Set((!reverseValue) ? (value + stepSize) : (value - stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Down:
				if (axis == Axis.Vertical && FindSelectableOnDown() == null)
				{
					Set((!reverseValue) ? (value - stepSize) : (value + stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			}
		}

		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		public void SetDirection(Direction direction, bool includeRectLayouts)
		{
			Axis axis = this.axis;
			bool reverseValue = this.reverseValue;
			this.direction = direction;
			if (includeRectLayouts)
			{
				if (this.axis != axis)
				{
					RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, keepPositioning: true, recursive: true);
				}
				if (this.reverseValue != reverseValue)
				{
					RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, keepPositioning: true, recursive: true);
				}
			}
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}
	}
}
