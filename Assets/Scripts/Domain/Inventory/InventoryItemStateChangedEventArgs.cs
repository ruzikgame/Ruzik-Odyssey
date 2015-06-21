using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItemStateChangedEventArgs : EventArgs
	{
		public Guid ItemId { get; set; }
		public InventoryItemState OldState { get; set; }
		public InventoryItemState NewState { get; set; }

		public InventoryItemStateChangedEventArgs(
			Guid itemId = default(Guid), 
			InventoryItemState oldState = InventoryItemState.Other,
			InventoryItemState newState = InventoryItemState.Other)
		{
			this.ItemId = itemId;
			this.OldState = oldState;
			this.NewState = newState;
		}
	}

}
