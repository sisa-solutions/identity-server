namespace Sisa.Abstractions;

public interface ITenant
{
    Guid? TenantId { get; }

    void SetTenant(Guid tenantId);
}
