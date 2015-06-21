using System;

namespace RuzikOdyssey.Common
{
	public class ProgressUpdatedEventArgs : EventArgs
	{
		public string ActionName { get; set; }
		public int ActionProgress { get; set; }
	}
}
