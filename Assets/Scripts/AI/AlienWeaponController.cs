using UnityEngine;
using System;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Ai
{
	public class AlienWeaponController : MonoBehaviour
	{
		public Transform mainWeaponPrefab;
		public Transform secondaryWeaponPrefab;
		
		public float mainWeaponShootingRate = 0.25f;
		public float secondaryWeaponShootingRate = 0.25f;
		
		public Vector2 mainWeaponPosition = Vector2.zero;
		public Vector2 secondaryWeaponPosition = Vector2.zero;

		public bool attackIsControlledFromAnimator = true;

		public AudioClip mainWeaponSoundEffect;
		public AudioClip secondaryWeaponSoundEffect;
		public float mainWeaponSoundEffectVolume = 1.0f;
		public float secondWeaponSoundEffectVolume = 1.0f;
	
		private float mainWeaponCooldown = 0.25f;
		private float secondaryWeaponCooldown = 0.5f;

		private Animator animator;

		private void Start()
		{
			animator = this.gameObject.GetComponent<Animator>();
			if (animator == null)
			{
				Debug.LogWarning(String.Format(
					"Failed to initialize an animator for game object with tag {0}", gameObject.name));
			}
		}

		private void Update()
		{
			if (mainWeaponCooldown > 0) mainWeaponCooldown -= Time.deltaTime;
			if (secondaryWeaponCooldown > 0) secondaryWeaponCooldown -= Time.deltaTime;
		}
	
		public void AttackWithMainWeapon()
		{
			if (!CanAttackWithMainWeapon() || mainWeaponPrefab == null) return;

			if (animator != null) animator.SetBool("IsShooting", true);

			mainWeaponCooldown = mainWeaponShootingRate;

			if (!attackIsControlledFromAnimator) 
			{
				ShootMainWeapon();
			}
		}

		private void ShootMainWeapon()
		{
			var shot = (Transform)Instantiate(mainWeaponPrefab);
			shot.position = new Vector2(transform.position.x + mainWeaponPosition.x,
			                            transform.position.y + mainWeaponPosition.y);

			if (mainWeaponSoundEffect != null) 
				SoundEffectsController.Instance.Play(mainWeaponSoundEffect, mainWeaponSoundEffectVolume);
		}

		public void FinishShootingMainWeapon()
		{
			if (animator != null) animator.SetBool("IsShooting", false);
		}
		
		public void AttackWithSecondaryWeapon()
		{
			if (!CanAttackWithSecondaryWeapon() || !HasSecondaryWeapon()) return;

			secondaryWeaponCooldown = secondaryWeaponShootingRate;

			var shot = (Transform)Instantiate(secondaryWeaponPrefab);
			shot.position = new Vector2(transform.position.x + secondaryWeaponPosition.x,
			                            transform.position.y + secondaryWeaponPosition.y);

			if (secondaryWeaponSoundEffect != null) 
				SoundEffectsController.Instance.Play(secondaryWeaponSoundEffect, secondWeaponSoundEffectVolume);
		}
		
		private bool CanAttackWithMainWeapon()
		{
			return mainWeaponCooldown <= 0f;
		}

		private bool CanAttackWithSecondaryWeapon()
		{
			return secondaryWeaponCooldown <= 0f;
		}

		public bool HasSecondaryWeapon()
		{
			return secondaryWeaponPrefab != null;
		}
	}
}
