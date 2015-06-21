using System.Collections.Generic;
using System;

namespace RuzikOdyssey.Domain.Store
{
	public class StoreItem 
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string SpriteName { get; set; }
		public StoreItemPrice Price { get; set; }
		public StoreItemCategory Category { get; set; }
	}

	public class AircraftStoreItem : StoreItem
	{
		public int Weight { get; set; }
		public string ThumbnailName { get; set; }
	}
}