using System.Linq.Expressions;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

namespace Sisa.Identity.Domain.Specifications;

public sealed class RoleSpecifications<TResult>(Expression<Func<Role, TResult>> selector)
    : Specification<Role, TResult>(selector) where TResult : class
{
    public RoleSpecifications(
        Guid id,
        Expression<Func<Role, TResult>> selector) : this(selector)
    {
        Builder
            .Include(x => x.RoleClaims)
            .Where(x => x.Id == id);
    }

    public RoleSpecifications(
        IFilteringParams filteringParams
        , Expression<Func<Role, TResult>> selector) : this(selector)
    {
        Builder
            .Include(x => x.RoleClaims);

         Builder
            .Filter(filteringParams);
    }

    public RoleSpecifications(
        IFilteringParams filteringParams
        , IEnumerable<ISortingParams> sortingParams
        , IPagingParams pagingParams
        , Expression<Func<Role, TResult>> selector) : this(selector)
    {
        Builder
            .Filter(filteringParams);

        if (sortingParams.Any())
        {
            Builder.Sort(sortingParams);
        }
        else
        {
            Builder.OrderBy(x => x.NormalizedName!);
        }

        Builder
            .Sort(sortingParams)
            .Paginate(pagingParams);
    }
}
