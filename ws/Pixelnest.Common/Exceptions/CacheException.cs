using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixelnest.Common.Exceptions
{
    [Serializable]
    public class CacheException : Exception
    {
        public CacheException() { }
        public CacheException(string message) : base(message) { }
        public CacheException(string message, Exception inner) : base(message, inner) { }
        protected CacheException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
