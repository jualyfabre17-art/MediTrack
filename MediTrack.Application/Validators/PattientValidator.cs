using FluentValidation;
using MediTrack.Application.Dtos.Patient;
using MediTrack.Domain.Interfaces;

namespace MediTrack.Application.Validators;

public class PatientValidator : AbstractValidator<PatientCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

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
            .NotEmpty().WithMessage("Identification number is required")
            .MaximumLength(20).WithMessage("Identification number must not exceed 20 characters")
            .MustAsync(BeUniqueIdentificationNumber)
            .WithMessage("Identification number already exists");

        RuleFor(p => p.DateOfBirth)
            .Must(BeValidAge)
            .WithMessage("Patient must be at least 18 years old");

        RuleFor(p => p.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0")
            .LessThan(300).WithMessage("Height must be less than 300 cm");

        RuleFor(p => p.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThan(500).WithMessage("Weight must be less than 500 kg");

        RuleFor(p => p.Address)
            .NotNull().WithMessage("Address is required");

        RuleFor(p => p.Address.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(200);
    }

    private async Task<bool> BeUniqueIdentificationNumber(string identificationNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identificationNumber))
            return true;

        var exists = await _unitOfWork.Patients
            .ExistsAsync(p => p.IdentificationNumber == identificationNumber);
        return !exists;
    }

    private bool BeValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 18;
    }
}
