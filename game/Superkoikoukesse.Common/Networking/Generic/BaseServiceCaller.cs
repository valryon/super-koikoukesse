using System;
using System.IO;
using System.Json;
using System.Net;
using Superkoikoukesse.Common.Utils;

namespace Superkoikoukesse.Common.Networking
{
	/// <summary>
	/// Utilities to reach Game Webservice
	/// </summary>
	public abstract class BaseServiceCaller
	{
		public BaseServiceCaller ()
		{
		}

		/// <summary>
		/// Get the service url to call
		/// </summary>
		/// <returns>The service URL.</returns>
		public abstract Uri GetServiceUrl ();

		/// <summary>
		/// Make an asynchronous request to the POST webservice
		/// </summary>
		/// <param name="decrypt">If set to <c>true</c> decrypt.</param>
		/// <param name="callbackSuccess">Callback success.</param>
		/// <param name="callbackFailure">Callback failure.</param>
		protected void RequestPostJsonAsync (string requestBodyJson, Action<ServiceResponse> callbackSuccess, Action<int, Exception> callbackFailure)
		{
			Uri url = GetServiceUrl ();

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			//request.ContentType = "application/json";

			string body = requestBodyJson;

			Logger.D( "Request body: " + body);

			if (Constants.ENABLE_ENCRYPTION) {
				body = EncryptionHelper.Encrypt (body);
				
				Logger.D( "Encrypted request body: " + body);
			}

			// TODO Améliorer ce hack...
			body = "r=" + System.Web.HttpUtility.UrlEncode (body);

			try {
				using (var streamWrite = new StreamWriter(request.GetRequestStream())) {
					streamWrite.Write (body);
					streamWrite.Close ();
				}

				asyncRequest (request, url, callbackSuccess, callbackFailure);
				
			} catch (WebException e) {
				Logger.W( "<- KO Network issues? " + e.Message + " "+url);
				if (callbackFailure != null) {
					callbackFailure (-1, e);
				}
			}
		}

		/// <summary>
		/// Make an asynchronous request to the webservice
		/// </summary>
		/// <param name="callbackSuccess">Callback success.</param>
		/// <param name="callbackFailure">Callback failure.</param>
		protected void RequestJsonAsync (Action<ServiceResponse> callbackSuccess, Action<int, Exception> callbackFailure)
		{
			Uri url = GetServiceUrl ();

			WebRequest request = WebRequest.Create (url);

			asyncRequest (request, url, callbackSuccess, callbackFailure);
		}

		/// <summary>
		/// Async request core
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="url">URL.</param>
		/// <param name="decrypt">If set to <c>true</c> decrypt.</param>
		/// <param name="callbackSuccess">Callback success.</param>
		/// <param name="callbackFailure">Callback failure.</param>
		private void asyncRequest (WebRequest request, Uri url, Action<ServiceResponse> callbackSuccess, Action<int, Exception> callbackFailure)
		{
			Logger.I("-> " + request.Method + " " + url);

			request.BeginGetResponse (result => {
				var webRequest = result.AsyncState as HttpWebRequest;

				try {
					WebResponse webResponse = webRequest.EndGetResponse (result);
					Stream streamResponse = webResponse.GetResponseStream ();
					string json = string.Empty;

					using (StreamReader reader = new StreamReader (streamResponse)) {
						json = reader.ReadToEnd ();
					}

					// Decrypt if necessary
					if (Constants.ENABLE_ENCRYPTION) {
						json = EncryptionHelper.Decrypt (json);
					}

					// Parse response
					ServiceResponse response = new ServiceResponse ();

					// Open Json
					JsonValue value = JsonObject.Parse (json);
					int code = Convert.ToInt32 (value ["c"].ToString ());
					string message = null;
					string data = null;
					if (value.ContainsKey ("e")) {
						message = value ["e"];
					}
					if (value.ContainsKey ("r")) {
						data = value ["r"].ToString ();
					}
					response.Code = code;
					response.Message = message;
					response.JsonData = data;

					if (code == 0) {
						Logger.I("<- OK ");
						if (callbackSuccess != null) {
							callbackSuccess (response);
						}
					} else {
						Logger.E( "<- KO " + code + ": " + message);
						if (callbackFailure != null) {
							callbackFailure (code, new ArgumentException (code + ": " + message));
						}
					}
				} 
				catch (WebException e) {
					Logger.W( "<- KO Network issues? " + e.Message + " "+url);
					if (callbackFailure != null) {
						callbackFailure (-1, e);
					}
				}
				catch (Exception e) {
					// Log and callback
					Logger.E( "WebserviceCaller.RequestJsonAsync ", e);
					if (callbackFailure != null) {
						callbackFailure (-1, e);
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

