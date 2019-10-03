using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class ComputeBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		public int count
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int stride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ComputeBuffer(int count, int stride)
			: this(count, stride, ComputeBufferType.Default, 3)
		{
		}

		public ComputeBuffer(int count, int stride, ComputeBufferType type)
			: this(count, stride, type, 3)
		{
		}

		internal ComputeBuffer(int count, int stride, ComputeBufferType type, int stackDepth)
		{
			if (count <= 0)
			{
				throw new ArgumentException("Attempting to create a zero length compute buffer", "count");
			}
			if (stride < 0)
			{
				throw new ArgumentException("Attempting to create a compute buffer with a negative stride", "stride");
			}
			m_Ptr = IntPtr.Zero;
			InitBuffer(this, count, stride, type);
		}

		~ComputeBuffer()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				DestroyBuffer(this);
			}
			else if (m_Ptr != IntPtr.Zero)
			{
				Debug.LogWarning("GarbageCollector disposing of ComputeBuffer. Please use ComputeBuffer.Release() or .Dispose() to manually release the buffer.");
			}
			m_Ptr = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void InitBuffer(ComputeBuffer buf, int count, int stride, ComputeBufferType type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void DestroyBuffer(ComputeBuffer buf);

		public void Release()
		{
			Dispose();
		}

		[SecuritySafeCritical]
		public void SetData(Array data)
		{
			InternalSetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SecurityCritical]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalSetData(Array data, int elemSize);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetCounterValue(uint counterValue);

		[SecuritySafeCritical]
		public void GetData(Array data)
		{
			InternalGetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[SecurityCritical]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalGetData(Array data, int elemSize);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffset);

		public IntPtr GetNativeBufferPtr()
		{
			INTERNAL_CALL_GetNativeBufferPtr(this, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetNativeBufferPtr(ComputeBuffer self, out IntPtr value);
	}
}
