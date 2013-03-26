using System;
using System.Xml.Serialization;
using System.IO;
using System.Web;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice de configuration
	/// </summary>
	public class WebservicePlayer : GenericModelWeberviceCaller<Player>
	{
		private string id;

		public WebservicePlayer (string playerId)
		{
			this.id = playerId;
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/player/" +   HttpUtility.UrlEncode(id));
		}

	}
}

