using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Exceptions
{
    public class DeletedException : Exception
    {
        public DeletedException(string message) : base(message) { }

        public DeletedException(string message, Exception inner) : base(message, inner) { }
    }
}
