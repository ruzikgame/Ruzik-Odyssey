using UnityEngine;

namespace RuzikOdyssey.Common
{
	public static class BoundsExtensions
	{
		public static float Top(this Bounds bounds)
		{
			return bounds.max.y;
		}
		
		public static float Bottom(this Bounds bounds)
		{
			return bounds.min.y;
		}
		
		public static float Left(this Bounds bounds)
		{
			return bounds.min.x;
		}
		
		public static float Right(this Bounds bounds)
		{
			return bounds.max.x;
		}
	}
}
