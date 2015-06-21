using System;

namespace RuzikOdyssey.Common
{
	public sealed class Lazy<T>
	{
		private T backingField;
		private bool isInitialized;
		private readonly Func<T> valueFactory;
		
		public T Value
		{
			get
			{
				if (!isInitialized)
				{
					backingField = valueFactory();
					isInitialized = true;
				}
				return backingField;
			}
			private set 
			{ 
				backingField = value; 
			}
		}
		
		public Lazy(Func<T> valueFactory)
		{
			this.valueFactory = valueFactory;
		}
	}
}