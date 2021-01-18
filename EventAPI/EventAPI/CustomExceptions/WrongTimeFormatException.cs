﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.CustomExceptions
{
    public class WrongTimeFormatException : Exception
    {
        public WrongTimeFormatException(string message) : base(message) { }
    }
}
