namespace Sisa.Identity.Infrastructure.Abstractions;

public class GrantTypeHandleResult
{
    public bool Succeeded { get; private set; }
    public DomainException? Exception { get; set; }
    public string Sub { get; private set; }

    public Dictionary<string, object> Extensions { get; private set; } = new Dictionary<string, object>();

    public GrantTypeHandleResult(string sub)
    {
        Succeeded = true;
        Sub = sub;
    }

    public GrantTypeHandleResult(DomainException exception) : this(string.Empty)
    {
        Succeeded = false;
        Exception = exception;
    }

    public GrantTypeHandleResult(DomainException exception, Dictionary<string, object> extensions)
        : this(exception)
    {
        Succeeded = false;
        Extensions = extensions;
    }
}
