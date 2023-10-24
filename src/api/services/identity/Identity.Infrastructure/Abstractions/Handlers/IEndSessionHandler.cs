using Sisa.Abstractions;

namespace Sisa.Identity.Infrastructure.Abstractions;

public interface IEndSessionHandler
{
    Task Handle();
}
