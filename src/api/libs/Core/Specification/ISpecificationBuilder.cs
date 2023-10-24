using System.Linq.Expressions;

namespace Sisa.Abstractions;

public interface ISpecificationBuilder<TEntity>
    where TEntity : class
{
    SpecificationBuilder<TEntity> Include(Expression<Func<TEntity, object?>> includeExpression);
    SpecificationBuilder<TEntity> Include(string includeString);

    SpecificationBuilder<TEntity> Where(Expression<Func<TEntity, bool>> criteria);

    SpecificationBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> orderByExpression);
    SpecificationBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression);

    SpecificationBuilder<TEntity> Filter(IFilteringParams filteringParams);
    SpecificationBuilder<TEntity> Sort(IEnumerable<ISortingParams> sortingParams);

    SpecificationBuilder<TEntity> GroupBy(Expression<Func<TEntity, object>> groupByExpression);

    SpecificationBuilder<TEntity> Paginate(IPagingParams pagingParams);

    SpecificationBuilder<TEntity> And(ISpecification<TEntity> specification);
    SpecificationBuilder<TEntity> Or(ISpecification<TEntity> specification);

    SpecificationBuilder<TEntity> AsNoTracking();
    SpecificationBuilder<TEntity> AsSplitQuery();
    SpecificationBuilder<TEntity> TagWith(string tag);
}
