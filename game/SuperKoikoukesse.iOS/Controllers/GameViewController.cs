
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;

namespace SuperKoikoukesse.iOS
{
	public partial class GameViewController : UIViewController
	{
		private Quizz m_quizz;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public GameViewController ()
			: base (UserInterfaceIdiomIsPhone ? "GameViewController_iPhone" : "GameViewController_iPad", null)
		{
			Logger.Log (LogLevel.Info, "GameView on "+ (UserInterfaceIdiomIsPhone ? "iPhone" : "iPad"));
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void LoadView ()
		{
			base.LoadView ();

			// Load background
			// TODO Bonne taille
			UIImage bgImage = UIImage.FromFile ("gui/ingame_background.png");
			View.BackgroundColor = UIColor.FromPatternImage(bgImage);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Initialize game database id first launch
			if(DatabaseService.Instance.Exists == false) {

				// Load gamedb.xml
				String xmlDatabase = File.ReadAllText(@"database/gamedb.xml");

				DatabaseService.Instance.InitializeFromXml(xmlDatabase);
			}

			// Prepare a quizz
			m_quizz = new Quizz ();
			m_quizz.Initialize ();

			// Display the image
			setViewQuestion (m_quizz.CurrentQuestion);
		}

		/// <summary>
		/// Setup all the view elements for a given question
		/// </summary>
		/// <param name="q">Q.</param>
		private void setViewQuestion(Question q) {

			Logger.Log (LogLevel.Info, "Setting up view for current question "+q);

			// Image
			string imgPath = ImageService.Instance.Getimage (q.CorrectAnswer);
			UIImage img = UIImage.FromFile(imgPath);
			gameImage.Image = img;

			// Answers
			game1Button.SetTitle (q.GetGameTitle(0), UIControlState.Normal);
			game2Button.SetTitle (q.GetGameTitle(1), UIControlState.Normal);
			game3Button.SetTitle (q.GetGameTitle(2), UIControlState.Normal);
			game4Button.SetTitle (q.GetGameTitle(3), UIControlState.Normal);
		}
	}
}

