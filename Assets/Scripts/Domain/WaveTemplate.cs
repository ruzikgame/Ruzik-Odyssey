using System.Collections.Generic;

namespace RuzikOdyssey.Domain
{
	public class WaveTemplate
	{
		public IList<int> ObjectIndices { get; set; }
		public float NextWaveInterval { get; set; }

		public static WaveTemplate WaitForSeconds(float nextWaveInterval)
		{
			return new WaveTemplate
			{
				ObjectIndices = new List<int>(),
				NextWaveInterval = nextWaveInterval
			};
		}
	}
}
