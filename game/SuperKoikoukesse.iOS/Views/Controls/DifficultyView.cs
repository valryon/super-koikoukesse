// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  [Register("DifficultyView")]
  public class DifficultyView : UIView
  {
    #region Members

    private NSLayoutConstraint _leading;
    private float _leadingBaseConstant;

    private NSLayoutConstraint _trailing;
    private float _trailingBaseConstant;

    #endregion

    #region Constructors

    public DifficultyView(IntPtr handle) : base(handle)
    {
      // HACK Not really nice, but...
      foreach (var c in this.Superview.Constraints)
      {
        if (c.FirstAttribute == NSLayoutAttribute.Leading && c.FirstItem == this)
          _leading = c;

        if (c.FirstAttribute == NSLayoutAttribute.Trailing && c.SecondItem == this)
          _trailing = c;
      }

      _leadingBaseConstant = _leading.Constant;
      _trailingBaseConstant = _trailing.Constant;
    }
   
    #endregion

    #region Methods

    public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
    {
      base.TouchesBegan(touches, evt);

      _leading.Constant = 60f;
      _trailing.Constant = 60f;
      this.SetNeedsUpdateConstraints();

      UIView.Animate(
        0.3f,
        () => this.LayoutIfNeeded()
      );
    }

    public override void TouchesEnded(MonoTouch.Foundation.NSSet touches, UIEvent evt)
    {
      base.TouchesEnded(touches, evt);

      _leading.Constant = _leadingBaseConstant;
      _trailing.Constant = _trailingBaseConstant;
      this.SetNeedsUpdateConstraints();

      UIView.Animate(
        0.3f,
        () => this.LayoutIfNeeded()
      );
    }

    #endregion

    #region Properties
    #endregion
  }
}

