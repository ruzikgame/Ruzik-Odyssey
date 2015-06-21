using RuzikOdyssey.Common;

namespace RuzikOdyssey.Domain
{
	public class AircraftInfo
	{
		public AircraftUiInfo Ui { get; set; }

		public override string ToString ()
		{
			return string.Format ("[AircraftInfo: Ui={0}]", Ui);
		}

		public class AircraftUiInfo 
		{
			/* TODO
			 * Replace with Property that can be suscribed to.
			 * Generally all values in models should be properties that 
			 * raise events on change.
			 * 
			 * Need to create a Newton.Json converter between T and Property<T>
			 */
			public string SceneSpriteName { get; set; }

			public override string ToString ()
			{
				return string.Format ("[AircraftUiInfo: SceneSpriteName={0}]", SceneSpriteName);
			}
		}
	}
}
