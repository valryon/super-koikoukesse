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
			Logger.I("Game Center Authentication requested...");

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
							Logger.E( "Game Center Authentication failed! " + error);
						} else {
							isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;

							if (isAuthenticated) {
								Logger.I("Game Center - " + PlayerId + "(" + DisplayName + ")");
							} else {
								Logger.W( "Game Center - disabled !");
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
						Logger.E( "Game Center Authentication failed! " + error);
					} else {
						m_isAuthenticated = GKLocalPlayer.LocalPlayer.Authenticated;
						
						if (m_isAuthenticated) {
							Logger.I("Game Center - " + PlayerId + "(" + DisplayName + ")");
						} else {
							Logger.W( "Game Center - disabled !");
						}
					}

					if (authenticationFinished != null) {
						authenticationFinished ();
					}
				});
			} */
			});
		}

		public override void AddScore (GameMode mode, GameDifficulties difficulty, int score)
		{
			if (mode != GameMode.VERSUS) {
				string leaderboardId = GetLeaderboardId (mode, difficulty);

				Logger.I("Game Center  - Adding score to " + leaderboardId + "...");

				GKScore gkScore = new GKScore (leaderboardId);
				gkScore.Value = score;

				gkScore.ReportScore ((error) => {
					if (error != null) {
						Logger.E( "Game Center - Score not submited! " + error);
					}
				});
			}
		}

		public override void GetBestScoreAndRank (GameMode mode, GameDifficulties difficulty, Action<int,int> gcRankCallback)
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

		public override void NewMatch (Action<VersusMatch> matchFoundCallback, Action cancelCallback, Action errorCallback, Action playerQuitCallback)
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
					}
				}

				// Add data
				CurrentMatch.Turns.Add (new VersusMatchTurn () {
					PlayerId = this.PlayerId,
					Score = score
				});

				bool isMatchOver = false;

				// Is the match over?
				// We should have one turn for each player
				if (CurrentMatch.Turns.Count > 0 
					&& CurrentMatch.Turns.Count % CurrentGKMatch.Participants.Length == 0) {
					isMatchOver = true;
				}

				CurrentMatch.IsEnded = isMatchOver;

				if (isMatchOver == false) {
					CurrentGKMatch.EndTurn (
						new GKTurnBasedParticipant[] {opponent},
						GKTurnBasedMatch.DefaultTimeout,
						NSData.FromString (CurrentMatch.ToJson ().ToBase64 ()), 
						(e) => {
						Logger.I("Game Center Turn ended");

						if (e != null) {
							Logger.E( e.DebugDescription);
						}
					}
					);
				} else {

					// Check for winner / loser
					int maxScore = 0;
					string winnerPlayerId = string.Empty;

					foreach (VersusMatchTurn turn in CurrentMatch.Turns) {
						if (turn.Score > maxScore) {
							winnerPlayerId = turn.PlayerId;
							maxScore = turn.Score;
						} else if (turn.Score == maxScore) {
							winnerPlayerId = string.Empty;
						}
					}

					// Set win and defeat state to the players
					foreach (GKTurnBasedParticipant participant in CurrentGKMatch.Participants) {
						if (string.IsNullOrEmpty (winnerPlayerId)) {
							participant.MatchOutcome = GKTurnBasedMatchOutcome.Tied;
						} else {
							if (participant.PlayerID == winnerPlayerId) {
								participant.MatchOutcome = GKTurnBasedMatchOutcome.Won;
							} else {
								participant.MatchOutcome = GKTurnBasedMatchOutcome.Lost;
							}
						}
					}

					CurrentGKMatch.EndMatchInTurn (
						NSData.FromString (CurrentMatch.ToJson ().ToBase64 ()), 
						(e) => {
						Logger.I("Game Center Match ended");
							
						if (e != null) {
							Logger.E( e.DebugDescription);
						}
					}
					);
				}


			} else {
				Logger.E( "Cannot end the turn because we're not in a match!");
			}
		}

		public override void QuitMatch (Action callback)
		{
			if (CurrentMatch != null) {
				
			} else {
				Logger.E( "Cannot quit because we're not in a match!");
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
			public event Action<VersusMatch> MatchFoundCallback;
			public event Action CancelCallback, ErrorCallback, PlayerQuitCallback;

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
				Logger.I("MatchMakerDelegate.WasCancelled");

				viewController.DismissViewController (true, null);

				if (CancelCallback != null)
					CancelCallback ();
			}

			public override void FailedWithError (GKTurnBasedMatchmakerViewController viewController, MonoTouch.Foundation.NSError error)
			{
				Logger.W( "MatchMakerDelegate.FailedWithError");

				viewController.DismissViewController (true, null);

				if (ErrorCallback != null)
					ErrorCallback ();
			}

			public override void FoundMatch (GKTurnBasedMatchmakerViewController viewController, GKTurnBasedMatch match)
			{
				Logger.I("MatchMakerDelegate.FoundMatch");

				viewController.DismissViewController (true, null);

				this.parent.CurrentGKMatch = match;

				bool matchError = false;

				// Match has data: it's not the first turn
				if (match.MatchData.Length > 0) {
					VersusMatch existingMatch = new VersusMatch ();

					try {

						string jsonBase64 = NSString.FromData (match.MatchData, NSStringEncoding.UTF8);
						string json = System.Text.Encoding.UTF8.GetString (Convert.FromBase64String (jsonBase64));

						existingMatch.FromJson (json.ToString ());
						this.parent.CurrentMatch = existingMatch;
					} catch (Exception e) {
						matchError = true;
						Logger.E( "GameCenterPlayer.FoundMatch", e);
					}
				}
				// No data: new match, 
				else {
					this.parent.CurrentMatch = new VersusMatch ();

					this.parent.CurrentMatch.MatchId = match.MatchID;
					this.parent.CurrentMatch.Player1Id = match.Participants [0].PlayerID;
					this.parent.CurrentMatch.Player2Id = match.Participants [1].PlayerID;

					// Set up outcomes
					match.Participants [0].MatchOutcome = GKTurnBasedMatchOutcome.First;
					match.Participants [1].MatchOutcome = GKTurnBasedMatchOutcome.Second;

					// TODO Select filter
					this.parent.CurrentMatch.Filter = new Filter ("0", "MP Filter", "defaultIcon",
					                                              // Test data
					                                              1990,
					                                              2050,
					                                              null,
					                                              new System.Collections.Generic.List<string> () {"combat"},
					                                              null
					);

				}

				if (matchError == false) {
					match.Remove (new GKNotificationHandler ((e) => {}));

					if (MatchFoundCallback != null)
						MatchFoundCallback (this.parent.CurrentMatch);
				} else {
					if (ErrorCallback != null)
						ErrorCallback ();
				}
			}

			public override void PlayerQuitForMatch (GKTurnBasedMatchmakerViewController viewController, GKTurnBasedMatch match)
			{
				Logger.I("MatchMakerDelegate.PlayerQuitForMatch");

				// Mark current player as quiter
				foreach(GKTurnBasedParticipant participant in match.Participants) {
					if(participant.PlayerID == this.parent.PlayerId) {
						participant.MatchOutcome = GKTurnBasedMatchOutcome.Quit;
					}
					else {
						// Win?
						participant.MatchOutcome = GKTurnBasedMatchOutcome.Won;
					}
				}

				//viewController.DismissViewController (true, null);

				// Delete the match
				match.Remove (new GKNotificationHandler ((error) => {
					Logger.E( error.DebugDescription);
				}));

				if (PlayerQuitCallback != null)
					PlayerQuitCallback ();
			}

		}
	}
}

