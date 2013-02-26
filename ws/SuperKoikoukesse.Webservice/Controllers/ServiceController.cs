using Pixelnest.Common;
using Pixelnest.Common.Json;
using SuperKoikoukesse.Webservice.Core.Dao;
using SuperKoikoukesse.Webservice.Core.Exceptions;
using SuperKoikoukesse.Webservice.Core.Games;
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
        private string dbPath; 

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            m_useEncryption = Convert.ToBoolean(ConfigurationManager.AppSettings["USE_ENCYRPTION"]);
            dbPath = Server.MapPath(ConfigurationManager.AppSettings["GAME_DB_PATH"].ToString());
        }

        /// <summary>
        /// State of the WS
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            ServiceStateModel model = new ServiceStateModel();
            return View(model);
        }

        /// <summary>
        /// Get the whole database list
        /// </summary>
        /// <returns></returns>
        public ActionResult Games()
        {
            ServiceResponse response = new ServiceResponse();
            response.Code = ErrorCodeEnum.Ok;

            try
            {
                GameInfoDb db = new GameInfoDb(dbPath);
                List<GameInfo> games = db.ReadAll();

                response.ResponseData = games;
            }
            catch (DatabaseNotFoundException dbe)
            {
                response.Code = ErrorCodeEnum.DatabaseNotFound;
                response.Message = dbe.ToString();
            }
            catch (Exception e)
            {
                response.Code = ErrorCodeEnum.ServiceError;
                response.Message = e.ToString();
            }

            return PrepareResponse(response);
        }

        /// <summary>
        /// Get the excluded games
        /// </summary>
        /// <returns></returns>
        public ActionResult Exclusions()
        {
            ServiceResponse response = new ServiceResponse();
            response.Code = ErrorCodeEnum.Ok;

            try
            {
                GameInfoDb db = new GameInfoDb(dbPath);
                List<GameInfo> games = db.ReadAll();

                response.ResponseData = games.Where(g => g.IsRemoved).Select(s => s.GameId).ToList();
            }
            catch (DatabaseNotFoundException dbe)
            {
                response.Code = ErrorCodeEnum.DatabaseNotFound;
                response.Message = dbe.ToString();
            }
            catch (Exception e)
            {
                response.Code = ErrorCodeEnum.ServiceError;
                response.Message = e.ToString();
            }

            return PrepareResponse(response);
        }

        /// <summary>
        /// Format and encrypt response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal ActionResult PrepareResponse(ServiceResponse response)
        {
            string json = JsonHelper.Serialize(response);

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