using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;



#if UNITY_IPHONE
public enum VungleAdOrientation
{
	Portrait,
    LandscapeLeft,
    LandscapeRight,
    PortraitUpsideDown,
    Landscape,
    All,
    AllButUpsideDown
}

public class VungleBinding
{
	static VungleBinding()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			VungleManager.noop();
	}


	[DllImport("__Internal")]
	private static extern void _vungleStartWithAppId( string appId );

	// Starts up the SDK with the given appId
	public static void startWithAppId( string appId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			Debug.Log("START - startWithAppId");
			_vungleStartWithAppId( appId );
		}
	}


	[DllImport("__Internal")]
	private static extern void _vungleSetSoundEnabled( bool enabled );

	// Enables/disables sound
	public static void setSoundEnabled( bool enabled )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_vungleSetSoundEnabled( enabled );
	}


	[DllImport("__Internal")]
	private static extern void _vungleEnableLogging( bool shouldEnable );

	// Enables/disables verbose logging
	public static void enableLogging( bool shouldEnable )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_vungleEnableLogging( shouldEnable );
	}


	[DllImport("__Internal")]
	private static extern bool _vungleIsAdAvailable();

	// Checks to see if a video ad is available
	public static bool isAdAvailable()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _vungleIsAdAvailable();
		return false;
	}


	[DllImport("__Internal")]
	private static extern void _vunglePlayAdWithOptions( bool incentivized, int orientation, string user );

	// Plays an ad with the given options. The user option is only supported for incentivized ads.
	public static void playAd( bool incentivized = false, string user = "", VungleAdOrientation orientation = VungleAdOrientation.All )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_vunglePlayAdWithOptions( incentivized, (int)orientation, user );
	}
}
#endif
