using FluentValidation;

namespace VerticalSlice.Api.Features.Addresses.CreateAddress;

public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(200).WithMessage("Street must not exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters")
            .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Zip code must be in format 12345 or 12345-6789")
            .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters");
    }
}
