using System.Collections.Generic;

namespace RuzikOdyssey.Domain.Store
{
	public class GameStore
	{
		public IList<StoreItem> Gold { get; set; }
		public IList<StoreItem> Corn { get; set; }
		public IList<StoreItem> Aircrafts { get; set; }
	}
}