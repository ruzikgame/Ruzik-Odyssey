using UnityEngine;
using System.Collections;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System;
using RuzikOdyssey.Common;

public class TouchButton : MonoBehaviour, ITouchControl
{
	public string buttonName;

	private GUITexture buttonTexture;

	protected void Start()
	{
		buttonTexture = this.gameObject.GetComponent<GUITexture>();
		if (buttonTexture == null) throw new UnityException("Failed to find touch button texture");

		Rect initialPixelInset = buttonTexture.pixelInset;
		// Scale pixel inset recrangle
		buttonTexture.pixelInset = new Rect(initialPixelInset.x * GameEnvironment.ScaleOffset.x,
		                                    initialPixelInset.y * GameEnvironment.ScaleOffset.y,
		                                    initialPixelInset.width * GameEnvironment.ScaleOffset.x,
		                                    initialPixelInset.height * GameEnvironment.ScaleOffset.y);

		RegisterEvents();
	}

	protected virtual void RegisterEvents()
	{
		EventsBroker.Publish(String.Format("{0}_Touch", buttonName), ref Touch);
	}

	public event EventHandler<EventArgs> Touch;

	protected virtual void OnTouch()
	{
		Touch(this, EventArgs.Empty);
	}

	public void TriggerTouch()
	{
		OnTouch();
	}
	
	public bool HitTest(Vector2 position)
	{
		return buttonTexture.HitTest(position);
	}
}

