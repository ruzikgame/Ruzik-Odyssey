using System;

namespace RuzikOdyssey.Level
{
	public sealed class PlayerLostEventArgs : EventArgs
	{
		public int GoldEarned { get; set; }
		public int CornEarned { get; set; }
	}
}
