using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Application
	{
		public delegate void AdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled, string errorMsg);

		public delegate void LowMemoryCallback();

		public delegate void LogCallback(string condition, string stackTrace, LogType type);

		internal static AdvertisingIdentifierCallback OnAdvertisingIdentifierCallback;

		private static LogCallback s_LogCallbackHandler;

		private static LogCallback s_LogCallbackHandlerThreaded;

		private static volatile LogCallback s_RegisterLogCallbackDeprecated;

		[Obsolete("This property is deprecated, please use LoadLevelAsync to detect if a specific scene is currently loading.")]
		public static bool isLoadingLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static int streamedBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isPlaying
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isFocused
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isEditor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isWebPlayer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[ThreadAndSerializationSafe]
		public static RuntimePlatform platform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string buildGUID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool isMobilePlatform
		{
			get
			{
				switch (platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
				case RuntimePlatform.TizenPlayer:
					return true;
				default:
					return false;
				}
			}
		}

		public static bool isConsolePlatform
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return platform == RuntimePlatform.PS4 || platform == RuntimePlatform.XboxOne;
			}
		}

		public static bool runInBackground
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("use Application.isEditor instead")]
		public static bool isPlayer => !isEditor;

		internal static bool isBatchmode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal static bool isTestRun
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal static bool isHumanControllingUs
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string dataPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string streamingAssetsPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[SecurityCritical]
		public static string persistentDataPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string temporaryCachePath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string srcValue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string absoluteURL
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string unityVersion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string version
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string installerName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string identifier
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static ApplicationInstallMode installMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static ApplicationSandboxType sandboxType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string productName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string companyName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static string cloudProjectId
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("Application.webSecurityEnabled is no longer supported, since the Unity Web Player is no longer supported by Unity.")]
		[ThreadAndSerializationSafe]
		public static bool webSecurityEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("Application.webSecurityHostUrl is no longer supported, since the Unity Web Player is no longer supported by Unity.")]
		[ThreadAndSerializationSafe]
		public static string webSecurityHostUrl
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static int targetFrameRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static SystemLanguage systemLanguage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("Use SetStackTraceLogType/GetStackTraceLogType instead")]
		public static StackTraceLogType stackTraceLogType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static ThreadPriority backgroundLoadingPriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static NetworkReachability internetReachability
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool genuine
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool genuineCheckAvailable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal static bool submitAnalytics
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("This property is deprecated, please use SplashScreen.isFinished instead")]
		public static bool isShowingSplashScreen => !SplashScreen.isFinished;

		[Obsolete("Use SceneManager.sceneCountInBuildSettings")]
		public static int levelCount => SceneManager.sceneCountInBuildSettings;

		[Obsolete("Use SceneManager to determine what scenes have been loaded")]
		public static int loadedLevel => SceneManager.GetActiveScene().buildIndex;

		[Obsolete("Use SceneManager to determine what scenes have been loaded")]
		public static string loadedLevelName => SceneManager.GetActiveScene().name;

		public static event LowMemoryCallback lowMemory;

		public static event LogCallback logMessageReceived
		{
			add
			{
				s_LogCallbackHandler = (LogCallback)Delegate.Combine(s_LogCallbackHandler, value);
				SetLogCallbackDefined(defined: true);
			}
			remove
			{
				s_LogCallbackHandler = (LogCallback)Delegate.Remove(s_LogCallbackHandler, value);
			}
		}

		public static event LogCallback logMessageReceivedThreaded
		{
			add
			{
				s_LogCallbackHandlerThreaded = (LogCallback)Delegate.Combine(s_LogCallbackHandlerThreaded, value);
				SetLogCallbackDefined(defined: true);
			}
			remove
			{
				s_LogCallbackHandlerThreaded = (LogCallback)Delegate.Remove(s_LogCallbackHandlerThreaded, value);
			}
		}

		[RequiredByNativeCode]
		private static void CallLowMemory()
		{
			Application.lowMemory?.Invoke();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Quit();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void CancelQuit();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Unload();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern float GetStreamProgressForLevelByName(string levelName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetStreamProgressForLevel(int levelIndex);

		public static float GetStreamProgressForLevel(string levelName)
		{
			return GetStreamProgressForLevelByName(levelName);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool CanStreamedLevelBeLoadedByName(string levelName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CanStreamedLevelBeLoaded(int levelIndex);

		public static bool CanStreamedLevelBeLoaded(string levelName)
		{
			return CanStreamedLevelBeLoadedByName(levelName);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern string[] GetBuildTags();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void CaptureScreenshot(string filename, [DefaultValue("0")] int superSize);

		[ExcludeFromDocs]
		public static void CaptureScreenshot(string filename)
		{
			int superSize = 0;
			CaptureScreenshot(filename, superSize);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool HasProLicense();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool HasAdvancedLicense();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool HasARGV(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern string GetValueForARGV(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use Object.DontDestroyOnLoad instead")]
		[GeneratedByOldBindingsGenerator]
		public static extern void DontDestroyOnLoad(Object mono);

		private static string ObjectToJSString(object o)
		{
			if (o == null)
			{
				return "null";
			}
			if (o is string)
			{
				string text = o.ToString().Replace("\\", "\\\\");
				text = text.Replace("\"", "\\\"");
				text = text.Replace("\n", "\\n");
				text = text.Replace("\r", "\\r");
				text = text.Replace("\0", "");
				text = text.Replace("\u2028", "");
				text = text.Replace("\u2029", "");
				return '"' + text + '"';
			}
			if (o is int || o is short || o is uint || o is ushort || o is byte)
			{
				return o.ToString();
			}
			if (o is float)
			{
				NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
				return ((float)o).ToString(numberFormat);
			}
			if (o is double)
			{
				NumberFormatInfo numberFormat2 = CultureInfo.InvariantCulture.NumberFormat;
				return ((double)o).ToString(numberFormat2);
			}
			if (o is char)
			{
				if ((char)o == '"')
				{
					return "\"\\\"\"";
				}
				return '"' + o.ToString() + '"';
			}
			if (o is IList)
			{
				IList list = (IList)o;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("new Array(");
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(ObjectToJSString(list[i]));
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}
			return ObjectToJSString(o.ToString());
		}

		public static void ExternalCall(string functionName, params object[] args)
		{
			Internal_ExternalCall(BuildInvocationForArguments(functionName, args));
		}

		private static string BuildInvocationForArguments(string functionName, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(ObjectToJSString(args[i]));
			}
			stringBuilder.Append(')');
			stringBuilder.Append(';');
			return stringBuilder.ToString();
		}

		public static void ExternalEval(string script)
		{
			if (script.Length > 0 && script[script.Length - 1] != ';')
			{
				script += ';';
			}
			Internal_ExternalCall(script);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_ExternalCall(string script);

		internal static void InvokeOnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
		{
			if (OnAdvertisingIdentifierCallback != null)
			{
				OnAdvertisingIdentifierCallback(advertisingId, trackingEnabled, string.Empty);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool RequestAdvertisingIdentifierAsync(AdvertisingIdentifierCallback delegateMethod);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void OpenURL(string url);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("For internal use only")]
		[GeneratedByOldBindingsGenerator]
		public static extern void ForceCrash(int mode);

		[RequiredByNativeCode]
		private static void CallLogCallback(string logString, string stackTrace, LogType type, bool invokedOnMainThread)
		{
			if (invokedOnMainThread)
			{
				s_LogCallbackHandler?.Invoke(logString, stackTrace, type);
			}
			s_LogCallbackHandlerThreaded?.Invoke(logString, stackTrace, type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void SetLogCallbackDefined(bool defined);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern StackTraceLogType GetStackTraceLogType(LogType logType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern AsyncOperation RequestUserAuthorization(UserAuthorization mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool HasUserAuthorization(UserAuthorization mode);

		[Obsolete("Application.RegisterLogCallback is deprecated. Use Application.logMessageReceived instead.")]
		public static void RegisterLogCallback(LogCallback handler)
		{
			RegisterLogCallback(handler, threaded: false);
		}

		[Obsolete("Application.RegisterLogCallbackThreaded is deprecated. Use Application.logMessageReceivedThreaded instead.")]
		public static void RegisterLogCallbackThreaded(LogCallback handler)
		{
			RegisterLogCallback(handler, threaded: true);
		}

		private static void RegisterLogCallback(LogCallback handler, bool threaded)
		{
			if (s_RegisterLogCallbackDeprecated != null)
			{
				logMessageReceived -= s_RegisterLogCallbackDeprecated;
				logMessageReceivedThreaded -= s_RegisterLogCallbackDeprecated;
			}
			s_RegisterLogCallbackDeprecated = handler;
			if (handler != null)
			{
				if (threaded)
				{
					logMessageReceivedThreaded += handler;
				}
				else
				{
					logMessageReceived += handler;
				}
			}
		}

		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevel(int index)
		{
			SceneManager.LoadScene(index, LoadSceneMode.Single);
		}

		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevel(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Single);
		}

		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevelAdditive(int index)
		{
			SceneManager.LoadScene(index, LoadSceneMode.Additive);
		}

		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevelAdditive(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Additive);
		}

		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAsync(int index)
		{
			return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
		}

		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAsync(string levelName)
		{
			return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
		}

		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAdditiveAsync(int index)
		{
			return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		}

		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAdditiveAsync(string levelName)
		{
			return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		}

		[Obsolete("Use SceneManager.UnloadScene")]
		public static bool UnloadLevel(int index)
		{
			return SceneManager.UnloadScene(index);
		}

		[Obsolete("Use SceneManager.UnloadScene")]
		public static bool UnloadLevel(string scenePath)
		{
			return SceneManager.UnloadScene(scenePath);
		}
	}
}
