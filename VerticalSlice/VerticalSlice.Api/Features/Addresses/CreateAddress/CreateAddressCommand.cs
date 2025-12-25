using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.CreateAddress;

public record CreateAddressCommand(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country) : IRequest<Result<AddressResponse>>;
