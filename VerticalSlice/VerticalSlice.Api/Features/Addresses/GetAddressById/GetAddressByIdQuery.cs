using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.GetAddressById;

public record GetAddressByIdQuery(Guid Id) : IRequest<Result<AddressResponse>>;
