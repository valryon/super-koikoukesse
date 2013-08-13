using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class VersusMatchsViewController : UIViewController
	{
		public VersusMatchsViewController (IntPtr handle) : base (handle)
		{
		}

		partial void OnNewMatchTouched (MonoTouch.Foundation.NSObject sender) 
		{
			PerformSegue("VersusToNewVersus", this);
		}
	}
}

