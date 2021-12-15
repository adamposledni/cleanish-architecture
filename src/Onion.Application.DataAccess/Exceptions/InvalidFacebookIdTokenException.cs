using Onion.Application.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Exceptions
{
    public class InvalidFacebookIdTokenException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidFacebookIdToken";

        public InvalidFacebookIdTokenException() : base(MESSAGE_KEY)
        {
        }
    }
}
