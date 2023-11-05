using Sisa.Abstractions;
using Sisa.Enums;

namespace Sisa.Data.Params;

public partial class SortingParams : ISortingParams
{
    SortDirection ISortingParams.Sort => (SortDirection)Sort;
}
