using Microsoft.EntityFrameworkCore.Query;
using Onion.Application.DataAccess.Database.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Onion.Infrastructure.DataAccess.Database.Specifications;

public interface ISpecification<T> where T : BaseEntity
{
    Expression<Func<T, bool>> Filter { get; set; }
    ICollection<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; set; }
    int? Take { get; set; }
    int? Skip { get; set; }

}
