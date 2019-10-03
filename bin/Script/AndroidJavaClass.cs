using System;

namespace UnityEngine
{
	public class AndroidJavaClass : AndroidJavaObject
	{
		public AndroidJavaClass(string className)
		{
			_AndroidJavaClass(className);
		}

		internal AndroidJavaClass(IntPtr jclass)
		{
			if (jclass == IntPtr.Zero)
			{
				throw new Exception("JNI: Init'd AndroidJavaClass with null ptr!");
			}
			m_jclass = AndroidJNI.NewGlobalRef(jclass);
			m_jobject = IntPtr.Zero;
		}

		private void _AndroidJavaClass(string className)
		{
			DebugPrint("Creating AndroidJavaClass from " + className);
			using (AndroidJavaObject androidJavaObject = AndroidJavaObject.FindClass(className))
			{
				m_jclass = AndroidJNI.NewGlobalRef(androidJavaObject.GetRawObject());
				m_jobject = IntPtr.Zero;
			}
		}
	}
}
