using System.Collections.Generic;
using System;

namespace RuzikOdyssey.UI.Elements
{
	[Obsolete]
	public sealed class StoreItemsCategory
	{
		public string Name { get; set; }
		public IList<UIStoreItem> Items { get; set; }
	}
}
