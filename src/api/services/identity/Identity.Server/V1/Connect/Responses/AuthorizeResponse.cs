namespace Sisa.Identity.Server.V1.Connect.Responses;

/// <summary>
/// Represents the response for the Device Verification.
/// </summary>
public record class AuthorizeResponse
{
    /// <summary>
    /// Client
    /// </summary>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// The scopes.
    /// </summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// The user code.
    /// </summary>
    public string UserCode { get; set; } = string.Empty;
}
