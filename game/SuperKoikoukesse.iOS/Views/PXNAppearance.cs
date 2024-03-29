// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Drawing;
using MonoTouch.CoreAnimation;

namespace SuperKoikoukesse.iOS
{
  public class PXNAppearance
  {
    #region Members
    #endregion

    #region Constructors

    public PXNAppearance()
    {
      SetNavigationBarAppearance();
      SetNavigationItemAppearance();
      SetPageControlAppearance();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Set the appearance of the navigation bar.
    /// </summary>
    private void SetNavigationBarAppearance()
    {
      // appearance
      var bar = UINavigationBar.Appearance;

      // text attributes
      var attributes = new UITextAttributes();
      attributes.TextColor = UIColor.FromWhiteAlpha(white: 0.306f, alpha: 1f);
      attributes.TextShadowOffset = new UIOffset(0, 0);
      attributes.TextShadowColor = UIColor.Clear;
      attributes.Font = UIFont.SystemFontOfSize(16);

      // navbar
      bar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
      bar.BackgroundColor = PXNConstants.COLOR_NAVIGATION;
      bar.ShadowImage = CreateNavigationBarShadowImage();
      bar.SetTitleTextAttributes(attributes);
      bar.SetTitleVerticalPositionAdjustment(3f, UIBarMetrics.Default);
    }

    private UIImage CreateNavigationBarShadowImage()
    {
      // Create the border
      var border = new CALayer();
      border.Frame = new RectangleF(0, 0, 1, 1);
      border.BackgroundColor = PXNConstants.BRAND_BORDER.CGColor;

      // Init context
      UIGraphics.BeginImageContextWithOptions(new SizeF(1f, 0.5f), true, 0f);

      // Render
      border.RenderInContext(UIGraphics.GetCurrentContext());
      var image = UIGraphics.GetImageFromCurrentImageContext();

      // End context
      UIGraphics.EndImageContext();

      return image;
    }

    /// <summary>
    /// Customize the appearance of the navigation item (UiBarButtonItem).
    /// </summary>
    private void SetNavigationItemAppearance()
    {
      // appearance
      var item = UIBarButtonItem.Appearance;

      // normal text attributes
      var normal = new UITextAttributes();
      normal.TextColor = PXNConstants.BRAND_COLOR;
      normal.TextShadowOffset = new UIOffset(0, 0);
      normal.TextShadowColor = UIColor.Clear;
      normal.Font = UIFont.SystemFontOfSize(16);

      // highlight text attributes
      var highlight = new UITextAttributes();
      highlight.TextColor = UIColor.White;
      highlight.TextShadowOffset = new UIOffset(0, 0);
      highlight.TextShadowColor = UIColor.Clear;
      highlight.Font = UIFont.SystemFontOfSize(16);

      // button
      item.SetTitleTextAttributes(normal, UIControlState.Normal);
      item.SetTitleTextAttributes(highlight, UIControlState.Highlighted);
      item.SetBackgroundImage(new UIImage(), UIControlState.Normal, UIBarMetrics.Default);
      item.SetBackgroundVerticalPositionAdjustment(2f, UIBarMetrics.Default);

      // back button
      var image = new UIImage("button_arrow.png").CreateResizableImage(new UIEdgeInsets(0, 11, 0, 0));
      item.SetBackButtonBackgroundImage(image, UIControlState.Normal, UIBarMetrics.Default);
      item.SetBackButtonTitlePositionAdjustment(new UIOffset(4f, 0), UIBarMetrics.Default);
      item.SetBackButtonBackgroundVerticalPositionAdjustment(1f, UIBarMetrics.Default);
    }

    public void SetPageControlAppearance()
    {
      // appearance
      var item = UIPageControl.Appearance;

      item.CurrentPageIndicatorTintColor = PXNConstants.BRAND_COLOR;
      item.PageIndicatorTintColor = UIColor.FromWhiteAlpha(white: 0.827f, alpha: 1.000f);
    }

    #endregion

    #region Properties
    #endregion
  }
}

