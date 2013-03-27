
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuDifficultyViewController : UIViewController
	{
		private GameModes m_mode;

		public MenuDifficultyViewController ()
			: base ("MenuDifficultyView", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		partial void easyButtonPressed (MonoTouch.Foundation.NSObject sender){
			SelectDifficulty(GameDifficulties.Easy);
		}
		partial void hardButtonPressed (MonoTouch.Foundation.NSObject sender){
			SelectDifficulty(GameDifficulties.Hard);
		}
		partial void expertButtonPressed (MonoTouch.Foundation.NSObject sender){
			SelectDifficulty(GameDifficulties.Expert);
		}
		partial void nolifeButtonPressed (MonoTouch.Foundation.NSObject sender){
			SelectDifficulty(GameDifficulties.Nolife);
		}

		private void SelectDifficulty(GameDifficulties diff) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToGameView (m_mode, diff);
		}

		partial void backButtonPressed (MonoTouch.Foundation.NSObject sender){
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}

		/// <summary>
		/// Sets the selected game mode.
		/// </summary>
		/// <param name="mode">Mode.</param>
		public void SetGameMode(GameModes mode) {
			m_mode = mode;
		}
	}
}

