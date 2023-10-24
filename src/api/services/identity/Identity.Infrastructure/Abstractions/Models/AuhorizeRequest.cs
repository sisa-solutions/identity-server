namespace Sisa.Identity.Infrastructure.Abstractions;

public class AuthorizeRequest
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; private set; }
    public string? UserName { get; private set; }
    public string? Password { get; private set; }

    public AuthorizeRequest(string? clientId)
    {
        ClientId = clientId;
    }

    public void SetClientSecret(string? clientSecret) => ClientSecret = clientSecret;

    public void SetUserCredential(string? userName, string? password)
    {
        UserName = userName;
        Password = password;
    }
}
