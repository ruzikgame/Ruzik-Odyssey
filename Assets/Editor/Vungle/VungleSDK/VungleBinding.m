//
//  VungleBinding.m
//  VungleTest
//
//  Created by Mike Desaro on 6/5/12.
//  Copyright (c) 2012 prime31. All rights reserved.
//
#import <VungleSDK/VungleSDK.h>
#import "VungleManager.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil




void _vungleStartWithAppId( const char * appId )
{
	if( [[VungleSDK sharedSDK] respondsToSelector:@selector(setPluginName:version:)] )
		[[VungleSDK sharedSDK] performSelector:@selector(setPluginName:version:) withObject:@"unity" withObject:@"2.0"];
	
	[[VungleManager sharedManager] startWithAppId:GetStringParam( appId )];
}


void _vungleSetSoundEnabled( BOOL enabled )
{
	[VungleSDK sharedSDK].muted = !enabled;
}


void _vungleEnableLogging( BOOL shouldEnable )
{
	[[VungleSDK sharedSDK] setLoggingEnabled:shouldEnable];
}


BOOL _vungleIsAdAvailable()
{
	return [[VungleSDK sharedSDK] isCachedAdAvailable];
}


void _vunglePlayAdWithOptions( BOOL incentivized, int orientation, const char * user )
{
	UIInterfaceOrientationMask orientationMask;
	switch( orientation )
	{
		case 0:
			orientationMask = UIInterfaceOrientationPortrait;
			break;
		case 1:
			orientationMask = UIInterfaceOrientationLandscapeLeft;
			break;
		case 2:
			orientationMask = UIInterfaceOrientationLandscapeRight;
			break;
		case 3:
			orientationMask = UIInterfaceOrientationPortraitUpsideDown;
			break;
		case 4:
			orientationMask = UIInterfaceOrientationMaskLandscape;
			break;
		case 5:
			orientationMask = UIInterfaceOrientationMaskAll;
			break;
		case 6:
			orientationMask = UIInterfaceOrientationMaskAllButUpsideDown;
			break;
	}
	
	NSMutableDictionary *options = [NSMutableDictionary dictionary];
	[options setObject:@(incentivized) forKey:VunglePlayAdOptionKeyIncentivized];
	[options setObject:@(orientationMask) forKey:VunglePlayAdOptionKeyOrientations];
	
	NSString *userString = GetStringParamOrNil( user );
	if( userString )
		[options setObject:userString forKey:VunglePlayAdOptionKeyUser];
	
	[[VungleManager sharedManager] playAdWithOptions:options];
}

