﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException(string message) : base(message) { }

        public InvalidIdException(string message, Exception inner) : base(message, inner) { }
    }
}