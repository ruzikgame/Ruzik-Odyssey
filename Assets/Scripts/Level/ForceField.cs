using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class ForceField : MonoBehaviour
	{
		public float speedDecrease = 0.5f;
		public float speedDecreaseTime = 3f;

		public GameObject slowDownEffect;

		private void Start()
		{
			this.gameObject.GetComponent<Rigidbody2D>().velocity = GameEnvironment.ForegroundSpeed;
		}

		private void OnTriggerEnter2D(Collider2D otherCollider)
		{
			if (otherCollider.CompareTag("Player"))
			{
				var effectGameObject = (GameObject)Instantiate(slowDownEffect, 
				                                               otherCollider.gameObject.transform.position, 
				                                               transform.rotation);
				var effectBehavior = effectGameObject.GetComponent<SlowDownEffect>();
				effectBehavior.speedDecrease = speedDecrease;
				effectBehavior.speedDecreaseTime = speedDecreaseTime;
			}
		}
	}
}
