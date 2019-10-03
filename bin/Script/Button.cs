using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Button", 30)]
	public class Button : Selectable, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
	{
		[Serializable]
		public class ButtonClickedEvent : UnityEvent
		{
		}

		[FormerlySerializedAs("onClick")]
		[SerializeField]
		private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

		public ButtonClickedEvent onClick
		{
			get
			{
				return m_OnClick;
			}
			set
			{
				m_OnClick = value;
			}
		}

		protected Button()
		{
		}

		private void Press()
		{
			if (IsActive() && IsInteractable())
			{
				m_OnClick.Invoke();
			}
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				Press();
			}
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			Press();
			if (IsActive() && IsInteractable())
			{
				DoStateTransition(SelectionState.Pressed, instant: false);
				StartCoroutine(OnFinishSubmit());
			}
		}

		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			DoStateTransition(base.currentSelectionState, instant: false);
		}
	}
}
