using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public class PassByMovementStrategy : MovementStrategy
	{
		protected override Vector2 CalculateMovementDirection(Vector2 currentPosition, Vector2 playerPosition, float deltaTime)
		{
			return new Vector2(-1, 0);
		} 
	}
}
