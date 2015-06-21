using UnityEngine;

namespace RuzikOdyssey.Common
{
	public static class OtherExtensions
	{
		public static Vector2 RandomNormilazed(this Vector2 vector)
		{
			return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
		}
	}
}
