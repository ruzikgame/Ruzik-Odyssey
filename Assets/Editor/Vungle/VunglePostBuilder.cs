using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using System.Diagnostics;


public class VunglePostBuilder : MonoBehaviour
{
	[PostProcessBuild]
	private static void onPostProcessBuildPlayer( BuildTarget target, string pathToBuiltProject )
	{
		if( target == BuildTarget.Android )
		{
			if( !File.Exists( Path.Combine( Application.dataPath, "Plugins/Android/AndroidManifest.xml" ) ) )
			{
				UnityEngine.Debug.Log( "Could not find an AndroidManifest.xml file. Generating one now. You will need to rebuild your project for the changes to take affect." );
				generateAndroidManifest();
			}
		}
		else if( target == BuildTarget.iOS )
		{
			// grab the path to the postProcessor.py file
			var scriptPath = Path.Combine( Application.dataPath, "Editor/Vungle/VunglePostProcessor.py" );

			// sanity check
			if( !File.Exists( scriptPath ) )
			{
				UnityEngine.Debug.LogError( "Vungle post builder could not find the VunglePostProcessor.py file. Did you accidentally delete it?" );
				return;
			}

			var pathToNativeCodeFiles = Path.Combine( Application.dataPath, "Editor/Vungle/VungleSDK" );

			var args = string.Format( "\"{0}\" \"{1}\" \"{2}\"", scriptPath, pathToBuiltProject, pathToNativeCodeFiles );
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "python2.6",
					Arguments = args,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			proc.Start();
		}
	}


	[UnityEditor.MenuItem( "Tools/Vungle/Generate AndroidManifest.xml file" )]
	static void generateAndroidManifestMenuItem()
	{
		generateAndroidManifest();
	}


	private static void generateAndroidManifest()
	{
		var baseManifestPath = Path.Combine( Application.dataPath, "Editor/Vungle/BaseAndroidManifest.xml" );
		if( !File.Exists( baseManifestPath ) )
		{
			EditorUtility.DisplayDialog( "Vungle Error", "The BaseAndroidManifest.xml file could not be found. Please reimport the plugin and try again", "OK" );
			return;
		}

		var androidManifestPath = Path.Combine( Application.dataPath, "Plugins/Android/AndroidManifest.xml" );
		if( File.Exists( androidManifestPath ) )
		{
			if( !EditorUtility.DisplayDialog( "Vungle", "There is already an AndroidManifest.xml present. Should we overwrite the file with a fresh AndroidManifest.xml file (any changes you made to it will be lost)?", "Overwrite It", "Abort" ) )
				return;

			File.Delete( androidManifestPath );
		}

		var fileContents = File.ReadAllText( baseManifestPath );

		// the only thing we need in there is the package so a simple replace will suffice
		fileContents = fileContents.Replace( "CURRENT_PACKAGE_NAME", PlayerSettings.bundleIdentifier );

		File.WriteAllText( androidManifestPath, fileContents );
		AssetDatabase.Refresh();
	}

}
