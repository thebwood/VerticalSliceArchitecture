using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.GetAddressById;

public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, Result<AddressResponse>>
{
    private readonly AddressDbContext _context;

    public GetAddressByIdHandler(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AddressResponse>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .Where(a => a.Id == request.Id)
            .Select(a => new AddressResponse(
                a.Id,
                a.Street,
                a.City,
                a.State,
                a.ZipCode,
                a.Country,
                a.CreatedAt,
                a.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (address is null)
        {
            return Result<AddressResponse>.Failure(
                Error.NotFound("Address.NotFound", $"Address with ID {request.Id} was not found"));
        }

        return Result<AddressResponse>.Success(address);
    }
}
