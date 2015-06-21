using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class ObstaclesSpawnController : ExtendedMonoBehaviour
	{
		public GameObject heron;
		public GameObject mine;
		public GameObject smallForceField;
		public GameObject largeForceField;
		
		public float spawnInterval = 4f;

		private ICollection<ObstacleGroup> obstacleGroups;

		private void Start ()
		{
			obstacleGroups = new List<ObstacleGroup>()
			{
				new ObstacleGroup(heron, Game.WarzoneBounds),
				new ObstacleGroup(mine, Game.WarzoneBounds),
				new ObstacleGroup(smallForceField, Game.WarzoneBounds),
				new ObstacleGroup(largeForceField, Game.WarzoneBounds)
			};

			InvokeRepeating("Spawn", 0.0f, spawnInterval);
		}
		
		private void Spawn()
		{
			var index = Random.Range(0, obstacleGroups.Count);
			var obstacleGroup = obstacleGroups.ElementAt(index);

			foreach (var obstacle in obstacleGroup.Obstacles)
			{
				Instantiate(
					obstacle.ObstacleObject, 
		            new Vector2(Game.WarzoneBounds.Right() + obstacle.Size().x / 2, 
				            obstacle.PositionRange.GetNumberInRange()), 
					transform.rotation);
			}
		}
	}







	internal class ObstacleGroup
	{
		public ICollection<Obstacle> Obstacles { get; set; }

		public ObstacleGroup(GameObject gameObject1, Bounds bounds)
		{
			this.Obstacles = new List<Obstacle>()
			{
				new Obstacle(gameObject1, bounds)
			};
		}
	}
}
