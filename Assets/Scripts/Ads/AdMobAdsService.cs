using UnityEngine;
using GoogleMobileAds.Api;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Ads
{
	public sealed class AdMobAdsService : MonoBehaviour
	{
		private const string UnusedId = "unused";

		private static readonly AdMobAdsService instance = new AdMobAdsService();
		public static AdMobAdsService Instance { get { return instance; } }

		private InterstitialAd interstitialAd;
		private InterstitialAd videoAd;
		
		static AdMobAdsService() {}
		private AdMobAdsService() 
		{
#if UNITY_EDITOR
			var adUnitId = UnusedId;
			var videoAdId = UnusedId;
#elif UNITY_ANDROID
			var adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
#elif UNITY_IPHONE
			var adUnitId = "ca-app-pub-1384659154698612/7265813883";
			var videoAdId = "ca-app-pub-1384659154698612/1186655887";
#else
			var adUnitId = "unexpected_platform";
#endif

			var vungleIosAppId = "com.cocosgames.RuzikOdyssey";
			var vungleAndroidAppId = "com.cocosgames.RuzikOdyssey";
			
			interstitialAd = new InterstitialAd(adUnitId);
			videoAd = new InterstitialAd(videoAdId);

			interstitialAd.AdLoaded += InterstitialAd_Loaded;
			interstitialAd.AdFailedToLoad += InterstitialAd_FailedToLoad;
			interstitialAd.AdOpened += InterstitialAd_Opened;
			interstitialAd.AdClosing += InterstitialAd_Closing;
			interstitialAd.AdClosed += InterstitialAd_Closed;
			interstitialAd.AdLeftApplication += InterstitialAd_LeftApplication;

			RequestInterstitialAd();
			RequestVideoAd();

			Vungle.init(vungleAndroidAppId, vungleIosAppId);
			Vungle.onAdEndedEvent += Vungle_OnAdEndedEventHandler;
			Vungle.onAdStartedEvent += Vungle_OnAdStartedEventHandler;
			Vungle.onAdViewedEvent += Vungle_OnViewedEventHandler;
			Vungle.onCachedAdAvailableEvent += Vungle_OnCachedAdAvailableEventHandler;
		}

		public bool InterstitialAdIsReady()
		{
			return interstitialAd.IsLoaded();
		}

		public bool VideoAdIsReady()
		{
			return Vungle.isAdvertAvailable() || videoAd.IsLoaded();
		}

		private void RequestInterstitialAd()
		{
			interstitialAd.LoadAd(CreateAdRequest());
		}

		private void RequestVideoAd()
		{
			videoAd.LoadAd(CreateVideoAdRequest());
		}

		private AdRequest CreateAdRequest()
		{
			return new AdRequest.Builder()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
					.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
					.AddKeyword("game")
					.SetGender(Gender.Male)
					.SetBirthday(new DateTime(1995, 1, 1))
					.TagForChildDirectedTreatment(true)
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
					.SetBirthday(new DateTime(1995, 1, 1))
					.TagForChildDirectedTreatment(true)
					.AddExtra("color_bg", "9B30FF")
					.Build();	
		}
		
		public bool ShowInterstitialAd()
		{
			if (interstitialAd.IsLoaded())
			{
				interstitialAd.Show();
				return true;
			}
			else
			{
				Log.Warning("Interstitial is not ready yet.");
				return false;
			}
		}

		public void HideInterstitialAd()
		{
			interstitialAd.Destroy();
			RequestInterstitialAd();
		}

		private void InterstitialAd_Loaded(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLoaded event received.");
		}
		
		private void InterstitialAd_FailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Log.Debug("HandleInterstitialFailedToLoad event received with message: " + args.Message);
		}
		
		private void InterstitialAd_Opened(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialOpened event received");
		}
		
		private void InterstitialAd_Closing(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialClosing event received");
		}
		
		private void InterstitialAd_Closed(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialClosed event received");
		}
		
		private void InterstitialAd_LeftApplication(object sender, EventArgs args)
		{
			Log.Debug("HandleInterstitialLeftApplication event received");
		}

		private void Vungle_OnAdEndedEventHandler()
		{

		}

		private void Vungle_OnAdStartedEventHandler()
		{

		}

		private void Vungle_OnViewedEventHandler(double timeWatched, double totalDuration)
		{

		}

		private void Vungle_OnCachedAdAvailableEventHandler()
		{

		}

	}
}
