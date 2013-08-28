// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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

