using System;
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	public static class Localization
	{
		public static string Get(string key)
		{
			return NSBundle.MainBundle.LocalizedString(key, "");
		}
	}
}

