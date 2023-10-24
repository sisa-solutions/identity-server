using System.Linq.Expressions;

namespace Sisa.Abstractions;

/// <summary>
/// ISpecification is the interface that will be used to create specifications
/// </summary>
public interface ISpecification { }

/// <summary>
/// ISpecification is the interface that will be used to create specifications
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISpecification<TEntity> : ISpecification
    where TEntity : class
{
    /// <summary>
    /// List of associated entities to include in the query
    /// </summary>
    List<Expression<Func<TEntity, object?>>> Includes { get; }

    /// <summary>
    /// List of associated entities to include in the query
    /// </summary>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Criteria is the expression that will be used to filter the data
    /// </summary>
    Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    /// OrderBy is the expression that will be used to order the data ascending
    /// </summary>
    Expression<Func<TEntity, object>>? OrderBy { get; }

    /// <summary>
    /// OrderByDescending is the expression that will be used to order the data descending
    /// </summary>
    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    IEnumerable<ISortingParams>? SortingParams { get; }

    IFilteringParams? FilteringParams { get; }

    /// <summary>
    /// GroupBy is the expression that will be used to group the data
    /// </summary>
    Expression<Func<TEntity, object>>? GroupBy { get; }

    /// <summary>
    /// Select is the expression that will be used to select the data
    /// </summary>
    IPagingParams? PagingParams { get; }

    /// <summary>
    /// And is the expression that will be used to join the data
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    ISpecification<TEntity> And(ISpecification<TEntity> specification);

    /// <summary>
    /// Or is the expression that will be used to join the data
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    ISpecification<TEntity> Or(ISpecification<TEntity> specification);

    /// <summary>
    /// EnableTracking is a boolean value that indicates if tracking is enabled
    /// </summary>
    bool EnableTracking { get; }

    /// <summary>
    /// SplitQuery is a boolean value that indicates if split query is enabled
    /// </summary>
    bool EnableSplitQuery { get; }

    string Tag { get; }
}

/// <summary>
/// ISpecification is the interface that will be used to create specifications with a selector
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ISpecification<TEntity, TResult> : ISpecification<TEntity>
    where TEntity : class
{
    Expression<Func<TEntity, TResult>> Selector { get; }
}
