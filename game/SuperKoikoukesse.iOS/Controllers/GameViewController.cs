
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
		}
	}
}

