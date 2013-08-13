using System;
using System.Json;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Kind of objects returned by webservice calls
	/// </summary>
	public interface IServiceOutput
	{
		/// <summary>
		/// Fills the object with the specified json
		/// </summary>
		/// <param name="json">Json.</param>
		void BuildFromJsonObject (JsonValue json);
	}
}

