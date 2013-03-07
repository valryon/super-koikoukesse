using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice de configuration
	/// </summary>
	public class ServiceConfiguration : GenericService<GameConfiguration>
	{
		public ServiceConfiguration ()
		{
		}

		public override Uri GetServiceUrl ()
		{
			throw new NotImplementedException ();
		}

	}
}

