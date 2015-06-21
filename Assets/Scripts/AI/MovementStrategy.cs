using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public abstract class MovementStrategy : MonoBehaviour, IMovementStrategy
	{
		public Vector2 GetMovementDirection (Vector2 currentPosition, Vector2 playerPosition, bool isInWarzone, float deltaTime)
		{
			if (!isInWarzone) return new Vector2(-1, 0);
			return CalculateMovementDirection(currentPosition, playerPosition, deltaTime);
		}

		protected abstract Vector2 CalculateMovementDirection(Vector2 currentPosition, Vector2 playerPosition, float deltaTime);
	}
}

