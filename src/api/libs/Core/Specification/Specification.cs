using System.Linq.Expressions;

namespace Sisa.Abstractions;

public class Specification<TEntity> : ISpecification<TEntity>
    where TEntity : class
{
    public virtual ISpecificationBuilder<TEntity> Builder { get; }

    public Specification()
    {
        Builder = new SpecificationBuilder<TEntity>(this);
    }

    public Specification(Expression<Func<TEntity, bool>> criteria) : this()
    {
        Criteria = criteria;
    }

    public Specification(Expression<Func<TEntity, bool>> criteria, IPagingParams pagingParams) : this(criteria)
    {
        PagingParams = pagingParams;
    }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public List<Expression<Func<TEntity, object?>>> Includes { get; } = [];

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public List<string> IncludeStrings { get; } = [];

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public IEnumerable<ISortingParams>? SortingParams { get; private set; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public IFilteringParams? FilteringParams { get; private set; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    public IPagingParams? PagingParams { get; private set; }

    /// <summary>
    /// EnableTracking is a boolean value that indicates if tracking is enabled
    /// </summary>
    public bool EnableTracking { get; private set; } = true;

    /// <summary>
    /// SplitQuery is a boolean value that indicates if split query is enabled
    /// </summary>
    public bool EnableSplitQuery { get; private set; } = true;

    /// <summary>
    /// Tag is a string that can be used to tag a specification
    /// </summary>
    public string Tag { get; private set; } = string.Empty;

    /// <summary>
    /// Include is the expression that will be used to include the data
    /// </summary>
    /// <param name="includeExpression"></param>
    public virtual void AddInclude(Expression<Func<TEntity, object?>> includeExpression)
        => Includes.Add(includeExpression);

    /// <summary>
    /// Include is the expression that will be used to include the data
    /// </summary>
    /// <param name="includeString"></param>
    public virtual void AddInclude(string includeString)
        => IncludeStrings.Add(includeString);

    /// <summary>
    /// Criteria is the expression that will be used to filter the data
    /// </summary>
    /// <param name="criteria"></param>
    public virtual void ApplyCriteria(Expression<Func<TEntity, bool>> criteria)
        => Criteria = criteria;

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    /// <param name="orderByExpression"></param>
    public virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        => OrderBy = orderByExpression;

    /// <summary>
    /// OrderBy is the expression that will be used to order the data
    /// </summary>
    /// <param name="orderByDescendingExpression"></param>
    public virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        => OrderByDescending = orderByDescendingExpression;

    /// <summary>
    /// GroupBy is the expression that will be used to group the data
    /// </summary>
    /// <param name="groupByExpression"></param>
    public virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        => GroupBy = groupByExpression;

    public virtual void ApplyPaging(IPagingParams pagingParams)
        => PagingParams = pagingParams;


    public virtual void ApplySort(IEnumerable<ISortingParams> sortingParams)
        => SortingParams = sortingParams;

    public virtual void ApplyFilter(IFilteringParams filteringParams)
        => FilteringParams = filteringParams;

    public virtual ISpecification<TEntity> And(ISpecification<TEntity> specification)
    {
        if (specification.Criteria is null)
        {
            throw new ArgumentNullException(nameof(specification.Criteria));
        }

        if (Criteria is null)
        {
            Criteria = specification.Criteria;
        }
        else
        {
            var newCriteria = Expression.AndAlso(Criteria.Body, specification.Criteria.Body);

            Criteria = Expression.Lambda<Func<TEntity, bool>>(newCriteria, Criteria.Parameters);
        }

        return this;
    }

    public virtual ISpecification<TEntity> Or(ISpecification<TEntity> specification)
    {
        if (specification.Criteria is null)
        {
            throw new ArgumentNullException(nameof(specification.Criteria));
        }

        if (Criteria is null)
        {
            Criteria = specification.Criteria;
        }
        else
        {
            var newCriteria = Expression.OrElse(Criteria.Body, specification.Criteria.Body);

            Criteria = Expression.Lambda<Func<TEntity, bool>>(newCriteria, Criteria.Parameters);
        }

        return this;
    }

    public virtual void AsNoTracking()
        => EnableTracking = false;

    public virtual void AsSplitQuery()
        => EnableSplitQuery = false;

    /// <summary>
    /// Tag is a string that can be used to tag a specification
    /// </summary>
    /// <param name="tag"></param>
    public virtual void TagWith(string tag)
        => Tag = tag;
}

public class Specification<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector) : Specification<TEntity>, ISpecification<TEntity, TResult>
    where TEntity : class
{
    public Expression<Func<TEntity, TResult>> Selector { get; private set; } = selector;

    public Specification(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> criteria) : this(selector)
    {
        ApplyCriteria(criteria);
    }

    public Specification(Expression<Func<TEntity, TResult>> selector, IPagingParams pagingParams) : this(selector)
    {
        ApplyPaging(pagingParams);
    }

    public Specification(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> criteria, IPagingParams pagingParams) : this(selector, criteria)
    {
        ApplyPaging(pagingParams);
    }
}
