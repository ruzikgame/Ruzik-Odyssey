using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using System;

namespace RuzikOdyssey.ViewModels
{
	public class StartSceneViewModel : ExtendedMonoBehaviour
	{
		public event EventHandler<ProgressUpdatedEventArgs> LoadingProgressUpdated;
		public event EventHandler<EventArgs> LoadingFinished;

		private void Awake()
		{
			SubscribeToEvents();
		}

		private void Start()
		{
			StartCoroutine(GlobalModel.InitializeAsync());
		}

		private void OnDestroy()
		{
			UnsubscribeFromEvents();
		}

		private void SubscribeToEvents()
		{
			EventsBroker.Subscribe<ProgressUpdatedEventArgs>(
				Events.Global.ModelLoadingProgressUpdated, 
				GameModel_LoadingProgressUpdate); 

			EventsBroker.Subscribe<EventArgs>(Events.Global.ModelLoadingFinished, GameModel_LoadingFinished);
		}

		private void UnsubscribeFromEvents()
		{
			EventsBroker.Unsubscribe<ProgressUpdatedEventArgs>(
				Events.Global.ModelLoadingProgressUpdated, 
				GameModel_LoadingProgressUpdate); 
			
			EventsBroker.Unsubscribe<EventArgs>(Events.Global.ModelLoadingFinished, GameModel_LoadingFinished);
		}

		private void OnLoadingProgressUpdated(ProgressUpdatedEventArgs e)
		{
			if (LoadingProgressUpdated != null) LoadingProgressUpdated(this, e);
		}

		private void OnLoadingFinished(EventArgs e)
		{
			if (LoadingFinished != null) LoadingFinished(this, EventArgs.Empty);
		}

		private void GameModel_LoadingProgressUpdate(object sender, ProgressUpdatedEventArgs e)
		{
			OnLoadingProgressUpdated(e);
		}

		private void GameModel_LoadingFinished(object sender, EventArgs e)
		{
			OnLoadingFinished(e);
		}
	}
}
