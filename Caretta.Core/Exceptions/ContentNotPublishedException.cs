using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Exceptions
{
    public class ContentNotPublishedException : Exception
    {
        public ContentNotPublishedException(string message) : base(message) { }

        public ContentNotPublishedException(string message, Exception inner) : base(message, inner) { }
    }
}
