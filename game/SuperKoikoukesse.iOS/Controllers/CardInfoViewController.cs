using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
  public partial class CardInfoViewController : AbstractCardViewController
  {
    #region Members
    #endregion

    #region Constructors

    public CardInfoViewController() : base("CardInfoView")
    {
    }

    #endregion

    #region Methods
    #endregion

    #region Handlers

    partial void OnCreditsTouched(NSObject sender)
    {
      if (CreditsDisplayed != null) CreditsDisplayed();
    }

    #endregion

    #region Properties

    public event Action CreditsDisplayed;

    #endregion
  }
}

