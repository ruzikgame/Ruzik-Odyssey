using RuzikOdyssey.Domain.Store;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using System;
using System.Linq;
using RuzikOdyssey.Domain.Inventory;

namespace RuzikOdyssey.ViewModels
{
	public class StoreSceneViewModel : ViewModel
	{
		public static StoreItemCategory StoreCategory { get; set; }

		private IList<StoreItem> availableStoreItems = new List<StoreItem>();
		public IList<StoreItem> AvailableStoreItems 
		{ 
			get { return this.availableStoreItems; } 
			private set 
			{
				this.availableStoreItems = value;
				OnAvailableItemsUpdated();
			} 
		}

		public event EventHandler AvailableStoreItemsUpdated;
		
		private void OnAvailableItemsUpdated()
		{
			if (AvailableStoreItemsUpdated != null) AvailableStoreItemsUpdated(this, EventArgs.Empty);
		}

		protected override void Start ()
		{
			base.Start();

			this.AvailableStoreItems = RetrieveAvailableStoreItems(StoreCategory);
		}

		private IList<StoreItem> RetrieveAvailableStoreItems(StoreItemCategory category)
		{
			switch (category)
			{
				case StoreItemCategory.Aircrafts: return GlobalModel.Store.Aircrafts;
				case StoreItemCategory.Gold: return GlobalModel.Store.Gold;
				case StoreItemCategory.Corn: return GlobalModel.Store.Corn;
				default:
					Log.Error("Failed to retrieve store items for category {0}.", EnumUtility.GetName(category));
					return new List<StoreItem>();
			}
		}

		public void View_StoreItemPurchased(object sender, ItemActedOnEventArgs e)
		{
			var storeItem = AvailableStoreItems.SingleOrDefault(x => x.Id == e.ItemId);

			if (storeItem == null)
			{
				Log.Error("Failed to find an item in {0} store with ID {1}.", StoreCategory, e.ItemId);
				return;
			}

			if (storeItem.Category == StoreItemCategory.Aircrafts)
			{
				var aircarftStoreItem = (AircraftStoreItem)storeItem;

				var aircraftInventoryItem = new InventoryItem
				{
					Id = aircarftStoreItem.Id,
					Category = InventoryItemCategory.Aircraft,
					Name = aircarftStoreItem.Name,
					SpriteName = aircarftStoreItem.SpriteName,
					ThumbnailName = aircarftStoreItem.ThumbnailName,
				};

				// TODO: Should not be allowed to purchased in the first place
				if (!GlobalModel.Inventory.PurchasedItems.Any(x => x.Id == aircarftStoreItem.Id))
				{
					GlobalModel.Inventory.PurchasedItems.Add(aircraftInventoryItem);
				}
			}
		}
	}
}
