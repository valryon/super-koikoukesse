// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;

namespace SuperKoikoukesse.iOS
{
  [Register("PXNNavigationButton")]
  public class PXNNavigationButton : UIButton
  {
    #region Members
    #endregion

    #region Constructors

    public PXNNavigationButton(IntPtr handle) : base(handle) {}

    public override void AwakeFromNib()
    {
      base.AwakeFromNib();
      Style();
    }

    #endregion

    #region Methods

    public void Style()
    {
      // Button
      AdjustsImageWhenHighlighted = false;

      // Font
      TitleLabel.Font = UIFont.SystemFontOfSize(16);

      // Title
      SetTitleColor(PXNConstants.BRAND_COLOR, UIControlState.Normal);
    }

    // Simple override to set the alpha on the whole button when highlighted
    public override bool Highlighted
    {
      get
      {
        return base.Highlighted;
      }
      set
      {
        base.Highlighted = value;

        if (base.Highlighted)
        {
          this.Alpha = 0.2f;
        }
        else
        {
          // Animate back to the default alpha
          UIView.Animate(0.4f, () => this.Alpha = 1f);
        }
      }
    }

    #endregion

    #region Properties
    #endregion
  }
}

