using UnityEngine;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Common
{
	public class Property<T>
	{
		private T value;
		private string name;

		public T Value 
		{ 
			get { return this.value; }
			set 
			{
				this.value = value;
				OnPropertyChanged();
			}
		}


		private EventHandler<PropertyChangedEventArgs<T>> propertyChangedDelegate;
		/// <summary>
		/// Occurs when the property value is changed.
		/// </summary>
		/// <remarks>
		/// Explicit Add and Remove methods are necessary to make the class work 
		/// on iOS devices. Compiler-generated event causes the app to crash randomly.
		/// </remarks>
		public event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged
		{
			add { propertyChangedDelegate += value; }
			remove { propertyChangedDelegate -= value; }
		}

		public Property() : this(default(T)) { }

		public Property(string name, bool publish = false) : this(default(T), name, publish) { }

		public Property(T value, string name = "", bool publish = false)
		{
			this.name = name;

			if (publish) PublishEvents();

			this.value = value;
		}

		protected virtual void OnPropertyChanged()
		{
			if (propertyChangedDelegate != null) 
				propertyChangedDelegate(this, new PropertyChangedEventArgs<T> { PropertyValue = this.Value });
		}

		private void PublishEvents()
		{
			EventsBroker.Publish<PropertyChangedEventArgs<T>>(
				String.Format("{0}_PropertyChanged", this.name), 
				ref propertyChangedDelegate);
		}

		public static implicit operator T(Property<T> property)
		{
			return property.Value;
		}

		public static implicit operator Property<T>(T value)
		{
			return new Property<T>(value);
		}
	}


}