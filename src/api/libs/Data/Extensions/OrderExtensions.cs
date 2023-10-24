using System.Linq.Expressions;

using Sisa.Abstractions;
using Sisa.Constants;
using Sisa.Enums;

namespace Sisa.Extensions;

public static partial class OrderExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, IEnumerable<ISortingParams> sortingParams)
        where TEntity : class
    {
        Type entityType = typeof(TEntity);
        ParameterExpression parameter = Expression.Parameter(entityType, "x");
        Expression expression = query.Expression;

        bool first = true;

        foreach (var sortingParam in sortingParams)
        {
            var orderByMethod = sortingParam.Sort == SortDirection.DESC ?
                (first ? OrderByMethods.OrderByDescending : OrderByMethods.ThenByDescending) :
                (first ? OrderByMethods.OrderBy : OrderByMethods.ThenBy);

            first = false;

            // there are 2 case
            // property of this entity - no . in the name
            // property of navigation entity - have . in the name, can be multiple

            var fields = sortingParam.Field.Split('.');
            var property = entityType.GetProperty(fields[0]);

            if (property == null)
                throw new ArgumentException($"Property {sortingParam.Field} not found in type {entityType.Name}");

            Expression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            Expression orderByExpression = Expression.Lambda(propertyAccess, parameter);

            if (fields.Length > 1)
            {
                var navigationProperties = fields.Skip(1);

                foreach (var navigationProperty in navigationProperties)
                {
                    property = property.PropertyType.GetProperty(navigationProperty);

                    if (property == null)
                        throw new ArgumentException($"Property {navigationProperty} not found in type {entityType.Name}");

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    orderByExpression = Expression.Lambda(propertyAccess, parameter);
                }
            }

            expression = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                [entityType, property.PropertyType],
                expression,
                Expression.Quote(orderByExpression));
        }

        return query.Provider.CreateQuery<TEntity>(expression);
    }
}
