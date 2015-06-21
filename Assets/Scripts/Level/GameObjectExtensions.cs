using UnityEngine;
using System;
using System.Collections.Generic;

namespace RuzikOdyssey.Common
{
	public static class GameObjectExtensions
	{
		public static Vector2 RendererSize(this GameObject gameObject)
		{
			try
			{
				return gameObject.GetComponent<Renderer>().bounds.size.ToVector2();
			}
			catch (Exception ex)
			{
				Log.Error("Failed to determine renderer size for {0}. Exception: {1}", gameObject.name, ex);
				return Vector2.zero;
			}
		}

		public static Vector2 ToVector2(this Vector3 vector)
		{
			return (Vector2)vector;
		}

		public static T GetComponentOrThrow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null) throw new Exception(String.Format("GameObject {0} is missing a mandatory component {1}",
			                                                         gameObject.name, typeof(T).Name));
			
			return component;
		}

		public static T GetComponentInChildrenOrThrow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponentInChildren<T>();
			if (component == null) throw new Exception(String.Format("GameObject {0}'s children are missing a mandatory component {1}",
			                                                         gameObject.name, typeof(T).Name));
			
			return component;
		}

		public static GameObject FindOrThrow(string name)
		{
			var gameObject = GameObject.Find(name);
			if (gameObject == null) 
				throw new Exception(String.Format("Failed to find game object named {0} in the hierarchy", name));

			return gameObject;
		}

		public static float Scale2D(this GameObject gameObject)
		{
			return Math.Min(gameObject.transform.lossyScale.x, 
			                gameObject.transform.lossyScale.y);
		}

		public static GameObject SingleChild(this GameObject gameObject)
		{
			if (gameObject.transform == null) return null;
			if (gameObject.transform.childCount > 1) 
				throw new UnityException(String.Format("{0} has more than one child."));

			return gameObject.transform.GetChild(0).gameObject;
		}

		public static IList<GameObject> Children(this GameObject gameObject)
		{
			if (gameObject.transform == null) return null;

			var children = new List<GameObject>();

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				children.Add(gameObject.transform.GetChild(i).gameObject);
			}

			return children;
		}

		public static GameObject GetChild(this GameObject gameObject, int childIndex)
		{
			if (gameObject.transform == null) return null;
			if (gameObject.transform.childCount < childIndex + 1) return null;

			return gameObject.transform.GetChild(childIndex).gameObject;
		}

		public static int ChildCount(this GameObject gameObject)
		{
			return gameObject.transform == null ? 0 : gameObject.transform.childCount;
		}

		public static void DestroyAllChildren(this GameObject gameObject)
		{
			var childCount = gameObject.ChildCount();
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(gameObject.GetChild(i));
			}
		}
	}
}
