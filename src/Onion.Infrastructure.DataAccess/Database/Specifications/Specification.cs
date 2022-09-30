using Microsoft.EntityFrameworkCore.Query;
using Onion.Application.DataAccess.Database.Entities;
using System.Linq.Expressions;

namespace Onion.Infrastructure.DataAccess.Database.Specifications;

public class Specification<T> : ISpecification<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> Filter { get; set; }

    public ICollection<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();

    public Expression<Func<T, object>> OrderBy { get; set; }

    public int? Take { get; set; }

    public int? Skip { get; set; }
}