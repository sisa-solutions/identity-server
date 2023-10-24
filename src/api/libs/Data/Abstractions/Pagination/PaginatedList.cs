using Microsoft.EntityFrameworkCore;

using Sisa.Abstractions;

namespace Sisa.Data;

public class PaginatedList<TSource> : List<TSource>, IPaginatedList<TSource>
{
    public long ItemCount { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public int PageCount { get; private set; }

    // public bool HasPreviousPage => Page > 1;

    // public bool HasNextPage => Page < PageCount;

    public PaginatedList(IEnumerable<TSource> items, long itemCount, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;

        ItemCount = itemCount;

        PageCount = (int)Math.Ceiling(ItemCount / (double)pageSize);

        AddRange(items);
    }

    public static IPaginatedList<TSource> Create(IEnumerable<TSource> source, int pageIndex, int pageSize)
    {
        var itemCount = source.LongCount();

        if (pageSize < 1)
        {
            return new PaginatedList<TSource>(source, itemCount, pageIndex, 0);
        }

        var items = source.Skip(pageIndex * pageSize).Take(pageSize);

        return new PaginatedList<TSource>(items, itemCount, pageIndex, pageSize);
    }

    public static IPaginatedList<TSource> Create(IEnumerable<TSource> source, IPagingParams pagingParams)
        => Create(source, pagingParams.PageIndex, pagingParams.PageSize);

    public static async ValueTask<IPaginatedList<TSource>> CreateAsync(IQueryable<TSource> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var itemCount = await source.LongCountAsync(cancellationToken);

        if (pageSize < 1)
        {
            return new PaginatedList<TSource>(source, itemCount, pageIndex, 0);
        }

        var items = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<TSource>(items, itemCount, pageIndex, pageSize);
    }

    public static async ValueTask<IPaginatedList<TSource>> CreateAsync(IQueryable<TSource> source, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        => await CreateAsync(source, pagingParams.PageIndex, pagingParams.PageSize, cancellationToken);

    private IPaginatedList GetPaginatedList() => this;

    public string ToJsonInfo() => "{}"; //JsonSerializer.Serialize(GetPaginatedList(), JsonConstants.JSON_SERIALIZER_OPTIONS);
}
