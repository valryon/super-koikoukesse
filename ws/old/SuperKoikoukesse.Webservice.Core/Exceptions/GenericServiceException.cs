using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Exceptions
{
    [Serializable]
    public class GenericServiceException : Exception
    {
        public GenericServiceException() { }
        public GenericServiceException(string message) : base(message) { }
        public GenericServiceException(string message, Exception inner) : base(message, inner) { }
        protected GenericServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
