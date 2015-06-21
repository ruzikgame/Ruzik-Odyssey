using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{

	public static class GameObjectExtensions
	{
		public static T GetComponentOrThrow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null) throw new Exception(String.Format("GameObject {0} is missing mandatory component {1}",
			                                                         gameObject.name, typeof(T).Name));

			return component;
		}
	}
}
