using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class GUIUtility
	{
		internal static int s_SkinMode;

		internal static int s_OriginalID;

		internal static Vector2 s_EditorScreenPointOffset = Vector2.zero;

		internal static float pixelsPerPoint => Internal_GetPixelsPerPoint();

		internal static bool guiIsExiting
		{
			get;
			set;
		}

		public static int hotControl
		{
			get
			{
				return Internal_GetHotControl();
			}
			set
			{
				Internal_SetHotControl(value);
			}
		}

		public static int keyboardControl
		{
			get
			{
				return Internal_GetKeyboardControl();
			}
			set
			{
				Internal_SetKeyboardControl(value);
			}
		}

		public static string systemCopyBuffer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		internal static bool mouseUsed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool hasModalWindow
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal static bool textFieldInput
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static int GetControlID(FocusType focus)
		{
			return GetControlID(0, focus);
		}

		public static int GetControlID(GUIContent contents, FocusType focus)
		{
			return GetControlID(contents.hash, focus);
		}

		public static int GetControlID(FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(0, focus, position);
		}

		public static int GetControlID(int hint, FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(hint, focus, position);
		}

		public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
		{
			return Internal_GetNextControlID2(contents.hash, focus, position);
		}

		public static object GetStateObject(Type t, int controlID)
		{
			return GUIStateObjects.GetStateObject(t, controlID);
		}

		public static object QueryStateObject(Type t, int controlID)
		{
			return GUIStateObjects.QueryStateObject(t, controlID);
		}

		public static void ExitGUI()
		{
			guiIsExiting = true;
			throw new ExitGUIException();
		}

		internal static GUISkin GetDefaultSkin()
		{
			return Internal_GetDefaultSkin(s_SkinMode);
		}

		internal static GUISkin GetBuiltinSkin(int skin)
		{
			return Internal_GetBuiltinSkin(skin) as GUISkin;
		}

		[RequiredByNativeCode]
		internal static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
		{
			return false;
		}

		internal static void CleanupRoots()
		{
		}

		[RequiredByNativeCode]
		internal static void BeginGUI(int skinMode, int instanceID, int useGUILayout)
		{
			s_SkinMode = skinMode;
			s_OriginalID = instanceID;
			GUI.skin = null;
			guiIsExiting = false;
			if (useGUILayout != 0)
			{
				GUILayoutUtility.Begin(instanceID);
			}
			GUI.changed = false;
		}

		[RequiredByNativeCode]
		internal static void EndGUI(int layoutType)
		{
			try
			{
				if (Event.current.type == EventType.Layout)
				{
					switch (layoutType)
					{
					case 1:
						GUILayoutUtility.Layout();
						break;
					case 2:
						GUILayoutUtility.LayoutFromEditorWindow();
						break;
					}
				}
				GUILayoutUtility.SelectIDList(s_OriginalID, isWindow: false);
				GUIContent.ClearStaticCache();
			}
			finally
			{
				Internal_ExitGUI();
			}
		}

		[RequiredByNativeCode]
		internal static bool EndGUIFromException(Exception exception)
		{
			Internal_ExitGUI();
			return ShouldRethrowException(exception);
		}

		[RequiredByNativeCode]
		internal static bool EndContainerGUIFromException(Exception exception)
		{
			return ShouldRethrowException(exception);
		}

		internal static bool ShouldRethrowException(Exception exception)
		{
			while (exception is TargetInvocationException && exception.InnerException != null)
			{
				exception = exception.InnerException;
			}
			return exception is ExitGUIException;
		}

		internal static void CheckOnGUI()
		{
			if (Internal_GetGUIDepth() <= 0)
			{
				throw new ArgumentException("You can only call GUI functions from inside OnGUI.");
			}
		}

		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIClip.Unclip(guiPoint) + s_EditorScreenPointOffset;
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector = GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector.x;
			guiRect.y = vector.y;
			return guiRect;
		}

		public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
		{
			return GUIClip.Clip(screenPoint) - s_EditorScreenPointOffset;
		}

		public static Rect ScreenToGUIRect(Rect screenRect)
		{
			Vector2 vector = ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
			screenRect.x = vector.x;
			screenRect.y = vector.y;
			return screenRect;
		}

		public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float Internal_GetPixelsPerPoint();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetControlID(int hint, FocusType focus);

		private static int Internal_GetNextControlID2(int hint, FocusType focusType, Rect rect)
		{
			return INTERNAL_CALL_Internal_GetNextControlID2(hint, focusType, ref rect);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int INTERNAL_CALL_Internal_GetNextControlID2(int hint, FocusType focusType, ref Rect rect);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern int GetPermanentControlID();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetHotControl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_SetHotControl(int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void UpdateUndoName();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool GetChanged();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void SetChanged(bool changed);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetKeyboardControl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_SetKeyboardControl(int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void SetDidGUIWindowsEatLastEvent(bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern GUISkin Internal_GetDefaultSkin(int skinMode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object Internal_GetBuiltinSkin(int skin);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_ExitGUI();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern int Internal_GetGUIDepth();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_BeginContainer(int instanceID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_EndContainer();
	}
}
