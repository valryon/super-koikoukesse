using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Core.Engine.Exceptions
{
    [Serializable]
    public class ContentLoadException : GameException
    {
        public ContentLoadException() { }
        public ContentLoadException(string message) : base(message) { }
        public ContentLoadException(string message, Exception inner) : base(message, inner) { }
        protected ContentLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
