using UnityEngine;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Ai
{
	public class ContinuousMovementsMovementStrategy : MovementStrategy
	{
		private Vector2 targetPosition;

		private float allowedTargetPositionDistanceError = 0.1f;

		private float movementCooldown;

		private void Start()
		{
			targetPosition = GetRandomPositionWithinWarzone();
		}

		protected override Vector2 CalculateMovementDirection(Vector2 currentPosition, Vector2 playerPosition, float deltaTime)
		{
			if (IsAtStop) 
			{
				movementCooldown -= deltaTime;
				return Vector2.zero;
			}

			if (Vector2.Distance(currentPosition, targetPosition) <= allowedTargetPositionDistanceError)
			{
				PauseMovement();
				targetPosition = GetRandomPositionWithinWarzone();
			}

			return (targetPosition - currentPosition).normalized;
		}

		private Vector2 GetRandomPositionWithinWarzone()
		{
			var x = Random.Range(-1f, 6.5f);
			var y = GameHelper.Instance.WarzoneBounds.RandomVerticalPositionWithinBounds(this.gameObject);
			return new Vector2(x, y);
		}

		private bool IsAtStop { get { return movementCooldown > 0; } }

		private void PauseMovement()
		{
			movementCooldown = Random.Range(1f, 10f);
		}
	}

}
