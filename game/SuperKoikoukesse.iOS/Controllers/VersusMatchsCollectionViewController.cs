using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
	[Register ("VersusMatchsCollectionViewController")]
	public class VersusMatchsCollectionViewController : UICollectionViewController
	{
		private static NSString CellId = new NSString ("VersusMatchCell");
		// Make sure it matches the reusable ID in storyboard
		private bool isLoaded;
		private List<VersusMatch> matchs;

		public VersusMatchsCollectionViewController (IntPtr handle) : base (handle)
		{
			matchs = new List<VersusMatch> ();
			isLoaded = false;

			PlayerCache.Instance.AuthenticatedPlayer.ListMatchs (
				(matches) => {
					isLoaded = true;

					matchs = matches;

					InvokeOnMainThread( () => {
						CollectionView.ReloadData();
					});
				},
				() => {
					// Error
					isLoaded = true;
				}
			);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

//			CollectionView.RegisterClassForCell (typeof(VersusMatchsCollectionViewCell), VersusMatchsCollectionViewCell.Key);
		}

		public override int NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return matchs.Count;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			UICollectionViewCell cell = collectionView.DequeueReusableCell (CellId, indexPath) as UICollectionViewCell;

			VersusMatch match = matchs [indexPath.Item];

			// Fill cell properties using TAG.
			// 1 : Player 1 profile picture
			// 2 : Player 1 ID
			// 3 : Player 1 score
			// 11: Player 2 profile picture
			// 12: Player 2 ID
			// 13: Player 2 score

			// -- Player 1
			UIImageView ImagePlayer1 = cell.ViewWithTag (1) as UIImageView;
			ImagePlayer1.Image = UIImage.FromFile ("icon.png");
			UILabel LabelPlayer1Id = cell.ViewWithTag (2) as UILabel;
			LabelPlayer1Id.Text = "Loading";
			UILabel LabelPlayer1Score = cell.ViewWithTag (3) as UILabel;
			LabelPlayer1Score.Text = "";

			// Look for player1 turn
			var player1Turn = match.Turns.Where (m => m.PlayerId == match.Player1Id).FirstOrDefault ();
			if (player1Turn == null) {
				LabelPlayer1Score.Text = "Not played yet";
			}
			else {
				LabelPlayer1Score.Text = player1Turn.Score.ToString();
			}

			// Retrieve player info and picture
			GameCenterHelper.GetPlayer(match.Player1Id, (player) => {

				InvokeOnMainThread(() => {
					LabelPlayer1Id.Text = player.DisplayName;
				});

				GameCenterHelper.GetProfileImage(player, (image) => {
					InvokeOnMainThread(() => {
						ImagePlayer1.Image = image;
					});
				});
			});

			// -- Player 2

			UIImageView ImagePlayer2 = cell.ViewWithTag (11) as UIImageView;
			ImagePlayer2.Image = UIImage.FromFile ("icon.png");
			UILabel LabelPlayer2Id = cell.ViewWithTag (12) as UILabel;
			LabelPlayer2Id.Text = "Loading";
			UILabel LabelPlayer2Score = cell.ViewWithTag (13) as UILabel;
			LabelPlayer2Score.Text = "";

			// Look for a turn too
			var player2Turn = match.Turns.Where (m => m.PlayerId == match.Player2Id).FirstOrDefault ();
			if (player2Turn == null) {
				LabelPlayer2Score.Text = "Not played yet";
			}
			else {
				LabelPlayer2Score.Text = player2Turn.Score.ToString();
			}

			// Picture
			GameCenterHelper.GetPlayer(match.Player2Id, (player) => {

				InvokeOnMainThread(() => {
					LabelPlayer2Id.Text = player.DisplayName;
				});

				GameCenterHelper.GetProfileImage(player, (image) => {
					InvokeOnMainThread(() => {
						ImagePlayer2.Image = image;
					});
				});
			});

			return cell;
		}
	}
}

