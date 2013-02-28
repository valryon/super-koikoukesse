using System;
using System.Net;
using System.IO;
using Superkoikoukesse.Common.Utils;

namespace Superkoikoukesse.Common.Networking
{
	/// <summary>
	/// Utilities to reach Game Webservice
	/// </summary>
	public class WebserviceCaller
	{
		public WebserviceCaller ()
		{
		}

		/// <summary>
		/// Make an asynchronous request to the webservice
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="callbackSuccess">Callback success.</param>
		/// <param name="callbackFailure">Callback failure.</param>
		public void RequestJsonAsync (Uri url, bool decrypt, Action<string> callbackSuccess, Action<Exception> callbackFailure)
		{
			WebRequest request = WebRequest.Create (url);

			Logger.Log (LogLevel.Info, "RequestJsonAsync " + url);

			request.BeginGetResponse ((result) => {

				var webRequest = result.AsyncState as HttpWebRequest;
				
				try {
					WebResponse response = webRequest.EndGetResponse (result);

					Stream responseStream = response.GetResponseStream ();

					string json = string.Empty;
					using(StreamReader reader = new StreamReader(responseStream)) {
						json = reader.ReadToEnd();
					}
					// Decrypt if necessary

					//Returns Json


				} catch (Exception e) {

					// Log and callback
					Logger.Log (LogLevel.Error, "WebserviceCaller.RequestJsonAsync ", e);
						
					if (callbackFailure != null) {
						callbackFailure (e);
					}
				}

			}, request);
		}

		/// <summary>
		/// Test connection with webservice
		/// </summary>
		public bool IsHostReachable (string hostUrl)
		{
			return NetworkAvailability.IsHostReachable (hostUrl);
		}
	}
}

