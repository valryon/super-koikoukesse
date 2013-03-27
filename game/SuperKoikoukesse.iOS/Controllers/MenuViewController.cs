
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuViewController : UIViewController
	{
		private HelpGeneralViewController helpViewController;

		public MenuViewController ()
			: base ("MenuView", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			createPanels();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Initialize game database id first launch
			if (DatabaseService.Instance.Exists == false) {
				
				// Load gamedb.xml
				String xmlDatabase = File.ReadAllText (@"database/gamedb.xml");
				
				DatabaseService.Instance.InitializeFromXml (xmlDatabase);
			}

			// Set fonts manually because Interface Builder is a dick.
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			scoreAttackButton.TitleLabel.Font = appDelegate.CustomFont;
			timeAttackButton.TitleLabel.Font = appDelegate.CustomFont;
			survivalButon.TitleLabel.Font = appDelegate.CustomFont;

			// Load help once
			helpViewController = new HelpGeneralViewController ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			debugButton.SetTitle ("DEBUG: " + Constants.DebugMode, UIControlState.Normal);
		}

		#region Scroll view and pagination

		private void createPanels()
		{
			scrollView.Scrolled += ScrollViewScrolled;
			
			int count = 10;
			RectangleF scrollFrame = scrollView.Frame;
			scrollFrame.Width = scrollFrame.Width * count;
			scrollView.ContentSize = scrollFrame.Size;
			
			for (int i=0; i<count; i++)
			{
				UILabel label = new UILabel();
				label.TextColor = UIColor.Black;
				label.TextAlignment = UITextAlignment.Center;
				label.Text = i.ToString();
				
				if (i % 2 == 0)
					label.BackgroundColor = UIColor.Red;
				else
					label.BackgroundColor = UIColor.Blue;
				
				RectangleF frame = scrollView.Frame;
				PointF location = new PointF();
				location.X = frame.Width * i;
				
				frame.Location = location;
				label.Frame = frame;
				
				scrollView.AddSubview(label);
			}

			pageControl.Pages = count;
		}

		private void ScrollViewScrolled (object sender, EventArgs e)
		{
			double page = Math.Floor((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
			
			pageControl.CurrentPage = (int)page;
		}

		#endregion


		partial void scoreAttackButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToDifficultiesView (GameModes.ScoreAttack);
		}

		partial void timeAttackButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToDifficultiesView (GameModes.TimeAttack);
		}

		partial void survivalButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToDifficultiesView (GameModes.Survival);
		}

		/// <summary>
		/// Force config reload (DEBUG)
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void configButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.UpdateConfiguration ();
		}

		partial void debugButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			Constants.DebugMode = !Constants.DebugMode;
			debugButton.SetTitle ("DEBUG: " + Constants.DebugMode, UIControlState.Normal);
			Logger.Log (LogLevel.Info, "Debug mode? " + Constants.DebugMode);
		}

		partial void creditsButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToCreditsView ();
		}

		partial void helpButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			showHelp ();
		}

		private void showHelp ()
		{
			View.AddSubview (helpViewController.View);
		}
	}
}

