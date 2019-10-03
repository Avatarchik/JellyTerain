using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	public sealed class GameObject : Object
	{
		public Transform transform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int layer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[Obsolete("GameObject.active is obsolete. Use GameObject.SetActive(), GameObject.activeSelf or GameObject.activeInHierarchy.")]
		public bool active
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool activeSelf
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool activeInHierarchy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isStatic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		internal bool isStaticBatchable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public string tag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Scene scene
		{
			get
			{
				INTERNAL_get_scene(out Scene value);
				return value;
			}
		}

		public GameObject gameObject => this;

		public GameObject(string name)
		{
			Internal_CreateGameObject(this, name);
		}

		public GameObject()
		{
			Internal_CreateGameObject(this, null);
		}

		public GameObject(string name, params Type[] components)
		{
			Internal_CreateGameObject(this, name);
			foreach (Type componentType in components)
			{
				AddComponent(componentType);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern GameObject CreatePrimitive(PrimitiveType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[GeneratedByOldBindingsGenerator]
		public extern Component GetComponent(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			GetComponentFastPath(typeof(T), new IntPtr(&castHelper.onePointerFurtherThanT));
			return castHelper.t;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern Component GetComponentByName(string type);

		public Component GetComponent(string type)
		{
			return GetComponentByName(type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[GeneratedByOldBindingsGenerator]
		public extern Component GetComponentInChildren(Type type, bool includeInactive);

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type type)
		{
			return GetComponentInChildren(type, includeInactive: false);
		}

		[ExcludeFromDocs]
		public T GetComponentInChildren<T>()
		{
			bool includeInactive = false;
			return GetComponentInChildren<T>(includeInactive);
		}

		public T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)(object)GetComponentInChildren(typeof(T), includeInactive);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[GeneratedByOldBindingsGenerator]
		public extern Component GetComponentInParent(Type type);

		public T GetComponentInParent<T>()
		{
			return (T)(object)GetComponentInParent(typeof(T));
		}

		public Component[] GetComponents(Type type)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, null);
		}

		public T[] GetComponents<T>()
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: false, includeInactive: true, reverse: false, null);
		}

		public void GetComponents(Type type, List<Component> results)
		{
			GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, results);
		}

		public void GetComponents<T>(List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: false, recursive: false, includeInactive: true, reverse: false, results);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type type)
		{
			bool includeInactive = false;
			return GetComponentsInChildren(type, includeInactive);
		}

		public Component[] GetComponentsInChildren(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: true, includeInactive, reverse: false, null);
		}

		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: false, null);
		}

		public void GetComponentsInChildren<T>(bool includeInactive, List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: false, results);
		}

		public T[] GetComponentsInChildren<T>()
		{
			return GetComponentsInChildren<T>(includeInactive: false);
		}

		public void GetComponentsInChildren<T>(List<T> results)
		{
			GetComponentsInChildren(includeInactive: false, results);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type type)
		{
			bool includeInactive = false;
			return GetComponentsInParent(type, includeInactive);
		}

		public Component[] GetComponentsInParent(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])GetComponentsInternal(type, useSearchTypeAsArrayReturnType: false, recursive: true, includeInactive, reverse: true, null);
		}

		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: true, results);
		}

		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return (T[])GetComponentsInternal(typeof(T), useSearchTypeAsArrayReturnType: true, recursive: true, includeInactive, reverse: true, null);
		}

		public T[] GetComponentsInParent<T>()
		{
			return GetComponentsInParent<T>(includeInactive: false);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Array GetComponentsInternal(Type type, bool useSearchTypeAsArrayReturnType, bool recursive, bool includeInactive, bool reverse, object resultList);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern Component AddComponentInternal(string className);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetActive(bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("gameObject.SetActiveRecursively() is obsolete. Use GameObject.SetActive(), which is now inherited by children.")]
		[GeneratedByOldBindingsGenerator]
		public extern void SetActiveRecursively(bool state);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool CompareTag(string tag);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern GameObject FindGameObjectWithTag(string tag);

		public static GameObject FindWithTag(string tag)
		{
			return FindGameObjectWithTag(tag);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern GameObject[] FindGameObjectsWithTag(string tag);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessageUpwards(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessageUpwards(methodName, value, options);
		}

		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			SendMessageUpwards(methodName, null, options);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessage(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessage(methodName, value, options);
		}

		public void SendMessage(string methodName, SendMessageOptions options)
		{
			SendMessage(methodName, null, options);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			BroadcastMessage(methodName, parameter, options);
		}

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object parameter = null;
			BroadcastMessage(methodName, parameter, options);
		}

		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			BroadcastMessage(methodName, null, options);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Component Internal_AddComponentWithType(Type componentType);

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component AddComponent(Type componentType)
		{
			return Internal_AddComponentWithType(componentType);
		}

		public T AddComponent<T>() where T : Component
		{
			return AddComponent(typeof(T)) as T;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_CreateGameObject([Writable] GameObject mono, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern GameObject Find(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_scene(out Scene value);
	}
}
