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
	public class ServiceGetPlayer : BaseModelServiceCaller<Player>
	{
		private string mId;

		public ServiceGetPlayer (string playerId)
		{
			this.mId = playerId;
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "ws/player/" + mId);
		}

	}

	/// <summary>
	/// Create a new player profile
	/// </summary>
	public class ServiceCreatePlayer : BaseServiceCaller
	{
		private Player mPlayer;
		
		public ServiceCreatePlayer (Player p)
		{
			this.mPlayer = p;
		}
		
		/// <summary>
		/// Create a new player. We use a POST request to avoid spam
		/// </summary>
		public void CreatePlayer (Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (mPlayer.Id));
			json.Add ("platform", new JsonPrimitive ("ios")); // TODO Android
			json.Add ("credits", new JsonPrimitive (mPlayer.Credits));
			json.Add ("coins", new JsonPrimitive (mPlayer.Coins));
			
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
	public class ServicePlayerCredits : BaseServiceCaller
	{
		private Player mPlayer;

		public ServicePlayerCredits (Player p)
		{
			this.mPlayer = p;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="credits"></param>
		public void AddCredits (int credits, Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (mPlayer.Id));
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
	public class ServicePlayerCoins : BaseServiceCaller
	{
		private Player mPlayer;
		
		public ServicePlayerCoins (Player p)
		{
			this.mPlayer = p;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="credits"></param>
		public void AddCoins (int coins, Action callback = null, Action<int> callbackFailure = null)
		{
			JsonObject json = new JsonObject ();
			json.Add ("player", new JsonPrimitive (mPlayer.Id));
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

