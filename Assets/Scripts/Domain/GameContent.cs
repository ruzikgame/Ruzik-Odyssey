using System.Collections.Generic;

namespace RuzikOdyssey.Domain
{
	public class GameContent
	{
		public string Version { get; set; }
		public IList<ChapterContent> Chapters { get; set; }
	}
	
}
