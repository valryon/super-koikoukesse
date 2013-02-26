using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.Service
{
    /// <summary>
    /// Available error codes for webservice
    /// </summary>
    public enum ErrorCodeEnum : int
    {
        Ok  = 0,
        DatabaseNotFound = 100,
        ServiceError = 500,
    }
}