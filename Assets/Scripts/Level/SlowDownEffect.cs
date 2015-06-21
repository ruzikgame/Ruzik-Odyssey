using UnityEngine;
using RuzikOdyssey.Player;
using System.Collections;

namespace RuzikOdyssey.Level
{
	public class SlowDownEffect : MonoBehaviour
	{
		public float speedDecrease = 0.5f;
		public float speedDecreaseTime = 5.0f;

		private RuzikController playerController;

		private void Start()
		{
			playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<RuzikController>();
			if (playerController == null) throw new UnityException("Failed to slow down game object");

			playerController.SlowDown(speedDecrease);
			Invoke("CancelEffect", speedDecreaseTime);
		}

		private void CancelEffect()
		{
			playerController.CancelSlowDown();
			Destroy(this.gameObject);
		}
	}
}
