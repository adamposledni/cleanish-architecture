using Onion.Application.DataAccess.Database.Entities;

namespace Onion.Infrastructure.DataAccess.Database.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification) where TEntity : BaseEntity
    {
        if (specification == null) return query;

        if (specification.Filter != null)
        {
            query = query.Where(specification.Filter);
        }
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        if (specification.Take != null)
        {
            query = query.Take(specification.Take.Value);
        }
        if (specification.Skip != null)
        {
            query = query.Skip(specification.Take.Value);
        }
        query = specification.Includes.Aggregate(query, (current, include) => include(current));

        return query;
    }
}
