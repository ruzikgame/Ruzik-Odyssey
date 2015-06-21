using UnityEngine;

namespace RuzikOdyssey.UI.Elements
{
	public class InventoryItemDescription : MonoBehaviour
	{
		public UILabel equipButtonTitle;

		public UILabel itemName;
		public UISprite image;

		public UILabel rarity;
		public UILabel itemClass;
		public UILabel level;

		public InventoryItemDescriptionFeature feature1;
		public InventoryItemDescriptionFeature feature2;
		public InventoryItemDescriptionFeature feature3;
		public InventoryItemDescriptionFeature feature4;

		public InventoryItemDescriptionPrice price;
		public InventoryItemDescriptionPrice upgradePrice;
	}
}