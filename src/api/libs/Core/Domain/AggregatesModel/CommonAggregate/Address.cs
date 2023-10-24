using Sisa.Abstractions;
using Sisa.Enums;

namespace Sisa.Domain.AggregatesModel.CommonAggregate;

public class Address : ValueObject
{
    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the State/Province.
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// Gets or sets the district.
    /// </summary>
    public string District { get; set; }

    /// <summary>
    /// Gets or sets the ward.
    /// </summary>
    public string Ward { get; set; }

    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the house number.
    /// </summary>
    // public string HouseNumber { get; set; }

    /// <summary>
    /// Gets or sets the street address line 1.
    /// Here, you must include primary information, including the street address.
    /// </summary>
    public string AddressLine1 { get; set; }

    /// <summary>
    /// Gets or sets the street address line 2.
    /// Here, you can include additional information, such as the apartment number.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Gets or sets the street address line 3.
    /// Here, you can include additional information, such as the apartment number.
    /// </summary>
    public string? AddressLine3 { get; set; }

    /// <summary>
    /// Gets or sets the type of address.
    /// </summary>
    public AddressType AddressType { get; set; }

    // Empty constructor in this case is required by EF Core,
    // because has a complex type as a parameter in the default constructor.
    public Address()
    {
        Country = string.Empty;
        Province = string.Empty;
        District = string.Empty;
        Ward = string.Empty;
        PostalCode = string.Empty;
        AddressType = AddressType.UNSPECIFIED;

        AddressLine1 = string.Empty;
        AddressLine2 = string.Empty;
        AddressLine3 = string.Empty;
    }

    public Address(string country, string province, string district, string ward, string postalCode, string addressLine1, string? addressLine2, string? addressLine3, AddressType type)
    {
        Country = country;
        Province = province;
        District = district;
        Ward = ward;
        PostalCode = postalCode;

        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        AddressLine3 = addressLine3;
        AddressType = type;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Country;
        yield return Province;
        yield return District;
        yield return Ward;
        yield return PostalCode;
        yield return AddressType;

        yield return AddressLine1;
        yield return AddressLine2 ?? string.Empty;
        yield return AddressLine3 ?? string.Empty;
    }
}
