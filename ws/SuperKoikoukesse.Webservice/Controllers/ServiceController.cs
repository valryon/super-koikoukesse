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
        /// Get the excluded games
        /// </summary>
        /// <returns></returns>
        public ActionResult Exclusions()
        {
            ServiceResponse response = new ServiceResponse();
            response.Code = ErrorCodeEnum.Ok;

            try
            {
                GamesDb db = new GamesDb();
                List<Game> games = db.ReadAll();

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