using System;
using System.Collections.Generic;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.Level
{
	public class WaveTemplatesCollectionIterator
	{
		private int nextGroupIndex = 0;
		private IList<WaveTemplate> collection;

		public WaveTemplatesCollectionIterator(IList<WaveTemplate> collection)
		{
			this.collection = collection;

			collection.Add(WaveTemplate.WaitForSeconds(2.0f));
		}

		public virtual WaveTemplate GetNext()
		{
			if (nextGroupIndex >= collection.Count) return null;
			
			var group = collection[nextGroupIndex];
			nextGroupIndex++;
			
			if (nextGroupIndex == collection.Count) 
			{
				OnLastItemReturned();
			}
			
			return group;
		}
				
		public event EventHandler<EventArgs> LastItemReturned;
		
		protected virtual void OnLastItemReturned()
		{
			if (LastItemReturned != null) LastItemReturned(this, EventArgs.Empty);
		}
	}
}
