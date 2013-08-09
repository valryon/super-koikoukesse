using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	[Register ("VersusMatchsCollectionViewController")]
	public class VersusMatchsCollectionViewController : UICollectionViewController
	{
		public VersusMatchsCollectionViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Register any custom UICollectionViewCell classes
//			CollectionView.RegisterClassForCell (typeof(VersusMatchsCollectionViewControllerCell), VersusMatchsCollectionViewControllerCell.Key);
			
			// Note: If you use one of the Collection View Cell templates to create a new cell type,
			// you can register it using the RegisterNibForCell() method like this:
			//
			// CollectionView.RegisterNibForCell (MyCollectionViewCell.Nib, MyCollectionViewCell.Key);
		}

		public override int NumberOfSections (UICollectionView collectionView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			// TODO: return the actual number of items in the section
			return 1;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
//			var cell = collectionView.DequeueReusableCell (VersusMatchsCollectionViewControllerCell.Key, indexPath) as VersusMatchsCollectionViewControllerCell;
			
			// TODO: populate the cell with the appropriate data based on the indexPath
			
//			return cell;
			return null;
		}
	}
}

