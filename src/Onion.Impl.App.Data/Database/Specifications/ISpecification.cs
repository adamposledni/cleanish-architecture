using Microsoft.EntityFrameworkCore.Query;
using Onion.App.Data.Database.Entities;
using System.Linq.Expressions;

namespace Onion.Impl.App.Data.Database.Specifications;

internal interface ISpecification<T> where T : BaseEntity
{
    Expression<Func<T, bool>> Filter { get; set; }
    Expression<Func<T, object>> OrderBy { get; set; }
    Expression<Func<T, object>> OrderByDesc { get; set; }
    ICollection<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }
    int? Skip { get; set; }
    int? Take { get; set; }

}
