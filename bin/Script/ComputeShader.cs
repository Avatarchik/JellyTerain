using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class ComputeShader : Object
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int FindKernel(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool HasKernel(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void GetKernelThreadGroupSizes(int kernelIndex, out uint x, out uint y, out uint z);

		public void SetFloat(string name, float val)
		{
			SetFloat(Shader.PropertyToID(name), val);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetFloat(int nameID, float val);

		public void SetInt(string name, int val)
		{
			SetInt(Shader.PropertyToID(name), val);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetInt(int nameID, int val);

		public void SetBool(string name, bool val)
		{
			SetInt(name, val ? 1 : 0);
		}

		public void SetBool(int nameID, bool val)
		{
			SetInt(nameID, val ? 1 : 0);
		}

		public void SetVector(string name, Vector4 val)
		{
			SetVector(Shader.PropertyToID(name), val);
		}

		public void SetVector(int nameID, Vector4 val)
		{
			INTERNAL_CALL_SetVector(this, nameID, ref val);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetVector(ComputeShader self, int nameID, ref Vector4 val);

		public void SetFloats(string name, params float[] values)
		{
			Internal_SetFloats(Shader.PropertyToID(name), values);
		}

		public void SetFloats(int nameID, params float[] values)
		{
			Internal_SetFloats(nameID, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetFloats(int nameID, float[] values);

		public void SetInts(string name, params int[] values)
		{
			Internal_SetInts(Shader.PropertyToID(name), values);
		}

		public void SetInts(int nameID, params int[] values)
		{
			Internal_SetInts(nameID, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetInts(int nameID, int[] values);

		public void SetTexture(int kernelIndex, string name, Texture texture)
		{
			SetTexture(kernelIndex, Shader.PropertyToID(name), texture);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetTexture(int kernelIndex, int nameID, Texture texture);

		public void SetTextureFromGlobal(int kernelIndex, string name, string globalTextureName)
		{
			SetTextureFromGlobal(kernelIndex, Shader.PropertyToID(name), Shader.PropertyToID(globalTextureName));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetTextureFromGlobal(int kernelIndex, int nameID, int globalTextureNameID);

		public void SetBuffer(int kernelIndex, string name, ComputeBuffer buffer)
		{
			SetBuffer(kernelIndex, Shader.PropertyToID(name), buffer);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetBuffer(int kernelIndex, int nameID, ComputeBuffer buffer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispatch(int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ);

		[ExcludeFromDocs]
		public void DispatchIndirect(int kernelIndex, ComputeBuffer argsBuffer)
		{
			uint argsOffset = 0u;
			DispatchIndirect(kernelIndex, argsBuffer, argsOffset);
		}

		public void DispatchIndirect(int kernelIndex, ComputeBuffer argsBuffer, [DefaultValue("0")] uint argsOffset)
		{
			if (argsBuffer == null)
			{
				throw new ArgumentNullException("argsBuffer");
			}
			if (argsBuffer.m_Ptr == IntPtr.Zero)
			{
				throw new ObjectDisposedException("argsBuffer");
			}
			Internal_DispatchIndirect(kernelIndex, argsBuffer, argsOffset);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DispatchIndirect(int kernelIndex, ComputeBuffer argsBuffer, uint argsOffset);
	}
}
