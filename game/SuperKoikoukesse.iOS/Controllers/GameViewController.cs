
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
		private NSTimer m_timer, m_animationTimer;
		private UIImage m_currentImage, m_pauseImage;
		private float m_currentPixelateFactor;
		private Random random;

		private GamePauseViewController pauseViewController;

		#region UIView stuff

		public GameViewController ()
			: base ("GameView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
			random = new Random (DateTime.Now.Millisecond);
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
			pauseViewController = new GamePauseViewController();
			pauseViewController.Resume = new Action(() => {
				pauseAction();
			});

			pauseViewController.Quit = new Action(() => {
				getBackToMenu();
			});

			string imgPath = "empty_screen.png";
			m_pauseImage = UIImage.FromFile (imgPath);
			gameImage.Image = m_pauseImage;
		}

		/// <summary>
		/// Initialize a new quizz game
		/// </summary>
		public void InitializeQuizz (GameModes mode, GameDifficulties diff, Filter f)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 

			// Prepare a quizz
			m_quizz = new Quizz ();
			m_quizz.Initialize (mode, diff, appDelegate.Configuration, f);
		
			// Consume one credit
			ProfileService.Instance.UseCredit ();
		}

		/// <summary>
		/// Display quizz and first question
		/// </summary>
		public void DisplayQuizz ()
		{
			updateViewToQuizz (m_quizz);
			updateViewToQuestion (m_quizz.CurrentQuestion);
		}

		/// <summary>
		/// Initialize the view for a new quizz
		/// </summary>
		private void updateViewToQuizz (Quizz q)
		{
			// Set timer in a thread
			var thread = new Thread (setGameTimer as ThreadStart);
			thread.Start ();

			// Display selected mode and difficulty
			modeLabel.Text = m_quizz.Mode.ToString () + " - " + m_quizz.Difficulty;

			// Display lives
			if (q.Mode == GameModes.Survival) {
				livesLabel.Hidden = false;
				livesCountLabel.Hidden = false;
			} else {
				livesLabel.Hidden = true;
				livesCountLabel.Hidden = true;
			}

			// Make sure we're not pausing
			if(pauseViewController != null) {
				pauseViewController.RemoveFromParentViewController();
			}
		}

		/// <summary>
		/// Launch the game timer
		/// </summary>
		private void setGameTimer ()
		{
			using (var pool = new NSAutoreleasePool()) {

				float timerInterval = 0.025f;

				// Every 1 sec we update game timer
				m_timer = NSTimer.CreateRepeatingScheduledTimer (timerInterval, delegate { 
					m_quizz.SubstractTime (timerInterval);

					if (m_quizz.TimeLeft < 0) {

						m_quizz.TimeIsOver ();

						// No answer selected
						this.InvokeOnMainThread (() => {
							nextQuestion ();
						});
					} else {
						// Update timer label and bar (UI Thread!)
						this.InvokeOnMainThread (() => {

							// Find the current bar height
							float maxSize = timeEmpty.Frame.Height;
							float timePercent = (m_quizz.TimeLeft / m_quizz.BaseTimeleft);
							float currentTimeHeight = maxSize * timePercent;

							timeFull.Frame.Height = currentTimeHeight; // Not working. At all. I'm sad.
							timeFullHeightConstraint.Constant = currentTimeHeight;
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

			// Question count
			questionCountLabel.Text = m_quizz.QuestionNumber.ToString ();

			// Lives
			if (m_quizz.Mode == GameModes.Survival) {
				livesCountLabel.Text = m_quizz.Lives.ToString ();
			}
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

				if (m_quizz.IsJokerAvailable) {
					jokerButton.Enabled = true;
				}

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
				// Stop timer
				stopGameTimer ();

				// Stats time
				m_quizz.EndQuizz ();

				// Show score
				var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
				appDelegate.SwitchToScoreView (m_quizz);
			}
		}

		private void getBackToMenu ()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}

		private void pauseAction ()
		{
			m_quizz.IsPaused = !m_quizz.IsPaused;

			if (m_quizz.IsPaused) {

				pauseViewController.View.Frame = View.Frame;
				View.AddSubview(pauseViewController.View);
				View.BringSubviewToFront(pauseViewController.View);

				// Mask game elements
				gameImage.Image = m_pauseImage;

				setGameButtonTitles (null);

				// Pause game
				stopGameTimer ();

			} else {

				pauseViewController.View.RemoveFromSuperview();

				gameImage.Image = m_currentImage;

				setGameButtonTitles (m_quizz.CurrentQuestion);

				// Resume game
				setGameTimer ();
			}
		}

		#endregion

		
		#region Image Transformations

		private UIView progressiveDrawView;
		
		private void setImageAnimation ()
		{
			// image size
			float imageBaseSizeWidth = gameImageScroll.Frame.Width;
			float imageBaseSizeHeight = gameImageScroll.Frame.Height;

			// Stop all running animations
			if (m_animationTimer != null) {
				m_animationTimer.Dispose ();
				m_animationTimer = null;
			}
		
			if (progressiveDrawView != null) {
				progressiveDrawView.RemoveFromSuperview ();
				progressiveDrawView = null;
			}

			string animationKey = "imageTransformation";

			// Stop all animations
			View.Layer.RemoveAnimation (animationKey);
			gameImage.Frame = new RectangleF (0, 0, imageBaseSizeWidth, imageBaseSizeHeight);

			Logger.Log (LogLevel.Debug, "Image transformation: " + m_quizz.ImageTransformation);

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

				UIView.BeginAnimations (animationKey);
				UIView.SetAnimationDuration (Constants.DezoomDuration);
				gameImage.Frame = new RectangleF (0, 0, imageBaseSizeWidth, imageBaseSizeHeight);
				UIView.CommitAnimations ();

				
			} else if (m_quizz.ImageTransformation == ImageTransformations.ProgressiveDrawing) {

				// Random corner
				float targetX = 0f;
				float targetY = 0f;

				int randomX = random.Next (3) - 1;
				int randomY = random.Next (3) - 1;

				switch (randomX) {
					
				case -1:
					// Left
					targetX = gameImage.Frame.X - gameImage.Frame.Width;
					break;
				case 0:
					// No move
					targetX = gameImage.Frame.X;
					break;
				case 1:
					// Right
					targetX = gameImage.Frame.X + gameImage.Frame.Width;
					break;
				}

				// Avoid black suare not moving...
				if (randomX == 0 && randomY == 0) {
					randomY = 1;
				}
				switch (randomY) {
					
				case -1:
					// Top
					targetY = gameImage.Frame.Y - gameImage.Frame.Height;
					break;
				case 0:
					// No move
					targetY = gameImage.Frame.Y;
					break;
				case 1:
					// Bottom
					targetY = gameImage.Frame.Y + gameImage.Frame.Height;
					break;
				}

				// Create black square
				progressiveDrawView = new UIView (
					new RectangleF (gameImage.Frame.X,
				               gameImage.Frame.Y,
				               gameImage.Frame.Width,
				               gameImage.Frame.Height
				)
				);
				progressiveDrawView.BackgroundColor = UIColor.Black;
				gameImageScroll.AddSubview (progressiveDrawView);

				UIView.Animate (
					Constants.DezoomDuration,
					() => {
					progressiveDrawView.Frame = new RectangleF (targetX, targetY, imageBaseSizeWidth, imageBaseSizeHeight);
				});
			} else if (m_quizz.ImageTransformation == ImageTransformations.Pixelization) {
				
				m_currentPixelateFactor = 0.025f;
				pixelateGameImage (m_currentPixelateFactor, imageBaseSizeWidth, imageBaseSizeHeight);
				
				// Thread animation
				var thread = new Thread (() => {

					using (var pool = new NSAutoreleasePool()) {

						float stepDuration = 0.5f;
						float time = 0f;

						m_animationTimer = NSTimer.CreateRepeatingScheduledTimer (stepDuration, delegate { 

							time += (float)m_animationTimer.TimeInterval;

							m_currentPixelateFactor += (stepDuration / Constants.PixelizationDuration) / 5f; // The magic number here is too slow the effet, because after 50% images just look like 100% images.

							if (time >= Constants.PixelizationDuration) {
								if (m_animationTimer != null) {
									m_animationTimer.Dispose ();
								}

								m_currentPixelateFactor = 1f;
							}

							pixelateGameImage (m_currentPixelateFactor, imageBaseSizeWidth, imageBaseSizeHeight);
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
		/// <param name="pixelateFactor">Between 0 and 1</param>
		private void pixelateGameImage (float pixelateFactor, float maxWidth, float maxHeight)
		{
			if (pixelateFactor <= 0f)
				pixelateFactor = 0.01f;
			if (pixelateFactor > 1f)
				pixelateFactor = 1f;

			var a = m_currentImage.Scale (new SizeF ((float)Math.Round (maxWidth * pixelateFactor), (float)Math.Round (maxHeight * pixelateFactor)));
			if (a != null) {
				a.Scale (new SizeF (maxWidth, maxHeight));
				
				this.BeginInvokeOnMainThread (() => {
					gameImage.Image = a;
				});
			}
		}
		
		#endregion
	}
}

