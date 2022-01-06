using System;

namespace Onion.Application.DataAccess.Exceptions.Common
{
    public abstract class NotFoundException : Exception
    {
        public Guid Id { get; set; }
        public string MessageKey { get; private set; }

        public NotFoundException(Guid id, string messageKey) : base()
        {
            Id = id;
            MessageKey = messageKey;
        }
    }
}
