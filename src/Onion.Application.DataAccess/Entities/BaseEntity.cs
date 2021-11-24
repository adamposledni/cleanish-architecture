using System;

namespace Onion.Application.DataAccess.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
