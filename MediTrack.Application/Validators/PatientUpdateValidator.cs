using FluentValidation;
using MediTrack.Application.Dtos.Patient;
using MediTrack.Domain.Interfaces;

namespace MediTrack.Application.Validators;

public class PatientUpdateValidator : AbstractValidator<PatientUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientUpdateValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Invalid patient ID");

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(p => p.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

        RuleFor(p => p.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(p => p.IdentificationNumber)
             .NotEmpty().WithMessage("Identification number is required.");

        RuleFor(p => p)
            .MustAsync(async (dto, cancellation) =>
            {
                var existingPatients = await _unitOfWork.Patients.FindAsync(p =>
                    p.IdentificationNumber == dto.IdentificationNumber && p.Id != dto.Id);

                return !existingPatients.Any();
            })
            .WithName(nameof(PatientUpdateDto.IdentificationNumber))
            .WithMessage("Identification number already exists");
    }



    private async Task<bool> BeUniqueIdentificationNumber(string identificationNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identificationNumber))
            return true;

        var exists = await _unitOfWork.Patients
            .ExistsAsync(p => p.IdentificationNumber == identificationNumber && p.Id != 0);
        return !exists;
    }

    private bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 18;
    }
}
