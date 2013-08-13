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

		public static void ShowAuthenticationRequired (Action authenticationRequested)
		{
			UIAlertView alert = new UIAlertView(
				"Game Center requis",
				"Vous devez vous connectez à Game Center pour jouer en multijoueur.",
				null,
				"Cancel", "Connect");
			
			alert.Show();
      alert.Dismissed += (object sender, UIButtonEventArgs e) => {
        if(e.ButtonIndex == 1) {
          if(authenticationRequested != null) {
            authenticationRequested();
          }
        }
      };
		}

		public static void ShowNotYourTurn ()
		{
			UIAlertView alert = new UIAlertView(
				"En attente",
				"Vous devez attendre que l'autre joueur ait joué son tour. TODO Voir le score en cours ?",
				null,
				"Ok");
			
			alert.Show();
		}

		public static void ShowMatchEnded() {
			UIAlertView alert = new UIAlertView(
				"Match terminé",
				"Ce match est terminé. TODO Voir le résultat ?",
				null,
				"Ok");
			
			alert.Show();
		}
	}
}

