using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;

namespace VerticalSlice.Api.Features.Addresses.UpdateAddress;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, Result<AddressResponse>>
{
    private readonly AddressDbContext _context;

    public UpdateAddressHandler(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AddressResponse>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (address is null)
        {
            return Result<AddressResponse>.Failure(
                Error.NotFound("Address.NotFound", $"Address with ID {request.Id} was not found"));
        }

        address.Street = request.Street;
        address.City = request.City;
        address.State = request.State;
        address.ZipCode = request.ZipCode;
        address.Country = request.Country;
        address.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new AddressResponse(
            address.Id,
            address.Street,
            address.City,
            address.State,
            address.ZipCode,
            address.Country,
            address.CreatedAt,
            address.UpdatedAt);

        return Result<AddressResponse>.Success(response);
    }
}
