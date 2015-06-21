using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;
using RuzikOdyssey.UI.Elements;
using RuzikOdyssey.ViewModels;
using System;
using RuzikOdyssey.Domain.Inventory;

namespace RuzikOdyssey.UI.Views
{
	public sealed class SmallItemsStoreSceneView : ExtendedMonoBehaviour
	{
		/// <summary>
		/// The default inventory items category that is show when the scene starts.
		/// </summary>
		private const InventoryItemCategory DefaultCategory = InventoryItemCategory.Weapons;

		public SmallItemsStoreSceneViewModel viewModel;

		public TextAsset storeItemsConfigFile;

		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;

		public GameObject[] highlightedTabs;
		public GameObject[] tabs;

		public GameObject storeItemsScrollView;
		public GameObject storeItemTemplate;

		public UISprite currentItemImage;
		public UILabel currentItemCaption;
		public UILabel itemDescription;
		public UILabel itemRarity;
		public UILabel itemClass;
		public UILabel itemStat1Name;
		public UILabel itemStat1Value;
		public UILabel itemStat2Name;
		public UILabel itemStat2Value;
		public UILabel itemStat3Name;
		public UILabel itemStat3Value;
		public UILabel itemStat4Name;
		public UILabel itemStat4Value;
		public UILabel itemGoldPrice;
		public UILabel itemCornPrice;

		private ICollection<InventoryItem> availableItems;

		private ItemsStoreTab selectedTab;
		private Guid selectedItemId;

		private void Awake()
		{

		}

		private void Start()
		{
			viewModel.AvailableItemsUpdated += ViewModel_AvailableItemsUpdated;
			viewModel.Gold.PropertyChanged += ViewModel_GoldPropertyChanged;
			viewModel.Corn.PropertyChanged += ViewModel_CornPropertyChanged;

			availableItems = viewModel.AvailableItems;
			goldAmountLabel.text = viewModel.Gold.Value.ToString();
			cornAmountLabel.text = viewModel.Corn.Value.ToString();

			this.ItemPurchased += viewModel.View_ItemPurchased;

			// Show default tab
			PopulateItemsForTab(null);
		}

		private void ViewModel_AvailableItemsUpdated(object sender, EventArgs e)
		{
			availableItems = viewModel.AvailableItems;

			RefreshItemsScrollView();
		}

		private void ViewModel_GoldPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = viewModel.Gold.Value.ToString();
		}

		private void ViewModel_CornPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = viewModel.Corn.Value.ToString();
		}

		private void ClearItemsScrollView()
		{
			storeItemsScrollView.DestroyAllChildren();
		}

		private void RefreshItemsScrollView()
		{
			ClearItemsScrollView();
			PopulateItemsForTab(selectedTab);
		}

		private void SelectItem(Guid itemId)
		{
			selectedItemId = itemId;

			var item = availableItems.SingleOrDefault(x => x.Id == itemId);

			if (item == null)
			{
				Log.Error("Failed to find an item with ID {0} in available items", itemId);
				return;
			}

			currentItemImage.spriteName = item.SpriteName;
			currentItemCaption.text = item.Name;
			itemDescription.text = item.Description;
			itemRarity.text = Enum.GetName(typeof(InventoryItemRarity), item.Rarity);
			itemClass.text = String.Format("Class {0}", item.Class);
			itemGoldPrice.text = item.Price.Gold.ToString();
			itemCornPrice.text = item.Price.Corn.ToString();

			switch (item.Category)
			{
				case InventoryItemCategory.Weapons:
					var weaponItem = (WeaponInventoryItem) item;
					itemStat1Name.text = "Fire Rate";
					itemStat1Value.text = weaponItem.FireRate.ToString();
					itemStat2Name.text = "Weight";
					itemStat2Value.text = weaponItem.Weight.ToString();
					itemStat3Name.text = String.Empty;
					itemStat3Value.text = String.Empty;
					itemStat4Name.text = String.Empty;
					itemStat4Value.text = String.Empty;
					break;

				case InventoryItemCategory.Engines:
					itemStat1Name.text = "Weight";
					itemStat1Value.text = item.Weight.ToString();
					itemStat2Name.text = "Power";
					itemStat2Value.text = String.Format("{0}{1}", item.Power >= 0 ? "+" : "-", item.Power);
					itemStat3Name.text = String.Empty;
					itemStat3Value.text = String.Empty;
					itemStat4Name.text = String.Empty;
					itemStat4Value.text = String.Empty;
					break;

				default:
					itemStat1Name.text = String.Empty;
					itemStat1Value.text = String.Empty;
					itemStat2Name.text = String.Empty;
					itemStat2Value.text = String.Empty;
					itemStat3Name.text = String.Empty;
					itemStat3Value.text = String.Empty;
					itemStat4Name.text = String.Empty;
					itemStat4Value.text = String.Empty;
					break;
			}
		}

		private void PopulateItemsForTab(ItemsStoreTab tab)
		{
			var selectedCateogry = tab != null ? tab.category : DefaultCategory;

			GameObject previousStoreItem = null;
			foreach (var item in availableItems.Where(x => x.Category == selectedCateogry))
			{
				var storeItem = NGUITools.AddChild(storeItemsScrollView, storeItemTemplate);
				storeItem.SingleChild().GetComponentOrThrow<UISprite>().spriteName = item.ThumbnailName;

				// Capture index for the anonimous delegate closure
				var capturedId = item.Id;
				UIEventListener.Get(storeItem.GetComponentOrThrow<UIButton>().gameObject).onClick += (obj) => 
				{ 
					SelectItem(capturedId); 
				};
				
				var storeItemSprite = storeItem.GetComponentOrThrow<UISprite>();
				
				storeItemSprite.topAnchor.target = storeItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				storeItemSprite.bottomAnchor.target = storeItemsScrollView.transform;
				storeItemSprite.topAnchor.absolute = 0;
				
				if (previousStoreItem == null) 
				{
					SelectItem(item.Id);
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
			
			storeItemsScrollView.GetComponentOrThrow<UIScrollView>().ResetPosition();
		}

		public void SelectTab(ItemsStoreTab tab)
		{
			ClearItemsScrollView();

			var tabIndex = tab.tabIndex;
			var selectedTabIndex = selectedTab != null ? selectedTab.tabIndex : 0;

			// De-highlight the currently selected tab
			highlightedTabs[selectedTabIndex].SetActive(false);
			tabs[selectedTabIndex].SetActive(true);

			// Anchor the tab next to the previously highlighted to the regular version of the tab
			if (selectedTabIndex < tabs.Length - 1) 
			{
				var tabOnRight = tabs[selectedTabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();

				var anchorSprite = tabs[selectedTabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = tabs[selectedTabIndex].transform;
				
				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}

			// Highlight the new tab
			highlightedTabs[tabIndex].SetActive(true);
			tabs[tabIndex].SetActive(false);

			// Anchor the next tab to the highlighted version of the tab
			if (tabIndex < tabs.Length - 1) 
			{
				var tabOnRight = tabs[tabIndex + 1];
				var tabOnRightSprite = tabOnRight.GetComponentOrThrow<UISprite>();

				var anchorSprite = highlightedTabs[tabIndex].GetComponentOrThrow<UISprite>();
				var anchorTransform = highlightedTabs[tabIndex].transform;

				tabOnRightSprite.leftAnchor.target = anchorTransform;
				tabOnRightSprite.leftAnchor.rect = anchorSprite;
			}

			selectedTab = tab;

			PopulateItemsForTab(selectedTab);
		}

		private void DeselectAllTabs()
		{
			foreach (var tab in highlightedTabs)
			{
				tab.SetActive(false);
			}

			foreach (var tab in tabs)
			{
				tab.SetActive(true);
			}
		}

		public event EventHandler<ItemPurchasedEventArgs> ItemPurchased;

		public void OnItemPurchased()
		{
			if (ItemPurchased != null) ItemPurchased(this, new ItemPurchasedEventArgs { ItemId = selectedItemId });
		}

		public void Game_BuyItemButtonClicked()
		{
			OnItemPurchased();
		}
	}


}

