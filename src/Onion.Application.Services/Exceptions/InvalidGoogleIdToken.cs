using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Exceptions
{
    public class InvalidGoogleIdTokenException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidGoogleIdToken";

        public InvalidGoogleIdTokenException() : base(MESSAGE_KEY)
        {
        }
    }
}
