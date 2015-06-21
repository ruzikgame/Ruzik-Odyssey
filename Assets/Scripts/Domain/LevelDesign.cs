using System.Collections.Generic;

namespace RuzikOdyssey.Domain
{
	public sealed class LevelDesign
	{
		public IList<WaveTemplate> Enemies { get; set; }
		public IList<WaveTemplate> Obstacles { get; set; }
	}
}
