// Copyright (c) 2013 Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace SuperKoikoukesse.iOS
{
  public abstract class AbstractCardViewController : UIViewController
  {
    #region Constants

    private const int TAG_SHADOW = 2;
    private const int TAG_CARD = 1;

    #endregion

    #region Members
    #endregion

    #region Constructors

    protected AbstractCardViewController(string nibName) 
      : base(nibName + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null) 
    {
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Shadow
      var shadow = View.ViewWithTag(TAG_SHADOW);
      if (shadow != null)
      {
        // Add the shadow to the background view
        shadow.Layer.CornerRadius = 3f;
        shadow.Layer.ShadowColor = UIColor.Black.CGColor;
        shadow.Layer.ShadowRadius = 1.5f;
        shadow.Layer.ShadowOffset = new SizeF(0, 0);
        shadow.Layer.ShadowOpacity = 0.2f;
      }
      
      // Card
      var card = View.ViewWithTag(TAG_CARD);
      if (card != null)
      {
        // Set the radius & mask on the card
        card.Layer.CornerRadius = 3f;
        card.Layer.MasksToBounds = true;
      }
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    #endregion

    #region Properties
    #endregion
  }
}

