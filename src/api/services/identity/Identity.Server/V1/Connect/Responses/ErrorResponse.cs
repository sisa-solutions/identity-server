namespace Sisa.Identity.Server.V1.Connect.Responses;

/// <summary>
/// Represents the response for the error.
/// </summary>
public record class ErrorResponse
{
    /// <summary>
    /// The error code.
    /// </summary>
    public string Error { get; init; } = string.Empty;

    /// <summary>
    /// The error description.
    /// </summary>
    public string ErrorDescription { get; init; } = string.Empty;
}
