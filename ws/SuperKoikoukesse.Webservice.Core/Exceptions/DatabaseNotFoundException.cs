using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Exceptions
{
    [Serializable]
    public class DatabaseNotFoundException : GenericServiceException
    {
        public DatabaseNotFoundException() { }
        public DatabaseNotFoundException(string message) : base(message) { }
        public DatabaseNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
