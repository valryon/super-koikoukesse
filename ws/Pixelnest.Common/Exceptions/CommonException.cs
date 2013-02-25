using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixelnest.Common.Exceptions
{
    [Serializable]
    public class CommonException : Exception
    {
        public CommonException() { }
        public CommonException(string message) : base(message) { }
        public CommonException(string message, Exception inner) : base(message, inner) { }
        protected CommonException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
