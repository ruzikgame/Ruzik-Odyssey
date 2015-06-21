using UnityEngine;
using System.Collections;
using RuzikOdyssey.Player;
using RuzikOdyssey.Level;
using System;

public class FireSecondaryWeaponButton : TouchButton 
{	
//	private WeaponController playerWeaponController;

	private new void Start()
	{
		base.Start();
 		
//		playerWeaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
//		if (playerWeaponController == null) 
//			throw new UnityException("Failed to retrieve weapon controller from the player game object");
	}
//
//	protected override void OnTouch()
//	{
//		if (playerWeaponController.HasSecondaryWeapon())
//		{
//			playerWeaponController.AttackWithSecondaryWeapon();
//		}
//	}
}
