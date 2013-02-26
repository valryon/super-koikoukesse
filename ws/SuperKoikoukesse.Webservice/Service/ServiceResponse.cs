using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SuperKoikoukesse.Webservice.Service
{
    /// <summary>
    /// Webservice formated response
    /// </summary>
    [DataContract]
    public class ServiceResponse
    {
        /// <summary>
        /// Error code
        /// </summary>
        [DataMember(Name = "code", IsRequired = true)]
        public ErrorCodeEnum Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        /// <summary>
        /// Serialized response
        /// </summary>
        [DataMember(Name = "r", EmitDefaultValue = false)]
        public object ResponseData { get; set; }
    }
}