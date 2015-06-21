using UnityEngine;
using RuzikOdyssey;
using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.UI.Views
{
	public sealed class CornStoreSceneView : ExtendedMonoBehaviour
	{
		public UILabel goldAmountLabel;
		public UILabel cornAmountLabel;
		
		private void Awake()
		{
			InitializeUi();
		}
		
		private void InitializeUi()
		{
			goldAmountLabel.text = GlobalModel.Gold.Value.ToString();
			cornAmountLabel.text = GlobalModel.Corn.Value.ToString();
		}
		
		private void SubscribeToEvent()
		{
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.GoldPropertyChanged, Gold_PropertyChanged);
			EventsBroker.Subscribe<PropertyChangedEventArgs<int>>(Events.Global.CornPropertyChanged, Corn_PropertyChanged);
		}
		
		private void Gold_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			goldAmountLabel.text = e.PropertyValue.ToString();
		}
		
		private void Corn_PropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			cornAmountLabel.text = e.PropertyValue.ToString();
		}
	}
}

