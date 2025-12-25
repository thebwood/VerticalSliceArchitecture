using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;
using VerticalSlice.Api.Extensions;

namespace VerticalSlice.Api.Features.Addresses.UpdateAddress;

public class UpdateAddressEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/addresses/{id:guid}", async (Guid id, UpdateAddressRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateAddressCommand(
                id,
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.Country);

            var result = await mediator.Send(command, ct);
            return result.ToHttpResult();
        })
        .WithName("UpdateAddress")
        .WithTags("Addresses")
        .Produces<AddressResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
