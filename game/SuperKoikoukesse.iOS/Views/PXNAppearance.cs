using System;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Drawing;

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
      SetBarButtonItemAppearance();
      SetBarBackButtonItemAppearance();
    }

    #endregion

    #region Methods

    private void SetNavigationBarAppearance()
    {
      var attributes = new UITextAttributes();
      attributes.TextColor = UIColor.FromWhiteAlpha(white: 0.306f, alpha: 1f);
      attributes.TextShadowOffset = new UIOffset(0, 0);
      attributes.TextShadowColor = UIColor.Clear;
      attributes.Font = UIFont.SystemFontOfSize(16);

      UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
      UINavigationBar.Appearance.BackgroundColor = PXNConstants.COLOR_NAVIGATION;
      UINavigationBar.Appearance.SetTitleTextAttributes(attributes);
      UINavigationBar.Appearance.SetTitleVerticalPositionAdjustment(2f, UIBarMetrics.Default);
    }

    private void SetBarButtonItemAppearance()
    {
      var normalAttributes = new UITextAttributes();
      normalAttributes.TextColor = PXNConstants.BRAND_COLOR;
      normalAttributes.TextShadowOffset = new UIOffset(0, 0);
      normalAttributes.TextShadowColor = UIColor.Clear;
      normalAttributes.Font = UIFont.SystemFontOfSize(16);

      var highlightAttributes = new UITextAttributes();
      highlightAttributes.TextColor = UIColor.White;
      highlightAttributes.TextShadowOffset = new UIOffset(0, 0);
      highlightAttributes.TextShadowColor = UIColor.Clear;
      highlightAttributes.Font = UIFont.SystemFontOfSize(16);

      UIBarButtonItem.Appearance.SetTitleTextAttributes(normalAttributes, UIControlState.Normal);
      UIBarButtonItem.Appearance.SetTitleTextAttributes(highlightAttributes, UIControlState.Highlighted);
      UIBarButtonItem.Appearance.SetBackgroundImage(new UIImage(), UIControlState.Normal, UIBarMetrics.Default);
    }

    private void SetBarBackButtonItemAppearance()
    {
//      UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(new UIImage(), UIControlState.Normal, UIBarMetrics.Default);
    }

    #endregion

    #region Properties
    #endregion
  }
}

