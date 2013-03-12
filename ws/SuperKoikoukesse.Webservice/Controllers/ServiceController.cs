using Pixelnest.Common;
using Pixelnest.Common.Json;
using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Core.Exceptions;
using SuperKoikoukesse.Webservice.Core.Model;
using SuperKoikoukesse.Webservice.Models;
using SuperKoikoukesse.Webservice.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    /// <summary>
    /// Controller for the game webservice
    /// </summary>
    public class ServiceController : Controller
    {
        private bool m_useEncryption;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            m_useEncryption = Convert.ToBoolean(ConfigurationManager.AppSettings["USE_ENCYRPTION"]);
        }

        /// <summary>
        /// Webservice UI
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Generic webservice method
        /// </summary>
        /// <param name="callingDelegate"></param>
        /// <returns></returns>
        internal ServiceResponse CallService(Func<object> callingDelegate)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                response.ResponseData = callingDelegate();
            }
            catch (EmptyInputException e)
            {
                response.Code = ErrorCodeEnum.EmptyRequest;
                response.Message = e.ToString();
            }
            catch (ParseRequestException e)
            {
                response.Code = ErrorCodeEnum.InvalidRequest; ;
                response.Message = e.ToString();
            }
            catch (Exception e)
            {
                response.Code = ErrorCodeEnum.ServiceError;
                response.Message = e.ToString();
            }

            return response;
        }

        /// <summary>
        /// Get the excluded games
        /// </summary>
        /// <returns></returns>
        public ActionResult GamesExclusions()
        {
            var response = CallService(() =>
            {
                GamesDb db = new GamesDb();
                List<Game> games = db.ReadAll();

                return games.Where(g => g.IsRemoved).Select(s => s.GameId).ToList();
            });

            return PrepareResponse(response);
        }

        /// <summary>
        /// Retrieve player informations. Create if new
        /// </summary>
        /// <returns></returns>
        public ActionResult PlayerInfo(string playerId)
        {
            var response = CallService(() =>
            {
                PlayersDb playersDb = new PlayersDb();
                Player p = playersDb.GetPlayer(playerId);

                if (p == null)
                {
                    p = playersDb.CreatePlayer(playerId);
                }

                return p;
            });

            return PrepareResponse(response);
        }

        /// <summary>
        /// Consume a credit from the player
        /// </summary>
        /// <param name="r">See documentation for expected json format</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PlayerConsumeCredits(string r)
        {
            var response = CallService(() =>
            {
                return null;
            });

            return PrepareResponse(response);
        }

        /// <summary>
        /// Add an entry of a played game
        /// </summary>
        /// <param name="r">See documentation for expected json format</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StatsAddGame(string r)
        {
            var response = CallService(() =>
            {
                AddGameHistoryInput newStat = DecryptJsonRequest<AddGameHistoryInput>(r);

                StatsDb db = new StatsDb();
                db.Add(newStat.ToDbModel());

                return null;
            });

            return PrepareResponse(response);
        }

        /// <summary>
        /// Get configuration for a specified target (0 = all)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ActionResult Config(int target = 0)
        {
            var response = CallService(() =>
             {
                 ConfigurationDb db = new ConfigurationDb();
                 GameConfiguration config = db.GetConfiguration(target);

                 return config;
             });

            return PrepareResponse(response);
        }

        /// <summary>
        /// Decrypt and parse a Json request
        /// </summary>
        /// <param name="encryptedJson"></param>
        /// <returns></returns>
        internal T DecryptJsonRequest<T>(string encryptedJson)
        {
            if (string.IsNullOrEmpty(encryptedJson))
            {
                throw new EmptyInputException("Input was empty!");
            }

            try
            {
                string clearJson = EncryptionHelper.Decrypt(encryptedJson);

                T item = JsonHelper.Deserialize<T>(clearJson);

                return item;
            }
            catch (Exception e)
            {
                throw new ParseRequestException("Error during the parsing of the json body of the request. Input : " + encryptedJson, e);
            }
        }

        /// <summary>
        /// Format and encrypt response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal ActionResult PrepareResponse(ServiceResponse response)
        {
            bool format = false;

#if DEBUG
            format = true;
#endif

            string json = JsonHelper.Serialize(response, format);

            // Encryption
            if (m_useEncryption)
            {
                return new ContentResult()
                {
                    Content = EncryptionHelper.Encrypt(json),
                    ContentType = "application/octet-stream"
                };
            }
            else
            {
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "text/json"
                };
            }
        }


    }
}