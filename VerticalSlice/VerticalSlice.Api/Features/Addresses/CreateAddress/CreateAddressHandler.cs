using MediatR;
using VerticalSlice.Api.Common;
using VerticalSlice.Api.Contracts;
using VerticalSlice.Api.Domain;

namespace VerticalSlice.Api.Features.Addresses.CreateAddress;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, Result<AddressResponse>>
{
    private readonly AddressDbContext _context;

    public CreateAddressHandler(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            Id = Guid.NewGuid(),
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Country = request.Country,
            CreatedAt = DateTime.UtcNow
        };

        _context.Addresses.Add(address);
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
