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

		public static void ShowDebugFilterTooRestrictive ()
		{
			UIAlertView alert = new UIAlertView(
				"Pas assez de jeux",
				"Le filtre saisi a remonté moins de 30 images. Le jeu va peut-être planter, il faut revoir ce filtre ou peupler la base de données..",
				null,
				"Ok");
			
			alert.Show();
		}

		public static void ShowAuthenticationRequired ()
		{
			UIAlertView alert = new UIAlertView(
				"Game Center requis",
				"Vous devez vous connectez à Game Center pour jouer en multijoueur.",
				null,
				"Ok");
			
			alert.Show();
		}
	}
}

