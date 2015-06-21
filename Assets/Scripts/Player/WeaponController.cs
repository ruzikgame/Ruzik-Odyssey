using UnityEngine;
using System;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Player
{
	public class AmmoChangedEventArgs : EventArgs
	{
		public int NewValue { get; set; }
	}

	public class WeaponController : MonoBehaviour
	{
		public Transform mainWeaponPrefab;
		public Transform secondaryWeaponPrefab;
		
		public float mainWeaponShootingRate = 0.25f;
		public float secondaryWeaponShootingRate = 0.25f;
		
		public Vector2 mainWeaponPosition = Vector2.zero;
		public Vector2 secondaryWeaponPosition = Vector2.zero;

		public bool attackIsControlledFromAnimator = true;

		public AudioClip cannonSFX;
		public AudioClip missileSFX;
		public float cannonSfxVolume = 1.0f;
		public float missileSfxVolume = 1.0f;

		public int cannonSfxRate = 3;
	
		private float cannonCooldown = 0.25f;
		private float missileCooldown = 0.5f;

		private int cannonSFXCooldown;

		public bool hasAmmo = false;

		public int missileAmmo = 0;

		private Animator animator;

		private void Start()
		{
			animator = this.gameObject.GetComponent<Animator>();
			if (animator == null)
			{
				Debug.LogWarning(String.Format(
					"Failed to initialize an animator for game object with tag {0}", gameObject.name));
			}

			cannonSFXCooldown = 0;
		}

		private void Update()
		{
			if (cannonCooldown > 0) cannonCooldown -= Time.deltaTime;
			if (missileCooldown > 0) missileCooldown -= Time.deltaTime;
		}

		public void ChangeMissileAmmo(int delta)
		{
			if (!hasAmmo) return;

			missileAmmo += delta;

			OnMissileAmmoChanged(missileAmmo);
		}

		public event EventHandler<AmmoChangedEventArgs> MissileAmmoChanged;

		private void OnMissileAmmoChanged(int newValue)
		{
			if (MissileAmmoChanged != null) MissileAmmoChanged(this, new AmmoChangedEventArgs { NewValue = newValue });
		}

		public void AttackWithMainWeapon()
		{
			if (!CanAttackWithMainWeapon() || mainWeaponPrefab == null) return;

			if (animator != null) animator.SetBool("IsShooting", true);

			cannonCooldown = mainWeaponShootingRate;

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

			if (ShouldPlayCannonSfx()) 
			{
				SoundEffectsController.Instance.Play(cannonSFX, cannonSfxVolume);
				cannonSFXCooldown = cannonSfxRate;
			}
			else 
			{
				cannonSFXCooldown -= 1;
			}
		}

		public void FinishShootingMainWeapon()
		{
			if (animator != null) animator.SetBool("IsShooting", false);
			cannonSFXCooldown = 0;
		}
		
		public void AttackWithSecondaryWeapon()
		{
			if (!CanAttackWithSecondaryWeapon() || !HasSecondaryWeapon()) return;

			missileCooldown = secondaryWeaponShootingRate;

			ChangeMissileAmmo(-1);

			var shot = (Transform)Instantiate(secondaryWeaponPrefab);
			shot.position = new Vector2(transform.position.x + secondaryWeaponPosition.x,
			                            transform.position.y + secondaryWeaponPosition.y);

			if (missileSFX != null) 
				SoundEffectsController.Instance.Play(missileSFX, missileSfxVolume);
		}
		
		private bool CanAttackWithMainWeapon()
		{
			return cannonCooldown <= 0f;
		}

		private bool CanAttackWithSecondaryWeapon()
		{
			return missileCooldown <= 0f && missileAmmo > 0;
		}

		public bool HasSecondaryWeapon()
		{
			return secondaryWeaponPrefab != null;
		}

		public bool ShouldPlayCannonSfx()
		{
			return cannonSFXCooldown <= 0 && cannonSFX != null;
		}
	}
}
