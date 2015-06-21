using RuzikOdyssey.Common;

namespace RuzikOdyssey.ViewModels
{
	public abstract class ViewModel : ExtendedMonoBehaviour
	{
		public Property<int> Gold { get; private set; }
		public Property<int> Corn { get; private set; }
		public Property<int> Gas { get; private set; }

		protected virtual void Awake()
		{
			Gold = new Property<int>();
			Corn = new Property<int>();
			Gas = new Property<int>();

			GlobalModel.Connect();
		}

		protected virtual void Start()
		{
			GlobalModel.Gold.PropertyChanged += GlobalModel_GoldPropertyChanged;
			GlobalModel.Corn.PropertyChanged += GlobalModel_CornPropertyChanged;

			Gold.Value = GlobalModel.Gold.Value;
			Corn.Value = GlobalModel.Corn.Value;
			Gas.Value = GlobalModel.Gas.Value;
		}

		private void GlobalModel_GoldPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			Gold.Value = GlobalModel.Gold.Value;
		}

		private void GlobalModel_CornPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			Corn.Value = GlobalModel.Corn.Value;
		}

		private void GlobalModel_GasPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
		{
			Gas.Value = GlobalModel.Gas.Value;
		}
	}
}