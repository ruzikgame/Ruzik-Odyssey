using UnityEngine;
using System.Collections;
using RuzikOdyssey.Common;

public class PauseMenu : MonoBehaviour
{	
	private void OnGUI()
	{
		GUI.skin.button = GameEnvironment.DefaultButtonStyle;

		if (GUI.Button(new Rect(724 * GameEnvironment.ScaleOffset.x, 202 * GameEnvironment.ScaleOffset.y, 
		                        600 * GameEnvironment.Scale, 200 * GameEnvironment.Scale), 
		               "Resume") )
		{
			GameEnvironment.Resume();
			Destroy(gameObject.GetComponent<PauseMenu>());
		}

		if (GUI.Button(new Rect(724 * GameEnvironment.ScaleOffset.x, 452 * GameEnvironment.ScaleOffset.y, 
		                        600 * GameEnvironment.Scale, 200 * GameEnvironment.Scale), 
		               "Restart") )
		{
			GameEnvironment.Resume();
			GameEnvironment.StartMission();
			Application.LoadLevel("default_level");  
		}
		
		if (GUI.Button(new Rect(724 * GameEnvironment.ScaleOffset.x, 702 * GameEnvironment.ScaleOffset.y, 
		                        600 * GameEnvironment.Scale, 200 * GameEnvironment.Scale), 
		               "Main menu") )
		{
			GameEnvironment.Resume();
			Application.LoadLevel("start_screen");  
		}
	}
}

