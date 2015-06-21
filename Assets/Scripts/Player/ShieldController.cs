using UnityEngine;
using System.Collections;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Player
{
	public class ShieldController : MonoBehaviour
	{
		public GameObject shieldEffect;

		private Shield shieldEffectBehavior;

		public Vector2 shieldAdjustment = new Vector2(1.5f, 0);

		private void Start()
		{
			var shieldEffectGameObject = (GameObject) Instantiate(shieldEffect, 
			                                                      gameObject.transform.position + (Vector3)shieldAdjustment, 
			                                                      transform.rotation);
			shieldEffectGameObject.transform.parent = gameObject.transform;
			shieldEffectBehavior = shieldEffectGameObject.GetComponentOrThrow<Shield>();

			ChangeShieldVisibility(false);
		}

		public float ShieldDamage(float damage)
		{
			shieldEffectBehavior.ShowShield();

			var shieldedDamage = damage / 2;

			return (damage - shieldedDamage);
		}

		public void ChangeShieldVisibility(bool isVisible)
		{
			if (shieldEffectBehavior == null) 
			{
				Log.Warning("ChangeShieldVisibility was called before ShieldController finished initialization");
				return;
			}

			shieldEffectBehavior.gameObject.GetComponent<Renderer>().enabled = isVisible;
		}
	}
}