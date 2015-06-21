using RuzikOdyssey.Common;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.Models
{
	internal sealed class LevelModel
	{
		public Property<int> Gold { get; private set; }

		public LevelModel()
		{
			Gold = new Property<int>(0, Properties.Level.Gold, true);
		}
	}
}
