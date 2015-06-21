using System.Collections.Generic;

namespace RuzikOdyssey.Domain.Inventory
{
	public class GameInventory
	{
		public ICollection<InventoryItem> AvailableItems { get; set; }
		public ICollection<InventoryItem> PurchasedItems { get; set; }
		public ICollection<InventoryItem> EquippedItems { get; set; }
	}
}
