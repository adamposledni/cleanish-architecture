using System;

namespace Onion.Application.Services.Exceptions
{
    public class BadRequestException : Exception
    {
        public string MessageKey { get; private set; }

        public BadRequestException(string messageKey) : base()
        {
            MessageKey = messageKey;
        }
    }
}
