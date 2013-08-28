// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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
