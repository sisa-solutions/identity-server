using System.Collections;
using System.Linq.Expressions;

using Sisa.Abstractions;
using Sisa.Enums;

namespace Sisa.Data;

public static partial class FilteringExtensions
{
    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, IFilteringParams filteringParams)
    {
        if (filteringParams == null || filteringParams.Rules == null || !filteringParams.Rules.Any())
        {
            return query;
        }

        var filterExpression = ParseFiltering<TEntity>(filteringParams);

        return query.Where(filterExpression);
    }

    private static Expression<Func<TEntity, bool>> ParseFiltering<TEntity>(IFilteringParams filteringParams)
    {
        ParameterExpression param = Expression.Parameter(typeof(TEntity), "x");
        Expression? combinedExpression = null;

        foreach (var filterRule in filteringParams.Rules)
        {
            Expression ruleExpression = BuildFilterExpression(param, filterRule);

            if (combinedExpression == null)
            {
                combinedExpression = ruleExpression;
            }
            else
            {
                if (filteringParams.Combinator == Combinator.AND)
                {
                    combinedExpression = Expression.AndAlso(combinedExpression, ruleExpression);
                }
                else if (filteringParams.Combinator == Combinator.OR)
                {
                    combinedExpression = Expression.OrElse(combinedExpression, ruleExpression);
                }
            }
        }

        if (combinedExpression == null)
        {
            throw new InvalidOperationException("No filtering parameters specified.");
        }

        if (filteringParams.Not)
        {
            combinedExpression = Expression.Not(combinedExpression);
        }

        return Expression.Lambda<Func<TEntity, bool>>(combinedExpression, param);
    }

    private static Expression BuildFilterExpression(ParameterExpression param, IFilterRule filterRule)
    {
        var property = filterRule.Field.Contains('.')
            ? filterRule.Field
                .Split('.')
                .Aggregate((Expression)param, Expression.PropertyOrField)
            : Expression.PropertyOrField(param, filterRule.Field);

        Expression value;

        if (property.Type != typeof(string))
        {
            if (property.Type == typeof(Guid))
            {
                if (!Guid.TryParse(filterRule.Value, out Guid guidValue))
                {
                    throw new ArgumentException($"Invalid Guid value: {filterRule.Value}");
                }

                value = Expression.Constant(guidValue);
            }
            else if(property.Type.BaseType == typeof(Enum))
            {
                if (!Enum.TryParse(property.Type, filterRule.Value, out var enumValue))
                {
                    throw new ArgumentException($"Invalid Enum value: {filterRule.Value}");
                }

                value = Expression.Constant(enumValue);
            }
            else
            {
                // TODO: Correct Type conversion
                value = Expression.Constant(filterRule.Value);
            }
        }
        else
        {
            value = Expression.Constant(filterRule.Value);
        }

        var operation = BuildOperationExpression(property, value, filterRule.Operator);

        if (filterRule.Not)
        {
            operation = Expression.Not(operation);
        }

        return operation;
    }

    private static Expression BuildOperationExpression(Expression property, Expression value, Operator op)
    {
        return op switch
        {
            // 1 equals
            Operator.EQUAL => Expression.Equal(property, value),

            // 2 not equals
            Operator.NOT_EQUAL => Expression.NotEqual(property, value),

            // 3 greater than
            Operator.GREATER_THAN => Expression.GreaterThan(property, value),

            // 4 greater than or equal
            Operator.GREATER_THAN_OR_EQUAL => Expression.GreaterThanOrEqual(property, value),

            // 5 less than
            Operator.LESS_THAN => Expression.LessThan(property, value),

            // 6 less than or equal
            Operator.LESS_THAN_OR_EQUAL => Expression.LessThanOrEqual(property, value),

            // 7 contains
            Operator.CONTAINS => property.Type switch
            {
                var type when type.Name != nameof(String) && type.GetInterface(nameof(IEnumerable)) != null
                    => Expression.Call(
                        typeof(Enumerable).GetMethods()
                            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(string))
                        , property
                        , value
                    ),
                _ => Expression.Call(property, property.Type.GetMethod(nameof(Enumerable.Contains), [property.Type])!, value),
            },

            // 8 not contains
            Operator.NOT_CONTAINS => property.Type switch
            {
                var type when type.Name != nameof(String) && type.GetInterface(nameof(IEnumerable)) != null
                    => Expression.Not(Expression.Call(
                        typeof(Enumerable).GetMethods()
                            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(string))
                        , property
                        , value)
                    ),
                _ => Expression.Not(Expression.Call(property, property.Type.GetMethod(nameof(Enumerable.Contains), [property.Type])!, value)),
            },

            // 9 starts with
            Operator.STARTS_WITH => Expression.Call(property, typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!, value),

            // 10 not starts with
            Operator.NOT_STARTS_WITH => Expression.Not(Expression.Call(property, typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!, value)),

            // 11 ends with
            Operator.ENDS_WITH => Expression.Call(property, typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)])!, value),

            // 12 not ends with
            Operator.NOT_ENDS_WITH => Expression.Not(Expression.Call(property, typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)])!, value)),

            // 13 in
            Operator.IN => Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [typeof(string)], value, property),

            // 14 not in
            Operator.NOT_IN => Expression.Not(Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [typeof(string)], value, property)),

            // 15 between
            Operator.BET_WEEN => Expression.AndAlso(
                Expression.GreaterThanOrEqual(property, Expression.Property(value, "Start")),
                Expression.LessThanOrEqual(property, Expression.Property(value, "End"))),

            // 16 not between
            Operator.NOT_BETWEEN => Expression.OrElse(
                Expression.LessThan(property, Expression.Property(value, "Start")),
                Expression.GreaterThan(property, Expression.Property(value, "End"))),

            // 17 is null
            Operator.IS_NULL => Expression.Equal(property, Expression.Constant(null)),

            // 18 is not null
            Operator.IS_NOT_NULL => Expression.NotEqual(property, Expression.Constant(null)),

            // Handle other operators as needed
            _ => throw new ArgumentException($"Unsupported operator: {op}"),
        };
    }
}
