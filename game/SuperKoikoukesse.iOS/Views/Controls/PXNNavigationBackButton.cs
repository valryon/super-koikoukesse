// Copyright (c) 2013 Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace SuperKoikoukesse.iOS
{
  [Register("PXNNavigationBackButton")]
  public class PXNNavigationBackButton : PXNNavigationButton
  {
    #region Constants

    public static readonly UIImage BACK_IMAGE = new UIImage("button_arrow.png");

    #endregion

    #region Members
    #endregion

    #region Constructors

    public PXNNavigationBackButton(IntPtr handle) : base(handle) {}

    public override void AwakeFromNib()
    {
      base.AwakeFromNib();

      TitleEdgeInsets = new UIEdgeInsets(0, 10, 0, 0);
      SetImage(BACK_IMAGE, UIControlState.Normal);
    }

    #endregion

    #region Methods

    public override SizeF IntrinsicContentSize
    {
      get
      {
        // Get the old one
        var baseSize = base.IntrinsicContentSize;

        // Add the TitleEdgeInsets values to the intrinsic size
        return new SizeF(baseSize.Width + this.TitleEdgeInsets.Left + this.TitleEdgeInsets.Right,
                         baseSize.Height + this.TitleEdgeInsets.Top + this.TitleEdgeInsets.Bottom);
      }
    }

    #endregion

    #region Properties
    #endregion
  }
}

