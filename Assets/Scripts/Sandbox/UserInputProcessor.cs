using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Level;

namespace Sandbox.RuzikOdyssey.Player
{

	public sealed class UserInputProcessor : MonoBehaviour
	{
		private IList<ITouchControl> touchControls;
		
		private int movementTouchId;

		public event EventHandler<InputChangedEventArgs> InputChanged;
		public event EventHandler<EventArgs> NoInput;

		private void OnInputChanged(Touch touch)
		{
			InputChanged(this, new InputChangedEventArgs { Position = touch.position, Delta = touch.deltaPosition });
		}

		private void OnNoInput()
		{
			NoInput(this, EventArgs.Empty);
		}

		private void RegisterEvents()
		{
			EventsBroker.Publish(Events.Input.InputChanged, ref InputChanged);
			EventsBroker.Publish(Events.Input.NoInput, ref NoInput);
		}

		private void Awake()
		{
			touchControls = GameObject.FindGameObjectsWithTag("TouchControl")
									  .Select(x => (ITouchControl)x.GetComponent(typeof(ITouchControl)))
									  .ToList();

			movementTouchId = -1;
		}
		
		private void Update() 
		{	
			foreach (var touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					var touchedControl = touchControls.FirstOrDefault(control => control.HitTest(touch.position));
					if (touchedControl != null) 
					{
						touchedControl.TriggerTouch();
						continue;
					}
				}

				if (touch.fingerId == movementTouchId)
				{
					if (touch.phase == TouchPhase.Moved)
					{
						OnInputChanged(touch);
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						movementTouchId = -1;
						OnNoInput();
					}
				}
				else if (movementTouchId < 0 && touch.phase == TouchPhase.Began)
				{
					movementTouchId = touch.fingerId;
					OnInputChanged(touch);
				}
			}
		}
	}

}
