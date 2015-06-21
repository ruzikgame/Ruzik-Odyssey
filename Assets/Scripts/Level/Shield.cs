using UnityEngine;
using System;

namespace RuzikOdyssey.Level
{
	public class Shield : MonoBehaviour
	{
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

		public void ShowShield()
		{
			if (animator != null) animator.SetBool("IsActive", true);
		}

		public void HideShield()
		{
			if (animator != null) animator.SetBool("IsActive", false);
		}
	}
}
