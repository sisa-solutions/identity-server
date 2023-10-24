namespace Sisa.Abstractions;

public interface IPaginatedList
{
    long ItemCount { get; }
    int PageIndex { get; }
    int PageSize { get; }
    int PageCount { get; }
}

public interface IPaginatedList<out TSource> : IPaginatedList, IReadOnlyList<TSource>
{
    string ToJsonInfo();
}
