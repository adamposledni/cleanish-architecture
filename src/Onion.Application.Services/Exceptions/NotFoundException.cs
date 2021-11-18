using System;

namespace Onion.Application.Services.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        public int Id { get; set; }
        public string MessageKey { get; private set; }

        public NotFoundException(int id, string messageKey) : base()
        {
            Id = id;
            MessageKey = messageKey;
        }
    }
}
