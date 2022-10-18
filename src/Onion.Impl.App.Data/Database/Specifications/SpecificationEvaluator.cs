using Onion.App.Data.Database.Entities;

namespace Onion.Impl.App.Data.Database.Specifications;

internal static class SpecificationEvaluator
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
        if (specification.OrderBy == null && specification.OrderByDesc != null)
        {
            query = query.OrderByDescending(specification.OrderByDesc);
        }

        query = specification.Includes.Aggregate(query, (current, include) => include(current));


        return query;
    }
}
