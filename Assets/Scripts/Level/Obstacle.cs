using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class Obstacle
	{
		public GameObject ObstacleObject { get; set; }
		public Range PositionRange { get; set; }
		
		public Obstacle(GameObject obstacleObject, Bounds bounds)
		{
			this.ObstacleObject = obstacleObject;
			this.PositionRange = new Range()
			{
				Min = bounds.Bottom() + ObstacleObject.RendererSize().y / 2,
				Max = bounds.Top() - ObstacleObject.RendererSize().y / 2,
			};
		}
		
		public Vector2 Size()
		{
			return ObstacleObject.RendererSize();
		}
	}
}
