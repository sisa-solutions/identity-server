using Sisa.Abstractions;
using Sisa.Enums;

namespace Sisa.Data.Params;

public partial class FilterRule : IFilterRule
{
    Operator IFilterRule.Operator => (Operator)Operator;

    Combinator IFilterRule.Combinator => (Combinator)Combinator;

    IEnumerable<IFilterRule> IFilterRule.Rules => Rules;
}

public partial class FilteringParams : IFilteringParams
{
    Combinator IFilteringParams.Combinator => (Combinator)Combinator;

    IEnumerable<IFilterRule> IFilteringParams.Rules => Rules;
}
