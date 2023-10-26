using System.Text.Json.Serialization;

namespace Sisa.Identity.Api.V1.Account.Responses;

/// <summary>
/// Represents a response to login.
/// </summary>
public record LoginResponse
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    [JsonIgnore]
    public string RedirectUrl { get; set; } = string.Empty;
}
