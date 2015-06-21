using UnityEngine;
using RuzikOdyssey.Common;
using RuzikOdyssey;
using RuzikOdyssey.Level;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using RuzikOdyssey.UI.Elements;
using RuzikOdyssey.Domain.Inventory;
using System;
using RuzikOdyssey.ViewModels;

namespace RuzikOdyssey.UI.Views
{
	public sealed class HangarSceneView : ExtendedMonoBehaviour
	{
		/// <summary>
		/// The default inventory items category that is show when the scene starts.
		/// </summary>
		private const InventoryItemCategory DefaultInventoryCategory = InventoryItemCategory.Weapons;

		public HangarSceneViewModel viewModel;

		public GameObject[] purchasedItemsHighlightedTabs;
		public GameObject[] purchasedItemsTabs;
		
		public GameObject purchasedItemsScrollView;
		public GameObject equippedItemsScrollView;

		public GameObject purchasedItemTemplate;
		public GameObject equippedItemTemplate;

		public GameObject itemDescriptionPopup;
		public InventoryItemDescription itemDescription;

		public GameObject storePopup;
		public GameObject popupsContainer;
		
		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;
		public UILabel gasAmountLabel;
		
		public UILabel storeGoldAmountLabel;
		public UILabel storeCornAmountLabel;

		public UITexture aircraftTexture;

		private ICollection<InventoryItem> purchasedItems;
		private ICollection<InventoryItem> equippedItems;
		
		private ItemsStoreTab selectedTab;

		private Guid selectedItemId;
		private bool equippedItemSelected;

		public GameObject videoAdButton;
		
		private void Awake()
		{
			
		}
		
		private void Start()
		{
			videoAdButton.SetActive(Vungle.isAdvertAvailable());
			Vungle.onCachedAdAvailableEvent += Vungle_OnCachedAdAvailableEventHandler;

			viewModel.PurchasedItemsUpdated += ViewModel_PurchasedItemsUpdated;
			viewModel.EquippedItemsUpdated += ViewModel_EquippedItemsUpdated;
			viewModel.AircraftInfoChanged += ViewModel_AircraftInfoChanged;

			purchasedItems = viewModel.PurchasedItems;
			equippedItems = viewModel.EquippedItems;

			this.ItemStateChanged += viewModel.View_ItemStateChanged;
			this.ItemUpgraded += viewModel.View_ItemUpgraded;
			this.AircraftSelected += viewModel.View_AircraftSelected;

			goldAmountLabel.BindTo(viewModel.Gold);
			cornAmountLabel.BindTo(viewModel.Corn);
			gasAmountLabel.BindTo(viewModel.Gas).WithFormat("{0}/10");

			storeGoldAmountLabel.BindTo(viewModel.Gold);
			storeCornAmountLabel.BindTo(viewModel.Corn);

			// Show default tab
			PopulatePurchasedItems(null);
			PopulateEquippedItems();

			popupsContainer.SetActive(false);
		}

		public event EventHandler<InventoryItemStateChangedEventArgs> ItemStateChanged;

		public void OnItemStateChanged(Guid itemId, InventoryItemState oldState, InventoryItemState newState)
		{
			if (ItemStateChanged != null) 
				ItemStateChanged(this, new InventoryItemStateChangedEventArgs
         		{
					ItemId = itemId,
					OldState = oldState,
					NewState = newState
				});
		}

		public event EventHandler<InventoryItemSelectedEventArgs> AircraftSelected;

		public void OnAircraftSelected(Guid aircraftItemId)
		{
			Log.Debug("START - OnAircraftSelected");

			if (AircraftSelected != null) 
				AircraftSelected(this, new InventoryItemSelectedEventArgs(aircraftItemId));
		}
		
		public void Game_EquipItemButtonClicked()
		{
			SetItemDescriptionVisible(false);

			OnItemStateChanged(selectedItemId, 
			                   equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased,
			                   equippedItemSelected ? InventoryItemState.Purchased : InventoryItemState.Equipped);
		}

		private void ViewModel_PurchasedItemsUpdated(object sender, EventArgs e)
		{
			purchasedItems = viewModel.PurchasedItems;
			
			RefreshPurchasedItemsScrollView();
		}

		private void ViewModel_EquippedItemsUpdated(object sender, EventArgs e)
		{
			equippedItems = viewModel.EquippedItems;

			RefreshEquippedItemsScrollView();
		}

		private void ViewModel_AircraftInfoChanged(object sender, EventArgs e)
		{
			Log.Debug("START - ViewModel_AircraftInfoChanged");

			if (aircraftTexture.mainTexture != null)
			{
				Resources.UnloadAsset(aircraftTexture.mainTexture);
			}
			
			var aircraftTexturePath = String.Format("Aircrafts/Aircraft_{0}", GlobalModel.Aircraft.Value.Ui.SceneSpriteName);
			aircraftTexture.mainTexture = Resources.Load(aircraftTexturePath) as Texture2D;
		}
		
		private void ClearPurchasedItemsScrollView()
		{
			purchasedItemsScrollView.DestroyAllChildren();
		}

		private void ClearEquippedItemsScrollView()
		{
			equippedItemsScrollView.DestroyAllChildren();
		}
		
		private void RefreshPurchasedItemsScrollView()
		{
			ClearPurchasedItemsScrollView();
			PopulatePurchasedItems(selectedTab);
		}

		private void RefreshEquippedItemsScrollView()
		{
			ClearEquippedItemsScrollView();
			PopulateEquippedItems();
		}

		public void Game_PurchasedItemSelected(Guid itemId)
		{
			SelectItem(itemId, false);
			SetItemDescriptionVisible(true);
		}

		public void Game_EquippedItemSelected(Guid itemId)
		{
			SelectItem(itemId, true);
			SetItemDescriptionVisible(true);
		}

		private void SelectItem(Guid itemId, bool itemIsEquipped)
		{
			selectedItemId = itemId;
			equippedItemSelected = itemIsEquipped;

			var item = equippedItemSelected 
				? equippedItems.FirstOrDefault(x => x.Id == selectedItemId) 
				: purchasedItems.FirstOrDefault(x => x.Id == selectedItemId);

			if (item == null)
			{
				Log.Error("Failed to select item with ID {0}. Failed to find item in {1} items.",
				          selectedItemId, equippedItemSelected ? "equipped" : "purchased");
				return;
			}

			itemDescription.equipButtonTitle.text = equippedItemSelected ? "Unequip" : "Equip";

			itemDescription.itemName.text = item.Name;
			itemDescription.image.spriteName = item.SpriteName;

			itemDescription.rarity.text = Enum.GetName(typeof(InventoryItemCategory), item.Rarity);
			itemDescription.itemClass.text = String.Format("Class {0}", item.Class);

			itemDescription.price.gold.text = (item.Price.Gold / 2).ToString();
			itemDescription.level.text = item.Level.ToString();

			itemDescription.upgradePrice.gold.text = (1).ToString();

			switch (item.Category)
			{
				case InventoryItemCategory.Weapons:
					var weaponItem = (WeaponInventoryItem) item;

					itemDescription.feature1.featureName.text = "Fire Rate";
					itemDescription.feature1.value.text = weaponItem.FireRate.ToString();
					itemDescription.feature1.improvement.text = "+0";

					itemDescription.feature2.featureName.text = "Weight";
					itemDescription.feature2.value.text = weaponItem.Weight.ToString();
					itemDescription.feature2.improvement.text = "+0";
							
					itemDescription.feature3.gameObject.SetActive(false);
					itemDescription.feature4.gameObject.SetActive(false);
					break;
					
				case InventoryItemCategory.Engines:
					itemDescription.feature1.featureName.text = "Weight";
					itemDescription.feature1.value.text = item.Weight.ToString();
					itemDescription.feature1.improvement.text = "+0";

					itemDescription.feature2.featureName.text = "Power";
					itemDescription.feature2.value.text = String.Format("{0}{1}", item.Power >= 0 ? "+" : "-", item.Power);
					itemDescription.feature2.improvement.text = "+0";
						
					itemDescription.feature3.gameObject.SetActive(false);
					itemDescription.feature4.gameObject.SetActive(false);
					break;
					
				default:
					HideAllItemDescriptionFeatures();
					break;
			}
		}

		private void HideAllItemDescriptionFeatures()
		{
			itemDescription.feature1.gameObject.SetActive(false);
			itemDescription.feature2.gameObject.SetActive(false);
			itemDescription.feature3.gameObject.SetActive(false);
			itemDescription.feature4.gameObject.SetActive(false);
		}

		public void Game_CloseItemDescriptionButtonClicked()
		{
			SetItemDescriptionVisible(false);
		}

		public void Game_SellItemButtonClicked()
		{
			SetItemDescriptionVisible(false);

			OnItemStateChanged(selectedItemId, 
			                   equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased,
			                   InventoryItemState.Available);
		}

		public void Game_UpgradeItemButtonClicked()
		{
			OnItemUpgraded(selectedItemId, equippedItemSelected ? InventoryItemState.Equipped : InventoryItemState.Purchased);
		}

		private void OnItemUpgraded(Guid id, InventoryItemState state)
		{
			if (ItemUpgraded != null) ItemUpgraded(this, new InventoryItemUpgradedEventArgs(id, state));
		}

		public event EventHandler<InventoryItemUpgradedEventArgs> ItemUpgraded;

		private void SetItemDescriptionVisible(bool isVisible)
		{
			popupsContainer.SetActive(isVisible);
			itemDescriptionPopup.SetActive(isVisible);
			storePopup.SetActive(!isVisible);

			purchasedItemsScrollView.SetActive(!isVisible);
			equippedItemsScrollView.SetActive(!isVisible);
		}

		public void Game_AircraftSelected(Guid aircraftItemId)
		{
			Log.Debug("START - Game_AircraftSelected");

			var item = purchasedItems
				.SingleOrDefault(x => x.Category == InventoryItemCategory.Aircraft && x.Id == aircraftItemId);
			
			if (item == null)
			{
				Log.Error("Failed to select aircraft with ID {0}. Failed to find item in purchased items.",
				          aircraftItemId);
				return;
			}

			OnAircraftSelected(aircraftItemId);
		}

		private void PopulateEquippedItems()
		{
			GameObject previousEquippedItem = null;
			foreach (var item in equippedItems)
			{
				var equippedItem = NGUITools.AddChild(equippedItemsScrollView, equippedItemTemplate);
				var largeInventoryItem = equippedItem.GetComponentOrThrow<LargeInventoryItem>();
				
				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(equippedItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					Game_EquippedItemSelected(capturedId); 
				};

				largeInventoryItem.thumbnail.spriteName = item.ThumbnailName;
				largeInventoryItem.classLabel.text = String.Format("C{0}", item.Class);
				largeInventoryItem.levelLabel.text = String.Format("L{0}", item.Level);
				largeInventoryItem.rarityLabel.text = Enum.GetName(typeof(InventoryItemCategory), item.Rarity);

				// TODO: Use generic UIRect for anchoring
				var equippedItemTexture = equippedItem.GetComponentOrThrow<UITexture>();
				
				equippedItemTexture.leftAnchor.target = equippedItemsScrollView.transform;
				equippedItemTexture.leftAnchor.absolute = 0;
				
				equippedItemTexture.rightAnchor.target = equippedItemsScrollView.transform;
				equippedItemTexture.rightAnchor.absolute = 0;
				
				if (previousEquippedItem == null) 
				{
					// Most left item in the scroll view
				}
				else
				{
					equippedItemTexture.topAnchor.target = previousEquippedItem.transform;
					equippedItemTexture.topAnchor.absolute = 0;
					equippedItemTexture.topAnchor.relative = 0;
				}
				
				equippedItemTexture.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				equippedItemTexture.ResetAndUpdateAnchors();
				
				previousEquippedItem = equippedItem;
			}
			
			equippedItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}

		private void PopulatePurchasedItems(InventoryItemCategory itemsCategory)
		{
			Log.Debug("Selected category {0}", Enum.GetName(typeof(InventoryItemCategory), itemsCategory));
			
			Log.Debug("Purchased items of category {0}: {1}",
			          Enum.GetName(typeof(InventoryItemCategory), itemsCategory), 
			          purchasedItems.Where(x => x.Category == itemsCategory).Count());
			
			GameObject previousStoreItem = null;
			foreach (var item in purchasedItems.Where(x => x.Category == itemsCategory))
			{
				var storeItem = NGUITools.AddChild(purchasedItemsScrollView, purchasedItemTemplate);
				storeItem.SingleChild().GetComponentOrThrow<UISprite>().spriteName = item.ThumbnailName;

				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(storeItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					if (item.Category == InventoryItemCategory.Aircraft) Game_AircraftSelected(capturedId);
					else Game_PurchasedItemSelected(capturedId); 
				};

				var storeItemSprite = storeItem.GetComponentOrThrow<UISprite>();
				
				storeItemSprite.topAnchor.target = purchasedItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				storeItemSprite.bottomAnchor.target = purchasedItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				if (previousStoreItem == null) 
				{
					// Most left item in the scroll view
				}
				else
				{
					storeItemSprite.leftAnchor.target = previousStoreItem.transform;
					storeItemSprite.leftAnchor.absolute = 0;
					storeItemSprite.leftAnchor.relative = 1f;
				}
				
				storeItemSprite.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
				
				storeItemSprite.ResetAndUpdateAnchors();
				
				previousStoreItem = storeItem;
			}
			
			purchasedItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}

		private void PopulatePurchasedItems(ItemsStoreTab tab)
		{
			var selectedCategory = tab != null ? tab.category : DefaultInventoryCategory;
			PopulatePurchasedItems(selectedCategory);
		}
		
		public void Game_PurchasedItemsTabSelected(ItemsStoreTab tab)
		{
			ClearPurchasedItemsScrollView();
			
			var tabIndex = tab.tabIndex;
			var selectedTabIndex = selectedTab != null ? selectedTab.tabIndex : 0;
			
			// De-highlight the currently selected tab
			purchasedItemsHighlightedTabs[selectedTabIndex].SetActive(false);
			purchasedItemsTabs[selectedTabIndex].SetActive(true);
			
			// Anchor the tab next to the previously highlighted to the regular version of the tab
			if (selectedTabIndex < purchasedItemsTabs.Length - 1) 
			{
				// TODO: Use generic UIRect for anchoring
				var tabOnRight = purchasedItemsTabs[selectedTabIndex + 1];
				var tabOnRightTexture = tabOnRight.GetComponentOrThrow<UITexture>();
				
				var anchorTexture = purchasedItemsTabs[selectedTabIndex].GetComponentOrThrow<UITexture>();
				var anchorTransform = purchasedItemsTabs[selectedTabIndex].transform;
				
				tabOnRightTexture.leftAnchor.target = anchorTransform;
				tabOnRightTexture.leftAnchor.rect = anchorTexture;
			}
			
			// Highlight the new tab
			purchasedItemsHighlightedTabs[tabIndex].SetActive(true);
			purchasedItemsTabs[tabIndex].SetActive(false);
			
			// Anchor the next tab to the highlighted version of the tab
			if (tabIndex < purchasedItemsTabs.Length - 1) 
			{
				// TODO: Use generic UIRect for anchoring
				var tabOnRight = purchasedItemsTabs[tabIndex + 1];
				var tabOnRightTexture = tabOnRight.GetComponentOrThrow<UITexture>();
				
				var anchorTexture = purchasedItemsHighlightedTabs[tabIndex].GetComponentOrThrow<UITexture>();
				var anchorTransform = purchasedItemsHighlightedTabs[tabIndex].transform;
				
				tabOnRightTexture.leftAnchor.target = anchorTransform;
				tabOnRightTexture.leftAnchor.rect = anchorTexture;
			}
			
			selectedTab = tab;

			Log.Debug("Selected tab {0}", selectedTab.tabIndex);
			
			PopulatePurchasedItems(selectedTab);
		}
		
		private void DeselectAllTabs()
		{
			foreach (var tab in purchasedItemsHighlightedTabs)
			{
				tab.SetActive(false);
			}
			
			foreach (var tab in purchasedItemsTabs)
			{
				tab.SetActive(true);
			}
		}

		public void Game_OnCloseStorePopupButtonClicked()
		{
			SetStorePopUpVisible(false);
		}

		public void Game_OnStoreButtonClicked()
		{
			SetStorePopUpVisible(true);
		}

		private void SetStorePopUpVisible(bool isVisible)
		{
			popupsContainer.SetActive(isVisible);
			itemDescriptionPopup.SetActive(!isVisible);
			storePopup.SetActive(isVisible);

			purchasedItemsScrollView.SetActive(!isVisible);
			equippedItemsScrollView.SetActive(!isVisible);
		}

		public void Game_AircraftsTabSelected()
		{
			ClearPurchasedItemsScrollView();
			
			var selectedTabIndex = selectedTab != null ? selectedTab.tabIndex : 0;
			
			// De-highlight the currently selected tab
			purchasedItemsHighlightedTabs[selectedTabIndex].SetActive(false);
			purchasedItemsTabs[selectedTabIndex].SetActive(true);
			
			// Anchor the tab next to the previously highlighted to the regular version of the tab
			if (selectedTabIndex < purchasedItemsTabs.Length - 1) 
			{
				// TODO: Use generic UIRect for anchoring
				var tabOnRight = purchasedItemsTabs[selectedTabIndex + 1];
				var tabOnRightTexture = tabOnRight.GetComponentOrThrow<UITexture>();
				
				var anchorTexture = purchasedItemsTabs[selectedTabIndex].GetComponentOrThrow<UITexture>();
				var anchorTransform = purchasedItemsTabs[selectedTabIndex].transform;
				
				tabOnRightTexture.leftAnchor.target = anchorTransform;
				tabOnRightTexture.leftAnchor.rect = anchorTexture;
			}
			
			Log.Debug("Selected tab {0}", 6);
			
			PopulatePurchasedItems(InventoryItemCategory.Aircraft);
		}

		public void Game_ShowVideoAdButtonClicked()
		{
			Log.Debug("START - Game_ShowVideoAdButtonClicked");

			if (Vungle.isAdvertAvailable())
			{
				Log.Info("Playing video ad from Vungle.");
				Vungle.playAd(true, "generic-user");
			}
			else 
			{
				Log.Warning("Failed to play video ad. Ad is not available.");
			}
		}

		private void Vungle_OnCachedAdAvailableEventHandler()
		{
			Log.Debug("START - Vungle_OnCachedAdAvailableEventHandler");
			videoAdButton.SetActive(true);
		}
	}
}

