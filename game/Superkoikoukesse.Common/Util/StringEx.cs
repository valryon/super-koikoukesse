using System;

namespace Superkoikoukesse.Common
{
	public static class StringEx
	{
		public static string ToBase64(this string s) {
			byte[] stringData = System.Text.Encoding.UTF8.GetBytes(s);
			return Convert.ToBase64String(stringData);
		}
	}
}

