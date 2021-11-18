using System;

namespace Onion.Application.DataAccess.Database.Entities
{
    public abstract class DatabaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
