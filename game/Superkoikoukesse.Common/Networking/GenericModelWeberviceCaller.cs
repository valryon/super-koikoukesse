using System;
using System.Json;
using Superkoikoukesse.Common.Networking;

namespace Superkoikoukesse.Common
{

	/// <summary>
	/// Generic webservice caller
	/// </summary>
	public abstract class GenericModelWeberviceCaller<T> : BaseWebserviceCaller
		where T:IServiceOutput,new()
	{
		protected bool UseEncryption;

		public GenericModelWeberviceCaller ()
			: base()
		{
			UseEncryption = true;
		}

		/// <summary>
		/// Request the webservice
		/// </summary>
		public override void Request (Action<T> callback, Action<Exception> callbackFailure)
		{
			// Get the URL
			Uri uri = GetServiceUrl ();

			// Make the call
			this.RequestJsonAsync (
				uri,
				UseEncryption,
				(response => {

				try {
					// Transform the json to a JsonObject
					JsonValue jsonObject = JsonValue.Parse (response.JsonData);

					T serviceOutput = new T ();
					serviceOutput.BuildFromJsonObject (jsonObject);

					PostRequest(serviceOutput);

					if(callback != null) {
						callback(serviceOutput);
					}
				}
				catch(Exception e) {
					// Log and callback
					Logger.LogException (LogLevel.Error, "GenericService.Request ", e);
					
					if (callbackFailure != null) {
						callbackFailure (e);
					}
				}
			}),
				callbackFailure
			);
		}

		protected virtual T PostRequest (T parsedObject) {
			return parsedObject;
		}

		/// <summary>
		/// Get the service url to call
		/// </summary>
		/// <returns>The service URL.</returns>
		public abstract Uri GetServiceUrl ();
	}
}

