using MediatR;
using VerticalSlice.Api.Common;

namespace VerticalSlice.Api.Features.Addresses.DeleteAddress;

public record DeleteAddressCommand(Guid Id) : IRequest<Result>;
