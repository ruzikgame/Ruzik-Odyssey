using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Common
{
	public static class GameEnvironment
	{ 
		public const float DesignWidth = 2048f; 
		public const float DesignHeight = 1154f;
		public static Vector2 ScaleOffset;
		public static float Scale;
		public static GUISkin DefaultSkin;
		public static GUIStyle DefaultButtonStyle;

		public static Vector2 ForegroundSpeed = new Vector2(-1, 0);

		public static bool IsPaused { get; private set; }
		public static bool IsGameOver { get; private set; }

		static GameEnvironment()
		{
			ScaleOffset.x = Screen.width / DesignWidth;
			ScaleOffset.y = Screen.height / DesignHeight;
			Scale = Mathf.Max(ScaleOffset.x, ScaleOffset.y); 
			DefaultSkin = Resources.Load("default_menu_skin") as GUISkin;
			DefaultButtonStyle = new GUIStyle(DefaultSkin.GetStyle("button"));
			DefaultButtonStyle.fontSize = (int)(Scale * DefaultButtonStyle.fontSize);

			IsPaused = false;
			IsGameOver = false;
		}

		public static void Pause()
		{
			Time.timeScale = 0;
			IsPaused = true;
		}

		public static void Resume()
		{
			Time.timeScale = 1;
			IsPaused = false;
		}

		public static void StartMission()
		{
			IsGameOver = false;
			EventsBroker.ClearSubscribtions();
		}

		public static void GameOver()
		{
			IsGameOver = true;
		}



	}
}

