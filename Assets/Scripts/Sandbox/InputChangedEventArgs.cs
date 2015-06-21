using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{

	public sealed class InputChangedEventArgs : EventArgs
	{
		public Vector2 Position { get; set; }
		public Vector2 Delta { get; set; }
	}

}
