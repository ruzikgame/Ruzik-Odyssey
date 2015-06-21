namespace RuzikOdyssey.Domain.Inventory
{
	public class AmmoInventoryItem : InventoryItem
	{
		public int Attack { get; set; }
		public int Quantity { get; set; }

		public AmmoInventoryItem()
		{
			this.Category = InventoryItemCategory.Ammo;
		}
	}
}
