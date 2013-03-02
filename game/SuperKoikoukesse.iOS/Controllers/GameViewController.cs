
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
		private UIImage m_currentImage, m_pauseImage;

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

			// Set fonts manually because Interface Builder is a dick.
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			game1Button.TitleLabel.Font = appDelegate.CustomFont;
			game2Button.TitleLabel.Font = appDelegate.CustomFont;
			game3Button.TitleLabel.Font = appDelegate.CustomFont;
			game4Button.TitleLabel.Font = appDelegate.CustomFont;
			timeTitleLabel.Font =appDelegate.CustomFont; 
			timeLeftLabel.Font = appDelegate.CustomFont;
			scoreTitleLabel.Font = appDelegate.CustomFont;
			scoreLabel.Font = appDelegate.CustomFont;
			jokerButton.Font = appDelegate.CustomFont;
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

		partial void pauseButtonPressed (MonoTouch.Foundation.NSObject sender) {
			pauseAction();
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

			// Load the pause/inactive screen image
			string imgPath = "empty_screen.png";
			m_pauseImage = UIImage.FromFile (imgPath);
			gameImage.Image = m_pauseImage;

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
			m_currentImage = UIImage.FromFile (imgPath);
			gameImage.Image = m_currentImage;

			// Answers
			setGameButtonTitles (q);
		}

		private void setGameButtonTitles(Question q) {

			// Disable buttons
			if (q == null) {

				jokerButton.Enabled = false;

				game1Button.Enabled = false;
				game2Button.Enabled = false;
				game3Button.Enabled = false;
				game4Button.Enabled = false;
				game1Button.SetTitle ("", UIControlState.Normal);
				game2Button.SetTitle ("", UIControlState.Normal);
				game3Button.SetTitle ("", UIControlState.Normal);
				game4Button.SetTitle ("", UIControlState.Normal);
			} 
			// Buttons for current question
			else {

				jokerButton.Enabled = true;

				game1Button.Enabled = true;
				game2Button.Enabled = true;
				game3Button.Enabled = true;
				game4Button.Enabled = true;
				game1Button.SetTitle (q.GetGameTitle (0), UIControlState.Normal);
				game2Button.SetTitle (q.GetGameTitle (1), UIControlState.Normal);
				game3Button.SetTitle (q.GetGameTitle (2), UIControlState.Normal);
				game4Button.SetTitle (q.GetGameTitle (3), UIControlState.Normal);
			}
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

		private void pauseAction() {

			m_quizz.IsPaused = !m_quizz.IsPaused;

			if (m_quizz.IsPaused) {

				// Mask game elements
				gameImage.Image = m_pauseImage;

				setGameButtonTitles(null);

				// Pause game
				stopGameTimer();

			} else {

				gameImage.Image = m_currentImage;

				setGameButtonTitles(m_quizz.CurrentQuestion);

				// Resume game
				setGameTimer();
			}
		}

		#endregion
	}
}

