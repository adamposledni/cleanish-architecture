using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Exceptions
{
    public class InvalidRequestException : BadRequestException
    {
        private const string MESSAGE_KEY = "InvalidRequest";

        public InvalidRequestException(string details) : base(MESSAGE_KEY, details)
        {
        }
    }
}
