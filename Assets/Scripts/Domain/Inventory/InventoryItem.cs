using System;

namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItem
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
		public string ShortName { get; set; }
		public string Description { get; set; }

		public string SpriteName { get; set; }
		public string ThumbnailName { get; set; }

		public int Weight { get; set; }
		public int Power { get; set; }
		public int Energy { get; set; }

		public int Level { get; set; }
		public int Class { get; set; }
		public InventoryItemCategory Category { get; set; }

		public InventoryItemPrice Price { get; set; }
		public InventoryItemRarity Rarity { get; set; }

		public override string ToString ()
		{
			return string.Format ("[InventoryItem: Id={0}, Name={1}, ShortName={2}, Description={3}, SpriteName={4}, ThumbnailName={5}, Weight={6}, Power={7}, Energy={8}, Level={9}, Class={10}, Category={11}, Price={12}, Rarity={13}]", Id, Name, ShortName, Description, SpriteName, ThumbnailName, Weight, Power, Energy, Level, Class, Category, Price, Rarity);
		}
	}
}
