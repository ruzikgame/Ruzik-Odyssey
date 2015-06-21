using UnityEngine;

namespace RuzikOdyssey.Common
{
	public static class MonoBehaviourExtensions
	{
		public static GameObject InstantiateWithVelocity(
			this GameObject gameObject,
			Vector2 position, float velocity, Vector2 direction)
		{
			var instance = (GameObject)Object.Instantiate(gameObject, position, new Quaternion(0, 0, 0, 0));
			instance.GetComponent<Rigidbody2D>().velocity = velocity * direction;
			
			return instance;
		}

		public static GameObject InstantiateAtPosition(
			this GameObject gameObject,
			Vector2 position, 
			Vector2 force = default(Vector2), 
			ForceMode2D forceMode = ForceMode2D.Force)
		{
			var instance = (GameObject)Object.Instantiate(gameObject, position, new Quaternion(0, 0, 0, 0));
			instance.GetComponent<Rigidbody2D>().AddForce(force, forceMode);
			
			return instance;
		}


		public static GameObject InstantiateWithinBounds(
			this GameObject gameObject, 
			Bounds bounds,
			Vector2 force = default(Vector2), 
			ForceMode2D forceMode = ForceMode2D.Force)
		{
			var size = gameObject.RendererSize();
			
			var position = new Vector2(
				Random.Range(bounds.Left() + size.x /2, bounds.Right() - size.x / 2),
				Random.Range(bounds.Bottom() + size.y /2, bounds.Top() - size.y / 2)
				);

			return gameObject.InstantiateAtPosition(position, force, forceMode);
		}

		public static GameObject InstantiateAtBoundsEntrance(
			this GameObject gameObject, 
			Bounds bounds,
			Vector2 force = default(Vector2),
			ForceMode2D forceMode = ForceMode2D.Force)
		{
			var size = gameObject.RendererSize();
			
			var position = new Vector2(
				bounds.Right() + size.x / 2,
				Random.Range(bounds.Bottom() + size.y /2, bounds.Top() - size.y / 2)
			);
			
			return gameObject.InstantiateAtPosition(position, force, forceMode);
		}

		public static GameObject InstantiateNearSelf(this GameObject gameObject, GameObject instance)
		{
			return (GameObject)Object.Instantiate(instance, 
			                                      gameObject.transform.position, 
			                                      gameObject.transform.rotation);
		}
	}
}
