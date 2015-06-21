using UnityEngine;
using System.Collections;
using System;

public class LevelBoundariesController : MonoBehaviour 
{
	private void OnTriggerExit2D(Collider2D otherCollider)
	{
		Destroy(otherCollider.gameObject);
	}
}
