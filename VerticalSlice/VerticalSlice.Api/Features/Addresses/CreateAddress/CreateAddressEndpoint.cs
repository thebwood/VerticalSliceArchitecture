using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;
using VerticalSlice.Api.Extensions;

namespace VerticalSlice.Api.Features.Addresses.CreateAddress;

public class CreateAddressEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/addresses", async (CreateAddressCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.ToCreatedResult("GetAddressById", r => new { id = r.Id });
        })
        .WithName("CreateAddress")
        .WithTags("Addresses")
        .Produces<AddressResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
