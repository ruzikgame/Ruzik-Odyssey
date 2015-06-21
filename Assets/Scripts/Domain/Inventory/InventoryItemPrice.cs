namespace RuzikOdyssey.Domain.Inventory
{
	public class InventoryItemPrice
	{
		public int Gold { get; set; }
		public int Corn { get; set; }

		public override string ToString ()
		{
			return string.Format ("[InventoryItemPrice: Gold={0}, Corn={1}]", Gold, Corn);
		}
	}
}
