using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;

namespace SuperKoikoukesse.iOS
{
  public partial class CardModeViewController : UIViewController
  {
    #region Members

    private GameMode _mode;

    #endregion

    #region Constructors

    public CardModeViewController(GameMode mode) 
      : base ("CardModeView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
      _mode = mode;
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Get the mode i18n key (the enum name corresponds to the i18n key)
      var mode = _mode.ToString().ToLower();

      // Add a border below the header
      var borderBottom = new CALayer();
      borderBottom.Frame = new RectangleF(0, ViewHeader.Frame.Height, ViewHeader.Frame.Width, 0.5f);
      borderBottom.BackgroundColor = UIColor.FromHSB(0, 0, 0.7f).CGColor;
      ViewHeader.Layer.AddSublayer(borderBottom);

      // Set the text
      LabelTitle.Text = Localization.Get("mode." + mode + ".title");
      LabelDescriptionMain.Text = Localization.Get("mode." + mode + ".desc.main");
      LabelDescriptionSub.Text = Localization.Get("mode." + mode + ".desc.sub");

      // Resize
      LabelTitle.SizeToFit();
      LabelDescriptionMain.SizeToFit();
      LabelDescriptionSub.SizeToFit();

      // Add the shadow to the background view
      ViewShadow.Layer.CornerRadius = 3f;
      ViewShadow.Layer.ShadowColor = UIColor.Black.CGColor;
      ViewShadow.Layer.ShadowRadius = 1.5f;
      ViewShadow.Layer.ShadowOffset = new SizeF(0, 0);
      ViewShadow.Layer.ShadowOpacity = 0.2f;

      // Set the radius & mask on the card
      ViewCard.Layer.CornerRadius = 3f;
      ViewCard.Layer.MasksToBounds = true;
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      // TODO if the game has just been installed, the default label will be displayed
      // find a workaround in this case
      var profile = PlayerCache.Instance.CachedPlayer;
      if (profile != null)
      {

        // TODO HACK Crash on iPhone, LabelLifes is null
//        if (LabelLifes != null)
//        {
//          LabelLifes.Text = "(sur " + profile.Credits.ToString() + ")";
//        }
      }
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    #endregion

    #region Handlers

    partial void OnPlayTouched(NSObject sender)
    {
      if (GameModeSelected != null)
      {
        GameModeSelected(_mode);
      }
    }

    #endregion

    #region Properties

    public event Action<GameMode> GameModeSelected;

    #endregion
  }
}

