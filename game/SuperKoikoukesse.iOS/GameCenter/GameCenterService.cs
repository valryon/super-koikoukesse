using System;
using MonoTouch.GameKit;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	/// <summary>
	/// Game center integration
	/// </summary>
	public class GameCenterService : PlayerService
	{
		private UIViewController m_viewController;
		private bool m_isAuthenticated;

		public GameCenterService (UIViewController viewController)
		{
			m_viewController = viewController;
		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public override void Authenticate ()
		{
			m_isAuthenticated = false;
			Logger.Log(LogLevel.Info, "Game Center Authentication requested...");

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
						var authenticated = GKLocalPlayer.LocalPlayer.Authenticated;
					}

					if(error != null) {
						Logger.Log(LogLevel.Error, "Game Center Authentication failed! "+error);
					}
					else {
						m_isAuthenticated = true;
					}
				};
			} else {
				// Versions prior to iOS 6.0
				GKLocalPlayer.LocalPlayer.Authenticate ((error) => {
					if(error != null) {
						Logger.Log(LogLevel.Error, "Game Center Authentication failed! "+error);
					}
					else {
						m_isAuthenticated = true;
					}
				});
			}
		}

		public override void AddScore (GameModes mode, GameDifficulties difficulty, int score)
		{
//			GKScore *myScoreValue = [[[GKScore alloc] initWithCategory:@"testleaderboard"] autorelease];
//			myScoreValue.value = score;
//			
//			[myScoreValue reportScoreWithCompletionHandler:^(NSError *error){
//				if(error != nil){
//					NSLog(@"Score Submission Failed");
//				} else {
//					NSLog(@"Score Submitted");
//				}
//				
//			 }];

			GKScore gkScore = new GKScore(GetLeaderboardId(mode,difficulty));
			gkScore.Value = score;

			gkScore.ReportScore( (error) => {
				if(error != null) {
					Logger.Log(LogLevel.Error,"Game Center - Score not submited! " + error);
				}
			});
		}

		public override string PlayerId {
			get {
				return GKLocalPlayer.LocalPlayer.DisplayName;
			}
		}

		public override bool IsAuthenticated {
			get {
				return m_isAuthenticated;
			}
		}
	}
}

