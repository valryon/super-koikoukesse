// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Json;
using Superkoikoukesse.Common.Networking;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Generic webservice caller linked to a type
	/// </summary>
	public abstract class BaseModelServiceCaller<T> : BaseServiceCaller
		where T:IServiceOutput,new()
	{
		public BaseModelServiceCaller ()
			: base()
		{
		}

		/// <summary>
		/// Request the webservice
		/// </summary>
		public void Request (Action<T> callback, Action<int, Exception> callbackFailure)
		{
			// Make the call
			this.RequestJsonAsync (
				(response => {

				try {
					// Transform the json to a JsonObject
					JsonValue jsonObject = JsonValue.Parse (response.JsonData);

					T serviceOutput = new T ();
					serviceOutput.BuildFromJsonObject (jsonObject);

					PostRequest (serviceOutput, true);

					if (callback != null) {
						callback (serviceOutput);
					}
				} catch (Exception e) {
					// Log and callback
					Logger.E( "GenericService.Request ", e);

					PostRequest (default(T), false);

					if (callbackFailure != null) {
						callbackFailure (-1, e);
					}
				}
			}),
			(code, ex) => {
				PostRequest (default(T), false);
				if (callbackFailure != null) {
					callbackFailure (code, ex);
				}
			}
			);
		}

		protected virtual T PostRequest (T parsedObject, bool success)
		{
			return parsedObject;
		}
	}
}

