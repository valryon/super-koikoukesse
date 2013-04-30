using System;
using MonoTouch.GameKit;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.Foundation;
using Superkoikoukesse.Common.Utils;

namespace SuperKoikoukesse.iOS
{
	/// <summary>
	/// Game center integration
	/// </summary>
	public class GameCenterPlayer : AuthenticatedPlayer
	{
		private bool isAuthenticated;

		public event Action<UIViewController> ShowGameCenter;

		public GameCenterPlayer ()
			: base()
		{
		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public override void Authenticate (Action authenticationFinished)
		{
			isAuthenticated = false;
			Logger.Log (LogLevel.Info, "Game Center Authentication requested...");

			// On main thread
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.InvokeOnMainThread (() => {

				if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
					//
					// iOS 6.0 and newer
					//
					GKLocalPlayer.LocalPlayer.AuthenticateHandler = (ui, error) => {
					
						// If ui is null, that means the user is already authenticated,
						// for example, if the user used Game Center directly to log in
					
						if (ui != null) {
							ShowGameCenter (ui);
						} else {
							// Check if you are authenticated:
							isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;
						}

						if (error != null) {
							Logger.Log (LogLevel.Error, "Game Center Authentication failed! " + error);
						} else {
							isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;

							if (isAuthenticated) {
								Logger.Log (LogLevel.Info, "Game Center - " + PlayerId + "(" + DisplayName + ")");
							} else {
								Logger.Log (LogLevel.Warning, "Game Center - disabled !");
							}
						}

						if (authenticationFinished != null) {
							authenticationFinished ();
						}
					};
				} /* else {
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

					if (authenticationFinished != null) {
						authenticationFinished ();
					}
				});
			} */
			});
		}

		public override void AddScore (GameModes mode, GameDifficulties difficulty, int score)
		{
			if (mode != GameModes.Versus) {
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
		}

		public override void GetBestScoreAndRank (GameModes mode, GameDifficulties difficulty, Action<int,int> gcRankCallback)
		{
			if (IsAuthenticated) {

				if (gcRankCallback != null) {

					string leaderboardId = GetLeaderboardId (mode, difficulty);
					GKLeaderboard leaderboard = new GKLeaderboard ();
					leaderboard.GroupIdentifier = leaderboardId;

					leaderboard.LoadScores ((scoreArray, error) => {

						if (leaderboard.LocalPlayerScore != null) {
							int bestScore = (int)leaderboard.LocalPlayerScore.Value;
							int bestRank = leaderboard.LocalPlayerScore.Rank;

							gcRankCallback (bestRank, bestScore);
						}
					});

				}
			}
		}

		public override void NewMatch (Action matchFoundCallback, Action cancelCallback, Action errorCallback, Action playerQuitCallback)
		{
			GKMatchRequest matchRequest = new GKMatchRequest ();
			matchRequest.MinPlayers = 2;
			matchRequest.MaxPlayers = 2;
			matchRequest.DefaultNumberOfPlayers = 2;

			GKTurnBasedMatchmakerViewController matchMakerVc = new GKTurnBasedMatchmakerViewController (matchRequest);

			var mmDelegate = new MatchMakerDelegate (this);
			mmDelegate.MatchFoundCallback += matchFoundCallback;
			mmDelegate.CancelCallback += cancelCallback;
			mmDelegate.ErrorCallback += errorCallback;
			mmDelegate.PlayerQuitCallback += playerQuitCallback;
			matchMakerVc.Delegate = mmDelegate;

			ShowGameCenter (matchMakerVc);
		}

		public override void EndMatchTurn (int score, Action callback)
		{
			if (CurrentMatch != null) {

				// Find opponent
				GKTurnBasedParticipant opponent = null;

				foreach (var player in CurrentGKMatch.Participants) {
					if (player.PlayerID != this.PlayerId) {
						opponent = player;
						break;
					}
				}

				// Add data
				CurrentMatch.Turns.Add (new VersusMatchTurn() {
					PlayerId = this.PlayerId,
					Score = score
				});

				CurrentGKMatch.EndTurn (
					new GKTurnBasedParticipant[] {opponent},
					GKTurnBasedMatch.DefaultTimeout,
					NSData.FromString (CurrentMatch.ToJson()), 
					(e) => {
					Logger.Log (LogLevel.Info, "Game Center Turn ended");

					if (e != null) {
						Logger.Log (LogLevel.Error, e.LocalizedDescription);
					}
				});

			} else {
				Logger.Log (LogLevel.Error, "Cannot end the turn because we're not in a match!");
			}
		}

		public override void QuitMatch (Action callback)
		{
			if (CurrentMatch != null) {
				
			} else {
				Logger.Log (LogLevel.Error, "Cannot quit because we're not in a match!");
			}
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
				return isAuthenticated;
			}
		}
	
		internal GKTurnBasedMatch CurrentGKMatch { get; set; }

		// Delegate for turn-based Game Center

		private class MatchMakerDelegate : GKTurnBasedMatchmakerViewControllerDelegate
		{
			public event Action MatchFoundCallback, CancelCallback, ErrorCallback, PlayerQuitCallback;

			private GameCenterPlayer parent;

			public MatchMakerDelegate (GameCenterPlayer parent)
			{
				this.parent = parent;
			}

			protected override void Dispose (bool disposing)
			{
				this.parent = null;
				base.Dispose (disposing);
			}

			public override void WasCancelled (GKTurnBasedMatchmakerViewController viewController)
			{
				Logger.Log (LogLevel.Info, "MatchMakerDelegate.WasCancelled");

				viewController.DismissViewController (true, null);

				if (CancelCallback != null)
					CancelCallback ();
			}

			public override void FailedWithError (GKTurnBasedMatchmakerViewController viewController, MonoTouch.Foundation.NSError error)
			{
				Logger.Log (LogLevel.Warning, "MatchMakerDelegate.FailedWithError");

				viewController.DismissViewController (true, null);

				if (ErrorCallback != null)
					ErrorCallback ();
			}

			public override void FoundMatch (GKTurnBasedMatchmakerViewController viewController, GKTurnBasedMatch match)
			{
				Logger.Log (LogLevel.Info, "MatchMakerDelegate.FoundMatch");

				viewController.DismissViewController (true, null);

				this.parent.CurrentGKMatch = match;

				// Match has data: it's not the first turn
				if(match.MatchData.Length> 0) {
					VersusMatch existingMatch = new VersusMatch();
					existingMatch.FromJson(NSString.FromData(match.MatchData, NSStringEncoding.UTF8).ToString());
					this.parent.CurrentMatch = existingMatch;
				}
				// No data: new match, 
				else {
					this.parent.CurrentMatch = new VersusMatch();

					this.parent.CurrentMatch.MatchId = match.MatchID;
					this.parent.CurrentMatch.Player1Id = match.Participants[0].PlayerID;
					this.parent.CurrentMatch.Player2Id = match.Participants[1].PlayerID;

					this.parent.CurrentMatch.Filter = new Filter();
				}

				if (MatchFoundCallback != null)
					MatchFoundCallback ();
			}

			public override void PlayerQuitForMatch (GKTurnBasedMatchmakerViewController viewController, GKTurnBasedMatch match)
			{
				Logger.Log (LogLevel.Info, "MatchMakerDelegate.PlayerQuitForMatch");

				//viewController.DismissViewController (true, null);

				// Delete the match
				match.Remove (new GKNotificationHandler ((error) => {
					Logger.Log (LogLevel.Error, error.DebugDescription);
				}));

				if (PlayerQuitCallback != null)
					PlayerQuitCallback ();
			}

		}
	}
}

