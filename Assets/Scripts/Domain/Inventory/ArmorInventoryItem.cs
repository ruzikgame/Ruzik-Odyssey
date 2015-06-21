namespace RuzikOdyssey.Domain.Inventory
{
	public class ArmorInventoryItem : InventoryItem
	{
		public int Armor { get; set; }

		public ArmorInventoryItem()
		{
			this.Category = InventoryItemCategory.Fuselage;
		}
	}
}
