using System.Collections.Generic;

namespace RuzikOdyssey.Domain
{
	public sealed class GameChapter
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public bool IsLocked { get; set; }

		public IList<GameLevel> Levels { get; set; }
	}
}