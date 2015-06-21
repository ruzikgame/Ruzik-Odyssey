using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using RuzikOdyssey.Player;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey
{
	public class GameInputController : MonoBehaviour 
	{
//		private IList<ITouchControl> buttons;
		public RuzikController playerController;

		private int movementTouchId = -1;

		private void Start()
		{
//			buttons = GameObject.FindGameObjectsWithTag("TouchControl")
//				.Select(x => (ITouchControl)x.GetComponent(typeof(ITouchControl)))
//				.ToList();
//
//			Debug.Log("Found touch controls: " + buttons.Count);
//
//			playerController = GameObject.FindGameObjectWithTag("Player")
//				.GetComponent<RuzikController>();
//			if (playerController == null) throw new UnityException("Failed to initialize player controller");
//
//			movementTouchId = -1;
		}

		private void Update() 
		{	
			if (Input.touchCount > 2 && Input.touchCount <= 0) 
			{
				playerController.Stop();
				return;
			}

			foreach (var touch in Input.touches)
			{
				var isButtonTouch = false;
//				foreach (var button in buttons)
//				{
//					if (button.HitTest(touch.position)) 
//					{
//						isButtonTouch = true;
//						if (touch.phase == TouchPhase.Began)
//						{
//							button.TriggerTouch();
//						}
//						else if (touch.fingerId == movementTouchId) ProcessMovementTouch(touch);
//					}
//				}

				if (isButtonTouch && Input.touchCount == 1) playerController.Stop();
				if (isButtonTouch || GameEnvironment.IsGameOver) continue;


				if (movementTouchId < 0 && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
				{
					movementTouchId = touch.fingerId;
					playerController.Stop();
					playerController.Move(touch.position, touch.deltaPosition);
				}
				else if (touch.fingerId == movementTouchId) ProcessMovementTouch(touch);
		    }
		}

		private void ProcessMovementTouch(Touch touch)
		{
			if (GameEnvironment.IsGameOver) return;

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
			{
				playerController.AttackWithMainWeapon();
				playerController.Move(touch.position, touch.deltaPosition);
			}
			else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				movementTouchId = -1;
				playerController.Stop();
			}
		}
	}
}
