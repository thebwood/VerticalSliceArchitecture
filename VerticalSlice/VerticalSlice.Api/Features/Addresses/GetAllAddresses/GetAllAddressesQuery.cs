using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.GetAllAddresses;

public record GetAllAddressesQuery : IRequest<Result<List<AddressResponse>>>;
