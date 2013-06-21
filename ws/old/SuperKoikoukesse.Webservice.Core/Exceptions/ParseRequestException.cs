using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Exceptions
{
    [Serializable]
    public class ParseRequestException : GenericServiceException
    {
        public ParseRequestException() { }
        public ParseRequestException(string message) : base(message) { }
        public ParseRequestException(string message, Exception inner) : base(message, inner) { }
        protected ParseRequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
