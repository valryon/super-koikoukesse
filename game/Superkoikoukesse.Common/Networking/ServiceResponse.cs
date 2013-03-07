
using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Superkoikoukesse.Common
{
	public partial class ServiceResponse : DialogViewController
	{
		public ServiceResponse () : base (UITableViewStyle.Grouped, null)
		{
			Root = new RootElement ("ServiceResponse") {
				new Section ("First Section"){
					new StringElement ("Hello", () => {
						new UIAlertView ("Hola", "Thanks for tapping!", null, "Continue").Show (); 
					}),
					new EntryElement ("Name", "Enter your name", String.Empty)
				},
				new Section ("Second Section"){
				},
			};
		}
	}
}
