using UnityEngine;
using RuzikOdyssey.Level;
using RuzikOdyssey.Ai;
using System;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Player
{
	public sealed class RuzikController : MonoBehaviour
	{
		public float damageFromCollision;

		public AudioClip explosionSfx;

		public MovementController movementController;
		public WeaponController weaponController;

		public HealthController healthController;
		public EnergyController energyController;
		public ShieldController shieldController;

		private bool shieldEnabled = false;

		private void Start()
		{
		}

		public void FireMissile()
		{
			weaponController.AttackWithSecondaryWeapon();
		}

		public void SetShieldActive(bool shieldActive)
		{
			this.shieldEnabled = shieldActive;
			shieldController.ChangeShieldVisibility(shieldEnabled);
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag("Enemy"))
			{
				otherCollider.gameObject.GetComponent<AlienController>()
										.ApplyDamage(damageFromCollision);
			} 
			else if (otherCollider.CompareTag("PickUp"))
			{
				var pickUp = otherCollider.gameObject.GetComponentOrThrow<PickUp>();

				switch (pickUp.type)
				{
					case PickUpType.Health:
						healthController.Change(pickUp.amount);
						break;
					case PickUpType.Energy:
						energyController.Change(pickUp.amount);
						break;
					case PickUpType.Weapon:
						weaponController.ChangeMissileAmmo(10);
						break;
					default:
						Debug.LogWarning("Unrecognized pick up type");
						break;
				}
				Destroy(pickUp.gameObject);
			}
		}

		public void AttackWithMainWeapon()
		{
			weaponController.AttackWithMainWeapon();
		}

		public void Move(Vector2 position, Vector2 deltaPosition)
		{
			movementController.Move(position, deltaPosition);
		}

		public void Stop()
		{
			movementController.Stop();
			weaponController.FinishShootingMainWeapon();
		}

		public void ApplyDamage(float damage)
		{
			var remainingDamage = (energyController.amount > 0 && shieldEnabled)
				? shieldController.ShieldDamage(damage) 
				: damage;
			var energyLeft = energyController.Change(remainingDamage - damage);

			if (energyLeft <= 0) 
			{
				/* TODO
				 * Notify UI control that it must change state to off.
				 */

				shieldEnabled = false;
				shieldController.ChangeShieldVisibility(shieldEnabled);
			}

			var healthLeft = healthController.Change(-remainingDamage);

			if (healthLeft < 0.1) Die ();
		}

		private void Die()
		{
			Destroy (gameObject);
			OnDied();
		}

		public event EventHandler<EventArgs> Died;
		private void OnDied()
		{
			if (Died != null) Died(this, EventArgs.Empty);
		}

		public void SlowDown(float speedDecrease)
		{
			movementController.ApplyAccelarationMultiplier(speedDecrease);
		}

		public void CancelSlowDown()
		{
			movementController.ResetAccelarationMultiplier();
		}

	}
}
