using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Input Field", 31)]
	public class InputField : Selectable, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, ILayoutElement, IEventSystemHandler
	{
		public enum ContentType
		{
			Standard,
			Autocorrected,
			IntegerNumber,
			DecimalNumber,
			Alphanumeric,
			Name,
			EmailAddress,
			Password,
			Pin,
			Custom
		}

		public enum InputType
		{
			Standard,
			AutoCorrect,
			Password
		}

		public enum CharacterValidation
		{
			None,
			Integer,
			Decimal,
			Alphanumeric,
			Name,
			EmailAddress
		}

		public enum LineType
		{
			SingleLine,
			MultiLineSubmit,
			MultiLineNewline
		}

		public delegate char OnValidateInput(string text, int charIndex, char addedChar);

		[Serializable]
		public class SubmitEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class OnChangeEvent : UnityEvent<string>
		{
		}

		protected enum EditState
		{
			Continue,
			Finish
		}

		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[6]
		{
			' ',
			'.',
			',',
			'\t',
			'\r',
			'\n'
		};

		[SerializeField]
		[FormerlySerializedAs("text")]
		protected Text m_TextComponent;

		[SerializeField]
		protected Graphic m_Placeholder;

		[SerializeField]
		private ContentType m_ContentType = ContentType.Standard;

		[FormerlySerializedAs("inputType")]
		[SerializeField]
		private InputType m_InputType = InputType.Standard;

		[FormerlySerializedAs("asteriskChar")]
		[SerializeField]
		private char m_AsteriskChar = '*';

		[FormerlySerializedAs("keyboardType")]
		[SerializeField]
		private TouchScreenKeyboardType m_KeyboardType = TouchScreenKeyboardType.Default;

		[SerializeField]
		private LineType m_LineType = LineType.SingleLine;

		[FormerlySerializedAs("hideMobileInput")]
		[SerializeField]
		private bool m_HideMobileInput = false;

		[FormerlySerializedAs("validation")]
		[SerializeField]
		private CharacterValidation m_CharacterValidation = CharacterValidation.None;

		[FormerlySerializedAs("characterLimit")]
		[SerializeField]
		private int m_CharacterLimit = 0;

		[FormerlySerializedAs("onSubmit")]
		[FormerlySerializedAs("m_OnSubmit")]
		[FormerlySerializedAs("m_EndEdit")]
		[SerializeField]
		private SubmitEvent m_OnEndEdit = new SubmitEvent();

		[FormerlySerializedAs("onValueChange")]
		[FormerlySerializedAs("m_OnValueChange")]
		[SerializeField]
		private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

		[FormerlySerializedAs("onValidateInput")]
		[SerializeField]
		private OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor = false;

		[FormerlySerializedAs("selectionColor")]
		[SerializeField]
		private Color m_SelectionColor = new Color(56f / 85f, 206f / 255f, 1f, 64f / 85f);

		[SerializeField]
		[FormerlySerializedAs("mValue")]
		protected string m_Text = string.Empty;

		[SerializeField]
		[Range(0f, 4f)]
		private float m_CaretBlinkRate = 0.85f;

		[SerializeField]
		[Range(1f, 5f)]
		private int m_CaretWidth = 1;

		[SerializeField]
		private bool m_ReadOnly = false;

		protected int m_CaretPosition = 0;

		protected int m_CaretSelectPosition = 0;

		private RectTransform caretRectTrans = null;

		protected UIVertex[] m_CursorVerts = null;

		private TextGenerator m_InputTextCache;

		private CanvasRenderer m_CachedInputRenderer;

		private bool m_PreventFontCallback = false;

		[NonSerialized]
		protected Mesh m_Mesh;

		private bool m_AllowInput = false;

		private bool m_ShouldActivateNextUpdate = false;

		private bool m_UpdateDrag = false;

		private bool m_DragPositionOutOfBounds = false;

		private const float kHScrollSpeed = 0.05f;

		private const float kVScrollSpeed = 0.1f;

		protected bool m_CaretVisible;

		private Coroutine m_BlinkCoroutine = null;

		private float m_BlinkStartTime = 0f;

		protected int m_DrawStart = 0;

		protected int m_DrawEnd = 0;

		private Coroutine m_DragCoroutine = null;

		private string m_OriginalText = "";

		private bool m_WasCanceled = false;

		private bool m_HasDoneFocusTransition = false;

		private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

		private Event m_ProcessingEvent = new Event();

		private BaseInput input
		{
			get
			{
				if ((bool)EventSystem.current && (bool)EventSystem.current.currentInputModule)
				{
					return EventSystem.current.currentInputModule.input;
				}
				return null;
			}
		}

		private string compositionString => (!(input != null)) ? Input.compositionString : input.compositionString;

		protected Mesh mesh
		{
			get
			{
				if (m_Mesh == null)
				{
					m_Mesh = new Mesh();
				}
				return m_Mesh;
			}
		}

		protected TextGenerator cachedInputTextGenerator
		{
			get
			{
				if (m_InputTextCache == null)
				{
					m_InputTextCache = new TextGenerator();
				}
				return m_InputTextCache;
			}
		}

		public bool shouldHideMobileInput
		{
			get
			{
				switch (Application.platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
				case RuntimePlatform.TizenPlayer:
				case RuntimePlatform.tvOS:
					return m_HideMobileInput;
				default:
					return true;
				}
			}
			set
			{
				SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
			}
		}

		private bool shouldActivateOnSelect => Application.platform != RuntimePlatform.tvOS;

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (text == value)
				{
					return;
				}
				if (value == null)
				{
					value = "";
				}
				value = value.Replace("\0", string.Empty);
				if (m_LineType == LineType.SingleLine)
				{
					value = value.Replace("\n", "").Replace("\t", "");
				}
				if (this.onValidateInput != null || characterValidation != 0)
				{
					m_Text = "";
					OnValidateInput onValidateInput = this.onValidateInput ?? new OnValidateInput(Validate);
					m_CaretPosition = (m_CaretSelectPosition = value.Length);
					int num = (characterLimit <= 0) ? value.Length : Math.Min(characterLimit, value.Length);
					for (int i = 0; i < num; i++)
					{
						char c = onValidateInput(m_Text, m_Text.Length, value[i]);
						if (c != 0)
						{
							m_Text += c;
						}
					}
				}
				else
				{
					m_Text = ((characterLimit <= 0 || value.Length <= characterLimit) ? value : value.Substring(0, characterLimit));
				}
				if (m_Keyboard != null)
				{
					m_Keyboard.text = m_Text;
				}
				if (m_CaretPosition > m_Text.Length)
				{
					m_CaretPosition = (m_CaretSelectPosition = m_Text.Length);
				}
				else if (m_CaretSelectPosition > m_Text.Length)
				{
					m_CaretSelectPosition = m_Text.Length;
				}
				SendOnValueChangedAndUpdateLabel();
			}
		}

		public bool isFocused => m_AllowInput;

		public float caretBlinkRate
		{
			get
			{
				return m_CaretBlinkRate;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value) && m_AllowInput)
				{
					SetCaretActive();
				}
			}
		}

		public int caretWidth
		{
			get
			{
				return m_CaretWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CaretWidth, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public Text textComponent
		{
			get
			{
				return m_TextComponent;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_TextComponent, value))
				{
					EnforceTextHOverflow();
				}
			}
		}

		public Graphic placeholder
		{
			get
			{
				return m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_Placeholder, value);
			}
		}

		public Color caretColor
		{
			get
			{
				return (!customCaretColor) ? textComponent.color : m_CaretColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref m_CaretColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public bool customCaretColor
		{
			get
			{
				return m_CustomCaretColor;
			}
			set
			{
				if (m_CustomCaretColor != value)
				{
					m_CustomCaretColor = value;
					MarkGeometryAsDirty();
				}
			}
		}

		public Color selectionColor
		{
			get
			{
				return m_SelectionColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref m_SelectionColor, value))
				{
					MarkGeometryAsDirty();
				}
			}
		}

		public SubmitEvent onEndEdit
		{
			get
			{
				return m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndEdit, value);
			}
		}

		[Obsolete("onValueChange has been renamed to onValueChanged")]
		public OnChangeEvent onValueChange
		{
			get
			{
				return onValueChanged;
			}
			set
			{
				onValueChanged = value;
			}
		}

		public OnChangeEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValueChanged, value);
			}
		}

		public OnValidateInput onValidateInput
		{
			get
			{
				return m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValidateInput, value);
			}
		}

		public int characterLimit
		{
			get
			{
				return m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
				{
					UpdateLabel();
				}
			}
		}

		public ContentType contentType
		{
			get
			{
				return m_ContentType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_ContentType, value))
				{
					EnforceContentType();
				}
			}
		}

		public LineType lineType
		{
			get
			{
				return m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_LineType, value))
				{
					SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
					EnforceTextHOverflow();
				}
			}
		}

		public InputType inputType
		{
			get
			{
				return m_InputType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_InputType, value))
				{
					SetToCustom();
				}
			}
		}

		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return m_KeyboardType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_KeyboardType, value))
				{
					SetToCustom();
				}
			}
		}

		public CharacterValidation characterValidation
		{
			get
			{
				return m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value))
				{
					SetToCustom();
				}
			}
		}

		public bool readOnly
		{
			get
			{
				return m_ReadOnly;
			}
			set
			{
				m_ReadOnly = value;
			}
		}

		public bool multiLine => m_LineType == LineType.MultiLineNewline || lineType == LineType.MultiLineSubmit;

		public char asteriskChar
		{
			get
			{
				return m_AsteriskChar;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_AsteriskChar, value))
				{
					UpdateLabel();
				}
			}
		}

		public bool wasCanceled => m_WasCanceled;

		protected int caretPositionInternal
		{
			get
			{
				return m_CaretPosition + compositionString.Length;
			}
			set
			{
				m_CaretPosition = value;
				ClampPos(ref m_CaretPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return m_CaretSelectPosition + compositionString.Length;
			}
			set
			{
				m_CaretSelectPosition = value;
				ClampPos(ref m_CaretSelectPosition);
			}
		}

		private bool hasSelection => caretPositionInternal != caretSelectPositionInternal;

		public int caretPosition
		{
			get
			{
				return m_CaretSelectPosition + compositionString.Length;
			}
			set
			{
				selectionAnchorPosition = value;
				selectionFocusPosition = value;
			}
		}

		public int selectionAnchorPosition
		{
			get
			{
				return m_CaretPosition + compositionString.Length;
			}
			set
			{
				if (compositionString.Length == 0)
				{
					m_CaretPosition = value;
					ClampPos(ref m_CaretPosition);
				}
			}
		}

		public int selectionFocusPosition
		{
			get
			{
				return m_CaretSelectPosition + compositionString.Length;
			}
			set
			{
				if (compositionString.Length == 0)
				{
					m_CaretSelectPosition = value;
					ClampPos(ref m_CaretSelectPosition);
				}
			}
		}

		private static string clipboard
		{
			get
			{
				return GUIUtility.systemCopyBuffer;
			}
			set
			{
				GUIUtility.systemCopyBuffer = value;
			}
		}

		public virtual float minWidth => 0f;

		public virtual float preferredWidth
		{
			get
			{
				if (textComponent == null)
				{
					return 0f;
				}
				TextGenerationSettings generationSettings = textComponent.GetGenerationSettings(Vector2.zero);
				return textComponent.cachedTextGeneratorForLayout.GetPreferredWidth(m_Text, generationSettings) / textComponent.pixelsPerUnit;
			}
		}

		public virtual float flexibleWidth => -1f;

		public virtual float minHeight => 0f;

		public virtual float preferredHeight
		{
			get
			{
				if (this.textComponent == null)
				{
					return 0f;
				}
				Text textComponent = this.textComponent;
				Vector2 size = this.textComponent.rectTransform.rect.size;
				TextGenerationSettings generationSettings = textComponent.GetGenerationSettings(new Vector2(size.x, 0f));
				return this.textComponent.cachedTextGeneratorForLayout.GetPreferredHeight(m_Text, generationSettings) / this.textComponent.pixelsPerUnit;
			}
		}

		public virtual float flexibleHeight => -1f;

		public virtual int layoutPriority => 1;

		protected InputField()
		{
			EnforceTextHOverflow();
		}

		protected void ClampPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > text.Length)
			{
				pos = text.Length;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (m_Text == null)
			{
				m_Text = string.Empty;
			}
			m_DrawStart = 0;
			m_DrawEnd = m_Text.Length;
			if (m_CachedInputRenderer != null)
			{
				m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);
			}
			if (m_TextComponent != null)
			{
				m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
				m_TextComponent.RegisterDirtyMaterialCallback(UpdateCaretMaterial);
				UpdateLabel();
			}
		}

		protected override void OnDisable()
		{
			m_BlinkCoroutine = null;
			DeactivateInputField();
			if (m_TextComponent != null)
			{
				m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
				m_TextComponent.UnregisterDirtyMaterialCallback(UpdateCaretMaterial);
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (m_CachedInputRenderer != null)
			{
				m_CachedInputRenderer.Clear();
			}
			if (m_Mesh != null)
			{
				Object.DestroyImmediate(m_Mesh);
			}
			m_Mesh = null;
			base.OnDisable();
		}

		private IEnumerator CaretBlink()
		{
			m_CaretVisible = true;
			yield return null;
			while (isFocused && m_CaretBlinkRate > 0f)
			{
				float blinkPeriod = 1f / m_CaretBlinkRate;
				bool blinkState = (Time.unscaledTime - m_BlinkStartTime) % blinkPeriod < blinkPeriod / 2f;
				if (m_CaretVisible != blinkState)
				{
					m_CaretVisible = blinkState;
					if (!hasSelection)
					{
						MarkGeometryAsDirty();
					}
				}
				yield return null;
			}
			m_BlinkCoroutine = null;
		}

		private void SetCaretVisible()
		{
			if (m_AllowInput)
			{
				m_CaretVisible = true;
				m_BlinkStartTime = Time.unscaledTime;
				SetCaretActive();
			}
		}

		private void SetCaretActive()
		{
			if (!m_AllowInput)
			{
				return;
			}
			if (m_CaretBlinkRate > 0f)
			{
				if (m_BlinkCoroutine == null)
				{
					m_BlinkCoroutine = StartCoroutine(CaretBlink());
				}
			}
			else
			{
				m_CaretVisible = true;
			}
		}

		private void UpdateCaretMaterial()
		{
			if (m_TextComponent != null && m_CachedInputRenderer != null)
			{
				m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);
			}
		}

		protected void OnFocus()
		{
			SelectAll();
		}

		protected void SelectAll()
		{
			caretPositionInternal = text.Length;
			caretSelectPositionInternal = 0;
		}

		public void MoveTextEnd(bool shift)
		{
			int length = text.Length;
			if (shift)
			{
				caretSelectPositionInternal = length;
			}
			else
			{
				caretPositionInternal = length;
				caretSelectPositionInternal = caretPositionInternal;
			}
			UpdateLabel();
		}

		public void MoveTextStart(bool shift)
		{
			int num = 0;
			if (shift)
			{
				caretSelectPositionInternal = num;
			}
			else
			{
				caretPositionInternal = num;
				caretSelectPositionInternal = caretPositionInternal;
			}
			UpdateLabel();
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.isSupported;
		}

		private void UpdateCaretFromKeyboard()
		{
			RangeInt selection = m_Keyboard.selection;
			int start = selection.start;
			int end = selection.end;
			bool flag = false;
			if (caretPositionInternal != start)
			{
				flag = true;
				caretPositionInternal = start;
			}
			if (caretSelectPositionInternal != end)
			{
				caretSelectPositionInternal = end;
				flag = true;
			}
			if (flag)
			{
				m_BlinkStartTime = Time.unscaledTime;
				UpdateLabel();
			}
		}

		protected virtual void LateUpdate()
		{
			if (m_ShouldActivateNextUpdate)
			{
				if (!isFocused)
				{
					ActivateInputFieldInternal();
					m_ShouldActivateNextUpdate = false;
					return;
				}
				m_ShouldActivateNextUpdate = false;
			}
			if (InPlaceEditing() || !isFocused)
			{
				return;
			}
			AssignPositioningIfNeeded();
			if (m_Keyboard == null || m_Keyboard.done)
			{
				if (m_Keyboard != null)
				{
					if (!m_ReadOnly)
					{
						this.text = m_Keyboard.text;
					}
					if (m_Keyboard.wasCanceled)
					{
						m_WasCanceled = true;
					}
				}
				OnDeselect(null);
				return;
			}
			string text = m_Keyboard.text;
			if (m_Text != text)
			{
				if (m_ReadOnly)
				{
					m_Keyboard.text = m_Text;
				}
				else
				{
					m_Text = "";
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c == '\r' || c == '\u0003')
						{
							c = '\n';
						}
						if (onValidateInput != null)
						{
							c = onValidateInput(m_Text, m_Text.Length, c);
						}
						else if (characterValidation != 0)
						{
							c = Validate(m_Text, m_Text.Length, c);
						}
						if (lineType == LineType.MultiLineSubmit && c == '\n')
						{
							m_Keyboard.text = m_Text;
							OnDeselect(null);
							return;
						}
						if (c != 0)
						{
							m_Text += c;
						}
					}
					if (characterLimit > 0 && m_Text.Length > characterLimit)
					{
						m_Text = m_Text.Substring(0, characterLimit);
					}
					if (!m_Keyboard.canGetSelection)
					{
						int num2 = caretPositionInternal = (caretSelectPositionInternal = m_Text.Length);
					}
					else
					{
						UpdateCaretFromKeyboard();
					}
					if (m_Text != text)
					{
						m_Keyboard.text = m_Text;
					}
					SendOnValueChangedAndUpdateLabel();
				}
			}
			else if (m_Keyboard.canGetSelection)
			{
				UpdateCaretFromKeyboard();
			}
			if (m_Keyboard.done)
			{
				if (m_Keyboard.wasCanceled)
				{
					m_WasCanceled = true;
				}
				OnDeselect(null);
			}
		}

		[Obsolete("This function is no longer used. Please use RectTransformUtility.ScreenPointToLocalPointInRectangle() instead.")]
		public Vector2 ScreenToLocal(Vector2 screen)
		{
			Canvas canvas = m_TextComponent.canvas;
			if (canvas == null)
			{
				return screen;
			}
			Vector3 vector = Vector3.zero;
			if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				vector = m_TextComponent.transform.InverseTransformPoint(screen);
			}
			else if (canvas.worldCamera != null)
			{
				Ray ray = canvas.worldCamera.ScreenPointToRay(screen);
				new Plane(m_TextComponent.transform.forward, m_TextComponent.transform.position).Raycast(ray, out float enter);
				vector = m_TextComponent.transform.InverseTransformPoint(ray.GetPoint(enter));
			}
			return new Vector2(vector.x, vector.y);
		}

		private int GetUnclampedCharacterLineFromPosition(Vector2 pos, TextGenerator generator)
		{
			if (!multiLine)
			{
				return 0;
			}
			float num = pos.y * m_TextComponent.pixelsPerUnit;
			float num2 = 0f;
			for (int i = 0; i < generator.lineCount; i++)
			{
				UILineInfo uILineInfo = generator.lines[i];
				float topY = uILineInfo.topY;
				float num3 = topY;
				UILineInfo uILineInfo2 = generator.lines[i];
				float num4 = num3 - (float)uILineInfo2.height;
				if (num > topY)
				{
					float num5 = topY - num2;
					if (num > topY - 0.5f * num5)
					{
						return i - 1;
					}
					return i;
				}
				if (num > num4)
				{
					return i;
				}
				num2 = num4;
			}
			return generator.lineCount;
		}

		protected int GetCharacterIndexFromPosition(Vector2 pos)
		{
			TextGenerator cachedTextGenerator = m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator.lineCount == 0)
			{
				return 0;
			}
			int unclampedCharacterLineFromPosition = GetUnclampedCharacterLineFromPosition(pos, cachedTextGenerator);
			if (unclampedCharacterLineFromPosition < 0)
			{
				return 0;
			}
			if (unclampedCharacterLineFromPosition >= cachedTextGenerator.lineCount)
			{
				return cachedTextGenerator.characterCountVisible;
			}
			UILineInfo uILineInfo = cachedTextGenerator.lines[unclampedCharacterLineFromPosition];
			int startCharIdx = uILineInfo.startCharIdx;
			int lineEndPosition = GetLineEndPosition(cachedTextGenerator, unclampedCharacterLineFromPosition);
			for (int i = startCharIdx; i < lineEndPosition && i < cachedTextGenerator.characterCountVisible; i++)
			{
				UICharInfo uICharInfo = cachedTextGenerator.characters[i];
				Vector2 vector = uICharInfo.cursorPos / m_TextComponent.pixelsPerUnit;
				float num = pos.x - vector.x;
				float num2 = vector.x + uICharInfo.charWidth / m_TextComponent.pixelsPerUnit - pos.x;
				if (num < num2)
				{
					return i;
				}
			}
			return lineEndPosition;
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left && m_TextComponent != null && m_Keyboard == null;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = true;
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
				caretSelectPositionInternal = GetCharacterIndexFromPosition(localPoint) + m_DrawStart;
				MarkGeometryAsDirty();
				m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textComponent.rectTransform, eventData.position, eventData.pressEventCamera);
				if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
				{
					m_DragCoroutine = StartCoroutine(MouseDragOutsideRect(eventData));
				}
				eventData.Use();
			}
		}

		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			while (m_UpdateDrag && m_DragPositionOutOfBounds)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos);
				Rect rect = textComponent.rectTransform.rect;
				if (multiLine)
				{
					if (localMousePos.y > rect.yMax)
					{
						MoveUp(shift: true, goToFirstChar: true);
					}
					else if (localMousePos.y < rect.yMin)
					{
						MoveDown(shift: true, goToLastChar: true);
					}
				}
				else if (localMousePos.x < rect.xMin)
				{
					MoveLeft(shift: true, ctrl: false);
				}
				else if (localMousePos.x > rect.xMax)
				{
					MoveRight(shift: true, ctrl: false);
				}
				UpdateLabel();
				float delay = (!multiLine) ? 0.05f : 0.1f;
				yield return new WaitForSecondsRealtime(delay);
			}
			m_DragCoroutine = null;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				m_UpdateDrag = false;
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			bool allowInput = m_AllowInput;
			base.OnPointerDown(eventData);
			if (!InPlaceEditing() && (m_Keyboard == null || !m_Keyboard.active))
			{
				OnSelect(eventData);
				return;
			}
			if (allowInput)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
				int num3 = caretSelectPositionInternal = (caretPositionInternal = GetCharacterIndexFromPosition(localPoint) + m_DrawStart);
			}
			UpdateLabel();
			eventData.Use();
		}

		protected EditState KeyPressed(Event evt)
		{
			EventModifiers modifiers = evt.modifiers;
			bool flag = (SystemInfo.operatingSystemFamily != OperatingSystemFamily.MacOSX) ? ((modifiers & EventModifiers.Control) != EventModifiers.None) : ((modifiers & EventModifiers.Command) != EventModifiers.None);
			bool flag2 = (modifiers & EventModifiers.Shift) != EventModifiers.None;
			bool flag3 = (modifiers & EventModifiers.Alt) != EventModifiers.None;
			bool flag4 = flag && !flag3 && !flag2;
			switch (evt.keyCode)
			{
			case KeyCode.Backspace:
				Backspace();
				return EditState.Continue;
			case KeyCode.Delete:
				ForwardSpace();
				return EditState.Continue;
			case KeyCode.Home:
				MoveTextStart(flag2);
				return EditState.Continue;
			case KeyCode.End:
				MoveTextEnd(flag2);
				return EditState.Continue;
			case KeyCode.A:
				if (flag4)
				{
					SelectAll();
					return EditState.Continue;
				}
				break;
			case KeyCode.C:
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = "";
					}
					return EditState.Continue;
				}
				break;
			case KeyCode.V:
				if (flag4)
				{
					Append(clipboard);
					return EditState.Continue;
				}
				break;
			case KeyCode.X:
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = "";
					}
					Delete();
					SendOnValueChangedAndUpdateLabel();
					return EditState.Continue;
				}
				break;
			case KeyCode.LeftArrow:
				MoveLeft(flag2, flag);
				return EditState.Continue;
			case KeyCode.RightArrow:
				MoveRight(flag2, flag);
				return EditState.Continue;
			case KeyCode.UpArrow:
				MoveUp(flag2);
				return EditState.Continue;
			case KeyCode.DownArrow:
				MoveDown(flag2);
				return EditState.Continue;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				if (lineType != LineType.MultiLineNewline)
				{
					return EditState.Finish;
				}
				break;
			case KeyCode.Escape:
				m_WasCanceled = true;
				return EditState.Finish;
			}
			char c = evt.character;
			if (!multiLine && (c == '\t' || c == '\r' || c == '\n'))
			{
				return EditState.Continue;
			}
			if (c == '\r' || c == '\u0003')
			{
				c = '\n';
			}
			if (IsValidChar(c))
			{
				Append(c);
			}
			if (c == '\0' && compositionString.Length > 0)
			{
				UpdateLabel();
			}
			return EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			switch (c)
			{
			case '\u007f':
				return false;
			case '\t':
			case '\n':
				return true;
			default:
				return m_TextComponent.font.HasCharacter(c);
			}
		}

		public void ProcessEvent(Event e)
		{
			KeyPressed(e);
		}

		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (!isFocused)
			{
				return;
			}
			bool flag = false;
			while (Event.PopEvent(m_ProcessingEvent))
			{
				if (m_ProcessingEvent.rawType == EventType.KeyDown)
				{
					flag = true;
					EditState editState = KeyPressed(m_ProcessingEvent);
					if (editState == EditState.Finish)
					{
						DeactivateInputField();
						break;
					}
				}
				EventType type = m_ProcessingEvent.type;
				if (type == EventType.ValidateCommand || type == EventType.ExecuteCommand)
				{
					string commandName = m_ProcessingEvent.commandName;
					if (commandName != null && commandName == "SelectAll")
					{
						SelectAll();
						flag = true;
					}
				}
			}
			if (flag)
			{
				UpdateLabel();
			}
			eventData.Use();
		}

		private string GetSelectedString()
		{
			if (!hasSelection)
			{
				return "";
			}
			int num = caretPositionInternal;
			int num2 = caretSelectPositionInternal;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (caretSelectPositionInternal + 1 >= text.Length)
			{
				return text.Length;
			}
			int num = text.IndexOfAny(kSeparators, caretSelectPositionInternal + 1);
			return (num != -1) ? (num + 1) : text.Length;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
				return;
			}
			int num4 = (!ctrl) ? (caretSelectPositionInternal + 1) : FindtNextWordBegin();
			if (shift)
			{
				caretSelectPositionInternal = num4;
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num4);
			}
		}

		private int FindtPrevWordBegin()
		{
			if (caretSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = text.LastIndexOfAny(kSeparators, caretSelectPositionInternal - 2);
			return (num != -1) ? (num + 1) : 0;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
				return;
			}
			int num4 = (!ctrl) ? (caretSelectPositionInternal - 1) : FindtPrevWordBegin();
			if (shift)
			{
				caretSelectPositionInternal = num4;
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num4);
			}
		}

		private int DetermineCharacterLine(int charPos, TextGenerator generator)
		{
			for (int i = 0; i < generator.lineCount - 1; i++)
			{
				UILineInfo uILineInfo = generator.lines[i + 1];
				if (uILineInfo.startCharIdx > charPos)
				{
					return i;
				}
			}
			return generator.lineCount - 1;
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= cachedInputTextGenerator.characters.Count)
			{
				return 0;
			}
			UICharInfo uICharInfo = cachedInputTextGenerator.characters[originalPos];
			int num = DetermineCharacterLine(originalPos, cachedInputTextGenerator);
			if (num <= 0)
			{
				return (!goToFirstChar) ? originalPos : 0;
			}
			UILineInfo uILineInfo = cachedInputTextGenerator.lines[num];
			int num2 = uILineInfo.startCharIdx - 1;
			UILineInfo uILineInfo2 = cachedInputTextGenerator.lines[num - 1];
			for (int i = uILineInfo2.startCharIdx; i < num2; i++)
			{
				UICharInfo uICharInfo2 = cachedInputTextGenerator.characters[i];
				if (uICharInfo2.cursorPos.x >= uICharInfo.cursorPos.x)
				{
					return i;
				}
			}
			return num2;
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= cachedInputTextGenerator.characterCountVisible)
			{
				return text.Length;
			}
			UICharInfo uICharInfo = cachedInputTextGenerator.characters[originalPos];
			int num = DetermineCharacterLine(originalPos, cachedInputTextGenerator);
			if (num + 1 >= cachedInputTextGenerator.lineCount)
			{
				return (!goToLastChar) ? originalPos : text.Length;
			}
			int lineEndPosition = GetLineEndPosition(cachedInputTextGenerator, num + 1);
			UILineInfo uILineInfo = cachedInputTextGenerator.lines[num + 1];
			for (int i = uILineInfo.startCharIdx; i < lineEndPosition; i++)
			{
				UICharInfo uICharInfo2 = cachedInputTextGenerator.characters[i];
				if (uICharInfo2.cursorPos.x >= uICharInfo.cursorPos.x)
				{
					return i;
				}
			}
			return lineEndPosition;
		}

		private void MoveDown(bool shift)
		{
			MoveDown(shift, goToLastChar: true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
			}
			int num4 = (!multiLine) ? text.Length : LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar);
			if (shift)
			{
				caretSelectPositionInternal = num4;
			}
			else
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = num4);
			}
		}

		private void MoveUp(bool shift)
		{
			MoveUp(shift, goToFirstChar: true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
			}
			int num4 = multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0;
			if (shift)
			{
				caretSelectPositionInternal = num4;
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num4);
			}
		}

		private void Delete()
		{
			if (!m_ReadOnly && caretPositionInternal != caretSelectPositionInternal)
			{
				if (caretPositionInternal < caretSelectPositionInternal)
				{
					m_Text = text.Substring(0, caretPositionInternal) + text.Substring(caretSelectPositionInternal, text.Length - caretSelectPositionInternal);
					caretSelectPositionInternal = caretPositionInternal;
				}
				else
				{
					m_Text = text.Substring(0, caretSelectPositionInternal) + text.Substring(caretPositionInternal, text.Length - caretPositionInternal);
					caretPositionInternal = caretSelectPositionInternal;
				}
			}
		}

		private void ForwardSpace()
		{
			if (!m_ReadOnly)
			{
				if (hasSelection)
				{
					Delete();
					SendOnValueChangedAndUpdateLabel();
				}
				else if (caretPositionInternal < text.Length)
				{
					m_Text = text.Remove(caretPositionInternal, 1);
					SendOnValueChangedAndUpdateLabel();
				}
			}
		}

		private void Backspace()
		{
			if (!m_ReadOnly)
			{
				if (hasSelection)
				{
					Delete();
					SendOnValueChangedAndUpdateLabel();
				}
				else if (caretPositionInternal > 0)
				{
					m_Text = text.Remove(caretPositionInternal - 1, 1);
					caretSelectPositionInternal = --caretPositionInternal;
					SendOnValueChangedAndUpdateLabel();
				}
			}
		}

		private void Insert(char c)
		{
			if (!m_ReadOnly)
			{
				string text = c.ToString();
				Delete();
				if (characterLimit <= 0 || this.text.Length < characterLimit)
				{
					m_Text = this.text.Insert(m_CaretPosition, text);
					caretSelectPositionInternal = (caretPositionInternal += text.Length);
					SendOnValueChanged();
				}
			}
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			SendOnValueChanged();
			UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (onValueChanged != null)
			{
				onValueChanged.Invoke(text);
			}
		}

		protected void SendOnSubmit()
		{
			if (onEndEdit != null)
			{
				onEndEdit.Invoke(m_Text);
			}
		}

		protected virtual void Append(string input)
		{
			if (m_ReadOnly || !InPlaceEditing())
			{
				return;
			}
			int i = 0;
			for (int length = input.Length; i < length; i++)
			{
				char c = input[i];
				if (c >= ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\n')
				{
					Append(c);
				}
			}
		}

		protected virtual void Append(char input)
		{
			if (!m_ReadOnly && InPlaceEditing())
			{
				int num = Math.Min(selectionFocusPosition, selectionAnchorPosition);
				if (onValidateInput != null)
				{
					input = onValidateInput(text, num, input);
				}
				else if (characterValidation != 0)
				{
					input = Validate(text, num, input);
				}
				if (input != 0)
				{
					Insert(input);
				}
			}
		}

		protected void UpdateLabel()
		{
			if (m_TextComponent != null && m_TextComponent.font != null && !m_PreventFontCallback)
			{
				m_PreventFontCallback = true;
				string text = (compositionString.Length <= 0) ? this.text : (this.text.Substring(0, m_CaretPosition) + compositionString + this.text.Substring(m_CaretPosition));
				string text2 = (inputType != InputType.Password) ? text : new string(asteriskChar, text.Length);
				bool flag = string.IsNullOrEmpty(text);
				if (m_Placeholder != null)
				{
					m_Placeholder.enabled = flag;
				}
				if (!m_AllowInput)
				{
					m_DrawStart = 0;
					m_DrawEnd = m_Text.Length;
				}
				if (!flag)
				{
					Vector2 size = m_TextComponent.rectTransform.rect.size;
					TextGenerationSettings generationSettings = m_TextComponent.GetGenerationSettings(size);
					generationSettings.generateOutOfBounds = true;
					cachedInputTextGenerator.PopulateWithErrors(text2, generationSettings, base.gameObject);
					SetDrawRangeToContainCaretPosition(caretSelectPositionInternal);
					text2 = text2.Substring(m_DrawStart, Mathf.Min(m_DrawEnd, text2.Length) - m_DrawStart);
					SetCaretVisible();
				}
				m_TextComponent.text = text2;
				MarkGeometryAsDirty();
				m_PreventFontCallback = false;
			}
		}

		private bool IsSelectionVisible()
		{
			if (m_DrawStart > caretPositionInternal || m_DrawStart > caretSelectPositionInternal)
			{
				return false;
			}
			if (m_DrawEnd < caretPositionInternal || m_DrawEnd < caretSelectPositionInternal)
			{
				return false;
			}
			return true;
		}

		private static int GetLineStartPosition(TextGenerator gen, int line)
		{
			line = Mathf.Clamp(line, 0, gen.lines.Count - 1);
			UILineInfo uILineInfo = gen.lines[line];
			return uILineInfo.startCharIdx;
		}

		private static int GetLineEndPosition(TextGenerator gen, int line)
		{
			line = Mathf.Max(line, 0);
			if (line + 1 < gen.lines.Count)
			{
				UILineInfo uILineInfo = gen.lines[line + 1];
				return uILineInfo.startCharIdx - 1;
			}
			return gen.characterCountVisible;
		}

		private void SetDrawRangeToContainCaretPosition(int caretPos)
		{
			if (cachedInputTextGenerator.lineCount <= 0)
			{
				return;
			}
			Vector2 size = cachedInputTextGenerator.rectExtents.size;
			if (multiLine)
			{
				IList<UILineInfo> lines = cachedInputTextGenerator.lines;
				int num = DetermineCharacterLine(caretPos, cachedInputTextGenerator);
				if (caretPos > m_DrawEnd)
				{
					m_DrawEnd = GetLineEndPosition(cachedInputTextGenerator, num);
					UILineInfo uILineInfo = lines[num];
					float topY = uILineInfo.topY;
					UILineInfo uILineInfo2 = lines[num];
					float num2 = topY - (float)uILineInfo2.height;
					if (num == lines.Count - 1)
					{
						float num3 = num2;
						UILineInfo uILineInfo3 = lines[num];
						num2 = num3 + uILineInfo3.leading;
					}
					int num4;
					for (num4 = num; num4 > 0; num4--)
					{
						UILineInfo uILineInfo4 = lines[num4 - 1];
						float topY2 = uILineInfo4.topY;
						if (topY2 - num2 > size.y)
						{
							break;
						}
					}
					m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, num4);
					return;
				}
				if (caretPos < m_DrawStart)
				{
					m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, num);
				}
				int num5 = DetermineCharacterLine(m_DrawStart, cachedInputTextGenerator);
				int i = num5;
				UILineInfo uILineInfo5 = lines[num5];
				float topY3 = uILineInfo5.topY;
				UILineInfo uILineInfo6 = lines[i];
				float topY4 = uILineInfo6.topY;
				UILineInfo uILineInfo7 = lines[i];
				float num6 = topY4 - (float)uILineInfo7.height;
				if (i == lines.Count - 1)
				{
					float num7 = num6;
					UILineInfo uILineInfo8 = lines[i];
					num6 = num7 + uILineInfo8.leading;
				}
				for (; i < lines.Count - 1; i++)
				{
					UILineInfo uILineInfo9 = lines[i + 1];
					float topY5 = uILineInfo9.topY;
					UILineInfo uILineInfo10 = lines[i + 1];
					num6 = topY5 - (float)uILineInfo10.height;
					if (i + 1 == lines.Count - 1)
					{
						float num8 = num6;
						UILineInfo uILineInfo11 = lines[i + 1];
						num6 = num8 + uILineInfo11.leading;
					}
					if (topY3 - num6 > size.y)
					{
						break;
					}
				}
				m_DrawEnd = GetLineEndPosition(cachedInputTextGenerator, i);
				while (num5 > 0)
				{
					UILineInfo uILineInfo12 = lines[num5 - 1];
					topY3 = uILineInfo12.topY;
					if (topY3 - num6 > size.y)
					{
						break;
					}
					num5--;
				}
				m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, num5);
				return;
			}
			IList<UICharInfo> characters = cachedInputTextGenerator.characters;
			if (m_DrawEnd > cachedInputTextGenerator.characterCountVisible)
			{
				m_DrawEnd = cachedInputTextGenerator.characterCountVisible;
			}
			float num9 = 0f;
			if (caretPos > m_DrawEnd || (caretPos == m_DrawEnd && m_DrawStart > 0))
			{
				m_DrawEnd = caretPos;
				for (m_DrawStart = m_DrawEnd - 1; m_DrawStart >= 0; m_DrawStart--)
				{
					float num10 = num9;
					UICharInfo uICharInfo = characters[m_DrawStart];
					if (num10 + uICharInfo.charWidth > size.x)
					{
						break;
					}
					float num11 = num9;
					UICharInfo uICharInfo2 = characters[m_DrawStart];
					num9 = num11 + uICharInfo2.charWidth;
				}
				m_DrawStart++;
			}
			else
			{
				if (caretPos < m_DrawStart)
				{
					m_DrawStart = caretPos;
				}
				m_DrawEnd = m_DrawStart;
			}
			while (m_DrawEnd < cachedInputTextGenerator.characterCountVisible)
			{
				float num12 = num9;
				UICharInfo uICharInfo3 = characters[m_DrawEnd];
				num9 = num12 + uICharInfo3.charWidth;
				if (num9 > size.x)
				{
					break;
				}
				m_DrawEnd++;
			}
		}

		public void ForceLabelUpdate()
		{
			UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.LatePreRender)
			{
				UpdateGeometry();
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (shouldHideMobileInput)
			{
				if (m_CachedInputRenderer == null && m_TextComponent != null)
				{
					GameObject gameObject = new GameObject(base.transform.name + " Input Caret", typeof(RectTransform), typeof(CanvasRenderer));
					gameObject.hideFlags = HideFlags.DontSave;
					gameObject.transform.SetParent(m_TextComponent.transform.parent);
					gameObject.transform.SetAsFirstSibling();
					gameObject.layer = base.gameObject.layer;
					caretRectTrans = gameObject.GetComponent<RectTransform>();
					m_CachedInputRenderer = gameObject.GetComponent<CanvasRenderer>();
					m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);
					gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
					AssignPositioningIfNeeded();
				}
				if (!(m_CachedInputRenderer == null))
				{
					OnFillVBO(mesh);
					m_CachedInputRenderer.SetMesh(mesh);
				}
			}
		}

		private void AssignPositioningIfNeeded()
		{
			if (m_TextComponent != null && caretRectTrans != null && (caretRectTrans.localPosition != m_TextComponent.rectTransform.localPosition || caretRectTrans.localRotation != m_TextComponent.rectTransform.localRotation || caretRectTrans.localScale != m_TextComponent.rectTransform.localScale || caretRectTrans.anchorMin != m_TextComponent.rectTransform.anchorMin || caretRectTrans.anchorMax != m_TextComponent.rectTransform.anchorMax || caretRectTrans.anchoredPosition != m_TextComponent.rectTransform.anchoredPosition || caretRectTrans.sizeDelta != m_TextComponent.rectTransform.sizeDelta || caretRectTrans.pivot != m_TextComponent.rectTransform.pivot))
			{
				caretRectTrans.localPosition = m_TextComponent.rectTransform.localPosition;
				caretRectTrans.localRotation = m_TextComponent.rectTransform.localRotation;
				caretRectTrans.localScale = m_TextComponent.rectTransform.localScale;
				caretRectTrans.anchorMin = m_TextComponent.rectTransform.anchorMin;
				caretRectTrans.anchorMax = m_TextComponent.rectTransform.anchorMax;
				caretRectTrans.anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
				caretRectTrans.sizeDelta = m_TextComponent.rectTransform.sizeDelta;
				caretRectTrans.pivot = m_TextComponent.rectTransform.pivot;
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			using (VertexHelper vertexHelper = new VertexHelper())
			{
				if (!isFocused)
				{
					vertexHelper.FillMesh(vbo);
				}
				else
				{
					Vector2 roundingOffset = m_TextComponent.PixelAdjustPoint(Vector2.zero);
					if (!hasSelection)
					{
						GenerateCaret(vertexHelper, roundingOffset);
					}
					else
					{
						GenerateHightlight(vertexHelper, roundingOffset);
					}
					vertexHelper.FillMesh(vbo);
				}
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			if (!m_CaretVisible)
			{
				return;
			}
			if (m_CursorVerts == null)
			{
				CreateCursorVerts();
			}
			float num = m_CaretWidth;
			int num2 = Mathf.Max(0, caretPositionInternal - m_DrawStart);
			TextGenerator cachedTextGenerator = m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator == null || cachedTextGenerator.lineCount == 0)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (num2 < cachedTextGenerator.characters.Count)
			{
				UICharInfo uICharInfo = cachedTextGenerator.characters[num2];
				zero.x = uICharInfo.cursorPos.x;
			}
			zero.x /= m_TextComponent.pixelsPerUnit;
			if (zero.x > m_TextComponent.rectTransform.rect.xMax)
			{
				zero.x = m_TextComponent.rectTransform.rect.xMax;
			}
			int index = DetermineCharacterLine(num2, cachedTextGenerator);
			UILineInfo uILineInfo = cachedTextGenerator.lines[index];
			zero.y = uILineInfo.topY / m_TextComponent.pixelsPerUnit;
			UILineInfo uILineInfo2 = cachedTextGenerator.lines[index];
			float num3 = (float)uILineInfo2.height / m_TextComponent.pixelsPerUnit;
			for (int i = 0; i < m_CursorVerts.Length; i++)
			{
				m_CursorVerts[i].color = caretColor;
			}
			m_CursorVerts[0].position = new Vector3(zero.x, zero.y - num3, 0f);
			m_CursorVerts[1].position = new Vector3(zero.x + num, zero.y - num3, 0f);
			m_CursorVerts[2].position = new Vector3(zero.x + num, zero.y, 0f);
			m_CursorVerts[3].position = new Vector3(zero.x, zero.y, 0f);
			if (roundingOffset != Vector2.zero)
			{
				for (int j = 0; j < m_CursorVerts.Length; j++)
				{
					UIVertex uIVertex = m_CursorVerts[j];
					uIVertex.position.x += roundingOffset.x;
					uIVertex.position.y += roundingOffset.y;
				}
			}
			vbo.AddUIVertexQuad(m_CursorVerts);
			int num4 = Screen.height;
			int targetDisplay = m_TextComponent.canvas.targetDisplay;
			if (targetDisplay > 0 && targetDisplay < Display.displays.Length)
			{
				num4 = Display.displays[targetDisplay].renderingHeight;
			}
			zero.y = (float)num4 - zero.y;
			input.compositionCursorPos = zero;
		}

		private void CreateCursorVerts()
		{
			m_CursorVerts = new UIVertex[4];
			for (int i = 0; i < m_CursorVerts.Length; i++)
			{
				m_CursorVerts[i] = UIVertex.simpleVert;
				m_CursorVerts[i].uv0 = Vector2.zero;
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			int num = Mathf.Max(0, caretPositionInternal - m_DrawStart);
			int num2 = Mathf.Max(0, caretSelectPositionInternal - m_DrawStart);
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			num2--;
			TextGenerator cachedTextGenerator = m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator.lineCount <= 0)
			{
				return;
			}
			int num4 = DetermineCharacterLine(num, cachedTextGenerator);
			int lineEndPosition = GetLineEndPosition(cachedTextGenerator, num4);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.zero;
			simpleVert.color = selectionColor;
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			for (int i = num; i <= num2 && i < cachedTextGenerator.characterCount; i++)
			{
				if (i == lineEndPosition || i == num2)
				{
					UICharInfo uICharInfo = cachedTextGenerator.characters[num];
					UICharInfo uICharInfo2 = cachedTextGenerator.characters[i];
					float x = uICharInfo.cursorPos.x / m_TextComponent.pixelsPerUnit;
					UILineInfo uILineInfo = cachedTextGenerator.lines[num4];
					vector = new Vector2(x, uILineInfo.topY / m_TextComponent.pixelsPerUnit);
					float x2 = (uICharInfo2.cursorPos.x + uICharInfo2.charWidth) / m_TextComponent.pixelsPerUnit;
					float y = vector.y;
					UILineInfo uILineInfo2 = cachedTextGenerator.lines[num4];
					vector2 = new Vector2(x2, y - (float)uILineInfo2.height / m_TextComponent.pixelsPerUnit);
					if (vector2.x > m_TextComponent.rectTransform.rect.xMax || vector2.x < m_TextComponent.rectTransform.rect.xMin)
					{
						vector2.x = m_TextComponent.rectTransform.rect.xMax;
					}
					int currentVertCount = vbo.currentVertCount;
					simpleVert.position = new Vector3(vector.x, vector2.y, 0f) + (Vector3)roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector2.x, vector2.y, 0f) + (Vector3)roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector2.x, vector.y, 0f) + (Vector3)roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector.x, vector.y, 0f) + (Vector3)roundingOffset;
					vbo.AddVert(simpleVert);
					vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
					vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
					num = i + 1;
					num4++;
					lineEndPosition = GetLineEndPosition(cachedTextGenerator, num4);
				}
			}
		}

		protected char Validate(string text, int pos, char ch)
		{
			if (characterValidation == CharacterValidation.None || !base.enabled)
			{
				return ch;
			}
			if (characterValidation == CharacterValidation.Integer || characterValidation == CharacterValidation.Decimal)
			{
				bool flag = pos == 0 && text.Length > 0 && text[0] == '-';
				bool flag2 = text.Length > 0 && text[0] == '-' && ((caretPositionInternal == 0 && caretSelectPositionInternal > 0) || (caretSelectPositionInternal == 0 && caretPositionInternal > 0));
				bool flag3 = caretPositionInternal == 0 || caretSelectPositionInternal == 0;
				if (!flag || flag2)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '-' && (pos == 0 || flag3))
					{
						return ch;
					}
					if (ch == '.' && characterValidation == CharacterValidation.Decimal && !text.Contains("."))
					{
						return ch;
					}
				}
			}
			else if (characterValidation == CharacterValidation.Alphanumeric)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.Name)
			{
				if (char.IsLetter(ch))
				{
					if (char.IsLower(ch) && (pos == 0 || text[pos - 1] == ' '))
					{
						return char.ToUpper(ch);
					}
					if (char.IsUpper(ch) && pos > 0 && text[pos - 1] != ' ' && text[pos - 1] != '\'')
					{
						return char.ToLower(ch);
					}
					return ch;
				}
				if (ch == '\'' && !text.Contains("'") && (pos <= 0 || (text[pos - 1] != ' ' && text[pos - 1] != '\'')) && (pos >= text.Length || (text[pos] != ' ' && text[pos] != '\'')))
				{
					return ch;
				}
				if (ch == ' ' && (pos <= 0 || (text[pos - 1] != ' ' && text[pos - 1] != '\'')) && (pos >= text.Length || (text[pos] != ' ' && text[pos] != '\'')))
				{
					return ch;
				}
			}
			else if (characterValidation == CharacterValidation.EmailAddress)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
				if (ch == '@' && text.IndexOf('@') == -1)
				{
					return ch;
				}
				if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
				{
					return ch;
				}
				if (ch == '.')
				{
					char c = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
					char c2 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
					if (c != '.' && c2 != '.')
					{
						return ch;
					}
				}
			}
			return '\0';
		}

		public void ActivateInputField()
		{
			if (!(m_TextComponent == null) && !(m_TextComponent.font == null) && IsActive() && IsInteractable())
			{
				if (isFocused && m_Keyboard != null && !m_Keyboard.active)
				{
					m_Keyboard.active = true;
					m_Keyboard.text = m_Text;
				}
				m_ShouldActivateNextUpdate = true;
			}
		}

		private void ActivateInputFieldInternal()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			if (TouchScreenKeyboard.isSupported)
			{
				if (input.touchSupported)
				{
					TouchScreenKeyboard.hideInput = shouldHideMobileInput;
				}
				m_Keyboard = ((inputType != InputType.Password) ? TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine) : TouchScreenKeyboard.Open(m_Text, keyboardType, autocorrection: false, multiLine, secure: true));
				MoveTextEnd(shift: false);
			}
			else
			{
				input.imeCompositionMode = IMECompositionMode.On;
				OnFocus();
			}
			m_AllowInput = true;
			m_OriginalText = text;
			m_WasCanceled = false;
			SetCaretVisible();
			UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (shouldActivateOnSelect)
			{
				ActivateInputField();
			}
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				ActivateInputField();
			}
		}

		public void DeactivateInputField()
		{
			if (!m_AllowInput)
			{
				return;
			}
			m_HasDoneFocusTransition = false;
			m_AllowInput = false;
			if (m_Placeholder != null)
			{
				m_Placeholder.enabled = string.IsNullOrEmpty(m_Text);
			}
			if (m_TextComponent != null && IsInteractable())
			{
				if (m_WasCanceled)
				{
					text = m_OriginalText;
				}
				if (m_Keyboard != null)
				{
					m_Keyboard.active = false;
					m_Keyboard = null;
				}
				m_CaretPosition = (m_CaretSelectPosition = 0);
				SendOnSubmit();
				input.imeCompositionMode = IMECompositionMode.Auto;
			}
			MarkGeometryAsDirty();
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			DeactivateInputField();
			base.OnDeselect(eventData);
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (IsActive() && IsInteractable() && !isFocused)
			{
				m_ShouldActivateNextUpdate = true;
			}
		}

		private void EnforceContentType()
		{
			switch (contentType)
			{
			case ContentType.Standard:
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Autocorrected:
				m_InputType = InputType.AutoCorrect;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.IntegerNumber:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			case ContentType.DecimalNumber:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
				m_CharacterValidation = CharacterValidation.Decimal;
				break;
			case ContentType.Alphanumeric:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
				m_CharacterValidation = CharacterValidation.Alphanumeric;
				break;
			case ContentType.Name:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NamePhonePad;
				m_CharacterValidation = CharacterValidation.Name;
				break;
			case ContentType.EmailAddress:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
				m_CharacterValidation = CharacterValidation.EmailAddress;
				break;
			case ContentType.Password:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Pin:
				m_LineType = LineType.SingleLine;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			}
			EnforceTextHOverflow();
		}

		private void EnforceTextHOverflow()
		{
			if (m_TextComponent != null)
			{
				if (multiLine)
				{
					m_TextComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
				}
				else
				{
					m_TextComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
				}
			}
		}

		private void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
		{
			if (contentType == ContentType.Custom)
			{
				return;
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (contentType == allowedContentTypes[i])
				{
					return;
				}
			}
			contentType = ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (contentType != ContentType.Custom)
			{
				contentType = ContentType.Custom;
			}
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			if (m_HasDoneFocusTransition)
			{
				state = SelectionState.Highlighted;
			}
			else if (state == SelectionState.Pressed)
			{
				m_HasDoneFocusTransition = true;
			}
			base.DoStateTransition(state, instant);
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}
	}
}
