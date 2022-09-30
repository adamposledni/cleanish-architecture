using Onion.Application.DataAccess.Database.Entities;
using System.Linq.Expressions;

namespace Onion.Infrastructure.DataAccess.Database.Specifications;

public class OldSpecification<T> : ISpecification<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> Filter { get; set; }

    public ICollection<Expression<Func<T, object>>> Includes { get; private set; } = new List<Expression<Func<T, object>>>();

    public Expression<Func<T, object>> OrderBy { get; set; }

    public int? Take { get; set; }

    public int? Skip { get; set; }
}