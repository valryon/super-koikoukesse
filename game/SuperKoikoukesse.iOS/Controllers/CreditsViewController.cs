using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Text;

namespace SuperKoikoukesse.iOS
{
  public partial class CreditsViewController : UIViewController
  {
    #region Members
    #endregion

    #region Constructors

    public CreditsViewController()
      : base ("CreditsView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      
      // Load all publishers name and display them
      StringBuilder credits = new StringBuilder();

      var publishers = GameDatabase.Instance.GetPublishers();

      foreach (var publisher in publishers)
      {
        credits.Append(publisher + " ");
      }

      creditsLabel.Text = credits.ToString();
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    #endregion

    #region Handlers

    partial void backButtonPressed(MonoTouch.Foundation.NSObject sender)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.SwitchToMenuView();
    }

    #endregion

    #region Properties
    #endregion
  }
}

