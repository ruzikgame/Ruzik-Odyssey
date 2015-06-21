using RuzikOdyssey;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.UI.Views
{
	public sealed class SettingsSceneView : ExtendedMonoBehaviour
	{
		public UILabel gameContentVersionLabel;

		private void Awake()
		{
			GlobalModel.Connect();
		}

		private void Start()
		{
			gameContentVersionLabel.text = GlobalModel.Content.Version;
		}
	}
}