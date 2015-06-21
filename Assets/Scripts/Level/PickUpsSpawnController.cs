using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class PickUpsSpawnController : ExtendedMonoBehaviour
	{
		public GameObject healthPickUp;
		public GameObject secondaryWeaponPickUp;
		
		public int spawnInterval = 60;
		public int spawnDelay = 15;
		
		private void Start ()
		{
			InvokeRepeating("Spawn", spawnDelay, 1);
			healthPickUp.GetComponent<Rigidbody2D>().velocity = GameEnvironment.ForegroundSpeed;
			secondaryWeaponPickUp.GetComponent<Rigidbody2D>().velocity = GameEnvironment.ForegroundSpeed;
		}
		
		private void Spawn()
		{
			var dice = Random.Range(0, spawnInterval);

			if (dice > 1) return;

			var pickUpTypeDice = Random.Range(0, 10);

			if (pickUpTypeDice < 5) healthPickUp.InstantiateAtBoundsEntrance(Game.WarzoneBounds);
			else secondaryWeaponPickUp.InstantiateAtBoundsEntrance(Game.WarzoneBounds);
		}


	}
}

