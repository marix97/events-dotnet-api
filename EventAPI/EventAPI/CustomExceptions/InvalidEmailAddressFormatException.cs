using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.CustomExceptions
{
    public class InvalidEmailAddressFormatException : Exception
    {
        public InvalidEmailAddressFormatException(string message) : base(message) { }
    }
}
