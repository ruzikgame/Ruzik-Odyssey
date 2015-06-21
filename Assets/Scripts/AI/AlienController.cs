using UnityEngine;
using RuzikOdyssey.Weapons;
using RuzikOdyssey.Player;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;
using System;
using RuzikOdyssey.Enemies;

using Random = UnityEngine.Random;

namespace RuzikOdyssey.Ai
{
	public class AlienController : MonoBehaviour
	{
		public GameObject energyBall;

		public float warzoneSpeed = 50f;
		public int scoreForKill = 1;

		public float damageFromCollision = 1.0f;

		public GameObject deathExplosion;
		public float deathExplosionDuration = 2.0f;
		public float deathDelay = 0.0f;

		public AudioClip deathExplosionSfx;
		public float deathExplosionSfxVolume = 0.5f;

		private bool isInWarzone;

		private float nonWarzoneSpeed = 100f;

		public float health;

		public int maxEnergyDrop = 2;

		[SerializeField]
		private int agression;

		[SerializeField]
		private int intelligence;

		private AlienWeaponController weaponController;
		private MovementStrategy movementStrategy;

		private float speed;

		private bool isDying = false;

		public event EventHandler<EventArgs> Destroyed;

		protected virtual void OnDestroyed()
		{
			if (Destroyed != null) Destroyed(this, EventArgs.Empty);
		}

		public event EventHandler<EnemyDiedEventArgs> Died;

		protected virtual void OnDied()
		{
			if (Died != null) Died(this, new EnemyDiedEventArgs { ScoreForKill = scoreForKill });
		}

		public void ApplyDamage(float damage)
		{
			TakeDamage(damage);
		}

		private void Start()
		{
			weaponController = GetComponent<AlienWeaponController>();
			movementStrategy = GetComponent<MovementStrategy>();

			isInWarzone = false;
			speed = nonWarzoneSpeed;
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.tag.Equals("WarzoneBoundary")) OnEnterWarzone();

			if (otherCollider.CompareTag("Player"))
				otherCollider.gameObject.GetComponent<RuzikController>()
										.ApplyDamage(damageFromCollision);
		}

		private void OnWeaponHit(GameObject gameObject)
		{

		}

		private void OnEnterWarzone()
		{
			isInWarzone = true;
			speed = warzoneSpeed;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}

		private void TakeDamage(float damage)
		{
			if (isDying) return;

			health -= damage;
			if (health <= 0) Die();
		}

		private void Die()
		{
			isDying = true;

			OnDied();

			DropEnergy();

			SoundEffectsController.Instance.PlayPlayerTaunt();

			if (deathExplosion != null) 
			{
				var explosion = Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
				Destroy(explosion, deathExplosionDuration);
			}

			Invoke("DestroySelf", deathDelay);
		}

		private void DestroySelf()
		{
			if (deathExplosionSfx != null) 
				SoundEffectsController.Instance.Play(deathExplosionSfx, deathExplosionSfxVolume);

			Destroy(this.gameObject);
		}

		private void DropEnergy()
		{
			var energyAmount = Random.Range(0, maxEnergyDrop);

			for (int i = 0; i < energyAmount; i++)
			{
				energyBall.InstantiateAtPosition(
					this.transform.position,
					5.0f * Vector2.one.RandomNormilazed(), ForceMode2D.Impulse);
			}
		}

		private void Update()
		{
			if (isInWarzone && weaponController != null && !isDying)
			{
				weaponController.AttackWithMainWeapon();
				weaponController.AttackWithSecondaryWeapon();
			}
		}

		private void FixedUpdate()
		{
			if (isDying)
			{
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				return;
			}

			var movementDirection =  
				movementStrategy.GetMovementDirection(transform.localPosition, 
				                                      GameHelper.Instance.PlayerPosition, 
				                                      isInWarzone,
				                                      Time.fixedDeltaTime);
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().AddForce(movementDirection * speed);
		}

		private void OnDestroy()
		{
			OnDestroyed();
		}
	}
}
