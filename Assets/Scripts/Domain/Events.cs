namespace RuzikOdyssey.Domain
{
	public static class Events
	{
		public static class Global
		{
			public const string GoldPropertyChanged = "Global_Gold_PropertyChanged";
			public const string CornPropertyChanged = "Global_Corn_PropertyChanged";
			public const string GasPropertyChanged = "Global_Gas_PropertyChanged";
			public const string CurrentLevelIndexPropertyChanged = "Global_CurrentLevelIndex_PropertyChanged";
			public const string CurrentLevelDifficultyPropertyChanged = "Global_CurrentLevelDifficulty_PropertyChanged";

			public const string ModelLoadingProgressUpdated = "Global_Model_LoadingProgressUpdated";
			public const string ModelLoadingFinished = "Global_Model_LoadingFinished";
		}

		public static class Level
		{
			public const string PlayerWonLevel = "Level_PlayerWonLevel";
			public const string ScorePropertyChanged = "Level_Gold_Property_Changed";
		}
	}
}
