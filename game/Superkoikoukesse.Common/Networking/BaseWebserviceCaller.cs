using System;
using System.IO;
using System.Json;
using System.Net;
using Superkoikoukesse.Common.Utils;

namespace Superkoikoukesse.Common.Networking
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

	/// <summary>
	/// Utilities to reach Game Webservice
	/// </summary>
	public abstract class BaseWebserviceCaller
	{
		public BaseWebserviceCaller ()
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
		protected void RequestPostJsonAsync (string requestBodyJson, Action<ServiceResponse> callbackSuccess, Action<Exception> callbackFailure)
		{
			Uri url = GetServiceUrl ();
			
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			//request.ContentType = "application/json";

			string body = requestBodyJson;

			Logger.Log (LogLevel.Debug, "Request body: " + body);

			if (Constants.UseEncryption) {
				body = EncryptionHelper.Encrypt(body);
				
				Logger.Log (LogLevel.Debug, "Encrypted request body: " + body);
			}

			body = "r=" + System.Web.HttpUtility.UrlEncode(body);

			using (var streamWrite = new StreamWriter(request.GetRequestStream())) {
				streamWrite.Write (body);
				streamWrite.Close ();
			}

			asyncRequest (request, url, callbackSuccess, callbackFailure);
		}

		/// <summary>
		/// Make an asynchronous request to the webservice
		/// </summary>
		/// <param name="callbackSuccess">Callback success.</param>
		/// <param name="callbackFailure">Callback failure.</param>
		protected void RequestJsonAsync (Action<ServiceResponse> callbackSuccess, Action<Exception> callbackFailure)
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
		private void asyncRequest (WebRequest request, Uri url, Action<ServiceResponse> callbackSuccess, Action<Exception> callbackFailure)
		{
			Logger.Log (LogLevel.Info, "-> " + request.Method + " " + url);
			if (IsHostReachable (url.ToString ())) {
				if (callbackFailure != null) {
					callbackFailure (new ArgumentException (url + " is not reachable."));
				}
				return;
			}
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
					if (Constants.UseEncryption) {
						json = EncryptionHelper.Decrypt (json);
					}
					// Parse response
					ServiceResponse response = new ServiceResponse ();
					// Open Json
					JsonValue value = JsonObject.Parse (json);
					int code = Convert.ToInt32 (value ["code"].ToString ());
					string message = null;
					string data = null;
					if (value.ContainsKey ("message")) {
						message = value ["message"];
					}
					if (value.ContainsKey ("r")) {
						data = value ["r"].ToString ();
					}
					response.Code = code;
					response.Message = message;
					response.JsonData = data;
					if (code == 0) {
						Logger.Log (LogLevel.Info, "<- OK ");
						if (callbackSuccess != null) {
							callbackSuccess (response);
						}
					} else {
						Logger.Log (LogLevel.Error, "<- KO " + code + ": " + message);
						if (callbackFailure != null) {
							callbackFailure (new ArgumentException (code + ": " + message));
						}
					}
				} catch (Exception e) {
					// Log and callback
					Logger.LogException (LogLevel.Error, "WebserviceCaller.RequestJsonAsync ", e);
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

