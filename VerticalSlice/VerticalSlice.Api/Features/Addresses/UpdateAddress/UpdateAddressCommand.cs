using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.UpdateAddress;

public record UpdateAddressRequest(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country);

public record UpdateAddressCommand(
    Guid Id,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country) : IRequest<Result<AddressResponse>>;
