using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Exceptions
{
    [Serializable]
    public class UnknowObjectException : Exception
    {
        public UnknowObjectException() { }
        public UnknowObjectException(string message) : base(message) { }
        public UnknowObjectException(string message, Exception inner) : base(message, inner) { }
        protected UnknowObjectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
