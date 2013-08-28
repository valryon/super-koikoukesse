// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;

namespace SuperKoikoukesse.iOS
{
  public partial class CardModeViewController : AbstractCardViewController
  {
    #region Members

    private GameMode _mode;

    #endregion

    #region Constructors

    public CardModeViewController(GameMode mode) : base("CardModeView")
    {
      _mode = mode;
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Get the mode i18n key (the enum name corresponds to the i18n key)
      var mode = _mode.ToString().ToLower();
      var color = PXNConstants.GetModeColor(_mode);

      // Icon
      ImageIcon.Image = new UIImage("icon_mode_" + mode + ".png");

      // Colors
      LabelTitle.TextColor = color;
      ViewCard.BackgroundColor = color;

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

    public override UIColor GetColor()
    {
      return PXNConstants.GetModeColor(_mode);
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

