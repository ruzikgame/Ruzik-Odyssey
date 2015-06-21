using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItemSelectedEventArgs : EventArgs
	{
		public Guid ItemId { get; set; }

		public InventoryItemSelectedEventArgs(Guid itemId)
		{
			this.ItemId = itemId;
		}
	}
}