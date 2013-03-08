using System;

namespace Superkoikoukesse.Common
{
	public static class Constants
	{
		public static string WebserviceUrl = "http://skkk.dmayance.com/"; // End with slash!

		public static string EncryptionKey = "p_o6u-e/t*+!";

		public static string DatabaseLocation = "superkoikoukesse.sqlite";

		public static string ImagesRootLocation = "database/images/";

#if DEBUG
		public static bool DebugMode = true;
#else 
		public static bool DebugMode = false;
#endif
	}
}

