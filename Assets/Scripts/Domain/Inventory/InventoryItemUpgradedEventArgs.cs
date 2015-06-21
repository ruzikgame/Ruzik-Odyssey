using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItemUpgradedEventArgs : EventArgs
	{
		public Guid ItemId { get; set; }
		public InventoryItemState ItemState { get; set; }

		public InventoryItemUpgradedEventArgs(Guid id, InventoryItemState state = InventoryItemState.Other)
		{
			this.ItemId = id;
			this.ItemState = state;
		}
	}
}
