using System;

namespace Superkoikoukesse.Common
{
	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error
	}

	/// <summary>
	/// Simple log wrapper
	/// </summary>
	public static class Logger
	{
		public static void Log (LogLevel level, string message)
		{
			Console.WriteLine (level.ToString ().ToUpper () + ": " + message);
		}

		public static void LogException (LogLevel level, string source, Exception e)
		{
			Console.WriteLine (level.ToString ().ToUpper () + ": Exception from " + source+" - "+e.ToString());
		}
	}
}

