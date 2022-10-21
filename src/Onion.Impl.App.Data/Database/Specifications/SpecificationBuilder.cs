using Microsoft.EntityFrameworkCore.Query;
using Onion.App.Data.Database.Entities;
using System.Linq.Expressions;

namespace Onion.Impl.App.Data.Database.Specifications;

public class SpecificationBuilder<T> where T : BaseEntity
{
    private readonly ISpecification<T> _specification = new Specification<T>();

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

    public SpecificationBuilder<T> SetOrderByDesc(Expression<Func<T, object>> orderByDesc)
    {
        _specification.OrderByDesc = orderByDesc;
        return this;
    }

    public SpecificationBuilder<T> SetSkip(int skip)
    {
        _specification.Skip = skip;
        return this;
    }

    public SpecificationBuilder<T> SetTake(int take)
    {
        _specification.Take = take;
        return this;
    }

    public ISpecification<T> Build()
    {
        return _specification;
    }
}
