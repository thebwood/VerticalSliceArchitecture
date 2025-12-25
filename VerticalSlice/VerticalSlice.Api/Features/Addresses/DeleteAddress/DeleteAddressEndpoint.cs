using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Extensions;

namespace VerticalSlice.Api.Features.Addresses.DeleteAddress;

public class DeleteAddressEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/addresses/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteAddressCommand(id), ct);
            return result.ToHttpResult();
        })
        .WithName("DeleteAddress")
        .WithTags("Addresses")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
