using UnityEngine;
using System;

namespace RuzikOdyssey.Player
{
	public class LimitedForceDeltaMovementController : MovementController
	{
		public int maximumForce = 150;

		private Vector2 movement = Vector2.zero;
		
		private float leftBoundary;
		private float rightBoundary;
		private float topBoundary;
		private float bottomBoudary;

		private Vector2 lastPosition;

		private void Start()
		{
			leftBoundary = -(Camera.main.aspect * Camera.main.orthographicSize - 1.0f);
			rightBoundary = Camera.main.aspect * Camera.main.orthographicSize - 2.0f;
			topBoundary = Camera.main.orthographicSize - 1.0f;
			bottomBoudary = -(Camera.main.orthographicSize - 0.5f);

			lastPosition = Camera.main.WorldToScreenPoint(transform.position);
		}

		public override void Move(Vector2 position, Vector2 deltaPosition)
		{		
			movement += deltaPosition;
			
			if ((transform.position.x <= leftBoundary && movement.x < 0) || 
			    (transform.position.x >= rightBoundary && movement.x > 0))
			{
				movement.x = 0;
			}
			if ((transform.position.y <= bottomBoudary && movement.y < 0) || 
			    (transform.position.y >= topBoundary && movement.y > 0))
			{
				movement.y = 0;
			}
		}

		public override void Stop()
		{
			movement = Vector2.zero;
		}
		
		private void FixedUpdate()
		{
			var force = movement;

			if (Math.Abs(force.x) > maximumForce) force.x = Math.Sign(force.x) * maximumForce;
			if (Math.Abs(force.y) > maximumForce) force.y = Math.Sign(force.y) * maximumForce;

			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().AddForce(accelarationMultiplier * force);

			var currentPosition = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
			var deltaPosition =  currentPosition - lastPosition;

			movement.x = (Math.Abs(movement.x) - Math.Sign(movement.x) * deltaPosition.x > 0)
						 ? movement.x -= deltaPosition.x
						 : 0;
			movement.y = (Math.Abs(movement.y) - Math.Sign(movement.y) * deltaPosition.y > 0)
						 ? movement.y -= deltaPosition.y
						 : 0;

			lastPosition = currentPosition;
		}
	}
}
