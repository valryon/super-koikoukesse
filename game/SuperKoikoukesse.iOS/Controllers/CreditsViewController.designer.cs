// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace SuperKoikoukesse.iOS
{
	[Register ("CreditsViewController")]
	partial class CreditsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView TextPublishers { get; set; }

		[Action ("OnDismissTouched:")]
		partial void OnDismissTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TextPublishers != null) {
				TextPublishers.Dispose ();
				TextPublishers = null;
			}
		}
	}
}
