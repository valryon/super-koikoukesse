using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	[Register ("VersusMatchsCollectionViewController")]
	public class VersusMatchsCollectionViewController : UICollectionViewController
	{
		private static NSString CellId = new NSString("VersusMatchCell"); // Make sure it matches the reusable ID in storyboard

		public VersusMatchsCollectionViewController (IntPtr handle) : base (handle)
		{
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
			return 2;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			UICollectionViewCell cell = collectionView.DequeueReusableCell (CellId, indexPath) as UICollectionViewCell;

			// Fill cell properties using TAG.
			// 1 : Player 1 profile picture
			// 2 : Player 1 ID
			// 3 : Player 1 score
			// 11: Player 2 profile picture
			// 12: Player 2 ID
			// 13: Player 2 score
			UIImageView ImagePlayer1 = cell.ViewWithTag (1) as UIImageView;
			ImagePlayer1.Image = UIImage.FromFile ("icon.png");
			UILabel LabelPlayer1Id = cell.ViewWithTag (2) as UILabel;
			UILabel LabelPlayer1Score = cell.ViewWithTag (3) as UILabel;

			UIImageView ImagePlayer2 = cell.ViewWithTag (11) as UIImageView;
			ImagePlayer2.Image = UIImage.FromFile ("icon.png");
			UILabel LabelPlayer2Id = cell.ViewWithTag (12) as UILabel;
			UILabel LabelPlayer2Score = cell.ViewWithTag (13) as UILabel;

			return cell;
		}
	}
}

