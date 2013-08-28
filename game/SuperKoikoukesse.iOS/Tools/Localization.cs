// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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

