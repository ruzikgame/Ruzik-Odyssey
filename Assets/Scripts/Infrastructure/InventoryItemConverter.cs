using System;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Domain.Inventory;
using RuzikOdyssey.Domain.Store;
using Newtonsoft.Json.Linq;
using RuzikOdyssey.Common;
using System.Linq;

namespace RuzikOdyssey.Infrastructure
{
	public class InventoryItemConverter : JsonCreationConverter<InventoryItem>
	{
#if UNITY_EDITOR
		private const string ItemCategoryKey = "category";
#elif UNITY_IOS
		private const string ItemCategoryKey = "category";
#else
		private const string ItemCategoryKey = "category";
#endif

		protected override InventoryItem Create(Type objectType, JObject jObject)
		{
			switch (RetrieveCategory(jObject))
			{
				case InventoryItemCategory.Weapons: return new WeaponInventoryItem();
				case InventoryItemCategory.Ammo: return new AmmoInventoryItem();
				case InventoryItemCategory.Fuselage: return new ArmorInventoryItem();
				default: return new InventoryItem();
			}
		}

		private InventoryItemCategory RetrieveCategory(JObject jObject)
		{
			if (jObject[ItemCategoryKey] == null)
			{
				Log.Error("Serialized object misses '{0}' key. Object: \n{1}", 
				          ItemCategoryKey, jObject.ToString());
				return default(InventoryItemCategory);
			}

			return jObject[ItemCategoryKey].ToObject<InventoryItemCategory>();
		}
	}

	public class StoreItemConverter : JsonCreationConverter<StoreItem>
	{
#if UNITY_EDITOR
		private const string ItemCategoryKey = "category";
#elif UNITY_IOS
		private const string ItemCategoryKey = "category";
#else
		private const string ItemCategoryKey = "category";
#endif

		protected override StoreItem Create(Type objectType, JObject jObject)
		{
			switch (RetrieveCategory(jObject))
			{
				case StoreItemCategory.Aircrafts: return new AircraftStoreItem();
				default: return new StoreItem();
			}
		}
		
		private StoreItemCategory RetrieveCategory(JObject jObject)
		{
			if (jObject[ItemCategoryKey] == null)
			{
				Log.Error("Serialized object misses '{0}' key. Object: \n{1}", 
				          ItemCategoryKey, jObject.ToString());
				return default(StoreItemCategory);
			}
			
			return jObject[ItemCategoryKey].ToObject<StoreItemCategory>();
		}
	}
}
