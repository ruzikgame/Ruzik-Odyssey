using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class ItemPurchasedEventArgs : EventArgs
	{
		public Guid ItemId { get; set; }
	}
}