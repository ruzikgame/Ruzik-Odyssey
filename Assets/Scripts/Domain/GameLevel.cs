namespace RuzikOdyssey.Domain
{
	public sealed class GameLevel
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public bool IsLocked { get; set; }
		public int Medals { get; set; }
		public int MaxMedals { get; set; }
	}
}