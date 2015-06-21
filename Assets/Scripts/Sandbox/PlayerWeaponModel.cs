using UnityEngine;

namespace Sandbox.RuzikOdyssey.Player
{
	public class PlayerWeaponModel : Model
	{
		public Transform mainWeaponPrefab;
		public Transform secondaryWeaponPrefab;
		
		public float mainWeaponShootingRate = 0.25f;
		public float secondaryWeaponShootingRate = 0.25f;
		
		public Vector2 mainWeaponPosition = Vector2.zero;
		public Vector2 secondaryWeaponPosition = Vector2.zero;
		

		public AudioClip mainWeaponSoundEffect;
		public AudioClip secondaryWeaponSoundEffect;
		public float mainWeaponSoundEffectVolume = 1.0f;
		public float secondWeaponSoundEffectVolume = 1.0f;
		
//		private float mainWeaponCooldown = 0.25f;
//		private float secondaryWeaponCooldown = 0.5f;
	}

	public abstract class Model : Component
	{

	}
}
