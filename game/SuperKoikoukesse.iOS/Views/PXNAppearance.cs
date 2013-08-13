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
    }

    #endregion

    #region Methods

    public void SetNavigationBarAppearance()
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

    #endregion

    #region Properties
    #endregion
  }
}

