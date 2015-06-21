using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;
using RuzikOdyssey.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.Domain
{
	public sealed class GameProgress
	{
		public int Gold { get; set; }
		public int Corn { get; set; }
		public int Gas { get; set; }

		public int CurrentChapterIndex { get; set; }
		public int CurrentLevelIndex { get; set; }

		public int CurrentLevelDifficulty { get; set; }

		public IList<GameChapter> Chapters { get; set; }

		public override string ToString ()
		{
			return string.Format ("[GameProgress: Gold={0}, Corn={1}, Gas={2}, CurrentChapterIndex={3}, CurrentLevelIndex={4}, CurrentLevelDifficulty={5}, Chapters={6}]", Gold, Corn, Gas, CurrentChapterIndex, CurrentLevelIndex, CurrentLevelDifficulty, Chapters);
		}
	}

	public static class GameProgressExtensions
	{
		public static GameChapter GetCurrentChapter(this GameProgress progress)
		{
			return progress.CurrentChapterIndex < progress.Chapters.Count 
				? progress.Chapters[progress.CurrentChapterIndex] 
				: null; 
		}
		
		public static GameLevel GetCurrentLevel(this GameProgress progress)
		{
			var currentChapter = progress.GetCurrentChapter();
			
			if (currentChapter == null) return null;
			if (progress.CurrentLevelIndex >= currentChapter.Levels.Count) return null;
			
			return currentChapter.Levels[progress.CurrentLevelIndex]; 
		}
	}
}