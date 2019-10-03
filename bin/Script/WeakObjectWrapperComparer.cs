using System.Collections.Generic;

namespace System.ComponentModel
{
	internal sealed class WeakObjectWrapperComparer : EqualityComparer<WeakObjectWrapper>
	{
		public override bool Equals(WeakObjectWrapper x, WeakObjectWrapper y)
		{
			if (x == null && y == null)
			{
				return false;
			}
			if (x == null || y == null)
			{
				return false;
			}
			WeakReference weak = x.Weak;
			WeakReference weak2 = y.Weak;
			if (!weak.IsAlive && !weak2.IsAlive)
			{
				return false;
			}
			return weak.Target == weak2.Target;
		}

		public override int GetHashCode(WeakObjectWrapper obj)
		{
			return obj?.TargetHashCode ?? 0;
		}
	}
}
