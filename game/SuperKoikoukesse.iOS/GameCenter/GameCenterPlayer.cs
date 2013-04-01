using System;
using MonoTouch.GameKit;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	/// <summary>
	/// Game center integration
	/// </summary>
	public class GameCenterPlayer : AuthenticatedPlayer
	{
		private UIViewController m_viewController;
		private bool m_isAuthenticated;

		public GameCenterPlayer (UIViewController viewController)
			: base()
		{
			m_viewController = viewController;
		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public override void Authenticate (Action authenticationFinished)
		{
			m_isAuthenticated = false;
			Logger.Log (LogLevel.Info, "Game Center Authentication requested...");

			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				//
				// iOS 6.0 and newer
				//
				GKLocalPlayer.LocalPlayer.AuthenticateHandler = (ui, error) => {
					
					// If ui is null, that means the user is already authenticated,
					// for example, if the user used Game Center directly to log in
					
					if (ui != null) {
						m_viewController.PresentModalViewController (ui, true);
					} else {
						// Check if you are authenticated:
						m_isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;
					}

					if (error != null) {
						Logger.Log (LogLevel.Error, "Game Center Authentication failed! " + error);
					} else {
						m_isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;

						if (m_isAuthenticated) {
							Logger.Log (LogLevel.Info, "Game Center - " + PlayerId + "(" + DisplayName + ")");
						} else {
							Logger.Log (LogLevel.Warning, "Game Center - disabled !");
						}
					}

					if(authenticationFinished != null) {
						authenticationFinished();
					}
				};
			} else {
				// Versions prior to iOS 6.0
				GKLocalPlayer.LocalPlayer.Authenticate ((error) => {
					if (error != null) {
						Logger.Log (LogLevel.Error, "Game Center Authentication failed! " + error);
					} else {
						m_isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;
						
						if (m_isAuthenticated) {
							Logger.Log (LogLevel.Info, "Game Center - " + PlayerId + "(" + DisplayName + ")");
						} else {
							Logger.Log (LogLevel.Warning, "Game Center - disabled !");
						}
					}

					if(authenticationFinished != null) {
						authenticationFinished();
					}
				});
			}
		}

		public override void AddScore (GameModes mode, GameDifficulties difficulty, int score)
		{
			string leaderboardId = GetLeaderboardId (mode, difficulty);

			Logger.Log (LogLevel.Info, "Game Center  - Adding score to " + leaderboardId + "...");

			GKScore gkScore = new GKScore (leaderboardId);
			gkScore.Value = score;

			gkScore.ReportScore ((error) => {
				if (error != null) {
					Logger.Log (LogLevel.Error, "Game Center - Score not submited! " + error);
				}
			});
		}

		public override string DisplayName {
			get {
				if (IsAuthenticated) {
					return GKLocalPlayer.LocalPlayer.Alias;
				} else {
					return base.PlayerId;
				}
			}
		}

		public override string PlayerId {
			get {
				if (IsAuthenticated) {
					return GKLocalPlayer.LocalPlayer.PlayerID;
				} else {
					return base.PlayerId;
				}
			}
		}

		public override bool IsAuthenticated {
			get {
				return m_isAuthenticated;
			}
		}
	}
}

