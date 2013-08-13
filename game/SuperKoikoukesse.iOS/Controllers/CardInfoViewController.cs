using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
  public partial class CardInfoViewController : UIViewController
  {
    #region Members
    #endregion

    #region Constructors

    public CardInfoViewController() 
      : base ("CardInfoView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    #endregion

    #region Handlers

    partial void OnShopTouched(MonoTouch.Foundation.NSObject sender)
    {
     
    }

    partial void OnCreditsTouched(MonoTouch.Foundation.NSObject sender)
    {
      
    }

    #endregion

    #region Properties
    #endregion
  }
}
