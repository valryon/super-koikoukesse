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
      : base ("CardInfo"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
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

    partial void creditsButtonPressed(MonoTouch.Foundation.NSObject sender)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.SwitchToCreditsView();
    }

    partial void shopButtonPressed(MonoTouch.Foundation.NSObject sender)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.SwitchToShopView();
    }

    #endregion

    #region Properties
    #endregion
  }
}

