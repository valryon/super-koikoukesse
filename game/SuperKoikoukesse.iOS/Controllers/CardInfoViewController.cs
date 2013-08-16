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

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      ViewLogo.Layer.ShadowColor = UIColor.Black.CGColor;
      ViewLogo.Layer.ShadowRadius = 3f;
      ViewLogo.Layer.ShadowOffset = new SizeF(0, 0);
      ViewLogo.Layer.ShadowOpacity = 0.5f;
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

