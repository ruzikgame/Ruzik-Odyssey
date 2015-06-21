using System;

namespace RuzikOdyssey.Ai
{
	public class MovementStrategyFactory
	{
		private Random random;

		private static MovementStrategyFactory instance;

		public static MovementStrategyFactory Instance 
		{ 
			get
			{
				if (instance == null) 
				{ 
					instance = new MovementStrategyFactory();
				}
				return instance;
			}
		}

		private MovementStrategyFactory()
		{
			this.random = new Random();
		}

		public IMovementStrategy Create(QualityLevel intelligence, QualityLevel aggression)
		{
			var aggressionLevel = random.Next(1, 11);
			var intellectLevel = random.Next(1, 11);
			return Create (aggressionLevel, intellectLevel);
		}

		public IMovementStrategy Create(int aggressionLevel, int intellectLevel)
		{
			return new PassByMovementStrategy();
		}
	}

	public enum QualityLevel
	{
		Other = 0,
		Low = 1,
		Average = 2,
		High = 3
	}
}
