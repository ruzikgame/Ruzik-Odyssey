using UnityEngine;
using System;

namespace RuzikOdyssey.Player
{
	public class LimitedSpeedDeltaMovementController : MovementController
	{
		public Vector2 speed  = Vector2.one;
		private Vector2 movement = Vector2.zero;
		
		private float leftBoundary;
		private float rightBoundary;
		private float topBoundary;
		private float bottomBoudary;
		
		private void Start()
		{
			leftBoundary = -(Camera.main.aspect * Camera.main.orthographicSize - 1.0f);
			rightBoundary = Camera.main.aspect * Camera.main.orthographicSize - 2.0f;
			topBoundary = Camera.main.orthographicSize - 1.0f;
			bottomBoudary = -(Camera.main.orthographicSize - 0.5f);
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
			GetComponent<Rigidbody2D>().velocity = new Vector2(Math.Abs(movement.x) > speed.x 
			                                   ? Math.Sign(movement.x) * speed.x 
			                                   : movement.x, 
			                                   Math.Abs(movement.y) > speed.y 
			                                   ? Math.Sign(movement.y) * speed.y 
			                                   : movement.y);
			
			movement -= GetComponent<Rigidbody2D>().velocity;
		}
	}
}
