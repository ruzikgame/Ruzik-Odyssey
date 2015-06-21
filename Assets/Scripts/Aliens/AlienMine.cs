using UnityEngine;
using System.Collections;
using RuzikOdyssey.Ai;
using RuzikOdyssey.Player;
using System;
using System.Linq;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Aliens
{
	public class AlienMine : MonoBehaviour 
	{
		public int damage = 10;
		public float detonationDelay = 1.0f;

		public GameObject detonationVfx;
		public GameObject explosionVfx;
		public AudioClip explosionSfx;

		private Vector2 movement;
		private string hitTag;

		private Animator animator;
		
		private void Start()
		{
			GetComponent<Rigidbody2D>().velocity = GameEnvironment.ForegroundSpeed;

			animator = this.gameObject.GetComponentOrThrow<Animator>();
		}
		
		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag("Player")) 
			{
				var explosion = this.gameObject.InstantiateNearSelf(detonationVfx);
				explosion.GetComponent<Rigidbody2D>().velocity = GameEnvironment.ForegroundSpeed;
				Destroy(explosion, 5);

				Invoke("Detonate", detonationDelay);
			}
		}

		private void Detonate()
		{
			animator.SetBool("IsDetonating", true);
		}

		private void Explode()
		{
			var radiusCollider = (CircleCollider2D) this.GetComponent<Collider2D>();

			var playerCollider = Physics2D
				.OverlapCircleAll(this.transform.position, radiusCollider.radius * this.gameObject.Scale2D())
				.SingleOrDefault(x => x.CompareTag("Player"));

			if (playerCollider != null)
			{
				var ruzikController = playerCollider.gameObject.GetComponent<RuzikController>();
				ruzikController.ApplyDamage(damage);
			}

			var explosion = this.gameObject.InstantiateNearSelf(explosionVfx);
			Destroy(explosion, 5);
			SoundEffectsController.Instance.Play(explosionSfx);

			Destroy(this.gameObject);
		}
	}
}
