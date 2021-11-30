using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Exceptions
{
    public class InvalidEmailPasswordException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidEmailPassword";
        public InvalidEmailPasswordException() : base(MESSAGE_KEY)
        {
        }
    }
}
