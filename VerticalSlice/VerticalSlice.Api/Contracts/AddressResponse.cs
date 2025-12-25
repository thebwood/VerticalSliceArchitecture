namespace VerticalSlice.Api.Contracts;

public record AddressResponse(
    Guid Id,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
