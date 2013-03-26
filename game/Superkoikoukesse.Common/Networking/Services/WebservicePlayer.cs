using System;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using Superkoikoukesse.Common.Networking;
using System.Json;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Get a player profile (or test if it exists)
	/// </summary>
	public class WebserviceGetPlayer : GenericModelWeberviceCaller<Player>
	{
		private string id;

		public WebserviceGetPlayer (string playerId)
		{
			this.id = playerId;
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/player/" + HttpUtility.UrlEncode (id));
		}

	}

	/// <summary>
	/// Create a new player profile
	/// </summary>
	public class WebserviceCreatePlayer : BaseWebserviceCaller
	{
		private Player player;
		
		public WebserviceCreatePlayer (Player p)
		{
			this.player = p;
		}
		
		/// <summary>
		/// Create a new player. We use a POST request to avoid spam
		/// </summary>
		public void CreatePlayer ()
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("platform", new JsonPrimitive ("ios")); // TODO Android
			json.Add ("credits", new JsonPrimitive (player.Credits));
			json.Add ("coins", new JsonPrimitive (player.Coins));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				
			}, 
			(exception) => {
				
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/player/" + HttpUtility.UrlEncode (player.Id));
		}
		
	}

	/// <summary>
	/// Manage player credits
	/// </summary>
	public class WebservicePlayerCredits : BaseWebserviceCaller
	{
		private Player player;

		public WebservicePlayerCredits (Player p)
		{
			this.player = p;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="credits"></param>
		public void UseCredits (int credits)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("credits", new JsonPrimitive (credits));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				
			}, 
			(exception) => {
				
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/player/" + HttpUtility.UrlEncode (player.Id)+"/credits");
		}
		
	}

	/// <summary>
	/// Manage player coins
	/// </summary>
	public class WebservicePlayerCoins : BaseWebserviceCaller
	{
		private Player player;
		
		public WebservicePlayerCoins (Player p)
		{
			this.player = p;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="credits"></param>
		public void UseCoins (int coins)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("coins", new JsonPrimitive (coins));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				
			}, 
			(exception) => {
				
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/player/" + HttpUtility.UrlEncode (player.Id)+"/coins");
		}

		
	}
}

