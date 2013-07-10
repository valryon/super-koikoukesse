using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Exceptions
{
    [Serializable]
    public class ObjectExistsException : Exception
    {
        public ObjectExistsException() { }
        public ObjectExistsException(string message) : base(message) { }
        public ObjectExistsException(string message, Exception inner) : base(message, inner) { }
        protected ObjectExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
