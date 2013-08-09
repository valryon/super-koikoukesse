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

    public GameViewController() : base("GameView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
      mRandom = new Random(DateTime.Now.Millisecond);
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      StyleView();

      mTimerBarSize = ConstraintTimer.Constant;

      // Load the pause/inactive screen image
      mPauseViewController = new GamePauseViewController();
      mPauseViewController.Resume = Pause;
      mPauseViewController.Quit = Back;

      mPauseImage = UIImage.FromFile("empty_screen.png");
      ImageGame.Image = mPauseImage;
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      SetShadow(ViewImageShadow);
    }

    #endregion

    #region Methods

    #region Styling

    /// <summary>
    /// Style the main view.
    /// </summary>
    public void StyleView()
    {
      ViewCombo.Layer.CornerRadius = 6;
      ViewCombo.Layer.MasksToBounds = true;
      ViewImageShadow.Layer.CornerRadius = 13;
      ScrollViewImageGame.Layer.CornerRadius = 13;

      CreateParticlesEngine(ViewEmitter);

      // Informations
      StyleInformations(ViewInformations);

      // Answers
      StyleAnswers(ViewAnswers);

      // Buttons
      ButtonGame1.TitleLabel.TextAlignment = UITextAlignment.Center;
      ButtonGame2.TitleLabel.TextAlignment = UITextAlignment.Center;
      ButtonGame3.TitleLabel.TextAlignment = UITextAlignment.Center;
      ButtonGame4.TitleLabel.TextAlignment = UITextAlignment.Center;
    }

    public void SetShadow(UIView view)
    {
      view.Layer.ShadowColor = UIColor.Black.CGColor;
      view.Layer.ShadowOpacity = 0.4f;
      view.Layer.ShadowRadius = 10f;
    }

    public void CreateParticlesEngine(UIView view)
    {
      var emitter = new PXNComboGauge(
        x: ViewEmitter.Frame.Width,
        y: ViewEmitter.Frame.Height / 2,
        width: 1f,
        height: ViewEmitter.Frame.Height - 4
      );

      view.Layer.AddSublayer(emitter);
    }

    /// <summary>
    /// Style the informations view.
    /// </summary>
    /// <param name="view">View.</param>
    public void StyleInformations(UIView view)
    {
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

    #endregion

    #region Quizz

    /// <summary>
    /// Initialize a new quizz game
    /// </summary>
    public void InitializeQuizz(GameMode mode, GameDifficulties diff, Filter f)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 

      // Prepare a quizz
      mQuizz = new Quizz();
      mQuizz.Initialize(mode, diff, appDelegate.Configuration, f);
    
      // Consume one credit
      PlayerCache.Instance.UseCredit();
    }

    /// <summary>
    /// Display quizz and first question
    /// </summary>
    public void DisplayQuizz()
    {
      UpdateViewWithQuizz(mQuizz);
      UpdateViewWithQuestion(mQuizz.CurrentQuestion);
    }

    /// <summary>
    /// Initialize the view for a new quizz
    /// </summary>
    private void UpdateViewWithQuizz(Quizz q)
    {
      // Set timer in a thread
      var thread = new Thread(SetGameTimer as ThreadStart);
      thread.Start();

      // Display selected mode and difficulty
      LabelMode.Text = mQuizz.Mode.ToString() + " - " + mQuizz.Difficulty;

      // Display lives
//      if (q.Mode == GameMode.SURVIVAL)
//      {
//        livesImage.Hidden = false;
//      }
//      else
//      {
//        livesImage.Hidden = true;
//      }

      // Make sure we're not pausing
      if (mPauseViewController != null)
      {
        mPauseViewController.View.RemoveFromSuperview();
      }
    }

    #endregion

    #region Navigation

    private void Back()
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 

      if (mPauseViewController != null)
      {
        mPauseViewController.View.RemoveFromSuperview();
      }

      appDelegate.SwitchToMenuView();
    }

    private void Pause()
    {
      mQuizz.IsPaused = !mQuizz.IsPaused;

      if (mQuizz.IsPaused)
      {
        mPauseViewController.View.Frame = View.Window.Frame;
        View.AddSubview(mPauseViewController.View);
        View.BringSubviewToFront(mPauseViewController.View);

        // Mask game elements
        ImageGame.Image = mPauseImage;

        SetButtonsTitle(null);
      }
      else
      {
        mPauseViewController.View.RemoveFromSuperview();

        ImageGame.Image = mCurrentImage;

        SetButtonsTitle(mQuizz.CurrentQuestion);
      }
    }

    #endregion

    #region Game

    /// <summary>
    /// Launch the game timer
    /// </summary>
    private void SetGameTimer()
    {
      using (var pool = new NSAutoreleasePool())
      {

        // Every 1 sec we update game timer
        mTimer = NSTimer.CreateRepeatingScheduledTimer(TIMER_UPDATE_FREQUENCY, delegate
          { 

          if (mQuizz.IsPaused == false)
          {
            mQuizz.SubstractTime(TIMER_UPDATE_FREQUENCY);

            updateImageTransformation(TIMER_UPDATE_FREQUENCY);

            if (mQuizz.TimeLeft < 0)
            {

              mQuizz.SetTimeOver();

              // No answer selected
              this.InvokeOnMainThread(() => {
                GoToNextQuestion();
              });
            }
            else
            {
              // Update timer label and bar (UI Thread!)
              this.InvokeOnMainThread(() => {

                // Find the current bar height
                float pos = mQuizz.TimeLeft * mTimerBarSize / mQuizz.BaseTimeleft;
                if (ConstraintTimer.Constant > 125f)
                {
                  ConstraintTimer.Constant = pos - 100f;
                }

                ConstraintTimer.Constant = pos;
                LabelTime.Text = mQuizz.TimeLeft.ToString("00");
              });
            }
          }
          });

        NSRunLoop.Current.Run();
      }
    }

    /// <summary>
    /// Stop the game timer
    /// </summary>
    private void StopGameTimer()
    {
      if (mTimer != null)
      {
        mTimer.Invalidate();
        mTimer.Dispose();
        mTimer = null;
      }
    }

    /// <summary>
    /// Setup all the view elements for a given question
    /// </summary>
    /// <param name="q">Q.</param>
    private void UpdateViewWithQuestion(Question q)
    {
      Logger.I ( "Setting up view for current question " + q);

      // Timer
      LabelTime.Text = mQuizz.TimeLeft.ToString("00");

      // Image
      string imgPath = ImageDatabase.Instance.Getimage(q.CorrectAnswer);
      mCurrentImage = UIImage.FromFile(imgPath);
      ImageGame.Image = mCurrentImage;

      setImageAnimation();

      // Answers
      SetButtonsTitle(q);

      // Score
      LabelScore.Text = mQuizz.Score.ToString("000000");

      // Combo
      var width = 0f;
      switch (mQuizz.Combo)
      {
        case 2:
          LabelCombo.Text = "x2";
          width = 82f;
          break;
        case 3:
          LabelCombo.Text = "x3";
          width = 41f;
          break;
        case 4:
          LabelCombo.Text = "x4";
          width = 0f;
          break;
        default:
          LabelCombo.Text = "x1";
          width = 123f;
          break;
      }

      UIView.Animate(1f, 0.1f, UIViewAnimationOptions.CurveEaseIn, () => 
      {
        ConstraintCombo.Constant = width;
        //ViewEmitter.SetNeedsLayout();
        ViewEmitter.LayoutIfNeeded();
      }, null);

      /*
       * Joker
       */

      // Enable the joker if enough questions has been answered correctly
      ButtonJoker.Enabled = mQuizz.IsJokerAvailable;

      // Animate the joker bottom space constraints to reflect the current state
      UIView.Animate(0.3, 0, UIViewAnimationOptions.CurveEaseOut, () => { 
        switch (mQuizz.JokerPartCount)
        {
          case 0:
            ConstraintJoker.Constant = -60;
            break;
          case 1:
            ConstraintJoker.Constant = -40;
            break;
          case 2:
            ConstraintJoker.Constant = -20;
            break;
          default:
            ConstraintJoker.Constant = 0;
            break;
        }

        ButtonJoker.LayoutIfNeeded();
      }, null);

      // Joker content
      if (Constants.DEBUG_MODE)
      {
        ButtonJoker.SetTitle("Joker (" + mQuizz.JokerPartCount + ")", UIControlState.Normal);
      }
      else
      {
        ButtonJoker.SetTitle("Joker", UIControlState.Normal);
      }

      /*
       * Questions
       */

      // Question count
      LabelCount.Text = mQuizz.QuestionNumber.ToString();

      /*
       * Lives
       */ 

      // Display the correct number of lives
      if (mQuizz.Mode == GameMode.SURVIVAL)
      {
//        switch (mQuizz.Lives)
//        {
//          case 1:
//            livesImage.Image = new UIImage("lives_1.png");
//            break;
//          case 2:
//            livesImage.Image = new UIImage("lives_2.png");
//            break;
//          case 3:
//            livesImage.Image = new UIImage("lives_3.png");
//            break;
//          default:
//          // TODO 0 state if needed
//            break;
//        }
      }
    }

    private void SetButtonsTitle(Question q)
    {
      // Disable buttons
      if (q == null)
      {

        ButtonJoker.Enabled = false;

        ButtonGame1.Enabled = false;
        ButtonGame2.Enabled = false;
        ButtonGame3.Enabled = false;
        ButtonGame4.Enabled = false;
        ButtonGame1.SetTitle("", UIControlState.Normal);
        ButtonGame2.SetTitle("", UIControlState.Normal);
        ButtonGame3.SetTitle("", UIControlState.Normal);
        ButtonGame4.SetTitle("", UIControlState.Normal);
      } 
      // Buttons for current question
      else
      {

        if (mQuizz.IsJokerAvailable)
        {
          ButtonJoker.Enabled = true;
        }

        ButtonGame1.Enabled = true;
        ButtonGame2.Enabled = true;
        ButtonGame3.Enabled = true;
        ButtonGame4.Enabled = true;

        // TODO PAL
        ButtonGame1.SetTitle(q.GetGameTitle(0, GameZone.PAL, mQuizz.TextTransformation), UIControlState.Normal);
        ButtonGame2.SetTitle(q.GetGameTitle(1, GameZone.PAL, mQuizz.TextTransformation), UIControlState.Normal);
        ButtonGame3.SetTitle(q.GetGameTitle(2, GameZone.PAL, mQuizz.TextTransformation), UIControlState.Normal);
        ButtonGame4.SetTitle(q.GetGameTitle(3, GameZone.PAL, mQuizz.TextTransformation), UIControlState.Normal);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">Answer pressed, -1 if no response</param>
    private void Answer(int index, bool isJoker = false)
    {
      mQuizz.SelectAnswer(index, isJoker);

      GoToNextQuestion();
    }

    private void GoToNextQuestion()
    {
      mQuizz.SetNextQuestion();
      
      if (mQuizz.IsOver == false)
      {
        UpdateViewWithQuestion(mQuizz.CurrentQuestion);
      }
      else
      {
        // Stop timer
        StopGameTimer();

        // Stats time
        mQuizz.EndQuizz();

        // Show score
        var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
        appDelegate.SwitchToScoreView(mQuizz);
      }
    }

    #endregion

    #endregion

    #region Handlers

    partial void OnButton1Touched(NSObject sender)
    {
      Answer(0);
    }

    partial void OnButton2Touched(NSObject sender)
    {
      Answer(1);
    }

    partial void OnButton3Touched(NSObject sender)
    {
      Answer(2);
    }

    partial void OnButton4Touched(NSObject sender)
    {
      Answer(3);
    }

    partial void OnPauseTouched(NSObject sender)
    {
      Pause();
    }

    partial void OnJokerTouched(NSObject sender)
    {
      mQuizz.UseJoker();

      GoToNextQuestion();
    }

    #endregion

    #region Image Transformations
    private void updateImageTransformation(float elapsedTime)
    {

      if (mQuizz.ImageTransformation == ImageTransformations.NONE)
      {
        return;
      }

      mImageTransformationElapsedTime += elapsedTime;

      // Check countdown, to know if we should update image
      mAnimationIntervalCountdown -= elapsedTime;
      if (mAnimationIntervalCountdown <= 0f)
      {
        mAnimationIntervalCountdown = mAnimationIntervalBase;
      }
      else
      {
        // Do not update 
        return;
      }

      // Update the image transformation 
      // REMEMBER WE ARE ON THE TIMER THREAD
      // Not the UI one.
      // Don't worry, system will bring it back to you if you forget. So kind.
      GPUImageFilter filter = null;

      // Zoom and unzoom
      if (mQuizz.ImageTransformation == ImageTransformations.UNZOOM)
      {

        float startZoomFactor = Constants.ANIMATION_DEZOOM_FACTOR;
        float duration = Constants.ANIMATION_DEZOOM_DURATION;

        mImageTransformationElapsedTime = Math.Min(mImageTransformationElapsedTime, duration);

        // From elapsed time and animation duration
        // Get the current zoom factor (10x, 8x, etc)
        // We stop at 1x and not 0x that's why we substract 1 here.
        float stepValue = (startZoomFactor - 1) / duration;
        float currentZoomFactor = startZoomFactor - (mImageTransformationElapsedTime * stepValue);

        BeginInvokeOnMainThread(() => {

          // image size
          float imageBaseSizeWidth = ScrollViewImageGame.Frame.Width;
          float imageBaseSizeHeight = ScrollViewImageGame.Frame.Height;

          float width = imageBaseSizeWidth * currentZoomFactor;
          float height = imageBaseSizeHeight * currentZoomFactor;

          width = Math.Max(width, imageBaseSizeWidth);
          height = Math.Max(height, imageBaseSizeHeight);

          // Center largest image in the scroll view
          float x = (imageBaseSizeWidth / 2) - (width / 2);
          float y = (imageBaseSizeHeight / 2) - (height / 2);

          ImageGame.Frame = new RectangleF(x, y, width, height);
        });
      }
      else if (mQuizz.ImageTransformation == ImageTransformations.PROGRESSIVE_DRAWING)
      {

        float duration = Constants.ANIMATION_PROGRESSIVE_DRAWING_DURATION;
        mImageTransformationElapsedTime = Math.Min(mImageTransformationElapsedTime, duration);
			
        // All in the UI thread so we can acces UI things properties
        BeginInvokeOnMainThread(() => {

          if (mProgressiveDrawView == null)
          {

            // Choose a random corner
            float targetX = 0f;
            float targetY = 0f;

            int randomX = mRandom.Next(3) - 1;
            int randomY = mRandom.Next(3) - 1;

            switch (randomX)
            {

              case -1:
							// Left
                targetX = - ScrollViewImageGame.Frame.Width;
                break;
              case 0:
							// No move
                targetX = 0;
                break;
              case 1:
							// Right
                targetX = ScrollViewImageGame.Frame.Width;
                break;
            }

            // Avoid black square not moving...
            if (randomX == 0 && randomY == 0)
            {
              randomY = 1;
            }
            switch (randomY)
            {

              case -1:
							// Top
                targetY = - ScrollViewImageGame.Frame.Height;
                break;
              case 0:
							// No move
                targetY = 0;
                break;
              case 1:
							// Bottom
                targetY = ScrollViewImageGame.Frame.Height;
                break;
            }

            // Two directions? Speed is slower (= animation last longer)
            if (randomX != 0 && randomY != 0)
            {
              duration = duration + (duration * (1 / 2));
            }

            mProgressiveDrawTargetX = targetX;
            mProgressiveDrawTargetY = targetY;

            // Create black square
            mProgressiveDrawView = new UIView(
              new RectangleF(0, 0,
                       ScrollViewImageGame.Frame.Width,
                       ScrollViewImageGame.Frame.Height
            )
            );

            mProgressiveDrawView.BackgroundColor = UIColor.Black;
            ScrollViewImageGame.AddSubview(mProgressiveDrawView);
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

            mProgressiveDrawView.Frame = new RectangleF(x, y, ScrollViewImageGame.Frame.Width, ScrollViewImageGame.Frame.Height);
          }
					
        });
      }
      else if (mQuizz.ImageTransformation == ImageTransformations.PIXELIZATION)
      {

        // Pixelate!
        GPUImagePixellateFilter pixellateFilter = new GPUImagePixellateFilter();

        float duration = Constants.ANIMATION_PIXELISATION_DURATION;
        mImageTransformationElapsedTime = Math.Min(mImageTransformationElapsedTime, duration);

        // Get the pixelate factor
        // From 0 (clear) to 1f (max, do not do that)
        float startPixelateFactor = 0.07f;
        float stepValue = (startPixelateFactor / duration);
        float currentPixelateFactor = startPixelateFactor - (mImageTransformationElapsedTime * stepValue);

        pixellateFilter.FractionalWidthOfAPixel = currentPixelateFactor;

        // Set the filter
        filter = pixellateFilter;
      }
      else if (mQuizz.ImageTransformation == ImageTransformations.TEST)
      {

        GPUImageSwirlFilter testFilter = new GPUImageSwirlFilter();

        float duration = 5f;
        mImageTransformationElapsedTime = Math.Min(mImageTransformationElapsedTime, duration);

        float startValue = 1f;
        float stepValue = (startValue / duration);
        float currentValue = startValue + (mImageTransformationElapsedTime * stepValue);

        testFilter.Radius = currentValue;

        // Set the filter
        filter = testFilter;
      } 

      if (filter != null)
      {

        // Generic filter call
        UIImage processedImage = filter.ImageByFilteringImage(mCurrentImage);

        // Set the image
        BeginInvokeOnMainThread(() => {
          ImageGame.Image = processedImage;
        });
      }
    }

    /// <summary>
    /// Initialize image transformation
    /// </summary>
    private void setImageAnimation()
    {
      // Define frequency
      switch (mQuizz.ImageTransformation)
      {
        case ImageTransformations.PIXELIZATION: 
          mAnimationIntervalBase = Constants.ANIMATION_PIXELISATION_DURATION / 12f;
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
      updateImageTransformation(0f);

      // The black square from progressive drawing
      if (mProgressiveDrawView != null)
      {
        mProgressiveDrawView.RemoveFromSuperview();
        mProgressiveDrawView = null;
      }
    }
    #endregion
  }
}

