namespace Sisa.Identity.Api.V1.Connect.Responses;

/// <summary>
/// Represents the response for the Device Verification.
/// </summary>
public record class VerifyResponse
{
    /// <summary>
    /// The error code.
    /// </summary>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// The error description.
    /// </summary>
    public string ErrorDescription { get; set; } = string.Empty;

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
