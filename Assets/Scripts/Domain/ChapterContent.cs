using System.Collections.Generic;

namespace RuzikOdyssey.Domain
{
	public class ChapterContent
	{
		public int Number { get; set; }
		public IList<LevelContent> Levels { get; set; }
	}
}
