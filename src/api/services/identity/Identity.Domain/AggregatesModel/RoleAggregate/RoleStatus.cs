namespace Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public enum RoleStatus : byte
{
    UNSPECIFIED,
    NEW,
    ACTIVE,
    DEACTIVATE,
}
