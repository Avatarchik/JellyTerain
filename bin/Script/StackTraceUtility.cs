using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class StackTraceUtility
	{
		private static string projectFolder = "";

		[RequiredByNativeCode]
		internal static void SetProjectFolder(string folder)
		{
			projectFolder = folder.Replace("\\", "/");
		}

		[SecuritySafeCritical]
		[RequiredByNativeCode]
		public static string ExtractStackTrace()
		{
			StackTrace stackTrace = new StackTrace(1, fNeedFileInfo: true);
			return ExtractFormattedStackTrace(stackTrace).ToString();
		}

		private static bool IsSystemStacktraceType(object name)
		{
			string text = (string)name;
			return text.StartsWith("UnityEditor.") || text.StartsWith("UnityEngine.") || text.StartsWith("System.") || text.StartsWith("UnityScript.Lang.") || text.StartsWith("Boo.Lang.") || text.StartsWith("UnityEngine.SetupCoroutine");
		}

		public static string ExtractStringFromException(object exception)
		{
			string message = "";
			string stackTrace = "";
			ExtractStringFromExceptionInternal(exception, out message, out stackTrace);
			return message + "\n" + stackTrace;
		}

		[RequiredByNativeCode]
		[SecuritySafeCritical]
		internal static void ExtractStringFromExceptionInternal(object exceptiono, out string message, out string stackTrace)
		{
			if (exceptiono == null)
			{
				throw new ArgumentException("ExtractStringFromExceptionInternal called with null exception");
			}
			Exception ex = exceptiono as Exception;
			if (ex == null)
			{
				throw new ArgumentException("ExtractStringFromExceptionInternal called with an exceptoin that was not of type System.Exception");
			}
			StringBuilder stringBuilder = new StringBuilder((ex.StackTrace != null) ? (ex.StackTrace.Length * 2) : 512);
			message = "";
			string text = "";
			while (ex != null)
			{
				text = ((text.Length != 0) ? (ex.StackTrace + "\n" + text) : ex.StackTrace);
				string text2 = ex.GetType().Name;
				string text3 = "";
				if (ex.Message != null)
				{
					text3 = ex.Message;
				}
				if (text3.Trim().Length != 0)
				{
					text2 += ": ";
					text2 += text3;
				}
				message = text2;
				if (ex.InnerException != null)
				{
					text = "Rethrow as " + text2 + "\n" + text;
				}
				ex = ex.InnerException;
			}
			stringBuilder.Append(text + "\n");
			StackTrace stackTrace2 = new StackTrace(1, fNeedFileInfo: true);
			stringBuilder.Append(ExtractFormattedStackTrace(stackTrace2));
			stackTrace = stringBuilder.ToString();
		}

		[RequiredByNativeCode]
		internal static string PostprocessStacktrace(string oldString, bool stripEngineInternalInformation)
		{
			if (oldString == null)
			{
				return string.Empty;
			}
			string[] array = oldString.Split('\n');
			StringBuilder stringBuilder = new StringBuilder(oldString.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
			}
			for (int j = 0; j < array.Length; j++)
			{
				string text = array[j];
				if (text.Length == 0 || text[0] == '\n' || text.StartsWith("in (unmanaged)"))
				{
					continue;
				}
				if (stripEngineInternalInformation && text.StartsWith("UnityEditor.EditorGUIUtility:RenderGameViewCameras"))
				{
					break;
				}
				if (stripEngineInternalInformation && j < array.Length - 1 && IsSystemStacktraceType(text))
				{
					if (IsSystemStacktraceType(array[j + 1]))
					{
						continue;
					}
					int num = text.IndexOf(" (at");
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
				}
				if (text.IndexOf("(wrapper managed-to-native)") == -1 && text.IndexOf("(wrapper delegate-invoke)") == -1 && text.IndexOf("at <0x00000> <unknown method>") == -1 && (!stripEngineInternalInformation || !text.StartsWith("[") || !text.EndsWith("]")))
				{
					if (text.StartsWith("at "))
					{
						text = text.Remove(0, 3);
					}
					int num2 = text.IndexOf("[0x");
					int num3 = -1;
					if (num2 != -1)
					{
						num3 = text.IndexOf("]", num2);
					}
					if (num2 != -1 && num3 > num2)
					{
						text = text.Remove(num2, num3 - num2 + 1);
					}
					text = text.Replace("  in <filename unknown>:0", "");
					text = text.Replace("\\", "/");
					text = text.Replace(projectFolder, "");
					text = text.Replace('\\', '/');
					int num4 = text.LastIndexOf("  in ");
					if (num4 != -1)
					{
						text = text.Remove(num4, 5);
						text = text.Insert(num4, " (at ");
						text = text.Insert(text.Length, ")");
					}
					stringBuilder.Append(text + "\n");
				}
			}
			return stringBuilder.ToString();
		}

		[SecuritySafeCritical]
		internal static string ExtractFormattedStackTrace(StackTrace stackTrace)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				MethodBase method = frame.GetMethod();
				if (method == null)
				{
					continue;
				}
				Type declaringType = method.DeclaringType;
				if (declaringType == null)
				{
					continue;
				}
				string @namespace = declaringType.Namespace;
				if (@namespace != null && @namespace.Length != 0)
				{
					stringBuilder.Append(@namespace);
					stringBuilder.Append(".");
				}
				stringBuilder.Append(declaringType.Name);
				stringBuilder.Append(":");
				stringBuilder.Append(method.Name);
				stringBuilder.Append("(");
				int j = 0;
				ParameterInfo[] parameters = method.GetParameters();
				bool flag = true;
				for (; j < parameters.Length; j++)
				{
					if (!flag)
					{
						stringBuilder.Append(", ");
					}
					else
					{
						flag = false;
					}
					stringBuilder.Append(parameters[j].ParameterType.Name);
				}
				stringBuilder.Append(")");
				string text = frame.GetFileName();
				if (text != null && (!(declaringType.Name == "Debug") || !(declaringType.Namespace == "UnityEngine")) && (!(declaringType.Name == "Logger") || !(declaringType.Namespace == "UnityEngine")) && (!(declaringType.Name == "DebugLogHandler") || !(declaringType.Namespace == "UnityEngine")) && (!(declaringType.Name == "Assert") || !(declaringType.Namespace == "UnityEngine.Assertions")) && (!(method.Name == "print") || !(declaringType.Name == "MonoBehaviour") || !(declaringType.Namespace == "UnityEngine")))
				{
					stringBuilder.Append(" (at ");
					if (text.Replace("\\", "/").StartsWith(projectFolder))
					{
						text = text.Substring(projectFolder.Length, text.Length - projectFolder.Length);
					}
					stringBuilder.Append(text);
					stringBuilder.Append(":");
					stringBuilder.Append(frame.GetFileLineNumber().ToString());
					stringBuilder.Append(")");
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}
	}
}
