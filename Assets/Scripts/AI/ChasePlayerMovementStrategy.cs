using UnityEngine;
using RuzikOdyssey.Level;
using System;

namespace RuzikOdyssey.Ai
{
	public class ChasePlayerMovementStrategy : MovementStrategy
	{
		private float targetXPosition;
		
		private float allowedTargetPositionDistanceError = 0.1f;
		
		private void Start()
		{
			targetXPosition = GetRandomXPositionWithinWarzone();
		}
		
		protected override Vector2 CalculateMovementDirection(Vector2 currentPosition, Vector2 playerPosition, float deltaTime)
		{
			if (Math.Abs(targetXPosition - currentPosition.x) <= allowedTargetPositionDistanceError)
			{
				targetXPosition = GetRandomXPositionWithinWarzone();
			}

			var targetPosition = new Vector2(targetXPosition, playerPosition.y);

			return (targetPosition - currentPosition).normalized;
		}
		
		private float GetRandomXPositionWithinWarzone()
		{
			return UnityEngine.Random.Range(-1f, 6.5f);
		}
	}
}
