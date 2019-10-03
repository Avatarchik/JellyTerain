using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class LocationService
	{
		public bool isEnabledByUser
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public LocationServiceStatus status
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public LocationInfo lastData
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Start([DefaultValue("10f")] float desiredAccuracyInMeters, [DefaultValue("10f")] float updateDistanceInMeters);

		[ExcludeFromDocs]
		public void Start(float desiredAccuracyInMeters)
		{
			float updateDistanceInMeters = 10f;
			Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[ExcludeFromDocs]
		public void Start()
		{
			float updateDistanceInMeters = 10f;
			float desiredAccuracyInMeters = 10f;
			Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Stop();
	}
}
