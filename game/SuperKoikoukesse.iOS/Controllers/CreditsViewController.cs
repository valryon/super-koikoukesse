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

    public CreditsViewController(IntPtr handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      
      // Load all publishers name and display them
      var credits = new StringBuilder();
      var publishers = GameDatabase.Instance.GetPublishers();

      foreach (var p in publishers)
      {
        credits.Append(p + " ");
      }

      LabelCredits.Text = credits.ToString();
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    #endregion

    #region Handlers

    partial void OnDismissTouched(NSObject sender)
    {
      DismissViewController(true, null);
    }

    #endregion

    #region Properties
    #endregion
  }
}

