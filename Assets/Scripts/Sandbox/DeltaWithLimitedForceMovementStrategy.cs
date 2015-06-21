using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using RuzikOdyssey.Common;

namespace Sandbox.RuzikOdyssey.Player
{
	public class DeltaWithLimitedForceMovementStrategy : IMovementStrategy
	{
		private const int maximumForce = 150;

		private Vector2 movement;
		private Bounds warzoneBounds;
		private Vector2 lastPosition;

		public DeltaWithLimitedForceMovementStrategy(Vector2 currentPosition)
		{
			lastPosition = currentPosition;
			movement = Vector2.zero;
			warzoneBounds = GameHelper.Instance.WarzoneBounds;
		}

		public void Move(Vector2 currentPosition, Vector2 inputPosition, Vector2 inputDelta)
		{
			movement += inputDelta;

			if ((currentPosition.x <= warzoneBounds.Left() && movement.x < 0) || 
			    (currentPosition.x >= warzoneBounds.Right() && movement.x > 0))
			{
				movement.x = 0;
			}
			if ((currentPosition.y <= warzoneBounds.Bottom() && movement.y < 0) || 
			    (currentPosition.y >= warzoneBounds.Top() && movement.y > 0))
			{
				movement.y = 0;
			}
		}

		public Vector2 GetAppliedForce(Vector2 currentPosition)
		{
			var force = movement;
			
			if (Math.Abs(force.x) > maximumForce) force.x = Math.Sign(force.x) * maximumForce;
			if (Math.Abs(force.y) > maximumForce) force.y = Math.Sign(force.y) * maximumForce;

			var deltaPosition =  currentPosition - lastPosition;
			
			movement.x = (Math.Abs(movement.x) - Math.Sign(movement.x) * deltaPosition.x > 0)
						? movement.x -= deltaPosition.x
						: 0;
			movement.y = (Math.Abs(movement.y) - Math.Sign(movement.y) * deltaPosition.y > 0)
						? movement.y -= deltaPosition.y
						: 0;
			
			lastPosition = currentPosition;

			return force;
		}

		public void Stop()
		{
			movement = Vector2.zero;
		}
	}
}
