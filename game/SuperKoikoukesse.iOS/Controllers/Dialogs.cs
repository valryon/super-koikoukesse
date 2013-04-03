using System;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	public static class Dialogs
	{
		public static void ShowNoMoreCreditsDialogs ()
		{
			UIAlertView alert = new UIAlertView(
				"Plus de crédits !",
				"Vous n'avez plus de crédits, il vous faut en acheter d'autres ou attendre demain pour rejouer !",
				null,
				"Ok",
				"Acheter des crédits");

			alert.Show();
		}
	}
}

