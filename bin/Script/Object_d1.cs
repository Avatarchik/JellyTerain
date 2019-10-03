using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	[RequiredByNativeCode]
	public class Object
	{
		private IntPtr m_CachedPtr;

		internal static int OffsetOfInstanceIDInCPlusPlusObject = -1;

		public string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public HideFlags hideFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object Internal_CloneSingle(Object data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object Internal_CloneSingleWithParent(Object data, Transform parent, bool worldPositionStays);

		private static Object Internal_InstantiateSingle(Object data, Vector3 pos, Quaternion rot)
		{
			return INTERNAL_CALL_Internal_InstantiateSingle(data, ref pos, ref rot);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object INTERNAL_CALL_Internal_InstantiateSingle(Object data, ref Vector3 pos, ref Quaternion rot);

		private static Object Internal_InstantiateSingleWithParent(Object data, Transform parent, Vector3 pos, Quaternion rot)
		{
			return INTERNAL_CALL_Internal_InstantiateSingleWithParent(data, parent, ref pos, ref rot);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Object INTERNAL_CALL_Internal_InstantiateSingleWithParent(Object data, Transform parent, ref Vector3 pos, ref Quaternion rot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private static extern int GetOffsetOfInstanceIDInCPlusPlusObject();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void EnsureRunningOnMainThread();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void Destroy(Object obj, [DefaultValue("0.0F")] float t);

		[ExcludeFromDocs]
		public static void Destroy(Object obj)
		{
			float t = 0f;
			Destroy(obj, t);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void DestroyImmediate(Object obj, [DefaultValue("false")] bool allowDestroyingAssets);

		[ExcludeFromDocs]
		public static void DestroyImmediate(Object obj)
		{
			bool allowDestroyingAssets = false;
			DestroyImmediate(obj, allowDestroyingAssets);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[GeneratedByOldBindingsGenerator]
		public static extern Object[] FindObjectsOfType(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void DontDestroyOnLoad(Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void DestroyObject(Object obj, [DefaultValue("0.0F")] float t);

		[ExcludeFromDocs]
		public static void DestroyObject(Object obj)
		{
			float t = 0f;
			DestroyObject(obj, t);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("use Object.FindObjectsOfType instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern Object[] FindSceneObjectsOfType(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("use Resources.FindObjectsOfTypeAll instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern Object[] FindObjectsOfTypeIncludingAssets(Type type);

		[Obsolete("Please use Resources.FindObjectsOfTypeAll instead")]
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return Resources.FindObjectsOfTypeAll(type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public override extern string ToString();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool DoesObjectWithInstanceIDExist(int instanceID);

		[SecuritySafeCritical]
		public unsafe int GetInstanceID()
		{
			if (m_CachedPtr == IntPtr.Zero)
			{
				return 0;
			}
			if (OffsetOfInstanceIDInCPlusPlusObject == -1)
			{
				OffsetOfInstanceIDInCPlusPlusObject = GetOffsetOfInstanceIDInCPlusPlusObject();
			}
			return *(int*)(void*)new IntPtr(m_CachedPtr.ToInt64() + OffsetOfInstanceIDInCPlusPlusObject);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object other)
		{
			Object @object = other as Object;
			if (@object == null && other != null && !(other is Object))
			{
				return false;
			}
			return CompareBaseObjects(this, @object);
		}

		public static implicit operator bool(Object exists)
		{
			return !CompareBaseObjects(exists, null);
		}

		private static bool CompareBaseObjects(Object lhs, Object rhs)
		{
			bool flag = (object)lhs == null;
			bool flag2 = (object)rhs == null;
			if (flag2 && flag)
			{
				return true;
			}
			if (flag2)
			{
				return !IsNativeObjectAlive(lhs);
			}
			if (flag)
			{
				return !IsNativeObjectAlive(rhs);
			}
			return object.ReferenceEquals(lhs, rhs);
		}

		private static bool IsNativeObjectAlive(Object o)
		{
			return o.GetCachedPtr() != IntPtr.Zero;
		}

		private IntPtr GetCachedPtr()
		{
			return m_CachedPtr;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			CheckNullArgument(original, "The Object you want to instantiate is null.");
			if (original is ScriptableObject)
			{
				throw new ArgumentException("Cannot instantiate a ScriptableObject with a position and rotation");
			}
			return Internal_InstantiateSingle(original, position, rotation);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
		{
			if (parent == null)
			{
				return Internal_InstantiateSingle(original, position, rotation);
			}
			CheckNullArgument(original, "The Object you want to instantiate is null.");
			return Internal_InstantiateSingleWithParent(original, parent, position, rotation);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original)
		{
			CheckNullArgument(original, "The Object you want to instantiate is null.");
			return Internal_CloneSingle(original);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent)
		{
			return Instantiate(original, parent, instantiateInWorldSpace: false);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
		{
			if (parent == null)
			{
				return Internal_CloneSingle(original);
			}
			CheckNullArgument(original, "The Object you want to instantiate is null.");
			return Internal_CloneSingleWithParent(original, parent, instantiateInWorldSpace);
		}

		public static T Instantiate<T>(T original) where T : Object
		{
			CheckNullArgument(original, "The Object you want to instantiate is null.");
			return (T)Internal_CloneSingle(original);
		}

		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
		{
			return (T)Instantiate((Object)original, position, rotation);
		}

		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
		{
			return (T)Instantiate((Object)original, position, rotation, parent);
		}

		public static T Instantiate<T>(T original, Transform parent) where T : Object
		{
			return Instantiate(original, parent, worldPositionStays: false);
		}

		public static T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
		{
			return (T)Instantiate((Object)original, parent, worldPositionStays);
		}

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(FindObjectsOfType(typeof(T)));
		}

		public static T FindObjectOfType<T>() where T : Object
		{
			return (T)FindObjectOfType(typeof(T));
		}

		private static void CheckNullArgument(object arg, string message)
		{
			if (arg == null)
			{
				throw new ArgumentException(message);
			}
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type)
		{
			Object[] array = FindObjectsOfType(type);
			if (array.Length > 0)
			{
				return array[0];
			}
			return null;
		}

		public static bool operator ==(Object x, Object y)
		{
			return CompareBaseObjects(x, y);
		}

		public static bool operator !=(Object x, Object y)
		{
			return !CompareBaseObjects(x, y);
		}
	}
}
