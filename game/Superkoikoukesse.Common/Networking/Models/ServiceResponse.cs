using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice response format
	/// </summary>
	public class ServiceResponse
	{
		public int Code { get; set; }

		public string Message { get; set; }

		public string JsonData { get; set; }

		public ServiceResponse ()
		{
		}
	}
}
