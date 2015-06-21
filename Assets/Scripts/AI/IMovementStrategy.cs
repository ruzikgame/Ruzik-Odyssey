using System;
using UnityEngine;

namespace RuzikOdyssey.Ai
{
	public interface IMovementStrategy
	{
		Vector2 GetMovementDirection(Vector2 currentPosition, Vector2 playerPosition, bool isInWarzone, float deltaTime);
	}
}