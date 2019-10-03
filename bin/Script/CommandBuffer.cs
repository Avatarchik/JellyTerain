using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	[UsedByNativeCode]
	public sealed class CommandBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		public string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int sizeInBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public CommandBuffer()
		{
			m_Ptr = IntPtr.Zero;
			InitBuffer(this);
		}

		~CommandBuffer()
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
			ReleaseBuffer();
			m_Ptr = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void InitBuffer(CommandBuffer buf);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void ReleaseBuffer();

		public void Release()
		{
			Dispose();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetComputeFloatParam(ComputeShader computeShader, string name, float val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetComputeIntParam(ComputeShader computeShader, string name, int val);

		public void SetComputeVectorParam(ComputeShader computeShader, string name, Vector4 val)
		{
			INTERNAL_CALL_SetComputeVectorParam(this, computeShader, name, ref val);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetComputeVectorParam(CommandBuffer self, ComputeShader computeShader, string name, ref Vector4 val);

		public void SetComputeFloatParams(ComputeShader computeShader, string name, params float[] values)
		{
			Internal_SetComputeFloats(computeShader, name, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetComputeFloats(ComputeShader computeShader, string name, float[] values);

		public void SetComputeIntParams(ComputeShader computeShader, string name, params int[] values)
		{
			Internal_SetComputeInts(computeShader, name, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetComputeInts(ComputeShader computeShader, string name, int[] values);

		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt)
		{
			Internal_SetComputeTextureParam(computeShader, kernelIndex, name, ref rt);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, ref RenderTargetIdentifier rt);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, string name, ComputeBuffer buffer);

		public void DispatchCompute(ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
		{
			Internal_DispatchCompute(computeShader, kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DispatchCompute(ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ);

		public void DispatchCompute(ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset)
		{
			Internal_DispatchComputeIndirect(computeShader, kernelIndex, indirectBuffer, argsOffset);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DispatchComputeIndirect(ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void CopyCounterValue(ComputeBuffer src, ComputeBuffer dst, uint dstOffset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Clear();

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
		{
			MaterialPropertyBlock properties = null;
			DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
		}

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex)
		{
			MaterialPropertyBlock properties = null;
			int shaderPass = -1;
			DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
		}

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
		{
			MaterialPropertyBlock properties = null;
			int shaderPass = -1;
			int submeshIndex = 0;
			DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
		}

		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
			{
				submeshIndex = Mathf.Clamp(submeshIndex, 0, mesh.subMeshCount - 1);
				Debug.LogWarning($"submeshIndex out of range. Clampped to {submeshIndex}.");
			}
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			Internal_DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
		}

		private void Internal_DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
		{
			INTERNAL_CALL_Internal_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_DrawMesh(CommandBuffer self, Mesh mesh, ref Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties);

		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material, int submeshIndex)
		{
			int shaderPass = -1;
			DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material)
		{
			int shaderPass = -1;
			int submeshIndex = 0;
			DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		public void DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass)
		{
			if (renderer == null)
			{
				throw new ArgumentNullException("renderer");
			}
			if (submeshIndex < 0)
			{
				submeshIndex = Mathf.Max(submeshIndex, 0);
				Debug.LogWarning($"submeshIndex out of range. Clampped to {submeshIndex}.");
			}
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			Internal_DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass);

		[ExcludeFromDocs]
		private void Internal_DrawRenderer(Renderer renderer, Material material, int submeshIndex)
		{
			int shaderPass = -1;
			Internal_DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		[ExcludeFromDocs]
		private void Internal_DrawRenderer(Renderer renderer, Material material)
		{
			int shaderPass = -1;
			int submeshIndex = 0;
			Internal_DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount)
		{
			MaterialPropertyBlock properties = null;
			DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount)
		{
			MaterialPropertyBlock properties = null;
			int instanceCount = 1;
			DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			Internal_DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		private void Internal_DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties)
		{
			INTERNAL_CALL_Internal_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_DrawProcedural(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties);

		[ExcludeFromDocs]
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			MaterialPropertyBlock properties = null;
			DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		[ExcludeFromDocs]
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
		{
			MaterialPropertyBlock properties = null;
			int argsOffset = 0;
			DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			if (bufferWithArgs == null)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Internal_DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		private void Internal_DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			INTERNAL_CALL_Internal_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Internal_DrawProceduralIndirect(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		[ExcludeFromDocs]
		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count)
		{
			MaterialPropertyBlock properties = null;
			DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, properties);
		}

		[ExcludeFromDocs]
		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices)
		{
			MaterialPropertyBlock properties = null;
			int count = matrices.Length;
			DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, properties);
		}

		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, [DefaultValue("matrices.Length")] int count, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			if (!SystemInfo.supportsInstancing)
			{
				throw new InvalidOperationException("DrawMeshInstanced is not supported.");
			}
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			if (matrices == null)
			{
				throw new ArgumentNullException("matrices");
			}
			if (count < 0 || count > Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length))
			{
				throw new ArgumentOutOfRangeException("count", $"Count must be in the range of 0 to {Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length)}.");
			}
			if (count > 0)
			{
				Internal_DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, properties);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties);

		[ExcludeFromDocs]
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			MaterialPropertyBlock properties = null;
			DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
		}

		[ExcludeFromDocs]
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs)
		{
			MaterialPropertyBlock properties = null;
			int argsOffset = 0;
			DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
		}

		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			if (!SystemInfo.supportsInstancing)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			if (bufferWithArgs == null)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			Internal_DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		public void SetRenderTarget(RenderTargetIdentifier rt)
		{
			SetRenderTarget_Single(ref rt, 0, CubemapFace.Unknown, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel)
		{
			SetRenderTarget_Single(ref rt, mipLevel, CubemapFace.Unknown, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace)
		{
			SetRenderTarget_Single(ref rt, mipLevel, cubemapFace, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			SetRenderTarget_Single(ref rt, mipLevel, cubemapFace, depthSlice);
		}

		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth)
		{
			SetRenderTarget_ColDepth(ref color, ref depth, 0, CubemapFace.Unknown, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel)
		{
			SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, CubemapFace.Unknown, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace)
		{
			SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, cubemapFace, 0);
		}

		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, cubemapFace, depthSlice);
		}

		public void SetRenderTarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth)
		{
			SetRenderTarget_Multiple(colors, ref depth);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetRenderTarget_Single(ref RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace, int depthSlice);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetRenderTarget_ColDepth(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetRenderTarget_Multiple(RenderTargetIdentifier[] color, ref RenderTargetIdentifier depth);

		public void CopyTexture(RenderTargetIdentifier src, RenderTargetIdentifier dst)
		{
			CopyTexture_Internal(ref src, -1, -1, -1, -1, -1, -1, ref dst, -1, -1, -1, -1, 1);
		}

		public void CopyTexture(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement)
		{
			CopyTexture_Internal(ref src, srcElement, -1, -1, -1, -1, -1, ref dst, dstElement, -1, -1, -1, 2);
		}

		public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip, RenderTargetIdentifier dst, int dstElement, int dstMip)
		{
			CopyTexture_Internal(ref src, srcElement, srcMip, -1, -1, -1, -1, ref dst, dstElement, dstMip, -1, -1, 3);
		}

		public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY)
		{
			CopyTexture_Internal(ref src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, ref dst, dstElement, dstMip, dstX, dstY, 4);
		}

		public void SetViewport(Rect pixelRect)
		{
			INTERNAL_CALL_SetViewport(this, ref pixelRect);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetViewport(CommandBuffer self, ref Rect pixelRect);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void CopyTexture_Internal(ref RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, ref RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY, int mode);

		public void Blit(Texture source, RenderTargetIdentifier dest)
		{
			Blit_Texture(source, ref dest, null, -1);
		}

		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat)
		{
			Blit_Texture(source, ref dest, mat, -1);
		}

		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			Blit_Texture(source, ref dest, mat, pass);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Blit_Texture(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass);

		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
		{
			Blit_Identifier(ref source, ref dest, null, -1);
		}

		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
		{
			Blit_Identifier(ref source, ref dest, mat, -1);
		}

		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			Blit_Identifier(ref source, ref dest, mat, pass);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass);

		[ExcludeFromDocs]
		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, Material mat)
		{
			int pass = -1;
			Blit_Identifier(ref source, ref dest, mat, pass);
		}

		[ExcludeFromDocs]
		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest)
		{
			int pass = -1;
			Material mat = null;
			Blit_Identifier(ref source, ref dest, mat, pass);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void GetTemporaryRT(int nameID, int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("FilterMode.Point")] FilterMode filter, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing, [DefaultValue("false")] bool enableRandomWrite);

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
		{
			bool enableRandomWrite = false;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			bool enableRandomWrite = false;
			int antiAliasing = 1;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
		{
			bool enableRandomWrite = false;
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
		{
			bool enableRandomWrite = false;
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
		{
			bool enableRandomWrite = false;
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			FilterMode filter = FilterMode.Point;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height)
		{
			bool enableRandomWrite = false;
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			FilterMode filter = FilterMode.Point;
			int depthBuffer = 0;
			GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void ReleaseTemporaryRT(int nameID);

		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
		}

		[ExcludeFromDocs]
		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			float depth = 1f;
			INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ClearRenderTarget(CommandBuffer self, bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		public void SetGlobalFloat(string name, float value)
		{
			SetGlobalFloat(Shader.PropertyToID(name), value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalFloat(int nameID, float value);

		public void SetGlobalVector(string name, Vector4 value)
		{
			SetGlobalVector(Shader.PropertyToID(name), value);
		}

		public void SetGlobalVector(int nameID, Vector4 value)
		{
			INTERNAL_CALL_SetGlobalVector(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetGlobalVector(CommandBuffer self, int nameID, ref Vector4 value);

		public void SetGlobalColor(string name, Color value)
		{
			SetGlobalColor(Shader.PropertyToID(name), value);
		}

		public void SetGlobalColor(int nameID, Color value)
		{
			INTERNAL_CALL_SetGlobalColor(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetGlobalColor(CommandBuffer self, int nameID, ref Color value);

		public void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			SetGlobalMatrix(Shader.PropertyToID(name), value);
		}

		public void SetGlobalMatrix(int nameID, Matrix4x4 value)
		{
			INTERNAL_CALL_SetGlobalMatrix(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetGlobalMatrix(CommandBuffer self, int nameID, ref Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void EnableShaderKeyword(string keyword);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void DisableShaderKeyword(string keyword);

		public void SetViewMatrix(Matrix4x4 view)
		{
			INTERNAL_CALL_SetViewMatrix(this, ref view);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetViewMatrix(CommandBuffer self, ref Matrix4x4 view);

		public void SetProjectionMatrix(Matrix4x4 proj)
		{
			INTERNAL_CALL_SetProjectionMatrix(this, ref proj);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetProjectionMatrix(CommandBuffer self, ref Matrix4x4 proj);

		public void SetViewProjectionMatrices(Matrix4x4 view, Matrix4x4 proj)
		{
			INTERNAL_CALL_SetViewProjectionMatrices(this, ref view, ref proj);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetViewProjectionMatrices(CommandBuffer self, ref Matrix4x4 view, ref Matrix4x4 proj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalDepthBias(float bias, float slopeBias);

		public void SetGlobalFloatArray(string propertyName, List<float> values)
		{
			SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
		}

		public void SetGlobalFloatArray(int nameID, List<float> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Count == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetGlobalFloatArrayListImpl(nameID, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetGlobalFloatArrayListImpl(int nameID, object values);

		public void SetGlobalFloatArray(string propertyName, float[] values)
		{
			SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalFloatArray(int nameID, float[] values);

		public void SetGlobalVectorArray(string propertyName, List<Vector4> values)
		{
			SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
		}

		public void SetGlobalVectorArray(int nameID, List<Vector4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Count == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetGlobalVectorArrayListImpl(nameID, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetGlobalVectorArrayListImpl(int nameID, object values);

		public void SetGlobalVectorArray(string propertyName, Vector4[] values)
		{
			SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalVectorArray(int nameID, Vector4[] values);

		public void SetGlobalMatrixArray(string propertyName, List<Matrix4x4> values)
		{
			SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
		}

		public void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Count == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetGlobalMatrixArrayListImpl(nameID, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetGlobalMatrixArrayListImpl(int nameID, object values);

		public void SetGlobalMatrixArray(string propertyName, Matrix4x4[] values)
		{
			SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalMatrixArray(int nameID, Matrix4x4[] values);

		public void SetGlobalTexture(string name, RenderTargetIdentifier value)
		{
			SetGlobalTexture(Shader.PropertyToID(name), value);
		}

		public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
		{
			SetGlobalTexture_Impl(nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetGlobalTexture_Impl(int nameID, ref RenderTargetIdentifier rt);

		public void SetGlobalBuffer(string name, ComputeBuffer value)
		{
			SetGlobalBuffer(Shader.PropertyToID(name), value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetGlobalBuffer(int nameID, ComputeBuffer value);

		public void SetShadowSamplingMode(RenderTargetIdentifier shadowmap, ShadowSamplingMode mode)
		{
			SetShadowSamplingMode_Impl(ref shadowmap, mode);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetShadowSamplingMode_Impl(ref RenderTargetIdentifier shadowmap, ShadowSamplingMode mode);

		public void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			IssuePluginEventInternal(callback, eventID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void IssuePluginEventInternal(IntPtr callback, int eventID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void BeginSample(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void EndSample(string name);
	}
}
