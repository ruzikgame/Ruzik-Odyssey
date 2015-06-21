using UnityEngine;
using RuzikOdyssey.Common;
using System;
using RuzikOdyssey.Level;
using RuzikOdyssey.Domain;
using RuzikOdyssey.ViewModels;
using RuzikOdyssey.UI;

namespace RuzikOdyssey.Views
{
	public sealed class LevelSceneView : ExtendedMonoBehaviour
	{
		public GameObject popupsContainer;
		public GameObject playerWonPopup;
		public GameObject playerLostPopup;
		public GameObject pausePopup;

		public float wonLevelUIDelay = 1.0f;

		public LevelSceneViewModel viewModel;

		public UIToggle shieldToggle;
		public UILabel scoreLabel;
		public UILabel missileAmmoLabel;

		public UILabel playerWonPopupGoldLabel;
		public UILabel playerWonPopupCornLabel;
		public UILabel playerWonPopupEnemiesKilledLabel;

		public UILabel playerLostPopupGoldLabel;
		public UILabel playerLostPopupCornLabel;
		public UILabel playerLostPopupEnemiesKilledLabel;

		public event EventHandler<EventArgs> FireMissileButtonClicked;
		public event EventHandler<ToggleStateChangedEventArgs> ShieldToggleStateChanged;

		public event EventHandler LevelResumed;
		public event EventHandler LevelPaused;
		public event EventHandler LevelRestarted;
		public event EventHandler LevelExited;

		private void Awake()
		{
			viewModel.PlayerWonLevel += ViewModel_PlayerWon;
			viewModel.PlayerLost += ViewModel_PlayerLost;

			this.FireMissileButtonClicked += viewModel.View_FireMissileButtonClicked;
			this.ShieldToggleStateChanged += viewModel.View_ShieldToggleStateChanged;

			this.LevelExited += viewModel.View_LevelExited;
			this.LevelPaused += viewModel.View_LevelPaused;
			this.LevelRestarted += viewModel.View_LevelRestarted;
			this.LevelResumed += viewModel.View_LevelResumed;

			missileAmmoLabel.BindTo(viewModel.MissileAmmo);
			scoreLabel.BindTo(viewModel.Score);
		}

		private void Start()
		{
			CloseAllPopups();
		}

		private void CloseAllPopups()
		{
			popupsContainer.SetActive(false);
			playerWonPopup.SetActive(false);
			playerLostPopup.SetActive(false);
			pausePopup.SetActive(false);
		}

		private void ViewModel_PlayerWon(object sender, PlayerWonLevelEventArgs e)
		{
			Log.Info("Player Won!!!!!");

			playerWonPopupGoldLabel.text = e.GoldEarned.ToString();
			playerWonPopupCornLabel.text = e.CornEarned.ToString();
			playerWonPopupEnemiesKilledLabel.text = 100 + "%";

			popupsContainer.SetActive(true);
			playerWonPopup.SetActive(true);
		}

		private void ViewModel_PlayerLost(object sender, PlayerLostEventArgs e)
		{
			Log.Info("Player lost level!");

			playerLostPopupGoldLabel.text = e.GoldEarned.ToString();
			playerLostPopupCornLabel.text = e.CornEarned.ToString();
			playerLostPopupEnemiesKilledLabel.text = 0 + "%";

			popupsContainer.SetActive(true);
			playerLostPopup.SetActive(true);
		}

		public void Game_OnPopupScreenShadowClicked()
		{
			CloseAllPopups();
		}

		public void Game_OnFireMissileButtonClicked()
		{
			if (FireMissileButtonClicked != null) FireMissileButtonClicked(this, EventArgs.Empty);
		}

		public void Game_OnShieldToggleStateChanged()
		{
			var isOn = shieldToggle.value;

			if (ShieldToggleStateChanged != null) 
				ShieldToggleStateChanged(this, new ToggleStateChangedEventArgs { ToggleIsOn = isOn });
		}

		public void Game_OnPauseButtonClicked()
		{
			popupsContainer.SetActive(true);
			pausePopup.SetActive(true);

			if (LevelPaused != null) LevelPaused(this, EventArgs.Empty);
		}

		public void Game_OnHomeButtonClicked()
		{
			CloseAllPopups();

			if (LevelExited != null) LevelExited(this, EventArgs.Empty);
		}

		public void Game_OnResumeButtonClicked()
		{
			CloseAllPopups();

			if (LevelResumed != null) LevelResumed(this, EventArgs.Empty);
		}

		public void Game_OnRestartButtonClicked()
		{
			CloseAllPopups();

			if (LevelRestarted != null) LevelRestarted(this, EventArgs.Empty);
		}
	}
}
