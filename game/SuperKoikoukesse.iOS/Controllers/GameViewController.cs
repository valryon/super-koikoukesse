
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;
using System.Threading;
using MonoTouch.CoreGraphics;
using GPUImage;
using GPUImage.Filters;

namespace SuperKoikoukesse.iOS
{
	public partial class GameViewController : UIViewController
	{
		private const float timerUpdateFrequency = 0.025f;

		private Quizz m_quizz;
		private NSTimer m_timer;
		private UIImage m_currentImage, m_pauseImage;
		private Random random;

		private float m_timerBarSize;
		private float imageTransformationElapsedTime;

		// Image transformations
		private float animationIntervalBase,animationIntervalCountdown;
		private UIView progressiveDrawView;
		private float progressiveDrawTargetX,progressiveDrawTargetY;

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

				// Every 1 sec we update game timer
				m_timer = NSTimer.CreateRepeatingScheduledTimer (timerUpdateFrequency, delegate { 

					if (m_quizz.IsPaused == false) 
					{
						m_quizz.SubstractTime (timerUpdateFrequency);

						updateImageTransformation(timerUpdateFrequency);

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
								{
									timerLabelSize.Constant = pos - 100f;
								}

								timerBarSize.Constant = pos;
								timeLeftLabel.Text = m_quizz.TimeLeft.ToString ("00");
							});
						}
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

			} else {

				pauseViewController.View.RemoveFromSuperview();

				gameImage.Image = m_currentImage;

				setGameButtonTitles (m_quizz.CurrentQuestion);
			}
		}

		#endregion

		
		#region Image Transformations



		private void updateImageTransformation(float elapsedTime) {

			if (m_quizz.ImageTransformation == ImageTransformations.None) {
				return;
			}

			imageTransformationElapsedTime += elapsedTime;

			// Check countdown, to know if we should update image
			animationIntervalCountdown -= elapsedTime;
			if (animationIntervalCountdown <= 0f) {
				animationIntervalCountdown = animationIntervalBase;
			} else {
				// Do not update 
				return;
			}

			// Update the image transformation 
			// REMEMBER WE ARE ON THE TIMER THREAD
			// Not the UI one.
			// Don't worry, system will bring it back to you if you forget. So kind.
			GPUImageFilter filter = null;

			// Zoom and unzoom
			if (m_quizz.ImageTransformation == ImageTransformations.Unzoom) {

				float startZoomFactor = Constants.DezoomFactor;
				float duration = Constants.DezoomDuration;

				imageTransformationElapsedTime = Math.Min (imageTransformationElapsedTime, duration);

				// From elapsed time and animation duration
				// Get the current zoom factor (10x, 8x, etc)
				// We stop at 1x and not 0x that's why we substract 1 here.
				float stepValue = (startZoomFactor - 1) / duration;
				float currentZoomFactor = startZoomFactor - (imageTransformationElapsedTime * stepValue);

				BeginInvokeOnMainThread (() => {

					// image size
					float imageBaseSizeWidth = gameImageScroll.Frame.Width;
					float imageBaseSizeHeight = gameImageScroll.Frame.Height;

					float width = imageBaseSizeWidth * currentZoomFactor;
					float height = imageBaseSizeHeight * currentZoomFactor;

					width = Math.Max(width, imageBaseSizeWidth);
					height = Math.Max(height, imageBaseSizeHeight);

					// Center largest image in the scroll view
					float x = (imageBaseSizeWidth/2) - (width / 2);
					float y = (imageBaseSizeHeight/2) - (height / 2);

					gameImage.Frame = new RectangleF (x, y, width, height);
				});
			} else if (m_quizz.ImageTransformation == ImageTransformations.ProgressiveDrawing) {

				float duration = Constants.ProgressiveDrawingDuration;
				imageTransformationElapsedTime = Math.Min (imageTransformationElapsedTime, duration);
			
				// All in the UI thread so we can acces UI things properties
				BeginInvokeOnMainThread (() => {

					if (progressiveDrawView == null) {

						// Choose a random corner
						float targetX = 0f;
						float targetY = 0f;

						int randomX = random.Next (3) - 1;
						int randomY = random.Next (3) - 1;

						switch (randomX) {

							case -1:
							// Left
							targetX = - gameImageScroll.Frame.Width;
							break;
							case 0:
							// No move
							targetX = 0;
							break;
							case 1:
							// Right
							targetX = gameImageScroll.Frame.Width;
							break;
						}

						// Avoid black square not moving...
						if (randomX == 0 && randomY == 0) {
							randomY = 1;
						}
						switch (randomY) {

							case -1:
							// Top
							targetY = - gameImageScroll.Frame.Height;
							break;
							case 0:
							// No move
							targetY = 0;
							break;
							case 1:
							// Bottom
							targetY = gameImageScroll.Frame.Height;
							break;
						}

						// Two directions? Speed is slower (= animation last longer)
						if(randomX != 0 && randomY != 0) {
							duration = duration  + (duration * (1/2));
						}

						progressiveDrawTargetX = targetX;
						progressiveDrawTargetY = targetY;

						// Create black square
						progressiveDrawView = new UIView (
							new RectangleF (0,0,
						                gameImageScroll.Frame.Width,
						                gameImageScroll.Frame.Height
						                )
							);

						progressiveDrawView.BackgroundColor = UIColor.Black;
						gameImageScroll.AddSubview (progressiveDrawView);
					} 
					else 
					{
						// Move the black square
						// The the movement value for each second
						// Remember that we start from 0,0
						float progressiveDrawingStepValueX = progressiveDrawTargetX / duration;
						float progressiveDrawingStepValueY = progressiveDrawTargetY / duration;

						// Apply the movement for the elapsed time
						float x = progressiveDrawingStepValueX * imageTransformationElapsedTime;
						float y = progressiveDrawingStepValueY * imageTransformationElapsedTime;

						progressiveDrawView.Frame = new RectangleF (x, y, gameImageScroll.Frame.Width, gameImageScroll.Frame.Height);
					}
					
				});
			} else if (m_quizz.ImageTransformation == ImageTransformations.Pixelization) {

				// Pixelate!
				GPUImagePixellateFilter pixellateFilter = new GPUImagePixellateFilter ();

				float duration = Constants.PixelizationDuration;
				imageTransformationElapsedTime = Math.Min (imageTransformationElapsedTime, duration);

				// Get the pixelate factor
				// From 0 (clear) to 1f (max, do not do that)
				float startPixelateFactor = 0.07f;
				float stepValue = (startPixelateFactor / duration);
				float currentPixelateFactor = startPixelateFactor - (imageTransformationElapsedTime * stepValue);

				pixellateFilter.FractionalWidthOfAPixel = currentPixelateFactor;

				// Set the filter
				filter = pixellateFilter;
			}  else if (m_quizz.ImageTransformation == ImageTransformations.Test) {

				// Pixelate!
				GPUImagePixellateFilter pixellateFilter = new GPUImagePixellateFilter ();

				float duration = Constants.PixelizationDuration;
				imageTransformationElapsedTime = Math.Min (imageTransformationElapsedTime, duration);

				// Get the pixelate factor
				// From 0 (clear) to 1f (max, do not do that)
				float startPixelateFactor = 0.01f;
				float stepValue = (startPixelateFactor / duration);
				float currentPixelateFactor = startPixelateFactor - (imageTransformationElapsedTime * stepValue);

				pixellateFilter.FractionalWidthOfAPixel = currentPixelateFactor;

				// Set the filter
				filter = pixellateFilter;
			} 

			if(filter != null) {

				// Generic filter call
				UIImage processedImage = filter.ImageByFilteringImage (m_currentImage);

				// Set the image
				BeginInvokeOnMainThread (() => {
					gameImage.Image = processedImage;
				});
			}
		}

		/// <summary>
		/// Initialize image transformation
		/// </summary>
		private void setImageAnimation ()
		{
			// Define frequency
			switch (m_quizz.ImageTransformation) {
				case ImageTransformations.Pixelization: 
					animationIntervalBase = Constants.PixelizationDuration / 12f;
					break;
				default:

					animationIntervalBase = timerUpdateFrequency;
					break;
			}
			// Reset any related var
			imageTransformationElapsedTime = 0;

			// Set first image to display
			animationIntervalCountdown = 0f;
			updateImageTransformation (0f);

			// The black square from progressive drawing
			if (progressiveDrawView != null) {
				progressiveDrawView.RemoveFromSuperview ();
				progressiveDrawView = null;
			}
		}
		
		#endregion
	}
}

