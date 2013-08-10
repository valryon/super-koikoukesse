using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Simple log wrapper
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Debug
		/// </summary>
		/// <param name="message">Message.</param>
		public static void D (string message)
		{
			Console.WriteLine ("DEBUG: " + message);
		}

		/// <summary>
		/// Info
		/// </summary>
		/// <param name="message">Message.</param>
		public static void I (string message)
		{
			Console.WriteLine ("INFO : " + message);
		}

		/// <summary>
		/// Warning
		/// </summary>
		/// <param name="message">Message.</param>
		public static void W (string message)
		{
			Console.WriteLine ("WARN : " + message);
		}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="message">Message.</param>
		public static void E (string message)
		{
			Console.WriteLine ("ERROR: " + message);
		}

		/// <summary>
		/// Error with exception
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
		public static void E (string message, Exception e)
		{
			Console.WriteLine ("ERROR: " + message + "\n" + e.ToString ());
		}

#if IOS
		/// <summary>
		/// Error with exception
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
		public static void E (string message, MonoTouch.Foundation.NSError e)
		{
			Console.WriteLine ("ERROR: " + message + "\n" + e.ToString ());
		}
#endif
	}


}

