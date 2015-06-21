using UnityEngine;
using System;

namespace Sandbox.RuzikOdyssey.Player
{
	public interface IPlayerWeaponController
	{

	}

	public class PlayerWeaponController : MonoBehaviour, IPlayerWeaponController 
	{
		public bool attackIsControlledFromAnimator = true;

		private PlayerModel model;
		private Animator animator;

		private void Awake()
		{
			model = new PlayerModel();
			animator = gameObject.GetComponentOrThrow<Animator>();
		}

		private void Update()
		{
			if (model.CannonCooldown > 0) model.CannonCooldown -= Time.deltaTime;
			if (model.MissileCooldown > 0) model.MissileCooldown -= Time.deltaTime;
		}
		
		public void AttackWithMainWeapon()
		{
			if (!CanAttackWithCannon() || model.CannonShotPrefab == null) return;

			if (animator != null) animator.SetBool("IsShooting", true);

			model.CannonCooldown = model.CannonFiringRate;

			if (!attackIsControlledFromAnimator) 
			{
				FireCannon();
			}
		}

		private void FireCannon()
		{
			var shot = Instantiate(model.CannonShotPrefab);
			shot.transform.position = (Vector2)transform.position + model.CannonShotPosition;

			if (model.CannonFiringSFX != null) 
				SoundEffectsController.Instance.Play(model.CannonFiringSFX, model.CannonFiringSFXVolume);
		}

		public void FinishFireCannon()
		{
			if (animator != null) animator.SetBool("IsShooting", false);
		}
		
		public void AttackWithSecondaryWeapon()
		{
			if (!CanAttackWithMissile() || model.MissileShotPrefab == null) return;

			model.MissileCooldown = model.MissileFiringRate;



			var shot = Instantiate(model.MissileShotPrefab);
			shot.transform.position = (Vector2)transform.position + model.MissileShotPosition;

			if (model.MissileFiringSFX != null) 
				SoundEffectsController.Instance.Play(model.MissileFiringSFX, model.MissileFiringSFXVolume);
		}
		
		private bool CanAttackWithCannon()
		{
			return model.CannonCooldown <= 0;
		}

		private bool CanAttackWithMissile()
		{
			return model.MissileCooldown <= 0;
		}

		public bool HasMissile()
		{
			return model.MissileShotPrefab != null;
		}
	}
}
