using System;
using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.UI.Elements;

namespace RuzikOdyssey.UI.Views
{
	public sealed class Chapter1MapSceneView : ExtendedMonoBehaviour
	{	
		public GameObject levelDescriptionPopup;

		public MapLocation[] levels;

		private void Awake()
		{
			GlobalModel.Connect();
		}
		
		private void Start()
		{
			var gameProgress = GlobalModel.Progress;

			var chapter = GlobalModel.Progress.Chapters[0];

			for (int i = 0; i < chapter.Levels.Count; i++)
			{
				var level = chapter.Levels[i];

				levels[i].LocationMedalsLabel.text = String.Format("{0}/{1}", level.Medals, level.MaxMedals);
				levels[i].LocationPointer.SetActive(!level.IsLocked);
				levels[i].LocationLock.SetActive(level.IsLocked);
				levels[i].CurrentLocationArrow.SetActive(gameProgress.CurrentLevelIndex == i);
			}
		}

		public void SelectLevel(int index)
		{
			if (GlobalModel.Progress.GetCurrentChapter().Levels[index].IsLocked) return;

			GlobalModel.CurrentLevelIndex.Value = index;
			GlobalModel.Save();

			LoadDashboardScene();
		}

		public void ShowLevelDescriptionPopup()
		{
			levelDescriptionPopup.SetActive(true);
		}

		public void HideLevelDescriptionPopup()
		{
			levelDescriptionPopup.SetActive(false);
		}
	}
}

