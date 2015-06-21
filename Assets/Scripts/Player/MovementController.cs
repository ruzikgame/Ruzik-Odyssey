using UnityEngine;

namespace RuzikOdyssey.Player
{
	public abstract class MovementController : MonoBehaviour
	{
		protected float accelarationMultiplier = 1.0f;

		private int numberOfSlowDownEffects = 0;

		public abstract void Move(Vector2 position, Vector2 deltaPosition);
		public abstract void Stop();

		public void ApplyAccelarationMultiplier(float multiplier)
		{
			numberOfSlowDownEffects++;

			if (this.accelarationMultiplier < 1 && multiplier < 1) return;

			this.accelarationMultiplier *= multiplier;
		}

		public void ResetAccelarationMultiplier()
		{
			numberOfSlowDownEffects--;
			if (numberOfSlowDownEffects == 0) accelarationMultiplier = 1.0f;
		}
	}
}
