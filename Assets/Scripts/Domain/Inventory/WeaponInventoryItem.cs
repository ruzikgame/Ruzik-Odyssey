namespace RuzikOdyssey.Domain.Inventory
{
	public class WeaponInventoryItem : InventoryItem
	{
		public int FireRate { get; set; }

		public WeaponInventoryItem()
		{
			this.Category = InventoryItemCategory.Weapons;
		}
	}
}