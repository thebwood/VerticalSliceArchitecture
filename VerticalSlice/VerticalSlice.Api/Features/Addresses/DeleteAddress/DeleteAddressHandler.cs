using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Common;

namespace VerticalSlice.Api.Features.Addresses.DeleteAddress;

public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, Result>
{
    private readonly AddressDbContext _context;

    public DeleteAddressHandler(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (address is null)
        {
            return Result.Failure(
                Error.NotFound("Address.NotFound", $"Address with ID {request.Id} was not found"));
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
