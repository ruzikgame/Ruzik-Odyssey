using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Common
{
	public class PropertyChangedEventArgs<T> : EventArgs
	{
		public T PropertyValue { get; set; }
	}
}