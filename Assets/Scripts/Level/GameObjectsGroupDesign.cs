using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{

	public class GameObjectsGroupDesign
	{
		public List<GameObject> Objects { get; set; }
		public float NextGroupInterval { get; set; }

		public static GameObjectsGroupDesign WaitForSeconds(float nextGroupInterval)
		{
			return new GameObjectsGroupDesign
			{
				Objects = new List<GameObject>(),
				NextGroupInterval = nextGroupInterval
			};
		}
	}

	
}
