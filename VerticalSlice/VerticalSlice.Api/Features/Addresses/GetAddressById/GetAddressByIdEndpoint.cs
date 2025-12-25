using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;
using VerticalSlice.Api.Extensions;

namespace VerticalSlice.Api.Features.Addresses.GetAddressById;

public class GetAddressByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/addresses/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAddressByIdQuery(id), ct);
            return result.ToHttpResult();
        })
        .WithName("GetAddressById")
        .WithTags("Addresses")
        .Produces<AddressResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
