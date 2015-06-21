using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{

	public interface IMovementStrategy
	{
		void Move(Vector2 currentPosition, Vector2 inputPosition, Vector2 inputDelta);
		Vector2 GetAppliedForce(Vector2 currentPosition);
		void Stop();
	}
	
}
