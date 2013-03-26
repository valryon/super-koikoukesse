using System;

namespace Superkoikoukesse.Common
{
	public static class Constants
	{
		public static string WebserviceUrl = "http://skkk.dmayance.com/"; // End with slash!

		public static string EncryptionKey = "p_o6u-e/t*+!";

		public static bool UseEncryption = true;

		public static string DatabaseLocation = "superkoikoukesse.sqlite";

		public static string ImagesRootLocation = "database/images/";

		public static string ConfigFileLocation = "gameconfig.xml";

#if DEBUG
		public static bool DebugMode = true;
#else 
		public static bool DebugMode = false;
#endif

		public static int ProfileCacheDuration = 0;

		#region Animations

		public static float SplashScreenFadeDuration = 0.5f;

		public static float DezoomFactor = 10f;

		public static float DezoomDuration = 5f; //seconds

		#endregion
	}
}

