using RuzikOdyssey.Common;
using RuzikOdyssey.Domain.Inventory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RuzikOdyssey.ViewModels
{
	public sealed class SmallItemsStoreSceneViewModel : ViewModel
	{
		private ICollection<InventoryItem> availableItems = new List<InventoryItem>();
		public ICollection<InventoryItem> AvailableItems 
		{ 
			get { return availableItems; } 
			private set
			{
				availableItems = value;
				OnAvailableItemsUpdated();
			}
		}

		public event EventHandler AvailableItemsUpdated;

		protected override void Start ()
		{
			base.Start();

			AvailableItems = GlobalModel.Inventory.AvailableItems;
		}

		private void OnAvailableItemsUpdated()
		{
			if (AvailableItemsUpdated != null) AvailableItemsUpdated(this, EventArgs.Empty);
		}

		public void View_ItemPurchased(object sender, ItemPurchasedEventArgs e)
		{
			Log.Debug("BEFORE - Available items: {0}, purchased items: {1}", 
			          GlobalModel.Inventory.AvailableItems.Count, GlobalModel.Inventory.PurchasedItems.Count);

			var item = GlobalModel.Inventory.AvailableItems.FirstOrDefault(x => x.Id == e.ItemId);

			if (item == null)
			{
				Log.Error("Failed to purchase item with ID {0}. Failed to find item in available items.",
				          e.ItemId);
				return;
			}

			if (!VerifySufficientFunds(item.Price)) return;

			var itemRemoved = GlobalModel.Inventory.AvailableItems.Remove(item);

			if (!itemRemoved)
			{
				Log.Error("Failed to purchase item with ID {0}. Failed to remove item from available items.",
				          e.ItemId);
				return;
			}

			OnAvailableItemsUpdated();

			Charge(item.Price);

			GlobalModel.Inventory.PurchasedItems.Add(item);

			Log.Debug("AFTER - Available items: {0}, purchased items: {1}", 
			          GlobalModel.Inventory.AvailableItems.Count, GlobalModel.Inventory.PurchasedItems.Count);
		}

		private bool VerifySufficientFunds(InventoryItemPrice price)
		{
			return (GlobalModel.Gold.Value >= price.Gold && GlobalModel.Corn.Value >= price.Corn);
		}

		private void Charge(InventoryItemPrice price)
		{
			GlobalModel.Gold.Value -= price.Gold;
			GlobalModel.Corn.Value -= price.Corn;
		}
	}
}
