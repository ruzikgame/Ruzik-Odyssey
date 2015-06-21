using UnityEngine;

namespace RuzikOdyssey.Common
{
	public class Range
	{
		public float Min { get; set; }
		public float Max { get; set; }
		
		public float GetNumberInRange()
		{
			return Random.Range(Min, Max);
		}
	}
}