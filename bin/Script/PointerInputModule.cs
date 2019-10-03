using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
	public abstract class PointerInputModule : BaseInputModule
	{
		protected class ButtonState
		{
			private PointerEventData.InputButton m_Button = PointerEventData.InputButton.Left;

			private MouseButtonEventData m_EventData;

			public MouseButtonEventData eventData
			{
				get
				{
					return m_EventData;
				}
				set
				{
					m_EventData = value;
				}
			}

			public PointerEventData.InputButton button
			{
				get
				{
					return m_Button;
				}
				set
				{
					m_Button = value;
				}
			}
		}

		protected class MouseState
		{
			private List<ButtonState> m_TrackedButtons = new List<ButtonState>();

			public bool AnyPressesThisFrame()
			{
				for (int i = 0; i < m_TrackedButtons.Count; i++)
				{
					if (m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			public bool AnyReleasesThisFrame()
			{
				for (int i = 0; i < m_TrackedButtons.Count; i++)
				{
					if (m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			public ButtonState GetButtonState(PointerEventData.InputButton button)
			{
				ButtonState buttonState = null;
				for (int i = 0; i < m_TrackedButtons.Count; i++)
				{
					if (m_TrackedButtons[i].button == button)
					{
						buttonState = m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					ButtonState buttonState2 = new ButtonState();
					buttonState2.button = button;
					buttonState2.eventData = new MouseButtonEventData();
					buttonState = buttonState2;
					m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			public void SetButtonState(PointerEventData.InputButton button, PointerEventData.FramePressState stateForMouseButton, PointerEventData data)
			{
				ButtonState buttonState = GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}
		}

		public class MouseButtonEventData
		{
			public PointerEventData.FramePressState buttonState;

			public PointerEventData buttonData;

			public bool PressedThisFrame()
			{
				return buttonState == PointerEventData.FramePressState.Pressed || buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			public bool ReleasedThisFrame()
			{
				return buttonState == PointerEventData.FramePressState.Released || buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}
		}

		public const int kMouseLeftId = -1;

		public const int kMouseRightId = -2;

		public const int kMouseMiddleId = -3;

		public const int kFakeTouchesId = -4;

		protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData>();

		private readonly MouseState m_MouseState = new MouseState();

		protected bool GetPointerData(int id, out PointerEventData data, bool create)
		{
			if (!m_PointerData.TryGetValue(id, out data) && create)
			{
				data = new PointerEventData(base.eventSystem)
				{
					pointerId = id
				};
				m_PointerData.Add(id, data);
				return true;
			}
			return false;
		}

		protected void RemovePointerData(PointerEventData data)
		{
			m_PointerData.Remove(data.pointerId);
		}

		protected PointerEventData GetTouchPointerEventData(Touch input, out bool pressed, out bool released)
		{
			PointerEventData data;
			bool pointerData = GetPointerData(input.fingerId, out data, create: true);
			data.Reset();
			pressed = (pointerData || input.phase == TouchPhase.Began);
			released = (input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended);
			if (pointerData)
			{
				data.position = input.position;
			}
			if (pressed)
			{
				data.delta = Vector2.zero;
			}
			else
			{
				data.delta = input.position - data.position;
			}
			data.position = input.position;
			data.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(data, m_RaycastResultCache);
			RaycastResult raycastResult2 = data.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(m_RaycastResultCache);
			m_RaycastResultCache.Clear();
			return data;
		}

		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		protected PointerEventData.FramePressState StateForMouseButton(int buttonId)
		{
			bool mouseButtonDown = base.input.GetMouseButtonDown(buttonId);
			bool mouseButtonUp = base.input.GetMouseButtonUp(buttonId);
			if (mouseButtonDown && mouseButtonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (mouseButtonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (mouseButtonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		protected virtual MouseState GetMousePointerEventData()
		{
			return GetMousePointerEventData(0);
		}

		protected virtual MouseState GetMousePointerEventData(int id)
		{
			PointerEventData data;
			bool pointerData = GetPointerData(-1, out data, create: true);
			data.Reset();
			if (pointerData)
			{
				data.position = base.input.mousePosition;
			}
			Vector2 mousePosition = base.input.mousePosition;
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				data.position = new Vector2(-1f, -1f);
				data.delta = Vector2.zero;
			}
			else
			{
				data.delta = mousePosition - data.position;
				data.position = mousePosition;
			}
			data.scrollDelta = base.input.mouseScrollDelta;
			data.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(data, m_RaycastResultCache);
			RaycastResult raycastResult2 = data.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(m_RaycastResultCache);
			m_RaycastResultCache.Clear();
			GetPointerData(-2, out PointerEventData data2, create: true);
			CopyFromTo(data, data2);
			data2.button = PointerEventData.InputButton.Right;
			GetPointerData(-3, out PointerEventData data3, create: true);
			CopyFromTo(data, data3);
			data3.button = PointerEventData.InputButton.Middle;
			m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), data);
			m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), data2);
			m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), data3);
			return m_MouseState;
		}

		protected PointerEventData GetLastPointerEventData(int id)
		{
			GetPointerData(id, out PointerEventData data, create: false);
			return data;
		}

		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			if (!useDragThreshold)
			{
				return true;
			}
			return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		protected virtual void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject newEnterTarget = (Cursor.lockState != CursorLockMode.Locked) ? pointerEvent.pointerCurrentRaycast.gameObject : null;
			HandlePointerExitAndEnter(pointerEvent, newEnterTarget);
		}

		protected virtual void ProcessDrag(PointerEventData pointerEvent)
		{
			if (!pointerEvent.IsPointerMoving() || Cursor.lockState == CursorLockMode.Locked || pointerEvent.pointerDrag == null)
			{
				return;
			}
			if (!pointerEvent.dragging && ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		public override bool IsPointerOverGameObject(int pointerId)
		{
			PointerEventData lastPointerEventData = GetLastPointerEventData(pointerId);
			if (lastPointerEventData != null)
			{
				return lastPointerEventData.pointerEnter != null;
			}
			return false;
		}

		protected void ClearSelection()
		{
			BaseEventData baseEventData = GetBaseEventData();
			foreach (PointerEventData value in m_PointerData.Values)
			{
				HandlePointerExitAndEnter(value, null);
			}
			m_PointerData.Clear();
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + GetType());
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, PointerEventData> pointerDatum in m_PointerData)
			{
				if (pointerDatum.Value != null)
				{
					stringBuilder.AppendLine("<B>Pointer:</b> " + pointerDatum.Key);
					stringBuilder.AppendLine(pointerDatum.Value.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
			if (eventHandler != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}
	}
}
