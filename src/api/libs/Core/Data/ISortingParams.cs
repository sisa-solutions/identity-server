using Sisa.Enums;

namespace Sisa.Abstractions;

public interface ISortingParams
{
    string Field { get; }
    SortDirection Sort { get; } // ASC or DESC
}
