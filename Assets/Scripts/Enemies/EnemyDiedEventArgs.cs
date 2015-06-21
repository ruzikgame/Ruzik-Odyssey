using System;

namespace RuzikOdyssey.Enemies
{
	public sealed class EnemyDiedEventArgs : EventArgs
	{
		public int ScoreForKill { get; set; }
	}
}
