using RuzikOdyssey.Common;
using RuzikOdyssey.Domain.Inventory;
using System.Collections.Generic;
using System;
using System.Linq;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.ViewModels
{
	public sealed class HangarSceneViewModel : ViewModel
	{
		private ICollection<InventoryItem> purchasedItems = new List<InventoryItem>();
		private ICollection<InventoryItem> equippedItems = new List<InventoryItem>();

		public ICollection<InventoryItem> PurchasedItems 
		{ 
			get { return purchasedItems; } 
			private set
			{
				purchasedItems = value;
				OnPurchasedItemsUpdated();
			}
		}

		public ICollection<InventoryItem> EquippedItems
		{
			get { return equippedItems; }
			private set
			{
				equippedItems = value;
				OnEquippedItemsUpdated();
			}
		}

		private AircraftInfo aircraft = new AircraftInfo();
		public AircraftInfo Aircraft
		{
			get { return aircraft; }
			private set 
			{
				aircraft = value;
				OnAircraftInfoChanged();
			}
		}
		
		public event EventHandler PurchasedItemsUpdated;
		public event EventHandler EquippedItemsUpdated;
		public event EventHandler AircraftInfoChanged;
		
		protected override void Start ()
		{
			base.Start();
			
			PurchasedItems = GlobalModel.Inventory.PurchasedItems;
			EquippedItems = GlobalModel.Inventory.EquippedItems;
			Aircraft = GlobalModel.Aircraft;

			GlobalModel.Aircraft.PropertyChanged += GlobalModel_AircraftInfoPropertyChanged;

			Log.Debug("HangarSceneViewModel - START. Purchased items: {0}, Equipped items: {1}",
			          PurchasedItems.Count, EquippedItems.Count);
		}

		private void GlobalModel_AircraftInfoPropertyChanged(object sender, PropertyChangedEventArgs<AircraftInfo> e)
		{
			Log.Debug("START - GlobalModel_AircraftInfoPropertyChanged");

			this.Aircraft = e.PropertyValue;

			Log.Debug("Aircraft Info. ViewModel: {0}, GlobalModel: {1}",
			          this.Aircraft.Ui.SceneSpriteName, GlobalModel.Aircraft.Value.Ui.SceneSpriteName);
		}

		private void OnPurchasedItemsUpdated()
		{
			if (PurchasedItemsUpdated != null) PurchasedItemsUpdated(this, EventArgs.Empty);
		}

		private void OnEquippedItemsUpdated()
		{
			if (EquippedItemsUpdated != null) EquippedItemsUpdated(this, EventArgs.Empty);
		}

		private void OnAircraftInfoChanged()
		{
			Log.Debug("START - OnAircraftInfoChanged");

			if (AircraftInfoChanged != null) AircraftInfoChanged(this, EventArgs.Empty);
		}

		public void View_AircraftSelected(object sender, InventoryItemSelectedEventArgs e)
		{
			Log.Debug("START - View_AircraftSelected");

			var selectedAircraftItem = GlobalModel.Inventory.PurchasedItems.SingleOrDefault(x => x.Id == e.ItemId);

			if (selectedAircraftItem == null)
			{
				Log.Error("Failed to find an aircraft with ID {0} in the purchased inventory items",
				          e.ItemId);
				return;
			}

			GlobalModel.Aircraft.Value = new AircraftInfo
			{
				Ui = new AircraftInfo.AircraftUiInfo
				{
					SceneSpriteName = selectedAircraftItem.SpriteName,
				}
			};
		}

		public void View_ItemUpgraded(object sender, InventoryItemUpgradedEventArgs e)
		{
			var item = GetInventoryItemsCollection(e.ItemState).FirstOrDefault(x => x.Id == e.ItemId);
			
			if (item == null)
			{
				Log.Error("Failed to upgrade item with ID {0}. Failed to find item in {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.ItemState));
				return;
			}

			item.Level += 1;
		}

		public void View_ItemStateChanged(object sender, InventoryItemStateChangedEventArgs e)
		{
			Log.Debug("BEFORE - Purchased items: {0}, Equipped items: {1}", 
			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);

			var item = GetInventoryItemsCollection(e.OldState).FirstOrDefault(x => x.Id == e.ItemId);

			if (item == null)
			{
				Log.Error("Failed to change state for item with ID {0}. Failed to find item in {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.OldState));
				return;
			}

			var itemRemoved = GetInventoryItemsCollection(e.OldState).Remove(item);

			if (!itemRemoved)
			{
				Log.Error("Failed to change state for item with ID {0}. Failed to remove item from {1} items.",
				          e.ItemId, Enum.GetName(typeof(InventoryItemState), e.OldState));
				return;
			}

			TriggerCollectionUpdatedEvent(e.OldState);

			GetInventoryItemsCollection(e.NewState).Add(item);

			TransferFunds(item, e.OldState, e.NewState);
			TriggerCollectionUpdatedEvent(e.NewState);

			Log.Debug("AFTER - Purchased items: {0}, Equipped items: {1}", 
			          GlobalModel.Inventory.PurchasedItems.Count, GlobalModel.Inventory.EquippedItems.Count);
		}

		private ICollection<InventoryItem> GetInventoryItemsCollection(InventoryItemState state)
		{
			switch (state)
			{
				case InventoryItemState.Available: return GlobalModel.Inventory.AvailableItems;
				case InventoryItemState.Purchased: return GlobalModel.Inventory.PurchasedItems;
				case InventoryItemState.Equipped: return GlobalModel.Inventory.EquippedItems;
				default: return new List<InventoryItem>();
			}
		}

		private void TriggerCollectionUpdatedEvent(InventoryItemState state)
		{
			switch (state)
			{
				case InventoryItemState.Purchased: 
					OnPurchasedItemsUpdated();
					break;
				case InventoryItemState.Equipped: 
					OnEquippedItemsUpdated();
					break;
			}
		}

		private void TransferFunds(InventoryItem item, InventoryItemState oldState, InventoryItemState newState)
		{
			if (newState == InventoryItemState.Available && oldState != InventoryItemState.Available)
			{
				GlobalModel.Gold.Value += item.Price.Gold / 2;
			}
		}
	}
}

