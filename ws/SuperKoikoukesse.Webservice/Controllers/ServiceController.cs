using Pixelnest.Common;
using Pixelnest.Common.Json;
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

        /// <summary>
        /// State of the WS
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServiceStateModel model = new ServiceStateModel();
            return View(model);
        }

        /// <summary>
        /// Get a bunch of games corresponding to the given criteria
        /// </summary>
        /// <returns></returns>
        public ActionResult Games()
        {
            m_useEncryption = Convert.ToBoolean(ConfigurationManager.AppSettings["USE_ENCYRPTION"]);

            ServiceResponse response = new ServiceResponse();
            response.Code = ErrorCodeEnum.Ok;
            response.Message = "Everything is OK!";

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