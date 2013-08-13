using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Collections.Generic;
using MonoTouch.GameKit;

namespace SuperKoikoukesse.iOS
{
  [Register ("VersusMatchsCollectionViewController")]
  public class VersusMatchsCollectionViewController : UICollectionViewController
  {
    private static NSString CellId = new NSString("VersusMatchCell");
    // Make sure it matches the reusable ID in storyboard
    private bool isLoaded;
    private Dictionary<string, GKPlayer> matchsPlayer;
    private List<VersusMatch> matchs;
    private GameLauncher mGameLauncher;

    public VersusMatchsCollectionViewController(IntPtr handle) : base (handle)
    {
      matchs = new List<VersusMatch>();
      matchsPlayer = new Dictionary<string, GKPlayer>();
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      isLoaded = false;

      PlayerCache.Instance.AuthenticatedPlayer.ListMatchs(
        (matches) => {

        matchs = new List<VersusMatch>();

        List<string> playerIds = new List<string>();

        foreach (var m in matches)
        {

          if (string.IsNullOrEmpty(m.Player1Id) == false && string.IsNullOrEmpty(m.Player2Id) == false)
          {
            matchs.Add(m);

            if (matchsPlayer.ContainsKey(m.Player1Id) == false)
            {
              playerIds.Add(m.Player1Id);
            }
            if (matchsPlayer.ContainsKey(m.Player2Id) == false)
            {
              playerIds.Add(m.Player2Id);
            }
          }
        }

        // Load player profiles in background
        GameCenterHelper.GetPlayers(playerIds.ToArray(), (players) => {
          matchsPlayer = new Dictionary<string, GKPlayer>();

          foreach (var p in players)
          {
            if (matchsPlayer.ContainsKey(p.PlayerID) == false)
            {
              matchsPlayer.Add(p.PlayerID, p);
            }
          }

          InvokeOnMainThread(() => {
            isLoaded = true;
            CollectionView.ReloadData();
          });
        });
      },
        () => {
        // Error
        isLoaded = true;
      }
      );
    }

    public override int NumberOfSections(UICollectionView collectionView)
    {
      return 1;
    }

    public override int GetItemsCount(UICollectionView collectionView, int section)
    {
      if (isLoaded == false)
      {
        return 0;
      }
      return matchs.Count;
    }

    public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
      UICollectionViewCell cell = collectionView.DequeueReusableCell(CellId, indexPath) as UICollectionViewCell;

      VersusMatch match = matchs[indexPath.Item];

      // Fill cell properties using TAG.
      // 1 : Player 1 profile picture
      // 2 : Player 1 ID
      // 3 : Player 1 score
      // 11: Player 2 profile picture
      // 12: Player 2 ID
      // 13: Player 2 score

      if (match.IsEnded)
      {
        cell.BackgroundColor = PXNConstants.BRAND_GREY;
      }
      else if (match.IsPlayerTurn(PlayerCache.Instance.AuthenticatedPlayer.PlayerId))
      {
        cell.BackgroundColor = PXNConstants.BRAND_COLOR;
      }
      else
      {
        cell.BackgroundColor = UIColor.White;
      }

      // -- Player 1
      GKPlayer player1 = null;
      if (matchsPlayer.TryGetValue(match.Player1Id, out player1))
      {
        UIImageView ImagePlayer1 = cell.ViewWithTag(1) as UIImageView;
        ImagePlayer1.Image = UIImage.FromFile("icon.png");

        UILabel LabelPlayer1Id = cell.ViewWithTag(2) as UILabel;
        LabelPlayer1Id.Text = player1.DisplayName;

        UILabel LabelPlayer1Score = cell.ViewWithTag(3) as UILabel;
        LabelPlayer1Score.Text = "";

        // Look for player1 turn
        var player1Turn = match.Turns.Where(m => m.PlayerId == match.Player1Id).FirstOrDefault();
        if (player1Turn == null)
        {
          LabelPlayer1Score.Text = "Not played yet";
        }
        else
        {
          LabelPlayer1Score.Text = player1Turn.Score.ToString();
        }

        //Picture
        GameCenterHelper.GetProfileImage(player1, (photo) => {
          InvokeOnMainThread(() => {
            ImagePlayer1.Image = photo;
          });
        });
      }
      else
      {
        Logger.E("Unknow player1: " + match.Player1Id);
      }

      // -- Player 2
      GKPlayer player2 = null;
      if (matchsPlayer.TryGetValue(match.Player2Id, out player2))
      {

        UIImageView ImagePlayer2 = cell.ViewWithTag(11) as UIImageView;
        ImagePlayer2.Image = UIImage.FromFile("icon.png");

        UILabel LabelPlayer2Id = cell.ViewWithTag(12) as UILabel;
        LabelPlayer2Id.Text = player2.DisplayName;

        UILabel LabelPlayer2Score = cell.ViewWithTag(13) as UILabel;
        LabelPlayer2Score.Text = "";

        // Look for a turn too
        var player2Turn = match.Turns.Where(m => m.PlayerId == match.Player2Id).FirstOrDefault();
        if (player2Turn == null)
        {
          LabelPlayer2Score.Text = "Not played yet";
        }
        else
        {
          LabelPlayer2Score.Text = player2Turn.Score.ToString();
        }

        // Picture
        GameCenterHelper.GetProfileImage(player2, (photo) => {
          InvokeOnMainThread(() => {
            ImagePlayer2.Image = photo;
          });
        });
      }
      else
      {
        Logger.E("Unknow player2: " + match.Player2Id);
      }

      return cell;
    }

    public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
    {
      VersusMatch match = matchs[indexPath.Item];

      if (match.IsEnded == false && match.IsPlayerTurn(PlayerCache.Instance.AuthenticatedPlayer.PlayerId))
      {
        PlayerCache.Instance.AuthenticatedPlayer.SetMatch(match);

        mGameLauncher = new GameLauncher(this);
        mGameLauncher.Launch("VersusToGame", GameMode.VERSUS, match.Difficulty, match.Filter);
      }
    }

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      base.PrepareForSegue(segue, sender);

      if (mGameLauncher != null && segue.Identifier == mGameLauncher.SegueId)
      {
        GameViewController gameVc = segue.DestinationViewController as GameViewController;

        mGameLauncher.Prepare(gameVc);
      }
    }
  }
}

