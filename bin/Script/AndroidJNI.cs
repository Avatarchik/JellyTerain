using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class AndroidJNI
	{
		private AndroidJNI()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int AttachCurrentThread();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int DetachCurrentThread();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetVersion();

		[ThreadAndSerializationSafe]
		public static IntPtr FindClass(string name)
		{
			INTERNAL_CALL_FindClass(name, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_FindClass(string name, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr FromReflectedMethod(IntPtr refMethod)
		{
			INTERNAL_CALL_FromReflectedMethod(refMethod, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_FromReflectedMethod(IntPtr refMethod, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr FromReflectedField(IntPtr refField)
		{
			INTERNAL_CALL_FromReflectedField(refField, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_FromReflectedField(IntPtr refField, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToReflectedMethod(IntPtr clazz, IntPtr methodID, bool isStatic)
		{
			INTERNAL_CALL_ToReflectedMethod(clazz, methodID, isStatic, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToReflectedMethod(IntPtr clazz, IntPtr methodID, bool isStatic, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToReflectedField(IntPtr clazz, IntPtr fieldID, bool isStatic)
		{
			INTERNAL_CALL_ToReflectedField(clazz, fieldID, isStatic, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToReflectedField(IntPtr clazz, IntPtr fieldID, bool isStatic, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetSuperclass(IntPtr clazz)
		{
			INTERNAL_CALL_GetSuperclass(clazz, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetSuperclass(IntPtr clazz, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsAssignableFrom(IntPtr clazz1, IntPtr clazz2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int Throw(IntPtr obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int ThrowNew(IntPtr clazz, string message);

		[ThreadAndSerializationSafe]
		public static IntPtr ExceptionOccurred()
		{
			INTERNAL_CALL_ExceptionOccurred(out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ExceptionOccurred(out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void ExceptionDescribe();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void ExceptionClear();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void FatalError(string message);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int PushLocalFrame(int capacity);

		[ThreadAndSerializationSafe]
		public static IntPtr PopLocalFrame(IntPtr ptr)
		{
			INTERNAL_CALL_PopLocalFrame(ptr, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_PopLocalFrame(IntPtr ptr, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewGlobalRef(IntPtr obj)
		{
			INTERNAL_CALL_NewGlobalRef(obj, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewGlobalRef(IntPtr obj, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void DeleteGlobalRef(IntPtr obj);

		[ThreadAndSerializationSafe]
		public static IntPtr NewLocalRef(IntPtr obj)
		{
			INTERNAL_CALL_NewLocalRef(obj, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewLocalRef(IntPtr obj, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void DeleteLocalRef(IntPtr obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsSameObject(IntPtr obj1, IntPtr obj2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int EnsureLocalCapacity(int capacity);

		[ThreadAndSerializationSafe]
		public static IntPtr AllocObject(IntPtr clazz)
		{
			INTERNAL_CALL_AllocObject(clazz, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_AllocObject(IntPtr clazz, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewObject(IntPtr clazz, IntPtr methodID, jvalue[] args)
		{
			INTERNAL_CALL_NewObject(clazz, methodID, args, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewObject(IntPtr clazz, IntPtr methodID, jvalue[] args, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectClass(IntPtr obj)
		{
			INTERNAL_CALL_GetObjectClass(obj, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetObjectClass(IntPtr obj, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool IsInstanceOf(IntPtr obj, IntPtr clazz);

		[ThreadAndSerializationSafe]
		public static IntPtr GetMethodID(IntPtr clazz, string name, string sig)
		{
			INTERNAL_CALL_GetMethodID(clazz, name, sig, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetMethodID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetFieldID(IntPtr clazz, string name, string sig)
		{
			INTERNAL_CALL_GetFieldID(clazz, name, sig, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetFieldID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticMethodID(IntPtr clazz, string name, string sig)
		{
			INTERNAL_CALL_GetStaticMethodID(clazz, name, sig, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetStaticMethodID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticFieldID(IntPtr clazz, string name, string sig)
		{
			INTERNAL_CALL_GetStaticFieldID(clazz, name, sig, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetStaticFieldID(IntPtr clazz, string name, string sig, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewStringUTF(string bytes)
		{
			INTERNAL_CALL_NewStringUTF(bytes, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewStringUTF(string bytes, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetStringUTFLength(IntPtr str);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetStringUTFChars(IntPtr str);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern string CallStringMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		public static IntPtr CallObjectMethod(IntPtr obj, IntPtr methodID, jvalue[] args)
		{
			INTERNAL_CALL_CallObjectMethod(obj, methodID, args, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CallObjectMethod(IntPtr obj, IntPtr methodID, jvalue[] args, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int CallIntMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CallBooleanMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short CallShortMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte CallByteMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char CallCharMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float CallFloatMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double CallDoubleMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long CallLongMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void CallVoidMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetStringField(IntPtr obj, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectField(IntPtr obj, IntPtr fieldID)
		{
			INTERNAL_CALL_GetObjectField(obj, fieldID, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetObjectField(IntPtr obj, IntPtr fieldID, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetBooleanField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte GetByteField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char GetCharField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short GetShortField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetIntField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long GetLongField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetFloatField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double GetDoubleField(IntPtr obj, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStringField(IntPtr obj, IntPtr fieldID, string val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetObjectField(IntPtr obj, IntPtr fieldID, IntPtr val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetBooleanField(IntPtr obj, IntPtr fieldID, bool val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetByteField(IntPtr obj, IntPtr fieldID, byte val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetCharField(IntPtr obj, IntPtr fieldID, char val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetShortField(IntPtr obj, IntPtr fieldID, short val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetIntField(IntPtr obj, IntPtr fieldID, int val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetLongField(IntPtr obj, IntPtr fieldID, long val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetFloatField(IntPtr obj, IntPtr fieldID, float val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetDoubleField(IntPtr obj, IntPtr fieldID, double val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern string CallStaticStringMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[ThreadAndSerializationSafe]
		public static IntPtr CallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args)
		{
			INTERNAL_CALL_CallStaticObjectMethod(clazz, methodID, args, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_CallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int CallStaticIntMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CallStaticBooleanMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short CallStaticShortMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte CallStaticByteMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char CallStaticCharMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float CallStaticFloatMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double CallStaticDoubleMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long CallStaticLongMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void CallStaticVoidMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern string GetStaticStringField(IntPtr clazz, IntPtr fieldID);

		[ThreadAndSerializationSafe]
		public static IntPtr GetStaticObjectField(IntPtr clazz, IntPtr fieldID)
		{
			INTERNAL_CALL_GetStaticObjectField(clazz, fieldID, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetStaticObjectField(IntPtr clazz, IntPtr fieldID, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetStaticBooleanField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte GetStaticByteField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char GetStaticCharField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short GetStaticShortField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetStaticIntField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long GetStaticLongField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetStaticFloatField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double GetStaticDoubleField(IntPtr clazz, IntPtr fieldID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticStringField(IntPtr clazz, IntPtr fieldID, string val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticObjectField(IntPtr clazz, IntPtr fieldID, IntPtr val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticBooleanField(IntPtr clazz, IntPtr fieldID, bool val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticByteField(IntPtr clazz, IntPtr fieldID, byte val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticCharField(IntPtr clazz, IntPtr fieldID, char val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticShortField(IntPtr clazz, IntPtr fieldID, short val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticIntField(IntPtr clazz, IntPtr fieldID, int val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticLongField(IntPtr clazz, IntPtr fieldID, long val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticFloatField(IntPtr clazz, IntPtr fieldID, float val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetStaticDoubleField(IntPtr clazz, IntPtr fieldID, double val);

		[ThreadAndSerializationSafe]
		public static IntPtr ToBooleanArray(bool[] array)
		{
			INTERNAL_CALL_ToBooleanArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToBooleanArray(bool[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToByteArray(byte[] array)
		{
			INTERNAL_CALL_ToByteArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToByteArray(byte[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToCharArray(char[] array)
		{
			INTERNAL_CALL_ToCharArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToCharArray(char[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToShortArray(short[] array)
		{
			INTERNAL_CALL_ToShortArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToShortArray(short[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToIntArray(int[] array)
		{
			INTERNAL_CALL_ToIntArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToIntArray(int[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToLongArray(long[] array)
		{
			INTERNAL_CALL_ToLongArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToLongArray(long[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToFloatArray(float[] array)
		{
			INTERNAL_CALL_ToFloatArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToFloatArray(float[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToDoubleArray(double[] array)
		{
			INTERNAL_CALL_ToDoubleArray(array, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToDoubleArray(double[] array, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr ToObjectArray(IntPtr[] array, IntPtr arrayClass)
		{
			INTERNAL_CALL_ToObjectArray(array, arrayClass, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ToObjectArray(IntPtr[] array, IntPtr arrayClass, out IntPtr value);

		public static IntPtr ToObjectArray(IntPtr[] array)
		{
			return ToObjectArray(array, IntPtr.Zero);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool[] FromBooleanArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte[] FromByteArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char[] FromCharArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short[] FromShortArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int[] FromIntArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long[] FromLongArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float[] FromFloatArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double[] FromDoubleArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern IntPtr[] FromObjectArray(IntPtr array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetArrayLength(IntPtr array);

		[ThreadAndSerializationSafe]
		public static IntPtr NewBooleanArray(int size)
		{
			INTERNAL_CALL_NewBooleanArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewBooleanArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewByteArray(int size)
		{
			INTERNAL_CALL_NewByteArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewByteArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewCharArray(int size)
		{
			INTERNAL_CALL_NewCharArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewCharArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewShortArray(int size)
		{
			INTERNAL_CALL_NewShortArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewShortArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewIntArray(int size)
		{
			INTERNAL_CALL_NewIntArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewIntArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewLongArray(int size)
		{
			INTERNAL_CALL_NewLongArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewLongArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewFloatArray(int size)
		{
			INTERNAL_CALL_NewFloatArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewFloatArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewDoubleArray(int size)
		{
			INTERNAL_CALL_NewDoubleArray(size, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewDoubleArray(int size, out IntPtr value);

		[ThreadAndSerializationSafe]
		public static IntPtr NewObjectArray(int size, IntPtr clazz, IntPtr obj)
		{
			INTERNAL_CALL_NewObjectArray(size, clazz, obj, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_NewObjectArray(int size, IntPtr clazz, IntPtr obj, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern bool GetBooleanArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern byte GetByteArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern char GetCharArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern short GetShortArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetIntArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern long GetLongArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern float GetFloatArrayElement(IntPtr array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern double GetDoubleArrayElement(IntPtr array, int index);

		[ThreadAndSerializationSafe]
		public static IntPtr GetObjectArrayElement(IntPtr array, int index)
		{
			INTERNAL_CALL_GetObjectArrayElement(array, index, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetObjectArrayElement(IntPtr array, int index, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetBooleanArrayElement(IntPtr array, int index, byte val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetByteArrayElement(IntPtr array, int index, sbyte val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetCharArrayElement(IntPtr array, int index, char val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetShortArrayElement(IntPtr array, int index, short val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetIntArrayElement(IntPtr array, int index, int val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetLongArrayElement(IntPtr array, int index, long val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetFloatArrayElement(IntPtr array, int index, float val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetDoubleArrayElement(IntPtr array, int index, double val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetObjectArrayElement(IntPtr array, int index, IntPtr obj);
	}
}
