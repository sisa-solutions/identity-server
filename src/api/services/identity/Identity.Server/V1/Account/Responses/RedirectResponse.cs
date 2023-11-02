namespace Sisa.Identity.Server.V1.Account.Responses;

/// <summary>
/// Represents a command to Redirect.
/// </summary>
public record class RedirectResponse
{
    /// <summary>
    /// Gets or sets the URL to redirect to.
    /// </summary>
    public string RedirectUrl { get; set; } = string.Empty;
}
