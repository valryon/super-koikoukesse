// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class SplashscreenViewController : UIViewController
  {
    #region Members
    #endregion

    #region Constructors

    public SplashscreenViewController(IntPtr handle) : base (handle)
    {

    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Set invisible
      View.UserInteractionEnabled = true;
      View.Alpha = 0f;

      // Fade in & out animation
      UIView.Animate(Constants.SPLASHSCREEN_OPEN_FADE_DURATION, 0, UIViewAnimationOptions.CurveEaseIn, OnStart, OnEnd);
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      // Hide the navbar, change the color of the navigationcontroller view
      NavigationController.SetNavigationBarHidden(true, animated);
      NavigationController.View.BackgroundColor = PXNConstants.BRAND_BACKGROUND;
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    private void GoToMenu()
    {
      PerformSegue("SplashToMenu", this);
    }

    #endregion

    #region Handlers

    /// <summary>
    /// Start handler.
    /// </summary>
    private void OnStart()
    {
      View.Alpha = 1f;
    }

    /// <summary>
    /// End handler.
    /// </summary>
    private void OnEnd()
    {
      // Wait
      Thread.Sleep(1000);

      // Animate back to invisible
      UIView.Animate(
        Constants.SPLASHSCREEN_CLOSE_FADE_DURATION,
        () => View.Alpha = 0f, 
        () => BeginInvokeOnMainThread(GoToMenu)
      );
    }

    #endregion

    #region Properties
    #endregion
  }
}

