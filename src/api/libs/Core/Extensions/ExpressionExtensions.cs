namespace System.Linq.Expressions;

public static partial class ExpressionExtensions
{
    public static Expression<Func<TEntity, bool>> And<TEntity>(
        this Expression<Func<TEntity, bool>> left,
        Expression<Func<TEntity, bool>> right)
    {
        var invokedExpr = Expression.Invoke(right, left.Parameters.Cast<Expression>());

        return Expression.Lambda<Func<TEntity, bool>>
            (Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
    }

    public static Expression<Func<TEntity, bool>> Or<TEntity>(
        this Expression<Func<TEntity, bool>> left,
        Expression<Func<TEntity, bool>> right)
    {
        var invokedExpr = Expression.Invoke(right, left.Parameters.Cast<Expression>());

        return Expression.Lambda<Func<TEntity, bool>>
            (Expression.OrElse(left.Body, invokedExpr), left.Parameters);
    }
}
