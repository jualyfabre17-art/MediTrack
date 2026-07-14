using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Patient;
using MediTrack.Application.Interfaces;
using MediTrack.Application.Validators;
using MediTrack.Domain.Entities;
using MediTrack.Domain.Interfaces;
using MediTrack.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.Application.Services;

public class PatientService : BaseService, IPatientService
{
    public PatientService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult<IEnumerable<PatientResponseDto>>> GetAllAsync()
    {
        try
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            var response = patients.Select(MapToResponseDto);
            return ServiceResult<IEnumerable<PatientResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<PatientResponseDto>>($"Error retrieving patients: {ex.Message}");
        }
    }

    public async Task<ServiceResult<PatientResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return ServiceResult<PatientResponseDto>.NotFound("Patient not found");

            var response = MapToResponseDto(patient);
            return ServiceResult<PatientResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<PatientResponseDto>($"Error retrieving patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<PatientResponseDto>> GetByMedicalRecordNumberAsync(string medicalRecordNumber)
    {
        try
        {
            var patient = await _unitOfWork.Patients
                .FindAsync(p => p.MedicalRecordNumber.ToString() == medicalRecordNumber);

            var patientFound = patient.FirstOrDefault();
            if (patientFound == null)
                return ServiceResult<PatientResponseDto>.NotFound("Patient not found");

            var response = MapToResponseDto(patientFound);
            return ServiceResult<PatientResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<PatientResponseDto>($"Error retrieving patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<PatientResponseDto>> CreateAsync(PatientCreateDto createDto)
    {
        try
        {
            // Validar DTO
            var validator = new PatientValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<PatientResponseDto>.ValidationError(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            // Mapear a entidad
            var patient = MapToEntity(createDto);
            patient.MedicalRecordNumber = MedicalRecordNumber.Generate();

            // Agregar y guardar
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CompleteAsync();

            var response = MapToResponseDto(patient);
            return ServiceResult<PatientResponseDto>.Success(response, "Patient created successfully");
        }
        catch (Exception ex)
        {
            return HandleError<PatientResponseDto>($"Error creating patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<PatientResponseDto>> UpdateAsync(PatientUpdateDto updateDto)
    {
        try
        {
            // Obtener paciente existente
            var existingPatient = await _unitOfWork.Patients.GetByIdAsync(updateDto.Id);
            if (existingPatient == null)
                return ServiceResult<PatientResponseDto>.NotFound("Patient not found");

            // Validar DTO (usar un validador para update)
            var validator = new PatientUpdateValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<PatientResponseDto>.ValidationError(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            // Actualizar entidad
            UpdateEntity(existingPatient, updateDto);

            await _unitOfWork.Patients.UpdateAsync(existingPatient);
            await _unitOfWork.CompleteAsync();

            var response = MapToResponseDto(existingPatient);
            return ServiceResult<PatientResponseDto>.Success(response, "Patient updated successfully");
        }
        catch (Exception ex)
        {
            return HandleError<PatientResponseDto>($"Error updating patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return ServiceResult<bool>.NotFound("Patient not found");

            // Soft delete - marcar como inactivo
            patient.IsActive = false;
            patient.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Success(true, "Patient deleted successfully");
        }
        catch (Exception ex)
        {
            return HandleError<bool>($"Error deleting patient: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<PatientResponseDto>>> SearchAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var patients = await _unitOfWork.Patients
                .FindAsync(p =>
                    p.FirstName.Contains(searchTerm) ||
                    p.LastName.Contains(searchTerm) ||
                    p.IdentificationNumber.Contains(searchTerm) ||
                    p.Email.Contains(searchTerm) ||
                    (p.FirstName + " " + p.LastName).Contains(searchTerm));

            var response = patients.Select(MapToResponseDto);
            return ServiceResult<IEnumerable<PatientResponseDto>>.Success(response);
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<PatientResponseDto>>($"Error searching patients: {ex.Message}");
        }
    }

    // Mapeadores
    private static Patient MapToEntity(PatientCreateDto dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            IdentificationNumber = dto.IdentificationNumber,
            BloodType = (Domain.Enums.BloodType)dto.BloodType,
            Height = dto.Height,
            Weight = dto.Weight,
            Allergies = dto.Allergies,
            MedicalHistory = dto.MedicalHistory,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            Address = new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country
            },
            MedicalRecordNumber = new MedicalRecordNumber()
        };
    }

    private static void UpdateEntity(Patient entity, PatientUpdateDto dto)
    {
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.Gender = dto.Gender;
        entity.IdentificationNumber = dto.IdentificationNumber;
        entity.BloodType = (Domain.Enums.BloodType)dto.BloodType;
        entity.Height = dto.Height;
        entity.Weight = dto.Weight;
        entity.Allergies = dto.Allergies;
        entity.MedicalHistory = dto.MedicalHistory;
        entity.EmergencyContactName = dto.EmergencyContactName;
        entity.EmergencyContactPhone = dto.EmergencyContactPhone;
        entity.Address.Street = dto.Address.Street;
        entity.Address.City = dto.Address.City;
        entity.Address.State = dto.Address.State;
        entity.Address.PostalCode = dto.Address.PostalCode;
        entity.Address.Country = dto.Address.Country;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    private static PatientResponseDto MapToResponseDto(Patient patient)
    {
        return new PatientResponseDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            FullName = patient.FullName,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            DateOfBirth = patient.DateOfBirth,
            Age = patient.GetAge(),
            Gender = patient.Gender,
            IdentificationNumber = patient.IdentificationNumber,
            MedicalRecordNumber = patient.MedicalRecordNumber.ToString(),
            BloodType = patient.BloodType.ToString(),
            Height = patient.Height,
            Weight = patient.Weight,
            Allergies = patient.Allergies,
            MedicalHistory = patient.MedicalHistory,
            EmergencyContactName = patient.EmergencyContactName,
            EmergencyContactPhone = patient.EmergencyContactPhone,
            Address = new AddressDto
            {
                Street = patient.Address.Street,
                City = patient.Address.City,
                State = patient.Address.State,
                PostalCode = patient.Address.PostalCode,
                Country = patient.Address.Country
            },
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt
        };
    }
}