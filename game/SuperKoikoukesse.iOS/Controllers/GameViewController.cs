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
using MonoTouch.CoreAnimation;

namespace SuperKoikoukesse.iOS
{
	public partial class GameViewController : UIViewController
	{
    #region Constants

		private const float TIMER_UPDATE_FREQUENCY = 0.025f;

    #endregion

    #region Members

		private Quizz mQuizz;
		private NSTimer mTimer;
		private UIImage mCurrentImage;
    private UIImage mPauseImage;
		private Random mRandom;

		private float mTimerBarSize;
		private float mImageTransformationElapsedTime;

		// Image transformations
		private float mAnimationIntervalBase;
    private float mAnimationIntervalCountdown;
		private UIView mProgressiveDrawView;
    private float mProgressiveDrawTargetX;
    private float mProgressiveDrawTargetY;

		private GamePauseViewController mPauseViewController;

    #endregion

    #region Constructors

    public GameViewController () : base ("GameView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
      mRandom = new Random (DateTime.Now.Millisecond);
    }

    #endregion

		#region UIView stuff

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
			mQuizz.UseJoker ();

			nextQuestion ();
		}

		#endregion
		
		#region Game 
    
    public override void ViewDidLoad()
    {
      base.ViewDidLoad ();

      StyleView();

      mTimerBarSize = timerBarSize.Constant;

      // Load the pause/inactive screen image
      mPauseViewController = new GamePauseViewController();
      mPauseViewController.Resume = () => pauseAction();
      mPauseViewController.Quit = () => getBackToMenu();

      mPauseImage = UIImage.FromFile("empty_screen.png");
      gameImage.Image = mPauseImage;
    }

    /// <summary>
    /// Style the main view.
    /// </summary>
    public void StyleView()
    {
      View.BackgroundColor = PXNConstants.BRAND_BACKGROUND;

      scoreLabel.TextColor = PXNConstants.MAIN_TEXT_COLOR;
      scoreTitleLabel.TextColor = PXNConstants.MAIN_TEXT_COLOR;

      gameImageScroll.Layer.CornerRadius = 13;

      // Informations
      StyleInformations(viewInformations);

      // Answers
      StyleAnswers(viewAnswers);

      // Buttons
      StyleButton(game1Button);
      StyleButton(game2Button);
      StyleButton(game3Button);
      StyleButton(game4Button);
    }

    /// <summary>
    /// Style the informations view.
    /// </summary>
    /// <param name="view">View.</param>
    public void StyleInformations(UIView view)
    {
      viewInformations.BackgroundColor = PXNConstants.BRAND_GREY;

      // Bottom border
      var border = new CALayer();
      border.Frame = new RectangleF(0, view.Frame.Height, view.Frame.Width, 1);
      border.BackgroundColor = PXNConstants.BRAND_BORDER.CGColor;

      // Bottom gradient
      var gradient = new CAGradientLayer();
      gradient.Colors = new CGColor[] {
        UIColor.FromHSBA(0, 0, 0.13f, 0.14f).CGColor,
        UIColor.FromHSBA(0, 0, 1f, 0).CGColor
      };
      gradient.Frame = new RectangleF(0, view.Frame.Height + 1, view.Frame.Width, 27);

      // Add layers
      view.Layer.AddSublayer(border);
      view.Layer.AddSublayer(gradient);
    }

    /// <summary>
    /// Style the answers view.
    /// </summary>
    /// <param name="view">View.</param>
    public void StyleAnswers(UIView view)
    {
      view.BackgroundColor = PXNConstants.BRAND_GREY;

      // Border top
      var border = new CALayer();
      border.Frame = new RectangleF(0, -1, view.Frame.Width, 1);
      border.BackgroundColor = PXNConstants.BRAND_BORDER.CGColor;

      // Gradient top
      var gradient = new CAGradientLayer();
      gradient.Colors = new CGColor[] {
        UIColor.FromHSBA(0, 0, 1f, 0).CGColor,
        UIColor.FromHSBA(0, 0, 0.13f, 0.14f).CGColor
      };
      gradient.Frame = new RectangleF(0, -28, view.Frame.Width, 27);

      // Add layers
      view.Layer.AddSublayer(border);
      view.Layer.AddSublayer(gradient);
    }

    /// <summary>
    /// Style a button.
    /// </summary>
    /// <param name="button">Button.</param>
    public void StyleButton(UIButton button)
    {
      button.SetBackgroundImage(new UIImage("button_iPhone.png"), UIControlState.Normal);

      // Text color + size
      button.Font = UIFont.FromName("HelveticaNeue", 15);
      button.SetTitleColor(UIColor.White, UIControlState.Normal);
      button.SetTitleColor(UIColor.FromHSB(0, 0, 0.8f), UIControlState.Highlighted);

      // Shadow color + offset
      button.SetTitleShadowColor(PXNConstants.HALF_ALPHA_BLACK, UIControlState.Normal);
      button.TitleShadowOffset = new SizeF(0, 1);

      // Edge
      button.TitleEdgeInsets = new UIEdgeInsets(0, 5, 0, 5);

      // Alignement + wrap
      button.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
      button.TitleLabel.TextAlignment = UITextAlignment.Center;
    }

		/// <summary>
		/// Initialize a new quizz game
		/// </summary>
		public void InitializeQuizz (GameModes mode, GameDifficulties diff, Filter f)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 

			// Prepare a quizz
			mQuizz = new Quizz ();
			mQuizz.Initialize (mode, diff, appDelegate.Configuration, f);
		
			// Consume one credit
			ProfileService.Instance.UseCredit ();
		}

		/// <summary>
		/// Display quizz and first question
		/// </summary>
		public void DisplayQuizz ()
		{
			updateViewToQuizz (mQuizz);
			updateViewToQuestion (mQuizz.CurrentQuestion);
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
			modeLabel.Text = mQuizz.Mode.ToString () + " - " + mQuizz.Difficulty;

			// Display lives
			if (q.Mode == GameModes.Survival) {
				livesImage.Hidden = false;
			} else {
				livesImage.Hidden = true;
			}

			// Make sure we're not pausing
			if(mPauseViewController != null) {
				mPauseViewController.View.RemoveFromSuperview();
			}
		}

		/// <summary>
		/// Launch the game timer
		/// </summary>
		private void setGameTimer ()
		{
			using (var pool = new NSAutoreleasePool()) {

				// Every 1 sec we update game timer
				mTimer = NSTimer.CreateRepeatingScheduledTimer (TIMER_UPDATE_FREQUENCY, delegate { 

					if (mQuizz.IsPaused == false) 
					{
						mQuizz.SubstractTime (TIMER_UPDATE_FREQUENCY);

						updateImageTransformation(TIMER_UPDATE_FREQUENCY);

						if (mQuizz.TimeLeft < 0) {

							mQuizz.TimeIsOver ();

							// No answer selected
							this.InvokeOnMainThread (() => {
								nextQuestion ();
							});
						} else {
							// Update timer label and bar (UI Thread!)
							this.InvokeOnMainThread (() => {

								// Find the current bar height
								float pos = mQuizz.TimeLeft * mTimerBarSize / mQuizz.BaseTimeleft;
								if (timerBarSize.Constant > 125f)
								{
									timerLabelSize.Constant = pos - 100f;
								}

								timerBarSize.Constant = pos;
								timeLeftLabel.Text = mQuizz.TimeLeft.ToString ("00");
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
			if (mTimer != null) {
				mTimer.Invalidate ();
				mTimer.Dispose ();
				mTimer = null;
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
			timeLeftLabel.Text = mQuizz.TimeLeft.ToString ("00");

			// Image
			string imgPath = ImageService.Instance.Getimage (q.CorrectAnswer);
			mCurrentImage = UIImage.FromFile (imgPath);
			gameImage.Image = mCurrentImage;

			setImageAnimation ();

			// Answers
			setGameButtonTitles (q);

			// Score
			scoreLabel.Text = mQuizz.Score.ToString ("000000");

			// Combo
			switch (mQuizz.Combo) {
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
			jokerButton.Enabled = mQuizz.IsJokerAvailable;

			// Animate the joker bottom space constraints to reflect the current state
			UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseOut, () => { 
				switch (mQuizz.JokerPartCount)
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
				jokerButton.SetTitle ("Joker (" + mQuizz.JokerPartCount + ")", UIControlState.Normal);
			} else {
				jokerButton.SetTitle ("Joker", UIControlState.Normal);
			}

			/*
			 * Questions
			 */

			// Question count
			questionCountLabel.Text = mQuizz.QuestionNumber.ToString ();

			/*
			 * Lives
			 */ 

			// Display the correct number of lives
			if (mQuizz.Mode == GameModes.Survival) {
				switch (mQuizz.Lives)
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

				if (mQuizz.IsJokerAvailable) {
					jokerButton.Enabled = true;
				}

				game1Button.Enabled = true;
				game2Button.Enabled = true;
				game3Button.Enabled = true;
				game4Button.Enabled = true;

				// TODO PAL
				game1Button.SetTitle (q.GetGameTitle (0, GameZones.PAL, mQuizz.TextTransformation), UIControlState.Normal);
				game2Button.SetTitle (q.GetGameTitle (1, GameZones.PAL, mQuizz.TextTransformation), UIControlState.Normal);
				game3Button.SetTitle (q.GetGameTitle (2, GameZones.PAL, mQuizz.TextTransformation), UIControlState.Normal);
				game4Button.SetTitle (q.GetGameTitle (3, GameZones.PAL, mQuizz.TextTransformation), UIControlState.Normal);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">Answer pressed, -1 if no response</param>
		private void gameButtonPressed (int index, bool isJoker = false)
		{
			mQuizz.SelectAnswer (index, isJoker);

			nextQuestion ();
		}

		private void nextQuestion ()
		{
			mQuizz.NextQuestion ();
			
			if (mQuizz.IsOver == false) {
				updateViewToQuestion (mQuizz.CurrentQuestion);
			} else {
				// Stop timer
				stopGameTimer ();

				// Stats time
				mQuizz.EndQuizz ();

				// Show score
				var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
				appDelegate.SwitchToScoreView (mQuizz);
			}
		}

		private void getBackToMenu ()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 

			if(mPauseViewController != null) {
				mPauseViewController.View.RemoveFromSuperview();
			}

			appDelegate.SwitchToMenuView ();
		}

		private void pauseAction ()
		{
			mQuizz.IsPaused = !mQuizz.IsPaused;

			if (mQuizz.IsPaused) {

				mPauseViewController.View.Frame = View.Frame;
				View.AddSubview(mPauseViewController.View);
				View.BringSubviewToFront(mPauseViewController.View);

				// Mask game elements
				gameImage.Image = mPauseImage;

				setGameButtonTitles (null);

			} else {

				mPauseViewController.View.RemoveFromSuperview();

				gameImage.Image = mCurrentImage;

				setGameButtonTitles (mQuizz.CurrentQuestion);
			}
		}

		#endregion

		
		#region Image Transformations



		private void updateImageTransformation(float elapsedTime) {

			if (mQuizz.ImageTransformation == ImageTransformations.None) {
				return;
			}

			mImageTransformationElapsedTime += elapsedTime;

			// Check countdown, to know if we should update image
			mAnimationIntervalCountdown -= elapsedTime;
			if (mAnimationIntervalCountdown <= 0f) {
				mAnimationIntervalCountdown = mAnimationIntervalBase;
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
			if (mQuizz.ImageTransformation == ImageTransformations.Unzoom) {

				float startZoomFactor = Constants.DezoomFactor;
				float duration = Constants.DezoomDuration;

				mImageTransformationElapsedTime = Math.Min (mImageTransformationElapsedTime, duration);

				// From elapsed time and animation duration
				// Get the current zoom factor (10x, 8x, etc)
				// We stop at 1x and not 0x that's why we substract 1 here.
				float stepValue = (startZoomFactor - 1) / duration;
				float currentZoomFactor = startZoomFactor - (mImageTransformationElapsedTime * stepValue);

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
			} else if (mQuizz.ImageTransformation == ImageTransformations.ProgressiveDrawing) {

				float duration = Constants.ProgressiveDrawingDuration;
				mImageTransformationElapsedTime = Math.Min (mImageTransformationElapsedTime, duration);
			
				// All in the UI thread so we can acces UI things properties
				BeginInvokeOnMainThread (() => {

					if (mProgressiveDrawView == null) {

						// Choose a random corner
						float targetX = 0f;
						float targetY = 0f;

						int randomX = mRandom.Next (3) - 1;
						int randomY = mRandom.Next (3) - 1;

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

						mProgressiveDrawTargetX = targetX;
						mProgressiveDrawTargetY = targetY;

						// Create black square
						mProgressiveDrawView = new UIView (
							new RectangleF (0,0,
						                gameImageScroll.Frame.Width,
						                gameImageScroll.Frame.Height
						                )
							);

						mProgressiveDrawView.BackgroundColor = UIColor.Black;
						gameImageScroll.AddSubview (mProgressiveDrawView);
					} 
					else 
					{
						// Move the black square
						// The the movement value for each second
						// Remember that we start from 0,0
						float progressiveDrawingStepValueX = mProgressiveDrawTargetX / duration;
						float progressiveDrawingStepValueY = mProgressiveDrawTargetY / duration;

						// Apply the movement for the elapsed time
						float x = progressiveDrawingStepValueX * mImageTransformationElapsedTime;
						float y = progressiveDrawingStepValueY * mImageTransformationElapsedTime;

						mProgressiveDrawView.Frame = new RectangleF (x, y, gameImageScroll.Frame.Width, gameImageScroll.Frame.Height);
					}
					
				});
			} else if (mQuizz.ImageTransformation == ImageTransformations.Pixelization) {

				// Pixelate!
				GPUImagePixellateFilter pixellateFilter = new GPUImagePixellateFilter ();

				float duration = Constants.PixelizationDuration;
				mImageTransformationElapsedTime = Math.Min (mImageTransformationElapsedTime, duration);

				// Get the pixelate factor
				// From 0 (clear) to 1f (max, do not do that)
				float startPixelateFactor = 0.07f;
				float stepValue = (startPixelateFactor / duration);
				float currentPixelateFactor = startPixelateFactor - (mImageTransformationElapsedTime * stepValue);

				pixellateFilter.FractionalWidthOfAPixel = currentPixelateFactor;

				// Set the filter
				filter = pixellateFilter;
			}  else if (mQuizz.ImageTransformation == ImageTransformations.Test) {

				GPUImageSwirlFilter testFilter = new GPUImageSwirlFilter();

				float duration = 5f;
				mImageTransformationElapsedTime = Math.Min (mImageTransformationElapsedTime, duration);

				float startValue = 1f;
				float stepValue = (startValue / duration);
				float currentValue = startValue + (mImageTransformationElapsedTime * stepValue);

				testFilter.Radius = currentValue;

				// Set the filter
				filter = testFilter;
			} 

			if(filter != null) {

				// Generic filter call
				UIImage processedImage = filter.ImageByFilteringImage (mCurrentImage);

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
			switch (mQuizz.ImageTransformation) {
				case ImageTransformations.Pixelization: 
					mAnimationIntervalBase = Constants.PixelizationDuration / 12f;
					break;
				default:
					// Update with the timer
					mAnimationIntervalBase = TIMER_UPDATE_FREQUENCY;
					break;
			}
			// Reset any related var
			mImageTransformationElapsedTime = 0;

			// Set first image to display
			mAnimationIntervalCountdown = 0f;
			updateImageTransformation (0f);

			// The black square from progressive drawing
			if (mProgressiveDrawView != null) {
				mProgressiveDrawView.RemoveFromSuperview ();
				mProgressiveDrawView = null;
			}
		}
		
		#endregion
	}
}

