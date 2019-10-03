using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class MaterialPropertyBlock
	{
		internal IntPtr m_Ptr;

		public bool isEmpty
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public MaterialPropertyBlock()
		{
			InitBlock();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InitBlock();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		internal extern void DestroyBlock();

		~MaterialPropertyBlock()
		{
			DestroyBlock();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Clear();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatImpl(int nameID, float value);

		private void SetVectorImpl(int nameID, Vector4 value)
		{
			INTERNAL_CALL_SetVectorImpl(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetVectorImpl(MaterialPropertyBlock self, int nameID, ref Vector4 value);

		private void SetMatrixImpl(int nameID, Matrix4x4 value)
		{
			INTERNAL_CALL_SetMatrixImpl(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetMatrixImpl(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetTextureImpl(int nameID, Texture value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetBufferImpl(int nameID, ComputeBuffer value);

		private void SetColorImpl(int nameID, Color value)
		{
			INTERNAL_CALL_SetColorImpl(this, nameID, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetColorImpl(MaterialPropertyBlock self, int nameID, ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Array ExtractArrayFromList(object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetFloatArrayImpl(int nameID, float[] values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetVectorArrayImpl(int nameID, Vector4[] values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetMatrixArrayImpl(int nameID, Matrix4x4[] values);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern float GetFloatImpl(int nameID);

		private Vector4 GetVectorImpl(int nameID)
		{
			INTERNAL_CALL_GetVectorImpl(this, nameID, out Vector4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetVectorImpl(MaterialPropertyBlock self, int nameID, out Vector4 value);

		private Matrix4x4 GetMatrixImpl(int nameID)
		{
			INTERNAL_CALL_GetMatrixImpl(this, nameID, out Matrix4x4 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetMatrixImpl(MaterialPropertyBlock self, int nameID, out Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern float[] GetFloatArrayImpl(int nameID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Vector4[] GetVectorArrayImpl(int nameID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Matrix4x4[] GetMatrixArrayImpl(int nameID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetFloatArrayImplList(int nameID, object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetVectorArrayImplList(int nameID, object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetMatrixArrayImplList(int nameID, object list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Texture GetTextureImpl(int nameID);

		public void SetFloat(string name, float value)
		{
			SetFloat(Shader.PropertyToID(name), value);
		}

		public void SetFloat(int nameID, float value)
		{
			SetFloatImpl(nameID, value);
		}

		public void SetVector(string name, Vector4 value)
		{
			SetVector(Shader.PropertyToID(name), value);
		}

		public void SetVector(int nameID, Vector4 value)
		{
			SetVectorImpl(nameID, value);
		}

		public void SetColor(string name, Color value)
		{
			SetColor(Shader.PropertyToID(name), value);
		}

		public void SetColor(int nameID, Color value)
		{
			SetColorImpl(nameID, value);
		}

		public void SetMatrix(string name, Matrix4x4 value)
		{
			SetMatrix(Shader.PropertyToID(name), value);
		}

		public void SetMatrix(int nameID, Matrix4x4 value)
		{
			SetMatrixImpl(nameID, value);
		}

		public void SetBuffer(string name, ComputeBuffer value)
		{
			SetBuffer(Shader.PropertyToID(name), value);
		}

		public void SetBuffer(int nameID, ComputeBuffer value)
		{
			SetBufferImpl(nameID, value);
		}

		public void SetTexture(string name, Texture value)
		{
			SetTexture(Shader.PropertyToID(name), value);
		}

		public void SetTexture(int nameID, Texture value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetTextureImpl(nameID, value);
		}

		public void SetFloatArray(string name, List<float> values)
		{
			SetFloatArray(Shader.PropertyToID(name), values);
		}

		public void SetFloatArray(int nameID, List<float> values)
		{
			SetFloatArray(nameID, (float[])ExtractArrayFromList(values));
		}

		public void SetFloatArray(string name, float[] values)
		{
			SetFloatArray(Shader.PropertyToID(name), values);
		}

		public void SetFloatArray(int nameID, float[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetFloatArrayImpl(nameID, values);
		}

		public void SetVectorArray(string name, List<Vector4> values)
		{
			SetVectorArray(Shader.PropertyToID(name), values);
		}

		public void SetVectorArray(int nameID, List<Vector4> values)
		{
			SetVectorArray(nameID, (Vector4[])ExtractArrayFromList(values));
		}

		public void SetVectorArray(string name, Vector4[] values)
		{
			SetVectorArray(Shader.PropertyToID(name), values);
		}

		public void SetVectorArray(int nameID, Vector4[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetVectorArrayImpl(nameID, values);
		}

		public void SetMatrixArray(string name, List<Matrix4x4> values)
		{
			SetMatrixArray(Shader.PropertyToID(name), values);
		}

		public void SetMatrixArray(int nameID, List<Matrix4x4> values)
		{
			SetMatrixArray(nameID, (Matrix4x4[])ExtractArrayFromList(values));
		}

		public void SetMatrixArray(string name, Matrix4x4[] values)
		{
			SetMatrixArray(Shader.PropertyToID(name), values);
		}

		public void SetMatrixArray(int nameID, Matrix4x4[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			SetMatrixArrayImpl(nameID, values);
		}

		public float GetFloat(string name)
		{
			return GetFloat(Shader.PropertyToID(name));
		}

		public float GetFloat(int nameID)
		{
			return GetFloatImpl(nameID);
		}

		public Vector4 GetVector(string name)
		{
			return GetVector(Shader.PropertyToID(name));
		}

		public Vector4 GetVector(int nameID)
		{
			return GetVectorImpl(nameID);
		}

		public Matrix4x4 GetMatrix(string name)
		{
			return GetMatrix(Shader.PropertyToID(name));
		}

		public Matrix4x4 GetMatrix(int nameID)
		{
			return GetMatrixImpl(nameID);
		}

		public void GetFloatArray(string name, List<float> values)
		{
			GetFloatArray(Shader.PropertyToID(name), values);
		}

		public void GetFloatArray(int nameID, List<float> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			GetFloatArrayImplList(nameID, values);
		}

		public float[] GetFloatArray(string name)
		{
			return GetFloatArray(Shader.PropertyToID(name));
		}

		public float[] GetFloatArray(int nameID)
		{
			return GetFloatArrayImpl(nameID);
		}

		public void GetVectorArray(string name, List<Vector4> values)
		{
			GetVectorArray(Shader.PropertyToID(name), values);
		}

		public void GetVectorArray(int nameID, List<Vector4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			GetVectorArrayImplList(nameID, values);
		}

		public Vector4[] GetVectorArray(string name)
		{
			return GetVectorArray(Shader.PropertyToID(name));
		}

		public Vector4[] GetVectorArray(int nameID)
		{
			return GetVectorArrayImpl(nameID);
		}

		public void GetMatrixArray(string name, List<Matrix4x4> values)
		{
			GetMatrixArray(Shader.PropertyToID(name), values);
		}

		public void GetMatrixArray(int nameID, List<Matrix4x4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			GetMatrixArrayImplList(nameID, values);
		}

		public Matrix4x4[] GetMatrixArray(string name)
		{
			return GetMatrixArray(Shader.PropertyToID(name));
		}

		public Matrix4x4[] GetMatrixArray(int nameID)
		{
			return GetMatrixArrayImpl(nameID);
		}

		public Texture GetTexture(string name)
		{
			return GetTexture(Shader.PropertyToID(name));
		}

		public Texture GetTexture(int nameID)
		{
			return GetTextureImpl(nameID);
		}

		[Obsolete("Use SetFloat instead (UnityUpgradable) -> SetFloat(*)", false)]
		public void AddFloat(string name, float value)
		{
			SetFloat(Shader.PropertyToID(name), value);
		}

		[Obsolete("Use SetFloat instead (UnityUpgradable) -> SetFloat(*)", false)]
		public void AddFloat(int nameID, float value)
		{
			SetFloat(nameID, value);
		}

		[Obsolete("Use SetVector instead (UnityUpgradable) -> SetVector(*)", false)]
		public void AddVector(string name, Vector4 value)
		{
			SetVector(Shader.PropertyToID(name), value);
		}

		[Obsolete("Use SetVector instead (UnityUpgradable) -> SetVector(*)", false)]
		public void AddVector(int nameID, Vector4 value)
		{
			SetVector(nameID, value);
		}

		[Obsolete("Use SetColor instead (UnityUpgradable) -> SetColor(*)", false)]
		public void AddColor(string name, Color value)
		{
			SetColor(Shader.PropertyToID(name), value);
		}

		[Obsolete("Use SetColor instead (UnityUpgradable) -> SetColor(*)", false)]
		public void AddColor(int nameID, Color value)
		{
			SetColor(nameID, value);
		}

		[Obsolete("Use SetMatrix instead (UnityUpgradable) -> SetMatrix(*)", false)]
		public void AddMatrix(string name, Matrix4x4 value)
		{
			SetMatrix(Shader.PropertyToID(name), value);
		}

		[Obsolete("Use SetMatrix instead (UnityUpgradable) -> SetMatrix(*)", false)]
		public void AddMatrix(int nameID, Matrix4x4 value)
		{
			SetMatrix(nameID, value);
		}

		[Obsolete("Use SetTexture instead (UnityUpgradable) -> SetTexture(*)", false)]
		public void AddTexture(string name, Texture value)
		{
			SetTexture(Shader.PropertyToID(name), value);
		}

		[Obsolete("Use SetTexture instead (UnityUpgradable) -> SetTexture(*)", false)]
		public void AddTexture(int nameID, Texture value)
		{
			SetTexture(nameID, value);
		}
	}
}
