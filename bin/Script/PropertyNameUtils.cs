using System.Runtime.CompilerServices;
using Unity.Bindings;

namespace UnityEngine
{
	internal class PropertyNameUtils
	{
		public static PropertyName PropertyNameFromString([NativeParameter(Unmarshalled = true)] string name)
		{
			PropertyNameFromString_Injected(name, out PropertyName ret);
			return ret;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PropertyNameFromString_Injected(string name, out PropertyName ret);
	}
}
