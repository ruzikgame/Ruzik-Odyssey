using UnityEngine;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Domain.Store;
using RuzikOdyssey.Level;
using RuzikOdyssey.Models;
using RuzikOdyssey.ViewModels;

namespace RuzikOdyssey.Common
{
	public abstract class ExtendedMonoBehaviour : MonoBehaviour
	{
		protected GameHelper Game
		{
			get { return GameHelper.Instance; }
		}

		protected GameModel GlobalModel
		{
			get { return GameModel.Instance; }
		}

		protected void LoadScene(string sceneName)
		{
			Application.LoadLevel(sceneName);
		}

		public void LoadStartScene()
		{
			LoadScene(Scenes.Start);
		}

		public void LoadDashboardScene()
		{
			LoadScene(Scenes.Dashboard);
		}

		public void LoadSettingsScene()
		{
			LoadScene(Scenes.Settings);
		}

		public void LoadGlobalMapScene()
		{
			LoadScene(Scenes.GlobalMap);
		}

		public void LoadChapter1MapScene()
		{
			LoadScene(Scenes.Chapter1Map);
		}

		public void LoadLevelScene()
		{
			LoadScene(Scenes.LevelScene);
		}

		public void LoadAircraftStoreScene()
		{
			StoreSceneViewModel.StoreCategory = StoreItemCategory.Aircrafts;
			LoadStoreScene();
		}
		
		public void LoadGoldStoreScene()
		{
			StoreSceneViewModel.StoreCategory = StoreItemCategory.Gold;
			LoadStoreScene();
		}

		public void LoadCornStoreScene()
		{
			StoreSceneViewModel.StoreCategory = StoreItemCategory.Corn;
			LoadStoreScene();
		}

		public void LoadUpgradesStoreScene()
		{
			LoadScene(Scenes.SmallItemsStoreScene);
		}

		public void LoadHangarScene()
		{
			LoadScene(Scenes.HangarScene);
		}

		private void LoadStoreScene()
		{
			LoadScene(Scenes.StoreScene);
		}
	}
}
