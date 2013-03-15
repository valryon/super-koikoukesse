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

		public GameCenterService (UIViewController viewController)
		{
			m_viewController = viewController;
		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public override bool Authenticate ()
		{
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
					Console.WriteLine ("Authentication result: {0}", error);
				};
			} else {
				// Versions prior to iOS 6.0
				GKLocalPlayer.LocalPlayer.Authenticate ((error) => {
					Console.WriteLine ("Authentication result: {0}", error);
				});
			}

			return false;
		}

		public override string PlayerId {
			get;
			set;
		}
	}
}

