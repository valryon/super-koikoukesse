using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Constants for the game (ingame, views, configuration)
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// URL of the koikoukesse webservice
		/// </summary>
		//public static string WEBSERVICE_URL = "http://localhost:9393/ws/"
		public static string WEBSERVICE_URL = "http://super-koikoukesse.herokuapp.com/ws/"; // End with slash!

		/// <summary>
		/// 16 char key for encryption
		/// </summary>
		public static string ENCRYPTION_KEY = "p_o6u-e/t*+!";

		/// <summary>
		/// Use encryption to communicate with webservice
		/// </summary>
		public static bool ENABLE_ENCRYPTION = true;

		/// <summary>
		/// Game database location
		/// </summary>
		public static string DATABASE_LOCATION = "superkoikoukesse.sqlite";

		/// <summary>
		/// Images database location
		/// </summary>
		public static string IMAGE_ROOT_LOCATION = "database/images/";

		/// <summary>
		/// Configuration file location
		/// </summary>
		public static string CONFIG_FILE_LOCATION = "gameconfig.xml";

#if DEBUG

		/// <summary>
		/// Enable debug mode
		/// </summary>
		public static bool DEBUG_MODE = true;
#else 
		/// <summary>
		/// Enable debug mode (disable in release mode)
		/// </summary>
		public static bool DEBUG_MODE = false;
#endif

		/// <summary>
		/// Time (minutes) to keep profile in cache without reloading it
		/// </summary>
		public static int PROFILE_CACHE_DURATION = 5;

		/// <summary>
		/// Default credits amount
		/// </summary>
		public static int BASE_CREDITS = 3;

		/// <summary>
		/// Defalut coins amount
		/// </summary>
		public static int BASE_COINS = 2500;

		#region Animations

#if DEBUG
		/// <summary>
		/// Animation time for splashscreen fade in 
		/// </summary>
		public static float SPLASHSCREEN_OPEN_FADE_DURATION = 0.1f;

		/// <summary>
		/// Animation time for splashscreen fade out
		/// </summary>
		public static float SPLASHSCREEN_CLOSE_FADE_DURATION = 0.1f;
#else
		/// <summary>
		/// Animation time for splashscreen fade in 
		/// </summary>
		public static float SPLASHSCREEN_OPEN_FADE_DURATION = 1.5f;

		/// <summary>
		/// Animation time for splashscreen fade out
		/// </summary>
		public static float SPLASHSCREEN_CLOSE_FADE_DURATION = 3f;
#endif

		/// <summary>
		/// Factor value for dezoom animation
		/// </summary>
		public static float ANIMATION_DEZOOM_FACTOR = 10f;

		/// <summary>
		/// Duration for dezoom animation
		/// </summary>
		public static float ANIMATION_DEZOOM_DURATION = 5f; //seconds

		/// <summary>
		/// Duration for pixelisation animation
		/// </summary>
		public static float ANIMATION_PIXELISATION_DURATION = 5f; //seconds

		/// <summary>
		/// Duration for progressive drawing animation
		/// </summary>
		public static float ANIMATION_PROGRESSIVE_DRAWING_DURATION = 5f; //seconds

		#endregion

		/// <summary>
		/// Maximum combo count
		/// </summary>
		public static int COMBO_MAXIMUM_COUNT = 4;

		/// <summary>
		/// Number of good questions to answer to enable joker
		/// </summary>
		public static int JOKER_PART_COUNT = 3;
	}


}

