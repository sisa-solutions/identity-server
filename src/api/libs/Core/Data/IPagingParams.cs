namespace Sisa.Abstractions;

public interface IPagingParams
{
    int PageIndex { get; }
    int PageSize { get; }
}
