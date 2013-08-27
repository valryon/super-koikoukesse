// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
  [Register("DifficultyView")]
  public class DifficultyView : UIView
  {
    #region Members
    #endregion

    #region Constructors

    public DifficultyView(IntPtr handle) : base(handle)
    {

    }

    #endregion

    #region Methods

    public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
    {
      base.TouchesBegan(touches, evt);

      BackgroundColor = PXNConstants.BRAND_COLOR;
    }

    public override void TouchesEnded(MonoTouch.Foundation.NSSet touches, UIEvent evt)
    {
      base.TouchesEnded(touches, evt);

      BackgroundColor = UIColor.Clear;
    }

    #endregion

    #region Properties
    #endregion
  }
}

