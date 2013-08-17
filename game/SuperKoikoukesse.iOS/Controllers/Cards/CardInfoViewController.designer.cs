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
	[Register ("CardInfoViewController")]
	partial class CardInfoViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView ViewLogo { get; set; }

		[Action ("OnCreditsTouched:")]
		partial void OnCreditsTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ViewLogo != null) {
				ViewLogo.Dispose ();
				ViewLogo = null;
			}
		}
	}
}
