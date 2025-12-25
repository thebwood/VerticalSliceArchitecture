using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;
using VerticalSlice.Api.Extensions;

namespace VerticalSlice.Api.Features.Addresses.GetAllAddresses;

public class GetAllAddressesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/addresses", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllAddressesQuery(), ct);
            return result.ToHttpResult();
        })
        .WithName("GetAllAddresses")
        .WithTags("Addresses")
        .Produces<List<AddressResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
