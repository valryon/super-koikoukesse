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
			return new Uri (Constants.WEBSERVICE_URL + "ws/player/" + id);
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
		public void CreatePlayer (Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("platform", new JsonPrimitive ("ios")); // TODO Android
			json.Add ("credits", new JsonPrimitive (player.Credits));
			json.Add ("coins", new JsonPrimitive (player.Coins));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				if(callback != null) {
					callback();
				}
			}, 
			(code, ex) => {
				if(callbackFailure != null) callbackFailure(code);
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "ws/player/");
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
		public void AddCredits (int credits, Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("credits", new JsonPrimitive (credits));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				if(callback != null) callback();
			}, 
			(code, ex) => {
				if(callbackFailure != null) callbackFailure(code);
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "ws/playerupdate/credits");
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
		public void AddCoins (int coins, Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (player.Id));
			json.Add ("coins", new JsonPrimitive (coins));
			
			this.RequestPostJsonAsync (json.ToString (), 
			                           (success) => {
				if(callback != null) callback();
			}, 
			(code, ex) => {
				if(callbackFailure != null) callbackFailure(code);
			});
			
		}
		
		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "ws/playerupdate/coins");
		}

		
	}
}

