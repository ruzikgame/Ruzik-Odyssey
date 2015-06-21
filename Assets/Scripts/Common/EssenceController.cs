using UnityEngine;
using System.Collections;
using System;

namespace RuzikOdyssey.Common
{
	public class EssenceController : MonoBehaviour
	{
		public float amount;
		public string gameObjectName;
		
		private float initialAmount;
		private BarController barController;
		
		private void Awake()
		{
			var bar = GameObjectExtensions.FindOrThrow(gameObjectName);
			barController = bar.GetComponentOrThrow<BarController>();
			
			initialAmount = amount;
		}
		
		public float Change(float delta)
		{
			amount += delta;
			
			if (amount > initialAmount) amount = initialAmount;
			if (amount < 0) amount = 0;
			
			UpdateBar(amount);

			return amount;
		}
		
		private void UpdateBar(float currentAmount)
		{
			int level = (int)(100 * currentAmount / initialAmount);
			
			barController.ShowLevel(level);
		}
	}
}

