using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.UI.Views
{
	public sealed class GlobalMapSceneView : ExtendedMonoBehaviour
	{
		/* TODO
		 * Use MapLocation class to specify chapter parameters 
		 * and to bind all location elements.
		 */
		public GameObject[] chapters;
		public GameObject[] currentLocationArrows;
		public GameObject[] locationLocks;
		public GameObject[] locationMedals;
		public UILabel[] locationMedalsLabels;

		private void Awake()
		{
			GlobalModel.Connect();
		}

		private void Start()
		{
			var gameProgress = GlobalModel.Progress;

			for (int i = 0; i < gameProgress.Chapters.Count; i++)
			{
				var chapter = gameProgress.Chapters[i];

				var medals = chapter.Levels.XSum(x => x.Medals);
				var maxMedals = chapter.Levels.XSum(x => x.MaxMedals);

				locationMedals[i].SetActive(!chapter.IsLocked);
				locationMedalsLabels[i].text = String.Format("{0}/{1}", medals, maxMedals);
				locationLocks[i].SetActive(chapter.IsLocked);
				currentLocationArrows[i].SetActive(gameProgress.CurrentChapterIndex == i);
			}
		}
	}

}

