using UnityEngine;
using System.Collections;
using RuzikOdyssey.Common;

public class GameOverMenu : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.skin.button = GameEnvironment.DefaultButtonStyle;

		if (GUI.Button(new Rect(724 * GameEnvironment.ScaleOffset.x, 327 * GameEnvironment.ScaleOffset.y, 
		                        600 * GameEnvironment.Scale, 200 * GameEnvironment.Scale), 
		               "Restart"))
		{
			GameEnvironment.StartMission();
			Application.LoadLevel("default_level");  
		}

		if (GUI.Button(new Rect(724 * GameEnvironment.ScaleOffset.x, 627 * GameEnvironment.ScaleOffset.y, 
		                        600 * GameEnvironment.Scale, 200 * GameEnvironment.Scale), 
		               "Main menu"))
		{
			Application.LoadLevel("start_screen");  
		}
	}
}

