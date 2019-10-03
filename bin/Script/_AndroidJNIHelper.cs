using System;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	internal sealed class _AndroidJNIHelper
	{
		public static IntPtr CreateJavaProxy(int delegateHandle, AndroidJavaProxy proxy)
		{
			return AndroidReflection.NewProxyInstance(delegateHandle, proxy.javaInterface.GetRawClass());
		}

		public static IntPtr CreateJavaRunnable(AndroidJavaRunnable jrunnable)
		{
			return AndroidJNIHelper.CreateJavaProxy(new AndroidJavaRunnableProxy(jrunnable));
		}

		public static IntPtr InvokeJavaProxyMethod(AndroidJavaProxy proxy, IntPtr jmethodName, IntPtr jargs)
		{
			int num = 0;
			if (jargs != IntPtr.Zero)
			{
				num = AndroidJNISafe.GetArrayLength(jargs);
			}
			AndroidJavaObject[] array = new AndroidJavaObject[num];
			for (int i = 0; i < num; i++)
			{
				IntPtr objectArrayElement = AndroidJNISafe.GetObjectArrayElement(jargs, i);
				array[i] = ((!(objectArrayElement != IntPtr.Zero)) ? null : new AndroidJavaObject(objectArrayElement));
			}
			using (AndroidJavaObject androidJavaObject = proxy.Invoke(AndroidJNI.GetStringUTFChars(jmethodName), array))
			{
				if (androidJavaObject == null)
				{
					return IntPtr.Zero;
				}
				return AndroidJNI.NewLocalRef(androidJavaObject.GetRawObject());
			}
		}

		public static jvalue[] CreateJNIArgArray(object[] args)
		{
			jvalue[] array = new jvalue[args.GetLength(0)];
			int num = 0;
			foreach (object obj in args)
			{
				if (obj == null)
				{
					array[num].l = IntPtr.Zero;
				}
				else if (AndroidReflection.IsPrimitive(obj.GetType()))
				{
					if (obj is int)
					{
						array[num].i = (int)obj;
					}
					else if (obj is bool)
					{
						array[num].z = (bool)obj;
					}
					else if (obj is byte)
					{
						array[num].b = (byte)obj;
					}
					else if (obj is short)
					{
						array[num].s = (short)obj;
					}
					else if (obj is long)
					{
						array[num].j = (long)obj;
					}
					else if (obj is float)
					{
						array[num].f = (float)obj;
					}
					else if (obj is double)
					{
						array[num].d = (double)obj;
					}
					else if (obj is char)
					{
						array[num].c = (char)obj;
					}
				}
				else if (obj is string)
				{
					array[num].l = AndroidJNISafe.NewStringUTF((string)obj);
				}
				else if (obj is AndroidJavaClass)
				{
					array[num].l = ((AndroidJavaClass)obj).GetRawClass();
				}
				else if (obj is AndroidJavaObject)
				{
					array[num].l = ((AndroidJavaObject)obj).GetRawObject();
				}
				else if (obj is Array)
				{
					array[num].l = ConvertToJNIArray((Array)obj);
				}
				else if (obj is AndroidJavaProxy)
				{
					array[num].l = AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy)obj);
				}
				else
				{
					if (!(obj is AndroidJavaRunnable))
					{
						throw new Exception("JNI; Unknown argument type '" + obj.GetType() + "'");
					}
					array[num].l = AndroidJNIHelper.CreateJavaRunnable((AndroidJavaRunnable)obj);
				}
				num++;
			}
			return array;
		}

		public static object UnboxArray(AndroidJavaObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("java/lang/reflect/Array");
			AndroidJavaObject androidJavaObject = obj.Call<AndroidJavaObject>("getClass", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getComponentType", new object[0]);
			string text = androidJavaObject2.Call<string>("getName", new object[0]);
			int num = androidJavaClass.Call<int>("getLength", new object[1]
			{
				obj
			});
			Array array;
			if (!androidJavaObject2.Call<bool>("IsPrimitive", new object[0]))
			{
				array = (Array)(("java.lang.String" == text) ? ((object)new string[num]) : ((object)((!("java.lang.Class" == text)) ? new AndroidJavaObject[num] : new AndroidJavaClass[num])));
			}
			else if ("I" == text)
			{
				array = new int[num];
			}
			else if ("Z" == text)
			{
				array = new bool[num];
			}
			else if ("B" == text)
			{
				array = new byte[num];
			}
			else if ("S" == text)
			{
				array = new short[num];
			}
			else if ("J" == text)
			{
				array = new long[num];
			}
			else if ("F" == text)
			{
				array = new float[num];
			}
			else if ("D" == text)
			{
				array = new double[num];
			}
			else
			{
				if (!("C" == text))
				{
					throw new Exception("JNI; Unknown argument type '" + text + "'");
				}
				array = new char[num];
			}
			for (int i = 0; i < num; i++)
			{
				array.SetValue(Unbox(androidJavaClass.CallStatic<AndroidJavaObject>("get", new object[2]
				{
					obj,
					i
				})), i);
			}
			return array;
		}

		public static object Unbox(AndroidJavaObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = obj.Call<AndroidJavaObject>("getClass", new object[0]);
			string b = androidJavaObject.Call<string>("getName", new object[0]);
			if ("java.lang.Integer" == b)
			{
				return obj.Call<int>("intValue", new object[0]);
			}
			if ("java.lang.Boolean" == b)
			{
				return obj.Call<bool>("booleanValue", new object[0]);
			}
			if ("java.lang.Byte" == b)
			{
				return obj.Call<byte>("byteValue", new object[0]);
			}
			if ("java.lang.Short" == b)
			{
				return obj.Call<short>("shortValue", new object[0]);
			}
			if ("java.lang.Long" == b)
			{
				return obj.Call<long>("longValue", new object[0]);
			}
			if ("java.lang.Float" == b)
			{
				return obj.Call<float>("floatValue", new object[0]);
			}
			if ("java.lang.Double" == b)
			{
				return obj.Call<double>("doubleValue", new object[0]);
			}
			if ("java.lang.Character" == b)
			{
				return obj.Call<char>("charValue", new object[0]);
			}
			if ("java.lang.String" == b)
			{
				return obj.Call<string>("toString", new object[0]);
			}
			if ("java.lang.Class" == b)
			{
				return new AndroidJavaClass(obj.GetRawObject());
			}
			if (androidJavaObject.Call<bool>("isArray", new object[0]))
			{
				return UnboxArray(obj);
			}
			return obj;
		}

		public static AndroidJavaObject Box(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (AndroidReflection.IsPrimitive(obj.GetType()))
			{
				if (obj is int)
				{
					return new AndroidJavaObject("java.lang.Integer", (int)obj);
				}
				if (obj is bool)
				{
					return new AndroidJavaObject("java.lang.Boolean", (bool)obj);
				}
				if (obj is byte)
				{
					return new AndroidJavaObject("java.lang.Byte", (byte)obj);
				}
				if (obj is short)
				{
					return new AndroidJavaObject("java.lang.Short", (short)obj);
				}
				if (obj is long)
				{
					return new AndroidJavaObject("java.lang.Long", (long)obj);
				}
				if (obj is float)
				{
					return new AndroidJavaObject("java.lang.Float", (float)obj);
				}
				if (obj is double)
				{
					return new AndroidJavaObject("java.lang.Double", (double)obj);
				}
				if (obj is char)
				{
					return new AndroidJavaObject("java.lang.Character", (char)obj);
				}
				throw new Exception("JNI; Unknown argument type '" + obj.GetType() + "'");
			}
			if (obj is string)
			{
				return new AndroidJavaObject("java.lang.String", (string)obj);
			}
			if (obj is AndroidJavaClass)
			{
				return new AndroidJavaObject(((AndroidJavaClass)obj).GetRawClass());
			}
			if (obj is AndroidJavaObject)
			{
				return (AndroidJavaObject)obj;
			}
			if (obj is Array)
			{
				return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(ConvertToJNIArray((Array)obj));
			}
			if (obj is AndroidJavaProxy)
			{
				return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy)obj));
			}
			if (obj is AndroidJavaRunnable)
			{
				return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNIHelper.CreateJavaRunnable((AndroidJavaRunnable)obj));
			}
			throw new Exception("JNI; Unknown argument type '" + obj.GetType() + "'");
		}

		public static void DeleteJNIArgArray(object[] args, jvalue[] jniArgs)
		{
			int num = 0;
			foreach (object obj in args)
			{
				if (obj is string || obj is AndroidJavaRunnable || obj is AndroidJavaProxy || obj is Array)
				{
					AndroidJNISafe.DeleteLocalRef(jniArgs[num].l);
				}
				num++;
			}
		}

		public static IntPtr ConvertToJNIArray(Array array)
		{
			Type elementType = array.GetType().GetElementType();
			if (AndroidReflection.IsPrimitive(elementType))
			{
				if (elementType == typeof(int))
				{
					return AndroidJNISafe.ToIntArray((int[])array);
				}
				if (elementType == typeof(bool))
				{
					return AndroidJNISafe.ToBooleanArray((bool[])array);
				}
				if (elementType == typeof(byte))
				{
					return AndroidJNISafe.ToByteArray((byte[])array);
				}
				if (elementType == typeof(short))
				{
					return AndroidJNISafe.ToShortArray((short[])array);
				}
				if (elementType == typeof(long))
				{
					return AndroidJNISafe.ToLongArray((long[])array);
				}
				if (elementType == typeof(float))
				{
					return AndroidJNISafe.ToFloatArray((float[])array);
				}
				if (elementType == typeof(double))
				{
					return AndroidJNISafe.ToDoubleArray((double[])array);
				}
				if (elementType == typeof(char))
				{
					return AndroidJNISafe.ToCharArray((char[])array);
				}
				return IntPtr.Zero;
			}
			if (elementType == typeof(string))
			{
				string[] array2 = (string[])array;
				int length = array.GetLength(0);
				IntPtr intPtr = AndroidJNISafe.FindClass("java/lang/String");
				IntPtr intPtr2 = AndroidJNI.NewObjectArray(length, intPtr, IntPtr.Zero);
				for (int i = 0; i < length; i++)
				{
					IntPtr intPtr3 = AndroidJNISafe.NewStringUTF(array2[i]);
					AndroidJNI.SetObjectArrayElement(intPtr2, i, intPtr3);
					AndroidJNISafe.DeleteLocalRef(intPtr3);
				}
				AndroidJNISafe.DeleteLocalRef(intPtr);
				return intPtr2;
			}
			if (elementType == typeof(AndroidJavaObject))
			{
				AndroidJavaObject[] array3 = (AndroidJavaObject[])array;
				int length2 = array.GetLength(0);
				IntPtr[] array4 = new IntPtr[length2];
				IntPtr intPtr4 = AndroidJNISafe.FindClass("java/lang/Object");
				IntPtr intPtr5 = IntPtr.Zero;
				for (int j = 0; j < length2; j++)
				{
					if (array3[j] != null)
					{
						array4[j] = array3[j].GetRawObject();
						IntPtr rawClass = array3[j].GetRawClass();
						if (intPtr5 != rawClass)
						{
							intPtr5 = ((!(intPtr5 == IntPtr.Zero)) ? intPtr4 : rawClass);
						}
					}
					else
					{
						array4[j] = IntPtr.Zero;
					}
				}
				IntPtr result = AndroidJNISafe.ToObjectArray(array4, intPtr5);
				AndroidJNISafe.DeleteLocalRef(intPtr4);
				return result;
			}
			throw new Exception("JNI; Unknown array type '" + elementType + "'");
		}

		public static ArrayType ConvertFromJNIArray<ArrayType>(IntPtr array)
		{
			Type elementType = typeof(ArrayType).GetElementType();
			if (AndroidReflection.IsPrimitive(elementType))
			{
				if (elementType == typeof(int))
				{
					return (ArrayType)(object)AndroidJNISafe.FromIntArray(array);
				}
				if (elementType == typeof(bool))
				{
					return (ArrayType)(object)AndroidJNISafe.FromBooleanArray(array);
				}
				if (elementType == typeof(byte))
				{
					return (ArrayType)(object)AndroidJNISafe.FromByteArray(array);
				}
				if (elementType == typeof(short))
				{
					return (ArrayType)(object)AndroidJNISafe.FromShortArray(array);
				}
				if (elementType == typeof(long))
				{
					return (ArrayType)(object)AndroidJNISafe.FromLongArray(array);
				}
				if (elementType == typeof(float))
				{
					return (ArrayType)(object)AndroidJNISafe.FromFloatArray(array);
				}
				if (elementType == typeof(double))
				{
					return (ArrayType)(object)AndroidJNISafe.FromDoubleArray(array);
				}
				if (elementType == typeof(char))
				{
					return (ArrayType)(object)AndroidJNISafe.FromCharArray(array);
				}
				return default(ArrayType);
			}
			if (elementType == typeof(string))
			{
				int arrayLength = AndroidJNISafe.GetArrayLength(array);
				string[] array2 = new string[arrayLength];
				for (int i = 0; i < arrayLength; i++)
				{
					IntPtr objectArrayElement = AndroidJNI.GetObjectArrayElement(array, i);
					array2[i] = AndroidJNISafe.GetStringUTFChars(objectArrayElement);
					AndroidJNISafe.DeleteLocalRef(objectArrayElement);
				}
				return (ArrayType)(object)array2;
			}
			if (elementType == typeof(AndroidJavaObject))
			{
				int arrayLength2 = AndroidJNISafe.GetArrayLength(array);
				AndroidJavaObject[] array3 = new AndroidJavaObject[arrayLength2];
				for (int j = 0; j < arrayLength2; j++)
				{
					IntPtr objectArrayElement2 = AndroidJNI.GetObjectArrayElement(array, j);
					array3[j] = new AndroidJavaObject(objectArrayElement2);
					AndroidJNISafe.DeleteLocalRef(objectArrayElement2);
				}
				return (ArrayType)(object)array3;
			}
			throw new Exception("JNI: Unknown generic array type '" + elementType + "'");
		}

		public static IntPtr GetConstructorID(IntPtr jclass, object[] args)
		{
			return AndroidJNIHelper.GetConstructorID(jclass, GetSignature(args));
		}

		public static IntPtr GetMethodID(IntPtr jclass, string methodName, object[] args, bool isStatic)
		{
			return AndroidJNIHelper.GetMethodID(jclass, methodName, GetSignature(args), isStatic);
		}

		public static IntPtr GetMethodID<ReturnType>(IntPtr jclass, string methodName, object[] args, bool isStatic)
		{
			return AndroidJNIHelper.GetMethodID(jclass, methodName, GetSignature<ReturnType>(args), isStatic);
		}

		public static IntPtr GetFieldID<ReturnType>(IntPtr jclass, string fieldName, bool isStatic)
		{
			return AndroidJNIHelper.GetFieldID(jclass, fieldName, GetSignature(typeof(ReturnType)), isStatic);
		}

		public static IntPtr GetConstructorID(IntPtr jclass, string signature)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = AndroidReflection.GetConstructorMember(jclass, signature);
				return AndroidJNISafe.FromReflectedMethod(intPtr);
			}
			catch (Exception ex)
			{
				IntPtr methodID = AndroidJNISafe.GetMethodID(jclass, "<init>", signature);
				if (!(methodID != IntPtr.Zero))
				{
					throw ex;
				}
				return methodID;
			}
			finally
			{
				AndroidJNISafe.DeleteLocalRef(intPtr);
			}
		}

		public static IntPtr GetMethodID(IntPtr jclass, string methodName, string signature, bool isStatic)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = AndroidReflection.GetMethodMember(jclass, methodName, signature, isStatic);
				return AndroidJNISafe.FromReflectedMethod(intPtr);
			}
			catch (Exception ex)
			{
				IntPtr methodIDFallback = GetMethodIDFallback(jclass, methodName, signature, isStatic);
				if (!(methodIDFallback != IntPtr.Zero))
				{
					throw ex;
				}
				return methodIDFallback;
			}
			finally
			{
				AndroidJNISafe.DeleteLocalRef(intPtr);
			}
		}

		private static IntPtr GetMethodIDFallback(IntPtr jclass, string methodName, string signature, bool isStatic)
		{
			try
			{
				return (!isStatic) ? AndroidJNISafe.GetMethodID(jclass, methodName, signature) : AndroidJNISafe.GetStaticMethodID(jclass, methodName, signature);
			}
			catch (Exception)
			{
			}
			return IntPtr.Zero;
		}

		public static IntPtr GetFieldID(IntPtr jclass, string fieldName, string signature, bool isStatic)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = AndroidReflection.GetFieldMember(jclass, fieldName, signature, isStatic);
				return AndroidJNISafe.FromReflectedField(intPtr);
			}
			catch (Exception ex)
			{
				IntPtr intPtr2 = (!isStatic) ? AndroidJNISafe.GetFieldID(jclass, fieldName, signature) : AndroidJNISafe.GetStaticFieldID(jclass, fieldName, signature);
				if (!(intPtr2 != IntPtr.Zero))
				{
					throw ex;
				}
				return intPtr2;
			}
			finally
			{
				AndroidJNISafe.DeleteLocalRef(intPtr);
			}
		}

		public static string GetSignature(object obj)
		{
			if (obj == null)
			{
				return "Ljava/lang/Object;";
			}
			Type type = (!(obj is Type)) ? obj.GetType() : ((Type)obj);
			if (AndroidReflection.IsPrimitive(type))
			{
				if (type.Equals(typeof(int)))
				{
					return "I";
				}
				if (type.Equals(typeof(bool)))
				{
					return "Z";
				}
				if (type.Equals(typeof(byte)))
				{
					return "B";
				}
				if (type.Equals(typeof(short)))
				{
					return "S";
				}
				if (type.Equals(typeof(long)))
				{
					return "J";
				}
				if (type.Equals(typeof(float)))
				{
					return "F";
				}
				if (type.Equals(typeof(double)))
				{
					return "D";
				}
				if (type.Equals(typeof(char)))
				{
					return "C";
				}
				return "";
			}
			if (type.Equals(typeof(string)))
			{
				return "Ljava/lang/String;";
			}
			if (obj is AndroidJavaProxy)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject(((AndroidJavaProxy)obj).javaInterface.GetRawClass());
				return "L" + androidJavaObject.Call<string>("getName", new object[0]) + ";";
			}
			if (type.Equals(typeof(AndroidJavaRunnable)))
			{
				return "Ljava/lang/Runnable;";
			}
			if (type.Equals(typeof(AndroidJavaClass)))
			{
				return "Ljava/lang/Class;";
			}
			if (type.Equals(typeof(AndroidJavaObject)))
			{
				if (obj == type)
				{
					return "Ljava/lang/Object;";
				}
				AndroidJavaObject androidJavaObject2 = (AndroidJavaObject)obj;
				using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getClass", new object[0]))
				{
					return "L" + androidJavaObject3.Call<string>("getName", new object[0]) + ";";
				}
			}
			if (AndroidReflection.IsAssignableFrom(typeof(Array), type))
			{
				if (type.GetArrayRank() != 1)
				{
					throw new Exception("JNI: System.Array in n dimensions is not allowed");
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('[');
				stringBuilder.Append(GetSignature(type.GetElementType()));
				return stringBuilder.ToString();
			}
			throw new Exception("JNI: Unknown signature for type '" + type + "' (obj = " + obj + ") " + ((type != obj) ? "instance" : "equal"));
		}

		public static string GetSignature(object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			foreach (object obj in args)
			{
				stringBuilder.Append(GetSignature(obj));
			}
			stringBuilder.Append(")V");
			return stringBuilder.ToString();
		}

		public static string GetSignature<ReturnType>(object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			foreach (object obj in args)
			{
				stringBuilder.Append(GetSignature(obj));
			}
			stringBuilder.Append(')');
			stringBuilder.Append(GetSignature(typeof(ReturnType)));
			return stringBuilder.ToString();
		}
	}
}
