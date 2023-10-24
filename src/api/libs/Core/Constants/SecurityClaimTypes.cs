namespace Sisa.Constants;

public static partial class SecurityClaimTypes
{
    /**
        Standard OpenID Connect claims:
        sub {string} The subject (end-user) identifier. This member is always present in a claims set.
        [ name ] {string} The full name of the end-user, with optional language tag.
        [ given_name ] {string} The given or first name of the end-user, with optional language tag.
        [ family_name ] {string} The surname(s) or last name(s) of the end-user, with optional language tag.
        [ middle_name ] {string} The middle name of the end-user, with optional language tag.
        [ nickname ] {string} The casual name of the end-user, with optional language tag.
        [ preferred_username ] {string} The username by which the end-user wants to be referred to at the client application.
        [ profile ] {string} The URL of the profile page for the end-user, with optional language tag.
        [ picture ] {string} The URL of the profile picture for the end-user.
        [ website ] {string} The URL of the end-user's web page or blog.
        [ email ] {string} The end-user's preferred email address.
        [ email_verified ] {true|false} True if the end-user's email address has been verified, else false.
        [ gender ] {"male"|"female"|?} The end-user's gender.
        [ birthdate ] {string} The end-user's birthday, represented in ISO 8601:2004 YYYY-MM-DD format. The year may be 0000, indicating that it is omitted. To represent only the year, YYYY format is allowed.
        [ zoneinfo ] {string} The end-user's time zone, e.g. Europe/Paris or America/Los_Angeles.
        [ locale ] {string} The end-user's locale, represented as a BCP47 language tag. This is typically an ISO 639-1 Alpha-2 language code in lowercase and an ISO 3166-1 Alpha-2 country code in uppercase, separated by a dash. For example, en-US or fr-CA.
        [ phone_number ] {string} The end-user's preferred telephone number, typically in E.164 format, for example +1 (425) 555-1212 or +56 (2) 687 2400.
        [ phone_number_verified ] {true|false} True if the end-user's telephone number has been verified, else false.
        [ address ] {object} A JSON object describing the end-user's preferred postal address with any of the following members:
            [ formatted ] {string} The full mailing address, with multiple lines if necessary. Newlines can be represented either as a \r\n or as a \n.
            [ street_address ] {string} The street address component, which may include house number, stree name, post office box, and other multi-line information. Newlines can be represented either as a \r\n or as a \n.
            [ locality ] {string} City or locality component.
            [ region ] {string} State, province, prefecture or region component.
            [ postal_code ] {string} Zip code or postal code component.
            [ country ] {string} Country name component.
        [ updated_at ] {number} Time the end-user's information was last updated, as number of seconds since the Unix epoch (1970-01-01T0:0:0Z) as measured in UTC until the date/time.

    */

    // User Id
    public const string Subject = "sub";

    // Full Name
    public const string Name = "name";

    // First Name
    public const string GivenName = "given_name";

    // Last Name
    public const string FamilyName = "family_name";

    // Middle Name
    public const string MiddleName = "middle_name";

    public const string NickName = "nickname";

    public const string FullName = "full_name";

    public const string DisplayName = "display_name";

    public const string UserName = "username";

    // Preferred User Name
    public const string PreferredUserName = "preferred_username";

    // URL of the End-User's profile page
    public const string Profile = "profile";

    // URL of the End-User's profile picture
    public const string Picture = "picture";

    // URL of the End-User's Web page or blog
    public const string WebSite = "website";

    public const string Email = "email";

    public const string EmailVerified = "email_verified";

    public const string Gender = "gender";
    public const string Status = "status";

    public const string BirthDate = "birthdate";

    /// <summary>String from the time zone database (http://www.twinsun.com/tz/tz-link.htm) representing the End-User's time zone. For example, Europe/Paris or America/Los_Angeles.</summary>
    public const string ZoneInfo = "zoneinfo";

    /// <summary>End-User's locale, represented as a BCP47 [RFC5646] language tag. This is typically an ISO 639-1 Alpha-2 [ISO639‑1] language code in lowercase and an ISO 3166-1 Alpha-2 [ISO3166‑1] country code in uppercase, separated by a dash. For example, en-US or fr-CA. As a compatibility note, some implementations have used an underscore as the separator rather than a dash, for example, en_US; Relying Parties MAY choose to accept this locale syntax as well.</summary>
    public const string Locale = "locale";

    /// <summary>End-User's preferred telephone number. E.164 (https://www.itu.int/rec/T-REC-E.164/e) is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or +56 (2) 687 2400. If the phone number contains an extension, it is RECOMMENDED that the extension be represented using the RFC 3966 [RFC3966] extension syntax, for example, +1 (604) 555-1234;ext=5678.</summary>
    public const string PhoneNumber = "phone_number";

    public const string PhoneNumberVerified = "phone_number_verified";

    public const string Address = "address";

    /// <summary>Time the End-User's information was last updated. Its value is a JSON number representing the number of seconds from 1970-01-01T0:0:0Z as measured in UTC until the date/time.</summary>
    public const string UpdatedAt = "updated_at";

    public const string SessionId = "sid";

    public const string ClientId = "client_id";

    public const string Scope = "scope";

    public const string Role = "role";
    public const string Permission = "permission";
    public const string HasPermission = "has_permission";
    public const string HasOneOfPermissions = "has_one_of_permissions";
    public const string HasAllOfPermissions = "has_all_of_permissions";
    public const string TenantId = "tenant_id";
}
