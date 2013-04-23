
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

		private float m_timerBarSize;

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

			m_timerBarSize = timerBarSize.Constant;

			gameImageScroll.Layer.CornerRadius = 27;

			game1Button.TitleLabel.TextAlignment = UITextAlignment.Center;
			game2Button.TitleLabel.TextAlignment = UITextAlignment.Center;
			game3Button.TitleLabel.TextAlignment = UITextAlignment.Center;
			game4Button.TitleLabel.TextAlignment = UITextAlignment.Center;

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
				livesImage.Hidden = false;
			} else {
				livesImage.Hidden = true;
			}

			// Make sure we're not pausing
			if(pauseViewController != null) {
				pauseViewController.View.RemoveFromSuperview();
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
							float pos = m_quizz.TimeLeft * m_timerBarSize / m_quizz.BaseTimeleft;
							if (timerBarSize.Constant > 125f)
								timerLabelSize.Constant = pos - 100f;
							timerBarSize.Constant = pos;
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

			// Score
			scoreLabel.Text = m_quizz.Score.ToString ("000000");

			// Combo
			switch (m_quizz.Combo) {
				case 2:
					comboImage.Image = new UIImage("combo_x2.png");
					break;
				case 3:
					comboImage.Image = new UIImage("combo_x3.png");
					break;
				case 4:
					comboImage.Image = new UIImage("combo_x4.png");
					break;
				default:
					comboImage.Image = new UIImage("combo_x1.png");
					break;
			}

			/*
			 * Joker
			 */

			// Enable the joker if enough questions has been answered correctly
			jokerButton.Enabled = m_quizz.IsJokerAvailable;

			// Animate the joker bottom space constraints to reflect the current state
			UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseOut, () => { 
				switch (m_quizz.JokerPartCount)
				{
					case 0:
						jokerBottomConstraints.Constant = -60;
						break;
					case 1:
						jokerBottomConstraints.Constant = -40;
						break;
					case 2:
						jokerBottomConstraints.Constant = -20;
						break;
					default:
						jokerBottomConstraints.Constant = 0;
						break;
				}

				jokerButton.LayoutIfNeeded();
			}, null);

			// Joker content
			if (Constants.DebugMode) {
				jokerButton.SetTitle ("Joker (" + m_quizz.JokerPartCount + ")", UIControlState.Normal);
			} else {
				jokerButton.SetTitle ("Joker", UIControlState.Normal);
			}

			/*
			 * Questions
			 */

			// Question count
			questionCountLabel.Text = m_quizz.QuestionNumber.ToString ();

			/*
			 * Lives
			 */ 

			// Display the correct number of lives
			if (m_quizz.Mode == GameModes.Survival) {
				switch (m_quizz.Lives)
				{
				case 1:
					livesImage.Image = new UIImage("lives_1.png");
					break;
				case 2:
					livesImage.Image = new UIImage("lives_2.png");
					break;
				case 3:
					livesImage.Image = new UIImage("lives_3.png");
					break;
				default:
					// TODO 0 state if needed
					break;
				}
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

			if(pauseViewController != null) {
				pauseViewController.View.RemoveFromSuperview();
			}

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

				float duration = Constants.ProgressiveDrawingDuration;

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

				// Avoid black square not moving...
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

				// Two directions? Speed is slower (= animation last longer)
				if(randomX != 0 && randomY != 0) {
					duration = duration  + (duration * (1/2));
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
					duration,
					() => {
					progressiveDrawView.Frame = new RectangleF (targetX, targetY, imageBaseSizeWidth, imageBaseSizeHeight);
				});
			} else if (m_quizz.ImageTransformation == ImageTransformations.Pixelization) {
				
				m_currentPixelateFactor = 0.025f;
				pixelateGameImage (m_currentPixelateFactor, imageBaseSizeWidth, imageBaseSizeHeight);
				
				// Thread animation
				var thread = new Thread (() => {

					using (var pool = new NSAutoreleasePool()) {

						float stepDuration = 0.15f;
						float time = 0f;

						m_animationTimer = NSTimer.CreateRepeatingScheduledTimer (stepDuration, delegate { 

							time += (float)m_animationTimer.TimeInterval;

							m_currentPixelateFactor += (stepDuration / Constants.PixelizationDuration) / 5f; // The magic number here is to slow the effet, because after 50% images just look like 100% images.

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

			// Downscale by N
			var a = m_currentImage.Scale (new SizeF ((float)Math.Round (maxWidth * pixelateFactor), (float)Math.Round (maxHeight * pixelateFactor)));
			if (a != null) {

				// Upscale by N
				a.Scale (new SizeF (maxWidth, maxHeight));

				// Convolve by NxN
				// - Create an empty image of the desired size

				this.BeginInvokeOnMainThread (() => {
					gameImage.Image = a;
				});
			}


		}
		
		#endregion
	}
}

