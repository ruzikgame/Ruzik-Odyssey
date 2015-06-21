using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{
	public class PlayerModel : MonoBehaviour
	{
		private void Awake()
		{

		}

		public float AccelerationMultiplier { get; set; }

		public float CannonCooldown { get; set; }
		public float CannonFiringRate { get; set; }

		public Vector2 CannonShotPosition { get; set; }

		public AudioClip CannonFiringSFX { get; set; }
		public float CannonFiringSFXVolume { get; set; }
		public GameObject CannonShotPrefab { get; set; }

		public float MissileCooldown { get; set; }
		public float MissileFiringRate { get; set; }

		public Vector2 MissileShotPosition { get; set; }

		public AudioClip MissileFiringSFX { get; set; }
		public float MissileFiringSFXVolume { get; set; }
		public GameObject MissileShotPrefab { get; set; }

	}
}
