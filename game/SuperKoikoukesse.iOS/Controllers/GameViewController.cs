
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;
using System.Threading;
using MonoTouch.CoreGraphics;

namespace SuperKoikoukesse.iOS
{
	public partial class GameViewController : UIViewController
	{
		private Quizz m_quizz;
		private bool m_isViewLoaded;
		private NSTimer m_timer, m_animationTimer;
		private UIImage m_currentImage, m_pauseImage;
		private float m_currentPixelateFactor;

		#region UIView stuff

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public GameViewController ()
			: base (UserInterfaceIdiomIsPhone ? "GameView_iPhone" : "GameView_iPad", null)
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
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			game1Button.TitleLabel.Font = appDelegate.CustomFont;
			game2Button.TitleLabel.Font = appDelegate.CustomFont;
			game3Button.TitleLabel.Font = appDelegate.CustomFont;
			game4Button.TitleLabel.Font = appDelegate.CustomFont;
			timeTitleLabel.Font = appDelegate.CustomFont; 
			timeLeftLabel.Font = appDelegate.CustomFont;
			scoreTitleLabel.Font = appDelegate.CustomFont;
			scoreLabel.Font = appDelegate.CustomFont;
			jokerButton.Font = appDelegate.CustomFont;
			comboLabel.Font = appDelegate.CustomFont;
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

		partial void pauseButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			pauseAction ();
		}

		partial void jokerButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			m_quizz.UseJoker ();

			nextQuestion ();
		}

		#endregion
		
		#region Game 

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Load the pause/inactive screen image
			string imgPath = "empty_screen.png";
			m_pauseImage = UIImage.FromFile (imgPath);
			gameImage.Image = m_pauseImage;

			// Display things the first time here because we need the UIImageView and other components to be created
			if (m_isViewLoaded == false) {
				updateViewToQuizz (m_quizz);
				updateViewToQuestion (m_quizz.CurrentQuestion);
				m_isViewLoaded = true;
			}
		}

		/// <summary>
		/// Initialize a new quizz game
		/// </summary>
		public void InitializeQuizz (GameModes mode, GameDifficulties diff)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 

			// Prepare a quizz
			m_quizz = new Quizz ();
			m_quizz.Initialize (mode, diff, appDelegate.Configuration);
		
			// Display game
			if (m_isViewLoaded) {
				updateViewToQuizz (m_quizz);
				updateViewToQuestion (m_quizz.CurrentQuestion);
			}
		}

		/// <summary>
		/// Initialize the view for a new quizz
		/// </summary>
		private void updateViewToQuizz (Quizz q)
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
					m_quizz.SubstractTime (1f);

					if (m_quizz.TimeLeft < 0) {

						m_quizz.TimeIsOver ();

						// No answer selected
						this.InvokeOnMainThread (() => {
							nextQuestion ();
						});
					} else {
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
		private void updateViewToQuestion (Question q)
		{
			Logger.Log (LogLevel.Info, "Setting up view for current question " + q);

			// Timer
			timeLeftLabel.Text = m_quizz.TimeLeft.ToString ("00");

			// Image
			string imgPath = ImageService.Instance.Getimage (q.CorrectAnswer);
			m_currentImage = UIImage.FromFile (imgPath);
			gameImage.Image = m_currentImage;

			setImageAnimation ();

			// Answers
			setGameButtonTitles (q);

			// Score & combo
			scoreLabel.Text = m_quizz.Score.ToString ("000000");
			comboLabel.Text = "x" + m_quizz.Combo;

			// Joker
			jokerButton.Enabled = m_quizz.IsJokerAvailable;

			if (Constants.DebugMode) {
				jokerButton.SetTitle ("JOKER = " + m_quizz.JokerPartCount, UIControlState.Normal);
			} else {
				jokerButton.SetTitle ("JOKER", UIControlState.Normal);
			}
		}

		private void setImageAnimation ()
		{
			// image size
			float imageBaseSizeWidth = gameImageScroll.Frame.Width;
			float imageBaseSizeHeight = gameImageScroll.Frame.Height;

			gameImage.Frame = new RectangleF (0, 0, imageBaseSizeWidth, imageBaseSizeHeight);

			if (m_animationTimer != null) {
				m_animationTimer.Dispose();
				m_animationTimer = null;
			}

			if (m_quizz.ImageTransformation == ImageTransformations.Unzoom) {

				// Images are 500*500
				// Scroll view is 250*250
				// Dezoom consists of center the image on a dimension and dezoom to its original size
				float startZoomFactor = Constants.DezoomFactor;
				float width = gameImage.Frame.Width * startZoomFactor;
				float height = gameImage.Frame.Height * startZoomFactor;

				float x = imageBaseSizeWidth - (width / 2);
				float y = imageBaseSizeHeight - (height / 2);

				gameImage.Frame = new RectangleF (x, y, width, height);

				UIView.Animate (
					Constants.DezoomDuration,
	                () => {
					gameImage.Frame = new RectangleF (0, 0, imageBaseSizeWidth, imageBaseSizeHeight);
				}
				);

			} else if (m_quizz.ImageTransformation == ImageTransformations.Pixelization) {

				m_currentPixelateFactor = 1f;
				pixelateGameImage(m_currentPixelateFactor, imageBaseSizeWidth, imageBaseSizeHeight);

				// Thread animation
				var thread = new Thread (() => {

					using (var pool = new NSAutoreleasePool()) {
						m_animationTimer = NSTimer.CreateRepeatingScheduledTimer (0.1f, delegate { 
							// TODO Améliorer, optimiser, tout ça...
							m_currentPixelateFactor += 2;

							// Stop animation at 85% before it became a pixel's mess
							if (m_currentPixelateFactor > (imageBaseSizeWidth * 0.5f)) {
								m_currentPixelateFactor = imageBaseSizeWidth;

								if(m_animationTimer != null) {
									m_animationTimer.Dispose();
								}
							}
							pixelateGameImage(m_currentPixelateFactor, imageBaseSizeWidth, imageBaseSizeHeight);
						});
					
						NSRunLoop.Current.Run ();
					}
				});
				thread.Start ();
			}
		}

		/// <summary>
		/// Downsample and upsample the game image to create a pixelate effect
		/// </summary>
		/// <param name="pixelateFactor">Pixelate factor.</param>
		private void pixelateGameImage(float pixelateFactor, float maxWidth, float maxHeight) {
						
			var a = m_currentImage.Scale (new SizeF (pixelateFactor, pixelateFactor));
			a.Scale (new SizeF (maxWidth, maxHeight));
			
			this.BeginInvokeOnMainThread (() => {
				gameImage.Image = a;
			});
		}

		private void setGameButtonTitles (Question q)
		{

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

				// TODO PAL
				game1Button.SetTitle (q.GetGameTitle (0, GameZones.PAL, m_quizz.TextTransformation), UIControlState.Normal);
				game2Button.SetTitle (q.GetGameTitle (1, GameZones.PAL, m_quizz.TextTransformation), UIControlState.Normal);
				game3Button.SetTitle (q.GetGameTitle (2, GameZones.PAL, m_quizz.TextTransformation), UIControlState.Normal);
				game4Button.SetTitle (q.GetGameTitle (3, GameZones.PAL, m_quizz.TextTransformation), UIControlState.Normal);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">Answer pressed, -1 if no response</param>
		private void gameButtonPressed (int index, bool isJoker = false)
		{
			m_quizz.SelectAnswer (index, isJoker);

			nextQuestion ();
		}

		private void nextQuestion ()
		{
			m_quizz.NextQuestion ();
			
			if (m_quizz.IsOver == false) {
				updateViewToQuestion (m_quizz.CurrentQuestion);
			} else {
				// Stats time
				m_quizz.SendQuizzData (null);

				// Back to the menu
				getBackToMenu ();
			}
		}

		private void getBackToMenu ()
		{
			stopGameTimer ();

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}

		private void pauseAction ()
		{

			m_quizz.IsPaused = !m_quizz.IsPaused;

			if (m_quizz.IsPaused) {

				// Mask game elements
				gameImage.Image = m_pauseImage;

				setGameButtonTitles (null);

				// Pause game
				stopGameTimer ();

			} else {

				gameImage.Image = m_currentImage;

				setGameButtonTitles (m_quizz.CurrentQuestion);

				// Resume game
				setGameTimer ();
			}
		}

		#endregion
	}
}

