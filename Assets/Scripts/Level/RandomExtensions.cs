using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using System.Linq;
using System;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Enemies;
using Random = UnityEngine.Random;
using Newtonsoft.Json;
using RuzikOdyssey.Models;

namespace RuzikOdyssey.Level
{
	public static class RandomExtensions
	{
		public static float RandomVerticalPositionWithinBounds(this Bounds bounds, GameObject gameObject)
		{
			return Random.Range(bounds.Bottom() + gameObject.RendererSize().y / 2, 
			                    bounds.Top() - gameObject.RendererSize().y / 2);
		}
	}
	
}
