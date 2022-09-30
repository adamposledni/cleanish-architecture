using Microsoft.EntityFrameworkCore.Query;
using Onion.Application.DataAccess.Database.Entities;
using System.Linq.Expressions;

namespace Onion.Infrastructure.DataAccess.Database.Specifications;

public class SpecificationBuilder<T> where T: BaseEntity
{
    private Specification<T> _specification = new();

    public SpecificationBuilder<T> SetFilter(Expression<Func<T, bool>> filter)
    {
        _specification.Filter = filter;
        return this;
    }

    public SpecificationBuilder<T> AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
    {
        _specification.Includes.Add(include);
        return this;
    }

    public SpecificationBuilder<T> SetOrderBy(Expression<Func<T, object>> orderBy)
    {
        _specification.OrderBy = orderBy;
        return this;
    }

    public SpecificationBuilder<T> SetTake(int take)
    {
        _specification.Take = take;
        return this;
    }

    public SpecificationBuilder<T> SetSkip(int skip)
    {
        _specification.Skip = skip;
        return this;
    }

    public Specification<T> Build()
    {
        return _specification;
    }
}
