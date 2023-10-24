using Microsoft.EntityFrameworkCore;

using Sisa.Abstractions;
using Sisa.Extensions;

namespace Sisa.Data;

public static partial class SpecificationExtensions
{
    public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> specification)
        where TEntity : class, IEntity
    {
        // Apply includes
        query = specification.Includes
            .Aggregate(query, (current, include) => current.Include(include));

        // Apply include strings
        query = specification.IncludeStrings
            .Aggregate(query, (current, include) => current.Include(include));

        if (specification.Criteria != null)
        {
            query = query
                .Where(specification.Criteria);
        }

        if (specification.FilteringParams != null)
        {
            query = query.Where(specification.FilteringParams);
        }

        // Apply ordering if expressions are set
        if (specification.SortingParams != null && specification.SortingParams.Any())
        {
            query = query.OrderBy(specification.SortingParams);
        }
        else
        {
            if (specification.OrderBy != null)
            {
                query = query
                    .OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query
                    .OrderByDescending(specification.OrderByDescending);
            }
        }

        // Apply GroupBy
        if (specification.GroupBy != null)
        {
            query = query
                .GroupBy(specification.GroupBy)
                .SelectMany(x => x);
        }

        if (!specification.EnableTracking)
        {
            query = query
                .AsNoTracking();
        }

        if (specification.EnableSplitQuery)
        {
            query = query
                .AsSingleQuery();
        }

        if (!string.IsNullOrWhiteSpace(specification.Tag))
        {
            query = query
                .TagWith(specification.Tag);
        }

        return query;
    }

    public static IQueryable<TResult> Specify<TEntity, TResult>(this IQueryable<TEntity> query, ISpecification<TEntity, TResult> specification)
        where TEntity : class, IEntity
    {
        if (specification.Selector is null)
        {
            throw new ArgumentNullException(nameof(specification.Selector));
        }

        // Apply includes
        query = specification.Includes
            .Aggregate(query, (current, include) => current.Include(include));

        // Apply include strings
        query = specification.IncludeStrings
            .Aggregate(query, (current, include) => current.Include(include));

        if (specification.Criteria != null)
        {
            query = query
                .Where(specification.Criteria);
        }

        if (specification.FilteringParams != null)
        {
            query = query.Where(specification.FilteringParams);
        }

        // Apply ordering if expressions are set
        if (specification.SortingParams != null && specification.SortingParams.Any())
        {
            query = query.OrderBy(specification.SortingParams);
        }
        else
        {
            if (specification.OrderBy != null)
            {
                query = query
                    .OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query
                    .OrderByDescending(specification.OrderByDescending);
            }
        }

        // Apply GroupBy
        if (specification.GroupBy != null)
        {
            query = query
                .GroupBy(specification.GroupBy)
                .SelectMany(x => x);
        }

        if (!specification.EnableTracking)
        {
            query = query
                .AsNoTracking();
        }

        if (specification.EnableSplitQuery)
        {
            query = query
                .AsSingleQuery();
        }

        if (!string.IsNullOrWhiteSpace(specification.Tag))
        {
            query = query
                .TagWith(specification.Tag);
        }

        return query
            .Select(specification.Selector);
    }
}
