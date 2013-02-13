using System;
using System.Collections.Generic;
using System.Linq;

using MonoMac.AppKit;
using MonoMac.Foundation;
using SuperKoikoukesse.Core.Main;

namespace SuperKoikoukesse.Mac
{
#if OSX
	class Program
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();
			
			using (var p = new NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new AppDelegate ();
				
				// Set our Application Icon
				NSImage appIcon = NSImage.ImageNamed ("SuperKoikoukesseicon.png");
				if(appIcon != null)
				{
					NSApplication.SharedApplication.ApplicationIconImage = appIcon;
				}
				NSApplication.Main (args);
			}
		}
	}
	
	class AppDelegate : NSApplicationDelegate
	{
		private SuperKoikoukesseGame game;
		
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			game = new SuperKoikoukesseGame();
			game.Run();
		}
		
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
#endif
}
