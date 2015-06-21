using System;
using UnityEngine;
using GoogleMobileAds.Api;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.UI.Views
{
	public sealed class DashboardSceneView : ExtendedMonoBehaviour
	{	
#if UNITY_EDITOR
		private string adUnitId = "unused";
		private string videoAdId = "unused";
#elif UNITY_ANDROID
		private string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
		private string videoAdId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
#elif UNITY_IPHONE
		private string adUnitId = "ca-app-pub-1384659154698612/7265813883";
		private string videoAdId = "ca-app-pub-1384659154698612/1186655887";
#else
		private string adUnitId = "unexpected_platform";
		private string videoAdId = "unexpected_platform";
#endif

		private InterstitialAd interstitialAd;
		private InterstitialAd videoAd;

		public GameObject storePopup;

		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;
		public UILabel gasAmountLabel;

		public UILabel storeGoldAmountLabel;
		public UILabel storeCornAmountLabel;

		public UILabel currentLevelNameLabel;
		public UILabel currentLevelDifficultyLabel;

		public UITexture aircraftTexture;

		public GameObject videoAdButton;

		private bool showAds = true;

		private void Awake()
		{
			interstitialAd = new InterstitialAd(adUnitId);
			
			interstitialAd.AdLoaded += InterstitialAd_Loaded;
			interstitialAd.AdFailedToLoad += InterstitialAd_FailedToLoad;
			interstitialAd.AdOpened += InterstitialAd_Opened;
			interstitialAd.AdClosing += InterstitialAd_Closing;
			interstitialAd.AdClosed += InterstitialAd_Closed;
			interstitialAd.AdLeftApplication += InterstitialAd_LeftApplication;

			videoAd = new InterstitialAd(videoAdId);
			videoAd.AdLoaded += AdMob_VideoAdLoaded;
			
			SubscribeToEvent();

			/* TODO
			 * View should not have access to Global Model. 
			 * Create a ViewModel for this view and move Global Model access there.
			 */
			GlobalModel.Connect();
		}

		private void Start()
		{
			if (showAds)
			{
				interstitialAd.LoadAd(CreateAdRequest());
				videoAd.LoadAd(CreateVideoAdRequest());
			}

			InitializeUI();
		}

		private void OnDestroy()
		{
			UnsubscribeFromEvents();
		}

		private void InitializeUI()
		{
			goldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			cornAmountLabel.text = GlobalModel.Corn.Value.ToString();
			gasAmountLabel.text = String.Format("{0}/10", GlobalModel.Gas.Value);

			currentLevelNameLabel.text = GlobalModel.Progress.GetCurrentLevel().Name;

			if (aircraftTexture.mainTexture != null)
			{
				Resources.UnloadAsset(aircraftTexture.mainTexture);
			}

			var aircraftTexturePath = String.Format("Aircrafts/Aircraft_{0}", GlobalModel.Aircraft.Value.Ui.SceneSpriteName);
			aircraftTexture.mainTexture = Resources.Load(aircraftTexturePath) as Texture2D;
		}

		private void SubscribeToEvent()
		{
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GasPropertyChanged, Gas_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelIndexPropertyChanged, 
				CurrentLevelIndex_PropertyChanged);

			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelDifficultyPropertyChanged, 
				CurrentLevelDifficulty_PropertyChanged);
		}

		public void UnsubscribeFromEvents()
		{
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(Events.Global.GasPropertyChanged, Gas_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelIndexPropertyChanged, 
				CurrentLevelIndex_PropertyChanged);
			
			EventsBroker.Unsubscribe<PropertyChangedEventArgs<int>>(
				Events.Global.CurrentLevelDifficultyPropertyChanged, 
				CurrentLevelDifficulty_PropertyChanged);
		}

		private void Gold_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = e.PropertyValue.ToString();
		}

		private void Corn_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = e.PropertyValue.ToString();
		}

		private void Gas_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			gasAmountLabel.text = String.Format("{0}/10", e.PropertyValue);
		}

		private void CurrentLevelIndex_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			/* REMARK
			 * GlobalModel can be null when this event handler is invoked the first time 
			 * because the event is triggered when GlobalModel construction is not done yet.
			 * 
			 * The right solution is to use event arguments instead of accessing GlobalModel directly.
			 */
			if (GlobalModel == null) return;

			var currentLevelName = GlobalModel.Progress.GetCurrentLevel().Name;
			this.currentLevelNameLabel.text = currentLevelName;
		}

		private void CurrentLevelDifficulty_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			switch (e.PropertyValue)
			{
				case 0: 
					currentLevelDifficultyLabel.text = "Easy";
					break;
				case 1: 
					currentLevelDifficultyLabel.text = "Medium";
					break;
				case 2: 
					currentLevelDifficultyLabel.text = "Hard";
					break;
				default:
					Log.Error("Failed to determine level difficulty for value {0}", e.PropertyValue);
					currentLevelDifficultyLabel.text = "??????";
					break;
			}
		}

		public void StartMission()
		{
			#if UNITY_EDITOR

			LoadLevelScene();

			#endif

			if (interstitialAd.IsLoaded() && showAds) 
			{
				interstitialAd.Show();
			}
			else 
			{
				GameEnvironment.StartMission();

				LoadLevelScene();
			}
		}

		public void Game_OnStoreButtonClicked()
		{
			storeGoldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			storeCornAmountLabel.text = GlobalModel.Corn.Value.ToString();

			storePopup.SetActive(true);
		}

		public void IncreaseMissionDifficulty()
		{
			/* TODO
			 * This logic does not belong to the view. Move to ViewModel.
			 */
			if (GlobalModel.CurrentLevelDifficulty.Value < 2) GlobalModel.CurrentLevelDifficulty.Value++;
		}

		public void DecreaseMissionDifficulty()
		{
			/* TODO
			 * This logic does not belong to the view. Move to ViewModel.
			 */
			if (GlobalModel.CurrentLevelDifficulty.Value > 0) GlobalModel.CurrentLevelDifficulty.Value--;
		}

		public void Game_OnCloseStorePopupButtonClicked()
		{
			storePopup.SetActive(false);
		}

		private AdRequest CreateAdRequest()
		{
			return new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
				.AddKeyword("game")
				.SetGender(Gender.Male)
				.SetBirthday(new DateTime(1985, 1, 1))
				.TagForChildDirectedTreatment(false)
				.AddExtra("color_bg", "9B30FF")
				.Build();	
		}

		private AdRequest CreateVideoAdRequest()
		{
			return new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
				.AddKeyword("game")
				.SetGender(Gender.Female)
				.SetBirthday(new DateTime(1985, 1, 1))
				.TagForChildDirectedTreatment(false)
				.AddExtra("color_bg", "9B30FF")
				.Build();	
		}

		private void InterstitialAd_Loaded(object sender, EventArgs args)
		{
			Log.Debug("ADS - InterstitialAd_Loaded");
		}
		
		private void InterstitialAd_FailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Log.Debug("InterstitialAd_FailedToLoad event received with message: " + args.Message);
		}
		
		private void InterstitialAd_Opened(object sender, EventArgs args)
		{
			Log.Debug("ADS - InterstitialAd_Opened");
		}
		
		private void InterstitialAd_Closing(object sender, EventArgs args)
		{
			Log.Debug("ADS - InterstitialAd_Closing");
		}
		
		private void InterstitialAd_Closed(object sender, EventArgs args)
		{
			Log.Debug("ADS - InterstitialAd_Closed");

			GameEnvironment.StartMission();
			LoadLevelScene();
		}
		
		private void InterstitialAd_LeftApplication(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLeftApplication event received");
		}

		public void Game_ShowVideoAdButtonClicked()
		{
			if (!showAds) return;

			Log.Debug("START - Game_ShowVideoAdButtonClicked");

			if (videoAd.IsLoaded())
			{
				Log.Info("Playing video ad from AdMob.");
				videoAd.Show();
			}
			else 
			{
				Log.Warning("Failed to play video ad. Ad is not available.");
			}
		}

		private void AdMob_VideoAdLoaded(object sender, EventArgs e)
		{
			if (videoAdButton == null) 
			{
				Log.Warning("Itended to modify an unloaded element videoAdButton in DashboardSceneView");
				return;
			}

			videoAdButton.SetActive(true);
		}
	}
}