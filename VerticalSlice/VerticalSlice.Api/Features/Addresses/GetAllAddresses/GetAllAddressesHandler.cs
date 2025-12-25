using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.GetAllAddresses;

public class GetAllAddressesHandler : IRequestHandler<GetAllAddressesQuery, Result<List<AddressResponse>>>
{
    private readonly AddressDbContext _context;

    public GetAllAddressesHandler(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<AddressResponse>>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _context.Addresses
            .Select(a => new AddressResponse(
                a.Id,
                a.Street,
                a.City,
                a.State,
                a.ZipCode,
                a.Country,
                a.CreatedAt,
                a.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Result<List<AddressResponse>>.Success(addresses);
    }
}
