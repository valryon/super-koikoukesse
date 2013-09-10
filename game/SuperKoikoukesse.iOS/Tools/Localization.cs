// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using MonoTouch.Foundation;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public static class Localization
	{
		public static string Get(string key)
		{
			return NSBundle.MainBundle.LocalizedString(key, "");
		}

    public static string GetDifficulty(GameDifficulty value)
    {
      return Get("challenge." + value.ToString().ToLower() + ".title");
    }

    public static string GetMode(GameMode value)
    {
      return Get("mode." + value.ToString().ToLower() + ".title");
    }
	}
}

