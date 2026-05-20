using ApplicationCore.Dto;
using ApplicationCore.Interfaces;
using ApplicationCore.Validators.Shared;
using FluentValidation;

namespace ApplicationCore.Validators;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    private readonly ICompanyRepository _companyRepository;

    public CreatePersonDtoValidator(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię nie może przekraczać 100 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imię zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie może przekraczać 200 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy format adresu email.")
            .MaximumLength(200).WithMessage("Email nie może przekraczać 200 znaków.");

        RuleFor(x => x.Phone)
            .Matches(@"^(\+?[\d\s\-().]{7,20})$")
                .WithMessage("Nieprawidłowy format numeru telefonu.")
            .When(x => x.Phone is not null);

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
                .WithMessage("Osoba musi mieć co najmniej 18 lat.")
            .GreaterThan(DateTime.Today.AddYears(-120))
                .WithMessage("Nieprawidłowa data urodzenia.")
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.Gender)
            .IsInEnum()
                .WithMessage("Nieprawidłowa wartość płci.");

        RuleFor(x => x.EmployerId)
            .MustAsync(EmployerExistsAsync)
                .WithMessage("Wskazana firma nie istnieje.")
            .When(x => x.EmployerId.HasValue);

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Adres jest wymagany.")
            .SetValidator(new AddressDtoValidator()!);

        RuleFor(x => x.Pesel)
            .Must(BeValidPesel)
            .WithMessage("Nieprawidłowy numer PESEL.")
            .When(x => x.Pesel is not null);
    }

    private bool BeValidPesel(string? pesel)
    {
        if (string.IsNullOrWhiteSpace(pesel)) return true;
        try
        {
            _ = new Models.Pesel(pesel);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> EmployerExistsAsync(Guid? employerId, CancellationToken ct) =>
        await _companyRepository.FindByIdAsync(employerId ?? Guid.Empty) is not null;
}
