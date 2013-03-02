
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;
using System.Threading;

namespace SuperKoikoukesse.iOS
{
	public partial class GameViewController : UIViewController
	{
		private Quizz m_quizz;
		private bool m_isViewLoaded;
		private NSTimer m_timer;

		#region UIView stuff

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public GameViewController ()
			: base (UserInterfaceIdiomIsPhone ? "GameViewController_iPhone" : "GameViewController_iPad", null)
		{
			Logger.Log (LogLevel.Info, "GameView on " + (UserInterfaceIdiomIsPhone ? "iPhone" : "iPad"));
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void LoadView ()
		{
			m_isViewLoaded = false;

			base.LoadView ();
		}

		partial void game1ButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			gameButtonPressed (0);
		}
		
		partial void game2ButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			gameButtonPressed (1);
		}
		
		partial void game3ButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			gameButtonPressed (2);
		}
		
		partial void game4ButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			gameButtonPressed (3);
		}

		#endregion
		
		#region Game 

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Initialize game database id first launch
			if (DatabaseService.Instance.Exists == false) {

				// Load gamedb.xml
				String xmlDatabase = File.ReadAllText (@"database/gamedb.xml");

				DatabaseService.Instance.InitializeFromXml (xmlDatabase);
			}

			// Display things the first time here because we need the UIImageView and other components to be created
			if (m_isViewLoaded == false) {
				setViewQuizz (m_quizz);
				setViewQuestion (m_quizz.CurrentQuestion);
				m_isViewLoaded = true;
			}
		}

		/// <summary>
		/// Initialize a new quizz game
		/// </summary>
		public void InitializeQuizz ()
		{
			// Prepare a quizz
			m_quizz = new Quizz ();
			m_quizz.Initialize ();
		
			// Display game
			if (m_isViewLoaded) {
				setViewQuizz (m_quizz);
				setViewQuestion (m_quizz.CurrentQuestion);
			}
		}

		/// <summary>
		/// Initialize the view for a new quizz
		/// </summary>
		private void setViewQuizz (Quizz q)
		{
			// Set timer in a thread
			var thread = new Thread (setGameTimer as ThreadStart);
			thread.Start ();
		}

		/// <summary>
		/// Launch the game timer
		/// </summary>
		private void setGameTimer ()
		{
			using (var pool = new NSAutoreleasePool()) {
				// Every 1 sec we update game timer
				m_timer = NSTimer.CreateRepeatingScheduledTimer (1f, delegate { 
					m_quizz.TimeLeft -= 1f;

					if (m_quizz.TimeLeft < 0) {
						m_quizz.TimeIsOver ();

						// No answer selected
						this.InvokeOnMainThread (() => {
							gameButtonPressed (-1);
						});
					}
					else {
						// Update label (UI Thread!)
						this.InvokeOnMainThread (() => {
							timeLeftLabel.Text = m_quizz.TimeLeft.ToString ("00");
						});
					}
				});

				NSRunLoop.Current.Run ();
			}
		}

		/// <summary>
		/// Stop the game timer
		/// </summary>
		private void stopGameTimer ()
		{
			if (m_timer != null) {
				m_timer.Invalidate ();
				m_timer.Dispose ();
				m_timer = null;
			}
		}

		/// <summary>
		/// Setup all the view elements for a given question
		/// </summary>
		/// <param name="q">Q.</param>
		private void setViewQuestion (Question q)
		{
			Logger.Log (LogLevel.Info, "Setting up view for current question " + q);

			// Timer
			timeLeftLabel.Text = m_quizz.TimeLeft.ToString ("00");

			// Image
			string imgPath = ImageService.Instance.Getimage (q.CorrectAnswer);
			UIImage img = UIImage.FromFile (imgPath);
			gameImage.Image = img;

			// Answers
			game1Button.SetTitle (q.GetGameTitle (0), UIControlState.Normal);
			game2Button.SetTitle (q.GetGameTitle (1), UIControlState.Normal);
			game3Button.SetTitle (q.GetGameTitle (2), UIControlState.Normal);
			game4Button.SetTitle (q.GetGameTitle (3), UIControlState.Normal);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">Answer pressed, -1 if no response</param>
		private void gameButtonPressed (int index)
		{
			m_quizz.SelectQuestion (index);

			m_quizz.NextQuestion ();

			if (m_quizz.IsOver == false) {
				setViewQuestion (m_quizz.CurrentQuestion);
			} else {
				// Back to the menu
				getBackToMenu();
			}
		}

		private void getBackToMenu() {
			stopGameTimer ();

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchView (GameState.Menu);
		}

		#endregion
	}
}

