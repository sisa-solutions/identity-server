namespace Sisa.Identity.Infrastructure.Abstractions;

public interface IResourceOwnerPasswordGrantTypeHandler
{
    Task<GrantTypeHandleResult> Handle(AuthorizeRequest request);
}
