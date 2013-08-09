using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.GameKit;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
  public partial class ScoreViewController : UIViewController
  {
    #region Members

    private Quizz _quizz;
    private static Dictionary<string, UIImage> _playerImageCache;

    #endregion

    #region Constructors

    public ScoreViewController(Quizz q) : base ("ScoreView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
		_quizz = q;
		_playerImageCache = new Dictionary<string, UIImage>();
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      //SetViewWithQuizz();
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    private void LoadPlayers(GKPlayer[] players, NSError error)
    {

      foreach (GKPlayer player in players)
      {
        player.LoadPhoto(GKPhotoSize.Normal, (image, loadImageError) => {

          _playerImageCache.Add(player.PlayerID, image);

          BeginInvokeOnMainThread(() => {
            // TODO Refresh images
          });
        });
      }
    }

    private void SetViewWithQuizz()
    {
      if (IsViewLoaded)
      {
//        this.coinsLabel.Text = quizz.EarnedCoins.ToString ();

//        // Highscore control
//        highScoreControl.SetScoreParameters (quizz.Mode, quizz.Difficulty, quizz.RankForLastScore, quizz.Score);
      }
    }

    #endregion

    #region Handlers

    partial void OnRetryTouched(NSObject sender)
    {
      if (PlayerCache.Instance.CachedPlayer.Credits > 0)
      {
        var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
//        appDelegate.SwitchToGameView(_quizz.Mode, _quizz.Difficulty, _quizz.Filter);
				// TODO
      }
      else
      {
        Dialogs.ShowNoMoreCreditsDialogs();
      }
    }

    partial void OnMenuTouched(NSObject sender)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.SwitchToMenuView();
    }

    #endregion

    #region Properties
    #endregion
  }
}

