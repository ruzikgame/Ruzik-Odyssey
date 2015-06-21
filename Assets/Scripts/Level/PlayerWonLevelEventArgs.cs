using System;

namespace RuzikOdyssey.Level
{
	public sealed class PlayerWonLevelEventArgs : EventArgs
	{
		public int GoldEarned { get; set; }
		public int CornEarned { get; set; }
	}
}
