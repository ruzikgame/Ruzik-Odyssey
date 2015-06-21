using UnityEngine;
using System.Collections;
using RuzikOdyssey.Common;

public class PauseButton : TouchButton 
{
	private GameObject ui;
	
	private new void Start()
	{
		base.Start();

		ui = GameObject.Find("UI");
		if (ui == null)
			throw new UnityException("Failed to find game object named 'ui' in the hierarchy");
	}
	
	protected override void OnTouch()
	{
		if (!GameEnvironment.IsPaused) 
		{
			ui.AddComponent<PauseMenu>();
			GameEnvironment.Pause();
		}
	}

}
