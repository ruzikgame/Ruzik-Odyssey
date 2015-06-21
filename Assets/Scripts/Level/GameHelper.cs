using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey.Common.UI;
using System;

namespace RuzikOdyssey.Level
{
	public class GameHelper : MonoBehaviour
	{
		public static GameHelper Instance { get; private set; }

		[SerializeField]
		private GameObject player;

		[SerializeField]
		private GameObject warzoneBoundary;

		public Vector2 PlayerPosition 
		{ 
			get { return player.gameObject.transform.localPosition; }
		}

		public Bounds WarzoneBounds 
		{ 
			get { return warzoneBoundary.GetComponent<Collider2D>().bounds; }
		}

		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple game helpers detected");
			Instance = this;
		}
	}
}
