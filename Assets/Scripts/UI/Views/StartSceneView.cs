using System;
using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.ViewModels;

namespace RuzikOdyssey.UI.Views
{
	public sealed class StartSceneView : ExtendedMonoBehaviour
	{
		private const double BackgroundDetailsScreenRatioThreshold = 1.45;
		private const int VerticalScreenAdjustment = -30;

		public UIWidget screenContainer;
		public GameObject backgroundDetails;
		public GameObject progressInfoContainer;
		public UILabel actionProgressLabel;
		public UIButton startButton;
		public UIButton optionsButton;

		public StartSceneViewModel viewModel;

		private void Awake()
		{
			viewModel.LoadingProgressUpdated += ViewModel_LoadingProgressUpdated;
			viewModel.LoadingFinished += ViewModel_LoadingFinished;
		}

		private void Start()
		{
			if ((double) Screen.width / Screen.height > BackgroundDetailsScreenRatioThreshold) 
			{
				backgroundDetails.SetActive(false);

				screenContainer.topAnchor.absolute = VerticalScreenAdjustment;
				screenContainer.bottomAnchor.absolute = VerticalScreenAdjustment;

				screenContainer.ResetAndUpdateAnchors();
			}

			// Disable UI until view model has finished loading
			SetIsUiEnabled(false);
			progressInfoContainer.SetActive(true);
		}

		private void ViewModel_LoadingProgressUpdated(object sender, ProgressUpdatedEventArgs e)
		{
			actionProgressLabel.text = String.Format("{0}... {1}%", e.ActionName, e.ActionProgress);
		}

		private void ViewModel_LoadingFinished(object sender, EventArgs e)
		{
			SetIsUiEnabled(true);
			progressInfoContainer.SetActive(false);

			Log.Info("Unloading unused assets");

			Resources.UnloadUnusedAssets();
		}

		private void SetIsUiEnabled(bool isUiEnabled)
		{
			startButton.isEnabled = isUiEnabled;
			optionsButton.isEnabled = isUiEnabled;
		}
	}
}
