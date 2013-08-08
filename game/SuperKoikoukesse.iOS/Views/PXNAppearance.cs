using System;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

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
      UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
      UINavigationBar.Appearance.BackgroundColor = PXNConstants.COLOR_NAVIGATION;
    }

    #endregion

    #region Properties
    #endregion
  }
}

