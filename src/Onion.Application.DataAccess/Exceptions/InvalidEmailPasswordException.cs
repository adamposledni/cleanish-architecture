using Onion.Application.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Exceptions
{
    public class InvalidEmailPasswordException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidEmailPassword";
        public InvalidEmailPasswordException() : base(MESSAGE_KEY)
        {
        }
    }
}
