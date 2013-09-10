// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
  /// <summary>
  /// A button that manages its content inset for AutoLayout.
  /// </summary>
  [Register("PXNButton")]
  public class PXNButton : UIButton
  {
    #region Members
    #endregion

    #region Constructors

    public PXNButton(IntPtr handle) : base(handle) {}

    #endregion

    #region Methods

    /// <summary>
    /// Gets the size of the intrinsic content.
    /// Override the base method to handle the TitleEdgeInsets size.
    /// </summary>
    /// <value>The size of the intrinsic content.</value>
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

