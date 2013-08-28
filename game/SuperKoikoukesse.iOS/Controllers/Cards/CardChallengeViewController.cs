// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Threading.Tasks;
using System.Threading;

namespace SuperKoikoukesse.iOS
{
  public partial class CardChallengeViewController : AbstractCardViewController
  {
    #region Members

    private GameMode _mode;

    private UIView _originalView;

    #endregion

    #region Constructors

    public CardChallengeViewController() : base("CardChallengeView")
    {
    }

    #endregion

    #region Methods

    public void SetMode(GameMode m)
    {
      _mode = m;
    }

    public void AddToView(UIView superview)
    {
      // Store the original view
      // We must store this view because it will be removed after.
      // Then, the challengeview will be flattened in the scroll view.
      // If we don't keep it, the previous card will disappear.
      _originalView = superview;

      // Add the challengeview to the main view
      superview.AddSubview(this.View);
      this.View.Frame = superview.Frame;

      IsPresented = true;

      // Transition
      UIView.Transition(
        fromView:   superview,
        toView:     this.View,
        duration:   0.6, 
        options:    UIViewAnimationOptions.TransitionFlipFromRight, 
        completion: null
      );
    }

    public void RemoveFromView()
    {
      // Add the original view instead
      View.Superview.AddSubview(_originalView);

      // Transition
      UIView.Transition(
        fromView:   View,
        toView:     _originalView,
        duration:   0.6, 
        options:    UIViewAnimationOptions.TransitionFlipFromLeft, 
        completion: null
      );  

      // Then, remove the challengeview
      View.RemoveFromSuperview();

      IsPresented = false;
    }

    #endregion

    #region Handlers

    partial void OnHideTouched(NSObject sender)
    {
      // Wait a little to let the button recover from the highlighted state.
      UIView.Animate(
        0.1f,
        () => ButtonHide.Highlighted = false,
        () => {
          if (Hidden != null)
            Hidden();
        }
      );
    }

    #endregion

    #region Properties

    public event Action<GameMode, GameDifficulties> DifficultySelected;
    public event Action Hidden;

    public bool IsPresented
    {
      get;
      private set;
    }

    #endregion
  }
}

