using UnityEngine;
using System.Collections;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Player;

namespace RuzikOdyssey.Weapons
{
	public class Weapon : MonoBehaviour 
	{
		public int damage = 1;
		public Vector2 speed = new Vector2(10, 10);
		public Vector2 direction = new Vector2(1, 0);
		public bool isEnemyShot = false;

		private Vector2 movement;

		private void Start()
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(speed.x * direction.x, speed.y * direction.y);
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.tag.Equals("Enemy") && !isEnemyShot)
		    {
				var alienController = otherCollider.gameObject.GetComponent<AlienController>();
				alienController.ApplyDamage(damage);
				Destroy(this.gameObject);
			}
			else if (otherCollider.tag.Equals("Player") && isEnemyShot) 
			{
				var ruzikController = otherCollider.gameObject.GetComponent<RuzikController>();
				ruzikController.ApplyDamage(damage);
				Destroy(this.gameObject);
			}
		}
	}
}
