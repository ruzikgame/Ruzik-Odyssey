using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{

	public class ObstaclesAndItemsLevelDesign
	{
		private List<GameObject> source;

		private int nextGroupIndex = 0;
		private List<GameObjectsGroupDesign> levelDesign;

		public ObstaclesAndItemsLevelDesign(List<GameObject> source)
		{
			this.source = source;
			LoadLevelDesign();
		}

		public GameObjectsGroupDesign GetNextGroup()
		{
			if (nextGroupIndex >= levelDesign.Count) return null;
			
			var group = levelDesign[nextGroupIndex];
			nextGroupIndex++;

			return group;
		}

		private void LoadLevelDesign()
		{
			levelDesign = new List<GameObjectsGroupDesign>
			{
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 5.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
				new GameObjectsGroupDesign
				{
					Objects = new List<GameObject>
					{
						source[0], source[0], source[0], source[0]
					},
					NextGroupInterval = 15.0f
				},
			};
		}

	}
}
