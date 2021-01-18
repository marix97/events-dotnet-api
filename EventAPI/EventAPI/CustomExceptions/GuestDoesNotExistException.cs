using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.CustomExceptions
{
    public class GuestDoesNotExistException : Exception
    {
        public GuestDoesNotExistException(string message) : base(message)
        {
        }
    }
}
