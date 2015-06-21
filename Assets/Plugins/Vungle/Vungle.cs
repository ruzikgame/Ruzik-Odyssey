using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



#if UNITY_IPHONE || UNITY_ANDROID
public class Vungle
{
	#region Events

	// Fired when a Vungle ad starts
	public static event Action onAdStartedEvent;

	// Fired when a Vungle ad finishes
	public static event Action onAdEndedEvent;

	// Fired when a Vungle ad is cached and ready to be displayed
	public static event Action onCachedAdAvailableEvent;

	// Fired when a Vungle video is dismissed and provides the time watched and total duration in that order.
	public static event Action<double,double> onAdViewedEvent;



	static void adStarted()
	{
		if( onAdStartedEvent != null )
			onAdStartedEvent();
	}


	static void adFinished()
	{
		if( onAdEndedEvent != null )
			onAdEndedEvent();
	}


	static void videoViewed( double timeWatched, double totalDuration )
	{
		if( onAdViewedEvent != null )
			onAdViewedEvent( timeWatched, totalDuration );
	}


	static void vungleMoviePlayedEvent( Dictionary<string,object> data )
	{
		adFinished();

		var completedView = bool.Parse( data["completedView"].ToString() );
		var timeWatched = double.Parse( data["playTime"].ToString() );

		// we fake the totalDuration and make it accurate only as far as if they completed it or not for iOS
		var totalDuration = completedView ? timeWatched : timeWatched * 2;

		if( onAdViewedEvent != null )
			onAdViewedEvent( timeWatched, totalDuration );
	}


	static void onCachedAdAvailable()
	{
		if( onCachedAdAvailableEvent != null )
			onCachedAdAvailableEvent();
	}

	#endregion


	static Vungle()
	{
#if UNITY_IPHONE
		VungleManager.vungleSDKwillShowAdEvent += adStarted;
		VungleManager.vungleSDKwillCloseAdEvent += vungleMoviePlayedEvent;
		VungleManager.vungleSDKhasCachedAdAvailableEvent += onCachedAdAvailable;
#elif UNITY_ANDROID
		VungleAndroidManager.onAdStartEvent += adStarted;
		VungleAndroidManager.onAdEndEvent += adFinished;
		VungleAndroidManager.onVideoViewEvent += videoViewed;
		VungleAndroidManager.onCachedAdAvailableEvent += onCachedAdAvailable;
#endif
	}


	// Initializes the Vungle SDK. Pass in your Android and iOS app ID's from the Vungle web portal.
	public static void init( string androidAppId, string iosAppId )
	{
#if UNITY_IPHONE
		Debug.Log("START - UNITY_IPHONE - init");
		VungleBinding.startWithAppId( iosAppId );
#elif UNITY_ANDROID
		VungleAndroid.init( androidAppId );
#endif
	}


	// Sets if sound should be enabled or not
	public static void setSoundEnabled( bool isEnabled )
	{
#if UNITY_IPHONE
		VungleBinding.setSoundEnabled( isEnabled );
#elif UNITY_ANDROID
		VungleAndroid.setSoundEnabled( isEnabled );
#endif
	}


	// Checks to see if a video is available
	public static bool isAdvertAvailable()
	{
#if UNITY_IPHONE
		return VungleBinding.isAdAvailable();
#elif UNITY_ANDROID
		return VungleAndroid.isVideoAvailable();
#else
		return false;
#endif
	}


	// Displays an ad with the given options. The user option is only supported for incentivized ads.
	public static void playAd( bool incentivized = false, string user = "" )
	{
#if UNITY_IPHONE
		VungleBinding.playAd( incentivized, user, VungleAdOrientation.All );
#elif UNITY_ANDROID
		VungleAndroid.playAd( incentivized, user );
#endif
	}

}
#endif