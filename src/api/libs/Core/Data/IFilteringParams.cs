using Sisa.Enums;

namespace Sisa.Abstractions;

public interface IFilterRule
{
    string Field { get; }
    string Value { get; }

    Operator Operator { get; } // EQ, NEQ, GT, GTE, LT, LTE, IN, NIN

    bool Not { get; }

    Combinator Combinator { get; } // AND or OR

    IEnumerable<IFilterRule> Rules { get; }
}

public interface IFilteringParams
{
    bool Not { get; }

    Combinator Combinator { get; } // AND or OR

    IEnumerable<IFilterRule> Rules { get; }
}
