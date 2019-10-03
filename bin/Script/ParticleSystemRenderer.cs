using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class ParticleSystemRenderer : Renderer
	{
		public ParticleSystemRenderMode renderMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float lengthScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float velocityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float cameraVelocityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float normalDirection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public ParticleSystemRenderSpace alignment
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector3 pivot
		{
			get
			{
				INTERNAL_get_pivot(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_pivot(ref value);
			}
		}

		public ParticleSystemSortMode sortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float sortingFudge
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float minParticleSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float maxParticleSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Mesh mesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int meshCount => Internal_GetMeshCount();

		public Material trailMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int activeVertexStreamsCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_pivot(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_pivot(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int Internal_GetMeshCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetMeshes(Mesh[] meshes);

		public void SetMeshes(Mesh[] meshes)
		{
			SetMeshes(meshes, meshes.Length);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetMeshes(Mesh[] meshes, int size);

		public void SetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
		{
			if (streams == null)
			{
				throw new ArgumentNullException("streams");
			}
			SetActiveVertexStreamsInternal(streams);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void SetActiveVertexStreamsInternal(object streams);

		public void GetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
		{
			if (streams == null)
			{
				throw new ArgumentNullException("streams");
			}
			GetActiveVertexStreamsInternal(streams);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetActiveVertexStreamsInternal(object streams);

		[Obsolete("EnableVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
		public void EnableVertexStreams(ParticleSystemVertexStreams streams)
		{
			Internal_SetVertexStreams(streams, enabled: true);
		}

		[Obsolete("DisableVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
		public void DisableVertexStreams(ParticleSystemVertexStreams streams)
		{
			Internal_SetVertexStreams(streams, enabled: false);
		}

		[Obsolete("AreVertexStreamsEnabled is deprecated. Use GetActiveVertexStreams instead.")]
		public bool AreVertexStreamsEnabled(ParticleSystemVertexStreams streams)
		{
			return Internal_GetEnabledVertexStreams(streams) == streams;
		}

		[Obsolete("GetEnabledVertexStreams is deprecated. Use GetActiveVertexStreams instead.")]
		public ParticleSystemVertexStreams GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
		{
			return Internal_GetEnabledVertexStreams(streams);
		}

		[Obsolete("Internal_SetVertexStreams is deprecated. Use SetActiveVertexStreams instead.")]
		internal void Internal_SetVertexStreams(ParticleSystemVertexStreams streams, bool enabled)
		{
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(activeVertexStreamsCount);
			GetActiveVertexStreams(list);
			if (enabled)
			{
				if ((streams & ParticleSystemVertexStreams.Position) != 0 && !list.Contains(ParticleSystemVertexStream.Position))
				{
					list.Add(ParticleSystemVertexStream.Position);
				}
				if ((streams & ParticleSystemVertexStreams.Normal) != 0 && !list.Contains(ParticleSystemVertexStream.Normal))
				{
					list.Add(ParticleSystemVertexStream.Normal);
				}
				if ((streams & ParticleSystemVertexStreams.Tangent) != 0 && !list.Contains(ParticleSystemVertexStream.Tangent))
				{
					list.Add(ParticleSystemVertexStream.Tangent);
				}
				if ((streams & ParticleSystemVertexStreams.Color) != 0 && !list.Contains(ParticleSystemVertexStream.Color))
				{
					list.Add(ParticleSystemVertexStream.Color);
				}
				if ((streams & ParticleSystemVertexStreams.UV) != 0 && !list.Contains(ParticleSystemVertexStream.UV))
				{
					list.Add(ParticleSystemVertexStream.UV);
				}
				if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != 0 && !list.Contains(ParticleSystemVertexStream.UV2))
				{
					list.Add(ParticleSystemVertexStream.UV2);
					list.Add(ParticleSystemVertexStream.AnimBlend);
					list.Add(ParticleSystemVertexStream.AnimFrame);
				}
				if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != 0 && !list.Contains(ParticleSystemVertexStream.Center))
				{
					list.Add(ParticleSystemVertexStream.Center);
					list.Add(ParticleSystemVertexStream.VertexID);
				}
				if ((streams & ParticleSystemVertexStreams.Size) != 0 && !list.Contains(ParticleSystemVertexStream.SizeXYZ))
				{
					list.Add(ParticleSystemVertexStream.SizeXYZ);
				}
				if ((streams & ParticleSystemVertexStreams.Rotation) != 0 && !list.Contains(ParticleSystemVertexStream.Rotation3D))
				{
					list.Add(ParticleSystemVertexStream.Rotation3D);
				}
				if ((streams & ParticleSystemVertexStreams.Velocity) != 0 && !list.Contains(ParticleSystemVertexStream.Velocity))
				{
					list.Add(ParticleSystemVertexStream.Velocity);
				}
				if ((streams & ParticleSystemVertexStreams.Lifetime) != 0 && !list.Contains(ParticleSystemVertexStream.AgePercent))
				{
					list.Add(ParticleSystemVertexStream.AgePercent);
					list.Add(ParticleSystemVertexStream.InvStartLifetime);
				}
				if ((streams & ParticleSystemVertexStreams.Custom1) != 0 && !list.Contains(ParticleSystemVertexStream.Custom1XYZW))
				{
					list.Add(ParticleSystemVertexStream.Custom1XYZW);
				}
				if ((streams & ParticleSystemVertexStreams.Custom2) != 0 && !list.Contains(ParticleSystemVertexStream.Custom2XYZW))
				{
					list.Add(ParticleSystemVertexStream.Custom2XYZW);
				}
				if ((streams & ParticleSystemVertexStreams.Random) != 0 && !list.Contains(ParticleSystemVertexStream.StableRandomXYZ))
				{
					list.Add(ParticleSystemVertexStream.StableRandomXYZ);
					list.Add(ParticleSystemVertexStream.VaryingRandomX);
				}
			}
			else
			{
				if ((streams & ParticleSystemVertexStreams.Position) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Position);
				}
				if ((streams & ParticleSystemVertexStreams.Normal) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Normal);
				}
				if ((streams & ParticleSystemVertexStreams.Tangent) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Tangent);
				}
				if ((streams & ParticleSystemVertexStreams.Color) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Color);
				}
				if ((streams & ParticleSystemVertexStreams.UV) != 0)
				{
					list.Remove(ParticleSystemVertexStream.UV);
				}
				if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != 0)
				{
					list.Remove(ParticleSystemVertexStream.UV2);
					list.Remove(ParticleSystemVertexStream.AnimBlend);
					list.Remove(ParticleSystemVertexStream.AnimFrame);
				}
				if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Center);
					list.Remove(ParticleSystemVertexStream.VertexID);
				}
				if ((streams & ParticleSystemVertexStreams.Size) != 0)
				{
					list.Remove(ParticleSystemVertexStream.SizeXYZ);
				}
				if ((streams & ParticleSystemVertexStreams.Rotation) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Rotation3D);
				}
				if ((streams & ParticleSystemVertexStreams.Velocity) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Velocity);
				}
				if ((streams & ParticleSystemVertexStreams.Lifetime) != 0)
				{
					list.Remove(ParticleSystemVertexStream.AgePercent);
					list.Remove(ParticleSystemVertexStream.InvStartLifetime);
				}
				if ((streams & ParticleSystemVertexStreams.Custom1) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Custom1XYZW);
				}
				if ((streams & ParticleSystemVertexStreams.Custom2) != 0)
				{
					list.Remove(ParticleSystemVertexStream.Custom2XYZW);
				}
				if ((streams & ParticleSystemVertexStreams.Random) != 0)
				{
					list.Remove(ParticleSystemVertexStream.StableRandomXYZW);
					list.Remove(ParticleSystemVertexStream.VaryingRandomX);
				}
			}
			SetActiveVertexStreams(list);
		}

		[Obsolete("Internal_GetVertexStreams is deprecated. Use GetActiveVertexStreams instead.")]
		internal ParticleSystemVertexStreams Internal_GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
		{
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(activeVertexStreamsCount);
			GetActiveVertexStreams(list);
			ParticleSystemVertexStreams particleSystemVertexStreams = ParticleSystemVertexStreams.None;
			if (list.Contains(ParticleSystemVertexStream.Position))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Position;
			}
			if (list.Contains(ParticleSystemVertexStream.Normal))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Normal;
			}
			if (list.Contains(ParticleSystemVertexStream.Tangent))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Tangent;
			}
			if (list.Contains(ParticleSystemVertexStream.Color))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Color;
			}
			if (list.Contains(ParticleSystemVertexStream.UV))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.UV;
			}
			if (list.Contains(ParticleSystemVertexStream.UV2))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.UV2BlendAndFrame;
			}
			if (list.Contains(ParticleSystemVertexStream.Center))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.CenterAndVertexID;
			}
			if (list.Contains(ParticleSystemVertexStream.SizeXYZ))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Size;
			}
			if (list.Contains(ParticleSystemVertexStream.Rotation3D))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Rotation;
			}
			if (list.Contains(ParticleSystemVertexStream.Velocity))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Velocity;
			}
			if (list.Contains(ParticleSystemVertexStream.AgePercent))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Lifetime;
			}
			if (list.Contains(ParticleSystemVertexStream.Custom1XYZW))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom1;
			}
			if (list.Contains(ParticleSystemVertexStream.Custom2XYZW))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom2;
			}
			if (list.Contains(ParticleSystemVertexStream.StableRandomXYZ))
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Random;
			}
			return particleSystemVertexStreams & streams;
		}
	}
}
